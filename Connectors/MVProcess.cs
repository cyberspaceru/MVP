using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MVP.DLL.Wrappers;
using Optional;

namespace MVP.Connectors
{
    public class MvProcess
    {
        public Process Current { get; private set; }
        public IntPtr Pointer { get; private set; }

        public bool IsDefined => Pointer != IntPtr.Zero && !Current.HasExited;
        public string ProcessName => Current.ProcessName;
        public long MemoryUsage => Current.WorkingSet64;
        public ProcessModuleCollection Modules => Current.Modules;

        private readonly Dictionary<string, long> _mModulesBaseAddress = new Dictionary<string, long>();

        private MvProcess(Process current)
        {
            this.Current = current;
            this.Pointer = Kernel32.OpenProcess(Kernel32.ProcessAccessFlags.VirtualMemoryRead, false, current.Id);
        }

        public bool DefineModule(string moduleName)
        {
            return this.GetModuleAddressByName(moduleName)
                .SomeWhen(x => x != ProcessUtils.UndefinedModuleAddress)
                .Map(x =>
                {
                    _mModulesBaseAddress.Add(moduleName, x);
                    return true;
                })
                .ValueOr(() => false);
        }

        public static MvProcess FindByName(string processName)
        {
            return Process.GetProcessesByName(processName)
                .SomeWhen(x => x.Length > 0)
                .Map(x =>
                {
                    Console.WriteLine("Found " + x.Length + " process(es).");
                    var greasy = (from process in x orderby process.WorkingSet64 select process).Last();
                    return greasy != null ? new MvProcess(greasy) : null;
                })
                .ValueOr(() => null);
        }

        public override string ToString()
        {
            var definedModulesDescription = _mModulesBaseAddress.Count.ToString();
            _mModulesBaseAddress.SomeWhen(x => x.Count != 0).MatchSome(x =>
            {
                definedModulesDescription += "\n[";
                foreach (var module in x)
                {
                    definedModulesDescription += "\n" + module.Key + " : " + module.Value.ToString("X");
                }
                definedModulesDescription += "\n]";
            });
            return $"{nameof(ProcessName)}: {ProcessName}, " +
                   $"\nMainModuleName: {Current.MainModule.ModuleName}," +
                   $"\n{nameof(Pointer)}: {Pointer.ToString("X")}," +
                   $"\n{nameof(MemoryUsage)}: {MemoryUsage / 1024f / 1024f }MB" +
                   $"\nDefined module(s): " + definedModulesDescription;
        }
    }
}
