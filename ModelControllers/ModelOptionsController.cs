using System;
using HowLeaky.InputModels;
using System.Collections.Generic;
using HowLeaky.DataModels;

namespace HowLeaky.ModelControllers
{
    public class ModelOptionsController : HLController
    {
        public ModelOptionsInputModel InputModel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ModelOptionsController(Simulation sim) : base(sim)
        {
            InputModel = new ModelOptionsInputModel();

            InitOutputModel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sim"></param>
        public ModelOptionsController(Simulation sim, List<InputModel> inputModels) : this(sim)
        {
            InputModel = (ModelOptionsInputModel)inputModels[0];
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
                if (InputModel.ResetSoilWaterAtDate)
                {
                    if (InputModel.ResetDateForSoilWater.MatchesDate(today))
                    {
                        for (int i = 0; i < Sim.SoilController.LayerCount; ++i)
                        {
                            Sim.SoilController.SoilWaterRelWP[i] = (InputModel.ResetValueForSWAtDate / 100.0) * Sim.SoilController.DrainUpperLimitRelWP[i];
                        }
                        Sim.SoilController.CalculateInitialValuesOfCumulativeSoilEvaporation();
                    }
                }
                if (InputModel.ResetResidueAtDate)
                {
                    if (InputModel.ResetDateForResidue.MatchesDate(today))
                    {
                        Sim.SoilController.TotalCropResidue = InputModel.ResetValueForResidue;
                        Sim.VegetationController.ResetCropResidue(InputModel.ResetValueForResidue);
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
            return ((InputModel.UsePERFECTCurveNoFn == Simulation.PERFECT_CN || InputModel.UsePERFECTCurveNoFn == Simulation.DEFAULT_CN) && !Sim.Force2011CurveNoFn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanCalculateLateralFlow()
        {
            return InputModel.CanCalculateLateralFlow;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetInitialPAW()
        {
            return InputModel.InitialPAW;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool UsePerfectUSLELSFn()
        {
            return InputModel.UsePERFECTUSLELSFn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override InputModel GetInputModel()
        {
            return InputModel;
        }
    }
}
