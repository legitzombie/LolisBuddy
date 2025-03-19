namespace VPet.Plugin.LolisBuddy.Utilities
{
    using System.Diagnostics;
    using System.Linq;

    public class GameDetector
    {

        public static bool HasGameDLLs(string processName)
        {
            try
            {
                Process process = Process.GetProcessesByName(processName).FirstOrDefault();
                if (process == null)
                    return false;

                foreach (ProcessModule module in process.Modules)
                {
                    if (GameDLLs.DLLs.Contains(module.ModuleName.ToLower()))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                // Ignore access denied errors
            }
            return false;
        }
    }

}
