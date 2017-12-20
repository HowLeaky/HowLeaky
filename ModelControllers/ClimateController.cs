using HowLeaky.DataModels;
using System;
using System.Collections.Generic;

namespace HowLeaky.ModelControllers
{
    public class ClimateController : HLController
    {
        public ClimateInputModel DataModel { get; set; }

        public double Latitude { get { return DataModel.Latitude; } }
        public double Longitude { get { return DataModel.Longitude; } }
        public double Temperature { get; set; }
        public double Rain { get; set; }
        public double MaxT { get; set; }
        public double MinT { get; set; }
        public double PanEvap { get; set; }
        public double Radiation { get; set; }

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
                MaxT = DataModel.MaxT[todayIndex];
                MinT = DataModel.MinT[todayIndex];

                PanEvap = DataModel.PanEvap[todayIndex];
                Radiation = DataModel.Radiation[todayIndex];
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
