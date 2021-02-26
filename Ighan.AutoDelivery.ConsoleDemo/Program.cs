using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text.Json;

using Ighan.AutoDelivery.ConsoleDemo.Models;
using Ighan.AutoDelivery.Core;

namespace Ighan.AutoDelivery.ConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var dt = DateTime.Now;

            var solution = JsonSerializer.Deserialize<Solution>(File.ReadAllText(Environment.CurrentDirectory + "\\config.json"));

            if (args != null && args.Length > 0)
                solution.Projects = solution.Projects.Where(f => args.Any(arg => f.Name == arg)).ToList();

            if (args == null || !args.Contains("monitoring"))
                solution.Projects = solution.Projects.Where(f => f.Name != "monitoring").ToList();

            GitRepositoryPuller.Pull(solution.RepositoryPath);

            foreach (var project in solution.Projects)
            {
                DotNetBuilder.Release(Path.Combine(solution.RepositoryPath, project.ProjectPath), Path.Combine(solution.DestinationPath, project.SiteName, "Publish"));

                IISWebSiteManager.Stop(project.SiteName);

                DirectoryFullGraphCopier.Copy(Path.Combine(solution.DestinationPath, project.SiteName, "WebSite"), Path.Combine(solution.DestinationPath, project.SiteName, "BackUp"));

                DirectoryFullGraphCopier.Copy(Path.Combine(solution.DestinationPath, project.SiteName, "Publish"), Path.Combine(solution.DestinationPath, project.SiteName, "WebSite"));

                foreach (var solutionSetting in solution.SolutionSettings)
                    FileContentReplacer.ReplaceByKey(Path.Combine(solution.DestinationPath, project.SiteName, "WebSite", solutionSetting.FileName), solutionSetting.Key, solutionSetting.NewValue);

                foreach (var projectSetting in project.ProjectSettings)
                    FileContentReplacer.ReplaceByKey(Path.Combine(solution.DestinationPath, project.SiteName, "WebSite", projectSetting.FileName), projectSetting.Key, projectSetting.NewValue);

                IISWebSiteManager.Start(project.SiteName);
            }

            var period = DateTime.Now - dt;

            Console.WriteLine($"Finished in: {period}");

            Console.ReadLine();
        }
    }
}
