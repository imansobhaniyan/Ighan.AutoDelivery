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

        public static void ReplaceByKey(string path, string key, string newValue)
        {
            var lines = File.ReadAllLines(path);

            var resultLines = new List<string>();

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Trim().StartsWith("//"))
                    continue;

                if (lines[i].Contains($"\"{key}\""))
                {
                    var addComma = lines[i].EndsWith(",");
                    resultLines.Add($"\"{key}\": \"{newValue}\"{(addComma ? "," : string.Empty)}//replaced");
                }
                else
                    resultLines.Add(lines[i]);
            }

            File.WriteAllLines(path, resultLines);
        }
    }
}
