using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ighan.AutoDelivery.ConsoleDemo.Models
{
    public class Solution
    {
        public string RepositoryPath { get; set; }

        public string DestinationPath { get; set; }

        public List<Project> Projects { get; set; }

        public List<Setting> SolutionSettings { get; set; }
    }
}
