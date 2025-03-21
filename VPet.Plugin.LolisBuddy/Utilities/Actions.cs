using LinePutScript.Converter;
using LinePutScript.Localization.WPF;
using VPet_Simulator.Core;
using VPet_Simulator.Windows.Interface;

namespace VPet.Plugin.LolisBuddy.Utilities
{
    public class Actions
    {
        public static ActionEntry action = new ActionEntry();

        public static void assignEvents(IMainWindow main)
        {
            main.Main.Event_TouchBody += TouchBodyHandler;
            main.Main.Event_TouchHead += TouchHeadHandler;
            main.Main.Event_WorkEnd += WorkEndHandler;
            main.Main.Event_WorkStart += WorkStartHandler;
            main.Event_TakeItem += EatHandler;
        }

        public static void EatHandler(Food food)
        {
            string name = food.Name.Translate();
            string type = food.Type.ToString().Translate();

            Interactions.Update(name, new ItemEntry { Name = name, Type = type });
        }

        public static void TouchBodyHandler()
        {
            Interactions.Update("touchbody", new TouchEntry { Name = "touchbody" });
        }

        public static void TouchHeadHandler()
        {
            Interactions.Update("touchhead", new TouchEntry { Name = "touchhead" });
        }

        public static void WorkEndHandler(WorkTimer.FinishWorkInfo info)
        {

        }

        public static void WorkStartHandler(GraphHelper.Work work)
        {
            string name = work.Name.Translate();
            Interactions.Update(name, new ActionEntry { Name = name });
        }

    }

    public class ActionEntry : IMemoryEntry
    {
        [Line] public string Name { get; set; }

        [Line] public int Interactions { get; set; } = 1;
    }
}
