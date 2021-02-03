using Microsoft.Web.Administration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Ighan.AutoDelivery.Core
{
    public class IISWebSiteManager
    {
        public static void Start(string name)
        {
            Console.WriteLine("Start => starting website with name " + name);

            new ServerManager().Sites.FirstOrDefault(f => f.Name == name).Start();

            new ServerManager().ApplicationPools.FirstOrDefault(f => f.Name == name).Start();

            Console.WriteLine("End => starting website");
        }

        public static void Stop(string name)
        {
            Console.WriteLine("Start => stoping website with name " + name);

            new ServerManager().Sites.FirstOrDefault(f => f.Name == name).Stop();

            new ServerManager().ApplicationPools.FirstOrDefault(f => f.Name == name).Stop();

            Thread.Sleep(2000);

            Console.WriteLine("End => stoping website");
        }
    }
}
