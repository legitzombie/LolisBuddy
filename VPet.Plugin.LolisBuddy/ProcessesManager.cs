using System;
using System.Collections.Generic;

namespace VPet.Plugin.LolisBuddy
{
    public class ProcessesManager
    {
        public ProcessesManager() { }

        private readonly HashSet<string> WindowsProcess = new() { "explorer", "taskmgr", "shellexperiencehost", "runtimebroker", "searchui", "systemsettings", "applicationframehost", "dwm", "sihost", "ctfmon", "winlogon", "smartscreen", "securityhealthsystray", "startmenuexperiencehost", "gamebarpresencewriter", "textinputhost", "svchost", "csrss", "lsass", "services", "wininit", "fontdrvhost", "msmpeng", "nisrv", "securityhealthservice", "wermgr", "conhost", "spoolsv", "rundll32", "dllhost", "perfmon", "taskhostw", "werfault", "audiodg", "wlanext", "igfxtray", "hkcmd", "hxtsr", "browser_broker", "deviceassociationframeworkproviderhost", "searchapp" };
        private readonly HashSet<string> Antivirus = new() { "avp", "egui", "avgui", "mbam", "mcshield", "avastui", "bitdefender", "f-secure", "nortonsecurity", "k7tssecurity" };
        private readonly HashSet<string> Browsers = new() { "chrome", "brave", "firefox", "edge", "opera", "vivaldi" };
        private readonly HashSet<string> GameLaunchers = new() { "steam", "epicgameslauncher", "origin", "uplay", "battlenet", "riotclient", "rockstarlauncher" };
        private readonly HashSet<string> MessagingApps = new() { "discord", "slack", "telegram", "whatsapp", "zoom", "skype", "teams", "signal", "messenger" };
        private readonly HashSet<string> DevelopmentTools = new() { "visualstudio", "vscode", "jetbrains", "eclipse", "androidstudio", "xcode" };
        private readonly HashSet<string> TextEditors = new() { "notepad", "notepad++", "sublime_text", "atom" };
        private readonly HashSet<string> ImageEditors = new() { "photoshop", "gimp", "paintdotnet", "krita", "coreldraw" };
        private readonly HashSet<string> ModelingSoftware = new() { "blender", "maya", "3dsmax", "cinema4d", "zbrush" };
        private readonly HashSet<string> RiggingTools = new() { "autorig", "mixamo", "houdini" };
        private readonly HashSet<string> GameEngines = new() { "unity", "unreal", "godot", "cryengine", "gamemaker" };
        private readonly HashSet<string> AudioTools = new() { "audacity", "reaper", "flstudio", "ableton", "protools" };
        private readonly HashSet<string> MusicPlayers = new() { "spotify", "itunes", "vlc", "winamp", "foobar2000" };
        private readonly HashSet<string> VideoPlayers = new() { "vlc", "mpc-hc", "mpv", "windowsmediaplayer", "quicktime" };
        private readonly HashSet<string> TorrentingApps = new() { "utorrent", "bittorrent", "qbittorrent", "deluge", "transmission", "tixati", "vuze" };
        private readonly HashSet<string> StreamingSoftware = new() { "obs", "streamlabs", "xsplit", "restream", "vmix" };
        private readonly HashSet<string> CloudStorage = new() { "dropbox", "googledrive", "onedrive", "megasync", "box", "syncthing", "nextcloud" };
        private readonly HashSet<string> VirtualMachines = new() { "vmware", "virtualbox", "parallels", "qemu", "hyper-v", "bluestacks", "ldplayer", "nox" };
        private readonly HashSet<string> BenchmarkingTools = new() { "cpuz", "gpuz", "hwmonitor", "aida64", "prime95", "3dmark", "userbenchmark", "cinebench" };
        private readonly HashSet<string> DownloadManagers = new() { "idm", "jdownloader", "eagleget", "free-download-manager", "folx" };
        private readonly HashSet<string> PasswordManagers = new() { "keepass", "lastpass", "bitwarden", "1password", "dashlane", "nordpass" };
        private readonly HashSet<string> CodeCompilers = new() { "gcc", "clang", "msbuild", "cmake", "make", "ninja" };
        private readonly HashSet<string> SystemUtilities = new() { "ccleaner", "glaryutilities", "advanced-systemcare", "defraggler", "winoptimizer" };
        private readonly HashSet<string> PDFReaders = new() { "adobeacrobat", "foxitreader", "sumatrapdf", "nitropdf", "xpdf" };
        private readonly HashSet<string> CADSoftware = new() { "autocad", "solidworks", "fusion360", "freecad", "rhino", "sketchup" };
        private readonly HashSet<string> ScreenshotTools = new() { "greenshot", "sharex", "snagit", "lightshot", "picpick" };
        private readonly HashSet<string> NoteTakingApps = new() { "onenote", "evernote", "notion", "simplenote", "obsidian", "xmind", "freemind" };
        private readonly HashSet<string> ImageViewers = new() { "photos", "irfanview", "xnview", "honeyview", "faststone", "acdsee", "nomacs" };
        private readonly HashSet<string> VPNClients = new() { "nordvpn", "expressvpn", "protonvpn", "windscribe", "openvpn" };

        public string Categorize(string processName, string windowTitle)
        {
            processName = processName.ToLower();

            if (Browsers.Contains(processName)) return "Browser";
            if (GameLaunchers.Contains(processName)) return "Game Launcher";
            if (MessagingApps.Contains(processName)) return "Messaging App";
            if (DevelopmentTools.Contains(processName)) return "Development Tool";
            if (TextEditors.Contains(processName)) return "Text Editor";
            if (ImageEditors.Contains(processName)) return "Image Editor";
            if (ModelingSoftware.Contains(processName)) return "3D Modeling Software";
            if (RiggingTools.Contains(processName)) return "Rigging Tool";
            if (GameEngines.Contains(processName)) return "Game Engine";
            if (AudioTools.Contains(processName)) return "Audio Editing Tool";
            if (MusicPlayers.Contains(processName)) return "Music Player";
            if (VideoPlayers.Contains(processName)) return "Video Player";
            if (TorrentingApps.Contains(processName)) return "Torrenting App";
            if (StreamingSoftware.Contains(processName)) return "Streaming Software";
            if (CloudStorage.Contains(processName)) return "Cloud Storage / Backup Service";
            if (VirtualMachines.Contains(processName)) return "Virtual Machine / Emulator";
            if (BenchmarkingTools.Contains(processName)) return "Benchmarking / Hardware Monitoring Tool";
            if (DownloadManagers.Contains(processName)) return "Download Manager";
            if (PasswordManagers.Contains(processName)) return "Password Manager";
            if (CodeCompilers.Contains(processName)) return "Code Compiler / Build Tool";
            if (SystemUtilities.Contains(processName)) return "System Utility / Optimization Tool";
            if (PDFReaders.Contains(processName)) return "PDF Reader / Document Viewer";
            if (CADSoftware.Contains(processName)) return "CAD Software";
            if (ScreenshotTools.Contains(processName)) return "Screenshot / Screen Annotation Tool";
            if (NoteTakingApps.Contains(processName)) return "Note-Taking / Mind Mapping App";
            if (ImageViewers.Contains(processName)) return "Image Viewer";
            if (VPNClients.Contains(processName)) return "VPN Client";
            if (Antivirus.Contains(processName)) return "Antivirus";

            return "Uncategorized";
        }

        public bool isBlacklisted(string name)
        {
            if (WindowsProcess.Contains(name)) return true;
            return false;
        }
    }
}
