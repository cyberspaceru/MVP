using System;
using System.Diagnostics;
using System.Linq;
using Optional;

namespace MVP.Connectors
{
    public static class ProcessUtils
    {
        public const long UndefinedModuleAddress = 0;

        public static ProcessModule GetModuleByName(this MvProcess process, string moduleName)
        {
            return process
                .SomeWhen(x => x.IsDefined)
                .Map(x =>
                {
                    return x.Modules.Cast<ProcessModule>().FirstOrDefault(s => s.ModuleName.Equals(moduleName, StringComparison.OrdinalIgnoreCase));
                })
                .ValueOr(() => null);
        }

        public static long GetModuleAddressByName(this MvProcess process, string moduleName)
        {
            return process.GetModuleByName(moduleName)
                .SomeNotNull()
                .Map(x => (long)x.BaseAddress)
                .ValueOr(() => 0);
        }

    }
}
