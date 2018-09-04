using HowLeaky.CustomAttributes;
using HowLeaky.DataModels;
using System;
using System.Collections.Generic;

namespace HowLeaky.ModelControllers
{
    public class ClimateController : HLController
    {
        public ClimateInputModel InputModel { get; set; }

        [Output]
        public double Latitude { get { return InputModel.Latitude; } }
        [Output]
        public double Longitude { get { return InputModel.Longitude; } }
        [Output]
        public double Temperature { get; set; }
        [Output("Daily rainfall amount as read directly from the P51 file", "mm", 1, AggregationTypeEnum.Sum) ]
        public double Rain { get; set; }  
        [Output("Daily max temperature as read directly from the P51 file.", "oC")]
        public double MaxTemp { get; set; } 
        [Output("Daily min temperature as read directly from the P51 file.", "oC")]
        public double MinTemp { get; set; }
        [Output("Daily pan evaporation as read directly from the P51 file.", "mm", 1, AggregationTypeEnum.Sum)]
        public double PanEvap { get; set; }
        [Output(" Daily solar radition as read directly from the P51 file.", "MJ/m2/day")]
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
            InputModel = (ClimateInputModel)inputModels[0];
            InitOutputModel();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public double RainOnDay(DateTime day)
        {
            int index = (day - InputModel.StartDate.Value).Days;

            if (index > 0 && index <= InputModel.Rain.Count - 1)
            {
                return InputModel.Rain[index];
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
                todayIndex = (Sim.Today - InputModel.StartDate.Value).Days;
                Temperature = (InputModel.MaxT[todayIndex] + InputModel.MinT[todayIndex]) / 2.0;

                Rain = InputModel.Rain[todayIndex] * InputModel.RainfallMultiplier;
                if (todayIndex > 0)
                {
                    YesterdaysRain = InputModel.Rain[todayIndex - 1];
                }
                else
                {
                    YesterdaysRain = 0;
                }
                MaxTemp = InputModel.MaxT[todayIndex];
                MinTemp = InputModel.MinT[todayIndex];

                PanEvap = InputModel.PanEvap[todayIndex] * InputModel.PanEvapMultiplier;
                SolarRadiation = InputModel.Radiation[todayIndex];
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
        /// <returns></returns>
        public override InputModel GetInputModel()
        {
            return InputModel;
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
                    sumrain += InputModel.Rain[index];
                }
            }
            return sumrain;
        }
    }
}
