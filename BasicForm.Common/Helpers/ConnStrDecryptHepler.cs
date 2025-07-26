using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace BasicForm.Common.Helpers
{
    public class ConnStrDecryptHepler
    {
        public static string DecryptWithPossibleIps(string encrypted)
        {
            var fullBytes = Convert.FromBase64String(encrypted);
            var iv = fullBytes.Take(16).ToArray();
            var cipherText = fullBytes.Skip(16).ToArray();

            var candidateIps = GetLocalIPv4Addresses();

            foreach (var ip in candidateIps)
            {
                try
                {
                    using var aes = Aes.Create();
                    aes.Key = Encoding.UTF8.GetBytes(PadKey(ip));
                    aes.IV = iv;

                    var decryptor = aes.CreateDecryptor();
                    var plainBytes = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
                    return Encoding.UTF8.GetString(plainBytes);
                }
                catch
                {
                    // 嘗試下一個 IP
                }
            }

            throw new Exception("無法根據本機 IP 解密連線字串");
        }

        private static List<string> GetLocalIPv4Addresses()
        {
            return Dns.GetHostEntry(Dns.GetHostName())
                      .AddressList
                      .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)
                      .Select(ip => ip.ToString())
                      .ToList();
        }

        private static string PadKey(string key) =>
            key.PadRight(32).Substring(0, 32);
    }
}
