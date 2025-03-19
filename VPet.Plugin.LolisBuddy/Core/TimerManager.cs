using System;
using System.Collections.Generic;
using System.Timers;

namespace VPet.Plugin.LolisBuddy.Core
{
    public class TimerManager
    {
        private Dictionary<string, Timer> timers = new Dictionary<string, Timer>();
        private Dictionary<string, ElapsedEventHandler> eventHandlers = new Dictionary<string, ElapsedEventHandler>();

        public TimerManager(string name, int delay, int chance) { this.name = name; interval = delay; this.chance = chance; }

        public string name { get; set; }
        public int interval { get; set; }
        public int chance { get; set; }

        public void AddOrUpdateTimer(Action callback)
        {

            if (timers.ContainsKey(name))
            {
                UpdateTimerInterval(name, interval, chance);
                return;
            }

            Timer timer = new Timer(interval);
            timer.AutoReset = false; // Prevent overlapping executions

            // Define the event handler separately to prevent multiple subscriptions
            ElapsedEventHandler handler = null;
            handler = (sender, e) =>
            {
                timer.Stop(); // Stop execution to prevent overlapping calls
                callback();
                timer.Start(); // Restart after execution completes
            };

            timer.Elapsed += handler;
            eventHandlers[name] = handler; // Store handler to manage unsubscribing later
            timer.Enabled = true;

            timers[name] = timer;
        }

        public void UpdateTimerInterval(string name, int newInterval, int chance)
        {
            if (timers.TryGetValue(name, out Timer timer))
            {
                timer.Enabled = false; // Fully disable before changing the interval
                timer.Interval = newInterval;
                interval = newInterval;
                this.chance = chance;
                timer.Enabled = true;
            }
        }

        public void RemoveTimer(string name)
        {
            if (timers.TryGetValue(name, out Timer timer))
            {
                if (eventHandlers.TryGetValue(name, out ElapsedEventHandler handler))
                {
                    timer.Elapsed -= handler; // Unsubscribe handler to prevent memory leaks
                    eventHandlers.Remove(name);
                }

                timer.Stop();
                timer.Dispose();
                timers.Remove(name);
            }
        }

        public void StopAllTimers()
        {
            foreach (var kvp in timers)
            {
                kvp.Value.Stop();
                kvp.Value.Dispose();
            }
            timers.Clear();
            eventHandlers.Clear();
        }
    }
}
