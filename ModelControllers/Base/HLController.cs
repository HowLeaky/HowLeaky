using HowLeaky.DataModels;
using HowLeaky.Interfaces;
using HowLeaky.OutputModels;
using HowLeaky.SyncModels;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace HowLeaky.ModelControllers
{
    public class HLController : CustomSyncModel
    {
        [XmlIgnore]
        public Simulation Sim { get; set; }
        [XmlIgnore]
        public OutputDataModel Output { get; set; }

        public HLController() { }

        public HLController(Simulation sim)
        {
            this.Sim = sim;
        }

        public void InitOutputModel()
        {
            this.Output = new OutputDataModel(this);
        }

        public virtual void Initialise() { }

        public virtual void Simulate() { }

        public virtual void SetStartOfDayParameters() { }

        public List<OutputDataModel> GetOutputModels()
        {
            List<OutputDataModel> outputModels = new List<OutputDataModel>();

            List<PropertyInfo> outputModelProperties = new List<PropertyInfo>(this.GetType().GetProperties().Where(p => p.Name == "Output"));

            foreach (PropertyInfo p in outputModelProperties)
            {
                outputModels.Add((OutputDataModel)p.GetValue(this));
            }
            return outputModels;
        }

        public virtual InputModel GetInputModel()
        {
            return null;
        }
    }
}
