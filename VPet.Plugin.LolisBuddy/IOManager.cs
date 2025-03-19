using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

        private string setDefaultPath(string path)
        {
            if (path == null)
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
            return path;
        }

        public List<T> LoadLPS<T>(string path = null, string name = null, bool encrypted = false, bool erase = false) where T : new()
        {
            path = setDefaultPath(path);
            EnsureDirectoryExists(path);
            List<T> lines = new List<T>();
            if (path.Contains("memory")) name ??= DateTime.Now.Date.ToString("yyyy-MM-dd");

            if (erase) EraseOldestFile(path, name);

            foreach (FileInfo fi in new DirectoryInfo(path).EnumerateFiles("*.lps"))
            {
                if (name != null && fi.Name != name + ".lps") continue;

                try
                {
                    string fileContent = File.ReadAllText(fi.FullName, Encoding.UTF8);

                    if (encrypted)
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

        private void EraseOldestFile(string path, string name)
        {

            List<FileInfo> files = new DirectoryInfo(path).EnumerateFiles("*.lps").ToList();
            int maxFiles = (path.Contains("short_term")) ? 1 : 14;

            if (files.Count > maxFiles)
            {
                FileInfo oldestFile = files.OrderBy(f => f.CreationTime).FirstOrDefault();
                if (oldestFile != null)
                {
                    try
                    {
                        string[] oldestFileLines = File.ReadAllLines(oldestFile.FullName, Encoding.UTF8);

                        if (oldestFileLines.Length > 0)
                        {
                            // Remove only matching lines from all other files
                            foreach (FileInfo file in files)
                            {
                                if (file != oldestFile)
                                {
                                    var currentLines = File.ReadAllLines(file.FullName, Encoding.UTF8).ToList();
                                    currentLines.RemoveAll(line => oldestFileLines.Contains(line));
                                    File.WriteAllLines(file.FullName, currentLines, Encoding.UTF8);
                                }
                            }
                        }

                        oldestFile.Delete();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"[EraseOldestFile] ERROR deleting file {oldestFile.Name}: {ex}");
                    }
                }
            }

        }


        private void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }

        public void SaveLPS(object obj, string path = null, string name = null, bool encrypt = false, bool append = false)
        {
            try
            {
                path = setDefaultPath(path);
                EnsureDirectoryExists(path);
                name ??= DateTime.Now.Date.ToString("yyyy-MM-dd");
                string filePath = Path.Combine(path, name + ".lps");

                if (obj == null) throw new ArgumentNullException(nameof(obj), "Cannot save a null object.");

                List<string> lines = new List<string>();

                if (encrypt) { ASCIIManager.DisplayASCII("VPet.Plugin.LolisBuddy.ASCII.baka.txt", filePath); }

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

                if (append || encrypt) File.AppendAllLines(filePath, lines, Encoding.UTF8);
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
