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
        private static long _mGlowObjectAnchor;
        private static long _mEntityListAnchor;

        private static void Main()
        {
            var mvProcess = FindProcess(ProcessName);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            InitilazeBaseAddresses(mvProcess);
            Console.WriteLine(sw.ElapsedMilliseconds + " ms.");
            Console.ReadKey();
        }

        private static void InitilazeBaseAddresses(MvProcess process)
        {
            // Find out the View Matrix base address:
            _mViewMatrixAnchor = 0x3 + (long)process.ScanModuleByPattern(ClientModule, Signatures.ViewMatrixPattern);
            _mViewMatrixAnchor = 0xB0 + process.ReadPointer32(_mViewMatrixAnchor);

            // Find out the GlowO Object base address: 
            _mGlowObjectAnchor = process.ReadPointer32((long)process[ClientModule].BaseAddress + Signatures.GlowObjectOffset);
            _mGlowObjectAnchor = process.ReadPointer32(_mGlowObjectAnchor);

            // Find out the Entity List base address: 
            _mEntityListAnchor = 0xB + (long)process.ScanModuleByPattern(ClientModule, Signatures.EntityListPattern);
            _mEntityListAnchor = process.ReadPointer32(_mEntityListAnchor);

            Entity localPlayer = Entity.ReadLocalPlayerByEntityListAnchor(process, _mEntityListAnchor);
            Console.WriteLine(localPlayer);
            Entity enemy = Entity.ReadAnyByEntityListAnchor(process, _mEntityListAnchor, 1);
            Console.WriteLine(enemy);
            Console.WriteLine(enemy.Anchor.ToString("X"));

            Console.WriteLine($"ViewMatrix: {_mViewMatrixAnchor:X}, GlowObject: {((long)process[ClientModule].BaseAddress + Signatures.GlowObjectOffset):X}, EntityList: {_mEntityListAnchor:X}");
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
