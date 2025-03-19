using System;
using System.Collections.Generic;
using System.Linq;

namespace VPet.Plugin.LolisBuddy
{
    class PreferenceManager
    {
        private Dictionary<string, float> preferences = new Dictionary<string, float>();
        private Dictionary<string, int> interactionHistory = new Dictionary<string, int>(); // Tracks how often AI interacts with an item

        private const float LearningRate = 0.1f;
        private const float BaseDecayRate = 0.01f;
        private const float OverindulgencePenalty = 0.05f;
        private const float CravingBoost = 0.15f; // If AI stops eating something for a while, it "misses" it
        private const int VarietyThreshold = 5; // If AI interacts with the same thing this many times, it starts wanting variety

        public enum Mood { Happy, Neutral, Sad, Sick }
        public enum Personality { Stable, Curious, Addictive, Balanced }

        public Mood CurrentMood { get; set; } = Mood.Neutral;
        public Personality AIpersonality { get; set; } = Personality.Balanced;

        public void UpdatePreference(string item, bool liked)
        {
            float moodMultiplier = GetMoodMultiplier();
            float tolerance = GetPersonalityTolerance();

            if (!preferences.ContainsKey(item))
            {
                preferences[item] = liked ? 1.0f * moodMultiplier : -1.0f * moodMultiplier;
                interactionHistory[item] = 0;
            }
            else
            {
                if (preferences[item] >= tolerance && liked)
                {
                    // AI is overexposed, it gets tired of this item
                    preferences[item] -= OverindulgencePenalty;
                }
                else
                {
                    preferences[item] += (liked ? LearningRate : -LearningRate) * moodMultiplier;
                }
            }

            preferences[item] = Math.Clamp(preferences[item], -1.0f, 1.0f);
            interactionHistory[item]++;

            ApplyVarietySeeking(item);
        }

        private float GetMoodMultiplier()
        {
            return CurrentMood switch
            {
                Mood.Happy => 1.5f,
                Mood.Sad => 2.0f,
                Mood.Sick => 0.5f,
                _ => 1.0f
            };
        }

        private float GetPersonalityTolerance()
        {
            return AIpersonality switch
            {
                Personality.Stable => 1.0f,   // Likes stay strong longer
                Personality.Curious => 0.6f,  // Gets tired of things faster
                Personality.Addictive => 1.2f, // Keeps liking things longer
                _ => 0.8f                     // Balanced
            };
        }

        private void ApplyVarietySeeking(string currentItem)
        {
            if (interactionHistory[currentItem] >= VarietyThreshold)
            {
                Console.WriteLine($"AI is getting bored of {currentItem} and wants something new!");
                // Slightly reduce preference for this item
                preferences[currentItem] -= OverindulgencePenalty / 2;

                // Find something new to like
                string newItem = FindNewPreference();
                if (!string.IsNullOrEmpty(newItem))
                {
                    preferences[newItem] += LearningRate;
                    Console.WriteLine($"AI is now interested in trying {newItem}!");
                }
            }
        }

        private string FindNewPreference()
        {
            var neutralItems = preferences.Where(p => Math.Abs(p.Value) < 0.3).Select(p => p.Key).ToList();
            if (neutralItems.Count > 0)
            {
                Random rand = new Random();
                return neutralItems[rand.Next(neutralItems.Count)];
            }
            return "";
        }

        public void DecayPreferences()
        {
            float decayRate = GetMoodDecayMultiplier() * BaseDecayRate;
            List<string> keys = new List<string>(preferences.Keys);

            foreach (var item in keys)
            {
                if (preferences[item] > 0)
                    preferences[item] -= decayRate;
                else if (preferences[item] < 0)
                    preferences[item] += decayRate;

                // If AI hasn't interacted with something in a long time, cravings kick in
                if (interactionHistory.ContainsKey(item) && interactionHistory[item] == 0 && preferences[item] > 0.5)
                {
                    preferences[item] += CravingBoost;
                    Console.WriteLine($"AI is craving {item} after not having it for a while!");
                }

                if (Math.Abs(preferences[item]) < decayRate)
                    preferences[item] = 0;

                interactionHistory[item] = Math.Max(0, interactionHistory[item] - 1);
            }
        }

        private float GetMoodDecayMultiplier()
        {
            return CurrentMood switch
            {
                Mood.Happy => 1.5f,
                Mood.Sad => 0.5f,
                Mood.Sick => 2.0f,
                _ => 1.0f
            };
        }

        public string GetOpinion(string item)
        {
            if (!preferences.ContainsKey(item))
                return "Neutral";

            float score = preferences[item];
            if (score > 0.5) return "Likes it";
            if (score < -0.5) return "Dislikes it";
            return "Neutral";
        }

        public void PrintPreferences()
        {
            foreach (var item in preferences)
            {
                Console.WriteLine($"{item.Key}: {preferences[item.Key]:F2} ({GetOpinion(item.Key)})");
            }
        }
    }

}
