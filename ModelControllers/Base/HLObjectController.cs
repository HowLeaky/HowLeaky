using HowLeaky.Interfaces;
using HowLeaky.OutputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HowLeaky.ModelControllers
{
    public class HLObjectController : HLController
    {
        public List<HLController> ChildControllers { get; set; }

        public HLObjectController() : base() { }

        public HLObjectController(Simulation sim) : base(sim)
        {
            ChildControllers = new List<HLController>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        new public List<OutputDataModel> GetOutputModels()
        {
            List<OutputDataModel> outputModels = new List<OutputDataModel>();

            foreach (HLController hlc in ChildControllers)
            {
                outputModels.AddRange(hlc.GetOutputModels());
            }

            return outputModels;
        }
    }
}
