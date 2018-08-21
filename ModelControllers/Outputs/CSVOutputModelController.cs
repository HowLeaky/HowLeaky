using HowLeaky.OutputModels;
using HowLeaky.Tools.ListExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowLeaky.ModelControllers.Outputs
{
    public class CSVOutputModelController : OutputModelController
    {
        //public override bool DateIsOutput { get; set; } = false;
        //Daily output writer
        StreamWriter outWriter;

        //Daily output writer
        StreamWriter outWriterMonthly = null;

        //Daily output writer
        StreamWriter outWriterYearly = null;

        bool WriteMonthly;
        bool WriteYearly;
        AggregationType AggregationType;

        List<double> currentMonthOutputData = null;

        List<double> currentYearOutputData = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Sim"></param>
        public CSVOutputModelController(Simulation Sim, string OutputPath, bool WriteMonthly = false, bool WriteYearly = false, AggregationType aggregationType = AggregationType.Sum) : base(Sim, false)
        {
            this.WriteMonthly = WriteMonthly;
            this.WriteYearly = WriteYearly;
            this.AggregationType = aggregationType;

            try
            {
                outWriter = new StreamWriter(Path.Combine(OutputPath, Sim.Index.ToString() + "_daily.csv"), false);

                //Create a monthly output
                if (WriteMonthly)
                {
                    outWriterMonthly = new StreamWriter(Path.Combine(OutputPath, Sim.Index.ToString() + "_monthly.csv"), false);
                }

                //Create an annnual output
                if (WriteYearly)
                {
                    outWriterYearly = new StreamWriter(Path.Combine(OutputPath, Sim.Index.ToString() + "_yearly.csv"), false);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            //PrepareVariableNamesForOutput();
        }


        /// <summary>
        /// 
        /// </summary>
        public override void PrepareVariableNamesForOutput()
        {
            base.PrepareVariableNamesForOutput();

            List<string> outputHeaders = new List<string>(Outputs.Select(x => x.Name));

            //outputHeaders.Insert(0, "Date");

            //foreach (OutputDataModel odm in OutputDataModels)
            //{
            //    string headers = WriteHeaders(odm);
            //    if (headers != "")
            //    {
            //        outputHeaders.Add(headers);
            //    }
            //}

            //Write the headers
            outWriter.WriteLine("Date," + String.Join(",", outputHeaders.ToArray()));

            //Create a monthly output
            if (WriteMonthly)
            {
                outWriterMonthly.WriteLine("Year,Month," + String.Join(",", outputHeaders.ToArray()));
            }

            //Create an annnual output
            if (WriteYearly)
            {
                outWriterYearly.WriteLine("Year" + String.Join(",", outputHeaders.ToArray()));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void WriteDailyData()
        {
            List<double> outputData = GetData();

            //Do Daily output
            outWriter.WriteLine(Sim.Today.ToString("dd/MM/yyyy") + "," + String.Join(",", outputData.ToArray()));


            //Check for do monthly output
            if (WriteMonthly == true)
            {
                if (currentMonthOutputData == null)
                {
                    currentMonthOutputData = new List<double>(outputData.Count).Fill(0);
                }

                //Add todays data
                for (int i = 0; i < currentMonthOutputData.Count; i++)
                {
                    currentMonthOutputData[i] += outputData[i];
                }


                if (Sim.Today.Day == System.DateTime.DaysInMonth(Sim.Today.Year, Sim.Today.Month) || Sim.Today == Sim.EndDate)
                {
                    //Do any aggregating
                    if (AggregationType == AggregationType.Mean)
                    {
                        for (int i = 0; i < currentMonthOutputData.Count; i++)
                        {
                            currentMonthOutputData[i] /= Sim.Today.Day;
                        }
                    }

                    outWriterMonthly.WriteLine(Sim.Today.Year + "," + Sim.Today.Month.ToString("00") + "," + String.Join(",", currentMonthOutputData.ToArray()));
                    currentMonthOutputData = new List<double>(outputData.Count).Fill(0);
                }
            }

            //Check for do annual output
            if (WriteYearly == true)
            {
                if (currentYearOutputData == null)
                {
                    currentYearOutputData = new List<double>(outputData.Count).Fill(0);
                }

                //Add todays data
                for (int i = 0; i < currentYearOutputData.Count; i++)
                {
                    currentYearOutputData[i] += outputData[i];
                }


                if (Sim.Today == new DateTime(Sim.Today.Year, 12, 31) || Sim.Today == Sim.EndDate)
                {
                    //Do any aggregating
                    if (AggregationType == AggregationType.Mean)
                    {
                        for (int i = 0; i < currentYearOutputData.Count; i++)
                        {
                            currentYearOutputData[i] /= Sim.Today.DayOfYear;
                        }
                    }
                    outWriterYearly.WriteLine(Sim.Today.Year + "," + String.Join(",", currentYearOutputData.ToArray()));
                    currentYearOutputData = new List<double>(outputData.Count).Fill(0);
                }
            }

            //foreach (OutputDataModel odm in OutputDataModels)
            //{
            //    string data = WriteData(odm);

            //    if (data != "")
            //    {
            //        outputData.Add(data);
            //    }
            //}

            //Write the data

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="odm"></param>
        /// <returns></returns>
        public string WriteData(OutputDataModel odm)
        {

            List<double> propertyValues = GetData(odm);
            List<string> propertyStringValues = new List<string>();

            //foreach (double d in propertyValues)
            // {
            String.Join(",", propertyValues.ToArray());
            //}

            if (propertyValues.Count == 0)
            {
                return "";
            }
            else
            {
                return String.Join(",", String.Join(",", propertyValues.ToArray()));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="odm"></param>
        /// <returns></returns>
        public string WriteHeaders(OutputDataModel odm, bool useSuffix = true)
        {
            List<string> propertyNames = GetHeaders(odm, useSuffix);

            if (propertyNames.Count == 0)
            {
                return "";
            }
            else
            {
                return String.Join(",", propertyNames.ToArray());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Finalise()
        {
            outWriter.Close();

            if(outWriterMonthly != null)
            {
                outWriterMonthly.Close();
            }

            if (outWriterYearly != null)
            {
                outWriterYearly.Close();
            }
        }
    }
}
