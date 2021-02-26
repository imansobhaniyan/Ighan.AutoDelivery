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

            if (Directory.Exists(sourcePath))
            {
                CreateDirectory(new DirectoryInfo(destinationPath));

                foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));

                foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
                    File.Copy(newPath, newPath.Replace(sourcePath, destinationPath), true);
            }
            else
            {
                var defaultColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Directory(source) not exists: " + sourcePath);
                Console.ForegroundColor = defaultColor;
            }

            Console.WriteLine($"End: coping");
        }

        private static void CreateDirectory(DirectoryInfo directoryInfo)
        {
            if (directoryInfo.Parent != null)
                CreateDirectory(directoryInfo.Parent);

            if (!directoryInfo.Exists)
                directoryInfo.Create();
        }
    }
}
