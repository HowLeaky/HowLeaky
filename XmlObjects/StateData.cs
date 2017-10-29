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
        public bool state { get; set; }

        public StateData() { }
    }
}
