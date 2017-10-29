using System;
using HowLeaky.CustomAttributes;
using HowLeaky.DataModels;
using HowLeaky.Models;
using HowLeakyWebsite.Tools.DHMCoreLib.Helpers;
using HowLeaky.Tools;

namespace HowLeaky.ModelControllers
{
    public enum IrrigationFormat { ifAutomaticDuringGrowthStage, ifAutomaticDuringWindow, ifFromSequenceFile };
    public enum IrrigationRunoffOptions { irIgnore, irProportional, irFromSequenceFile };
    public enum RingTankAdditionalInflowFormat { aiConstant, aiFromSequencyFile };
    public enum TargetAmountOptions { taFieldCapacity, taSaturation, taFixedAmount, taDULplus25Percent, taDULplus50Percent, taDULplus75Percent };

    public class IrrigationControllerOutput
    {
        //[Output]
        //[Unit("mm")]
        //public double IrrigationRunoff { get; set; }                         // Amount of runoff (mm) from irrigation
        //[Output]
        //[Unit("mm")]
        //public double IrrigationApplied { get; set; }                        // Amount of water (mm) applied during irrigation
        //[Output]
        //[Unit("mm")]
        //public double IrrigationInfiltration { get; set; }                   // Amount of water (mm) which infiltrates into soil during irrigation
        //[Output]
        //[Unit("ML")]
        //public double RingTankEvaporationLosses { get; set; }                // Ring-Tank Evaporation Losses (ML)
        //[Output]
        //[Unit("ML")]
        //public double RingTankSeepageLosses { get; set; }                    // Ring-Tank Seepage Losses (ML)
        //[Output]
        //[Unit("ML")]
        //public double RingTankOvertoppingLosses { get; set; }                // Ring-Tank Overtopping Losses (ML)
        //[Output]
        //[Unit("ML")]
        //public double RingTankIrrigationLosses { get; set; }                 // Ring-Tank Irrigation Losses (ML)
        //[Output]
        //[Unit("ML")]
        //public double RingTankTotalLosses { get; set; }                      // Ring-Tank Total Losses (ML)
        //[Output]
        //[Unit("ML")]
        //public double RingTankRunoffCaptureInflow { get; set; }              // Ring-Tank Captured Runoff Inflow ML)
        //[Output]
        //[Unit("ML")]
        //public double RingTankRainfalInflow { get; set; }                    // Ring-Tank Rainfall Inflow (ML)
        //[Output]
        //[Unit("ML")]
        //public double RingTankEffectiveAdditionalInflow { get; set; }        // Ring-Tank Effective Additional Inflow (ML)
        //[Output]
        //[Unit("ML")]
        //public double RingTankTotalAdditionalInflow { get; set; }            // Ring-Tank Total Additional Inflow (ML)
        //[Output]
        //[Unit("ML")]
        //public double RingTankTotalInflow { get; set; }                      // Ring-Tank Total Inflow (ML)
        //[Output]
        //[Unit("ML")]
        //public double RingTankIneffectiveAdditionalInflow { get; set; }      // Ring-Tank Ineffective Additional Inflow (ML)
        //[Output]
        //[Unit("ML")]
        //public double RingTankStorageVolume { get; set; }                    // Ring-Tank Storage Volume (ML)
        //[Output]
        //[Unit("pc")]
        //public double RingTankStorageLevel { get; set; }                     // Ring-Tank storage Level (%)
    }

    public class IrrigationControllerSummaryOutput
    {
        [Unit("ML")]
        public double irrigationLosses { get; set; }
        [Unit("ML")]
        public double evaporationLosses { get; set; }
        [Unit("ML")]
        public double seepageLosses { get; set; }
        [Unit("ML")]
        public double overtoppingLosses { get; set; }
        [Unit("ML")]
        public double runoffCaptureInflow { get; set; }
        [Unit("ML")]
        public double rainfallInflow { get; set; }
        [Unit("ML")]
        public double effectiveAdditionalnflow { get; set; }
        [Unit("ML")]
        public double totalAdditionalInflow { get; set; }
        [Unit("ML")]
        public double storageLevel { get; set; }
    }

    public class IrrigationControllerSO
    {
        [Unit("ML_per_yr")]
        public double RingTankIrrigationLosses { get; set; }
        [Unit("ML_per_yr")]
        public double RingTankIrrigationLossesDelivered { get; set; }
        [Unit("ML_per_yr")]
        public double RingTankEvaporationLosses { get; set; }
        [Unit("ML_per_yr")]
        public double RingTankSeepageLosses { get; set; }
        [Unit("ML_per_yr")]
        public double RingTankOvertoppingLosses { get; set; }
        [Unit("ML_per_yr")]
        public double RingTankRunoffCaptureInflow { get; set; }
        [Unit("ML_per_yr")]
        public double RingTankRainfallInflow { get; set; }
        [Unit("ML_per_yr")]
        public double RingTankEffectiveAdditionalInflow { get; set; }
        [Unit("ML_per_yr")]
        public double RingTankAdditionalInflow { get; set; }
        [Unit("ML_per_yr")]
        public double RingTanksStorageLevel { get; set; }
        [Unit("ML_per_yr")]
        public double RingTankPropDaysOverflow { get; set; }
        [Unit("ML_per_yr")]
        public double RingTankPropYearsOverflow { get; set; }
    }

    public class IrrigationController : HLObject
    {

        //--------------------------------------------------------------------------
        //Outputs
        //--------------------------------------------------------------------------
        IrrigationControllerOutput output = new IrrigationControllerOutput();

        IrrigationControllerSummaryOutput sum = new IrrigationControllerSummaryOutput();
        IrrigationControllerSO so = new IrrigationControllerSO();

        [Output]
        [Unit("mm")]
        public double out_IrrigationRunoff { get; set; }                         // Amount of runoff (mm) from irrigation
        [Output]
        [Unit("mm")]
        public double out_IrrigationApplied { get; set; }                        // Amount of water (mm) applied during irrigation
        [Output]
        [Unit("mm")]
        public double out_IrrigationInfiltration { get; set; }                   // Amount of water (mm) which infiltrates into soil during irrigation
        [Output]
        [Unit("ML")]
        public double out_RingTankEvaporationLosses { get; set; }                // Ring-Tank Evaporation Losses (ML)
        [Output]
        [Unit("ML")]
        public double out_RingTankSeepageLosses { get; set; }                    // Ring-Tank Seepage Losses (ML)
        [Output]
        [Unit("ML")]
        public double out_RingTankOvertoppingLosses { get; set; }                // Ring-Tank Overtopping Losses (ML)
        [Output]
        [Unit("ML")]
        public double out_RingTankIrrigationLosses { get; set; }                 // Ring-Tank Irrigation Losses (ML)
        [Output]
        [Unit("ML")]
        public double out_RingTankTotalLosses { get; set; }                      // Ring-Tank Total Losses (ML)
        [Output]
        [Unit("ML")]
        public double out_RingTankRunoffCaptureInflow { get; set; }              // Ring-Tank Captured Runoff Inflow ML)
        [Output]
        [Unit("ML")]
        public double out_RingTankRainfalInflow { get; set; }                    // Ring-Tank Rainfall Inflow (ML)
        [Output]
        [Unit("ML")]
        public double out_RingTankEffectiveAdditionalInflow { get; set; }        // Ring-Tank Effective Additional Inflow (ML)
        [Output]
        [Unit("ML")]
        public double out_RingTankTotalAdditionalInflow { get; set; }            // Ring-Tank Total Additional Inflow (ML)
        [Output]
        [Unit("ML")]
        public double out_RingTankTotalInflow { get; set; }                      // Ring-Tank Total Inflow (ML)
        [Output]
        [Unit("ML")]
        public double out_RingTankIneffectiveAdditionalInflow { get; set; }      // Ring-Tank Ineffective Additional Inflow (ML)
        [Output]
        [Unit("ML")]
        public double out_RingTankStorageVolume { get; set; }                    // Ring-Tank Storage Volume (ML)
        [Output]
        [Unit("pc")]
        public double out_RingTankStorageLevel { get; set; }                     // Ring-Tank storage Level (%)

        //--------------------------------------------------------------------------
        //Internals
        //--------------------------------------------------------------------------
        [Internal]
        public int daysSinceIrrigation { get; set; }                            // Number days since last irrigation
        [Internal]
        public bool firstIrrigation { get; set; }                               // Switch to indicate it is the first irrigation 
        [Internal]
        [Unit("mm")]
        public double irrigationRunoffAmount { get; set; }                      // Amount of runoff (mm) from irrigation 
        [Internal]
        [Unit("mm")]
        public double overflow { get; set; }                                    // Amount of overflow (mm) from ringtank
        [Internal]
        [Unit("mm")]
        public double irrigationApplied { get; set; }                           // Actual amount of irrigation (mm) that is applied
        [Internal]
        [Unit("mm")]
        public double irrigationAmount { get; set; }                            // Amount of water required for irrigation
        [Internal]
        public double irrigationAmountFromRingtank { get; set; }
        [Internal]
        [Unit("m3")]
        public double storageVolume { get; set; }
        [Internal]
        public double numDaysOverflow { get; set; }
        [Internal]
        public double numYearOverflow { get; set; }
        [Internal]
        public int lastOvertoppingYear { get; set; }



        public IrrigationDataModel dataModel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IrrigationController()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sim"></param>
        public IrrigationController(Simulation sim) : base(sim)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Simulate()
        {
            try
            {
                if (CanIrrigateToday())
                {
                    double required_amount = GetRequiredIrrigationAmount();
                    if (required_amount > 0)
                    {
                        double available_amount = GetAvailableAmountFromSupply(required_amount);
                        if (available_amount > 0)
                            Irrigate(available_amount);
                    }
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
        public override void Initialise()
        {
            storageVolume = 0;
            daysSinceIrrigation = -1;
            //	AdditionalInflowIndex=0;
            irrigationRunoffAmount = 0;
            firstIrrigation = true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="available_amount"></param>
        public void Irrigate(double available_amount)
        {
            RecordIrrigationEvent();
            irrigationApplied = available_amount;
            irrigationAmount = RemoveRunoffLosses(available_amount);
            if (irrigationAmount > 0)
                DistributeWaterThroughSoilLayers(irrigationAmount);
        }
        /// <summary>
        /// 
        /// </summary>
        public void SetStartOfDayParameters()
        {
            irrigationAmount = 0;
            out_IrrigationApplied = 0;
            irrigationRunoffAmount = 0;
            if (daysSinceIrrigation != -1)
            {
                ++daysSinceIrrigation;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void RecordIrrigationEvent()
        {
            daysSinceIrrigation = 0;
            sim.UpdateManagementEventHistory(ManagementEvent.meIrrigation, 0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanIrrigateToday()
        {
            switch (dataModel.IrrigFormat)
            {
                case (int)IrrigationFormat.ifAutomaticDuringGrowthStage: return CropWantsIrrigating() && WaitingPeriodExceeded();
                case (int)IrrigationFormat.ifAutomaticDuringWindow: return IsDateinIrrigationWindow() && CropWantsIrrigating() && WaitingPeriodExceeded();
                case (int)IrrigationFormat.ifFromSequenceFile: return IsDateInSequence();
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CropWantsIrrigating()
        {
            return (sim.VegetationController.CurrentCrop != null &&
                    sim.VegetationController.CurrentCrop.IsGrowing() &&
                    sim.VegetationController.CurrentCrop.StillRequiresIrrigating());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsDateinIrrigationWindow()
        {
            return DateUtilities.isDateInWindow(sim.today, dataModel.IrrigWindowStartDate, dataModel.IrrigWindowEndDate);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsDateInSequence()
        {
            return dataModel.IrrigSequence.ContainsDate(sim.today);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool WaitingPeriodExceeded()
        {
            return !(daysSinceIrrigation != -1 && daysSinceIrrigation < dataModel.IrrigationBufferPeriod);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetRequiredIrrigationAmount()
        {
            if (dataModel.IrrigFormat != (int)IrrigationFormat.ifFromSequenceFile)
            {
                if (sim.effective_rain <= 0.1)
                {
                    if (dataModel.SWDToIrrigate > 0.0 && sim.swd > dataModel.SWDToIrrigate)
                    {
                        switch (dataModel.TargetAmountOpt)
                        {
                            case (int)TargetAmountOptions.taFieldCapacity: return sim.swd;
                            case (int)TargetAmountOptions.taSaturation: return sim.satd;
                            case (int)TargetAmountOptions.taFixedAmount: return dataModel.FixedIrrigationAmount;
                            case (int)TargetAmountOptions.taDULplus25Percent: return sim.swd + (sim.satd - sim.swd) * 0.25;
                            case (int)TargetAmountOptions.taDULplus50Percent: return sim.swd + (sim.satd - sim.swd) * 0.50;
                            case (int)TargetAmountOptions.taDULplus75Percent: return sim.swd + (sim.satd - sim.swd) * 0.75;
                            default: return 0;
                        }
                    }
                }
            }
            else
            {
                dataModel.TargetAmountOpt = (int)TargetAmountOptions.taFixedAmount;
                return dataModel.IrrigSequence.ValueAtDate(sim.today);
            }
            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public double GetAvailableAmountFromSupply(double amount)
        {

            if (dataModel.UseRingTank)
            {
                double irrigatedarea_m2 = dataModel.IrrigatedArea * 10000.0;
                if (dataModel.IrrigDeliveryEfficiency > 0)
                {
                    double deliveffic = (dataModel.IrrigDeliveryEfficiency / 100.0);
                    double amount_req_from_ringtank_m3 = amount / 1000.0 * irrigatedarea_m2 / dataModel.IrrigDeliveryEfficiency / 100.0;//m^3      //divide by zero checked above

                    if (amount_req_from_ringtank_m3 < storageVolume)
                    {
                        storageVolume -= amount_req_from_ringtank_m3;
                        //our irrigation amount as calculated above does not change
                    }
                    else  //if we dont have enough water in the tank
                    {
                        double irrigation_available_m3 = storageVolume * dataModel.IrrigDeliveryEfficiency;
                        amount = irrigation_available_m3 / irrigatedarea_m2 * 1000.0;
                        storageVolume = 0;
                    }
                }
                else
                {
                    //amount = 0;
                    storageVolume = 0;
                }
            }
            return amount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public double RemoveRunoffLosses(double amount)
        {
            if (dataModel.IrrigRunoffOptions == (int)IrrigationRunoffOptions.irProportional)
            {
                double factor;
                if (firstIrrigation)
                {
                    factor = dataModel.IrrigRunoffProportion1 / 100.0;
                }
                else
                {
                    factor = dataModel.IrrigRunoffProportion2 / 100.0;
                }
                irrigationRunoffAmount = factor * amount;

                amount = amount - irrigationRunoffAmount;

                firstIrrigation = false; //assign this to true when planting.
            }
            else if (dataModel.IrrigRunoffOptions == (int)IrrigationRunoffOptions.irFromSequenceFile)
            {
                irrigationRunoffAmount = dataModel.IrrigRunoffSequence.ValueAtDate(sim.today);
                if (irrigationRunoffAmount < amount)
                    amount = amount - irrigationRunoffAmount;
                else if (irrigationRunoffAmount >= amount)
                {
                    irrigationRunoffAmount = amount;
                    amount = 0;
                }
            }
            if (amount < 0)
                amount = 0;
            return amount;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        public void DistributeWaterThroughSoilLayers(double amount)
        {
            double targetlayeramount = 0;
            int layercount = sim.in_LayerCount;
            for (int i = 0; i < layercount; ++i)
            {
                double dul = sim.DrainUpperLimit_rel_wp[i];
                double sat = sim.SaturationLimit_rel_wp[i];
                switch (dataModel.TargetAmountOpt)
                {
                    case (int)TargetAmountOptions.taFieldCapacity: targetlayeramount = dul; break;
                    case (int)TargetAmountOptions.taSaturation: targetlayeramount = sat; break;
                    case (int)TargetAmountOptions.taFixedAmount: targetlayeramount = sat; break;
                    case (int)TargetAmountOptions.taDULplus25Percent: targetlayeramount = dul + 0.25 * (sat - dul); break;
                    case (int)TargetAmountOptions.taDULplus50Percent: targetlayeramount = dul + 0.50 * (sat - dul); break;
                    case (int)TargetAmountOptions.taDULplus75Percent: targetlayeramount = dul + 0.75 * (sat - dul); break;
                }
                double layerdef = targetlayeramount - sim.SoilWater_rel_wp[i];
                if (amount > layerdef)
                    sim.SoilWater_rel_wp[i] = targetlayeramount;
                else
                    sim.SoilWater_rel_wp[i] = sim.SoilWater_rel_wp[i] + amount;
                amount = amount - layerdef;
                if (amount < 0)
                    i = layercount;
            }
            sim.swd = 0;
            for (int i = 0; i < layercount; ++i)
                sim.swd = sim.swd + (sim.DrainUpperLimit_rel_wp[i] - sim.SoilWater_rel_wp[i]);
            sim.sse1 = Math.Max(0, sim.sse1 - sim.swd);
            if (amount > 0)
                sim.effective_rain += amount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool PondingExists()
        {
            if (sim.VegetationController.CurrentCrop != null &&
                sim.VegetationController.CurrentCrop.IsGrowing())
                return (CansimulateIrrigation() && dataModel.UsePonding && sim.VegetationController.CurrentCrop.StillRequiresIrrigating());
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        public void ModelRingTank()
        {
            try
            {
                // NOTE- WE HAVE ALREADY IRRIGATED BY THIS STAGE
                if (CansimulateIrrigation() && dataModel.UseRingTank)
                {
                    double in_RingTankArea_ha = dataModel.RingTankArea * 10000.0;
                    if (dataModel.ResetRingTank && dataModel.ResetRingTankDate.MatchesDate(sim.today))
                        ResetRingTank();
                    else
                        simulateDailyRingTankWaterBalance();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetRingTank()
        {
            double maxvolume = dataModel.RingTankDepth * dataModel.RingTankArea; //m^3
            double oldvolume = storageVolume;
            storageVolume = maxvolume * dataModel.CapactityAtReset / 100.0;
            out_RingTankIrrigationLosses = 0;
            out_RingTankEvaporationLosses = 0;
            out_RingTankSeepageLosses = 0;
            out_RingTankOvertoppingLosses = 0;
            out_RingTankTotalLosses = 0;
            out_RingTankRunoffCaptureInflow = 0;
            out_RingTankRainfalInflow = 0;
            out_RingTankEffectiveAdditionalInflow = 0;
            out_RingTankTotalAdditionalInflow = 0;
            out_RingTankTotalInflow = 0;
            out_RingTankIneffectiveAdditionalInflow = 0;
            out_RingTankStorageVolume = storageVolume / 1000.0;

            if (!MathTools.DoublesAreEqual(dataModel.RingTankDepth * dataModel.RingTankArea, 0))
                out_RingTankStorageLevel = storageVolume / (dataModel.RingTankDepth * dataModel.RingTankArea) * 100.0;
            else
            {
                out_RingTankStorageLevel = 0;
                MathTools.LogDivideByZeroError("ModelRingTank", "in_RingTankDepth_m*in_RingTankArea_ha", "out_RingTankStorageLevel_pc");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void simulateDailyRingTankWaterBalance()
        {
            //inflows
            double runoff_capture_inflow_m3 = CalcRunoffCaptureInflow();
            double rainfall_inflow_m3 = sim.out_Rain_mm / 1000.0 * dataModel.RingTankArea;
            double additional_inflow_m3 = GetAdditionalTankInflow();
            double total_inflow_m3 = runoff_capture_inflow_m3 + rainfall_inflow_m3 + additional_inflow_m3;

            //losses
            double seepage_losses_m3 = dataModel.RingTankSeepageRate / 1000.0 * dataModel.RingTankArea;
            double evaporation_losses_m3 = sim.out_PanEvap_mm * dataModel.RingTankEvapCoefficient / 1000.0 * dataModel.RingTankArea;
            double seepage_plus_evap_losses_m3 = seepage_losses_m3 + evaporation_losses_m3;

            if (seepage_plus_evap_losses_m3 > storageVolume)
            {
                seepage_plus_evap_losses_m3 = storageVolume;
                if (seepage_losses_m3 < storageVolume / 2.0)
                    evaporation_losses_m3 = storageVolume - seepage_losses_m3;
                else if (evaporation_losses_m3 < storageVolume / 2.0)
                    seepage_losses_m3 = storageVolume - evaporation_losses_m3;
                else
                {
                    seepage_losses_m3 = storageVolume / 2.0;
                    evaporation_losses_m3 = storageVolume / 2.0;
                }
            }
            //NOTE - Irrigation losses have already been extracted before we call this routine. Irrigation effectively
            // begins at the start of the day.

            //storage
            double potential_storage_volume_m3 = CalcPotentialStorageVolume(total_inflow_m3, seepage_plus_evap_losses_m3);
            storageVolume = CalcActualStorageVolume(total_inflow_m3, seepage_plus_evap_losses_m3, potential_storage_volume_m3);

            //output variables
            out_RingTankIrrigationLosses = (irrigationApplied / 1000.0 * dataModel.IrrigatedArea * 10000.0) / 1000.0 / (dataModel.IrrigDeliveryEfficiency / 100.0);
            out_RingTankEvaporationLosses = evaporation_losses_m3 / 1000.0;
            out_RingTankSeepageLosses = seepage_losses_m3 / 1000.0;
            out_RingTankOvertoppingLosses = CalcOvertoppingAmount(potential_storage_volume_m3, storageVolume);
            out_RingTankTotalLosses = out_RingTankIrrigationLosses + out_RingTankEvaporationLosses + out_RingTankSeepageLosses + out_RingTankOvertoppingLosses;
            out_RingTankRunoffCaptureInflow = runoff_capture_inflow_m3 / 1000.0;
            out_RingTankRainfalInflow = rainfall_inflow_m3 / 1000.0;
            out_RingTankTotalAdditionalInflow = additional_inflow_m3 / 1000.0;
            out_RingTankEffectiveAdditionalInflow = CalcEffectiveAdditionalInflow(out_RingTankOvertoppingLosses, additional_inflow_m3);
            out_RingTankTotalInflow = out_RingTankRainfalInflow + out_RingTankRunoffCaptureInflow + additional_inflow_m3 / 1000.0;
            out_RingTankIneffectiveAdditionalInflow = additional_inflow_m3 / 1000.0 - out_RingTankEffectiveAdditionalInflow;
            out_RingTankStorageVolume = storageVolume / 1000.0;
            out_RingTankStorageLevel = CalcStorageLevel(storageVolume);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="potential_m3"></param>
        /// <param name="actual_m3"></param>
        /// <returns></returns>
        public double CalcOvertoppingAmount(double potential_m3, double actual_m3)
        {
            if (potential_m3 > actual_m3)
                return (potential_m3 - actual_m3) / 1000.0;
            return 0;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="overtopping_ML"></param>
        /// <param name="additionalinflow_m3"></param>
        /// <returns></returns>
        public double CalcEffectiveAdditionalInflow(double overtopping_ML, double additionalinflow_m3)
        {
            if (MathTools.DoublesAreEqual(overtopping_ML, 0))
            {
                return additionalinflow_m3 / 1000.0; //convert to ML
            }
            double value = additionalinflow_m3 / 1000.0 - overtopping_ML;
            return (value > 0 ? value : 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storage_volume_m3"></param>
        /// <returns></returns>
        public double CalcStorageLevel(double storage_volume_m3)
        {
            double level;
            double capacity_m3 = dataModel.RingTankDepth * dataModel.RingTankArea * 10000.0;
            if (!MathTools.DoublesAreEqual(capacity_m3, 0))
            {
                level = storage_volume_m3 / capacity_m3 * 100.0;
            }
            else
            {
                level = 0;
                MathTools.LogDivideByZeroError("ModelRingTank", "in_RingTankDepth_m*in_RingTankArea_ha", "out_RingTankStorageLevel_pc");
            }
            return level;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputs_m3"></param>
        /// <param name="outputs_m3"></param>
        /// <returns></returns>
        public double CalcPotentialStorageVolume(double inputs_m3, double outputs_m3)
        {

            double storagevolume = storageVolume + inputs_m3 - outputs_m3;
            return (storagevolume > 0 ? storagevolume : 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputs_m3"></param>
        /// <param name="outputs_m3"></param>
        /// <param name="potentialvolume"></param>
        /// <returns></returns>
        public double CalcActualStorageVolume(double inputs_m3, double outputs_m3, double potentialvolume)
        {
            double capacity_m3 = dataModel.RingTankDepth * dataModel.RingTankArea * 10000.0; //m^3
            return (potentialvolume < capacity_m3 ? potentialvolume : capacity_m3);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double CalcRunoffCaptureInflow()
        {
            double runoffinflow = sim.out_WatBal_Runoff_mm / 1000.0 * dataModel.CatchmentArea * 10000;  //m^3
            double runoffcapturerate_m3 = dataModel.RunoffCaptureRate * 1000.0;
            return (runoffinflow < runoffcapturerate_m3 ? runoffinflow : runoffcapturerate_m3);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetAdditionalTankInflow()
        {
            double value = 0;

            try
            {
                if (dataModel.AdditionalInflowFormat == (int)RingTankAdditionalInflowFormat.aiConstant)
                {
                    value = dataModel.AdditionalInflow * 1000.0;         //converting to m3
                }
                else
                {
                    value = dataModel.AdditionalInflowSequence.ValueAtDate(sim.today) * 1000.0;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return value;

        }
        /// <summary>
        /// 
        /// </summary>
        public void UpdateRingtankWaterBalance()
        {
            try
            {
                if (dataModel.UseRingTank)
                {
                    sum.irrigationLosses += out_RingTankIrrigationLosses;
                    sum.evaporationLosses += out_RingTankEvaporationLosses;
                    sum.seepageLosses += out_RingTankSeepageLosses;
                    sum.overtoppingLosses += out_RingTankOvertoppingLosses;
                    sum.runoffCaptureInflow += out_RingTankRunoffCaptureInflow;
                    sum.rainfallInflow += out_RingTankRainfalInflow;
                    sum.effectiveAdditionalnflow += out_RingTankEffectiveAdditionalInflow;
                    sum.totalAdditionalInflow += out_RingTankTotalAdditionalInflow;
                    sum.storageLevel += out_RingTankStorageLevel;
                    if (out_RingTankOvertoppingLosses > 0)
                    {
                        ++numDaysOverflow;
                        if (sim.year != lastOvertoppingYear)
                        {
                            ++numYearOverflow;
                            lastOvertoppingYear = sim.year;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ConsiderCoverEffects()
        {
            return (dataModel.IrrigRunoffOptions > 0 &&
                    dataModel.IrrigCoverEffects > 0 &&
                    irrigationAmount > 0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetCoverEffect(double crop_cover, double total_residue_cover)
        {
            double cover = 0;
            if (dataModel.IrrigCoverEffects == 0)
                cover = Math.Min(100.0, (crop_cover + total_residue_cover * (1 - crop_cover)) * 100.0);
            else if (dataModel.IrrigCoverEffects == 1)
                cover = Math.Min(100.0, (0 + total_residue_cover * (1 - 0)) * 100.0);
            else if (dataModel.IrrigCoverEffects == 2)
                cover = 0;
            return cover;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CansimulateIrrigation()
        {
            return true;

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
            if (dataModel.UseRingTank)
            {
                double simyears = sim.number_of_days_in_simulation / 365.0;
                so.RingTankIrrigationLosses = MathTools.Divide(sum.irrigationLosses, simyears);
                so.RingTankIrrigationLossesDelivered = so.RingTankIrrigationLosses * dataModel.IrrigDeliveryEfficiency / 100.0;
                so.RingTankEvaporationLosses = MathTools.Divide(sum.evaporationLosses, simyears);
                so.RingTankSeepageLosses = MathTools.Divide(sum.seepageLosses, simyears);
                so.RingTankOvertoppingLosses = MathTools.Divide(sum.overtoppingLosses, simyears);
                so.RingTankRunoffCaptureInflow = MathTools.Divide(sum.runoffCaptureInflow, simyears);
                so.RingTankRainfallInflow = MathTools.Divide(sum.rainfallInflow, simyears);
                so.RingTankEffectiveAdditionalInflow = MathTools.Divide(sum.effectiveAdditionalnflow, simyears);
                so.RingTankAdditionalInflow = MathTools.Divide(sum.totalAdditionalInflow, simyears);
                so.RingTanksStorageLevel = MathTools.Divide(sum.storageLevel, sim.number_of_days_in_simulation);
                so.RingTankPropDaysOverflow = MathTools.Divide(numDaysOverflow, sim.number_of_days_in_simulation) * 100.0;
                so.RingTankPropYearsOverflow = MathTools.Divide(numYearOverflow, simyears) * 100.0;
            }
        }
    }
}



