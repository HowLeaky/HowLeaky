using HowLeaky.CustomAttributes;
using System.Collections.Generic;


namespace HowLeaky.DataModels
{
    public class SolutesInputModel : InputModel
    {
        //Input Parameters
        public int StartConcOption { get; set; }
        [Unit("mg_per_kg")]
        public double Layer1InitialConc { get; set; }
        [Unit("mg_per_kg")]
        public double Layer2InitialConc { get; set; }
        [Unit("mg_per_kg")]
        public double Layer3InitialConc { get; set; }
        [Unit("mg_per_kg")]
        public double Layer4InitialConc { get; set; }
        [Unit("mg_per_kg")]
        public double Layer5InitialConc { get; set; }
        [Unit("mg_per_kg")]
        public double DefaultInitialConc { get; set; }
        [Unit("mg_per_L")]
        public double RainfallConcentration { get; set; }
        [Unit("mg_per_L")]
        public double IrrigationConcentration { get; set; }
        public double MixingCoefficient { get; set; }                   
    }
}

