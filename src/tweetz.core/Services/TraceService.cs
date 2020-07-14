using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace tweetz.core.Services
{
    internal static class TraceService
    {
        public static void Message(string msg, [CallerMemberName] string member = "", [CallerFilePath] string path = "", [CallerLineNumber] int line = 0)
        {
            var message = $"tweetz: {msg} [{member}] {Path.GetFileName(path)}({line})";
            Trace.WriteLine(message);
        }
    }
}