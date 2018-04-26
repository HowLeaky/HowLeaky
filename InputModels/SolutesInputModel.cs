using HowLeaky.CustomAttributes;
using System.Collections.Generic;


namespace HowLeaky.DataModels
{
    public class SolutesInputModel : InputModel
    {
        //Input Parameters
        [Input("StartConcOption")]
        public int StartConcOption { get; set; }

        [Input("Layer1InitialConc", "mg/kg")]
        public double Layer1InitialConc { get; set; }

        [Input("Layer2InitialConc", "mg/kg")]
        public double Layer2InitialConc { get; set; }

        [Input("Layer3InitialConc", "mg/kg")]
        public double Layer3InitialConc { get; set; }

        [Input("Layer4InitialConc", "mg/kg")]
        public double Layer4InitialConc { get; set; }

        [Input("Layer5InitialConc", "mg/kg")]
        public double Layer5InitialConc { get; set; }

        [Input("DefaultInitialConc", "mg/kg")]
        public double DefaultInitialConc { get; set; }

        [Input("RainfallConcentration", "mg/L")]
        public double RainfallConcentration { get; set; }

        [Input("IrrigationConcentration", "mg/L")]
        public double IrrigationConcentration { get; set; }

        [Input("MixingCoefficient")]
        public double MixingCoefficient { get; set; }                   
    }
}

