using Microsoft.Web.Administration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ighan.AutoDelivery.Core
{
    public class IISWebSiteManager
    {
        public static void Start(string name)
        {
            new ServerManager().Sites.FirstOrDefault(f => f.Name == name).Start();

            throw new NotImplementedException();
        }

        public static void Stop(string name)
        {
            new ServerManager().Sites.FirstOrDefault(f => f.Name == name).Stop();

            throw new NotImplementedException();
        }
    }
}
