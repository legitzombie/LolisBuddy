
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

        private float BaseDecayRate = 0.90f;
        private float OverindulgencePenalty = 1f;
        private float CravingBoost = 1f;
        private const int VarietyThreshold = 4;

        public enum Personality { Stable, Curious, Addictive, Balanced }
        public Personality AIpersonality { get; set; } = Personality.Balanced;

        public string Category { get; private set; } = "";

        public string GetMood() => AnimationManager.Instance.animation.ModeType.ToString();

        public void Update()
        {


            var categorizedEntries = new Dictionary<string, List<(string Name, float Usage)>>()
    {
        { "programspreferences", AIManager.ProgramMemory?.Select(p => (p.Process, MathF.Log(p.Runtime / 600000f + 1))).ToList() },
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
            float cravingBoost = CravingBoost * moodMultiplier * 0.1f; // Scaled-down boost

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

            // Compute variety penalty based on interaction frequency
            float varietyPenalty = CalculateVarietyPenalty(name);

            // Loop for each usage
            for (int i = 0; i < usage; i++)
            {
                float variation = (float)(random.NextDouble() * 0.02 - 0.01); // Adjusted range (-0.01 to +0.01)

                if (overused)
                {
                    float penalty = (OverindulgencePenalty + varietyPenalty) * (1f + variation);
                    penalty = Math.Clamp(penalty, 0.01f, 0.05f); // Prevent excessive decrease
                    entry.Likeability -= penalty;
                    Console.WriteLine($"AI is getting bored of {name}. Penalty: {penalty:F3}");
                }
                else
                {
                    float boost = (cravingBoost - varietyPenalty) * (1f + variation);
                    boost = Math.Clamp(boost, 0.01f, 0.05f); // Prevent huge boosts
                    entry.Likeability += boost;
                    Console.WriteLine($"AI is craving {name}. Boost: {boost:F3}");
                }

                interactionHistory[name]++;
            }

            // Apply natural decay to all preferences
            ApplyPreferenceDecay(usage);

            // Apply a bonus to underused options
            ApplyUnderuseBonus(name);

            // Clamp the Likeability value to stay within bounds
            entry.Likeability = Math.Clamp(entry.Likeability, -1.0f, 1.0f);
        }

        private void ApplyPreferenceDecay(int usage)
        {
            float decayRate = BaseDecayRate; // Each cycle, likeability moves 2% toward neutral (0)

            foreach (var entry in preferences)
            {
                    entry.Likeability *= decayRate; // Moves preference slowly back toward 0
                    if (Math.Abs(entry.Likeability) < 0.01f) // If near zero, reset to zero
                    {
                        entry.Likeability = 0;
                    }
            }
        }

        private float CalculateVarietyPenalty(string name)
        {
            int totalInteractions = interactionHistory.Values.Sum();
            float frequencyRatio = (float)interactionHistory[name] / Math.Max(1, totalInteractions);
            return Math.Clamp(VarietyThreshold * frequencyRatio, 0.01f, 0.05f); // Prevent huge penalties
        }

        private void ApplyUnderuseBonus(string currentName)
        {
            int totalInteractions = interactionHistory.Values.Sum();

            foreach (var pref in preferences)
            {
                if (pref.Name != currentName)
                {
                    float underuseBonus = VarietyThreshold * (1f - (float)interactionHistory[pref.Name] / Math.Max(1, totalInteractions));
                    pref.Likeability += Math.Clamp(underuseBonus * 0.05f, 0.001f, 0.02f); // Small controlled bonus
                }
            }
        }





        private float GetMoodMultiplier() => GetMood() switch
        {
            "Happy" => 1.1f,
            "Poorcondition" => 0.9f,
            "Ill" => 1.2f,
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
