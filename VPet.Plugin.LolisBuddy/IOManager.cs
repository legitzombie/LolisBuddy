using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using LinePutScript;
using LinePutScript.Converter;
using Path = System.IO.Path;

namespace VPet.Plugin.LolisBuddy
{
    public class IOManager
    {
        private static readonly string LogPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private SecurityManager securityManager = new SecurityManager();
        public IOManager() { }

        public List<T> LoadLPS<T>(string path, string name = null) where T : new()
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            List<T> lines = new List<T>();
            DirectoryInfo di = new DirectoryInfo(path);

            foreach (FileInfo fi in di.EnumerateFiles("*.lps"))
            {
                try
                {
                    if (name == null || fi.Name == name + ".lps")
                    {
                        string fileContent = File.ReadAllText(fi.FullName, Encoding.UTF8);

                        if (fi.Name == "memory.lps")
                        {
                            // Remove ASCII Art header if it exists
                            string art = ReadEmbeddedResource("VPet.Plugin.LolisBuddy.ASCII.baka.txt");
                            fileContent = fileContent.Substring(art.Length).Trim();

                            // Split encrypted entries properly
                            string[] encryptedEntries = fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                            StringBuilder decryptedContent = new StringBuilder();

                            foreach (string entry in encryptedEntries)
                            {
                                try
                                {
                                    string decrypted = securityManager.DecryptText(entry, securityManager.fetchKey());
                                    decryptedContent.AppendLine(decrypted);
                                }
                                catch (Exception decryptEx)
                                {
                                    MessageBox.Show($"Decryption Failed: {decryptEx.Message}\nData: {entry}");
                                }
                            }

                            fileContent = decryptedContent.ToString().Trim();
                        }

                        var tmp = new LpsDocument(fileContent);

                        foreach (ILine li in tmp)
                        {
                            if (li != null)
                                lines.Add(LPSConvert.DeserializeObject<T>(li));
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"[LoadLPS] ERROR processing file {fi.Name}: {ex}");
                }
            }
            return lines;
        }





        public void SaveLPS(object obj, string path, string name)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string filePath = Path.Combine(path, name + ".lps");
                bool isMemoryFile = name == "memory";
                List<string> lines = new List<string>();

                if (obj == null)
                {
                    throw new ArgumentNullException(nameof(obj), "Cannot save a null object.");
                }

                if (isMemoryFile)
                {
                    string art = ReadEmbeddedResource("VPet.Plugin.LolisBuddy.ASCII.baka.txt");
                    File.WriteAllText(filePath, art + Environment.NewLine, Encoding.UTF8);
        
                }

                if (obj is IEnumerable<object> list)
                {
                    foreach (var item in list)
                    {
                        LpsDocument lps = LPSConvert.SerializeObject(item);
                        string line = $"{name}:|{lps.ToString().Replace("\r", "").Replace("\n", "").Trim()}";

                        if (isMemoryFile)
                        {
                            string key = securityManager.fetchKey();
                            line = securityManager.EncryptText(line, key);
                        }

                        lines.Add(line);
                    }
                }
                else
                {
                    LpsDocument lps = LPSConvert.SerializeObject(obj);
                    string line = $"{name}:|{lps.ToString().Replace("\r", "").Replace("\n", "").Trim()}";

                    if (isMemoryFile)
                    {
                        string key = securityManager.fetchKey();
                        line = securityManager.EncryptText(line, key);
                    }

                    lines.Add(line);
                }

              
                if (isMemoryFile) File.AppendAllLines(filePath, lines, Encoding.UTF8);
                else File.WriteAllLines(filePath, lines, Encoding.UTF8);
          
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving object to {path}: {ex.Message}");
            }
        }




        static string ReadEmbeddedResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return string.Join("\n", reader.ReadToEnd().Split('\n')); // Adds \n after each line
            }
        }


    }
}
