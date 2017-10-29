using HowLeaky.CustomAttributes;
using HowLeaky.Models;
using HowLeaky.Tools;
using HowLeaky.Tools.DataObjects;
using HowLeaky.XmlObjects;
using HowLeakyWebsite.Tools.DHMCoreLib.Helpers;
using System;
using System.Xml.Serialization;

namespace HowLeaky.DataModels
{
    public enum PlantingRules { prPlantInWindow, prFixedAnualPlaning, prPlantFromSequenceFile };

    //<VegetationType href = "C:\REEF\Grains_HL_2015\P2R2\P2R2_Jo\Vegetation\A and B management Chickpeas CQ 1 JO.vege" text="A and B management Chickpeas - CQ 1" Description="A and B management Chickpeas - CQ 1">
    //<PotMaxLai default="3.5" LastModified="08/04/2011">3</PotMaxLai>
    //<PropGrowSeaForMaxLai>0.7</PropGrowSeaForMaxLai>
    //<PercentOfMaxLai1>5</PercentOfMaxLai1>
    //<PercentOfGrowSeason1>15</PercentOfGrowSeason1>
    //<PercentOfMaxLai2>75</PercentOfMaxLai2>
    //<PercentOfGrowSeason2 LastModified = "21/01/2011" > 50 </ PercentOfGrowSeason2 >
    //< SWPropForNoStress > 0.3 </ SWPropForNoStress >
    //< DegreeDaysPlantToHarvest default="1900" LastModified="21/01/2011">2000</DegreeDaysPlantToHarvest>
    //<SenesenceCoef default="0.75" LastModified="21/01/2011">1</SenesenceCoef>
    //<RadUseEffic default="2.4" LastModified="08/04/2011">1.6</RadUseEffic>
    //<HarvestIndex default="0.42" LastModified="21/01/2011">0.5</HarvestIndex>
    //<BaseTemp>0</BaseTemp>
    //<OptTemp>20</OptTemp>
    //<MaxRootDepth default="1200" LastModified="21/01/2011">600</MaxRootDepth>
    //<DailyRootGrowth default="15">12</DailyRootGrowth>
    //<WatStressForDeath>0.1</WatStressForDeath>
    //<DaysOfStressToDeath default="21">30</DaysOfStressToDeath>
    //<MaxResidueLoss default="4" LastModified="25/02/2011">5</MaxResidueLoss>
    //<BiomassAtFullCover default="5000" LastModified="21/04/2011">4000</BiomassAtFullCover>
    ////<PropGGDEnd>1</PropGGDEnd>
    public class PlantingFormat : IndexData
    {
        public DayMonthData StartPlantWindow { get; set; }
        public DayMonthData EndPlantWindow { get; set; }
        public DayMonthData PlantDate { get; set; }
        public StateData ForcePlanting { get; set; }
        public StateData MultiPlantInWindow { get; set; }
        public RotationOptions RotationOptions { get; set; }
        public Sequence PlantingDates { get; set; }
        public FallowSwitch FallowSwitch { get; set; }
        public RainfallSwitch RainfallSwitch { get; set; }
        public SoilWaterSwitch SoilWaterSwitch { get; set; }
        public RatoonCrop RatoonCrop { get; set; }

        public PlantingFormat() { }
    }

    public class RotationOptions : IndexData
    {
        public int MinContinuousRotations { get; set; }
        public int MaxContinuousRotations { get; set; }
        public int MinYearsBetweenSowing { get; set; }

        public RotationOptions() { }
    }

    public class FallowSwitch : StateData
    {
        public int MinFallowLength { get; set; }

        public FallowSwitch() { }
    }

    public class RainfallSwitch : StateData
    {
        public double PlantingRain { get; set; }
        public int DaysToTotalRain { get; set; }
        public int SowingDelay { get; set; }

        public RainfallSwitch() { }
    }

    public class SoilWaterSwitch : StateData
    {
        public double MinSoilWaterRatio { get; set; }
        public double MaxSoilWaterRatio { get; set; }
        public double AvailSWAtPlanting { get; set; }
        public double SoilDepthToSumPlantingSW { get; set; }

        public SoilWaterSwitch() { }
    }

    public class RatoonCrop : StateData
    {
        public int RatoonCount { get; set; }
        public double RatoonScaleFactor { get; set; }

        public RatoonCrop() { }

    }
    public class Waterlogging : StateData
    {
        public double WaterLoggingFactor1 { get; set; }
        public double WaterLoggingFactor2 { get; set; }

        public Waterlogging() { }
    }


    [XmlRoot("VegetationType")]
    public class LAIVegObjectDataModel : DataModel
    {
        //Input Parameters
        public double PotMaxLAI { get; set; }                   // The upper limit of the leaf area index (LAI) - development curve.
        public double PropGrowSeaForMaxLai { get; set; }        // The development stage for potential maximum LAI.
        public double BiomassAtFullCover { get; set; }          // The amount of dry plant residues (ie stubble, pasture litter etc) that results in complete coverage of the ground.  This parameter controls the relationship between the amount of crop residue and cover, which is used in calculating runoff and erosion.
        [Unit("mm_per_day")]
        public double DailyRootGrowth { get; set; }             // The daily increment in root depth.
        public double PropGDDEnd { get; set; }                  // Set the proportion of the growth cycle for which irrigation is possible.
        public double DaysOfStressToDeath { get; set; }         // The number of consecutive days that water supply is less than threshold before the crop is killed.
        public double PercentOfMaxLai1 { get; set; }            // Percent of Maximum LAI for the 1st development stage.
        public double PercentOfGrowSeason1 { get; set; }        // The development stage for the 1st LAI "point".
        public double PercentOfMaxLai2 { get; set; }            // Percent of Maximum LAI for the 2nd development stage.
        public double PercentOfGrowSeason2 { get; set; }        // The development stage for the 2nd LAI "point".
        [Unit("days")]
        public double DegreeDaysPlantToHarvest { get; set; }    // The sum of degree-days (temperature less the base temperature) between planting and harvest.  Controls the rate of crop development and the potential duration of the crop. Some plants develop to maturity and harvest more slowly than others - these accumulate more degree-days between plant and harvest.
        public double SenesenceCoef { get; set; }               // Rate of LAI decline after max LAI.
        public double RadUseEffic { get; set; }                 // Biomass production per unit of radiation.
        public double HarvestIndex { get; set; }                // The grain biomass (kg/ha) divided by the above-ground biomass at flowering (kg/ha)
        [Unit("oC")]
        public double BaseTemp { get; set; }                    // The lower limit of plant development and growth, with respect to temperature (the average day temperature, degrees Celsius). The base temperature of vegetation is dependent on the type of environment in which the plant has evolved, and any breeding for hot or cold conditions.
        [Unit("oC")]
        public double OptTemp { get; set; }                     // The temperature for maximum biomass production.  Biomass production is a linear function of temperature between the Base temperature and the Optimum temperature.
        [Unit("mm")]
        public double MaxRootDepth { get; set; }                // located in CustomVegObject - >The maximum depth of the roots from the soil surface.  For the LAI model, the model calculates daily root growth from the root depth increase parameter
        public double SWPropForNoStress { get; set; }           // Ratio of water supply to potential water supply that indicates a stress day
        public double MaxResidueLoss { get; set; }              //Decomposition Rate

        public DayMonthData PlantDate { get; set; }
        public PlantingFormat PlantingFormat { get; set; }
        public Waterlogging Waterlogging { get; set; }

        //Getters
        public int PlantingRulesOptions { get {return PlantingFormat.index; } }
        public DayMonthData PlantingWindowStartDate { get { return PlantingFormat.StartPlantWindow; } }
        public DayMonthData PlantingWindowEndDate { get { return PlantingFormat.EndPlantWindow; } }
        public bool ForcePlantingAtEndOfWindow { get { return PlantingFormat.ForcePlanting.state; } }
        public bool MultiPlantInWindow { get { return PlantingFormat.MultiPlantInWindow.state; } }
        public int RotationFormat { get { return PlantingFormat.RotationOptions.index; } }
        public int MinRotationCount { get { return PlantingFormat.RotationOptions.MinContinuousRotations; } }
        public int MaxRotationCount { get { return PlantingFormat.RotationOptions.MaxContinuousRotations; } }
        public int RestPeriodAfterChangingCrops { get { return PlantingFormat.RotationOptions.MinYearsBetweenSowing; } }
        public bool FallowSwitch { get { return PlantingFormat.FallowSwitch.state; } }
        [Unit("days")]
        public int MinimumFallowPeriod { get { return PlantingFormat.FallowSwitch.MinFallowLength; } }
        public bool PlantingRainSwitch { get { return PlantingFormat.RainfallSwitch.state; } }
        [Unit("mm")]
        public double RainfallPlantingThreshold { get { return PlantingFormat.RainfallSwitch.PlantingRain; } }
        [Unit("days")]
        public int RainfallSummationDays { get { return PlantingFormat.RainfallSwitch.DaysToTotalRain; } }
        public bool SoilWaterSwitch { get { return PlantingFormat.SoilWaterSwitch.state; } }
        [Unit("mm")]
        public double MinSoilWaterTopLayer { get { return PlantingFormat.SoilWaterSwitch.MinSoilWaterRatio; } }
        [Unit("mm")]
        public double MaxSoilWaterTopLayer { get { return PlantingFormat.SoilWaterSwitch.MaxSoilWaterRatio; } }
        public double SoilWaterReqToPlant { get { return PlantingFormat.SoilWaterSwitch.AvailSWAtPlanting; } }
        [Unit("mm")]
        public double DepthToSumPlantingWater { get { return PlantingFormat.SoilWaterSwitch.SoilDepthToSumPlantingSW; } set { } }
        public int SowingDelay { get { return PlantingFormat.RainfallSwitch.SowingDelay; } }
        public Sequence PlantingSequence { get { return PlantingFormat.PlantingDates; } }                           // The rate of removal of plant residues from the soil surface by decay. Fraction of current plant/crop residues that decay each day. Plant residues on the soil surface are used in calculation of soil evaporation, runoff and erosion.
        public bool WaterLoggingSwitch { get { return Waterlogging.state; } }
        public double WaterLoggingFactor1 { get { return Waterlogging.WaterLoggingFactor1; } }
        public double WaterLoggingFactor2 { get { return Waterlogging.WaterLoggingFactor2; } }
        public bool RatoonSwitch { get { return PlantingFormat.RatoonCrop.state; } }
        public int NumberOfRatoons { get { return PlantingFormat.RatoonCrop.RatoonCount; } }
        public double ScalingFactorForRatoons { get { return PlantingFormat.RatoonCrop.RatoonScaleFactor; } }

        //TODO: unmatched
        //MaxResidueLoss, WatStressForDeath
        
        public double MaximumResidueCover { get; set; }
        

        public LAIVegObjectDataModel() { }
    }

    public class LAIVegObjectController : VegObjectController
    {
        public LAIVegObjectDataModel dataModel { get; set; }

        public static int UNCONTROLLED = 0;
        public static int OPPORTUNITY = 1;
        public static int INCROPORDER = 2;

        public double heat_units { get; set; }
        public double heat_unit_index { get; set; }
        public int LastPlantYear { get; set; }
        //public bool TodayIsPlantDay { get; set; }
        public double dhrlt { get; set; }
        public double hrltp { get; set; }
        public double hufp { get; set; }
        public bool AllowMultiPlanting { get; set; }
        public double decompdays { get; set; }
        public double LAICurveY1active { get; set; }
        public double LAICurveY2active { get; set; }
        public int days_since_fallow { get; set; }
        public double max_calc_lai { get; set; }
        public double lai { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public LAIVegObjectController() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sim"></param>
        public LAIVegObjectController(Simulation sim) : base(sim)
        {
            predefined_residue = false;
            //TodayIsPlantDay = false;
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Initialise()
        {
            base.Initialise();
            Scurve();
            heat_unit_index = 0;
            hrltp = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        public void SimulateCrop()
        {
            if (ExistsInTheGround)
            {
                ++days_since_planting;
                CalculateTranspiration();

                if (CheckCropSurvives())
                {
                    sim.UpdateManagementEventHistory(ManagementEvent.meCropGrowing, sim.VegetationController.GetCropIndex(this));
                    if (TodayIsPlantDay) //remove this once debugging is done
                    {

                        lai = 0.02;       // this is here just to replicate the old code... see Brett about it.
                        //TodayIsPlantDay = false;
                    }

                    if (!ReadyToHarvest())
                    {
                        RecordCropStage();

                        CalculateGrowthStressFactors();

                        CalculateLeafAreaIndex();
                        CalculateBioMass();
                        CalculateRootGrowth();
                        //	lai=0;
                    }
                    else
                        HarvestTheCrop();
                }
                else
                    SimulateCropDeath();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override double GetPotentialSoilEvaporation()
        {
            if (sim.ModelOptionsController.in_UsePERFECTSoilEvapFn)
            {
                if (lai < 0.3)
                    return sim.out_PanEvap_mm * (1.0 - green_cover);
            }
            return sim.out_PanEvap_mm * (1.0 - crop_cover);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool IsSequenceCorrect()
        {
            //if the cropindex is less than 2, the this is either the current crop or the next crop.
            //int index=sim.SortedCropList.IndexOf(this);
            int index = 0;
            for (int i = 0; i < 10; i++)
            {
                if (sim.VegetationController.SortedCropList[i] == this)
                {
                    index = i;
                    break;
                }

            }

            if (index == 2)
                return (dataModel.RotationFormat != INCROPORDER);
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool DoesCropMeetSowingCriteria()
        {

            if (dataModel.PlantingRulesOptions == (int)PlantingRules.prFixedAnualPlaning)
            {
                if (dataModel.PlantDate.MatchesDate(sim.today))
                    return true;
            }
            else if (dataModel.PlantingRulesOptions == (int)PlantingRules.prPlantInWindow)
            {
                // run ALL planting tests up front before testing results so that results
                // can be added to the annotations on the time-series charts.
                bool satisifies_window_conditions = SatisifiesWindowConditions();
                bool satisifies_fallow_conditions = SatisifiesFallowConditions();
                bool satisifies_planting_rain_conditions = SatisifiesPlantingRainConditions();
                bool satisifies_soil_water_conditions = SatisifiesSoilWaterConditions();
                bool satisifies_MultiPlantInWindow = SatisifiesMultiPlantInWindow();
                if (satisifies_window_conditions && satisifies_MultiPlantInWindow)
                {
                    if (satisifies_fallow_conditions)
                    {
                        if (satisifies_planting_rain_conditions && satisifies_soil_water_conditions)
                            return true;
                    }
                    else if (dataModel.ForcePlantingAtEndOfWindow)
                    {
                        if (!HasAlreadyPlantedInThisWindow())
                        {
                            DateTime endplantingdate = new DateTime(sim.today.Year, dataModel.PlantingWindowEndDate.Month, dataModel.PlantingWindowEndDate.Day);
                            return (sim.today == endplantingdate);
                        }
                        return false;
                    }
                    else
                        ++missed_rotation_count;
                }
            }
            else if (dataModel.PlantingRulesOptions == (int)PlantingRules.prPlantFromSequenceFile)
            {
                return dataModel.PlantingSequence.ContainsDate(sim.today);
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool SatisifiesMultiPlantInWindow()
        {
            if (!dataModel.MultiPlantInWindow && LastSowingDate != DateUtilities.NULLDATE)
            {
                return !HasAlreadyPlantedInThisWindow();
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool HasAlreadyPlantedInThisWindow()
        {
            //Note there was a possible error here in previous version where the wrong year could have been used.
            return DateUtilities.isDateInWindow(LastSowingDate, dataModel.PlantingWindowStartDate, dataModel.PlantingWindowEndDate);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool SatisifiesWindowConditions()
        {
            bool result = DateUtilities.isDateInWindow(sim.today, dataModel.PlantingWindowStartDate, dataModel.PlantingWindowEndDate);
            if (result)
                sim.UpdateManagementEventHistory(ManagementEvent.meInPlantingWindow, sim.VegetationController.GetCropIndex(this));
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool SatisifiesPlantingRainConditions()
        {
            bool result = true;
            if (dataModel.PlantingRainSwitch)
            {
                int actual_sowing_delay = dataModel.SowingDelay;
                double sumrain = 0;
                int index;
                int count_rainfreedays = 0;
                int max = 3 * dataModel.SowingDelay;
                for (int i = 0; i < max; ++i)
                {
                    index = sim.climateindex - i;
                    if (index >= 0)
                    {
                        if ((sim.Rainfall)[index] < 5)
                            ++count_rainfreedays;
                    }
                    if (count_rainfreedays == dataModel.SowingDelay)
                    {
                        actual_sowing_delay = i;
                        i = max;
                    }
                }
                if (count_rainfreedays == dataModel.SowingDelay)
                {
                    int fallow_planting_rain = (int)sim.SumRain(dataModel.RainfallSummationDays, actual_sowing_delay);
                    result = (fallow_planting_rain > dataModel.RainfallPlantingThreshold);
                }
                else
                    result = false;
                if (result)
                    sim.UpdateManagementEventHistory(ManagementEvent.meMeetsRainfallPlantCritera, sim.VegetationController.GetCropIndex(this));
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool SatisifiesFallowConditions()
        {
            bool result = true;
            if (dataModel.FallowSwitch)
            {
                result = (sim.VegetationController.days_since_harvest >= dataModel.MinimumFallowPeriod);
                if (result)
                    sim.UpdateManagementEventHistory(ManagementEvent.meMeetsDaysSinceHarvestPlantCritera, sim.VegetationController.GetCropIndex(this));
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool SatisifiesSoilWaterConditions()
        {
            bool result = true;
            if (dataModel.SoilWaterSwitch)
            {
                double SumSW = 0.0;
                for (int i = 0; i < sim.in_LayerCount; ++i)
                {
                    if (sim.depth[i + 1] - sim.depth[i] > 0)
                    {
                        if (dataModel.DepthToSumPlantingWater > sim.depth[sim.in_LayerCount])
                        {
                            dataModel.DepthToSumPlantingWater = sim.depth[sim.in_LayerCount];
                        }
                        if (sim.depth[i + 1] < dataModel.DepthToSumPlantingWater)
                        {
                            SumSW += sim.SoilWater_rel_wp[i];
                        }
                        if (sim.depth[i] < dataModel.DepthToSumPlantingWater && sim.depth[i + 1] > dataModel.DepthToSumPlantingWater)
                        {
                            SumSW += sim.SoilWater_rel_wp[i] * (dataModel.DepthToSumPlantingWater - sim.depth[i]) / (sim.depth[i + 1] - sim.depth[i]);
                        }
                        if (!MathTools.DoublesAreEqual(sim.DrainUpperLimit_rel_wp[i], 0))
                        {
                            sim.mcfc[i] = Math.Max(sim.SoilWater_rel_wp[i] / sim.DrainUpperLimit_rel_wp[i], 0.0);
                        }
                        else
                        {
                            sim.mcfc[i] = 0;
                        }
                    }
                    else
                    {
                        SumSW = 0;
                        sim.mcfc[i] = 0;
                        MathTools.LogDivideByZeroError("SatisifiesSoilWaterConditions", "sim.depth[i+1]-sim.depth[i]", "SumSW");
                    }
                }
                result = (SumSW > dataModel.SoilWaterReqToPlant && sim.mcfc[0] >= dataModel.MinSoilWaterTopLayer && sim.mcfc[0] <= dataModel.MaxSoilWaterTopLayer);

                if (result)
                {
                    sim.UpdateManagementEventHistory(ManagementEvent.meMeetsSoilWaterPlantCritera, sim.VegetationController.GetCropIndex(this));
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool IsCropUnderMaxContinuousRotations()
        {
            if (dataModel.PlantingRulesOptions != (int)PlantingRules.prPlantFromSequenceFile && dataModel.RotationFormat != UNCONTROLLED)
            {
                if (FirstRotationDate != DateUtilities.NULLDATE)
                {
                    return (rotation_count + missed_rotation_count < dataModel.MaxRotationCount);
                }

            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool HasCropHadSufficientContinuousRotations()
        {

            if (dataModel.PlantingRulesOptions != (int)PlantingRules.prPlantFromSequenceFile && dataModel.RotationFormat != UNCONTROLLED)
            {
                if (FirstRotationDate != DateUtilities.NULLDATE)
                    return (rotation_count + missed_rotation_count >= dataModel.MinRotationCount);
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="today"></param>
        /// <returns></returns>
        public override bool HasCropBeenAbsentForSufficientYears(DateTime today)
        {
            if (dataModel.PlantingRulesOptions != (int)PlantingRules.prPlantFromSequenceFile && dataModel.RotationFormat != UNCONTROLLED)
            {
                if (LastHarvestDate != DateUtilities.NULLDATE)
                {
                    int months_since_last_sow = DateUtilities.MonthsBetween(today, LastHarvestDate);
                    return (months_since_last_sow >= dataModel.RestPeriodAfterChangingCrops);
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public new void Plant()
        {
            base.Plant();
            sim.VegetationController.CurrentCrop.ResetCover();
            heat_unit_index = 0;
            heat_units = 0;
            max_calc_lai = 0;
            hufp = 0;
            killdays = 0;
            ++rotation_count;
            //TodayIsPlantDay = true;
            if (sim.ModelOptionsController.in_ResetSoilWaterAtPlanting)
            {
                for (int i = 0; i < sim.in_LayerCount; ++i)
                    sim.SoilWater_rel_wp[i] = (sim.ModelOptionsController.in_ResetValueForSWAtPlanting_pc / 100.0) * sim.DrainUpperLimit_rel_wp[i];
            }
            sim.FManagementEvent = ManagementEvent.mePlanting;
            sim.UpdateManagementEventHistory(ManagementEvent.mePlanting, sim.VegetationController.GetCropIndex(this));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override double GetTotalCover()
        {
            total_cover = Math.Min(1.0, crop_cover + residue_cover * (1 - crop_cover));
            out_TotalCover_pc = total_cover * 100.0;
            return total_cover;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CheckCropSurvives()
        {
            if (sim.ModelOptionsController.in_IgnoreCropKill == false)
            {
                if (crop_stage <= 2.0)
                {
                    if (out_WaterStressIndex <= dataModel.SWPropForNoStress)
                        ++killdays;
                    else
                        killdays = 0;

                    if (killdays >= dataModel.DaysOfStressToDeath)
                        return false;
                }
                else
                    killdays = 0;
            }
            return true;
        }

        // ***********************************************************************
        // *  This subroutine calculates growth stress factors for todays_temp   *
        // *  and water                                                          *
        // ***********************************************************************
        /// <summary>
        /// 
        /// </summary>
        public void CalculateGrowthStressFactors()
        {
            // ******************************
            // *  Temperature stress index  *
            // ******************************
            // [Equation 2.235] from EPIC

            double ratio;
            if (!MathTools.DoublesAreEqual(dataModel.OptTemp, dataModel.BaseTemp))
                ratio = (sim.temperature - dataModel.BaseTemp) / (dataModel.OptTemp - dataModel.BaseTemp);
            else
            {
                ratio = 1;
                //dont log error for this one
            }
            out_TempStressIndex = Math.Sin(0.5 * Math.PI * ratio);
            out_TempStressIndex = Math.Max(out_TempStressIndex, 0.0);
            out_TempStressIndex = Math.Min(out_TempStressIndex, 1.0);

            // ************************
            // *  Water stress index  *
            // ************************

            out_WaterStressIndex = 1.0;
            if (out_PotTranspiration_mm > 0.0)
                out_WaterStressIndex = total_transpiration / out_PotTranspiration_mm;

            out_WaterStressIndex = Math.Max(out_WaterStressIndex, 0.0);
            out_WaterStressIndex = Math.Min(out_WaterStressIndex, 1.0);

            // *************************************
            // *  calculate minimum stress factor  *
            // *************************************
            out_GrowthRegulator = 1;
            out_GrowthRegulator = Math.Min(out_GrowthRegulator, out_TempStressIndex);
            out_GrowthRegulator = Math.Min(out_GrowthRegulator, out_WaterStressIndex);
            out_GrowthRegulator = MathTools.CheckConstraints(out_GrowthRegulator, 1.0, 0);
        }

        // *********************************************************************
        // *  This subroutine fits an s curve to two points.  It was derived   *
        // *  from EPIC3270.                                                   *
        // *********************************************************************
        /// <summary>
        /// 
        /// </summary>
        public void Scurve()
        {
            if (MathTools.DoublesAreEqual(dataModel.PercentOfMaxLai1, 1)) dataModel.PercentOfMaxLai1 = 0.99999;
            if (MathTools.DoublesAreEqual(dataModel.PercentOfMaxLai2, 1)) dataModel.PercentOfMaxLai2 = 0.99999;
            if (dataModel.PercentOfMaxLai1 > 0 && dataModel.PercentOfMaxLai2 > 0)
            {
                double value1 = dataModel.PercentOfGrowSeason1 / dataModel.PercentOfMaxLai1 - dataModel.PercentOfGrowSeason1;
                double value2 = dataModel.PercentOfGrowSeason2 / dataModel.PercentOfMaxLai2 - dataModel.PercentOfGrowSeason2;
                if (!MathTools.DoublesAreEqual(value1, 0) && !MathTools.DoublesAreEqual(value2, 0))
                {
                    double x = Math.Log(value1);
                    LAICurveY2active = (x - Math.Log(value2)) / (dataModel.PercentOfGrowSeason2 - dataModel.PercentOfGrowSeason1);
                    LAICurveY1active = x + dataModel.PercentOfGrowSeason1 * LAICurveY2active;
                }
                else
                    MathTools.LogDivideByZeroError("Scurve", "in_LAICurveX1/in_LAICurveY1-in_LAICurveX1 or in_LAICurveX2/in_LAICurveY2-in_LAICurveX2", "LAI Curves - Taking Logs");
            }
            else
            {
                MathTools.LogDivideByZeroError("Scurve", "in_LAICurveY1 or in_LAICurveY2", "LAICurveY2active or LAICurveY1active");
            }
        }

        // ****************************************************************************
        // *  This subroutine calculates leaf area index using the major              *
        // *  functions from the EPIC model                                           *
        // ****************************************************************************
        /// <summary>
        /// 
        /// </summary>
        public void CalculateLeafAreaIndex()
        {
            double HUF;   //Heat Unit Factor
            double dHUF;  //daily change in Heat Unit Factor
            double dlai;

            // ***************************
            // *  accumulate heat units  *
            // ***************************

            // accumulate heat units
            heat_units = heat_units + Math.Max(sim.temperature - dataModel.BaseTemp, 0.0);

            // caluclate heat unit index [Equation 2.191] from EPIC
            if (!MathTools.DoublesAreEqual(dataModel.DegreeDaysPlantToHarvest, 0))
                heat_unit_index = heat_units / dataModel.DegreeDaysPlantToHarvest;
            else
            {
                heat_unit_index = 1;
                MathTools.LogDivideByZeroError("CalculateLeafAreaIndex", "in_DegreeDaysToMaturity_days", "heat_unit_index");

            }

            // ***************************
            // *  calculate leaf growth  *
            // ***************************

            if (heat_unit_index < dataModel.PropGrowSeaForMaxLai)
            {
                // heat unit factor, [Equation 2.198] from EPIC
                double denom = (heat_unit_index + Math.Exp(LAICurveY1active - LAICurveY2active * heat_unit_index));
                if (!MathTools.DoublesAreEqual(denom, 0))
                    HUF = heat_unit_index / denom;
                else
                {
                    HUF = 0;
                    //not sure I should log this one
                }
                dHUF = HUF - hufp;
                hufp = HUF;

                // leaf area index [Equation 2.197] from EPIC
                //        Eqn 2.197 originally stated that
                //
                //        dlai = dHUF * MaxLAI * (1.0-Math.Exp(5.0*(laip-MaxLAI)))* sqrt(reg)
                //
                // This function NEVER allows lai to achieve MaxLAI under no stress
                // conditions due to the exponential term.  This term was removed.  Therefore,
                // lai development is governed by the S-Curve, Max lai, and stress factors
                // only.


                if (sim.ModelOptionsController.in_UsePERFECTLeafAreaFn)
                    dlai = dHUF * dataModel.PotMaxLAI * Math.Sqrt(out_GrowthRegulator);
                else
                    dlai = dHUF * dataModel.PotMaxLAI * out_GrowthRegulator;

                lai = lai + dlai;

                // store maximum lai
                if (lai > max_calc_lai)
                    max_calc_lai = lai;
            }

            // ******************************
            // *  calculate leaf senesence  *
            // ******************************

            if (heat_unit_index >= dataModel.PropGrowSeaForMaxLai && heat_unit_index <= 1)
            {
                if (dataModel.SenesenceCoef > 0)
                {
                    // leaf senesence [Equation 2.199] from EPIC
                    if (!MathTools.DoublesAreEqual(1.0 - dataModel.PropGrowSeaForMaxLai, 0))
                        lai = max_calc_lai * Math.Pow((1.0 - heat_unit_index) / (1.0 - dataModel.PropGrowSeaForMaxLai), dataModel.SenesenceCoef);
                    else
                        lai = 0;
                    lai = Math.Max(lai, 0);
                }
                else lai = 0;
            }
        }

        // ********************************************************************
        // *  Subroutine calculates biomass using EPIC type functions         *
        // ********************************************************************
        /// <summary>
        /// 
        /// </summary>
        public void CalculateBioMass()
        {
            double rad, par, hrlt, dhrlt;
            // *********************************
            // *  intercepted radiation (PAR)  *
            // *********************************
            // Assumes PAR is 50% of solar radiation
            //         Extinction Coefficient = 0.65
            rad = sim.out_SolarRad_MJ_per_m2_per_day;
            par = 0.5 * rad * (1.0 - Math.Exp(-0.65 * lai));

            // **********************
            // *  daylength factor  *
            // **********************
            // daylength factor from PERFECT
            hrlt = GetDayLength();
            dhrlt = hrlt - hrltp;
            if (hrltp < 0.01)
                dhrlt = 0.0;
            hrltp = hrlt;


            double rue = dataModel.RadUseEffic;
            double effectiverue = rue;

            if (dataModel.WaterLoggingSwitch && IsWaterLogged())
                effectiverue = rue * dataModel.WaterLoggingFactor2;

            // **************************
            // *  biomass accumulation  *
            // **************************
            // [Equation 2.193] from EPIC
            if (sim.ModelOptionsController.in_UsePERFECTDryMatterFn)
            {
                dry_matter += out_GrowthRegulator * par * effectiverue * Math.Pow(1.0 + dhrlt, 3.0);
                out_DryMatter_kg_per_ha = dry_matter * 10.0;
            }

            else
            {
                par = 0.5 * rad;
                dry_matter += effectiverue * par * out_WaterStressIndex * out_TempStressIndex * green_cover;
                out_DryMatter_kg_per_ha = dry_matter * 10.0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IsWaterLogged()
        {
            int i = 1;
            return (sim.SoilWater_rel_wp[i] > sim.DrainUpperLimit_rel_wp[i]);
        }


        // *******************************************************************
        // *  This subroutine calculates day length from latitude and day    *
        // *  number in the year.                                            *
        // *******************************************************************

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetDayLength()
        {
            double alat = (!MathTools.DoublesAreEqual(sim.ClimateController.Latitude, 0) ? sim.Latitude : -27);
            double sund = -2.2;
            int dayno = (sim.today - new DateTime(sim.today.Year, 1, 1 + 1)).Days;
            double theta = 0.0172142 * (dayno - 172.0);
            double sdcln = 0.00678 + 0.39762 * Math.Cos(theta) + 0.00613 * Math.Sin(theta) - 0.00661 * Math.Cos(2.0 * theta) - 0.00159 * Math.Sin(2.0 * theta);
            double dcln = Math.Asin(sdcln);
            double rlat = alat * 0.0174533;
            double dnlat = 0;
            if (!MathTools.DoublesAreEqual(Math.Cos(rlat), 0) && !MathTools.DoublesAreEqual(Math.Cos(dcln), 0))
                dnlat = -(Math.Sin(rlat) / Math.Cos(rlat)) * (Math.Sin(dcln) / Math.Cos(dcln));
            else
            {
                MathTools.LogDivideByZeroError("GetDayLength", "Math.Cos(rlat) or Math.Cos(dcln)", "dnlat");
            }
            double rsund = sund * 0.0174533;
            double twif = Math.Cos(rlat) * Math.Cos(dcln);
            double atwil = 0;
            if (!MathTools.DoublesAreEqual(twif, 0))
                atwil = Math.Sin(rsund) / twif;
            else
            {
                MathTools.LogDivideByZeroError("GetDayLength", "twif", "atwil");
            }
            double htwil = Math.Acos(atwil + dnlat);
            return 7.639437 * htwil;
        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculateRootGrowth()
        {
            out_RootDepth_mm = out_RootDepth_mm + dataModel.DailyRootGrowth;
            out_RootDepth_mm = MathTools.CheckConstraints(out_RootDepth_mm, MaximumRootDepth, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ReadyToHarvest()
        {
            return (heat_unit_index >= 1);
        }
        /// <summary>
        /// 
        /// </summary>
        public override void ResetCropParametersAfterHarvest()
        {
            if (today_is_harvest_day) //if harvest day
            {
                today_is_harvest_day = false;
                yield = 0;
                out_Yield_t_per_ha = 0;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public void HarvestTheCrop()
        {
            LastHarvestDate = sim.today;
            today_is_harvest_day = true;
            sim.FManagementEvent = ManagementEvent.meHarvest;
            sim.UpdateManagementEventHistory(ManagementEvent.meHarvest, sim.VegetationController.GetCropIndex(this));
            ++number_of_harvests;

            //	sim.days_since_harvest=0;    //should this also get reset at crop death
            //    soil_water_at_harvest=sim.total_soil_water;
            // ++number_of_fallows;
            //	CropStatus=csInFallow;
            yield = dataModel.HarvestIndex * dry_matter * 10.0;
            out_Yield_t_per_ha = yield / 1000.0;
            out_ResidueAmount_kg_per_ha = out_ResidueAmount_kg_per_ha + (dry_matter - yield / 10.0) * 0.95 * 10.0;

            //*****************

            green_cover = 0;
            //*****************
            sim.VegetationController.CalculateTotalResidue();

            cumulative_yield += yield;
            ResetParametersForEndOfCrop();

        }
        /// <summary>
        /// 
        /// </summary>
        public void SimulateCropDeath()
        {

            CalculateGrowthStressFactors();
            CalculateLeafAreaIndex();
            CalculateBioMass();
            CalculateRootGrowth();
            //    CalculateBioMass();//only needs to be here to replicate previous results
            // CalculateResidue();//only needs to be here to replicate previous results
            out_ResidueAmount_kg_per_ha = out_ResidueAmount_kg_per_ha + (dry_matter - yield) * 0.95 * 10.0;
            sim.VegetationController.CalculateTotalResidue();
            out_WaterStressIndex = 1.0;
            ++number_of_crops_killed;
            yield = 0;
            out_Yield_t_per_ha = 0;
            ResetParametersForEndOfCrop();

            //crop_stage=0;
            //dry_matter=0;
            //	soil_water_at_harvest=sim.total_soil_water;

            //CropStatus=csInFallow;
            //	crop_cover=0;
            //	total_transpiration=0;
            //	for(int i=0;i<sim.LayerCount;++i)
            //		sim.layer_transpiration[i]=0;
            //   lai=0;

        }
        /// <summary>
        /// 
        /// </summary>
        public void ResetParametersForEndOfCrop()
        {
            CropStatus = ModelControllers.CropStatus.csInFallow;
            ++number_of_fallows;
            soil_water_at_harvest = sim.total_soil_water;
            sim.VegetationController.days_since_harvest = 0;    //should this also get reset at crop death
            days_since_planting = 0;
            total_transpiration = 0;
            crop_cover = 0;
            crop_cover_percent = 0;
            crop_stage = 0;
            lai = 0;
            for (int i = 0; i < sim.in_LayerCount; ++i)
                sim.layer_transpiration[i] = 0;
            heat_unit_index = 0;
            out_RootDepth_mm = 0;
            green_cover = 0;
            out_GrowthRegulator = 0;
            dry_matter = 0;
            out_DryMatter_kg_per_ha = 0;
            out_PotTranspiration_mm = 0;

        }

        /// <summary>
        /// 
        /// </summary>
        public void RecordCropStage()
        {
            //   CropAnthesis=false;
            if (heat_unit_index <= dataModel.PropGrowSeaForMaxLai && !MathTools.DoublesAreEqual(dataModel.PropGrowSeaForMaxLai, 0))
            {
                crop_stage = heat_unit_index * 2.0 / dataModel.PropGrowSeaForMaxLai;
                //anth=0;
            }
            else
            {
                if (MathTools.DoublesAreEqual(dataModel.PropGrowSeaForMaxLai, 0))
                    MathTools.LogDivideByZeroError("RecordCropStage", "in_PropSeasonForMaxLAI", "crop_stage");
                //if(anth==0)
                //		{
                //			CropAnthesis=true;
                //			anth=1;
                //		}
                if (!MathTools.DoublesAreEqual(dataModel.PropGrowSeaForMaxLai, 1))
                    crop_stage = 2 + (heat_unit_index - dataModel.PropGrowSeaForMaxLai) / (1.0 - dataModel.PropGrowSeaForMaxLai);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override double CalculatePotentialTranspiration()
        {

            // Calculate potential transpiration
            if (sim.ModelOptionsController.in_UsePERFECTGroundCovFn)
                green_cover = Math.Min(lai / 3.0, 1.0);
            else
            {
                if (lai > 0)
                    green_cover = 1 - Math.Exp(-0.55 * (lai + 0.1));  //  changed br on 9/12/2005
                else
                    green_cover = 0;
            }

            green_cover = Math.Max(0.0, green_cover);
            crop_cover = Math.Max(crop_cover, green_cover);
            double value = Math.Min(sim.out_PanEvap_mm * green_cover, sim.out_PanEvap_mm - sim.out_WatBal_SoilEvap_mm);
            if (value > sim.out_PanEvap_mm)
                value = sim.out_PanEvap_mm;

            out_GreenCover_pc = green_cover * 100.0;
            crop_cover_percent = crop_cover * 100.0;
            if (dataModel.WaterLoggingSwitch && IsWaterLogged())
                value = value * dataModel.WaterLoggingFactor1;

            return value;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool StillRequiresIrrigating()
        {
            return (heat_units < dataModel.PropGDDEnd / 100.0 * (double)(dataModel.DegreeDaysPlantToHarvest));
        }

        /// <summary>
        /// 
        /// </summary>
        public new void CalculateResidue()
        {
            if (sim.ModelOptionsController.in_UsePERFECTResidueFn)
                CalculateResidue_PERFECT();
            else
                CalculateResidue_BR();
            total_cover = GetTotalCover();
            out_TotalCover_pc = total_cover * 100.0;
            accumulated_residue += residue_cover * 100.0;
        }


        /// <summary>
        /// 
        /// </summary>
        public void CalculateResidue_BR()
        {
            int seriesindex = sim.climateindex;
            double rain_yesterday = (seriesindex > 0 ? (sim.Rainfall)[seriesindex - 1] : 0);
            double rain_daybeforeyesterday = (seriesindex > 1 ? (sim.Rainfall)[seriesindex - 2] : 0);

            double mi = 4.0 / 7.0 * (Math.Min(sim.effective_rain, 4) / 4.0
                        + Math.Min(rain_yesterday, 4) / 8.0
                        + Math.Min(rain_daybeforeyesterday, 4) / 16.0);  //moisture index
                                                                         //there is a minor problem here...doesn't take into consideration irrigation in the previous days.

            double ti = Math.Max(sim.temperature / 32.0, 0);                   // temperature index
            decompdays = Math.Min(Math.Min(mi, ti), 1);  //  min=0 days, max =1day

            // Will change this to a non-linear function in the near future. BR 14/09/2010

            out_ResidueAmount_kg_per_ha = Math.Max(0, out_ResidueAmount_kg_per_ha - out_ResidueAmount_kg_per_ha * dataModel.MaxResidueLoss / 100.0 * decompdays);
            if (!MathTools.DoublesAreEqual(dataModel.BiomassAtFullCover, 0))
                residue_cover = Math.Min(1.0, out_ResidueAmount_kg_per_ha / dataModel.BiomassAtFullCover);
            else
            {
                residue_cover = 0;
                MathTools.LogDivideByZeroError("CalculateResidue_BR", "in_BiomassAtFullCover", "residue_cover");
            }
            if (residue_cover < 0) residue_cover = 0;
            out_ResidueCover_pc = residue_cover * 100.0;

        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculateResidue_PERFECT()
        {

            // ************************************************************
            // *  Subroutine decays residue and calculates surface cover  *
            // ************************************************************
            //   Decay residue using Sallaway's functions
            if (CropStatus == ModelControllers.CropStatus.csInFallow)
            {
                if (days_since_fallow < 60)
                    out_ResidueAmount_kg_per_ha = Math.Max(0, out_ResidueAmount_kg_per_ha - 15);
                else if (days_since_fallow >= 60)
                    out_ResidueAmount_kg_per_ha = Math.Max(0, out_ResidueAmount_kg_per_ha - 3);
                days_since_fallow += 1;

            }
            else
                out_ResidueAmount_kg_per_ha = Math.Max(0, out_ResidueAmount_kg_per_ha - 15);
            //  Calculate proportion cover from residue weight
            //  using Sallaway type functions
            residue_cover = dataModel.MaximumResidueCover * (1.0 - Math.Exp(-1.0 * out_ResidueAmount_kg_per_ha / 1000.0));
            if (residue_cover < 0) residue_cover = 0;
            if (residue_cover > 1) residue_cover = 1;
            out_ResidueCover_pc = residue_cover * 100.0;
        }
    }
}
