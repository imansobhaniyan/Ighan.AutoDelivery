using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management.Automation;
using System.Text;

namespace Ighan.AutoDelivery.Core
{
    public class DotNetBuilder
    {
        public static void Release(string path, string outPath)
        {
            Console.WriteLine($"Start: Building => {path} Into => {outPath}");

            using (var powerShell = PowerShell.Create())
            {
                var results = powerShell
                        .AddScript($"cd \"{path}\"")
                        .AddScript($"dotnet publish -c Release -o \"{outPath}\"")
                        .Invoke();

                foreach (var result in results)
                    Console.WriteLine(result);
            }

            Console.WriteLine("End: Building");
        }
    }
}
