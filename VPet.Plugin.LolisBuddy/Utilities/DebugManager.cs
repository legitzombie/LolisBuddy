using LinePutScript.Converter;
using VPet.Plugin.LolisBuddy.Sys;

namespace VPet.Plugin.LolisBuddy.Utilities
{
    class DebugManager
    {
        public static void logError(DebugEntry error)
        {
            IOManager.SaveLPS(error, FolderPath.Get("logs"), error.Title, false, true);
        }
    }

    public class DebugEntry
    {
        [Line] public string Title { get; set; }
        [Line] public string Error { get; set; }
    }
}
