using HowLeaky.OutputModels;
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
        public override bool DateIsOutput { get; set; } = false;
        StreamWriter outWriter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Sim"></param>
        public CSVOutputModelController(Simulation Sim, string OutputPath) : base(Sim)
        {
            outWriter = new StreamWriter(Path.Combine(OutputPath, Sim.Index + "_daily.csv"), false);

            PrepareVariableNamesForOutput();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void PrepareVariableNamesForOutput()
        {
            base.PrepareVariableNamesForOutput();

            List<string> outputHeaders = new List<string>(Outputs.Select(x=>x.Name));

            outputHeaders.Insert(0, "Date");

            //foreach (OutputDataModel odm in OutputDataModels)
            //{
            //    string headers = WriteHeaders(odm);
            //    if (headers != "")
            //    {
            //        outputHeaders.Add(headers);
            //    }
            //}

            //Write the headers
            outWriter.WriteLine(String.Join(",", outputHeaders.ToArray()));
        }

        /// <summary>
        /// 
        /// </summary>
        public override void WriteDailyData()
        {
            List<double> outputData = GetData();
            //foreach (OutputDataModel odm in OutputDataModels)
            //{
            //    string data = WriteData(odm);

            //    if (data != "")
            //    {
            //        outputData.Add(data);
            //    }
            //}

            //Write the data
            outWriter.WriteLine(Sim.Today.ToString("dd/MM/yyyy") + "," + String.Join(",", outputData.ToArray()));
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
        }
    }
}
