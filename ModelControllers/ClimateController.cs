using HowLeaky.CustomAttributes;
using HowLeaky.DataModels;
using System;
using System.Collections.Generic;

namespace HowLeaky.ModelControllers
{
    public class ClimateController : HLController
    {
        public ClimateInputModel DataModel { get; set; }

        [Output]
        public double Latitude { get { return DataModel.Latitude; } }
        [Output]
        public double Longitude { get { return DataModel.Longitude; } }
        [Output]
        public double Temperature { get; set; }
        [Output("Daily rainfall amount (mm) as read directly from the P51 file", "mm") ]
        public double Rain { get; set; }  
        [Output("Daily max temperature (oC) as read directly from the P51 file.", "oC")]
        public double MaxTemp { get; set; } 
        [Output("Daily min temperature (oC) as read directly from the P51 file.", "oC")]
        public double MinTemp { get; set; }
        [Output("Daily pan evaporation (mm) as read directly from the P51 file.", "mm")]
        public double PanEvap { get; set; }
        [Output(" Daily solar radition (mMJ/m^2/day) as read directly from the P51 file.", "MJ/m2/day")]
        public double SolarRadiation { get; set; }

        public double YesterdaysRain { get; set; }

        int todayIndex;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sim"></param>
        /// <param name="inputModels"></param>
        public ClimateController(Simulation sim, List<InputModel> inputModels) : base(sim)
        {
            DataModel = (ClimateInputModel)inputModels[0];
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public double RainOnDay(DateTime day)
        {
            int index = (day - DataModel.StartDate.Value).Days;

            if (index > 0 && index <= DataModel.Rain.Count - 1)
            {
                return DataModel.Rain[index];
            }
            return 0;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public override void Initialise()
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        public override void Simulate()
        {
            try
            {
                todayIndex = (Sim.Today - DataModel.StartDate.Value).Days;
                Temperature = DataModel.MaxT[todayIndex] + DataModel.MinT[todayIndex] / 2.0;

                Rain = DataModel.Rain[todayIndex];
                if (todayIndex > 0)
                {
                    YesterdaysRain = DataModel.Rain[todayIndex - 1];
                }
                else
                {
                    YesterdaysRain = 0;
                }
                MaxTemp = DataModel.MaxT[todayIndex];
                MinTemp = DataModel.MinT[todayIndex];

                PanEvap = DataModel.PanEvap[todayIndex];
                SolarRadiation = DataModel.Radiation[todayIndex];
            }
            catch (Exception e)
            {
                Sim.ControlError = "ClimateControllerSimulate";
                throw new Exception(e.Message);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public double SumRain(int n, int delay)
        {
            double sumrain = 0;
            int index;
            for (int i = 0; i < n; ++i)
            {
                index = todayIndex - i - delay;
                if (index >= 0)
                {
                    sumrain += DataModel.Rain[index];
                }
            }
            return sumrain;
        }
    }
}
