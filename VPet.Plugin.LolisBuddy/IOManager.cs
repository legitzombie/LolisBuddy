using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using LinePutScript;
using LinePutScript.Converter;

namespace VPet.Plugin.LolisBuddy
{
    public class IOManager
    {
        private SecurityManager securityManager = new SecurityManager();
        private ASCIIManager ASCIIManager = new ASCIIManager();

        public IOManager() { }

        private string setDefaultPath(string path)
        {
            if (path == null)
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
            return path;
        }

        public List<T> LoadLPS<T>(string path = null, string name = null, bool encrypted = false) where T : new()
        {
            path = setDefaultPath(path);
            EnsureDirectoryExists(path);
            List<T> lines = new List<T>();

            foreach (FileInfo fi in new DirectoryInfo(path).EnumerateFiles("*.lps"))
            {
                if (name != null && fi.Name != name + ".lps") continue;

                try
                {
                    string fileContent = File.ReadAllText(fi.FullName, Encoding.UTF8);

                    if (path.Contains("memory")) { fileContent = ASCIIManager.RemoveASCII("VPet.Plugin.LolisBuddy.ASCII.baka.txt", fileContent); }
                    if (encrypted) { fileContent = securityManager.DecryptLines(fileContent); }

                    foreach (ILine li in new LpsDocument(fileContent))
                    {
                        if (li != null) lines.Add(LPSConvert.DeserializeObject<T>(li));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"[LoadLPS] ERROR processing file {fi.Name}: {ex}");
                }
            }
            return lines;
        }

        private void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }

        public void SaveLPS(object obj, string path = null, string name = null, bool encrypt = false)
        {
            try
            {
                path = setDefaultPath(path);
                EnsureDirectoryExists(path);
                name ??= DateTime.Now.Date.ToString("yyyy-MM-dd");
                string filePath = Path.Combine(path, name + ".lps");

                if (obj == null) throw new ArgumentNullException(nameof(obj), "Cannot save a null object.");

                List<string> lines = new List<string>();

                if (encrypt) ASCIIManager.DisplayASCII("VPet.Plugin.LolisBuddy.ASCII.baka.txt", filePath);

                if (obj is IEnumerable<object> list)
                {
                    foreach (var item in list)
                    {
                        string line = FormatLPS(item, name, encrypt);
                        lines.Add(line);
                    }
                }
                else
                {
                    lines.Add(FormatLPS(obj, name, encrypt));
                }

                if (encrypt) File.AppendAllLines(filePath, lines, Encoding.UTF8);
                else File.WriteAllLines(filePath, lines, Encoding.UTF8);
            }
            catch (IOException)
            {
                
            }
        }


        private string FormatLPS(object obj, string name, bool encrypt)
        {
            string line = $"{name}:|{LPSConvert.SerializeObject(obj).ToString().Replace("\r", "").Replace("\n", "").Trim()}";
            return encrypt ? securityManager.EncryptText(line) : line;
        }
    }
}
