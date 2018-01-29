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
        StreamWriter dailySW;
        //StreamWriter monthlySW;
        //StreamWriter annualSW;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sim"></param>
        public OutputModelController(Simulation sim)
        {
            this.Sim = sim;

            //Create some stream writers from the simuation name
            dailyOutputs = new List<OutputDataModel>();
            dailySW = new StreamWriter(sim.Name + "_daily.csv", false);
            //monthlySW = new StreamWriter(sim.Name + "_monthly.csv", false);
            //annualSW = new StreamWriter(sim.Name + "_annual.csv", false);

            //Create a list of output models
            foreach (HLController hlc in Sim.ActiveControlllers)
            {
                //Get the daily Ouputs and add to the list
                if (hlc.GetType().BaseType == typeof(HLObjectController))
                {
                    dailyOutputs.AddRange(((HLObjectController)hlc).GetOutputModels());
                }
                else
                {
                    dailyOutputs.AddRange(hlc.GetOutputModels());
                }
            }

            List<string> outputHeaders = new List<string>();

            foreach(OutputDataModel odm in dailyOutputs)
            {
                outputHeaders.Add(odm.WriteHeaders());
            }

            //Write the headers
            dailySW.WriteLine(String.Join(",", outputHeaders.ToArray()));
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
            dailySW.WriteLine(String.Join(",", outputData.ToArray()));
        }

        public void Finalise()
        {
            dailySW.Close();
        }
    }
}
