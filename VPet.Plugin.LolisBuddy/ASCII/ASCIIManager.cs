using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace VPet.Plugin.LolisBuddy.ASCII
{
    public class ASCIIManager
    {

        public ASCIIManager() { }
        public static string ReadASCII(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return string.Join("\n", reader.ReadToEnd().Split('\n')); // Adds \n after each line
            }
        }

        public static string RemoveASCII(string resourceName, string content)
        {
            string resource = ReadASCII(resourceName);
            return content.StartsWith(resource) ? content.Substring(resource.Length).Trim() : content;
        }


        public static void DisplayASCII(string resourcePath, string filePath)
        {
            string art = ReadASCII(resourcePath);
            File.WriteAllText(filePath, art + Environment.NewLine, Encoding.UTF8);
        }
    }
}
