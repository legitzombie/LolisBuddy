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
using System.Security.Cryptography;
using System.Reflection;

namespace VPet.Plugin.LolisBuddy
{
    public class IOManager
    {
        private const string EncryptionKey = "Lolisecret"; // Change this for security
        private static string EncryptText(string text, string key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
                aes.IV = new byte[16];

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    byte[] inputBytes = Encoding.UTF8.GetBytes(text);
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
                    return Convert.ToBase64String(encryptedBytes);
                }
            }
        }

        // AES Decryption Method
        private static string DecryptText(string encryptedText, string key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
                aes.IV = new byte[16];

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }
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
                            if (fi.Name == "memory.lps")
                            {
                                string art = ReadEmbeddedResource("VPet.Plugin.LolisBuddy.ascii-art.txt");
                                fileContent = fileContent.Substring(art.Length).Trim();
                                MessageBox.Show(fileContent.Length.ToString());
                                fileContent = DecryptText(fileContent, EncryptionKey);
                            }
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
                               
                                if (firstIteration && fi.Name == "memory.lps")
                                {
                                    var art = ReadEmbeddedResource("VPet.Plugin.LolisBuddy.ascii-art.txt"); 
                                    File.WriteAllText(filePath, art);
                                    firstIteration = false;
                                }

                                lps = LPSConvert.SerializeObject(item);
                                string line = $"{name}:|{lps.ToString().Replace("\r", "").Replace("\n", "").Trim()}";
                                if (fi.Name == "memory.lps") line = EncryptText(line, EncryptionKey);
                                File.AppendAllText(filePath, Environment.NewLine + line);
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
