using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using HowLeaky.SyncModels;
using HowLeaky.DataModels;
using System.Xml.Linq;
using System.Linq;
using HowLeaky.Tools.Helpers;
using HowLeaky.Factories;
using HowLeaky.Tools.XML;
using System.ComponentModel;

namespace HowLeaky
{
    public class Project : CustomSyncModel, IXmlSerializable
    {
        //public List<Simulation> Simulations { get; set; }
        //Xml Simulation elements for lazy loading the simualtions
        public List<XElement> SimulationElements { get; set; }
        //Base input data models from the parameter files
        public List<InputModel> InputDataModels { get; set; }

        public List<BackgroundWorker> BackgroundWorkers;

        public string ContactDetails { get; set; }

        public int CurrentSimIndex = 0;
        public int NoSimsComplete = 0;

        public delegate void SimCompleteNotifier();

        public SimCompleteNotifier Notifier;

        /// <summary>
        /// Need default constructor for populating via Entity Framework 
        /// </summary>
        public Project()
        {
            //Simulations = new List<Simulation>();
            SimulationElements = new List<XElement>();
            InputDataModels = new List<InputModel>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public Project(string fileName) : this()
        {
            //Assume this a a legitimate hlk file
            ReadXml(XmlReader.Create(fileName));

            //Check that the data models are OK

            //Run the simulations
            //RunSimulations();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="noProcessors"></param>
        public void Run(int noProcessors = 0)
        {
            //Get the number of processors correct - a negative number will mean thats how many processors to leave to the user
            if (noProcessors < 1)
            {
                noProcessors = Environment.ProcessorCount - noProcessors;
            }
            else
            {
                noProcessors = Math.Max(noProcessors, Environment.ProcessorCount);
            }

            //Load the simulations - lazy load using string paths

            //Just run at the moment - Threading to come later

            //Pop the top simulation from the simulation elements
            foreach (XElement xe in SimulationElements)
            {
                List<InputModel> simModels = SimInputModelFactory.GenerateSimInputModels(xe, InputDataModels);

                Simulation sim = new Simulation(simModels);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public void ReadXml(XmlReader reader)
        {

            XDocument doc = XDocument.Load(reader);

            //Get the list of models
            XElement projectElement = doc.Element("Project");

            //Set the project properties
            CreatedBy = XMLUtilities.readXMLAttribute(projectElement.Attribute("CreatedBy"));
            DateTime? createdDate = DateUtilities.TryParseDate(XMLUtilities.readXMLAttribute(projectElement.Attribute("CreationDate")).Split(new char[] { ' ' })[0], "dd/MM/yyyy");
            CreatedDate = createdDate == null ? new DateTime(1, 1, 1) : createdDate.Value;
            ContactDetails = XMLUtilities.readXMLAttribute(projectElement.Attribute("ContactDetails"));
            ModifiedBy = XMLUtilities.readXMLAttribute(projectElement.Attribute("ModifiedBy"));

            Name = projectElement.Element("Name").Value.ToString();

            //Read all of the climate data models
            List<XElement> ClimateDatalements = new List<XElement>(projectElement.Elements("ClimateData").Elements("DataFile"));

            //Read all of the models
            List<XElement> TemplateElements = new List<XElement>(projectElement.Elements().Where(x => x.Name.ToString().Contains("Templates")));
            List<XElement> TypeElements = new List<XElement>();

            foreach (XElement te in TemplateElements)
            {
                foreach (XElement xe in te.Elements())
                {
                    TypeElements.Add(xe);
                }
            }
            //Read all of the simualtions
            SimulationElements = new List<XElement>(projectElement.Elements("Simulations").Elements("SimulationObject"));

            InputDataModels = new List<InputModel>();

            //Create input models from the xml elements
            foreach (XElement xe in TypeElements)
            {
                InputDataModels.Add(RawInputModelFactory.GenerateRawInputModel(xe));
            }

            //Create the Climate models - these aren't deserialised so don't come out of the factory
            foreach (XElement xe in ClimateDatalements)
            {
                ClimateInputModel cim = new ClimateInputModel();
                cim.FileName = xe.Attribute("href").Value.ToString();
                InputDataModels.Add(cim);
            }

            //Initialise the models
            foreach (InputModel im in InputDataModels)
            {
                im.Init();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(XmlWriter writer)
        {
            //This will output the models to a project directory
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberOfThreads"></param>
        public void RunSimulations(int numberOfThreads = -1)
        {
            int noCoresToUse = numberOfThreads;
            int noCores = Environment.ProcessorCount;

            //Simulation sim = SimulationFactory.GenerateSimulationXML(SimulationElements[0], InputDataModels);
            //sim.Run();

            //return;
            
            if (numberOfThreads <= 0)
            {
                noCoresToUse = noCores + numberOfThreads;
            }
            //Create database for outputs
            //SQLite

            //Reset the counters
            CurrentSimIndex = 0;
            NoSimsComplete = 0;

            //Create a list of background workers
            BackgroundWorkers = new List<BackgroundWorker>(noCoresToUse);

            //Populate the Background workers and run
            for (int i = 0; i < noCoresToUse; i++)
            {
                BackgroundWorkers.Add(new BackgroundWorker());
                BackgroundWorkers[i].DoWork += HLBackgroundWorker_DoWork;
                BackgroundWorkers[i].RunWorkerCompleted += HLBackgroundWorker_RunWorkerCompleted;

                XElement xe = GetSimulationElement();

                if (xe != null)
                {
                    // BackgroundWorkers[i].RunWorkerAsync(new List<object>(new object[] { xe, handler }));
                    BackgroundWorkers[i].RunWorkerAsync(new List<object>(new object[] { xe }));
                }
            }
        }

        public XElement GetSimulationElement()
        {
            XElement result = null;

            if (SimulationElements.Count > CurrentSimIndex)
            {
                result = SimulationElements[CurrentSimIndex];
                CurrentSimIndex++;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        private void CancelBackGroundWorkers()
        {
            foreach (BackgroundWorker bw in BackgroundWorkers)
                if (bw.IsBusy)
                {
                    bw.CancelAsync();
                }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HLBackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            List<object> Arguments = e.Argument as List<object>;

            XElement simElement = (XElement)Arguments[0];

            Simulation sim = SimulationFactory.GenerateSimulationXML(simElement, InputDataModels);
            sim.Run();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HLBackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {

            }
            else if (e.Error != null)
            {

            }
            else
            {

            }

            if (Notifier != null)
            {
                Notifier();
            }

            NoSimsComplete++;

            //Update Progress
            Console.WriteLine("{0} % Done.", ((double)NoSimsComplete / SimulationElements.Count * 100).ToString("0.00"));

            BackgroundWorker bw = (BackgroundWorker)sender;

            XElement nextSim = GetSimulationElement();

            if(nextSim == null)
            {
                return;
            }
            else
            {
                bw.RunWorkerAsync(nextSim);
            }
        }
    }
}
