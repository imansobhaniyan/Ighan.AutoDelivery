using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ighan.AutoDelivery.Core
{
    public class DirectoryFullGraphCopier
    {
        public static void Copy(string sourcePath, string destinationPath)
        {
            Console.WriteLine($"Start: coping => {sourcePath} into: {destinationPath}");

            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*",
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*",
                SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(sourcePath, destinationPath), true);

            Console.WriteLine($"End: coping");
        }
    }
}
