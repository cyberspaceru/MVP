using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MVP.Connectors;
using MVP.CSGO;
using MVP.Utils;
using Optional;

namespace MVP
{
    public class Program
    {
        private const string ProcessName = "csgo";
        private const string ClientModule = "client.dll";
        private const string EngineModule = "engine.dll";

        private static long _mViewMatrixBaseAddress;
        private static long _mLocalPlayerBaseAddress;
        private static long _mEntityListBaseAddress;

        private static void Main()
        {
            var mvProcess = FindProcess(ProcessName);
            DefineModules(mvProcess, new[] { ClientModule, EngineModule });
            if (mvProcess[ClientModule] == null)
            {
                Console.WriteLine($"Can't find \"{ClientModule}\" [MODULE] for: \n" + mvProcess);
                Console.ReadKey();
                return;
            }
            Stopwatch sw = new Stopwatch();;
            sw.Start();
            InitilazeBaseAddresses(mvProcess);
            Console.WriteLine(sw.ElapsedMilliseconds + " ms.");
            Console.ReadKey();
        }

        private static void InitilazeBaseAddresses(MvProcess process)
        {
            // Find out the View Matrix base address:
            _mViewMatrixBaseAddress = 0x3 + (long)process.ScanModuleByPattern(ClientModule, Offsets.ViewMatrixPattern);
            _mViewMatrixBaseAddress = 0xB0 + process.ReadInt32(_mViewMatrixBaseAddress);

            // Find out the Local Player base address: 
            _mLocalPlayerBaseAddress = 0x3 + (long)process.ScanModuleByPattern(ClientModule, Offsets.LocalPlayerPattern);
            _mLocalPlayerBaseAddress = process.ReadInt32(_mLocalPlayerBaseAddress);
            _mLocalPlayerBaseAddress = process.ReadInt32(_mLocalPlayerBaseAddress + 4);

            // Find out the Entity List base address: 
            _mEntityListBaseAddress = 0xB + (long)process.ScanModuleByPattern(ClientModule, Offsets.EntityListPattern);
            _mEntityListBaseAddress = process.ReadInt32(_mEntityListBaseAddress);

            Console.WriteLine($"ViewMatrix: {_mViewMatrixBaseAddress}, LocalPlayer: {_mLocalPlayerBaseAddress}, EntityList: {_mEntityListBaseAddress}");
        }

        private static void DefineModules(MvProcess process, string[] modules)
        {
            process.SomeNotNull()
                .MatchSome(x => modules.ToList().ForEach(m => x.DefineModule(m)));
        }

        private static MvProcess FindProcess(string processName)
        {
            var result = MvProcess.FindByName(processName);
            if (result == null)
            {
                ProcessEx.PrintAllProcessesNames();
                Console.WriteLine($"Can't find the process with name \"{ProcessName}\".");
            }
            return result;
        }
    }
}
