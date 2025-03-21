using LinePutScript.Converter;
using VPet.Plugin.LolisBuddy.Core;

namespace VPet.Plugin.LolisBuddy.Utilities
{
    public class Touch
    {
    }

    public class TouchEntry : IMemoryEntry
    {
        [Line] public string Name { get; set; }
        [Line] public int Touches { get; set; } = 1;
    }
}
