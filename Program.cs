using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MVP.Connectors;
using MVP.Utils;
using Optional;

namespace MVP
{
    public class Program
    {
        private const string ProcessName = "csgo";
        private const string ClientModule = "client.dll";
        private const string EngineModule = "engine.dll";

        private static void Main(string[] args)
        {
            var mvProcess = FindProcess(ProcessName);
            DefineModules(mvProcess, new[] { ClientModule, EngineModule });
            Console.WriteLine(mvProcess.ToString());
            Console.ReadKey();
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
                Console.WriteLine("Can't find the process with name \"" + ProcessName + "\".");
            }
            return result;
        }
    }
}
