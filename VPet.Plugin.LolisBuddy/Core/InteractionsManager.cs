using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using LinePutScript.Converter;
using LinePutScript.Localization.WPF;
using VPet.Plugin.LolisBuddy.Utilities;

namespace VPet.Plugin.LolisBuddy.Core
{

    public interface IMemoryEntry
    {
        string Name { get; set; }
    }

    public class InteractionsManager
    {

        private static InteractionsManager _instance;
        private static readonly object _lock = new object();

        public static InteractionsManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new InteractionsManager();
                    return _instance;
                }
            }
        }

        public static void Update<T>(string eventName, T item) where T : IMemoryEntry
        {
            if (item == null) return;

            var memoryDictionary = GetMemoryDictionary<T>();

            if (memoryDictionary.TryGetValue(item.Name, out var existingEntry))
            {
                IncrementCount(existingEntry);
            }
            else
            {
                AddNewEntry(item);
            }

            AIManager.Instance.saveMemory(GetMemoryType<T>());
        }

        private static void AddNewEntry<T>(T item) where T : IMemoryEntry
        {
            switch (item)
            {
                case ItemEntry itemEntry:
                    AIManager.ItemMemory.Add(itemEntry);
                    break;
                case ActionEntry actionEntry:
                    AIManager.ActionMemory.Add(actionEntry);
                    break;
                case TouchEntry touchEntry:
                    AIManager.TouchMemory.Add(touchEntry);
                    break;
            }
        }



        private static Dictionary<string, IMemoryEntry> GetMemoryDictionary<T>() where T : IMemoryEntry
        {
            return typeof(T) switch
            {
                Type t when t == typeof(ItemEntry) => AIManager.ItemMemory
                    .ToDictionary(item => item.Name, item => (IMemoryEntry)item),
                Type t when t == typeof(TouchEntry) => AIManager.TouchMemory
                    .ToDictionary(item => item.Name, item => (IMemoryEntry)item),
                Type t when t == typeof(ActionEntry) => AIManager.ActionMemory
                    .ToDictionary(item => item.Name, item => (IMemoryEntry)item),
                _ => throw new NotSupportedException($"Type {typeof(T).Name} is not supported.")
            };
        }




        private static void IncrementCount(IMemoryEntry entry)
        {
            switch (entry)
            {
                case ItemEntry itemEntry:
                    var item = AIManager.ItemMemory.First(a => a.Name == itemEntry.Name);
                    item.Eaten += 1;
                    break;
                case ActionEntry actionEntry:
                    var action = AIManager.ActionMemory.First(a => a.Name == actionEntry.Name);
                    action.Interactions += 1;
                    break;
                case TouchEntry touchEntry:
                    var touch = AIManager.TouchMemory.First(a => a.Name == touchEntry.Name);
                    touch.Touches += 1;
                    break;
            }
        }

        private static string GetMemoryType<T>()
        {
            return typeof(T) switch
            {
                Type t when t == typeof(ItemEntry) => "items",
                Type t when t == typeof(ActionEntry) => "interact",
                Type t when t == typeof(TouchEntry) => "touch",
                _ => throw new NotSupportedException($"Type {typeof(T).Name} is not supported.")
            };
        }


    }

    public class ItemEntry : IMemoryEntry
    {
        [Line] public string Name { get; set; }
        [Line] public string Type { get; set; }
        [Line] public int Eaten { get; set; } = 1;
    }
}
