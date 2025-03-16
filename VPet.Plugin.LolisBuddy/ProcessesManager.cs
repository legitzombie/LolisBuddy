using System.Collections.Generic;

namespace VPet.Plugin.LolisBuddy
{
    public class ProcessesManager
    {
        public ProcessesManager() { }

        private readonly HashSet<string> WindowsProcess = new()
        { "explorer", "taskmgr", "shellexperiencehost", "runtimebroker", "searchui", "systemsettings", "applicationframehost",
          "dwm", "sihost", "ctfmon", "winlogon", "smartscreen", "securityhealthsystray", "startmenuexperiencehost", "gamebarpresencewriter",
          "textinputhost", "svchost", "csrss", "lsass", "services", "wininit", "fontdrvhost", "msmpeng", "nisrv", "securityhealthservice",
          "wermgr", "conhost", "spoolsv", "rundll32", "dllhost", "perfmon", "taskhostw", "werfault", "audiodg", "wlanext", "igfxtray",
          "hkcmd", "hxtsr", "browser_broker", "deviceassociationframeworkproviderhost", "searchapp" };

        private readonly Dictionary<string, HashSet<string>> CategoryMapping = new()
        {
            { "Browser", new() { "chrome", "brave", "firefox", "edge", "opera", "vivaldi" } },
            { "Game Launcher", new() { "steam", "epicgameslauncher", "origin", "uplay", "battlenet", "riotclient", "rockstarlauncher" } },
            { "Messaging App", new() { "discord", "slack", "telegram", "whatsapp", "zoom", "skype", "teams", "signal", "messenger" } },
            { "Development Tool", new() { "visualstudio", "vscode", "jetbrains", "eclipse", "androidstudio", "xcode", "devenv" } },
            { "Text Editor", new() { "notepad", "notepad++", "sublime_text", "atom" } },
            { "Image Editor", new() { "photoshop", "gimp", "paintdotnet", "krita", "coreldraw" } },
            { "3D Modeling Software", new() { "blender", "maya", "3dsmax", "cinema4d", "zbrush" } },
            { "Rigging Tool", new() { "autorig", "mixamo", "houdini" } },
            { "Game Engine", new() { "unity", "unreal", "godot", "cryengine", "gamemaker" } },
            { "Audio Editing Tool", new() { "audacity", "reaper", "flstudio", "ableton", "protools" } },
            { "Music Player", new() { "spotify", "itunes", "vlc", "winamp", "foobar2000" } },
            { "Video Player", new() { "vlc", "mpc-hc", "mpv", "windowsmediaplayer", "quicktime" } },
            { "Torrenting App", new() { "utorrent", "bittorrent", "qbittorrent", "deluge", "transmission", "tixati", "vuze" } },
            { "Streaming Software", new() { "obs", "streamlabs", "xsplit", "restream", "vmix" } },
            { "Cloud Storage / Backup Service", new() { "dropbox", "googledrive", "onedrive", "megasync", "box", "syncthing", "nextcloud" } },
            { "Virtual Machine / Emulator", new() { "vmware", "virtualbox", "parallels", "qemu", "hyper-v", "bluestacks", "ldplayer", "nox" } },
            { "Benchmarking / Hardware Monitoring Tool", new() { "cpuz", "gpuz", "hwmonitor", "aida64", "prime95", "3dmark", "userbenchmark", "cinebench" } },
            { "Download Manager", new() { "idm", "jdownloader", "eagleget", "free-download-manager", "folx" } },
            { "Password Manager", new() { "keepass", "lastpass", "bitwarden", "1password", "dashlane", "nordpass" } },
            { "Code Compiler / Build Tool", new() { "gcc", "clang", "msbuild", "cmake", "make", "ninja" } },
            { "System Utility / Optimization Tool", new() { "ccleaner", "glaryutilities", "advanced-systemcare", "defraggler", "winoptimizer" } },
            { "PDF Reader / Document Viewer", new() { "adobeacrobat", "foxitreader", "sumatrapdf", "nitropdf", "xpdf" } },
            { "CAD Software", new() { "autocad", "solidworks", "fusion360", "freecad", "rhino", "sketchup" } },
            { "Screenshot / Screen Annotation Tool", new() { "greenshot", "sharex", "snagit", "lightshot", "picpick" } },
            { "Note-Taking / Mind Mapping App", new() { "onenote", "evernote", "notion", "simplenote", "obsidian", "xmind", "freemind" } },
            { "Image Viewer", new() { "photos", "irfanview", "xnview", "honeyview", "faststone", "acdsee", "nomacs" } },
            { "VPN Client", new() { "nordvpn", "expressvpn", "protonvpn", "windscribe", "openvpn" } },
            { "Antivirus", new() { "avp", "egui", "avgui", "mbam", "mcshield", "avastui", "bitdefender", "f-secure", "nortonsecurity", "k7tssecurity" } }
        };

        public List<string> Categorize(string processName, string windowTitle)
        {
            processName = processName.ToLower();
            windowTitle = windowTitle.ToLower();

            List<string> info = new List<string>();
            string category = "Uncategorized";
            string title = windowTitle; // Default to the original title

            // Prioritize detecting a game first
            if (GameDetector.HasGameDLLs(processName))
            {
                category = "Game";
            }

            // Iterate through category mappings
            foreach (var entry in CategoryMapping)
            {
                foreach (var keyword in entry.Value)
                {
                    if (keyword == processName || windowTitle.Contains(keyword))
                    {
                        category = entry.Key;
                        title = keyword; // Set the title to the matched keyword

                        // Special handling for browsers
                        if (category == "Browser")
                        {
                            title = WebpageDetector.Categorize(windowTitle);
                        }

                        return new List<string> { category, title }; // Return immediately after the first match
                    }
                }
            }


            return new List<string> { category, title };
        }


        public bool IsBlacklisted(string name)
        {
            return WindowsProcess.Contains(name.ToLower());
        }

    }


}
