using HowLeaky.CustomAttributes;
using System.Xml.Serialization;

namespace HowLeaky.XmlObjects
{
    //Class for XML Serialisation
    public class IndexData
    {
        [XmlAttribute]
        [Input("index")]
        public int index { get; set; }
        [XmlAttribute]
        [Input("text")]
        public string text { get; set; }

        public IndexData() { }
    }
}

