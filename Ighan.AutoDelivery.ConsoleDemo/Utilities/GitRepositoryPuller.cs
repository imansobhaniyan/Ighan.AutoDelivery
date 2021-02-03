using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;

namespace Ighan.AutoDelivery.Core
{
    public class GitRepositoryPuller
    {
        public static void Pull(string path)
        {
            Console.WriteLine("Start: Pulling => " + path);

            using (var powerShell = PowerShell.Create())
            {
                var results = powerShell
                        .AddScript($"cd \"{path}\"")
                        .AddScript("git pull")
                        .Invoke();

                foreach (var result in results)
                    Console.WriteLine(result);
            }

            Console.WriteLine("End: pulling");
        }
    }
}
