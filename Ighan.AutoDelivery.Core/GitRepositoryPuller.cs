using LibGit2Sharp;

using System;
using System.Collections.Generic;
using System.Text;

namespace Ighan.AutoDelivery.Core
{
    public class GitRepositoryPuller
    {
        public static void Pull(string path)
        {
            using (var powerShell = PowerShell.Create())
            {
                var result = powerShell
                        .AddScript($"cd \"{path}\"")
                        .AddScript("git pull")
                        .Invoke();
            }
        }
    }
}
