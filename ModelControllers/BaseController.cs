using HowLeaky.Models;
using System.Xml.Serialization;

namespace HowLeaky.ModelControllers
{
    public class HLObject
    {
        [XmlIgnore]
        public Simulation sim { get; set; }

        public HLObject() { }

        public HLObject(Simulation sim)
        {
            this.sim = sim;
        }

        public virtual void Initialise() { }

        public virtual void Simulate() { }
    }


}
