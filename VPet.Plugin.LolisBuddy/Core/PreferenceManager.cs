using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using LinePutScript.Converter;

namespace VPet.Plugin.LolisBuddy.Core
{
    public class PreferenceManager
    {
        private static readonly object _lock = new object();
        private static PreferenceManager _instance;

        public static PreferenceManager Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new PreferenceManager();
                }
            }
        }

        private List<PreferenceEntry> preferences = new();
        private readonly Dictionary<string, int> interactionHistory = new();

        private float LearningRate = LolisBuddy.AIsetting.LearningRate / 1000000f;
        private float BaseDecayRate = LolisBuddy.AIsetting.BaseDecayRate / 1000000f;
        private float OverindulgencePenalty = LolisBuddy.AIsetting.OverindulgencePenalty / 1000000f;
        private float CravingBoost = LolisBuddy.AIsetting.CravingBoost / 1000000f;
        private const int VarietyThreshold = 14;

        private void updateRates()
        {
            LearningRate = LolisBuddy.AIsetting.LearningRate / 1000000f;
            BaseDecayRate = LolisBuddy.AIsetting.BaseDecayRate / 1000000f;
            OverindulgencePenalty = LolisBuddy.AIsetting.OverindulgencePenalty / 1000000f;
            CravingBoost = LolisBuddy.AIsetting.CravingBoost / 1000000f;
        }

        public enum Personality { Stable, Curious, Addictive, Balanced }
        public Personality AIpersonality { get; set; } = Personality.Balanced;

        public string Category { get; private set; } = "";

        public string GetMood() => AnimationManager.Instance.animation.ModeType.ToString();

        public void Update(TimerManager timer)
        {
            updateRates();
            timer.UpdateTimerInterval("AIpersonality");


            var categorizedEntries = new Dictionary<string, List<(string Name, float Usage)>>()
    {
        { "programspreferences", AIManager.ProgramMemory?.Select(p => (p.Title, MathF.Log(p.Runtime / 600000f + 1))).ToList() },
        { "itemspreferences", AIManager.ItemMemory?.Select(i => (i.Name, (float)i.Eaten)).ToList() },
        { "actionspreferences", AIManager.ActionMemory?.Select(a => (a.Name, (float)a.Interactions)).ToList() },
        { "touchpreferences", AIManager.TouchMemory?.Select(t => (t.Name, (float)t.Touches)).ToList() }
    };

            foreach (var (category, entries) in categorizedEntries)
            {

                if (entries == null || !entries.Any()) continue; // Skip empty categories

                Category = category; // Assign category dynamically
                float globalAvgUsage = entries.Average(e => e.Usage);

                foreach (var (name, usage) in entries)
                {
                    bool overused = usage > (globalAvgUsage * 1.5f);

                    UpdatePreferenceList(name, overused, (int)usage);
                }

                //DecayPreferences();

                // Assign preferences to the correct AIManager property before clearing
                switch (Category)
                {
                    case "programspreferences":
                        AIManager.ProgramPreferences = preferences;
                        break;
                    case "itemspreferences":
                        AIManager.ItemPreferences = preferences;
                        break;
                    case "actionspreferences":
                        AIManager.ActionPreferences = preferences;
                        break;
                    case "touchpreferences":
                        AIManager.TouchPreferences = preferences;
                        break;
                }

                AIManager.Instance.saveMemory(category); // Save using category directly
                preferences = new();
            }
        }


        private void UpdatePreferenceList(string name, bool overused, int usage)
        {
            float moodMultiplier = GetMoodMultiplier();
            float cravingBoost = CravingBoost * moodMultiplier;

            Random random = new Random();

            // Ensure the history entry exists
            if (!interactionHistory.ContainsKey(name))
            {
                interactionHistory[name] = 0;
            }

            // Ensure an entry exists in preferences
            var entry = preferences.FirstOrDefault(p => p.Name == name);
            if (entry == null)
            {
                entry = new PreferenceEntry { Name = name, Likeability = 0f };
                preferences.Add(entry);
            }

            // Loop for each usage
            for (int i = 0; i < usage; i++)
            {
                float variation = (float)(random.NextDouble() * 0.1 - 0.05); // Random value between -0.05 and +0.05

                if (overused)
                {
                    float penalty = OverindulgencePenalty * (1f + variation);
                    entry.Likeability -= penalty;
                    Console.WriteLine($"AI is getting bored of {name}. Penalty: {penalty:F3}");
                }
                else
                {
                    float boost = cravingBoost * (1f + variation);
                    entry.Likeability += boost;
                    Console.WriteLine($"AI is craving {name}. Boost: {boost:F3}");
                }

                interactionHistory[name]++;
            }

            // Clamp the Likeability value to stay within bounds
            entry.Likeability = Math.Clamp(entry.Likeability, -1.0f, 1.0f);

        }




        private float GetMoodMultiplier() => GetMood() switch
        {
            "Happy" => 1.1f,
            "Poorcondition" => 0.9f,
            "Ill" => 1.2f,
            _ => 1.0f
        };

        private float GetPersonalityTolerance() => AIpersonality switch
        {
            Personality.Stable => 1.0f,
            Personality.Curious => 0.6f,
            Personality.Addictive => 1.2f,
            _ => 0.8f
        };

        private void ApplyVarietySeeking(string currentItem)
        {
            if (!interactionHistory.ContainsKey(currentItem)) return;

            if (interactionHistory[currentItem] >= VarietyThreshold)
            {
                Console.WriteLine($"AI is getting bored of {currentItem} and wants something new!");
                var entry = preferences.Find(p => p.Name == currentItem);
                if (entry != null) entry.Likeability -= OverindulgencePenalty / 2;

                string newItem = FindNewPreference();
                if (!string.IsNullOrEmpty(newItem))
                {
                    var newEntry = preferences.Find(p => p.Name == newItem);
                    if (newEntry != null)
                    {
                        newEntry.Likeability += LearningRate * 2;
                        Console.WriteLine($"AI is now interested in trying {newItem}!");
                    }
                }
            }
        }

        private string FindNewPreference()
        {
            var neutralItems = preferences.Where(p => Math.Abs(p.Likeability) < 0.3f).Select(p => p.Name).ToList();
            return neutralItems.Any() ? neutralItems[new Random().Next(neutralItems.Count)] : "";
        }

        private void DecayPreferences()
        {
            float decayRate = GetMoodDecayMultiplier() * BaseDecayRate;

            foreach (var entry in preferences)
            {
                float oldValue = entry.Likeability;
                entry.Likeability -= Math.Sign(entry.Likeability) * decayRate;

                if (Math.Abs(entry.Likeability) < 0.1f)
                    entry.Likeability = 0;

                if (interactionHistory.TryGetValue(entry.Name, out int interactions) && interactions == 1 && entry.Likeability < 0.3f)
                {
                    entry.Likeability += CravingBoost * 3;
                    Console.WriteLine($"AI is craving {entry.Name} after missing it!");
                }

                if (entry.Likeability <= -0.9f && interactions > 10)
                {
                    entry.Likeability = -0.3f;
                }

                if (entry.Likeability != oldValue)
                    Console.WriteLine($"Preference for {entry.Name} changed from {oldValue:F2} to {entry.Likeability:F2}");
            }
        }

        private float GetMoodDecayMultiplier() => GetMood() switch
        {
            "Happy" => 1.5f,
            "Poorcondition" => 0.5f,
            "Ill" => 2.0f,
            _ => 1.0f
        };

        public string GetOpinion(string item)
        {
            var entry = preferences.Find(p => p.Name == item);
            if (entry == null) return "Neutral";

            return entry.Likeability switch
            {
                > 0.5f => "Likes it",
                < -0.5f => "Dislikes it",
                _ => "Neutral"
            };
        }
    }

    public class PreferenceEntry
    {
        [Line] public string Name { get; set; }
        [Line] public float Likeability { get; set; }
        [Line] public string Personality { get; set; }
    }
}
