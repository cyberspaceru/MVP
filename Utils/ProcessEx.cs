using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVP.Utils
{
    public static class ProcessEx
    {
        public static List<string> GetAllProcessesNames()
        {
            var result = (from process in Process.GetProcesses() select process.ProcessName).ToList();
            result.Sort();
            return result;
        }

        public static void PrintAllProcessesNames()
        {
            GetAllProcessesNames().ForEach(Console.WriteLine);
        }

    }
}
