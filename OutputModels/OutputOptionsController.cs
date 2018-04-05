using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowLeaky.OutputModels
{
    public class OutputOption {
        public Type ControllerType { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Description { get; set; }

        public bool Active { get; set; } = true;
    }


    public class OutputOptionsController
    {
        public Project Project { get; set; }
        public List<Tuple<string, List<string>>> Outputs { get; set; }

        public OutputOptionsController(Project Project)
        {
            this.Project = Project;
        }
    }
}
