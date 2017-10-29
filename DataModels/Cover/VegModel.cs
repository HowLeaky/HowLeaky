using HowLeaky.ModelControllers;
using HowLeaky.Models;
using HowLeaky.Tools;
using HowLeakyWebsite.Tools.DHMCoreLib.Helpers;
using System;
using static HowLeaky.ModelControllers.VegetationController;

namespace HowLeaky.DataModels
{
    //public enum CropStatus { csInFallow, csPlanting, csGrowing, csHarvesting };

    public class VegObjectController : HLObject
    {
        //TODO:Refactor these variables
        //**************************************************************************
        // Output Parameters
        //**************************************************************************
        public double out_DaysSincePlant_days { get; set; }                         // Days since planting (days)
        public double out_LAI { get; set; }                                         // Leaf area index (LAI) [LAI model only]
        public double out_GreenCover_pc { get; set; }                               // Green cover (%) - living vegetation cover expressed as a percentage of total area
        public double out_ResidueCover_pc { get; set; }                             // Reside cover (%) - dead vegetation cover expressed as a percentage of total area
        public double out_TotalCover_pc { get; set; }                               // Total cover (%) - living and dead cover, calculated using Beer's Law, and expressed as a percentage of total area
        public double out_ResidueAmount_kg_per_ha { get; set; }                     // Residue biomass amount (kg/ha)
        public double out_DryMatter_kg_per_ha { get; set; }                         // Dry matter (kg/ha) - cumulative dry matter
        public double out_RootDepth_mm { get; set; }                                // Root depth (mm)
        public double out_Yield_t_per_ha { get; set; }                              // Yield (t/ha)
        public double out_PotTranspiration_mm { get; set; }                         // Potential transpiration (mm)
        public double out_GrowthRegulator { get; set; }                             // Growth regulator [LAI model only]
        public double out_WaterStressIndex { get; set; }                            // Water stress index [LAI model only]
        public double out_TempStressIndex { get; set; }                             // Temperature stress index [LAI model only]
        public double out_CropRainfall_mm { get; set; }                             // Crop Rainfall (mm)
        public double out_CropIrrigation_mm { get; set; }                           // Crop Irrigation (mm)
        public double out_CropRunoff_mm { get; set; }                               // Crop Runoff (mm)
        public double out_SoilEvaporation_mm { get; set; }                          // Crop Soil Evaporation (mm)
        public double out_Transpiration_mm { get; set; }                            // Crop Transpiration (mm)
        public double out_CropEvapoTranspiration_mm { get; set; }                   // Crop Evapotranspiration (mm)
        public double out_CropDrainage_mm { get; set; }                             // Crop Drainage (mm)
        public double out_CropLateralFlow_mm { get; set; }                          // Crop Lateral Flow (mm)
        public double out_CropOverflow_mm { get; set; }                             // Crop Overflow (mm)
        public double out_CropSoilErrosion_t_per_ha { get; set; }                   // Crop Soil Erosion (t/ha)
        public double out_CropSedimentDelivery_t_per_ha { get; set; }               // Crop Off Site Sediment Delivery (t/ha)
        public double out_PlantingCount { get; set; }                               // Crops Planted
        public double out_HarvestCount { get; set; }                                // Crops Harvested
        public double out_CropDeaths { get; set; }                                  // Crops Killed
        public double out_YieldPerHarvest_kg_per_ha_per_harvest { get; set; }       // Avg. Yield per Harvest (kg/ha/harvest)
        public double out_YieldPerPlant_kg_per_ha_per_plant { get; set; }           // Avg. Yield per Planting (kg/ha/plant)
        public double out_YieldPerYear_kg_per_ha_per_yr { get; set; }               // Avg. Yield per Year (kg/ha/yr)
        public double out_YieldDivTranspir_kg_per_ha_per_mm { get; set; }           // Yield/Transpiration (kg/ha/mm)
        public double out_ResidueCovDivTranspir_pc_per_mm { get; set; }             // Residue Cover/Transpiration (%/mm)
        public double out_PotMaxLAI { get; set; }                                   // Potential Maximum LAI

        //**************************************************************************
        // Internal Parameters
        //**************************************************************************
        public bool today_is_harvest_day { get; set; }
        public bool predefined_residue { get; set; }
        public CropStatus CropStatus { get; set; }
        public string Name { get; set; }
        public DateTime LastSowingDate { get; set; } = DateUtilities.NULLDATE;
        public DateTime LastHarvestDate { get; set; } = DateUtilities.NULLDATE;
        public DateTime FirstRotationDate { get; set; } = DateUtilities.NULLDATE;
        public int days_since_planting { get; set; }
        public int rotation_count { get; set; }
        public int missed_rotation_count { get; set; }
        public int number_of_plantings { get; set; }
        public int number_of_harvests { get; set; }
        public int number_of_crops_killed { get; set; }
        public int number_of_fallows { get; set; }
        public int killdays { get; set; }
        public double MaximumRootDepth { get; set; }
        public double dry_matter { get; set; }
        public double total_transpiration { get; set; }
        public double crop_stage { get; set; }
        public double green_cover { get; set; }
        public double crop_cover { get; set; }
        public double total_cover { get; set; }
        public double crop_cover_percent { get; set; }
        public double crop_residue { get; set; }
        public double residue_cover { get; set; }
        public double runoff { get; set; }
        public double drainage { get; set; }
        public double soil_evaporation { get; set; }
        public double total_evapotranspiration { get; set; }
        public double yield { get; set; }
        public double soil_water_at_planting { get; set; }
        public double soil_water_at_harvest { get; set; }
        public double cumulative_yield { get; set; }
        public double accumulated_cover { get; set; }
        public double accumulated_residue { get; set; }
        public double accumulated_transpiration { get; set; }
        public double total_crop_plantings { get; set; }                    //NEW
        public double total_crop_harvested { get; set; }                    //NEW
        public double total_crop_killed { get; set; }                       //NEW
        public double average_yield_perharvest { get; set; }                //NEW
        public double average_yield_perplanting { get; set; }               //NEW
        public double average_yield_peryear { get; set; }                   //NEW
        public double crops_harvested_div_crops_planted { get; set; }       //NEW
        public double yield_div_transpiration { get; set; }                 //NEW
        public double residue_cover_div_transpiration { get; set; }         //NEW

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

        public double so_AvgCropRainfall_mm { get; set; }
        public double so_AvgCropIrrigation_mm { get; set; }
        public double so_AvgCropRunoff_mm { get; set; }
        public double so_CropSoilEvaporation_mm { get; set; }
        public double so_CropTranspiration_mm { get; set; }
        public double so_AvgCropEvapoTranspiration_mm { get; set; }
        public double so_AvgCropOverflow_mm { get; set; }
        public double so_AvgCropDrainage_mm { get; set; }
        public double so_AvgCropLateralFlow_mm { get; set; }
        public double so_AvgCropSoilErrosion_t_per_ha { get; set; }
        public double so_AnnualCropSedimentDelivery_t_per_ha { get; set; }
        public bool TodayIsPlantDay
        {
            get
            {
                return days_since_planting == 0;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CropIndex
        {
            get
            {
                return VegetationController.GetCropIndex(this);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public VegetationController VegetationController { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool GetExistsInTheGround() { return (CropStatus == CropStatus.csPlanting || CropStatus == CropStatus.csGrowing); }
        /// <summary>
        /// 
        /// </summary>
        public bool ExistsInTheGround
        {
            get
            {
                return GetExistsInTheGround();
            }
        }

        //**************************************************************************

        public virtual void ResetCropParametersAfterHarvest() { }
        public virtual bool IsCropUnderMaxContinuousRotations() { return true; }
        public virtual bool SatisifiesMultiPlantInWindow() { return true; }
        public virtual bool HasCropHadSufficientContinuousRotations() { return true; }
        public virtual bool HasCropBeenAbsentForSufficientYears(DateTime today) { return true; }
        public virtual bool IsReadyToPlant() { return true; }
        public virtual bool StillRequiresIrrigating() { return false; }
        public virtual bool IsSequenceCorrect() { return true; }
        public virtual bool IsGrowing() { return (CropStatus == CropStatus.csGrowing); }
        public virtual bool InitialisedMeasuredInputs() { return false; }
        public virtual bool DoesCropMeetSowingCriteria() { return false; }
        public virtual bool GetIsLAIModel() { return false; }
        public virtual bool GetInFallow() { return (CropStatus == CropStatus.csInFallow); }
        public virtual double GetPotentialSoilEvaporation() { return 0; }
        public virtual double GetTotalCover() { return 0; }
        public virtual double CalculatePotentialTranspiration() { return 0; }


        /// <summary>
        /// 
        /// </summary>
        public VegObjectController() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sim"></param>
        public VegObjectController(Simulation sim) : base(sim)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ResetRotationCount()
        {
            rotation_count = 0;
            missed_rotation_count = 0;
            FirstRotationDate = sim.today;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool GetIsPlanting()
        {
            //TOD:Check this ishould be double
            return (MathTools.DoublesAreEqual(out_DaysSincePlant_days, 0));
        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateTranspiration()
        {
            for (int i = 0; i < sim.in_LayerCount; ++i)
            {
                sim.layer_transpiration[i] = 0;
            }
            out_PotTranspiration_mm = CalculatePotentialTranspiration();



            if (out_PotTranspiration_mm > 0)
            {
                double psup;
                //	std::vector<double>density;
                //	std::vector<double>supply;
                //	std::vector<double>root_penetration;

                double[] density = new double[10];
                double[] supply = new double[10];
                double[] root_penetration = new double[10];

                //        total_transpiration=1;//REMOVE THIS
                //		return ;//REMOVE THIS

                //	density.resize(sim.in_LayerCount);
                //	supply.resize(sim.in_LayerCount);
                //	root_penetration.resize(sim.in_LayerCount);




                // initialize transpiration array
                for (int i = 0; i < sim.in_LayerCount; ++i)
                {
                    sim.layer_transpiration[i] = 0;
                }
                //  Calculate soil water supply index

                for (int i = 0; i < sim.in_LayerCount; ++i)
                {
                    double denom = sim.DrainUpperLimit_rel_wp[i];
                    if (MathTools.DoublesAreEqual(denom, 0))
                    {
                        sim.mcfc[i] = MathTools.CheckConstraints(sim.SoilWater_rel_wp[i] / denom, 1.0, 0.0);
                    }
                    else
                    {
                        sim.mcfc[i] = 0;
                        //LogDivideByZeroError("CalculateTranspiration","sim.DrainUpperLimit_rel_wp[i]","sim.mcfc[i]");
                    }

                    if (sim.mcfc[i] >= 0.3)
                        supply[i] = 1.0;
                    else
                        supply[i] = sim.mcfc[i] / 0.3;
                }


                //  Calculate root penetration per layer (root_penetration)
                //  Calculate root density per layer (density)

                root_penetration[0] = 1.0;
                density[0] = 1.0;
                for (int i = 1; i < sim.in_LayerCount; ++i)
                {
                    if (sim.depth[i + 1] - sim.depth[i] > 0)
                    {
                        root_penetration[i] = Math.Min(1.0, Math.Max(out_RootDepth_mm - sim.depth[i], 0) / (sim.depth[i + 1] - sim.depth[i]));
                        if (sim.depth[i + 1] > 300)
                        {
                            if (MathTools.DoublesAreEqual(MaximumRootDepth, 300))
                            {
                                density[i] = Math.Max(0.0, (1.0 - 0.50 * Math.Min(1.0, (sim.depth[i + 1] - 300.0) / (MaximumRootDepth - 300.0))));
                            }
                            else
                            {
                                density[i] = 0.5;
                                //dont log this error
                            }
                        }
                        else
                            density[i] = 1.0;
                    }
                    else
                    {
                        root_penetration[i] = 0;
                        density[i] = 1.0;
                        //string text1="sim.depth["+string(i+1)+"]-sim.depth["+string(i)+"] ("+FormatFloat("0.#",sim.depth[i+1])+"-"+FormatFloat("0.#",sim.depth[i])+")";
                        //LogDivideByZeroError("CalculateTranspiration",text1,"root_penetration["+String(i)+"]");
                    }
                }

                // Calculate transpiration from each layer

                psup = 0;
                for (int i = 0; i < sim.in_LayerCount; ++i)
                {

                    if (root_penetration[i] < 1.0 && sim.mcfc[i] <= (1.0 - root_penetration[i]))
                        sim.layer_transpiration[i] = 0.0;
                    else
                        sim.layer_transpiration[i] = density[i] * supply[i] * out_PotTranspiration_mm;

                    if (sim.layer_transpiration[i] > sim.SoilWater_rel_wp[i])
                        sim.layer_transpiration[i] = Math.Max(0.0, sim.SoilWater_rel_wp[i]);
                    psup = psup + sim.layer_transpiration[i];

                }


                // reduce transpiration if more than potential transpiration
                if (!MathTools.DoublesAreEqual(psup, 0) && psup > out_PotTranspiration_mm)
                {
                    for (int i = 0; i < sim.in_LayerCount; ++i)
                        sim.layer_transpiration[i] *= out_PotTranspiration_mm / psup;
                }
                total_transpiration = 0;
                for (int i = 0; i < sim.in_LayerCount; ++i)
                {
                    total_transpiration += sim.layer_transpiration[i];
                    // sim.layer_transpiration[i]=layer_transpiration[i];
                }

            }
            else
            {
                for (int i = 0; i < sim.in_LayerCount; ++i)
                    sim.layer_transpiration[i] = 0;
                total_transpiration = 0;
            }
            accumulated_transpiration += total_transpiration;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double CalcFallowSoilWater()
        {
            if (soil_water_at_harvest > 0)
                return soil_water_at_planting - soil_water_at_harvest;
            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Plant()
        {
            ++sim.total_number_plantings;
            sim.accumulate_cov_day_before_planting += sim.total_residue_cover;
            VegObjectController lastcrop = sim.VegetationController.CurrentCrop;
            sim.VegetationController.CurrentCrop = this;
            LastSowingDate = sim.today;
            if (lastcrop != this || rotation_count == 0)
            {
                lastcrop.ResetRotationCount();
                sim.VegetationController.CurrentCrop.ResetRotationCount();
            }

            out_DaysSincePlant_days = 0;
            ++number_of_plantings;
            dry_matter = 0;
            out_DryMatter_kg_per_ha = 0;
            crop_stage = 0;
            out_RootDepth_mm = 0;
            yield = 0;
            soil_water_at_planting = sim.total_soil_water;
            CropStatus = CropStatus.csGrowing;

        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetCover()
        {
            green_cover = 0;
            crop_cover = 0;
            crop_cover_percent = 0;
            total_cover = 0;
            out_TotalCover_pc = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateResidue()
        {
            accumulated_residue += residue_cover * 100.0;
        }


        //void  ResizeSoilRelatedVector(int size)
        //{
        //  root_penetration.resize(size);
        //  layer_transpiration.resize(size);
        //}

        /// <summary>
        /// 
        /// </summary>
        public void InitialiseCropSummaryParameters()
        {
            total_crop_plantings = 0;
            total_crop_harvested = 0;
            total_crop_killed = 0;
            average_yield_perharvest = 0;
            average_yield_perplanting = 0;
            average_yield_peryear = 0;


            crops_harvested_div_crops_planted = 0;
            yield_div_transpiration = 0;
            residue_cover_div_transpiration = 0;


        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetCropIndex()
        {
            return sim.VegetationController.GetCropIndex(this);
        }
        /// <summary>
        /// 
        /// </summary>
        public void UpdateCropSummary()
        {
            double numyears = (double)(sim.number_of_days_in_simulation / 365.0);

            total_crop_plantings = number_of_plantings;                    //NEW
            total_crop_harvested = number_of_harvests;                    //NEW
            total_crop_killed = number_of_crops_killed;                       //NEW
            //TODO: Should this be a double
            if (!MathTools.DoublesAreEqual(total_crop_harvested, 0))
            {
                average_yield_perharvest = cumulative_yield / (double)total_crop_harvested;                //NEW
            }
            else
            {
                average_yield_perharvest = 0;                //NEW
            }
            average_yield_peryear = cumulative_yield / numyears;                   //NEW

            if (!MathTools.DoublesAreEqual(total_crop_plantings, 0))
            {
                crops_harvested_div_crops_planted = total_crop_harvested / (double)total_crop_plantings * 100.0;       //NEW
                average_yield_perplanting = cumulative_yield / (double)total_crop_plantings;               //NEW
            }
            else
            {
                crops_harvested_div_crops_planted = 0;
                average_yield_perplanting = 0;
            }
            if (!MathTools.DoublesAreEqual(accumulated_transpiration, 0))
            {
                yield_div_transpiration = cumulative_yield / accumulated_transpiration;                 //NEW
                residue_cover_div_transpiration = accumulated_residue / accumulated_transpiration;         //NEW
            }
            else
            {
                yield_div_transpiration = 0;
                residue_cover_div_transpiration = 0;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public void UpdateCropWaterBalance()
        {
            try
            {
                if (ExistsInTheGround)
                {
                    sum_crop_rainfall += sim.out_Rain_mm;
                    sum_crop_irrigation += sim.out_WatBal_Irrigation_mm;
                    sum_crop_runoff += sim.out_WatBal_Runoff_mm;
                    sum_crop_soilevaporation += sim.out_WatBal_SoilEvap_mm;
                    sum_crop_transpiration += total_transpiration;
                    sum_crop_evapotranspiration = sum_crop_soilevaporation + sum_crop_transpiration;
                    sum_crop_overflow += sim.out_WatBal_Overflow_mm;
                    sum_crop_drainage += sim.out_WatBal_DeepDrainage_mm;
                    sum_crop_lateralflow += sim.out_WatBal_LateralFlow_mm;
                    sum_crop_soilerosion += sim.out_Soil_HillSlopeErosion_t_per_ha;
                }

            }
            catch (Exception e)
            {
                throw (new Exception(e.Message));
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateMonthlyOutputs()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateSummaryOutputs()
        {
            double denom = number_of_plantings;
            so_AvgCropRainfall_mm = MathTools.Divide(sum_crop_rainfall, denom);
            so_AvgCropIrrigation_mm = MathTools.Divide(sum_crop_irrigation, denom);
            so_AvgCropRunoff_mm = MathTools.Divide(sum_crop_runoff, denom);
            so_CropSoilEvaporation_mm = MathTools.Divide(sum_crop_soilevaporation, denom);
            so_CropTranspiration_mm = MathTools.Divide(sum_crop_transpiration, denom);
            so_AvgCropEvapoTranspiration_mm = MathTools.Divide(sum_crop_evapotranspiration, denom);
            so_AvgCropOverflow_mm = MathTools.Divide(sum_crop_overflow, denom);
            so_AvgCropDrainage_mm = MathTools.Divide(sum_crop_drainage, denom);
            so_AvgCropLateralFlow_mm = MathTools.Divide(sum_crop_lateralflow, denom);
            so_AvgCropSoilErrosion_t_per_ha = MathTools.Divide(sum_crop_soilerosion, denom);
            so_AnnualCropSedimentDelivery_t_per_ha = MathTools.Divide(sum_crop_soilerosion, denom) * sim.in_SedDelivRatio;
        }
    }
}
