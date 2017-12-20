using HowLeaky.CustomAttributes;
using HowLeaky.DataModels;
using HowLeaky.Tools.DataObjects;
using HowLeaky.Tools.Helpers;
using System;
using System.Xml.Serialization;

namespace HowLeaky.ModelControllers.Veg
{
    [XmlRoot("VegetationType")]
    public class LAIVegObjectDataModel : VegObjectInputDataModel
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
        public int PlantingRulesOptions { get { return PlantingFormat.index; } }
        public DayMonthData PlantingWindowStartDate { get { return PlantingFormat.StartPlantWindow; } }
        public DayMonthData PlantingWindowEndDate { get { return PlantingFormat.EndPlantWindow; } }
        public bool ForcePlantingAtEndOfWindow { get { return PlantingFormat.ForcePlanting.State; } }
        public bool MultiPlantInWindow { get { return PlantingFormat.MultiPlantInWindow.State; } }
        public int RotationFormat { get { return PlantingFormat.RotationOptions.index; } }
        public int MinRotationCount { get { return PlantingFormat.RotationOptions.MinContinuousRotations; } }
        public int MaxRotationCount { get { return PlantingFormat.RotationOptions.MaxContinuousRotations; } }
        public int RestPeriodAfterChangingCrops { get { return PlantingFormat.RotationOptions.MinYearsBetweenSowing; } }
        public bool FallowSwitch { get { return PlantingFormat.FallowSwitch.State; } }
        [Unit("days")]
        public int MinimumFallowPeriod { get { return PlantingFormat.FallowSwitch.MinFallowLength; } }
        public bool PlantingRainSwitch { get { return PlantingFormat.RainfallSwitch.State; } }
        [Unit("mm")]
        public double RainfallPlantingThreshold { get { return PlantingFormat.RainfallSwitch.PlantingRain; } }
        [Unit("days")]
        public int RainfallSummationDays { get { return PlantingFormat.RainfallSwitch.DaysToTotalRain; } }
        public bool SoilWaterSwitch { get { return PlantingFormat.SoilWaterSwitch.State; } }
        [Unit("mm")]
        public double MinSoilWaterTopLayer { get { return PlantingFormat.SoilWaterSwitch.MinSoilWaterRatio; } }
        [Unit("mm")]
        public double MaxSoilWaterTopLayer { get { return PlantingFormat.SoilWaterSwitch.MaxSoilWaterRatio; } }
        public double SoilWaterReqToPlant { get { return PlantingFormat.SoilWaterSwitch.AvailSWAtPlanting; } }
        [Unit("mm")]
        public double DepthToSumPlantingWater { get { return PlantingFormat.SoilWaterSwitch.SoilDepthToSumPlantingSW; } set { } }
        public int SowingDelay { get { return PlantingFormat.RainfallSwitch.SowingDelay; } }
        public Sequence PlantingSequence { get { return PlantingFormat.PlantingDates; } }                           // The rate of removal of plant residues from the soil surface by decay. Fraction of current plant/crop residues that decay each day. Plant residues on the soil surface are used in calculation of soil evaporation, runoff and erosion.
        public bool WaterLoggingSwitch { get { return Waterlogging.State; } }
        public double WaterLoggingFactor1 { get { return Waterlogging.WaterLoggingFactor1; } }
        public double WaterLoggingFactor2 { get { return Waterlogging.WaterLoggingFactor2; } }
        public bool RatoonSwitch { get { return PlantingFormat.RatoonCrop.State; } }
        public int NumberOfRatoons { get { return PlantingFormat.RatoonCrop.RatoonCount; } }
        public double ScalingFactorForRatoons { get { return PlantingFormat.RatoonCrop.RatoonScaleFactor; } }

        //TODO: unmatched
        //MaxResidueLoss, WatStressForDeath
        public double MaximumResidueCover { get; set; }

        public LAIVegObjectDataModel() { }
    }

    public class LAIVegObjectController : VegObjectController
    {
        public LAIVegObjectDataModel DataModel { get; set; }

        public static int UNCONTROLLED = 0;
        public static int OPPORTUNITY = 1;
        public static int INCROPORDER = 2;

        public int LastPlantYear { get; set; }
        public int DaysSinceFallow { get; set; }
        public bool AllowMultiPlanting { get; set; }
        public double HeatUnits { get; set; }
        public double HeatUnitIndex { get; set; }
        //public bool TodayIsPlantDay { get; set; }
        public double Dhrlt { get; set; }
        public double Hrltp { get; set; }
        public double Hufp { get; set; }
        public double Decompdays { get; set; }
        public double LAICurveY1active { get; set; }
        public double LAICurveY2active { get; set; }
        public double MaxCalcLAI { get; set; }
        public double LAI { get; set; }

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
            PredefinedResidue = false;
            //TodayIsPlantDay = false;
        }

        public LAIVegObjectController(Simulation sim, LAIVegObjectDataModel dataModel) : this(sim)
        {
            this.DataModel = dataModel;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public override void Initialise()
        {
            base.Initialise();
            Scurve();
            HeatUnitIndex = 0;
            Hrltp = 0;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void SimulateCrop()
        {
            if (ExistsInTheGround)
            {
                ++DaysSincePlanting;
                CalculateTranspiration();

                if (CheckCropSurvives())
                {
                    Sim.UpdateManagementEventHistory(ManagementEvent.CropGrowing, Sim.VegetationController.GetCropIndex(this));
                    if (TodayIsPlantDay) //remove this once debugging is done
                    {

                        LAI = 0.02;       // this is here just to replicate the old code... see Brett about it.
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
                    {
                        HarvestTheCrop();
                    }
                }
                else
                {
                    SimulateCropDeath();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override double GetPotentialSoilEvaporation()
        {
            if (Sim.ModelOptionsController.DataModel.UsePERFECTSoilEvapFn)
            {
                if (LAI < 0.3)
                {
                    return Sim.ClimateController.PanEvap * (1.0 - GreenCover);
                }
            }
            return Sim.ClimateController.PanEvap * (1.0 - CropCover);
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
                if (Sim.VegetationController.SortedCropList[i] == this)
                {
                    index = i;
                    break;
                }

            }

            if (index == 2)
            {
                return (DataModel.RotationFormat != INCROPORDER);
            }
            return true;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool DoesCropMeetSowingCriteria()
        {
            if (DataModel.PlantingRulesOptions == (int)EPlantingRules.FixedAnualPlaning)
            {
                if (DataModel.PlantDate.MatchesDate(Sim.Today))
                {
                    return true;
                }
            }
            else if (DataModel.PlantingRulesOptions == (int)EPlantingRules.PlantInWindow)
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
                        {
                            return true;
                        }
                    }
                    else if (DataModel.ForcePlantingAtEndOfWindow)
                    {
                        if (!HasAlreadyPlantedInThisWindow())
                        {
                            DateTime endplantingdate = new DateTime(Sim.Today.Year, DataModel.PlantingWindowEndDate.Month, DataModel.PlantingWindowEndDate.Day);
                            return (Sim.Today == endplantingdate);
                        }
                        return false;
                    }
                    else
                    {
                        ++MissedRotationCount;
                    }
                }
            }
            else if (DataModel.PlantingRulesOptions == (int)EPlantingRules.PlantFromSequenceFile)
            {
                return DataModel.PlantingSequence.ContainsDate(Sim.Today);
            }
            return false;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool SatisifiesMultiPlantInWindow()
        {
            if (!DataModel.MultiPlantInWindow && LastSowingDate != DateUtilities.NULLDATE)
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
            return DateUtilities.isDateInWindow(LastSowingDate, DataModel.PlantingWindowStartDate, DataModel.PlantingWindowEndDate);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool SatisifiesWindowConditions()
        {
            bool result = DateUtilities.isDateInWindow(Sim.Today, DataModel.PlantingWindowStartDate, DataModel.PlantingWindowEndDate);
            if (result)
            {
                Sim.UpdateManagementEventHistory(ManagementEvent.InPlantingWindow, Sim.VegetationController.GetCropIndex(this));
            }
            return result;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool SatisifiesPlantingRainConditions()
        {
            bool result = true;
            if (DataModel.PlantingRainSwitch)
            {
                int actualSowingDelay = DataModel.SowingDelay;
                double sumrain = 0;
                int index;
                int count_rainfreedays = 0;
                int max = 3 * DataModel.SowingDelay;
                for (int i = 0; i < max; ++i)
                {
                    index = Sim.Climateindex - i;
                    if (index >= 0)
                    {
                        if (Sim.ClimateController.Rain < 5) //if ((Sim.ClimateController.Rainfall)[index] < 5)
                        {
                            ++count_rainfreedays;
                        }
                    }
                    if (count_rainfreedays == DataModel.SowingDelay)
                    {
                        actualSowingDelay = i;
                        i = max;
                    }
                }
                if (count_rainfreedays == DataModel.SowingDelay)
                {
                    int fallow_planting_rain = (int)Sim.ClimateController.SumRain(DataModel.RainfallSummationDays, actualSowingDelay);
                    result = (fallow_planting_rain > DataModel.RainfallPlantingThreshold);
                }
                else
                {
                    result = false;
                }
                if (result)
                {
                    Sim.UpdateManagementEventHistory(ManagementEvent.MeetsRainfallPlantCritera, Sim.VegetationController.GetCropIndex(this));
                }
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
            if (DataModel.FallowSwitch)
            {
                result = (Sim.VegetationController.DaysSinceHarvest >= DataModel.MinimumFallowPeriod);
                if (result)
                {
                    Sim.UpdateManagementEventHistory(ManagementEvent.MeetsDaysSinceHarvestPlantCritera, Sim.VegetationController.GetCropIndex(this));
                }
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
            if (DataModel.SoilWaterSwitch)
            {
                double SumSW = 0.0;
                for (int i = 0; i < Sim.SoilController.LayerCount; ++i)
                {
                    if (Sim.SoilController.Depth[i + 1] - Sim.SoilController.Depth[i] > 0)
                    {
                        if (DataModel.DepthToSumPlantingWater > Sim.SoilController.Depth[Sim.SoilController.LayerCount])
                        {
                            DataModel.DepthToSumPlantingWater = Sim.SoilController.Depth[Sim.SoilController.LayerCount];
                        }
                        if (Sim.SoilController.Depth[i + 1] < DataModel.DepthToSumPlantingWater)
                        {
                            SumSW += Sim.SoilController.SoilWaterRelWP[i];
                        }
                        if (Sim.SoilController.Depth[i] < DataModel.DepthToSumPlantingWater && Sim.SoilController.Depth[i + 1] > DataModel.DepthToSumPlantingWater)
                        {
                            SumSW += Sim.SoilController.SoilWaterRelWP[i] * (DataModel.DepthToSumPlantingWater - Sim.SoilController.Depth[i]) / (Sim.SoilController.Depth[i + 1] - Sim.SoilController.Depth[i]);
                        }
                        if (!MathTools.DoublesAreEqual(Sim.SoilController.DrainUpperLimitRelWP[i], 0))
                        {
                            Sim.SoilController.MCFC[i] = Math.Max(Sim.SoilController.SoilWaterRelWP[i] / Sim.SoilController.DrainUpperLimitRelWP[i], 0.0);
                        }
                        else
                        {
                            Sim.SoilController.MCFC[i] = 0;
                        }
                    }
                    else
                    {
                        SumSW = 0;
                        Sim.SoilController.MCFC[i] = 0;
                        MathTools.LogDivideByZeroError("SatisifiesSoilWaterConditions", "sim.depth[i+1]-sim.depth[i]", "SumSW");
                    }
                }
                result = (SumSW > DataModel.SoilWaterReqToPlant && Sim.SoilController.MCFC[0] >= DataModel.MinSoilWaterTopLayer && Sim.SoilController.MCFC[0] <= DataModel.MaxSoilWaterTopLayer);

                if (result)
                {
                    Sim.UpdateManagementEventHistory(ManagementEvent.MeetsSoilWaterPlantCritera, Sim.VegetationController.GetCropIndex(this));
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
            if (DataModel.PlantingRulesOptions != (int)EPlantingRules.PlantFromSequenceFile && DataModel.RotationFormat != UNCONTROLLED)
            {
                if (FirstRotationDate != DateUtilities.NULLDATE)
                {
                    return (RotationCount + MissedRotationCount < DataModel.MaxRotationCount);
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

            if (DataModel.PlantingRulesOptions != (int)EPlantingRules.PlantFromSequenceFile && DataModel.RotationFormat != UNCONTROLLED)
            {
                if (FirstRotationDate != DateUtilities.NULLDATE)
                {
                    return (RotationCount + MissedRotationCount >= DataModel.MinRotationCount);
                }
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
            if (DataModel.PlantingRulesOptions != (int)EPlantingRules.PlantFromSequenceFile && DataModel.RotationFormat != UNCONTROLLED)
            {
                if (LastHarvestDate != DateUtilities.NULLDATE)
                {
                    int months_since_last_sow = DateUtilities.MonthsBetween(today, LastHarvestDate);
                    return (months_since_last_sow >= DataModel.RestPeriodAfterChangingCrops);
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
            Sim.VegetationController.CurrentCrop.ResetCover();
            HeatUnitIndex = 0;
            HeatUnits = 0;
            MaxCalcLAI = 0;
            Hufp = 0;
            KillDays = 0;
            ++RotationCount;
            //TodayIsPlantDay = true;
            if (Sim.ModelOptionsController.DataModel.ResetSoilWaterAtPlanting)
            {
                for (int i = 0; i < Sim.SoilController.LayerCount; ++i)
                {
                    Sim.SoilController.SoilWaterRelWP[i] = (Sim.ModelOptionsController.DataModel.ResetValueForSWAtPlanting / 100.0) * Sim.SoilController.DrainUpperLimitRelWP[i];
                }
            }
            Sim.FManagementEvent = ManagementEvent.Planting;
            Sim.UpdateManagementEventHistory(ManagementEvent.Planting, Sim.VegetationController.GetCropIndex(this));
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override double GetTotalCover()
        {
            TotalCover = Math.Min(1.0, CropCover + ResidueCover * (1 - CropCover));
            Output.TotalCover = TotalCover * 100.0;
            return TotalCover;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CheckCropSurvives()
        {
            if (Sim.ModelOptionsController.DataModel.IgnoreCropKill == false)
            {
                if (CropStage <= 2.0)
                {
                    if (Output.WaterStressIndex <= DataModel.SWPropForNoStress)
                    {
                        ++KillDays;
                    }
                    else
                    {
                        KillDays = 0;
                    }
                    if (KillDays >= DataModel.DaysOfStressToDeath)
                    {
                        return false;
                    }
                }
                else
                {
                    KillDays = 0;
                }
            }
            return true;
        }
        
        /// <summary>
        /// This subroutine calculates growth stress factors for todays_temp
        /// and water
        /// </summary>
        public void CalculateGrowthStressFactors()
        {
            // ******************************
            // *  Temperature stress index  *
            // ******************************
            // [Equation 2.235] from EPIC

            double ratio;
            if (!MathTools.DoublesAreEqual(DataModel.OptTemp, DataModel.BaseTemp))
            {
                ratio = (Sim.ClimateController.Temperature - DataModel.BaseTemp) / (DataModel.OptTemp - DataModel.BaseTemp);
            }
            else
            {
                ratio = 1;
                //dont log error for this one
            }
            Output.TempStressIndex = Math.Sin(0.5 * Math.PI * ratio);
            Output.TempStressIndex = Math.Max(Output.TempStressIndex, 0.0);
            Output.TempStressIndex = Math.Min(Output.TempStressIndex, 1.0);

            // ************************
            // *  Water stress index  *
            // ************************

            Output.WaterStressIndex = 1.0;
            if (Output.PotTranspiration > 0.0)
            {
                Output.WaterStressIndex = TotalTranspiration / Output.PotTranspiration;
            }
            Output.WaterStressIndex = Math.Max(Output.WaterStressIndex, 0.0);
            Output.WaterStressIndex = Math.Min(Output.WaterStressIndex, 1.0);

            // *************************************
            // *  calculate minimum stress factor  *
            // *************************************
            Output.GrowthRegulator = 1;
            Output.GrowthRegulator = Math.Min(Output.GrowthRegulator, Output.TempStressIndex);
            Output.GrowthRegulator = Math.Min(Output.GrowthRegulator, Output.WaterStressIndex);
            Output.GrowthRegulator = MathTools.CheckConstraints(Output.GrowthRegulator, 1.0, 0);
        }
        
        /// <summary>
        /// This subroutine fits an s curve to two points.  It was derived   
        /// from EPIC3270.
        /// </summary>
        public void Scurve()
        {
            if (MathTools.DoublesAreEqual(DataModel.PercentOfMaxLai1, 1)) DataModel.PercentOfMaxLai1 = 0.99999;
            if (MathTools.DoublesAreEqual(DataModel.PercentOfMaxLai2, 1)) DataModel.PercentOfMaxLai2 = 0.99999;
            if (DataModel.PercentOfMaxLai1 > 0 && DataModel.PercentOfMaxLai2 > 0)
            {
                double value1 = DataModel.PercentOfGrowSeason1 / DataModel.PercentOfMaxLai1 - DataModel.PercentOfGrowSeason1;
                double value2 = DataModel.PercentOfGrowSeason2 / DataModel.PercentOfMaxLai2 - DataModel.PercentOfGrowSeason2;
                if (!MathTools.DoublesAreEqual(value1, 0) && !MathTools.DoublesAreEqual(value2, 0))
                {
                    double x = Math.Log(value1);
                    LAICurveY2active = (x - Math.Log(value2)) / (DataModel.PercentOfGrowSeason2 - DataModel.PercentOfGrowSeason1);
                    LAICurveY1active = x + DataModel.PercentOfGrowSeason1 * LAICurveY2active;
                }
                else
                {
                    MathTools.LogDivideByZeroError("Scurve", "in_LAICurveX1/in_LAICurveY1-in_LAICurveX1 or in_LAICurveX2/in_LAICurveY2-in_LAICurveX2", "LAI Curves - Taking Logs");
                }
            }
            else
            {
                MathTools.LogDivideByZeroError("Scurve", "in_LAICurveY1 or in_LAICurveY2", "LAICurveY2active or LAICurveY1active");
            }
        }
        
        /// <summary>
        /// This subroutine calculates leaf area index using the major             
        /// functions from the EPIC model
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
            HeatUnits = HeatUnits + Math.Max(Sim.ClimateController.Temperature - DataModel.BaseTemp, 0.0);

            // caluclate heat unit index [Equation 2.191] from EPIC
            if (!MathTools.DoublesAreEqual(DataModel.DegreeDaysPlantToHarvest, 0))
            {
                HeatUnitIndex = HeatUnits / DataModel.DegreeDaysPlantToHarvest;
            }
            else
            {
                HeatUnitIndex = 1;
                MathTools.LogDivideByZeroError("CalculateLeafAreaIndex", "in_DegreeDaysToMaturity_days", "heat_unit_index");

            }

            // ***************************
            // *  calculate leaf growth  *
            // ***************************

            if (HeatUnitIndex < DataModel.PropGrowSeaForMaxLai)
            {
                // heat unit factor, [Equation 2.198] from EPIC
                double denom = (HeatUnitIndex + Math.Exp(LAICurveY1active - LAICurveY2active * HeatUnitIndex));
                if (!MathTools.DoublesAreEqual(denom, 0))
                {
                    HUF = HeatUnitIndex / denom;
                }
                else
                {
                    HUF = 0;
                    //not sure I should log this one
                }
                dHUF = HUF - Hufp;
                Hufp = HUF;

                // leaf area index [Equation 2.197] from EPIC
                //        Eqn 2.197 originally stated that
                //
                //        dlai = dHUF * MaxLAI * (1.0-Math.Exp(5.0*(laip-MaxLAI)))* sqrt(reg)
                //
                // This function NEVER allows lai to achieve MaxLAI under no stress
                // conditions due to the exponential term.  This term was removed.  Therefore,
                // lai development is governed by the S-Curve, Max lai, and stress factors
                // only.


                if (Sim.ModelOptionsController.DataModel.UsePERFECTLeafAreaFn)
                {
                    dlai = dHUF * DataModel.PotMaxLAI * Math.Sqrt(Output.GrowthRegulator);
                }
                else
                {
                    dlai = dHUF * DataModel.PotMaxLAI * Output.GrowthRegulator;
                }
                LAI = LAI + dlai;

                // store maximum lai
                if (LAI > MaxCalcLAI)
                {
                    MaxCalcLAI = LAI;
                }
            }

            // ******************************
            // *  calculate leaf senesence  *
            // ******************************

            if (HeatUnitIndex >= DataModel.PropGrowSeaForMaxLai && HeatUnitIndex <= 1)
            {
                if (DataModel.SenesenceCoef > 0)
                {
                    // leaf senesence [Equation 2.199] from EPIC
                    if (!MathTools.DoublesAreEqual(1.0 - DataModel.PropGrowSeaForMaxLai, 0))
                    {
                        LAI = MaxCalcLAI * Math.Pow((1.0 - HeatUnitIndex) / (1.0 - DataModel.PropGrowSeaForMaxLai), DataModel.SenesenceCoef);
                    }
                    else
                    {
                        LAI = 0;
                    }
                    LAI = Math.Max(LAI, 0);
                }
                else LAI = 0;
            }
        }
        
        /// <summary>
        /// Subroutine calculates biomass using EPIC type functions
        /// </summary>
        public void CalculateBioMass()
        {
            double rad, par, hrlt, dhrlt;
            // *********************************
            // *  intercepted radiation (PAR)  *
            // *********************************
            // Assumes PAR is 50% of solar radiation
            //         Extinction Coefficient = 0.65
            rad = Sim.ClimateController.Radiation;
            par = 0.5 * rad * (1.0 - Math.Exp(-0.65 * LAI));

            // **********************
            // *  daylength factor  *
            // **********************
            // daylength factor from PERFECT
            hrlt = GetDayLength();
            dhrlt = hrlt - Hrltp;
            if (Hrltp < 0.01)
            {
                dhrlt = 0.0;
            }
            Hrltp = hrlt;
     
            double rue = DataModel.RadUseEffic;
            double effectiverue = rue;

            if (DataModel.WaterLoggingSwitch && IsWaterLogged())
            {
                effectiverue = rue * DataModel.WaterLoggingFactor2;
            }
            // **************************
            // *  biomass accumulation  *
            // **************************
            // [Equation 2.193] from EPIC
            if (Sim.ModelOptionsController.DataModel.UsePERFECTDryMatterFn)
            {
                DryMatter += Output.GrowthRegulator * par * effectiverue * Math.Pow(1.0 + dhrlt, 3.0);
                Output.DryMatter = DryMatter * 10.0;
            }

            else
            {
                par = 0.5 * rad;
                DryMatter += effectiverue * par * Output.WaterStressIndex * Output.TempStressIndex * GreenCover;
                Output.DryMatter = DryMatter * 10.0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IsWaterLogged()
        {
            int i = 1;
            return (Sim.SoilController.SoilWaterRelWP[i] > Sim.SoilController.DrainUpperLimitRelWP[i]);
        }

        /// <summary>
        /// This subroutine calculates day length from latitude and day    *
        /// number in the year. 
        /// </summary>
        /// <returns></returns>
        public double GetDayLength()
        {
            double alat = (!MathTools.DoublesAreEqual(Sim.ClimateController.Latitude, 0) ? Sim.ClimateController.Latitude : -27);
            double sund = -2.2;
            int dayno = (Sim.Today - new DateTime(Sim.Today.Year, 1, 1 + 1)).Days;
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
            {
                atwil = Math.Sin(rsund) / twif;
            }
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
            Output.RootDepth = Output.RootDepth + DataModel.DailyRootGrowth;
            Output.RootDepth = MathTools.CheckConstraints(Output.RootDepth, MaximumRootDepth, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ReadyToHarvest()
        {
            return (HeatUnitIndex >= 1);
        }
       
        /// <summary>
        /// 
        /// </summary>
        public override void ResetCropParametersAfterHarvest()
        {
            if (TodayIsHarvestDay) //if harvest day
            {
                TodayIsHarvestDay = false;
                Yield = 0;
                Output.Yield = 0;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void HarvestTheCrop()
        {
            LastHarvestDate = Sim.Today;
            TodayIsHarvestDay = true;
            Sim.FManagementEvent = ManagementEvent.Harvest;
            Sim.UpdateManagementEventHistory(ManagementEvent.Harvest, Sim.VegetationController.GetCropIndex(this));
            ++NumberOfHarvests;

            //	sim.days_since_harvest=0;    //should this also get reset at crop death
            //    soil_water_at_harvest=sim.total_soil_water;
            // ++number_of_fallows;
            //	CropStatus=csInFallow;
            Yield = DataModel.HarvestIndex * DryMatter * 10.0;
            Output.Yield = Yield / 1000.0;
            Output.ResidueAmount = Output.ResidueAmount + (DryMatter - Yield / 10.0) * 0.95 * 10.0;

            GreenCover = 0;

            Sim.VegetationController.CalculateTotalResidue();

            CumulativeYield += Yield;
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
            // CalculateBioMass();//only needs to be here to replicate previous results
            // CalculateResidue();//only needs to be here to replicate previous results
            Output.ResidueAmount = Output.ResidueAmount + (DryMatter - Yield) * 0.95 * 10.0;
            Sim.VegetationController.CalculateTotalResidue();
            Output.WaterStressIndex = 1.0;
            ++NumberOfCropsKilled;
            Yield = 0;
            Output.Yield = 0;
            ResetParametersForEndOfCrop();

            //crop_stage=0;
            //dry_matter=0;
            //soil_water_at_harvest=sim.total_soil_water;

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
            CropStatus = ModelControllers.CropStatus.Fallow;
            ++NumberOfFallows;
            SoilWaterAtHarvest = Sim.SoilController.TotalSoilWater;
            Sim.VegetationController.DaysSinceHarvest = 0;    //should this also get reset at crop death
            DaysSincePlanting = 0;
            TotalTranspiration = 0;
            CropCover = 0;
            CropCoverPercent = 0;
            CropStage = 0;
            LAI = 0;
            for (int i = 0; i < Sim.SoilController.LayerCount; ++i)
            {
                Sim.SoilController.LayerTranspiration[i] = 0;
            }
            HeatUnitIndex = 0;
            Output.RootDepth = 0;
            GreenCover = 0;
            Output.GrowthRegulator = 0;
            DryMatter = 0;
            Output.DryMatter = 0;
            Output.PotTranspiration = 0;

        }

        /// <summary>
        /// 
        /// </summary>
        public void RecordCropStage()
        {
            //   CropAnthesis=false;
            if (HeatUnitIndex <= DataModel.PropGrowSeaForMaxLai && !MathTools.DoublesAreEqual(DataModel.PropGrowSeaForMaxLai, 0))
            {
                CropStage = HeatUnitIndex * 2.0 / DataModel.PropGrowSeaForMaxLai;
                //anth=0;
            }
            else
            {
                if (MathTools.DoublesAreEqual(DataModel.PropGrowSeaForMaxLai, 0))
                {
                    MathTools.LogDivideByZeroError("RecordCropStage", "in_PropSeasonForMaxLAI", "crop_stage");
                }
                //if(anth==0)
                //		{
                //			CropAnthesis=true;
                //			anth=1;
                //		}
                if (!MathTools.DoublesAreEqual(DataModel.PropGrowSeaForMaxLai, 1))
                {
                    CropStage = 2 + (HeatUnitIndex - DataModel.PropGrowSeaForMaxLai) / (1.0 - DataModel.PropGrowSeaForMaxLai);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override double CalculatePotentialTranspiration()
        {

            // Calculate potential transpiration
            if (Sim.ModelOptionsController.DataModel.UsePERFECTGroundCovFn)
                GreenCover = Math.Min(LAI / 3.0, 1.0);
            else
            {
                if (LAI > 0)
                {
                    GreenCover = 1 - Math.Exp(-0.55 * (LAI + 0.1));  //  changed br on 9/12/2005
                }
                else
                {
                    GreenCover = 0;
                }
            }

            GreenCover = Math.Max(0.0, GreenCover);
            CropCover = Math.Max(CropCover, GreenCover);
            double value = Math.Min(Sim.ClimateController.PanEvap * GreenCover, Sim.ClimateController.PanEvap - Sim.SoilController.WatBal.SoilEvap);
            if (value > Sim.ClimateController.PanEvap)
            {
                value = Sim.ClimateController.PanEvap;
            }
            Output.GreenCover = GreenCover * 100.0;
            CropCoverPercent = CropCover * 100.0;
            if (DataModel.WaterLoggingSwitch && IsWaterLogged())
            {
                value = value * DataModel.WaterLoggingFactor1;
            }
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool StillRequiresIrrigating()
        {
            return (HeatUnits < DataModel.PropGDDEnd / 100.0 * (double)(DataModel.DegreeDaysPlantToHarvest));
        }

        /// <summary>
        /// 
        /// </summary>
        public new void CalculateResidue()
        {
            if (Sim.ModelOptionsController.DataModel.UsePERFECTResidueFn)
            {
                CalculateResidue_PERFECT();
            }
            else
            {
                CalculateResidue_BR();
            }
            TotalCover = GetTotalCover();
            Output.TotalCover = TotalCover * 100.0;
            AccumulatedResidue += ResidueCover * 100.0;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void CalculateResidue_BR()
        {
            int seriesindex = Sim.Climateindex;
            double rain_yesterday = Sim.ClimateController.RainOnDay(Sim.Today - new TimeSpan(1, 0, 0, 0));
            double rain_daybeforeyesterday = Sim.ClimateController.RainOnDay(Sim.Today - new TimeSpan(2, 0, 0, 0));

            double mi = 4.0 / 7.0 * (Math.Min(Sim.SoilController.EffectiveRain, 4) / 4.0
                        + Math.Min(rain_yesterday, 4) / 8.0
                        + Math.Min(rain_daybeforeyesterday, 4) / 16.0);  //moisture index
                                                                         //there is a minor problem here...doesn't take into consideration irrigation in the previous days.

            double ti = Math.Max(Sim.ClimateController.Temperature / 32.0, 0);                   // temperature index
            Decompdays = Math.Min(Math.Min(mi, ti), 1);  //  min=0 days, max =1day

            // Will change this to a non-linear function in the near future. BR 14/09/2010

            Output.ResidueAmount = Math.Max(0, Output.ResidueAmount - Output.ResidueAmount * DataModel.MaxResidueLoss / 100.0 * Decompdays);
            if (!MathTools.DoublesAreEqual(DataModel.BiomassAtFullCover, 0))
                ResidueCover = Math.Min(1.0, Output.ResidueAmount / DataModel.BiomassAtFullCover);
            else
            {
                ResidueCover = 0;
                MathTools.LogDivideByZeroError("CalculateResidue_BR", "in_BiomassAtFullCover", "residue_cover");
            }
            if (ResidueCover < 0) ResidueCover = 0;
            Output.ResidueCover = ResidueCover * 100.0;

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
            if (CropStatus == ModelControllers.CropStatus.Fallow)
            {
                if (DaysSinceFallow < 60)
                {
                    Output.ResidueAmount = Math.Max(0, Output.ResidueAmount - 15);
                }
                else if (DaysSinceFallow >= 60)
                {
                    Output.ResidueAmount = Math.Max(0, Output.ResidueAmount - 3);
                }
                DaysSinceFallow += 1;

            }
            else
                Output.ResidueAmount = Math.Max(0, Output.ResidueAmount - 15);
            //  Calculate proportion cover from residue weight
            //  using Sallaway type functions
            ResidueCover = DataModel.MaximumResidueCover * (1.0 - Math.Exp(-1.0 * Output.ResidueAmount / 1000.0));
            if (ResidueCover < 0)
            {
                ResidueCover = 0;
            }
            if (ResidueCover > 1)
            {
                ResidueCover = 1;
            }
            Output.ResidueCover = ResidueCover * 100.0;
        }
    }
}
