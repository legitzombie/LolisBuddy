using LinePutScript.Converter;

namespace VPet.Plugin.LolisBuddy.Utilities
{
    public class Foods
    {

    }

    public class FoodEntry
    {
        [Line] public string Name { get; set; }
        [Line] public int Eaten { get; set; }
    }
}
