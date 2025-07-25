using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BasicForm.Common.Heplers
{
    public class EnvFileHelper
    {
        public static string GetEnvFilePath()
        {
            var path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "MarkForm"
            );
            Directory.CreateDirectory(path);
            return Path.Combine(path, ".env");
        }

        public static void SaveEncryptedConnection(string encrypted)
        {
            var envPath = GetEnvFilePath();
            File.WriteAllText(envPath, $"ENCRYPTED_CONN={encrypted}");
        }

        public static string? LoadEncryptedConnection()
        {
            var envPath = GetEnvFilePath();
            if (!File.Exists(envPath)) return null;

            var line = File.ReadAllLines(envPath).FirstOrDefault(x => x.StartsWith("ENCRYPTED_CONN="));
            return line.Substring("ENCRYPTED_CONN=".Length).Trim();
        }
        public static string DecryptString(string encryptedText)
        {
            var fullBytes = Convert.FromBase64String(encryptedText);
            var iv = fullBytes.Take(16).ToArray();
            var cipherBytes = fullBytes.Skip(16).ToArray();

            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(PadKey(GetLocalIpAddress()));
            aes.IV = iv;

            var decryptor = aes.CreateDecryptor();
            var decryptedBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
        public static string GetLocalIpAddress()
        {
            return Dns.GetHostEntry(Dns.GetHostName())
                .AddressList.FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                ?.ToString() ?? throw new Exception("找不到本機 IP");
        }
        private static string PadKey(string key) =>
           key.PadRight(32).Substring(0, 32);
    }
}
