using System.Collections.Generic;

namespace Ighan.AutoDelivery.ConsoleDemo.Models
{
    public class Project
    {
        public string Name { get; set; }

        public string SiteName { get; set; }

        public string ProjectPath { get; set; }

        public List<Setting> ProjectSettings { get; set; }
    }
}
