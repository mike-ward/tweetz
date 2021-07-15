using System;
using System.ComponentModel;
using System.Runtime.Versioning;
using System.Text;
using Microsoft.Win32;
using tweetz.core.Interfaces;

#pragma warning disable CA1416

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
                    var path = $"\"{AppContext.BaseDirectory}\\tweetz.core.exe\"";
                    registryKey.SetValue(ApplicationName, path);
                }
                else
                {
                    registryKey.DeleteValue(ApplicationName);
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRegisteredInStartup)));
            }
        }

        private static string ApplicationName => ComputeMD5(AppContext.BaseDirectory);

        [SupportedOSPlatform("windows")]
        private static RegistryKey OpenStartupSubKey() => Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", writable: true)!;

        private static string ComputeMD5(string input)
        {
            using var md5 = System.Security.Cryptography.MD5.Create();

            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes  = md5.ComputeHash(inputBytes);
            return Convert.ToHexString(hashBytes);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}