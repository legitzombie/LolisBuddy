using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LinePutScript.Converter;
using VPet.Plugin.LolisBuddy.Sys;
using VPet.Plugin.LolisBuddy.UI;
using VPet.Plugin.LolisBuddy.Utilities;

namespace VPet.Plugin.LolisBuddy.Core
{
    public class PreferenceManager
    {
        // Singleton instance of PreferenceManager
        private static PreferenceManager _instance;
        private static readonly object _lock = new object(); // Lock object for thread safety

        public static PreferenceManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new PreferenceManager();
                    return _instance;
                }
            }
        }

        private List<PreferenceEntry> preferences = new List<PreferenceEntry>();
        private Dictionary<string, int> interactionHistory = new Dictionary<string, int>();

        private const float LearningRate = 0.005f;
        private const float BaseDecayRate = 0.00001f; // Slower decay
        private const float OverindulgencePenalty = 0.002f; // More boredom effect
        private const float CravingBoost = 0.03f; // Stronger craving effect
        private const int VarietyThreshold = 14;

        public enum Personality { Stable, Curious, Addictive, Balanced }
        public Personality AIpersonality { get; set; } = Personality.Balanced;

        public string Category = "";

        public string getMood()
        {
            return AnimationManager.Instance.animation.ModeType.ToString();
        }

        public void Update()
        {
            List<WindowEntry> programs = AIManager.ProgramMemory;
            List<FoodEntry> food = AIManager.FoodMemory;
            List<ActionEntry> actions = AIManager.ActionMemory;

            var allEntries = new List<(string Name, float Usage)>();

            if (programs != null)
            {
                allEntries.AddRange(programs.Select(item => (item.Title, MathF.Log(item.Runtime / 600000f + 1))));
                Category = "Apps";
            }
            if (food != null)
            {
                allEntries.AddRange(food.Select(meal => (meal.Name, (float)meal.Eaten)));
                Category = "Food";
            }
            if (actions != null)
            {
                allEntries.AddRange(actions.Select(action => (action.Name, (float)action.Interactions)));
                Category = "Action";
            }

            float globalAvgUsage = allEntries.Any() ? allEntries.Average(e => e.Usage) : 0f;

            foreach (var (name, usage) in allEntries)
            {
                bool overused = usage > (globalAvgUsage * 1.5f); // Was heavily used
                bool underused = usage < (globalAvgUsage * 0.5f); // Was ignored

                UpdatePreference(name, overused, underused);
            }

            DecayPreferences();

            if (preferences.Any())
            {
                if (programs != null) AIManager.ProgramPreferences = preferences;

                AIManager.Instance.saveMemory("preferences");
            }
        }

        public void UpdatePreference(string item, bool overused, bool underused)
        {
            float moodMultiplier = GetMoodMultiplier();
            float tolerance = GetPersonalityTolerance();

            var entry = preferences.Find(p => p.Name == item);
            if (entry == null)
            {
                preferences.Add(new PreferenceEntry
                {
                    Name = item,
                    Likeability = underused ? 0.3f * moodMultiplier : -0.3f * moodMultiplier
                });
                interactionHistory[item] = 1;
            }
            else
            {
                if (overused)
                {
                    entry.Likeability -= OverindulgencePenalty; // AI gets bored
                    Console.WriteLine($"AI is getting bored of {item}.");
                }
                else if (underused)
                {
                    entry.Likeability += CravingBoost * 2;
                }

                entry.Likeability = Math.Clamp(entry.Likeability, -1.0f, 1.0f);
                interactionHistory[item]++;
            }

            ApplyVarietySeeking(item);
        }

        private float GetMoodMultiplier()
        {
            return getMood() switch
            {
                "Happy" => 1.1f,
                "Poorcondition" => 0.9f,
                "Ill" => 1.2f,
                _ => 1.0f
            };
        }

        private float GetPersonalityTolerance()
        {
            return AIpersonality switch
            {
                Personality.Stable => 1.0f,
                Personality.Curious => 0.6f,
                Personality.Addictive => 1.2f,
                _ => 0.8f
            };
        }

        private void ApplyVarietySeeking(string currentItem)
        {
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
                        newEntry.Likeability += LearningRate * 2; // Encourage trying new things
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

        public void DecayPreferences()
        {
            float decayRate = GetMoodDecayMultiplier() * BaseDecayRate;

            foreach (var entry in preferences)
            {
                float oldValue = entry.Likeability;
                entry.Likeability -= Math.Sign(entry.Likeability) * decayRate;

                if (Math.Abs(entry.Likeability) < 0.1f)
                    entry.Likeability = 0;

                if (interactionHistory.ContainsKey(entry.Name) && interactionHistory[entry.Name] == 0 && entry.Likeability < 0.3f)
                {
                    entry.Likeability += CravingBoost * 3; // AI starts craving ignored things
                    Console.WriteLine($"AI is craving {entry.Name} after missing it!");
                }

                if (entry.Likeability <= -0.9f && interactionHistory[entry.Name] > 10)
                {
                    entry.Likeability = -0.3f; // Prevents permanent dislike
                }

                if (entry.Likeability != oldValue)
                    Console.WriteLine($"Preference for {entry.Name} changed from {oldValue:F2} to {entry.Likeability:F2}");
            }
        }

        private float GetMoodDecayMultiplier()
        {
            return getMood() switch
            {
                "Happy" => 1.5f,
                "Poorcondition" => 0.5f,
                "Ill" => 2.0f,
                _ => 1.0f
            };
        }

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
