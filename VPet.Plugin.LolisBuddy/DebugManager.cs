using System.IO;
using System.Reflection;
using LinePutScript.Converter;

namespace VPet.Plugin.LolisBuddy
{
    class DebugManager
    {
        private static readonly string LogFolderPath = Path.Combine(
        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "",
        @"logs\"
);

        private IOManager ioManager = new IOManager();
        public void logError(DebugEntry error)
        {
            ioManager.SaveLPS(error, LogFolderPath, error.Title, false, true);
        }
    }

    public class DebugEntry
    {
        [Line] public string Title { get; set; }
        [Line] public string Error { get; set; }
    }
}
