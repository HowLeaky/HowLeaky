using HowLeaky.CustomAttributes;
using HowLeaky.DataModels;
using HowLeaky.Models;
using System;


namespace HowLeaky.ModelControllers
{
    public class ClimateController : HLObject
    {
        public ClimateDataModel dataModel { get; set; }

        public double Latitude { get { return dataModel.Latitude; } }
        public double Longitude { get { return dataModel.Longitude; } }

        public ClimateController(Simulation sim) : base(sim) { }

        public override void Initialise()
        {
            throw new NotImplementedException();
        }

        public override void Simulate()
        {
            throw new NotImplementedException();
        }
    }
}
