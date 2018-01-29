using System;
using HowLeaky.InputModels;
using System.Collections.Generic;
using HowLeaky.DataModels;

namespace HowLeaky.ModelControllers
{
    public class ModelOptionsController : HLController
    {
        public ModelOptionsInputModel DataModel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ModelOptionsController(Simulation sim) : base(sim)
        {
            DataModel = new ModelOptionsInputModel();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sim"></param>
        public ModelOptionsController(Simulation sim, List<InputModel> inputModels) : this(sim)
        {
            DataModel = (ModelOptionsInputModel)inputModels[0];
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Initialise()
        {
            //Do nothing
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Simulate()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="today"></param>
        public void ApplyResetsIfAny(DateTime today)
        {
            try
            {
                if (DataModel.ResetSoilWaterAtDate)
                {
                    if (DataModel.ResetDateForSoilWater.MatchesDate(today))
                    {
                        for (int i = 0; i < Sim.SoilController.LayerCount; ++i)
                        {
                            Sim.SoilController.SoilWaterRelWP[i] = (DataModel.ResetValueForSWAtDate / 100.0) * Sim.SoilController.DrainUpperLimitRelWP[i];
                        }
                        Sim.SoilController.CalculateInitialValuesOfCumulativeSoilEvaporation();
                    }
                }
                if (DataModel.ResetResidueAtDate)
                {
                    if (DataModel.ResetDateForResidue.MatchesDate(today))
                    {
                        Sim.SoilController.TotalCropResidue = DataModel.ResetValueForResidue;
                        Sim.VegetationController.ResetCropResidue(DataModel.ResetValueForResidue);
                    }
                }
            }
            catch (Exception e)
            {
                Sim.ControlError = "ApplyResetsIfAny";
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool UsePerfectCurveNoFn()
        {
            return ((DataModel.UsePERFECTCurveNoFn == Simulation.PERFECT_CN || DataModel.UsePERFECTCurveNoFn == Simulation.DEFAULT_CN) && !Sim.Force2011CurveNoFn);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanCalculateLateralFlow()
        {
            return DataModel.CanCalculateLateralFlow;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetInitialPAW()
        {
            return DataModel.InitialPAW;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool UsePerfectUSLELSFn()
        {
            return DataModel.UsePERFECTUSLELSFn;
        }
    }
}
