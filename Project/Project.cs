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
using HowLeaky.ModelControllers.Outputs;
using HLRDB;
using System.Data.SQLite;
using System.IO;
using HowLeaky.ModelControllers;
using HowLeaky.OutputModels;
using System.Text;

namespace HowLeaky
{
    public enum OutputType { CSVOutput, SQLiteOutput, NetCDF }

    public class Project : CustomSyncModel, IXmlSerializable
    {
        public List<Simulation> Simulations { get; set; }
        //Xml Simulation elements for lazy loading the simualtions
        public List<XElement> SimulationElements { get; set; }
        //Base input data models from the parameter files
        public List<InputModel> InputDataModels { get; set; }

        public List<XElement> TypeElements { get; set; }

        public List<OutputDataElement> OutputDataElements { get; set; }

        public DateTime StartRunTime;

        public List<HLBackGroundWorker> BackgroundWorkers;

        public string ContactDetails { get; set; }

        public int CurrentSimIndex = 0;
        public int NoSimsComplete = 0;

        public bool HasOwnExecutableSpace = true;

        public OutputType OutputType = OutputType.SQLiteOutput;
        //public OutputType OutputType = OutputType.NetCDF;

        public delegate void SimCompleteNotifier();

        public SimCompleteNotifier Notifier;

        //Members for the output model
        public SQLiteConnection SQLConn;
        //public HLDBContext DBContext = null;
        //public HLNCFile HLNC = null;
        public string OutputPath { get; set; } = null;
        public string FileName { get; set; }

        public bool WriteMonthlyData = false;
        public bool WriteYearlyData = false;

        public AggregationType AggregationType = AggregationType.Sum;

        /// <summary>
        /// Need default constructor for populating via Entity Framework 
        /// </summary>
        public Project()
        {
            Simulations = new List<Simulation>();
            SimulationElements = new List<XElement>();
            InputDataModels = new List<InputModel>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public Project(string fileName) : this()
        {
            FileName = fileName;
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

                Simulation sim = new Simulation(this, simModels);
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
            //List<XElement> TypeElements = new List<XElement>();
            TypeElements = new List<XElement>();

            foreach (XElement te in TemplateElements)
            {
                foreach (XElement xe in te.Elements())
                {
                    TypeElements.Add(xe);
                }
            }
            //Read all of the simualtions
            SimulationElements = new List<XElement>();

            foreach (XElement simChild in projectElement.Elements("Simulations").Elements())
            {
                if (simChild.Name.ToString() == "SimulationObject")
                {
                    SimulationElements.Add(simChild);
                }
                else if (simChild.Name.ToString() == "Folder")
                {
                    SimulationElements.AddRange(simChild.Elements("SimulationObject"));
                }
            }

            InputDataModels = new List<InputModel>();

            //Create input models from the xml elements
            foreach (XElement xe in TypeElements)
            {
                InputDataModels.Add(RawInputModelFactory.GenerateRawInputModel(Path.GetDirectoryName(FileName).Replace("\\", "/"), xe));
            }

            //Create the Climate models - these aren't deserialised so don't come out of the factory
            foreach (XElement xe in ClimateDatalements)
            {
                ClimateInputModel cim = new ClimateInputModel();
                cim.FileName = xe.Attribute("href").Value.ToString().Replace("\\", "/");

                if(cim.FileName.Contains("./"))
                {
                    cim.FileName =(Path.GetDirectoryName(FileName).Replace("\\", "/") + "/" + cim.FileName);
                }

                InputDataModels.Add(cim);
            }

            //Initialise the models
            foreach (InputModel im in InputDataModels)
            {
                im.Init();
            }

            //Create the simualtions
            foreach (XElement xe in SimulationElements)
            {
                //For Testing
                //if(xe == SimulationElements[0])
                //
                Simulations.Add(SimulationFactory.GenerateSimulationXML(this, xe, InputDataModels));
            }

            OutputDataElements = OutputModelController.GetProjectOutputs(this);
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

            if(noCoresToUse <= 0)
            {
                noCoresToUse += noCores;
            }

            //Set up outputs (on main thread)
            if (OutputType == OutputType.CSVOutput)
            {
                FileInfo hlkFile = new FileInfo(FileName);

                if (OutputPath == null)
                {
                    OutputPath = hlkFile.Directory.FullName.Replace("\\","/");
                }

                if (!OutputPath.Contains(":") || OutputPath.Contains("./"))
                {
                    DirectoryInfo outDir = new DirectoryInfo(Path.Combine(hlkFile.Directory.FullName, OutputPath));
                    if (!outDir.Exists)
                    {
                        outDir.Create();
                    }

                    OutputPath = outDir.FullName;
                }
            }
            else if (OutputType == OutputType.SQLiteOutput)
            {
                if (OutputPath == null)
                {
                    OutputPath = FileName.Replace(".hlk", ".sqlite");
                    OutputPath = OutputPath.Replace("\\", "/");
                }

                //if (DBContext == null)
                //{
                //    DBContext = new HLDBContext(new SQLiteConnection("data source=" + OutputPath + ";foreign keys=false"), true);

                //    DBContext.Configuration.AutoDetectChangesEnabled = false;
                //    DBContext.Configuration.ValidateOnSaveEnabled = false;

                //    //DBContext.SaveChanges();
                //}

                //List<string> OutputIndicies = new List<string>();

                //for(int i = 0; i < OutputDataElements.Count; i++)
                //{
                //    OutputIndicies.Add("x" + (i + 1).ToString());
                //}
                if (SQLConn != null && SQLConn.State == System.Data.ConnectionState.Open)
                {
                    SQLConn.Close();
                    
                }

                if(File.Exists(OutputPath))
                {
                    File.Delete(OutputPath);
                }

                SQLiteConnection.CreateFile(OutputPath);

                SQLConn = new SQLiteConnection("Data Source=" + OutputPath + ";Version=3;");
                SQLConn.Open();

                //Will need to create tables
                //Data
                string sql = "create table data (SimId int, Day int," + String.Join(" double,", OutputDataElements.Where(j=>j.IsSelected==true).Select(x=>x.Name)) + " double)";
               // sql = "create table data (SimId int, Day int," + String.Join(" double,", OutputIndicies) + " double)";

                SQLiteCommand command = new SQLiteCommand(sql, SQLConn);
                command.ExecuteNonQuery();

                //Annual sum data
                sql = "create table annualdata (SimId int, Year int," + String.Join(" double,", OutputDataElements.Where(j => j.IsSelected == true).Select(x => x.Name)) + " double)";
               // sql = "create table annualdata (SimId int, Year int," + String.Join(" double,", OutputIndicies) + " double)";

                command = new SQLiteCommand(sql, SQLConn);
                command.ExecuteNonQuery();

                //Annual sum average data
                sql = "create table annualaveragedata (SimId int," + String.Join(" double,", OutputDataElements.Where(j => j.IsSelected == true).Select(x => x.Name)) + " double)";
               // sql = "create table annualaveragedata (SimId int," + String.Join(" double,", OutputIndicies) + " double)";

                command = new SQLiteCommand(sql, SQLConn);
                command.ExecuteNonQuery();

                //Outputs
                sql = "create table outputs (Name string, Description string, Units string, Controller string)";

                command = new SQLiteCommand(sql, SQLConn);
                command.ExecuteNonQuery();

                StringBuilder sb = new StringBuilder();

                sb.Append("INSERT INTO OUTPUTS (Name, Description , Units, Controller) VALUES ");

                foreach (OutputDataElement ode in OutputDataElements)
                {
                    string comma = ",";

                    if(ode == OutputDataElements.First())
                    {
                        comma = "";
                    }

                    sb.Append(comma + "(\"" + ode.Name + "\",\"" + ode.Output.Description + "\",\"" + ode.Output.Unit + "\",\"" + ode.HLController.GetType().Name + "\")");
                }

                sql = sb.ToString();

                command = new SQLiteCommand(sql, SQLConn);
                command.ExecuteNonQuery();

                

                //Simulations
                sql = "create table simulations (Id int, Name string, StartDate DATETIME, EndDate DATETIME)";

                command = new SQLiteCommand(sql, SQLConn);
                command.ExecuteNonQuery();

                //Models
                sql = "create table models (SimId int, Name string, InputType string, LongName string)";

                command = new SQLiteCommand(sql, SQLConn);
                command.ExecuteNonQuery();
            }
            else if (OutputType == OutputType.NetCDF)
            {
                //if (HLNC == null)
                //{
                //  //  HLNC = new HLNCFile(this, this.Simulations[0].StartDate, this.Simulations[0].EndDate, FileName.Replace(".hlk", ".nc"));
                //}
            }
            //SQLite

            //Reset the counters
            CurrentSimIndex = 0;
            NoSimsComplete = 0;

            StartRunTime = DateTime.Now;

            //Create a list of background workers
            BackgroundWorkers = new List<HLBackGroundWorker>(noCoresToUse);

            //Populate the Background workers and run
            for (int i = 0; i < noCoresToUse; i++)
            {
                BackgroundWorkers.Add(new HLBackGroundWorker());
                BackgroundWorkers[i].DoWork += HLBackgroundWorker_DoWork;
                BackgroundWorkers[i].RunWorkerCompleted += HLBackgroundWorker_RunWorkerCompleted;

                BackgroundWorkers[i].WorkerReportsProgress = true;
                BackgroundWorkers[i].WorkerSupportsCancellation = true;


                Simulation sim = GetSimulationElement();

                if (sim != null)
                {
                    BackgroundWorkers[i].Sim = sim;
                    // BackgroundWorkers[i].RunWorkerAsync(new List<object>(new object[] { xe, handler }));
                    //BackgroundWorkers[i].RunWorkerAsync(new List<object>(new object[] { sim }));
                    BackgroundWorkers[i].RunWorkerAsync();
                
                }
            }

            if (HasOwnExecutableSpace)
            {
                while (NoSimsComplete < Simulations.Count)
                {
                    System.Threading.Thread.Sleep(500);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Simulation GetSimulationElement()
        {
            Simulation result = null;

            if (Simulations.Count > CurrentSimIndex)
            {
                result = Simulations[CurrentSimIndex];
                CurrentSimIndex++;
            }

            if(result !=null)
            {
                result.Index = Simulations.IndexOf(result) + 1;
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
            HLBackGroundWorker hlbw = (HLBackGroundWorker)sender;

            //List<object> Arguments = e.Argument as List<object>;

            //Simulation sim = (Simulation)Arguments[0];

            ////hlbw.Sim = SimulationFactory.GenerateSimulationXML(simElement, InputDataModels);
            ////hlbw.Sim.Id = SimulationElements.IndexOf(simElement) + 1;

            //hlbw.Sim = sim;
            //hlbw.Sim

            //Setup output controllers
            try
            {
                hlbw.Sim.Run();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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

            HLBackGroundWorker hlbw = (HLBackGroundWorker)sender;

            //hlbw.Sim.OutputModelController.Finalise();
            
            NoSimsComplete++;

            //Update Progress
            Console.Write("\r{0} % Done.", ((double)NoSimsComplete / SimulationElements.Count * 100).ToString("0.00"));


            if (NoSimsComplete > Simulations.Count)
            {
                DateTime end = DateTime.Now;

                TimeSpan ts = end - StartRunTime;

                Console.WriteLine(ts);
            }

            Simulation nextSim = GetSimulationElement();

            if (nextSim == null)
            {
                return;
            }
            else
            {
                hlbw.Sim = nextSim;
                //hlbw.RunWorkerAsync(new List<object>(new object[] { nextSim }));
                hlbw.RunWorkerAsync();
            }

            if(NoSimsComplete == Simulations.Count)
            {
                if(OutputType == OutputType.SQLiteOutput)
                {
                    SQLConn.Close();
                }
            }
        }
    }
}
