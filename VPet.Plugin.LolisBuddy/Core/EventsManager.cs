using LinePutScript.Converter;
using LinePutScript.Localization.WPF;
using VPet.Plugin.LolisBuddy.Utilities;
using VPet_Simulator.Core;
using VPet_Simulator.Windows.Interface;

namespace VPet.Plugin.LolisBuddy.Core
{
    public class EventsManager
    {

        private static EventsManager _instance;
        private static readonly object _lock = new object();

        public static EventsManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new EventsManager();
                    return _instance;
                }
            }
        }

        public static ActionEntry action = new ActionEntry();

        public static void assignEvents(IMainWindow main)
        {
            main.Main.Event_TouchBody += TouchBodyHandler;
            main.Main.Event_TouchHead += TouchHeadHandler;
            main.Main.Event_WorkEnd += WorkEndHandler;
            main.Main.Event_WorkStart += WorkStartHandler;
            main.Event_TakeItem += EatHandler;
        }

        private static void EatHandler(Food food)
        {
            string name = food.Name.Translate();
            string type = food.Type.ToString().Translate();

            InteractionsManager.Update(name, new ItemEntry { Name = name, Type = type });
        }

        private static void TouchBodyHandler()
        {
            InteractionsManager.Update("touchbody", new TouchEntry { Name = "touchbody" });
        }

        private static void TouchHeadHandler()
        {
            InteractionsManager.Update("touchhead", new TouchEntry { Name = "touchhead" });
        }

        private static void WorkEndHandler(WorkTimer.FinishWorkInfo info)
        {

        }

        private static void WorkStartHandler(GraphHelper.Work work)
        {
            string name = work.Name.Translate();
            InteractionsManager.Update(name, new ActionEntry { Name = name });
        }

    }

    public class ActionEntry : IMemoryEntry
    {
        [Line] public string Name { get; set; }

        [Line] public int Interactions { get; set; } = 1;
    }
}
