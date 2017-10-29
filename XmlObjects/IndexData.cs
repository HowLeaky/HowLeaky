using System.Xml.Serialization;

namespace HowLeaky.XmlObjects
{
    //Class for XML Serialisation
    public class IndexData
    {
        [XmlAttribute]
        public int index { get; set; }
        [XmlAttribute]
        public string text { get; set; }

        public IndexData() { }
    }
}

