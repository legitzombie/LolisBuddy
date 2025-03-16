using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

        public List<T> LoadLPS<T>(string path, string name = null) where T : new()
        {
            EnsureDirectoryExists(path);
            List<T> lines = new List<T>();

            foreach (FileInfo fi in new DirectoryInfo(path).EnumerateFiles("*.lps"))
            {
                if (name != null && fi.Name != name + ".lps") continue;

                try
                {
                    string fileContent = File.ReadAllText(fi.FullName, Encoding.UTF8);

                    if (name != null && name.Contains("memory", StringComparison.OrdinalIgnoreCase))
                    {
                        fileContent = ASCIIManager.RemoveASCII("VPet.Plugin.LolisBuddy.ASCII.baka.txt", fileContent);
                        fileContent = securityManager.DecryptLines(fileContent);
                    }

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

        public void SaveLPS(object obj, string path, string name)
        {
            try
            {
                EnsureDirectoryExists(path);
                string filePath = Path.Combine(path, name + ".lps");

                if (obj == null) throw new ArgumentNullException(nameof(obj), "Cannot save a null object.");

                bool isMemory = name.Contains("memory", StringComparison.OrdinalIgnoreCase);
                List<string> lines = new List<string>();

                if (isMemory) ASCIIManager.DisplayASCII("VPet.Plugin.LolisBuddy.ASCII.baka.txt", filePath);

                if (obj is IEnumerable<object> list)
                {
                    foreach (var item in list)
                    {
                        string line = FormatLPS(item, name, isMemory);
                        lines.Add(line);
                    }
                }
                else
                {
                    lines.Add(FormatLPS(obj, name, isMemory));
                }

                if (isMemory) File.AppendAllLines(filePath, lines, Encoding.UTF8);
                else File.WriteAllLines(filePath, lines, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving object to {path}: {ex.Message}");
            }
        }

        private string FormatLPS(object obj, string name, bool encrypt)
        {
            string line = $"{name}:|{LPSConvert.SerializeObject(obj).ToString().Replace("\r", "").Replace("\n", "").Trim()}";
            return encrypt ? securityManager.EncryptText(line) : line;
        }
    }
}
