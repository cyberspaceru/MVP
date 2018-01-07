using System;
using System.Runtime.InteropServices;

namespace MVP.DLL.Wrappers
{
    internal class ProcessStatus
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct ModuleInfo
        {
            public IntPtr lpBaseOfDll;
            public uint SizeOfImage;
            public IntPtr EntryPoint;
        }

        [DllImport("psapi.dll", SetLastError = true)]
        public static extern bool GetModuleInformation(IntPtr hProcess, IntPtr hModule, out ModuleInfo lpmodinfo, uint cb);
    }
}
