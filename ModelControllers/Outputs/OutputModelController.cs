using HowLeaky.OutputModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowLeaky.ModelControllers
{
    public class OutputModelController : HLController
    {

        public List<OutputDataModel> dailyOutputs;
        StreamWriter outWriter;

        public OutputModelController() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sim"></param>
        public OutputModelController(Simulation sim)
        {
            this.Sim = sim;

            //Create some stream writers from the simuation name
            dailyOutputs = new List<OutputDataModel>();
            outWriter = new StreamWriter(sim.Name + "_daily.csv", false);

            //Create a list of output models
            foreach (HLController hlc in Sim.ActiveControlllers)
            {
                //Get the daily Ouputs and add to the list
                if (hlc.GetType().BaseType == typeof(HLObjectController))
                {
                    List<OutputDataModel> odms = ((HLObjectController)hlc).GetOutputModels();

                    if (odms != null)
                    {
                        dailyOutputs.AddRange(odms);
                    }
                }
                else
                {
                    List<OutputDataModel> odms = hlc.GetOutputModels();
                    if (odms != null)
                    {
                        dailyOutputs.AddRange(odms);
                    }
                }
            }

            List<string> outputHeaders = new List<string>();

            foreach (OutputDataModel odm in dailyOutputs)
            {
                outputHeaders.Add(odm.WriteHeaders());
            }

            //Write the headers
            outWriter.WriteLine(String.Join(",", outputHeaders.ToArray()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dailyOnly"></param>
        public void WriteData(bool dailyOnly = true)
        {
            List<string> outputData = new List<string>();

            foreach (OutputDataModel odm in dailyOutputs)
            {
                outputData.Add(odm.WriteData());
            }

            //Write the headers
            outWriter.WriteLine(String.Join(",", outputData.ToArray()));
        }

        /// <summary>
        /// 
        /// </summary>
        public void Finalise()
        {
            outWriter.Close();
        }
    }
}
