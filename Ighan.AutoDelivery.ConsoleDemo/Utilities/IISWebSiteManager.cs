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

            using (var serverManager = new ServerManager())
            {
                StartWebSite(name, serverManager);

                StartApplicationPool(name, serverManager);
            }

            Console.WriteLine("End => starting website");

            static void StartWebSite(string name, ServerManager serverManager)
            {
                var webSite = serverManager.Sites.FirstOrDefault(f => f.Name == name);

                WaitForWebSiteStopOrStartProcess(webSite);

                if (webSite.State == ObjectState.Stopped)
                    webSite.Start();

                WaitForWebSiteStopOrStartProcess(webSite);
            }

            static void StartApplicationPool(string name, ServerManager serverManager)
            {
                var applicationPool = serverManager.ApplicationPools.FirstOrDefault(f => f.Name == name);

                WaitForApplicationPoolStartOrStopProcess(applicationPool);

                if (applicationPool.State == ObjectState.Stopped)
                    applicationPool.Start();

                WaitForApplicationPoolStartOrStopProcess(applicationPool);
            }
        }

        public static void Stop(string name)
        {
            Console.WriteLine("Start => stoping website with name " + name);

            using(var serverManager = new ServerManager())
            {
                StopWebSite(name, serverManager);
                
                StopApplicationPool(name, serverManager);
            }

            Console.WriteLine("End => stoping website");

            static void StopWebSite(string name, ServerManager serverManager)
            {
                var webSite = serverManager.Sites.FirstOrDefault(f => f.Name == name);

                WaitForWebSiteStopOrStartProcess(webSite);

                if (webSite.State == ObjectState.Started)
                    webSite.Stop();

                WaitForWebSiteStopOrStartProcess(webSite);
            }

            static void StopApplicationPool(string name, ServerManager serverManager)
            {
                var applicationPool = serverManager.ApplicationPools.FirstOrDefault(f => f.Name == name);

                WaitForApplicationPoolStartOrStopProcess(applicationPool);

                if (applicationPool.State == ObjectState.Started)
                    applicationPool.Stop();

                WaitForApplicationPoolStartOrStopProcess(applicationPool);
            }
        }

        private static void WaitForApplicationPoolStartOrStopProcess(ApplicationPool applicationPool)
        {
            while (applicationPool.State == ObjectState.Starting || applicationPool.State == ObjectState.Stopping)
                Thread.Sleep(100);
        }

        private static void WaitForWebSiteStopOrStartProcess(Site webSite)
        {
            while (webSite.State == ObjectState.Starting || webSite.State == ObjectState.Stopping)
                Thread.Sleep(100);
        }
    }
}
