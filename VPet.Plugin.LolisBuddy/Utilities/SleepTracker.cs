using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using LinePutScript.Converter;
using VPet.Plugin.LolisBuddy.Core;
using VPet.Plugin.LolisBuddy.Sys;
using VPet_Simulator.Windows.Interface;

namespace VPet.Plugin.LolisBuddy.Utilities
{

    public class SleepTracker
    {
        private const int InactivityThresholdMinutes = 10; // Considered inactive after 10 minutes
        private static DateTime? sleepStart = null;

        public SleepTracker() { }

        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        [StructLayout(LayoutKind.Sequential)]
        private struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }

        public void CheckUserActivity(IMainWindow MW)
        {

            int idleTimeMinutes = GetIdleTime() / 60000;

            if (idleTimeMinutes >= InactivityThresholdMinutes)
            {
                if (sleepStart == null)
                {
                    sleepStart = DateTime.Now;
                }
            }
            else
            {
                if (sleepStart != null)
                {
                    DateTime wakeTime = DateTime.Now;
                    LogSleepData(sleepStart.Value, wakeTime);
                    sleepStart = null;
                    AnalyzeSleepPattern();
                    MW.Main.Say(LanguageManager.GenerateSentence(MW.Main.DisplayType.ModeType.ToString(), "Break").Dialogue);
                }
            }
        }

        private int GetIdleTime()
        {
            LASTINPUTINFO lii = new LASTINPUTINFO { cbSize = (uint)Marshal.SizeOf(typeof(LASTINPUTINFO)) };
            GetLastInputInfo(ref lii);
            return Environment.TickCount - (int)lii.dwTime;
        }

        private void LogSleepData(DateTime sleepTime, DateTime wakeTime)
        {
            IdleEntry sleep = new IdleEntry();
            sleep.sleepTime = sleepTime.ToString();
            sleep.wakeTime = wakeTime.ToString();
            sleep.durationTime = (wakeTime - sleepTime).ToString();


            if ((wakeTime - sleepTime).Hours >= 6) sleep.category = "sleep";
            else if ((wakeTime - sleepTime).Hours <= 1) sleep.category = "short_break";
            else sleep.category = "long_break";

            IOManager.SaveLPS(sleep, FolderPath.Get("memory", "behavior", "sleep", "long_term"), null, true, true);
            IOManager.SaveLPS(sleep, FolderPath.Get("memory", "behavior", "sleep", "short_term"), null, true, true);
        }


        private void AnalyzeSleepPattern()
        {
            List<IdleEntry> sleepEntries = IOManager
                .LoadLPS<IdleEntry>(FolderPath.Get("memory", "behavior", "sleep", "long_term"), null, true, true)
                .Where(entry => entry.category == "sleep")
                .ToList();

            if (sleepEntries == null || sleepEntries.Count == 0)
            {
                return;
            }

            List<DateTime> sleepTimes = new List<DateTime>();
            List<DateTime> wakeTimes = new List<DateTime>();
            List<TimeSpan> durations = new List<TimeSpan>();

            foreach (var entry in sleepEntries)
            {
                if (DateTime.TryParse(entry.sleepTime, out DateTime sleepTime) &&
                    DateTime.TryParse(entry.wakeTime, out DateTime wakeTime) &&
                    TimeSpan.TryParse(entry.durationTime, out TimeSpan duration))
                {
                    sleepTimes.Add(sleepTime);
                    wakeTimes.Add(wakeTime);
                    durations.Add(duration);
                }
            }

            if (sleepTimes.Count == 0)
            {
                return;
            }

            // Calculate averages
            TimeSpan avgSleepTime = TimeSpan.FromTicks((long)sleepTimes.Average(t => t.TimeOfDay.Ticks));
            TimeSpan avgWakeTime = TimeSpan.FromTicks((long)wakeTimes.Average(t => t.TimeOfDay.Ticks));
            TimeSpan avgDuration = TimeSpan.FromTicks((long)durations.Average(d => d.Ticks));

            // Determine consistency (standard deviation of sleep times)
            double sleepConsistency = Math.Sqrt(sleepTimes.Average(t => Math.Pow((t.TimeOfDay - avgSleepTime).TotalMinutes, 2)));

            IdleEntry sleepSchedule = new IdleEntry();
            sleepSchedule.sleepTime = avgSleepTime.ToString();
            sleepSchedule.wakeTime = avgWakeTime.ToString();
            sleepSchedule.durationTime = avgDuration.ToString();

            if (sleepConsistency < 30)
                sleepSchedule.consistent = true;
            else
                sleepSchedule.consistent = false;

            IOManager.SaveLPS(sleepSchedule, FolderPath.Get("memory", "behavior"), "sleep_schedule", true, false);
        }

    }
    public class IdleEntry
    {
        [Line] public string sleepTime { set; get; }
        [Line] public string wakeTime { set; get; }
        [Line] public string durationTime { set; get; }
        [Line] public bool consistent { set; get; }
        [Line] public string category { set; get; }
    }
}
