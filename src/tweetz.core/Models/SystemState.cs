using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Win32;
using tweetz.core.Infrastructure;

namespace tweetz.core.Models
{
    public class SystemState : ISystemState, INotifyPropertyChanged
    {
        public bool IsSleeping { get; set; }

        public bool IsRegisteredInStartup
        {
            get
            {
                using var registryKey = OpenStartupSubKey();
                return registryKey.GetValue(ApplicationName) != null;
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

        private static RegistryKey OpenStartupSubKey() => Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        private static string ComputeMD5(string input)
        {
            using var md5 = System.Security.Cryptography.MD5.Create();
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            var sb = new StringBuilder();
            for (var i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}