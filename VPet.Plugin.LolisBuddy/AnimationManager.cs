using VPet_Simulator.Core;
using VPet_Simulator.Windows.Interface;

namespace VPet.Plugin.LolisBuddy
{
    public class AnimationManager
    {
        public GraphInfo animation = new GraphInfo();

        public AnimationManager() { }

        public void fetchAnimation(IMainWindow main)
        {
            animation = main.Main.DisplayType;
        }
        public string debugMessage()
        {
            return $"Animation Type: {animation.Type} \n Animation Name: {animation.Name} \n Animation Mood: {animation.ModeType} \n Animation Animat: {animation.Animat}";
        }
    }
}
