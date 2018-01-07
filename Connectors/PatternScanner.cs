using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using MVP.DLL.Wrappers;

namespace MVP.Connectors
{
    public static class PatternScanner
    {
        private struct PatternElement
        {
            public readonly int Offset;
            public readonly byte Byte;

            public PatternElement(int offset, byte b)
            {
                this.Offset = offset;
                this.Byte = b;
            }
        }

        private static byte[] Read(IntPtr poccessPointer, IntPtr memoryAddress, uint bytesToRead, out int bytesRead)
        {
            var buffer = new byte[bytesToRead];
            Kernel32.ReadProcessMemory(poccessPointer, memoryAddress, buffer, bytesToRead, out var ptrBytesRead);
            bytesRead = ptrBytesRead.ToInt32();
            return buffer;
        }

        public static IntPtr ScanModuleByPattern(this MvProcess process, string moduleName, string pattern)
        {
            var processModule = process[moduleName];
            if (processModule == null)
            {
                Console.WriteLine($"Module {moduleName} is undefined.");
                return IntPtr.Zero;
            }
            return process.ScanByPattern(processModule.BaseAddress, processModule.ModuleMemorySize, pattern);
        }

        public static IntPtr ScanByPattern(this MvProcess process, IntPtr baseAddress, long size, string pattern)
        {
            return process.ScanByPattern(baseAddress, size, BuildPattern(pattern));
        }

        private static IntPtr ScanByPattern(this MvProcess process, IntPtr baseAddress, long size, PatternElement[] pattern)
        {
            var dump = Read(process.Pointer, baseAddress, (uint)size, out _);
            var first = pattern[0].Byte;
            for (var i = 0; i < dump.Length; i++)
            {
                if (dump[i] == first && pattern.All(t => t.Byte == dump[i + t.Offset]))
                {
                    return baseAddress + i;
                }
            }
            return IntPtr.Zero;
        }

        private static PatternElement[] BuildPattern(string source)
        {
            var spllited = source.Trim().Replace("??", "?").Split(' ');
            var pattern = new List<PatternElement>();
            for (var i = 0; i < spllited.Length; i++)
            {
                var s = spllited[i];
                if (!"?".Equals(s))
                {
                    pattern.Add(new PatternElement(i, byte.Parse(s, NumberStyles.HexNumber)));
                }
            }
            return pattern.ToArray();
        }

    }
}
