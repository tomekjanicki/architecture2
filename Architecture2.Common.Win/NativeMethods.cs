using System.Runtime.InteropServices;

namespace Architecture2.Common.Win
{
    internal class NativeMethods
    {
        [DllImport("kernel32")]
        public static extern bool AllocConsole();
        
    }
}