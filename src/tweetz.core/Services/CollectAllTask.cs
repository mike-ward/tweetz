using System;
using System.Threading.Tasks;

namespace tweetz.core.Services
{
    public static class CollectAllTask
    {
        public static ValueTask Execute()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            return default;
        }
    }
}