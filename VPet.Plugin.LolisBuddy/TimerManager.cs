using System;
using System.Collections.Generic;
using System.Timers;

namespace VPet.Plugin.LolisBuddy
{
    public class TimerManager
    {
        private Dictionary<string, Timer> timers = new Dictionary<string, Timer>();

        public TimerManager() { }

        public void AddOrUpdateTimer(string name, int interval, Action callback)
        {
            if (timers.ContainsKey(name))
            {
                UpdateTimerInterval(name, interval);
                return;
            }

            Timer timer = new Timer(interval);
            timer.Elapsed += (sender, e) => callback();
            timer.AutoReset = true;
            timer.Enabled = true;

            timers[name] = timer;
        }

        public void UpdateTimerInterval(string name, int newInterval)
        {
            if (timers.TryGetValue(name, out Timer timer))
            {
                timer.Stop();
                timer.Interval = newInterval;
                timer.Start();
            }
        }

        public void RemoveTimer(string name)
        {
            if (timers.TryGetValue(name, out Timer timer))
            {
                timer.Stop();
                timer.Dispose();
                timers.Remove(name);
            }
        }


        public void StopAllTimers()
        {
            foreach (var timer in timers.Values)
            {
                timer.Stop();
                timer.Dispose();
            }
            timers.Clear();
        }
    }

}
