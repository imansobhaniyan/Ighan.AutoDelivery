using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text.Json;

using Ighan.AutoDelivery.Core;

namespace Ighan.AutoDelivery.ConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var dt = DateTime.Now;

            var projects = JsonSerializer.Deserialize<List<Project>>(File.ReadAllText(Environment.CurrentDirectory + "\\config.json"));

            if (args != null && args.Length > 0)
                projects = projects.Where(f => args.Any(arg => f.Name == arg)).ToList();

            if (args == null || !args.Contains("monitoring"))
                projects = projects.Where(f => f.Name != "monitoring").ToList();

            foreach (var project in projects)
            {
                GitRepositoryPuller.Pull(project.Git.Path);

                DirectoryFullGraphRemover.Remove(project.ProjectOutputPath);

                DotNetBuilder.Release(project.ProjectPath, project.ProjectOutputPath);

                IISWebSiteManager.Stop(project.Site.Name);

                DirectoryFullGraphRemover.Remove(project.BackUpPath);

                DirectoryFullGraphCopier.Copy(project.Site.Path, project.BackUpPath);

                DirectoryFullGraphRemover.Remove(project.Site.Path);

                DirectoryFullGraphCopier.Copy(project.ProjectOutputPath, project.Site.Path);

                foreach (var projectSetting in project.ProjectSettings)
                    FileContentReplacer.Replace(projectSetting.FilePath, projectSetting.OldValue, projectSetting.NewValue);

                IISWebSiteManager.Start(project.Site.Name);
            }

            var period = DateTime.Now - dt;

            Console.WriteLine($"Finished in: {period}");

            Console.ReadLine();

            //try
            //{
            //    TestRequestSender.Send(project.TestRequestUrl);
            //}
            //catch
            //{
            //    IISWebSiteManager.Stop(project.Site.Name);

            //    DirectoryFullGraphRemover.Remove(project.Site.Path);

            //    DirectoryFullGraphCopier.Copy(project.BackUpPath, project.Site.Path);

            //    IISWebSiteManager.Start(project.Site.Name);
            //}

            //Console.WriteLine("Hello World!");
        }

        public class Project
        {
            public string Name { get; set; }

            public string BasePath { get; set; }

            public Site Site { get; set; }

            public Git Git { get; set; }

            public string ProjectPath { get; set; }

            public string ProjectOutputPath { get; set; }

            public string BackUpPath { get; set; }

            public string TestRequestUrl { get; set; }

            public List<Setting> ProjectSettings { get; set; }
        }

        public class Site
        {
            public string Name { get; set; }

            public string Path { get; set; }
        }

        public class Git
        {
            public string Path { get; set; }
        }

        public class Setting
        {
            public string FilePath { get; set; }

            public string OldValue { get; set; }

            public string NewValue { get; set; }
        }
    }
}
