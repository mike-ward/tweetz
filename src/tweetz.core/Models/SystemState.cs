using Microsoft.Win32;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using tweetz.core.Infrastructure;

namespace tweetz.core.Models
{
    public class SystemState : ISystemState, INotifyPropertyChanged
    {
        public bool IsSleeping { get; set; }

        [SupportedOSPlatform("windows")]
        public bool IsRegisteredInStartup
        {
            get
            {
                using var registryKey = OpenStartupSubKey();
                return registryKey.GetValue(ApplicationName) is not null;
            }

            set
            {
                if (IsRegisteredInStartup == value) return;

                using var registryKey = OpenStartupSubKey();
                if (value)
                {
                    var path = $"\"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\tweetz.core.exe\"";
                    registryKey.SetValue(ApplicationName, path);
                }
                else
                {
                    registryKey.DeleteValue(ApplicationName);
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRegisteredInStartup)));
            }
        }

        private static string ApplicationName => ComputeMD5(Assembly.GetExecutingAssembly().Location);

        [SupportedOSPlatform("windows")]
        private static RegistryKey OpenStartupSubKey() => Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", writable: true)!;

        private static string ComputeMD5(string input)
        {
            using var md5 = System.Security.Cryptography.MD5.Create();
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            var sb = new StringBuilder();
            for (var i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2", CultureInfo.InvariantCulture));
            }
            return sb.ToString();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}