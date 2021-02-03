using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Ighan.AutoDelivery.Core
{
    public class DotNetBuilder
    {
        public static void Release(string path, string outPath)
        {
            var psi = new ProcessStartInfo("cmd.exe", $"dotnet build -c Release -o {outPath}")
            {
                WorkingDirectory = path,
                RedirectStandardInput = true,
                UseShellExecute = false
            };

            Process.Start(psi).WaitForExit();

            throw new NotImplementedException();
        }
    }
}
