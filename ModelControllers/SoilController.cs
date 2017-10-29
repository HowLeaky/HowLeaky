using HowLeaky.CustomAttributes;
using HowLeaky.Models;
using System;


namespace HowLeaky.ModelControllers
{
    public class SoilController : HLObject
    {
        public SoilController() { }

        public SoilController(Simulation sim):base(sim) { }

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
