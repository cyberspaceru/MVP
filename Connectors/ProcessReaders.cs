using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVP.DLL.Wrappers;
using MVP.GameConcepts;

namespace MVP.Connectors
{
    public static class ProcessReaders
    {
        public static byte[] ReadByteArray(this MvProcess process, long address, uint pSize)
        {
            var buffer = new byte[pSize];
            try
            {
               Kernel32.ReadProcessMemory(process.Pointer, (IntPtr)address, buffer, pSize, 0U);
            }
            catch(Exception exception)
            {
                Console.WriteLine("Reading memory error for {\n" + process.ToString() + "\n}");
                Console.WriteLine("Exception's message: " + exception.Message);
            }
            return buffer;
        }

        public static char ReadChar(this MvProcess process, long address)
        {
            return BitConverter.ToChar(process.ReadByteArray(address, sizeof(Int16)), 0);
        }

        public static bool ReadBool(this MvProcess process, long address)
        {
            return BitConverter.ToBoolean(process.ReadByteArray(address, sizeof(Int16)), 0);
        }

        public static Int16 ReadInt16(this MvProcess process, long address)
        {
            return BitConverter.ToInt16(process.ReadByteArray(address, sizeof(Int16)), 0);
        }

        public static UInt16 ReadUInt16(this MvProcess process, long address)
        {
            return BitConverter.ToUInt16(process.ReadByteArray(address, sizeof(Int16)), 0);
        }

        public static Int32 ReadInt32(this MvProcess process, long address)
        {
            return BitConverter.ToInt32(process.ReadByteArray(address, sizeof(Int32)), 0);
        }

        public static UInt32 ReadUInt32(this MvProcess process, long address)
        {
            return BitConverter.ToUInt32(process.ReadByteArray(address, sizeof(Int16)), 0);
        }

        public static Int64 ReadInt64(this MvProcess process, long address)
        {
            return BitConverter.ToInt64(process.ReadByteArray(address, sizeof(Int64)), 0);
        }

        public static UInt64 ReadUInt64(this MvProcess process, long address)
        {
            return BitConverter.ToUInt64(process.ReadByteArray(address, sizeof(Int16)), 0);
        }

        public static float ReadFloat(this MvProcess process, long address)
        {
            return BitConverter.ToSingle(process.ReadByteArray(address, sizeof(float)), 0);
        }

        public static double ReadDouble(this MvProcess process, long address)
        {
            return BitConverter.ToDouble(process.ReadByteArray(address, sizeof(double)), 0);
        }

        public static string ReadStringAscii(this MvProcess process, long address, uint size)
        {
            return Encoding.ASCII.GetString(process.ReadByteArray(address, size));
        }

        public static string ReadStringUnicode(this MvProcess process, long address, uint size)
        {
            return Encoding.Unicode.GetString(process.ReadByteArray(address, size));
        }

        public static Vector3 ReadVector3(this MvProcess process, long address)
        {
            return new Vector3
            {
                x = BitConverter.ToSingle(process.ReadByteArray(address, sizeof(float)), 0),
                y = BitConverter.ToSingle(process.ReadByteArray(address + 4, sizeof(float)), 0),
                z = BitConverter.ToSingle(process.ReadByteArray(address + 8, sizeof(float)), 0)
            };
        }

        public static Vector3 ReadVector2(this MvProcess process, long address)
        {
            return new Vector2
            {
                x = BitConverter.ToSingle(process.ReadByteArray(address, sizeof(float)), 0),
                y = BitConverter.ToSingle(process.ReadByteArray(address + 4, sizeof(float)), 0)
            };
        }
    }
}
