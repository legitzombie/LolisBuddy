using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace VPet.Plugin.LolisBuddy
{
    public class SecurityManager
    {
        public SecurityManager() { }

        private const string encryptionKey = "Lolisecret"; // Change this for security

        public string EncryptText(string text)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(encryptionKey.PadRight(32).Substring(0, 32));
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
        public string DecryptText(string encryptedText, string key)
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

        public string DecryptLines(string fileContent)
        {
            string[] encryptedEntries = fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            StringBuilder decryptedContent = new StringBuilder();

            foreach (string entry in encryptedEntries)
            {
                try
                {
                    string decrypted = DecryptText(entry, encryptionKey);
                    decryptedContent.AppendLine(decrypted);
                }
                catch (Exception decryptEx)
                {
                    MessageBox.Show($"Decryption Failed: {decryptEx.Message}\nData: {entry}");
                }
            }

            fileContent = decryptedContent.ToString().Trim();
            return fileContent;
        }
    }
}
