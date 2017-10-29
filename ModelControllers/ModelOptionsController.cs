using HowLeaky.Tools;
using HowLeaky.Tools.DataObjects;
using HowLeaky.Models;
using System;


namespace HowLeaky.ModelControllers
{
    public class ModelOptionsController : HLObject
    {
        public bool in_ResetResidueAtDate { get; set; }                    
        public DayMonthData in_ResetDateForResidue { get; set; }
        public double in_ResetValueForResidue_kg_per_ha { get; set; }
        public bool in_ResetSoilWaterAtDate { get; set; }
        public DayMonthData in_ResetDateForSoilWater { get; set; }
        public double in_ResetValueForSWAtDate_pc { get; set; }
        public bool in_ResetSoilWaterAtPlanting { get; set; }
        public double in_ResetValueForSWAtPlanting_pc { get; set; }
        public bool in_CanCalculateLateralFlow { get; set; }
        public bool in_IgnoreCropKill { get; set; }
        public bool in_UsePERFECTDryMatterFn { get; set; }
        public bool in_UsePERFECTGroundCovFn { get; set; }
        public bool in_UsePERFECTSoilEvapFn { get; set; }
        public bool in_UsePERFECTLeafAreaFn { get; set; }
        public bool in_UsePERFECTResidueFn { get; set; }
        public bool in_UsePERFECT_USLE_LS_Fn { get; set; }
        public int in_UsePERFECTCurveNoFn { get; set; }
        public double in_InitialPAW { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ModelOptionsController() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sim"></param>
        public ModelOptionsController(Simulation sim) : base(sim) { }
        /// <summary>
        /// 
        /// </summary>
        public override void Initialise()
        {
            throw new NotImplementedException();
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
                if (in_ResetSoilWaterAtDate)
                {
                    if (in_ResetDateForSoilWater.MatchesDate(today))
                    {
                        for (int i = 0; i < sim.in_LayerCount; ++i)
                            sim.SoilWater_rel_wp[i] = (in_ResetValueForSWAtDate_pc / 100.0) * sim.DrainUpperLimit_rel_wp[i];
                        sim.CalculateInitialValuesOfCumulativeSoilEvaporation();
                    }
                }
                if (in_ResetResidueAtDate)
                {
                    if (in_ResetDateForResidue.MatchesDate(today))
                    {
                        sim.total_crop_residue = in_ResetValueForResidue_kg_per_ha;
                        sim.VegetationController.ResetCropResidue(in_ResetValueForResidue_kg_per_ha);
                    }
                }
            }
            catch (Exception e)
            {
                sim.ControlError = "ApplyResetsIfAny";
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool UsePerfectCurveNoFn()
        {
            return ((in_UsePERFECTCurveNoFn == Simulation.PERFECT_CN || in_UsePERFECTCurveNoFn == Simulation.DEFAULT_CN) && !sim.Force2011CurveNoFn);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanCalculateLateralFlow()
        {
            return in_CanCalculateLateralFlow;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetInitialPAW()
        {
            return in_InitialPAW;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool UsePerfectUSLELSFn()
        {
            return in_UsePERFECT_USLE_LS_Fn;
        }

    }
}
