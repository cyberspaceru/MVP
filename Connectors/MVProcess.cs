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
        public static long UndefinedModuleMark => ProcessUtils.UndefinedModuleMark;

        public Process Current { get; private set; }
        public IntPtr Pointer { get; private set; }

        public bool IsDefined => Pointer != IntPtr.Zero && !Current.HasExited;
        public string ProcessName => Current.ProcessName;
        public long MemoryUsage => Current.WorkingSet64;
        public ProcessModuleCollection Modules => Current.Modules;

        private readonly Dictionary<string, ProcessModule> _mDefinedModules = new Dictionary<string, ProcessModule>();

        public ProcessModule this[string moduleName] => _mDefinedModules.ContainsKey(moduleName) ? _mDefinedModules[moduleName] : null;

        private MvProcess(Process current)
        {
            this.Current = current;
            this.Pointer = Kernel32.OpenProcess(Kernel32.ProcessAccessFlags.VirtualMemoryRead, false, current.Id);
        }

        public bool DefineModule(string moduleName)
        {
            return this.GetModuleByName(moduleName)
                .SomeNotNull()
                .Map(x =>
                {
                    _mDefinedModules.Add(moduleName, x);
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
            var definedModulesDescription = _mDefinedModules.Count.ToString();
            _mDefinedModules.SomeWhen(x => x.Count != 0).MatchSome(x =>
            {
                definedModulesDescription += "\n[";
                foreach (var module in x)
                {
                    definedModulesDescription += "\n" + module.Key + " : " + module.Value.BaseAddress.ToString("X");
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
