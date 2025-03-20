using System;
using System.Collections.Generic;
using System.Timers;

namespace VPet.Plugin.LolisBuddy.Core
{
    public class TimerManager
    {
        private Dictionary<string, Timer> timers = new Dictionary<string, Timer>();
        private Dictionary<string, ElapsedEventHandler> eventHandlers = new Dictionary<string, ElapsedEventHandler>();

        public TimerManager(string name, int delay, int chance)
        {
            this.name = name;
            interval = delay;
            this.chance = chance;
        }

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

            Timer timer = new Timer(interval)
            {
                AutoReset = true, // Ensure it repeats
                Enabled = true
            };

            ElapsedEventHandler handler = (sender, e) =>
            {
                try
                {
                    callback();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Timer callback error: {ex.Message}");
                }
            };

            timer.Elapsed += handler;
            eventHandlers[name] = handler;
            timers[name] = timer;
        }

        public void UpdateTimerInterval(string name, int newInterval, int chance)
        {
            if (timers.TryGetValue(name, out Timer timer))
            {
                timer.Stop();
                timer.Interval = newInterval;
                interval = newInterval;
                this.chance = chance;

                timer.Start(); // ✅ Ensure it restarts after updating
            }
        }

        public void RemoveTimer(string name)
        {
            if (timers.TryGetValue(name, out Timer timer))
            {
                if (eventHandlers.TryGetValue(name, out ElapsedEventHandler handler))
                {
                    timer.Elapsed -= handler;
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
