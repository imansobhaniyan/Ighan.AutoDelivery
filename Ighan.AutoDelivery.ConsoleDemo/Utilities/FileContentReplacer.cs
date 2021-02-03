using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ighan.AutoDelivery.Core
{
    public class FileContentReplacer
    {
        public static void Replace(string path, string oldValue, string newValue)
        {
            File.WriteAllText(path, File.ReadAllText(path).Replace(oldValue, newValue));
        }
    }
}
