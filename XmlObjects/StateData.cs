using HowLeaky.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HowLeaky.XmlObjects
{
    //Class for XML Deserialisation
    public class StateData
    {
        [XmlAttribute]
        [Input("State")]
        public bool State { get; set; }

        public StateData() { }
    }
}
