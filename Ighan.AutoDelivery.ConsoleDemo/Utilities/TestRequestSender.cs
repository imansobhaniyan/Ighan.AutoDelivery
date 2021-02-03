using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Ighan.AutoDelivery.Core
{
    public class TestRequestSender
    {
        public static void Send(string url)
        {
            using (var webClient = new WebClient())
                webClient.DownloadString(url);

            throw new NotImplementedException();
        }
    }
}
