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
            Console.WriteLine("Please specify config postfix:");

            var configFilePostFix = Console.ReadLine();

            var dt = DateTime.Now;

            var solution = JsonSerializer.Deserialize<Solution>
                (File.ReadAllText(Environment.CurrentDirectory + $"\\config-{configFilePostFix}.json"));

            FilterProjects(args, solution);

            GitRepositoryPuller.Pull(solution.RepositoryPath);

            foreach (var project in solution.Projects)
            {
                DotNetBuilder.Release(Path.Combine(solution.RepositoryPath, project.ProjectPath), GetPath(solution, project, PathType.Publish));

                IISWebSiteManager.Stop(project.SiteName);

                DirectoryFullGraphCopier.Copy(GetPath(solution, project, PathType.WebSite), GetPath(solution, project, PathType.BackUp));

                DirectoryFullGraphCopier.Copy(GetPath(solution, project, PathType.Publish), GetPath(solution, project, PathType.WebSite));

                foreach (var solutionSetting in solution.SolutionSettings)
                    FileContentReplacer.ReplaceByKey(GetPath(solution, project, PathType.WebSite, solutionSetting.FileName), solutionSetting.Key, solutionSetting.NewValue);

                foreach (var projectSetting in project.ProjectSettings)
                    FileContentReplacer.ReplaceByKey(GetPath(solution, project, PathType.WebSite, projectSetting.FileName), projectSetting.Key, projectSetting.NewValue);

                IISWebSiteManager.Start(project.SiteName);
            }

            var period = DateTime.Now - dt;

            Console.WriteLine($"Finished in: {period}");

            Console.ReadLine();
        }

        public enum PathType
        {
            WebSite,
            BackUp,
            Publish,
        }

        public static string GetPath(Solution solution, Project project, PathType pathType, string fileName = null)
        {
            var result = Path.Combine(solution.DestinationPath, project.SiteName, pathType.ToString());
            
            if (!string.IsNullOrWhiteSpace(fileName))
                result = Path.Combine(result, fileName);

            return result;
        }

        private static void FilterProjects(string[] args, Solution solution)
        {
            if (args != null && args.Length > 0)
                solution.Projects = solution.Projects.Where(f => args.Any(arg => f.Name == arg)).ToList();

            if (args == null || !args.Contains("monitoring"))
                solution.Projects = solution.Projects.Where(f => f.Name != "monitoring").ToList();
        }
    }
}
