using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ighan.AutoDelivery.Core
{
    public class DirectoryFullGraphRemover
    {
        public static void Remove(string path)
        {
            Directory.Delete(path, true);

            Directory.CreateDirectory(path);
        }
    }
}
