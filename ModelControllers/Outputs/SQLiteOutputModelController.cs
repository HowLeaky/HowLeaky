using HLRDB;
using HowLeaky.DataModels;
using HowLeaky.OutputModels;
using HowLeaky.Tools.ListExtensions;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowLeaky.ModelControllers.Outputs
{
    public class SQLiteOutputModelController : OutputModelController
    {
        string InsertString = "";
        public override bool DateIsOutput { get; set; } = false;

        //public HLDBContext DBContext;
        public SQLiteConnection SQLConn { get; set; }

        public SQLiteOutputModelController() : base() { }

        List<List<double>> AnnualSumValues;
        List<double> AnnualAverageValues;
        int currentYear = -1;

        //public HLRDB.Simulation DBSim = null;
        //public List<HLRDB.Data> Data = new List<HLRDB.Data>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Sim"></param>
        public SQLiteOutputModelController(Simulation Sim, HLDBContext DBContext) : base(Sim)
        {
            //  this.DBContext = DBContext;

            PrepareVariableNamesForOutput();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Sim"></param>
        /// <param name="SQLConn"></param>
        public SQLiteOutputModelController(Simulation Sim, SQLiteConnection SQLConn) : base(Sim)
        {
            //  this.DBContext = DBContext;
            this.SQLConn = SQLConn;
            PrepareVariableNamesForOutput();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void PrepareVariableNamesForOutput()
        {
            base.PrepareVariableNamesForOutput();

            List<string> OutputNames = new List<string>(Outputs.Select(x => x.Name));

         //   List<string> OutputIndicies = new List<string>();

        //    List<string> ProjectOutputNames = new List<string>(Sim.Project.OutputDataElements.Select(x => x.Name));


            //for (int i = 0; i < OutputNames.Count; i++)
            //{
            //    OutputIndicies.Add("x" + (ProjectOutputNames.IndexOf(OutputNames[i]) + 1).ToString());
            //}

            //Prepare an array for annual outputs
            AnnualSumValues = new List<List<double>>();
            AnnualAverageValues = new List<double>(Outputs.Count).Fill(0);

            //Add sim number to the average values
            AnnualAverageValues.Insert(0, Sim.Id);

            //OutputNames.Insert(0, "Day");
            //OutputNames.Insert(0, "SimId");

            InsertString = "INSERT INTO [TABLE] ([INDICIES]," + String.Join(",", OutputNames) + ") VALUES ";

        //    InsertString = "INSERT INTO [TABLE] ([INDICIES]," + String.Join(",", OutputIndicies) + ") VALUES ";
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Finalise()
        {
            //DBContext.SaveChanges();
            //Add Daily data
            StringBuilder iString = new StringBuilder();
            iString.Append(InsertString.Replace("[TABLE]", "DATA").Replace("[INDICIES]", "SimId,Day"));

            for (int i = 0; i < Values.Count; i++)
            {
                iString.Append((i == 0 ? "" : ",") + "(" + String.Join(",", Values[i]) + ")");
            }

            SQLiteCommand command = new SQLiteCommand(iString.ToString(), SQLConn);
            command.ExecuteNonQuery();

            Values.Clear();

            //Add annual sum data
            iString = new StringBuilder();
            iString.Append(InsertString.Replace("[TABLE]", "ANNUALDATA").Replace("[INDICIES]", "SimId,Year"));

            for (int i = 0; i < AnnualSumValues.Count; i++)
            {
                iString.Append((i == 0 ? "" : ",") + "(" + String.Join(",", AnnualSumValues[i]) + ")");
            }

            command = new SQLiteCommand(iString.ToString(), SQLConn);
            command.ExecuteNonQuery();

            AnnualSumValues.Clear();

            //Add annual average data
            iString = new StringBuilder();
            iString.Append(InsertString.Replace("[TABLE]", "ANNUALAVERAGEDATA").Replace("[INDICIES]", "SimId"));

            iString.Append("(" + String.Join(",", AnnualAverageValues) + ")");

            command = new SQLiteCommand(iString.ToString(), SQLConn);
            command.ExecuteNonQuery();

            AnnualAverageValues.Clear();

            //Add the simulation
            string sql = "INSERT INTO SIMULATIONS (Id, Name, StartDate, EndDate) VALUES (" +
                Sim.Id.ToString() + ",\"" + Sim.Name + "\",\"" + Sim.StartDate.ToLongDateString() +
                "\",\"" + Sim.EndDate.ToLongDateString() + "\")";

            command = new SQLiteCommand(sql, SQLConn);
            command.ExecuteNonQuery();

            //Models
            iString = new StringBuilder();
            iString.Append("INSERT INTO MODELS (SimID, Name, InputType) VALUES ");
            foreach (InputModel im in Sim.InputModels)
            {
                string comma = ",";

                if (im == Sim.InputModels.First())
                {
                    comma = "";
                }
                if (im.LongName != null)
                {
                    string[] nameParts = im.LongName.Split(new char[] { ':' });
                    iString.Append(comma + "(" + Sim.Id.ToString() + ",\"" + nameParts[1] + "\",\"" + nameParts[0] + "\")");
                }
            }

            command = new SQLiteCommand(iString.ToString(), SQLConn);
            command.ExecuteNonQuery();

        }
        /// <summary>
        /// 
        /// </summary>
        public override void WriteDailyData()
        {
            int Day = (Sim.Today - Sim.StartDate).Days + 1;
            int Year = Sim.Today.Year;

            //int DaysInYear = 365 + (DateTime.IsLeapYear(Year) == true ? 1 : 0);

            int noYears = Sim.EndDate.Year - Sim.StartDate.Year + 1;

            List<double> TodaysValues = GetData();

            //Add to annualData
            if (currentYear != Year)
            {
                AnnualSumValues.Add(new List<double>(Outputs.Count).Fill(0));
                currentYear = Year;

                //Add SimID and year to the sums
                AnnualSumValues[AnnualSumValues.Count - 1].Insert(0, currentYear);
                AnnualSumValues[AnnualSumValues.Count - 1].Insert(0, Sim.Id);
            }

            //Add values to the annual sums
            for (int i = 0; i < TodaysValues.Count; i++)
            {
                AnnualSumValues[AnnualSumValues.Count - 1][i + 2] += TodaysValues[i];
            }

            //Add values to the annual averages
            for (int i = 0; i < TodaysValues.Count; i++)
            {
                AnnualAverageValues[i + 1] += TodaysValues[i] / noYears;
            }

            TodaysValues.Insert(0, Day);
            TodaysValues.Insert(0, Sim.Id);

            Values.Add(TodaysValues);
        }
    }
}
