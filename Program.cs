using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MVP.Connectors;
using MVP.CSGO;
using MVP.CSGO.GameObjects;
using MVP.Utils;
using Optional;

namespace MVP
{
    public class Program
    {
        private const string ProcessName = "csgo";
        private const string ClientModule = "client.dll";
        private const string EngineModule = "engine.dll";

        private static long _mViewMatrixAnchor;
        private static long _mLocalPlayerAnchor;
        private static long _mEntityListAnchor;

        private static void Main()
        {
            var mvProcess = FindProcess(ProcessName);
            DefineModules(mvProcess, new[] { ClientModule, EngineModule });
            Stopwatch sw = new Stopwatch();
            sw.Start();
            InitilazeBaseAddresses(mvProcess);
            Console.WriteLine(sw.ElapsedMilliseconds + " ms.");
            Console.ReadKey();
        }

        private static void InitilazeBaseAddresses(MvProcess process)
        {
            // Find out the View Matrix base address:
            _mViewMatrixAnchor = 0x3 + (long)process.ScanModuleByPattern(ClientModule, Signatures.ViewMatrix);
            _mViewMatrixAnchor = 0xB0 + process.ReadPointer32(_mViewMatrixAnchor);

            // Find out the Entity List base address: 
            _mEntityListAnchor = 0xB + (long)process.ScanModuleByPattern(ClientModule, Signatures.EntityList);
            _mEntityListAnchor = process.ReadPointer32(_mEntityListAnchor);

            Entity localPlayer = Entity.ReadLocalPlayerByEntityListAnchor(process, _mEntityListAnchor);
            Console.WriteLine(localPlayer);

            Console.WriteLine($"ViewMatrix: {_mViewMatrixAnchor:X}, LocalPlayer: {_mLocalPlayerAnchor:X}, EntityList: {_mEntityListAnchor:X}");
        }

        private static void DefineModules(MvProcess process, string[] modules)
        {
            process.SomeNotNull()
                .MatchSome(x =>
                {
                    modules.ToList().ForEach(m => x.DefineModule(m));
                });
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
