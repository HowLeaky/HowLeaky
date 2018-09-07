using HowLeaky.CustomAttributes;
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
    public enum SummaryTypeEnum { Monthly, Yearly };

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

        List<double> currentMonthOutputData = null;

        List<double> currentYearOutputData = null;

        List<AggregationTypeEnum> AggregationTypes = null;
        List<AggregationSequenceEnum> AggregationSequences = null;

        int MonthInCropCounter = 0;
        int YearInCropCounter = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Sim"></param>
        public CSVOutputModelController(Simulation Sim, string OutputPath, bool WriteMonthly = false, bool WriteYearly = false) : base(Sim, false)
        {
            this.WriteMonthly = WriteMonthly;
            this.WriteYearly = WriteYearly;

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
                CheckAggregatorTypesFilled();
                outWriterMonthly.WriteLine("Year,Month," + String.Join(",", outputHeaders.ToArray()));
            }

            //Create an annnual output
            if (WriteYearly)
            {
                CheckAggregatorTypesFilled();
                outWriterYearly.WriteLine("Year," + String.Join(",", outputHeaders.ToArray()));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CheckAggregatorTypesFilled()
        {
            if (AggregationTypes == null || AggregationSequences == null)
            {
                AggregationTypes = GetAggregationTypes();
                AggregationSequences = GetAggregationSequenceTypes();
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

            if (WriteMonthly == true)
            {
                DateTime ReportDate = new DateTime(Sim.Today.Year, Sim.Today.Month, System.DateTime.DaysInMonth(Sim.Today.Year, Sim.Today.Month));

                WriteSummaryData(outputData, ReportDate, ref MonthInCropCounter, ref currentMonthOutputData, outWriterMonthly, SummaryTypeEnum.Monthly);
            }

            if (WriteYearly == true)
            {
                DateTime ReportDate = new DateTime(Sim.Today.Year, 12, 31);

                WriteSummaryData(outputData, ReportDate, ref YearInCropCounter, ref currentYearOutputData, outWriterYearly, SummaryTypeEnum.Yearly);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputData"></param>
        /// <param name="ReportDate"></param>
        /// <param name="counter"></param>
        /// <param name="summaryOutputs"></param>
        /// <param name="outWriter"></param>
        /// <param name="summaryType"></param>
        public void WriteSummaryData(List<double> outputData, DateTime ReportDate, ref int counter, ref List<double> summaryOutputs, StreamWriter outWriter, SummaryTypeEnum summaryType)
        {

            if (ReportDate > Sim.EndDate)
            {
                ReportDate = Sim.EndDate;
            }

            if (summaryOutputs == null)
            {
                summaryOutputs = new List<double>(outputData.Count).Fill(0);
                counter = 0;
            }

            //Increment the incrop day counter
            if (Sim.VegetationController.CurrentCrop.DaysSincePlanting != 0)
            {
                counter++;
            }

            //Add todays data
            for (int i = 0; i < summaryOutputs.Count; i++)
            {
                if ((AggregationSequences[i] == AggregationSequenceEnum.InCrop && Sim.VegetationController.CurrentCrop.DaysSincePlanting > 0) ||
                    AggregationSequences[i] == AggregationSequenceEnum.Always)
                {
                    if (AggregationTypes[i] == AggregationTypeEnum.Max && outputData[i] > summaryOutputs[i])
                    {
                        summaryOutputs[i] = outputData[i];
                    }
                    else if (AggregationTypes[i] == AggregationTypeEnum.Current)
                    {
                        summaryOutputs[i] = outputData[i];
                    }
                    else
                    {
                        summaryOutputs[i] += outputData[i];
                    }
                }
            }

            if (Sim.Today == ReportDate)
            {
                //Do any averaging on reporting day
                for (int i = 0; i < summaryOutputs.Count; i++)
                {
                    if (AggregationTypes[i] == AggregationTypeEnum.Mean)
                    {
                        if (AggregationSequences[i] == AggregationSequenceEnum.InCrop)
                        {
                            if(counter == 0)
                            {
                                summaryOutputs[i] = 0;
                            }
                            else
                            {
                                summaryOutputs[i] /= counter;
                            }
                        }
                        if (AggregationSequences[i] == AggregationSequenceEnum.Always)
                        {
                            if (summaryType == SummaryTypeEnum.Monthly)
                            {
                                summaryOutputs[i] /= Sim.Today.Day;
                            }
                            if (summaryType == SummaryTypeEnum.Yearly)
                            {
                                summaryOutputs[i] /= Sim.Today.DayOfYear;
                            }
                        }
                    }
                }

                //Write file
                if (summaryType == SummaryTypeEnum.Monthly)
                {
                    outWriter.WriteLine(Sim.Today.Year + "," + Sim.Today.Month.ToString("00") + "," + String.Join(",", summaryOutputs.ToArray()));
                }
                if (summaryType == SummaryTypeEnum.Yearly)
                {
                    outWriter.WriteLine(Sim.Today.Year + "," + String.Join(",", summaryOutputs.ToArray()));
                }
                
                //Reset variables
                summaryOutputs = new List<double>(outputData.Count).Fill(0);
                counter = 0;
            }
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

            if (outWriterMonthly != null)
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
