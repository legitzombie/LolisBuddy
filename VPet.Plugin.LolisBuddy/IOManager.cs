using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinePutScript.Converter;
using LinePutScript;
using System.Runtime.CompilerServices;
using Panuon.WPF.UI;
using System.Windows;

namespace VPet.Plugin.LolisBuddy
{
    public class IOManager
    {
        public List <T> LoadLPS<T>(string path, string name = null) where T : new()
        {

            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException($"Directory not found: {path}");
            }

            List <T> lines = new List<T>();

            DirectoryInfo di = new DirectoryInfo(path);

                foreach (FileInfo fi in di.EnumerateFiles("*.lps"))
                {

                    try
                    {

                    if (name == null || fi.Name == name + ".lps")
                    {
                        string fileContent = File.ReadAllText(fi.FullName);

                        var tmp = new LpsDocument(fileContent);

                        foreach (ILine li in tmp)
                        {
                            if (li != null) lines.Add(LPSConvert.DeserializeObject<T>(li));
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
            DirectoryInfo di = new DirectoryInfo(path);

            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "Cannot save a null object.");
            }

            foreach (FileInfo fi in di.EnumerateFiles("*.lps"))
            {
                if (fi.Name == name + ".lps")
                {
                    try
                    {
                        LpsDocument lps;
                        string filePath = fi.FullName;

                        // If obj is a list, handle each entry separately
                        if (obj is IEnumerable<object> list)
                        {
                            bool firstIteration = true;
                            foreach (var item in list)
                            {
                                lps = LPSConvert.SerializeObject(item);
                                string line = $"{name}:|{lps.ToString().Replace("\r", "").Replace("\n", "").Trim()}";

                                if (firstIteration)
                                {
                                    // Write first entry (overwrite)
                                    File.WriteAllText(filePath, line);
                                    firstIteration = false;
                                }
                                else
                                {
                                    // Append subsequent entries
                                    File.AppendAllText(filePath, Environment.NewLine + line);
                                }
                            }
                        }
                        else
                        {
                            // Handle single object normally
                            lps = LPSConvert.SerializeObject(obj);
                            string line = $"{name}:|{lps.ToString().Replace("\r", "").Replace("\n", "").Trim()}";
                            File.WriteAllText(filePath, line);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving object to {path}: {ex.Message}");
                    }
                }
            }
        }


    }
}
