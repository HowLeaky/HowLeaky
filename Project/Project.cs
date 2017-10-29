using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using HowLeaky.SyncModels;
using HowLeaky.DataModels;

namespace HowLeaky.Models
{
    public class Project : CustomSyncModel, IXmlSerializable
    {
        public List<Simulation> simulations { get; set; }
        public List<DataModel> dataModels { get; set; }

        public string contactDetails { get; set; }

        /// <summary>
        /// Need default constructor for populating via Entity Framework 
        /// </summary>
        public Project()
        {
            simulations = new List<Simulation>();
            dataModels = new List<DataModel>();
        }

        public Project(string fileName) : this()
        {
            //Assume this a a legitamate hlk file

            //Load the Data Models



            //Check that the data models are OK



            //Load the simulations - lazy load using string paths
        }

        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
