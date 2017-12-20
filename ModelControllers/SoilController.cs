using HowLeaky.CustomAttributes;
using HowLeaky.DataModels;
using HowLeaky.OutputModels;
using HowLeaky.Tools.Helpers;
using System;
using System.Collections.Generic;
using HowLeaky.Tools.ListExtensions;
using HowLeaky.Interfaces;

namespace HowLeaky.ModelControllers
{
    public class SoilWaterBalanceOutputModel : OutputDataModel, IDailyOutput
    {
        //Water balance outputs
        [Unit("mm")]
        public double Irrigation { get; set; }                // Irrigation amount (mm) as calcaulted in irrigation module.
        [Unit("mm")]
        public double Runoff { get; set; }                    // Total Runoff amount (mm) - includes runoff from rainfall AND irrigation.
        [Unit("mm")]
        public double RunoffFromIrrigation { get; set; }      // Runoff amount from irrigation (mm).
        [Unit("mm")]
        public double RunoffFromRainfall { get; set; }        // Runoff amount from rainfall (mm).
        [Unit("mm")]
        public double SoilEvap { get; set; }                  // Soil evaporation (mm).
        [Unit("mm")]
        public double PotSoilEvap { get; set; }               // Potential soil evaporation (mm).
        [Unit("mm")]
        public double Transpiration { get; set; }             // Transpiration (mm) calculated from current crop.
        [Unit("mm")]
        public double EvapoTransp { get; set; }               // Evapo-transpiration (mm) is equal to the transpiration PLUS soil evaporation.
        [Unit("mm")]
        public double DeepDrainage { get; set; }              // Deep drainage (mm) which is the amount of drainge out of the bottom layer.
        [Unit("mm")]
        public double Overflow { get; set; }                  // Overflow (mm) ADD DEF HERE
        [Unit("mm")]
        public double LateralFlow { get; set; }               // Lateral flow (mm) ADD DEF HERE
        [Unit("mm")]
        public double VBE { get; set; }                       // Volume Balance Error
        [Unit("")]
        public double RunoffCurveNo { get; set; }                // Runoff curve number
        [Unit("")]
        public double RunoffRetentionNo { get; set; }            // Runoff retention number
    }
    public class SoilOutputModel : OutputDataModel, IDailyOutput
    {
        [Unit("t_per_ha")]                                                                     //Soil outputs
        public double HillSlopeErosion { get; set; }            // Hillslope errorsion (t/ha)
        [Unit("t_per_ha")]
        public double OffSiteSedDelivery { get; set; }          // Offsite sediment deliver (t/ha)
        [Unit("mm")]
        public double TotalSoilWater { get; set; }              // Total soil water (mm) is the sum of soil water in all layers
        [Unit("mm")]
        public double SoilWaterDeficit { get; set; }            // Soil water deficit (mm)
        public double Layer1SatIndex { get; set; }              // Layer 1 saturation index
        [Unit("kg_per_ha")]
        public double TotalCropResidue { get; set; }            // Total crop residue (kg/ha) - sum of all crops present
        [Unit("pc")]
        public double TotalResidueCover { get; set; }           // Total residue cover (%)  - based on all crops present
        [Unit("pc")]
        public double TotalCover { get; set; }                  // Total cover (%) - based on all crops present{get;set;}
        [Unit("mm")]
        public List<double> SoilWater { get; set; }             // Soil water in each layer (mm)
        [Unit("mm")]
        public List<double> Drainage { get; set; }              // Drainage in each layer

        public SoilOutputModel(int layerCount)
        {
            SoilWater = new List<double>(layerCount).Fill(0);
            Drainage = new List<double>(layerCount).Fill(0);
        }
    }

    public class SoilSummaryOutputModel : OutputDataModel
    {
        public double YrlyAvgRainfall_mm_per_yr { get; set; }           //
        public double YrlyAvgIrrigation_mm_per_yr { get; set; }         //
        public double YrlyAvgRunoff_mm_per_yr { get; set; }             //
        public double YrlyAvgSoilEvaporation_mm_per_yr { get; set; }    //
        public double YrlyAvgTranspiration_mm_per_yr { get; set; }      //
        public double YrlyAvgEvapotransp_mm_per_yr { get; set; }         //
        public double YrlyAvgOverflow_mm_per_yr { get; set; }           //
        public double YrlyAvgDrainage_mm_per_yr { get; set; }           //
        public double YrlyAvgLateralFlow_mm_per_yr { get; set; }        //
        public double YrlyAvgSoilErosion_T_per_ha_per_yr { get; set; }  //
        public double YrlyAvgOffsiteSedDel_T_per_ha_per_yr { get; set; }//
        public double TotalCropsPlanted { get; set; }                   //
        public double TotalCropsHarvested { get; set; }                 //
        public double TotalCropsKilled { get; set; }                    //
        public double AvgYieldPerHrvst_t_per_ha_per_hrvst { get; set; } //
        public double AvgYieldPerPlant_t_per_ha_per_plant { get; set; } //
        public double AvgYieldPerYr_t_per_ha_per_yr { get; set; }       //
        public double YrlyAvgCropRainfall_mm_per_yr { get; set; }       //
        public double YrlyAvgCropIrrigation_mm_per_yr { get; set; }     //
        public double YrlyAvgCropRunoff_mm_per_yr { get; set; }         //
        public double YrlyAvgCropSoilEvap_mm_per_yr { get; set; }        //
        public double YrlyAvgCropTransp_mm_per_yr { get; set; }          //
        public double YrlyAvgCropEvapotransp_mm_per_yr { get; set; }     //
        public double YrlyAvgCropOverflow_mm_per_yr { get; set; }       //
        public double YrlyAvgCropDrainage_mm_per_yr { get; set; }       //
        public double YrlyAvgCropLateralFlow_mm_per_yr { get; set; }    //
        public double YrlyAvgCropSoilErosion_T_per_ha_per_yr { get; set; }//
        public double YrlyAvgCropOffsiteSedDel_T_per_ha_per_yr { get; set; }//
        public double YrlyAvgFallowRainfall_mm_per_yr { get; set; }     //
        public double YrlyAvgFallowIrrigation_mm_per_yr { get; set; }   //
        public double YrlyAvgFallowRunoff_mm_per_yr { get; set; }       //
        public double YrlyAvgFallowSoilEvap_mm_per_yr { get; set; }      //
        public double YrlyAvgFallowTransp_mm_per_yr { get; set; }        //
        public double YrlyAvgFallowEvapotransp_mm_per_yr { get; set; }  //
        public double YrlyAvgFallowOverflow_mm_per_yr { get; set; }     //
        public double YrlyAvgFallowDrainage_mm_per_yr { get; set; }     //
        public double YrlyAvgFallowLateralFlow_mm_per_yr { get; set; }  //
        public double YrlyAvgFallowSoilErosion_T_per_ha_per_yr { get; set; }//
        public double YrlyAvgFallowOffsiteSedDel_T_per_ha_per_yr { get; set; }//
        public double YrlyAvgPotEvap_mm { get; set; }                   //
        public double YrlyAvgRunoffAsPercentOfInflow_pc { get; set; }   //
        public double YrlyAvgEvapAsPercentOfInflow_pc { get; set; }     //
        public double YrlyAvgTranspAsPercentOfInflow_pc { get; set; }   //
        public double YrlyAvgDrainageAsPercentOfInflow_pc { get; set; } //
        public double YrlyAvgPotEvapAsPercentOfInflow_pc { get; set; }  //
        public double YrlyAvgCropSedDel_t_per_ha_per_yr { get; set; }   //
        public double YrlyAvgFallowSedDel_t_per_ha_per_yr { get; set; } //
        public double RobinsonErrosionIndex { get; set; }                //
        public double YrlyAvgCover_pc { get; set; }                     //
        public double YrlyAvgFallowDaysWithMore50pcCov_days { get; set; }//
        public double AvgCoverBeforePlanting_pc { get; set; }           //
        public double SedimentEMCBeoreDR { get; set; }                  //
        public double SedimentEMCAfterDR { get; set; }                  //
        public double AvgSedConcInRunoff { get; set; }                   //
    }

    public class SoilMonthlyOutputModel : OutputDataModel
    {
        [Unit("mm")]
        public List<double> MthlyAvgRainfall { get; set; }                 //
        [Unit("mm")]
        public List<double> MthlyAvgEvaporation { get; set; }              //
        [Unit("mm")]
        public List<double> MthlyAvgTranspiration { get; set; }            //
        [Unit("mm")]
        public List<double> MthlyAvgRunoff { get; set; }                   //
        [Unit("mm")]
        public List<double> MthlyAvgDrainage { get; set; }                 //

        public SoilMonthlyOutputModel()
        {
            MthlyAvgRainfall = new List<double>(12).Fill(0);
            MthlyAvgEvaporation = new List<double>(12).Fill(0);
            MthlyAvgTranspiration = new List<double>(12).Fill(0);
            MthlyAvgRunoff = new List<double>(12).Fill(0);
            MthlyAvgDrainage = new List<double>(12).Fill(0);
        }
    }


    public class SoilController : HLController
    {
        public SoilInputModel DataModel { get; set; }

        public SoilWaterBalanceOutputModel WatBal { get; set; } = new SoilWaterBalanceOutputModel();
        public SoilOutputModel Soil { get; set; }  = new SoilOutputModel(0);
        public SoilMonthlyOutputModel MO { get; set; } = new SoilMonthlyOutputModel();
        public SoilSummaryOutputModel SO { get; set; } = new SoilSummaryOutputModel();

        //--------------------------------------------------------------------------
        // intermediate variables
        //--------------------------------------------------------------------------
        //public bool InRunoff { get; set; }
        public bool InRunoff2 { get; set; }
        public int RunoffEventCount2 { get; set; }
        public double PreviousTotalSoilWater { get; set; }
        public double EffectiveRain { get; set; }
        public double TotalSoilWater { get; set; }
        public double TotalCover { get; set; }
        public double TotalCoverPercent { get; set; }
        public double TotalCropResidue { get; set; }
        public double TotalResidueCover { get; set; }
        public double TotalResidueCoverPercent { get; set; }
        public double CropCover { get; set; }
        public double AccumulatedCover { get; set; }
        public double Runoff { get; set; }
        public double SedimentConc { get; set; }
        public double ErosionTPerHa { get; set; }
        public double OffsiteSedDelivery { get; set; }
        public double CumSedConc { get; set; }
        public double PeakSedConc { get; set; }
        public double swd { get; set; }
        public double satd { get; set; }
        public double sse1 { get; set; }
        public double sse2 { get; set; }
        public double se1 { get; set; }
        public double se2 { get; set; }
        public double se21 { get; set; }
        public double se22 { get; set; }
        public double dsr { get; set; }
        public double sedCatchmod { get; set; }
        public double saturationindex { get; set; }
        public double cn2 { get; set; }
        public double overflowmm { get; set; }
        public double rainSinceTillage { get; set; }
        public double infiltration { get; set; }
        public double lateralFlow { get; set; }
        public double potentialSoilEvaporation { get; set; }
        public double drainage { get; set; }
        public double runoffRetentionNumber { get; set; }
        public double usleLsFactor { get; set; }
        public double PredRh { get; set; }

        public double sum_rainfall { get; set; }
        public double sum_irrigation { get; set; }
        public double sum_runoff { get; set; }
        public double sum_potevap { get; set; }
        public double sum_soilevaporation { get; set; }
        public double sum_transpiration { get; set; }
        public double sum_evapotranspiration { get; set; }
        public double sum_overflow { get; set; }
        public double sum_drainage { get; set; }
        public double sum_lateralflow { get; set; }
        public double sum_soilerosion { get; set; }
        public double sum_crop_rainfall { get; set; }
        public double sum_crop_irrigation { get; set; }
        public double sum_crop_runoff { get; set; }
        public double sum_crop_soilevaporation { get; set; }
        public double sum_crop_transpiration { get; set; }
        public double sum_crop_evapotranspiration { get; set; }
        public double sum_crop_overflow { get; set; }
        public double sum_crop_drainage { get; set; }
        public double sum_crop_lateralflow { get; set; }
        public double sum_crop_soilerosion { get; set; }
        public double sum_fallow_rainfall { get; set; }
        public double sum_fallow_irrigation { get; set; }
        public double sum_fallow_overflow { get; set; }
        public double sum_fallow_lateralflow { get; set; }
        public double sum_fallow_runoff { get; set; }
        public double sum_fallow_soilevaporation { get; set; }
        public double sum_fallow_drainage { get; set; }
        public double sum_fallow_soilerosion { get; set; }
        public double fallow_efficiency { get; set; }
        public double sum_fallow_soilwater { get; set; }
        public double AccumulateCovDayBeforePlanting { get; set; }
        public double fallow_days_with_more_50pc_cov { get; set; }
        public double TotalNumberPlantings { get; set; }
        public double accumulated_crop_sed_deliv { get; set; }
        public double accumulated_fallow_sed_deliv { get; set; }

        public List<double> MCFC { get; set; }
        public List<double> SoilWaterRelWP { get; set; }
        public List<double> DrainUpperLimitRelWP { get; set; }
        public List<double> Depth { get; set; }
        public List<double> LayerTranspiration { get; set; }
        public List<double> Red { get; set; }
        public List<double> WF { get; set; }
        public List<double> SaturationLimitRelWP { get; set; }
        [Unit("mm")]
        public List<double> WiltingPointRelOD { get; set; }
        [Unit("mm")]
        public List<double> DULRelOD { get; set; }
        public List<double> AirDryLimitRelWP { get; set; }
        public List<double> KSat { get; set; }
        public List<double> SWCon { get; set; }
        public List<double> Seepage { get; set; }
        public List<double> MaxDrainage { get; set; }

        public int LayerCount { get { return DataModel.HorizonCount; } }
        /// <summary>
        /// 
        /// </summary>
        public SoilController(Simulation sim) : base(sim)
        {
            WatBal = new SoilWaterBalanceOutputModel();
            Soil = new SoilOutputModel(0);
            MO = new SoilMonthlyOutputModel();
            SO = new SoilSummaryOutputModel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sim"></param>
        /// <param name="inputModels"></param>
        public SoilController(Simulation sim, List<InputModel> inputModels) : this(sim)
        {
            DataModel = (SoilInputModel)inputModels[0];
            Soil = new SoilOutputModel(LayerCount);
        }

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
        public override void SetStartOfDayParameters()
        {
            try
            {
                EffectiveRain = Sim.ClimateController.Rain;
                swd = 0;
                satd = 0;
                for (int i = 0; i < LayerCount; ++i)
                {
                    satd = satd + (SaturationLimitRelWP[i] - SoilWaterRelWP[i]);
                    swd = swd + (DrainUpperLimitRelWP[i] - SoilWaterRelWP[i]);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateFallowWaterBalance()
        {
            try
            {
                if (Sim.VegetationController.InFallow())
                {
                    sum_fallow_rainfall += Sim.ClimateController.Rain;
                    sum_fallow_runoff += WatBal.Runoff;
                    sum_fallow_soilevaporation += WatBal.SoilEvap;
                    sum_fallow_drainage += WatBal.DeepDrainage;
                    sum_fallow_soilerosion += ErosionTPerHa;

                    if (TotalCover > 0.5)
                    {
                        ++fallow_days_with_more_50pc_cov;
                    }
                }
                else if (Sim.VegetationController.IsPlanting())
                    sum_fallow_soilwater += Sim.VegetationController.CalcFallowSoilWater();
            }
            catch (Exception e)
            {
                Sim.ControlError = "UpdateFallowWaterBalance";
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculateResidue()
        {
            try
            {
                Sim.VegetationController.CalculateResidue();
                //we already estimated these in the Runoff function- but will recalculate here.
                TotalCropResidue = Sim.VegetationController.GetTotalCropResidue();
                TotalResidueCover = Sim.VegetationController.GetTotalResidueCover();
                TotalResidueCoverPercent = Sim.VegetationController.GetTotalResidueCoverPercent();
                TotalCover = Sim.VegetationController.GetTotalCover();
                CropCover = Sim.VegetationController.GetCropCover();
                TotalCoverPercent = TotalCover * 100.0;
                AccumulatedCover += TotalCoverPercent;
            }
            catch (Exception e)
            {
                Sim.ControlError = "CalculateResidue";
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculateMonthlyOutputs()
        {
            double simyears = Sim.NumberOfDaysInSimulation / 365.0;
            for (int i = 0; i < 12; ++i)
            {
                MO.MthlyAvgRainfall[i] = MathTools.Divide(MO.MthlyAvgRainfall[i], simyears);
                MO.MthlyAvgEvaporation[i] = MathTools.Divide(MO.MthlyAvgEvaporation[i], simyears);
                MO.MthlyAvgTranspiration[i] = MathTools.Divide(MO.MthlyAvgTranspiration[i], simyears);
                MO.MthlyAvgRunoff[i] = MathTools.Divide(MO.MthlyAvgRunoff[i], simyears);
                MO.MthlyAvgDrainage[i] = MathTools.Divide(MO.MthlyAvgDrainage[i], simyears);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateMonthlyStatistics()
        {
            try
            {
                int monthindex = Sim.Month - 1;
                MO.MthlyAvgRainfall[monthindex] += Sim.ClimateController.Rain;
                MO.MthlyAvgEvaporation[monthindex] += WatBal.SoilEvap;
                MO.MthlyAvgTranspiration[monthindex] += Sim.VegetationController.GetTotalTranspiration();
                MO.MthlyAvgRunoff[monthindex] += WatBal.Runoff;
                MO.MthlyAvgDrainage[monthindex] += WatBal.DeepDrainage;
            }
            catch (Exception e)
            {
                Sim.ControlError = "UpdateMonthlyStatistics";
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculateSummaryOutputs()
        {
            double simyears = Sim.NumberOfDaysInSimulation / 365.0;

            SO.YrlyAvgRainfall_mm_per_yr = MathTools.Divide(sum_rainfall, simyears);
            SO.YrlyAvgIrrigation_mm_per_yr = MathTools.Divide(sum_irrigation, simyears);
            SO.YrlyAvgRunoff_mm_per_yr = MathTools.Divide(sum_runoff, simyears);
            SO.YrlyAvgSoilEvaporation_mm_per_yr = MathTools.Divide(sum_soilevaporation, simyears);
            SO.YrlyAvgTranspiration_mm_per_yr = MathTools.Divide(sum_transpiration, simyears);
            SO.YrlyAvgEvapotransp_mm_per_yr = MathTools.Divide(sum_evapotranspiration, simyears);
            SO.YrlyAvgOverflow_mm_per_yr = MathTools.Divide(sum_overflow, simyears);
            SO.YrlyAvgDrainage_mm_per_yr = MathTools.Divide(sum_drainage, simyears);
            SO.YrlyAvgLateralFlow_mm_per_yr = MathTools.Divide(sum_lateralflow, simyears);
            SO.YrlyAvgSoilErosion_T_per_ha_per_yr = MathTools.Divide(sum_soilerosion, simyears);
            SO.YrlyAvgOffsiteSedDel_T_per_ha_per_yr = MathTools.Divide(sum_soilerosion * DataModel.SedDelivRatio, simyears);
            SO.TotalCropsPlanted = Sim.VegetationController.GetTotalCropsPlanted();
            SO.TotalCropsHarvested = Sim.VegetationController.GetTotalCropsHarvested();
            SO.TotalCropsKilled = Sim.VegetationController.GetTotalCropsKilled();
            SO.AvgYieldPerHrvst_t_per_ha_per_hrvst = Sim.VegetationController.GetAvgYieldPerHarvest();
            SO.AvgYieldPerPlant_t_per_ha_per_plant = Sim.VegetationController.GetAvgYieldPerPlanting();
            SO.AvgYieldPerYr_t_per_ha_per_yr = Sim.VegetationController.GetAvgYieldPerYear();
            SO.YrlyAvgCropRainfall_mm_per_yr = MathTools.Divide(sum_crop_rainfall, simyears);
            SO.YrlyAvgCropIrrigation_mm_per_yr = MathTools.Divide(sum_crop_irrigation, simyears);
            SO.YrlyAvgCropRunoff_mm_per_yr = MathTools.Divide(sum_crop_runoff, simyears);
            SO.YrlyAvgCropSoilEvap_mm_per_yr = MathTools.Divide(sum_crop_soilevaporation, simyears);
            SO.YrlyAvgCropTransp_mm_per_yr = MathTools.Divide(sum_crop_transpiration, simyears);
            SO.YrlyAvgCropEvapotransp_mm_per_yr = MathTools.Divide(sum_crop_evapotranspiration, simyears);
            SO.YrlyAvgCropOverflow_mm_per_yr = MathTools.Divide(sum_crop_overflow, simyears);
            SO.YrlyAvgCropDrainage_mm_per_yr = MathTools.Divide(sum_crop_drainage, simyears);
            SO.YrlyAvgCropLateralFlow_mm_per_yr = MathTools.Divide(sum_crop_lateralflow, simyears);
            SO.YrlyAvgCropSoilErosion_T_per_ha_per_yr = MathTools.Divide(sum_crop_soilerosion, simyears);
            SO.YrlyAvgCropOffsiteSedDel_T_per_ha_per_yr = MathTools.Divide(sum_crop_soilerosion * DataModel.SedDelivRatio, simyears);
            SO.YrlyAvgFallowRainfall_mm_per_yr = MathTools.Divide(sum_fallow_rainfall, simyears);
            SO.YrlyAvgFallowIrrigation_mm_per_yr = MathTools.Divide(sum_fallow_irrigation, simyears);
            SO.YrlyAvgFallowRunoff_mm_per_yr = MathTools.Divide(sum_fallow_runoff, simyears);
            SO.YrlyAvgFallowSoilEvap_mm_per_yr = MathTools.Divide(sum_fallow_soilevaporation, simyears);
            SO.YrlyAvgFallowOverflow_mm_per_yr = MathTools.Divide(sum_fallow_overflow, simyears);
            SO.YrlyAvgFallowDrainage_mm_per_yr = MathTools.Divide(sum_fallow_drainage, simyears);
            SO.YrlyAvgFallowLateralFlow_mm_per_yr = MathTools.Divide(sum_fallow_lateralflow, simyears);
            SO.YrlyAvgFallowSoilErosion_T_per_ha_per_yr = MathTools.Divide(sum_fallow_soilerosion, simyears);
            SO.YrlyAvgFallowOffsiteSedDel_T_per_ha_per_yr = MathTools.Divide(sum_fallow_soilerosion * DataModel.SedDelivRatio, simyears);
            fallow_efficiency = MathTools.Divide(sum_fallow_soilwater * 100.0, sum_fallow_rainfall);
            SO.YrlyAvgPotEvap_mm = MathTools.Divide(sum_potevap, simyears);
            SO.YrlyAvgRunoffAsPercentOfInflow_pc = MathTools.Divide(SO.YrlyAvgRunoff_mm_per_yr * 100.0, SO.YrlyAvgRainfall_mm_per_yr + SO.YrlyAvgIrrigation_mm_per_yr);
            SO.YrlyAvgEvapAsPercentOfInflow_pc = MathTools.Divide(SO.YrlyAvgSoilEvaporation_mm_per_yr * 100.0, SO.YrlyAvgRainfall_mm_per_yr + SO.YrlyAvgIrrigation_mm_per_yr);
            SO.YrlyAvgTranspAsPercentOfInflow_pc = MathTools.Divide(SO.YrlyAvgTranspiration_mm_per_yr * 100.0, SO.YrlyAvgRainfall_mm_per_yr + SO.YrlyAvgIrrigation_mm_per_yr);
            SO.YrlyAvgDrainageAsPercentOfInflow_pc = MathTools.Divide(SO.YrlyAvgDrainage_mm_per_yr * 100.0, SO.YrlyAvgRainfall_mm_per_yr + SO.YrlyAvgIrrigation_mm_per_yr);
            SO.YrlyAvgPotEvapAsPercentOfInflow_pc = MathTools.Divide(SO.YrlyAvgPotEvap_mm * 100.0, SO.YrlyAvgRainfall_mm_per_yr + SO.YrlyAvgIrrigation_mm_per_yr);
            SO.YrlyAvgCropSedDel_t_per_ha_per_yr = MathTools.Divide(accumulated_crop_sed_deliv, simyears);
            SO.YrlyAvgFallowSedDel_t_per_ha_per_yr = MathTools.Divide(accumulated_fallow_sed_deliv, simyears);
            SO.RobinsonErrosionIndex = MathTools.Divide(SO.YrlyAvgSoilErosion_T_per_ha_per_yr * SO.YrlyAvgCover_pc, DataModel.FieldSlope);
            SO.YrlyAvgCover_pc = MathTools.Divide(AccumulatedCover, Sim.NumberOfDaysInSimulation);
            SO.YrlyAvgFallowDaysWithMore50pcCov_days = MathTools.Divide(fallow_days_with_more_50pc_cov * 100.0, Sim.NumberOfDaysInSimulation);
            SO.AvgCoverBeforePlanting_pc = MathTools.Divide(AccumulateCovDayBeforePlanting * 100.0, TotalNumberPlantings);
            SO.SedimentEMCBeoreDR = MathTools.MISSING_DATA_VALUE;
            SO.SedimentEMCAfterDR = MathTools.MISSING_DATA_VALUE;
            SO.AvgSedConcInRunoff = MathTools.Divide(sum_soilerosion * DataModel.SedDelivRatio * 100.0, sum_runoff);//for g/L
        }

        /// <summary>
        /// 
        /// </summary>
        public void TryModelSoilCracking()
        {
            try
            {
                if (DataModel.SoilCrack.State)
                {
                    //************************************************************************
                    //*                                                                      *
                    //*  This function allows for water to directly enter lower layers     *
                    //*  of the soil profile through cracks. For cracks to occur the top     *
                    //*  and second profile layers must be less than 30% and 50%             *
                    //*  respectively of field capacity. Cracks can extend down the          *
                    //*  profile using similar criteria. This subroutine assumes all         *
                    //*  cracks must exist at the surface. Water is placed into the          *
                    //*  lowest accessable layer first.                                      *
                    //*                                                                      *
                    //************************************************************************
                    int nod;
                    //  Initialise total water redistributed through cracks
                    double tred = 0;
                    for (int i = 0; i < LayerCount; ++i)
                    {
                        Red[i] = 0;
                        if (!MathTools.DoublesAreEqual(DrainUpperLimitRelWP[i], 0))
                        {
                            MCFC[i] = SoilWaterRelWP[i] / DrainUpperLimitRelWP[i];
                        }
                        else
                        {
                            MCFC[i] = 0;

                            LogDivideByZeroError("ModelSoilCracking", "DrainUpperLimit_rel_wp[i]", "mcfc[i]");
                        }
                        if (MCFC[i] < 0)
                        {
                            MCFC[i] = 0;
                        }
                        else if (MCFC[i] > 1)
                        {
                            MCFC[i] = 1;
                        }
                    }

                    //  Don't continue if rainfall is less than 10mm
                    if (EffectiveRain < 10)
                    {
                        return;
                    }
                    //  Check if profile is dry enough for cracking to occur.
                    if (MCFC[0] >= 0.3 || MCFC[1] >= 0.3)
                    {
                        return;
                    }
                    //  Calculate number of depths to which cracks extend
                    nod = 1;
                    for (int i = 1; i < LayerCount; ++i)
                    {
                        if (MCFC[i] >= 0.3)
                        {
                            i = LayerCount;
                        }
                        else
                        {
                            ++nod;
                        }
                    }
                    //  Fill cracks from lowest cracked layer first to a maximum of 50% of
                    //  field capacity.
                    tred = Math.Min(DataModel.MaxInfiltIntoCracks, EffectiveRain);
                    for (int i = nod - 1; i >= 0; --i)
                    {
                        Red[i] = Math.Min(tred, DrainUpperLimitRelWP[i] / 2.0 - SoilWaterRelWP[i]);
                        tred -= Red[i];
                        if (tred <= 0)
                        {
                            i = -1;
                        }
                    }

                    //  calculate effective rainfall after infiltration into cracks.
                    //  Note that redistribution of water into layer 1 is ignored.
                    EffectiveRain = EffectiveRain + Red[0] - Math.Min(DataModel.MaxInfiltIntoCracks, EffectiveRain);
                    Red[0] = 0.0;

                    //  calculate total amount of water in cracks
                    for (int i = 0; i < LayerCount; ++i)
                    {
                        tred += Red[i];
                    }
                }
            }
            catch (Exception e)
            {
                Sim.ControlError = "ModelSoilCracking";
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculateRunoff()
        {
            int progress = 0;
            try
            {
                //  *********************************************************************
                //  *  This subroutine calculates surface runoff using a modified form  *
                //  *  of USDA Curve numbers from CREAMS.  The input value of Curve     *
                //  *  Number for AMC II is adjusted to account for the effects of crop *
                //  *  and residue cover.  The magnitude of the reduction in CNII due   *
                //  *  to cover is governed by the user defined CNRED parameter.        *
                //  *                                                                   *
                //  *  Knisel, W.G. editor. CREAMS: A field-scale model for chemical,   *
                //  *  runoff and erosion from agricultural management systems.         *
                //  *  United States Department of Agriculture, Conservation Research   *
                //  *  Report no. 26.                                                   *
                //  *********************************************************************
                double sumh20;
                infiltration = 0.0;
                WatBal.Runoff = 0.0;
                runoffRetentionNumber = 0;
                double cn1, smx;

                //  ***************************************************
                //  *  Calculate cover effect on curve number (cn2).  *
                //  ***************************************************}
                CropCover = Sim.VegetationController.GetCropCoverIfLAIModel(CropCover);  //LAI Model uses cover from the end of the previous day whereas Cover model predefines at the start of the day
                cn2 = DataModel.RunoffCurveNumber - DataModel.RedInCNAtFullCover /* Check this -- in_CurveNumberReduction*/ * Math.Min(1.0, CropCover + TotalResidueCover * (1 - CropCover));
                progress = 1;

                //this could need attention!!!! Danny Rattray
                //  *******************************************************
                //  *  Calculate roughness effect on curve number (cn2).  *
                //  *******************************************************

                rainSinceTillage += EffectiveRain;
                if (!MathTools.DoublesAreEqual(DataModel.RainToRemoveRough, 0))
                {
                    if (rainSinceTillage < DataModel.RainToRemoveRough)
                    {
                        cn2 += Sim.TillageController.RoughnessRatio * DataModel.MaxRedInCNDueToTill * (rainSinceTillage / DataModel.RainToRemoveRough - 1);
                    }
                }

                if (EffectiveRain < 0.1)
                {
                    if (Sim.IrrigationController == null)
                    {
                        WatBal.Runoff = 0;
                    }
                    else
                    {
                        WatBal.Runoff = Sim.IrrigationController.Output.IrrigationRunoff;
                    }
                    return;
                }
                progress = 2;
                if (Sim.ModelOptionsController.UsePerfectCurveNoFn())
                {
                    //  *******************************************************
                    //  *  Calculate smx (CREAMS p14, equations i-3 and i-4)  *
                    //  *******************************************************
                    cn1 = -16.91 + 1.348 * cn2 - 0.01379 * cn2 * cn2 + 0.0001177 * cn2 * cn2 * cn2;
                    if (!MathTools.DoublesAreEqual(cn1, 0))
                    {
                        smx = 254.0 * ((100.0 / cn1) - 1.0);
                    }
                    else
                    {
                        smx = 0;

                        LogDivideByZeroError("CalculateRunoff", "cn1", "smx");
                    }
                    progress = 3;
                    //  ***************************************
                    //  *  Calculate retention parameter,  runoff_retention_number  *
                    //  ***************************************
                    sumh20 = 0.0;
                    for (int i = 0; i < LayerCount - 1; ++i)
                    {
                        if (!MathTools.DoublesAreEqual(SaturationLimitRelWP[i], 0))
                            sumh20 += WF[i] * (Math.Max(SoilWaterRelWP[i], 0) / SaturationLimitRelWP[i]);
                        else
                        {
                            LogDivideByZeroError("CalculateRunoff", "SaturationLimit_rel_wp[i]", "sumh20");
                        }
                    }
                    runoffRetentionNumber = (int)(smx * (1.0 - sumh20));
                    //REMOVE INT STATEMENT AFTER VALIDATION
                    progress = 4;
                }
                else
                {
                    // ******************************************************************
                    // *  MODIFIED Calculate smx (CREAMS p14, equations i-3 and i-4)  	*
                    // *  Fix" for oversize Smx at low CN                     			*
                    // *  e.g. >254mm for cn2<70                              			*
                    // *  Brett Robinson May 2011                             			*
                    // ******************************************************************
                    double temp = 265.0 + (Math.Exp(0.17 * (cn2 - 50)) + 1);
                    if (!MathTools.DoublesAreEqual(temp, 0))
                    {
                        if (cn2 > 83) // linear above cn2=83
                        {
                            smx = 6 + (100 - cn2) * 6.66;
                        }
                        else            // logistic for cn2<=83
                        {
                            smx = 254.0 - (265.0 * Math.Exp(0.17 * (cn2 - 50))) / temp;
                        }
                    }
                    else
                    {
                        smx = 0;

                        LogDivideByZeroError("CalculateRunoff", "(265.0+(exp(cn2)+1)", "smx");
                    }
                    progress = 3;
                    //  ***************************************
                    //  *  Calculate retention parameter,  runoff_retention_number  *
                    //  ***************************************
                    sumh20 = 0.0;
                    // * CREAMS and other model discount S for water content (linear from air dry to sat) *
                    // * old code = relative to WP, new code = rel to air dry                             *
                    // * Changes by Brett Robinson May 2011                                               *
                    for (int i = 0; i < LayerCount - 1; ++i)
                    {
                        double deno = SaturationLimitRelWP[i] + AirDryLimitRelWP[i];
                        if (!MathTools.DoublesAreEqual(deno, 0))
                        {
                            sumh20 = sumh20 + WF[i] * (SoilWaterRelWP[i] + AirDryLimitRelWP[i]) / deno;
                        }
                        else
                        {
                            LogDivideByZeroError("CalculateRunoff", "SaturationLimit_rel_wp[i]+AirDryLimit_rel_wp[i]", "sumh20");
                        }
                    }
                    runoffRetentionNumber = (int)(smx * (1.0 - sumh20));
                    //REMOVE INT STATEMENT AFTER VALIDATION
                    progress = 4;
                }

                //  *************************************************
                //  *  Calculate runoff (creams p14, equation i-1)  *
                //  *************************************************
                double denom = EffectiveRain + 0.8 * runoffRetentionNumber;
                double bas = EffectiveRain - 0.2 * runoffRetentionNumber;
                if (!MathTools.DoublesAreEqual(denom, 0) && bas > 0)
                {
                    WatBal.Runoff = Math.Pow(bas, 2.0) / denom;
                    infiltration = EffectiveRain - WatBal.Runoff;
                }
                else
                {
                    WatBal.Runoff = 0;
                    infiltration = EffectiveRain;
                }

                //add any runoff from irrigation.
                WatBal.Runoff += Sim.IrrigationController.Output.IrrigationRunoff;

            }
            catch (Exception e)
            {
                if (progress == 0)
                {
                    Sim.ControlError = "CalculateRunoff - Calculate initial roughness effect on curve number (cn2).";
                }
                else if (progress == 1)
                {
                    Sim.ControlError = "CalculateRunoff - Updating roughness effect on curve number (cn2).";
                }
                else if (progress == 2)
                {
                    Sim.ControlError = "CalculateRunoff - Calculate smx (CREAMS p14, equations i-3 and i-4).";
                }
                else if (progress == 3)
                {
                    Sim.ControlError = "CalculateRunoff - Calculate retention parameter,  runoff_retention_number.";
                }
                else if (progress == 4)
                {
                    Sim.ControlError = "CalculateRunoff - Calculate runoff (creams p14, equation i-1).";
                }
                else
                {
                    Sim.ControlError = "CalculateRunoff";
                }
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateTotalWaterBalance()
        {
            try
            {
                sum_rainfall += Sim.ClimateController.Rain;
                sum_irrigation += WatBal.Irrigation;
                sum_runoff += WatBal.Runoff;
                sum_potevap += WatBal.PotSoilEvap;
                sum_soilevaporation += WatBal.SoilEvap;
                sum_transpiration += Sim.VegetationController.GetTotalTranspiration();
                sum_evapotranspiration = sum_soilevaporation + sum_transpiration;
                sum_overflow += overflowmm;
                sum_drainage += WatBal.DeepDrainage;
                sum_lateralflow += WatBal.LateralFlow;
                sum_soilerosion += ErosionTPerHa;
            }
            catch (Exception e)
            {
                Sim.ControlError = "UpdateTotalWaterBalance";
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculatSoilEvaporation()
        {
            try
            {
                //  ********************************************************************
                //  *  This function calculates soil evaporation using the Ritchie     *
                //  *  model.                                                          *
                //  ********************************************************************

                //  Calculate potential soil evaporation
                //  From proportion of bare soil
                WatBal.PotSoilEvap = Sim.VegetationController.GetPotentialSoilEvaporation();

                if (Sim.IrrigationController != null && Sim.IrrigationController.PondingExists())
                {
                    WatBal.SoilEvap = WatBal.PotSoilEvap;
                }
                else
                {
                    //  Add crop residue effects
                    ////NOTE THAT THIS USED TO ONLY BE FOR THE LAI MODEL -  I"VE NOW MADE IT FOR EITHER

                    if (TotalCropResidue > 1.0)
                    {
                        WatBal.PotSoilEvap = WatBal.PotSoilEvap * (Math.Exp(-0.22 * TotalCropResidue / 1000.0));
                    }

                    //  *******************************
                    //  *  initialize daily variables
                    //  ******************************
                    se1 = 0.0;
                    WatBal.SoilEvap = 0.0;
                    se2 = 0.0;
                    se21 = 0.0;
                    se22 = 0.0;
                    //  **************************************************
                    //  * If infiltration has occurred then reset sse1.  *
                    //  * Reset sse2 if infiltration exceeds sse1.       *
                    //  **************************************************
                    if (infiltration > 0.0)
                    {
                        sse2 = Math.Max(0, sse2 - Math.Max(0, infiltration - sse1));
                        sse1 = Math.Max(0, sse1 - infiltration);
                        if (!MathTools.DoublesAreEqual(DataModel.Stage2SoilEvapCona, 0))
                            dsr = Math.Pow(sse2 / DataModel.Stage2SoilEvapCona, 2);
                        else
                        {
                            dsr = 0;

                            LogDivideByZeroError("CalculatSoilEvaporation", "in_Cona_mm_per_sqrroot_day", "dsr");
                        }
                    }
                    //  ********************************
                    //  *  Test for 1st stage drying.  *
                    //  ********************************
                    if (sse1 < DataModel.Stage1SoilEvapU)
                    {
                        //  *****************************************************************
                        //  *  1st stage evaporation for today. Set se1 equal to potential  *
                        //  *  soil evaporation but limited by U.                           *
                        //  *****************************************************************
                        se1 = Math.Min(WatBal.PotSoilEvap, DataModel.Stage1SoilEvapU - sse1);
                        se1 = Math.Max(0.0, Math.Min(se1, SoilWaterRelWP[0] + AirDryLimitRelWP[0]));

                        //  *******************************
                        //  *  Accumulate stage 1 drying  *
                        //  *******************************
                        sse1 = sse1 + se1;
                        //  ******************************************************************
                        //  *  Check if potential soil evaporation is satisfied by 1st stage *
                        //  *  drying.  If not, calculate some stage 2 drying(se2).          *
                        //  ******************************************************************
                        if (WatBal.PotSoilEvap > se1)
                        {
                            //  *****************************************************************************
                            //  * If infiltration on day, and potential_soil_evaporation.gt.se1 (ie. a deficit in evap) .and. sse2.gt.0 *
                            //  * than that portion of potential_soil_evaporation not satisfied by se1 should be 2nd stage. This *
                            //  * can be determined by Math.Sqrt(time)*in_Cona_mm_per_sqrroot_day with any remainder ignored.          *
                            //  * If sse2 is zero, then use Ritchie's empirical transition constant (0.6).  *
                            //  *****************************************************************************
                            if (sse2 > 0.0)
                                se2 = Math.Min(WatBal.PotSoilEvap - se1, DataModel.Stage2SoilEvapCona * Math.Pow(dsr, 0.5) - sse2);
                            else
                                se2 = 0.6 * (WatBal.PotSoilEvap - se1);

                            //  **********************************************************
                            //  *  Calculate stage two evaporation from layers 1 and 2.  *
                            //  **********************************************************

                            //  Any 1st stage will equal infiltration and therefore no net change in
                            //  soil water for layer 1 (ie can use SoilWater_rel_wp(1)+AirDryLimit_rel_wp(1) to determine se21.
                            se21 = Math.Max(0.0, Math.Min(se2, SoilWaterRelWP[0] + AirDryLimitRelWP[0]));
                            se22 = Math.Max(0.0, Math.Min(se2 - se21, SoilWaterRelWP[1] + AirDryLimitRelWP[1]));
                            //  ********************************************************
                            //  *  Re-Calculate se2 for when se2-se21 > SoilWater_rel_wp(2)+AirDryLimit_rel_wp(2)  *
                            //  ********************************************************
                            se2 = se21 + se22;
                            //  ************************************************
                            //  *  Update 1st and 2nd stage soil evaporation.  *
                            //  ************************************************
                            sse1 = DataModel.Stage1SoilEvapU;
                            sse2 += se2;
                            if (!MathTools.DoublesAreEqual(DataModel.Stage2SoilEvapCona, 0))
                                dsr = Math.Pow(sse2 / DataModel.Stage2SoilEvapCona, 2);
                            else
                            {
                                dsr = 0;
                                LogDivideByZeroError("CalculatSoilEvaporation", "in_Cona_mm_per_sqrroot_day", "dsr");
                            }
                        }
                        else
                        {
                            se2 = 0.0;
                        }
                    }
                    else
                    {
                        sse1 = DataModel.Stage1SoilEvapU;
                        //  ************************************************************************
                        //  *  No 1st stage drying. Calc. 2nd stage and remove from layers 1 & 2.  *
                        //  ************************************************************************
                        dsr = dsr + 1.0;
                        se2 = Math.Min(WatBal.PotSoilEvap, DataModel.Stage2SoilEvapCona * Math.Pow(dsr, 0.5) - sse2);
                        se21 = Math.Max(0.0, Math.Min(se2, SoilWaterRelWP[0] + AirDryLimitRelWP[0]));
                        se22 = Math.Max(0.0, Math.Min(se2 - se21, SoilWaterRelWP[1] + AirDryLimitRelWP[1]));
                        //  ********************************************************
                        //  *  Re-calculate se2 for when se2-se21 > SoilWater_rel_wp(2)+AirDryLimit_rel_wp(2)  *
                        //  ********************************************************
                        se2 = se21 + se22;
                        //  *****************************************
                        //  *   Update 2nd stage soil evaporation.  *
                        //  *****************************************
                        sse2 = sse2 + se2;
                        //  **************************************
                        //  *  calculate total soil evaporation  *
                        //  **************************************
                    }
                    WatBal.SoilEvap = se1 + se2;
                }
            }
            catch (Exception e)
            {
                Sim.ControlError = "CalculatSoilEvaporation";
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateWaterBalance()
        {
            //***********************************************************************
            //*  This subroutine performs the water balance. New nested loop        *
            //*  algorithm infiltrates and redistributes water in one pass.  This   *
            //*  new algorithm has many advantages over the previous one.  Firstly, *
            //*  it is more biophysically realistic; secondly, it considers the     *
            //*  effects of a restricted Ksat on both infiltration and              *
            //*  redistribution.   Previously, only redistribution was considered.  *
            //*  It should also bettern explain water movemnet under saturated      *
            //*  conditions.                                                        *
            //***********************************************************************
            double oflow = 0.0;
            overflowmm = 0;
            double drain = infiltration;

            //  1.  Add all infiltration/drainage and extract ET.
            //  2.  Cascade a proportion of all water greater than drained upper limit (FC)
            //  3.  If soil water content is still greater than upper limit (SWMAX), add
            //      all excess above upper limit to runoff

            for (int i = 0; i < LayerCount; ++i)
            {
                Seepage[i] = drain;
                if (i == 0)
                {
                    SoilWaterRelWP[i] += Seepage[i] - (WatBal.SoilEvap - se22) - LayerTranspiration[i];
                }
                else if (i == 1)
                {
                    SoilWaterRelWP[i] += Seepage[i] - LayerTranspiration[i] + Red[i] - se22;
                }
                else
                {
                    SoilWaterRelWP[i] += Seepage[i] - LayerTranspiration[i] + Red[i];
                }

                if (SoilWaterRelWP[i] > DrainUpperLimitRelWP[i])
                {
                    drain = SWCon[i] * (SoilWaterRelWP[i] - DrainUpperLimitRelWP[i]);
                    //			if(drain>(ksat[i]*12.0))
                    //				drain=ksat[i]*12.0;
                    if (drain > KSat[i])
                        drain = KSat[i];
                    else if (drain < 0)

                        drain = 0;
                    SoilWaterRelWP[i] -= drain;
                }
                else
                {
                    drain = 0;
                }
                if (SoilWaterRelWP[i] > SaturationLimitRelWP[i])
                {
                    oflow = SoilWaterRelWP[i] - SaturationLimitRelWP[i];
                    SoilWaterRelWP[i] = SaturationLimitRelWP[i];
                }

                int j = 0;
                while (oflow > 0)
                {

                    if (i - j == 0)    //look at first layer
                    {
                        overflowmm += oflow;
                        WatBal.Runoff = WatBal.Runoff + oflow;
                        infiltration -= oflow;
                        Seepage[0] -= oflow;         //drainage in first layer
                        oflow = 0;
                    }
                    else           //look at other layersException e
                    {
                        SoilWaterRelWP[i - j] += oflow;
                        Seepage[i - j + 1] -= oflow;
                        if (SoilWaterRelWP[i - j] > SaturationLimitRelWP[i - j])
                        {
                            oflow = SoilWaterRelWP[i - j] - SaturationLimitRelWP[i - j];
                            SoilWaterRelWP[i - j] = SaturationLimitRelWP[i - j];
                        }
                        else
                        {
                            oflow = 0;
                        }
                        ++j;

                    }
                }
            }
            double satrange = SaturationLimitRelWP[0] - DrainUpperLimitRelWP[0];
            double satamount = SoilWaterRelWP[0] - DrainUpperLimitRelWP[0];
            if (satamount > 0 && satrange > 0)
            {
                saturationindex = satamount / satrange;
            }
            else
            {
                saturationindex = 0;
            }
            Seepage[LayerCount] = drain;
            WatBal.DeepDrainage = drain;
            TotalSoilWater = 0;
            for (int i = 0; i < LayerCount; ++i)
                TotalSoilWater += SoilWaterRelWP[i];

        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculateErosion()
        {
            try
            {
                //  ***********************************************************************
                //  *  This subroutine calculates sediment yield in tonnes/ha using the   *
                //  *  Dave Freebairn method                                              *
                //  ***********************************************************************

                ErosionTPerHa = 0;
                sedCatchmod = 0;
                if (WatBal.Runoff <= 1)
                {
                    SedimentConc = 0;
                }
                else
                {
                    double conc = 0;
                    double cover;
                    if (!Sim.IrrigationController.ConsiderCoverEffects())
                        cover = Math.Min(100.0, (CropCover + TotalResidueCover * (1 - CropCover)) * 100.0);
                    else
                        cover = Sim.IrrigationController.GetCoverEffect(CropCover, TotalResidueCover);
                    if (cover < 50.0)

                        conc = 16.52 - 0.46 * cover + 0.0031 * cover * cover;  //% sediment concentration Exception e max g/l is 165.2 when cover =0;
                    else if (cover >= 50.0)
                        conc = -0.0254 * cover + 2.54;
                    conc = Math.Max(0.0, conc);
                    ErosionTPerHa = conc * usleLsFactor * DataModel.USLEK * DataModel.USLEP * WatBal.Runoff / 10.0;
                    sedCatchmod = conc * DataModel.USLEK * DataModel.USLEP * WatBal.Runoff / 10.0;
                }
                if (!MathTools.DoublesAreEqual(WatBal.Runoff, 0))
                {
                    if (!InRunoff2)
                        ++RunoffEventCount2;
                    InRunoff2 = true;

                    SedimentConc = ErosionTPerHa * 100.0 / WatBal.Runoff * DataModel.SedDelivRatio;    //sediment concentration in g/l
                    if (SedimentConc > PeakSedConc)
                        PeakSedConc = SedimentConc;
                }
                else
                {
                    // dont log a divide by zero error for this one
                    if (InRunoff2)
                        CumSedConc += PeakSedConc;
                    PeakSedConc = 0;
                    InRunoff2 = false;
                    SedimentConc = 0;
                }
                OffsiteSedDelivery = ErosionTPerHa * DataModel.SedDelivRatio;
            }
            catch (Exception e)
            {
                Sim.ControlError = "CalculateErosion";
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void TryModelLateralFlow()
        {
            try
            {
                if (Sim.ModelOptionsController.CanCalculateLateralFlow())
                {

                    // Calculate most limiting Kratio
                    double kr;
                    double kratio = 1.0;
                    for (int i = 1; i < LayerCount; ++i)
                    {
                        if (!MathTools.DoublesAreEqual(KSat[i], 0))
                            kr = KSat[i - 1] / KSat[i];
                        else
                            kr = 0;
                        if (kr > kratio)
                            kratio = kr;
                    }

                    //  Convert in_FieldSlope_pc from percent to degrees

                    double slopedeg = Math.Atan(DataModel.FieldSlope / 100.0) * 180.0 / 3.14159;

                    // Calculate PredRH - lateral flow partitioning

                    double LN_kratio = Math.Log(kratio);
                    double LN_angle = Math.Log(slopedeg);
                    double LN_kratio2 = LN_kratio * LN_kratio;
                    double LN_angle2 = LN_angle * LN_angle;
                    double LNK_lnang = LN_kratio * LN_angle;

                    double numer = 0.04487067 + (0.019797884 * LN_kratio) - (0.020606403 * LN_angle)
                           + (0.01010285 * LN_kratio2) + (0.01415831 * LN_angle2)
                           - (0.011046881 * LNK_lnang);
                    double denom = 1 - (0.11431376 * LN_kratio) - (0.35073561 * LN_angle)
                           + (0.013044911) * (LN_kratio2) + (0.040556192 * LN_angle2)
                           + (0.015858813 * LNK_lnang);
                    if (!MathTools.DoublesAreEqual(denom, 0))
                        PredRh = numer / denom;
                    else
                    {
                        PredRh = 0;

                    }
                    WatBal.LateralFlow = Seepage[LayerCount] * PredRh;
                    Seepage[LayerCount] = Seepage[LayerCount] * (1 - PredRh);
                }
                else
                    WatBal.LateralFlow = 0;
            }
            catch (Exception e)
            {
                Sim.ControlError = "CalculateLateralFlow";
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitialiseSoilParameters()
        {

            Depth = new List<double>(LayerCount + 1).Fill(0);
            Red = new List<double>(LayerCount).Fill(0);
            WF = new List<double>(LayerCount).Fill(0);
            SoilWaterRelWP = new List<double>(LayerCount).Fill(0);
            DrainUpperLimitRelWP = new List<double>(LayerCount).Fill(0);
            SaturationLimitRelWP = new List<double>(LayerCount).Fill(0);
            AirDryLimitRelWP = new List<double>(LayerCount).Fill(0);
            WiltingPointRelOD = new List<double>(LayerCount).Fill(0);
            DULRelOD = new List<double>(LayerCount).Fill(0);
            KSat = new List<double>(LayerCount).Fill(0);
            SWCon = new List<double>(LayerCount).Fill(0);
            Seepage = new List<double>(LayerCount + 1).Fill(0);
            LayerTranspiration = new List<double>(LayerCount).Fill(0);
            MCFC = new List<double>(LayerCount).Fill(0);

            //double val1 = ParameterDoubleValues["DepthOfHorizon"][i];
            //if (i > 0) val1 = val1 - ParameterDoubleValues["DepthOfHorizon"][i - 1];
            //double val2 = ParameterDoubleValues["InSituAirDryMoisture"][i];
            //double val3 = ParameterDoubleValues["WiltingPoint"][i];
            //double val4 = ParameterDoubleValues["FieldCapacity"][i];
            //double val5 = ParameterDoubleValues["SaturatedWaterContent"][i];

            //ParameterDoubleValues["PlantAvailableWater"][i] = (val4 - val3) / 100.0 * val1;
            //total += ParameterDoubleValues["PlantAvailableWater"][i];
            //ParameterDoubleValues["MaxDailyDrainageVolumeFromLayer"][i] = (val5 - val4) / 100.0 * val1;

            swd = 0;
            sse1 = 0;
            sse2 = 0;
            se1 = 0;
            se2 = 0;
            se21 = 0;
            se22 = 0;
            dsr = 0;
            CumSedConc = 0;
            PeakSedConc = 0;
            Depth[0] = 0;
            for (int i = 0; i < LayerCount; ++i)
            {
                Depth[i + 1] = (int)(DataModel.Depths.Values[i] + 0.5);
                Red[i] = 0;
                KSat[i] = (DataModel.Saturation.Values[i] - DataModel.FieldCapacity.Values[i]) / 100 * Depth[i + 1];
                //KSat[i] = dataModel.MaxDailyDrainVolume.Values[i]; //Check this --- in_LayerDrainageLimit_mm_per_day[i];///12.0;
            }

            TotalSoilWater = 0.0;
            PreviousTotalSoilWater = 0.0;
            for (int i = 0; i < LayerCount; ++i)
            {
                if (Depth[i + 1] - Depth[i] > 0)
                {
                    //PERFECT soil water alorithms relate all values to wilting point.
                    double deltadepth = (Depth[i + 1] - Depth[i]) * 0.01;
                    WiltingPointRelOD[i] = DataModel.WiltingPoint.Values[i] * deltadepth;
                    DULRelOD[i] = DataModel.FieldCapacity.Values[i] * deltadepth;
                    if (i == 0)
                    {
                        AirDryLimitRelWP[0] = WiltingPointRelOD[i] - DataModel.AirDry.Values[i] * deltadepth;
                    }
                    else if (i == 1)
                    {
                        AirDryLimitRelWP[1] = 0.5 * (WiltingPointRelOD[i] - DataModel.AirDry.Values[i] * deltadepth);
                    }
                    else
                    {
                        AirDryLimitRelWP[i] = 0;
                    }
                    DrainUpperLimitRelWP[i] = (DataModel.FieldCapacity.Values[i] * deltadepth) - WiltingPointRelOD[i];
                    SaturationLimitRelWP[i] = (DataModel.Saturation.Values[i] * deltadepth) - WiltingPointRelOD[i];
                }
                else
                {

                    DrainUpperLimitRelWP[i] = 0;
                    SaturationLimitRelWP[i] = 0;
                    AirDryLimitRelWP[0] = 0;
                }
            }

            for (int i = 0; i < LayerCount; ++i)
            {
                SoilWaterRelWP[i] = Sim.ModelOptionsController.GetInitialPAW() * DrainUpperLimitRelWP[i];

                if (SoilWaterRelWP[i] > SaturationLimitRelWP[i])
                {
                    SoilWaterRelWP[i] = SaturationLimitRelWP[i];
                }
                else if (SoilWaterRelWP[i] < 0)
                {
                    SoilWaterRelWP[i] = 0;
                }
                TotalSoilWater += SoilWaterRelWP[i];
            }

            TotalCropResidue = 0;
            TotalResidueCover = 0;  //0.707*(1.0-exp(-1.0*total_crop_residue/1000.0));
            TotalResidueCoverPercent = 0;


            CalculateInitialValuesOfCumulativeSoilEvaporation();

            CalculateDepthRetentionWeightFactors();

            CalculateDrainageFactors();

            CalculateUSLE_LSFactor();
            RunoffEventCount2 = 0;
            PredRh = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculateInitialValuesOfCumulativeSoilEvaporation()
        {
            //  Calculate initial values of cumulative soil evaporation
            if (DrainUpperLimitRelWP[0] - SoilWaterRelWP[0] > DataModel.Stage1SoilEvapU)
            {
                sse1 = DataModel.Stage1SoilEvapU;
                sse2 = Math.Max(0.0, DrainUpperLimitRelWP[0] - SoilWaterRelWP[0]) - DataModel.Stage1SoilEvapU;
            }
            else
            {
                sse1 = Math.Max(0.0, DrainUpperLimitRelWP[0] - SoilWaterRelWP[0]);
                sse2 = 0.0;
            }
            if (!MathTools.DoublesAreEqual(DataModel.Stage2SoilEvapCona, 0))
                dsr = Math.Pow(sse2 / DataModel.Stage2SoilEvapCona, 2.0);
            else
            {
                dsr = 0;

                LogDivideByZeroError("InitialiseSoilParameters", "in_Cona_mm_per_sqrroot_day", "dsr");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculateVolumeBalanceError()
        {
            try
            {
                double sse;
                double deltasw = TotalSoilWater - PreviousTotalSoilWater;
                if (Sim.ModelOptionsController.CanCalculateLateralFlow())
                {
                    if (!MathTools.DoublesAreEqual(Sim.ClimateController.Rain, MathTools.MISSING_DATA_VALUE))
                        sse = (WatBal.Irrigation + Sim.ClimateController.Rain) - (deltasw + WatBal.Runoff + WatBal.SoilEvap + Sim.VegetationController.GetTotalTranspiration() + Seepage[LayerCount] + WatBal.LateralFlow);
                    else
                        sse = (WatBal.Irrigation + 0) - (deltasw + WatBal.Runoff + WatBal.SoilEvap + Sim.VegetationController.GetTotalTranspiration() + Seepage[LayerCount] + WatBal.LateralFlow);
                }
                else
                {
                    if (!MathTools.DoublesAreEqual(Sim.ClimateController.Rain, MathTools.MISSING_DATA_VALUE))
                        sse = (WatBal.Irrigation + Sim.ClimateController.Rain) - (deltasw + WatBal.Runoff + WatBal.SoilEvap + Sim.VegetationController.GetTotalTranspiration() + Seepage[LayerCount]);
                    else
                        sse = (WatBal.Irrigation + 0) - (deltasw + WatBal.Runoff + WatBal.SoilEvap + Sim.VegetationController.GetTotalTranspiration() + Seepage[LayerCount]);
                }

                WatBal.VBE = (int)(sse * 1000000) / 100000.0;
            }
            catch (Exception e)
            {
                Sim.ControlError = "CalculateVolumeBalanceError";
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculateUSLE_LSFactor()
        {
            if (Sim.ModelOptionsController.UsePerfectUSLELSFn())
            {
                double aht = DataModel.FieldSlope * DataModel.SlopeLength / 100.0;
                double lambda = 3.281 * (Math.Sqrt(DataModel.SlopeLength * DataModel.SlopeLength + aht * aht));
                double theta;
                if (!MathTools.DoublesAreEqual(DataModel.SlopeLength, 0))
                    theta = Math.Asin(aht / DataModel.SlopeLength);
                else
                {
                    theta = 0;

                    LogDivideByZeroError("CalculateUSLE_LSFactor", "in_SlopeLength_m", "theta");
                }
                if (!MathTools.DoublesAreEqual(1.0 + DataModel.RillRatio, 0))
                {
                    if (DataModel.FieldSlope < 9.0)

                        usleLsFactor = Math.Pow(lambda / 72.6, DataModel.RillRatio / (1.0 + DataModel.RillRatio)) * (10.8 * Math.Sin(theta) + 0.03);
                    else
                        usleLsFactor = Math.Pow(lambda / 72.6, DataModel.RillRatio / (1.0 + DataModel.RillRatio)) * (16.8 * Math.Sin(theta) - 0.5);
                }
                else
                {
                    usleLsFactor = 0;

                    LogDivideByZeroError("CalculateUSLE_LSFactor", "1.0+in_RillRatio", "usle_ls_factor");
                }
            }
            else
            {
                if (!MathTools.DoublesAreEqual(1.0 + DataModel.RillRatio, 0))
                {
                    usleLsFactor = Math.Pow(DataModel.SlopeLength / 22.1, DataModel.RillRatio / (1.0 + DataModel.RillRatio)) * (0.065 + 0.0456 * DataModel.FieldSlope + 0.006541 * Math.Pow(DataModel.FieldSlope, 2));
                }
                else
                {
                    usleLsFactor = 0;

                    LogDivideByZeroError("CalculateUSLE_LSFactor", "1.0+in_RillRatio", "usle_ls_factor");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculateDepthRetentionWeightFactors()
        {
            double a, b;
            for (int i = 0; i < LayerCount - 1; ++i)
            {
                if (Depth[LayerCount - 1] > 0)
                {
                    a = -4.16 * (Depth[i] / Depth[LayerCount - 1]);
                    b = -4.16 * (Depth[i + 1] / Depth[LayerCount - 1]);
                    WF[i] = 1.016 * (Math.Exp(a) - Math.Exp(b));
                }
                else
                {
                    WF[i] = 0;

                    LogDivideByZeroError("CalculateDepthRetentionWeightFactors", "depth[in_LayerCount-1]", "wf[i]");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="s2"></param>
        /// <param name="s3"></param>
        public void LogDivideByZeroError(string s, string s2, string s3)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculateDrainageFactors()
        {
            for (int i = 0; i < LayerCount; ++i)
            {
                if (KSat[i] > 0)
                {
                    // I've got rid of the old PERFECTism regarding Ksat
                    // the commented bits below was just my testing algorithms,
                    // to make sure the results are identical to the reworked equations.
                    //	double oldksat=ksat[i]/12.0;
                    //	double val1 = 48.0/(2.0*(SaturationLimit_rel_wp[i]-DrainUpperLimit_rel_wp[i])/oldksat+24.0);
                    //	double val2 = 2.0*ksat[i]/(SaturationLimit_rel_wp[i]-DrainUpperLimit_rel_wp[i]+ksat[i]);
                    //	if(int(val1*1000000)!=int(val2*1000000))
                    //		throw(new Exception("error!", mtError, TMsgDlgButtons() << mbOK, 0);
                    //	swcon[i]=val2;
                    double temp = (SaturationLimitRelWP[i] - DrainUpperLimitRelWP[i] + KSat[i]);
                    if (!MathTools.DoublesAreEqual(temp, 0))
                        SWCon[i] = 2.0 * KSat[i] / temp;
                    else
                    {
                        SWCon[i] = 0;

                        LogDivideByZeroError("CalculateDrainageFactors", "SaturationLimit_rel_wp[i]-DrainUpperLimit_rel_wp[i]+ksat[i]", "swcon[i]");
                    }
                }
                else
                {
                    SWCon[i] = 0;
                }

                if (SWCon[i] < 0)
                {
                    SWCon[i] = 0;
                }
                else if (SWCon[i] > 1)
                {
                    SWCon[i] = 1;
                }
            }
        }
    }
}
