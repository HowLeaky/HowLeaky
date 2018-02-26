using System;
using HowLeaky.CustomAttributes;
using HowLeaky.DataModels;
using HowLeaky.Tools.Helpers;
using HowLeaky.OutputModels;
using System.Collections.Generic;
using HowLeaky.Interfaces;

namespace HowLeaky.ModelControllers
{
    public enum IrrigationFormat { AutomaticDuringGrowthStage, AutomaticDuringWindow, FromSequenceFile };
    public enum IrrigationRunoffOptions { Ignore, Proportional, FromSequenceFile };
    public enum RingTankAdditionalInflowFormat { Constant, FromSequencyFile };
    public enum TargetAmountOptions { FieldCapacity, Saturation, FixedAmount, DULplus25Percent, DULplus50Percent, DULplus75Percent };

    public class IrrigationControllerOutput : OutputDataModel, IDailyOutput
    {

    }

    public class IrrigationControllerSummaryOutput : OutputDataModel
    {
        [Unit("ML")]
        public double IrrigationLosses { get; set; }
        [Unit("ML")]
        public double EvaporationLosses { get; set; }
        [Unit("ML")]
        public double SeepageLosses { get; set; }
        [Unit("ML")]
        public double OvertoppingLosses { get; set; }
        [Unit("ML")]
        public double RunoffCaptureInflow { get; set; }
        [Unit("ML")]
        public double RainfallInflow { get; set; }
        [Unit("ML")]
        public double EffectiveAdditionalnflow { get; set; }
        [Unit("ML")]
        public double TotalAdditionalInflow { get; set; }
        [Unit("ML")]
        public double StorageLevel { get; set; }
    }

    public class IrrigationControllerSO : OutputDataModel
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

    public class IrrigationController : HLController
    {
        //--------------------------------------------------------------------------
        //Outputs
        //--------------------------------------------------------------------------
        //public IrrigationControllerOutput Output { get; set; } = new IrrigationControllerOutput();
        public IrrigationControllerSummaryOutput Sum { get; set; } = new IrrigationControllerSummaryOutput();
        public IrrigationControllerSO SO { get; set; } = new IrrigationControllerSO();

        //--------------------------------------------------------------------------
        //Internals
        //--------------------------------------------------------------------------
        [Internal]
        public int DaysSinceIrrigation { get; set; }                            // Number days since last irrigation
        [Internal]
        public bool FirstIrrigation { get; set; }                               // Switch to indicate it is the first irrigation 
        [Internal]
        [Unit("mm")]
        public double IrrigationRunoffAmount { get; set; }                      // Amount of runoff (mm) from irrigation 
        [Internal]
        [Unit("mm")]
        public double Overflow { get; set; }                                    // Amount of overflow (mm) from ringtank
        [Internal]
        [Unit("mm")]
        public double IrrigationAmount { get; set; }                            // Amount of water required for irrigation
        [Internal]
        public double IrrigationAmountFromRingtank { get; set; }
        [Internal]
        [Unit("m3")]
        public double StorageVolume { get; set; }
        [Internal]
        public double NumDaysOverflow { get; set; }
        [Internal]
        public double NumYearOverflow { get; set; }
        [Internal]
        public int LastOvertoppingYear { get; set; }

        public IrrigationInputModel InputModel { get; set; }

        //Reportbale Outputs
        [Output("Amount of runoff from irrigation", "mm")]
        public double IrrigationRunoff { get; set; }
        [Output("Amount of water applied during irrigation", "mm")]
        public double IrrigationApplied { get; set; }
        [Output("Amount of water which infiltrates into soil during irrigation", "mm")]
        public double IrrigationInfiltration { get; set; }
        [Output("Ring-Tank Evaporation Losses", "ML")]
        public double RingTankEvaporationLosses { get; set; }
        [Output("Ring-Tank Seepage Losses", "ML")]
        public double RingTankSeepageLosses { get; set; }
        [Output("Ring-Tank Overtopping Losses", "ML")]
        public double RingTankOvertoppingLosses { get; set; }
        [Output("Ring-Tank Irrigation Losses", "ML")]
        public double RingTankIrrigationLosses { get; set; }
        [Output("Ring-Tank Total Losses", "ML")]
        public double RingTankTotalLosses { get; set; }
        [Output("Ring-Tank Captured Runoff Inflow", "ML")]
        public double RingTankRunoffCaptureInflow { get; set; }
        [Output("Ring-Tank Rainfall Inflow", "ML")]
        public double RingTankRainfalInflow { get; set; }
        [Output("Ring-Tank Effective Additional Inflow", "ML")]
        public double RingTankEffectiveAdditionalInflow { get; set; }
        [Output("Ring-Tank Total Additional Inflow", "ML")]
        public double RingTankTotalAdditionalInflow { get; set; }
        [Output("Ring-Tank Total Inflow", "ML")]
        public double RingTankTotalInflow { get; set; }
        [Output("Ring-Tank Ineffective Additional Inflow", "ML")]
        public double RingTankIneffectiveAdditionalInflow { get; set; }
        [Output("Ring-Tank Storage Volume", "ML")]
        public double RingTankStorageVolume { get; set; }
        [Output("Ring-Tank storage Level", "%")]
        public double RingTankStorageLevel { get; set; }

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
        public IrrigationController(Simulation sim, List<InputModel> inputModels) : base(sim)
        {
            InputModel = (IrrigationInputModel)inputModels[0];

            InitOutputModel();
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
                    double requiredAmount = GetRequiredIrrigationAmount();
                    if (requiredAmount > 0)
                    {
                        double availableAmount = GetAvailableAmountFromSupply(requiredAmount);
                        if (availableAmount > 0)
                        {
                            Irrigate(availableAmount);
                        }
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
        /// <returns></returns>
        public override InputModel GetInputModel()
        {
            return InputModel;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialise()
        {
            StorageVolume = 0;
            DaysSinceIrrigation = -1;
            //	AdditionalInflowIndex=0;
            IrrigationRunoffAmount = 0;
            FirstIrrigation = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="available_amount"></param>
        public void Irrigate(double available_amount)
        {
            RecordIrrigationEvent();
            IrrigationApplied = available_amount;
            IrrigationAmount = RemoveRunoffLosses(available_amount);
            Sim.SoilController.Irrigation = IrrigationAmount;
            if (IrrigationAmount > 0)
            {
                DistributeWaterThroughSoilLayers(IrrigationAmount);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void SetStartOfDayParameters()
        {
            IrrigationAmount = 0;
            IrrigationApplied = 0;
            IrrigationRunoffAmount = 0;
            if (DaysSinceIrrigation != -1)
            {
                ++DaysSinceIrrigation;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RecordIrrigationEvent()
        {
            DaysSinceIrrigation = 0;
            Sim.UpdateManagementEventHistory(ManagementEvent.Irrigation, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanIrrigateToday()
        {
            switch (InputModel.IrrigFormat)
            {
                case (int)IrrigationFormat.AutomaticDuringGrowthStage: return CropWantsIrrigating() && WaitingPeriodExceeded();
                case (int)IrrigationFormat.AutomaticDuringWindow: return IsDateinIrrigationWindow() && CropWantsIrrigating() && WaitingPeriodExceeded();
                case (int)IrrigationFormat.FromSequenceFile: return IsDateInSequence();
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CropWantsIrrigating()
        {
            return (Sim.VegetationController.CurrentCrop != null &&
                    Sim.VegetationController.CurrentCrop.IsGrowing() &&
                    Sim.VegetationController.CurrentCrop.StillRequiresIrrigating());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsDateinIrrigationWindow()
        {
            return DateUtilities.isDateInWindow(Sim.Today, InputModel.IrrigWindowStartDate, InputModel.IrrigWindowEndDate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsDateInSequence()
        {
            return InputModel.IrrigSequence.ContainsDate(Sim.Today);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool WaitingPeriodExceeded()
        {
            return !(DaysSinceIrrigation != -1 && DaysSinceIrrigation < InputModel.IrrigationBufferPeriod);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetRequiredIrrigationAmount()
        {
            if (InputModel.IrrigFormat != (int)IrrigationFormat.FromSequenceFile)
            {
                if (Sim.SoilController.EffectiveRain <= 0.1)
                {
                    if (InputModel.SWDToIrrigate > 0.0 && Sim.SoilController.SoilWaterDeficit > InputModel.SWDToIrrigate)
                    {
                        switch (InputModel.TargetAmountOpt)
                        {
                            case (int)TargetAmountOptions.FieldCapacity: return Sim.SoilController.SoilWaterDeficit;
                            case (int)TargetAmountOptions.Saturation: return Sim.SoilController.Satd;
                            case (int)TargetAmountOptions.FixedAmount: return InputModel.FixedIrrigationAmount;
                            case (int)TargetAmountOptions.DULplus25Percent: return Sim.SoilController.SoilWaterDeficit + (Sim.SoilController.Satd - Sim.SoilController.SoilWaterDeficit) * 0.25;
                            case (int)TargetAmountOptions.DULplus50Percent: return Sim.SoilController.SoilWaterDeficit + (Sim.SoilController.Satd - Sim.SoilController.SoilWaterDeficit) * 0.50;
                            case (int)TargetAmountOptions.DULplus75Percent: return Sim.SoilController.SoilWaterDeficit + (Sim.SoilController.Satd - Sim.SoilController.SoilWaterDeficit) * 0.75;
                            default: return 0;
                        }
                    }
                }
            }
            else
            {
                InputModel.TargetAmountOpt = (int)TargetAmountOptions.FixedAmount;
                return InputModel.IrrigSequence.ValueAtDate(Sim.Today);
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

            if (InputModel.UseRingTank)
            {
                double irrigatedareaM2 = InputModel.IrrigatedArea * 10000.0;
                if (InputModel.IrrigDeliveryEfficiency > 0)
                {
                    double deliveffic = (InputModel.IrrigDeliveryEfficiency / 100.0);
                    double amountReqFromRingtankM3 = amount / 1000.0 * irrigatedareaM2 / InputModel.IrrigDeliveryEfficiency / 100.0;//m^3      //divide by zero checked above

                    if (amountReqFromRingtankM3 < StorageVolume)
                    {
                        StorageVolume -= amountReqFromRingtankM3;
                        //our irrigation amount as calculated above does not change
                    }
                    else  //if we dont have enough water in the tank
                    {
                        double irrigationAvailableM3 = StorageVolume * InputModel.IrrigDeliveryEfficiency;
                        amount = irrigationAvailableM3 / irrigatedareaM2 * 1000.0;
                        StorageVolume = 0;
                    }
                }
                else
                {
                    //amount = 0;
                    StorageVolume = 0;
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
            if (InputModel.IrrigRunoffOptions == (int)IrrigationRunoffOptions.Proportional)
            {
                double factor;
                if (FirstIrrigation)
                {
                    factor = InputModel.IrrigRunoffProportion1 / 100.0;
                }
                else
                {
                    factor = InputModel.IrrigRunoffProportion2 / 100.0;
                }
                IrrigationRunoffAmount = factor * amount;

                amount = amount - IrrigationRunoffAmount;

                FirstIrrigation = false; //assign this to true when planting.
            }
            else if (InputModel.IrrigRunoffOptions == (int)IrrigationRunoffOptions.FromSequenceFile)
            {
                IrrigationRunoffAmount = InputModel.IrrigRunoffSequence.ValueAtDate(Sim.Today);
                if (IrrigationRunoffAmount < amount)
                    amount = amount - IrrigationRunoffAmount;
                else if (IrrigationRunoffAmount >= amount)
                {
                    IrrigationRunoffAmount = amount;
                    amount = 0;
                }
            }
            if (amount < 0)
            {
                amount = 0;
            }
            return amount;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        public void DistributeWaterThroughSoilLayers(double amount)
        {
            double targetlayeramount = 0;
            int layercount = Sim.SoilController.LayerCount;
            for (int i = 0; i < layercount; ++i)
            {
                double dul = Sim.SoilController.DrainUpperLimitRelWP[i];
                double sat = Sim.SoilController.SaturationLimitRelWP[i];
                switch (InputModel.TargetAmountOpt)
                {
                    case (int)TargetAmountOptions.FieldCapacity: targetlayeramount = dul; break;
                    case (int)TargetAmountOptions.Saturation: targetlayeramount = sat; break;
                    case (int)TargetAmountOptions.FixedAmount: targetlayeramount = sat; break;
                    case (int)TargetAmountOptions.DULplus25Percent: targetlayeramount = dul + 0.25 * (sat - dul); break;
                    case (int)TargetAmountOptions.DULplus50Percent: targetlayeramount = dul + 0.50 * (sat - dul); break;
                    case (int)TargetAmountOptions.DULplus75Percent: targetlayeramount = dul + 0.75 * (sat - dul); break;
                }
                double layerdef = targetlayeramount - Sim.SoilController.SoilWaterRelWP[i];
                if (amount > layerdef)
                {
                    Sim.SoilController.SoilWaterRelWP[i] = targetlayeramount;
                }
                else
                {
                    Sim.SoilController.SoilWaterRelWP[i] = Sim.SoilController.SoilWaterRelWP[i] + amount;
                }
                amount = amount - layerdef;
                if (amount < 0)
                {
                    i = layercount;
                }
            }
            Sim.SoilController.SoilWaterDeficit = 0;
            for (int i = 0; i < layercount; ++i)
            {
                Sim.SoilController.SoilWaterDeficit = Sim.SoilController.SoilWaterDeficit + (Sim.SoilController.DrainUpperLimitRelWP[i] - Sim.SoilController.SoilWaterRelWP[i]);
            }
            Sim.SoilController.Sse1 = Math.Max(0, Sim.SoilController.Sse1 - Sim.SoilController.SoilWaterDeficit);
            if (amount > 0)
            {
                Sim.SoilController.EffectiveRain += amount;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool PondingExists()
        {
            if (Sim.VegetationController.CurrentCrop != null &&
                Sim.VegetationController.CurrentCrop.IsGrowing())
            {
                return (CansimulateIrrigation() && InputModel.UsePonding && Sim.VegetationController.CurrentCrop.StillRequiresIrrigating());
            }
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
                if (CansimulateIrrigation() && InputModel.UseRingTank)
                {
                    double in_RingTankArea_ha = InputModel.RingTankArea * 10000.0;
                    if (InputModel.ResetRingTank && InputModel.ResetRingTankDate.MatchesDate(Sim.Today))
                        ResetRingTank();
                    else
                        SimulateDailyRingTankWaterBalance();
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
            double maxvolume = InputModel.RingTankDepth * InputModel.RingTankArea; //m^3
            double oldvolume = StorageVolume;
            StorageVolume = maxvolume * InputModel.CapactityAtReset / 100.0;
            RingTankIrrigationLosses = 0;
            RingTankEvaporationLosses = 0;
            RingTankSeepageLosses = 0;
            RingTankOvertoppingLosses = 0;
            RingTankTotalLosses = 0;
            RingTankRunoffCaptureInflow = 0;
            RingTankRainfalInflow = 0;
            RingTankEffectiveAdditionalInflow = 0;
            RingTankTotalAdditionalInflow = 0;
            RingTankTotalInflow = 0;
            RingTankIneffectiveAdditionalInflow = 0;
            RingTankStorageVolume = StorageVolume / 1000.0;

            if (!MathTools.DoublesAreEqual(InputModel.RingTankDepth * InputModel.RingTankArea, 0))
            {
                RingTankStorageLevel = StorageVolume / (InputModel.RingTankDepth * InputModel.RingTankArea) * 100.0;
            }
            else
            {
                RingTankStorageLevel = 0;
                MathTools.LogDivideByZeroError("ModelRingTank", "in_RingTankDepth_m*in_RingTankArea_ha", "out_RingTankStorageLevel_pc");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SimulateDailyRingTankWaterBalance()
        {
            //inflows
            double runoffCaptureInflowM3 = CalcRunoffCaptureInflow();
            double rainfallInflowM3 = Sim.ClimateController.Rain / 1000.0 * InputModel.RingTankArea;
            double additionalInflowM3 = GetAdditionalTankInflow();
            double totalInflowM3 = runoffCaptureInflowM3 + rainfallInflowM3 + additionalInflowM3;

            //losses
            double seepageLossesM3 = InputModel.RingTankSeepageRate / 1000.0 * InputModel.RingTankArea;
            double evaporationLossesM3 = Sim.ClimateController.PanEvap * InputModel.RingTankEvapCoefficient / 1000.0 * InputModel.RingTankArea;
            double seepagePlusEvapLossesM3 = seepageLossesM3 + evaporationLossesM3;

            if (seepagePlusEvapLossesM3 > StorageVolume)
            {
                seepagePlusEvapLossesM3 = StorageVolume;
                if (seepageLossesM3 < StorageVolume / 2.0)
                {
                    evaporationLossesM3 = StorageVolume - seepageLossesM3;
                }
                else if (evaporationLossesM3 < StorageVolume / 2.0)
                {
                    seepageLossesM3 = StorageVolume - evaporationLossesM3;
                }
                else
                {
                    seepageLossesM3 = StorageVolume / 2.0;
                    evaporationLossesM3 = StorageVolume / 2.0;
                }
            }
            //NOTE - Irrigation losses have already been extracted before we call this routine. Irrigation effectively
            // begins at the start of the day.

            //storage
            double potentialStorageVolumeM3 = CalcPotentialStorageVolume(totalInflowM3, seepagePlusEvapLossesM3);
            StorageVolume = CalcActualStorageVolume(totalInflowM3, seepagePlusEvapLossesM3, potentialStorageVolumeM3);

            //output variables
            RingTankIrrigationLosses = (IrrigationApplied / 1000.0 * InputModel.IrrigatedArea * 10000.0) / 1000.0 / (InputModel.IrrigDeliveryEfficiency / 100.0);
            RingTankEvaporationLosses = evaporationLossesM3 / 1000.0;
            RingTankSeepageLosses = seepageLossesM3 / 1000.0;
            RingTankOvertoppingLosses = CalcOvertoppingAmount(potentialStorageVolumeM3, StorageVolume);
            RingTankTotalLosses = RingTankIrrigationLosses + RingTankEvaporationLosses + RingTankSeepageLosses + RingTankOvertoppingLosses;
            RingTankRunoffCaptureInflow = runoffCaptureInflowM3 / 1000.0;
            RingTankRainfalInflow = rainfallInflowM3 / 1000.0;
            RingTankTotalAdditionalInflow = additionalInflowM3 / 1000.0;
            RingTankEffectiveAdditionalInflow = CalcEffectiveAdditionalInflow(RingTankOvertoppingLosses, additionalInflowM3);
            RingTankTotalInflow = RingTankRainfalInflow + RingTankRunoffCaptureInflow + additionalInflowM3 / 1000.0;
            RingTankIneffectiveAdditionalInflow = additionalInflowM3 / 1000.0 - RingTankEffectiveAdditionalInflow;
            RingTankStorageVolume = StorageVolume / 1000.0;
            RingTankStorageLevel = CalcStorageLevel(StorageVolume);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="potentialM3"></param>
        /// <param name="actualM3"></param>
        /// <returns></returns>
        public double CalcOvertoppingAmount(double potentialM3, double actualM3)
        {
            if (potentialM3 > actualM3)
            {
                return (potentialM3 - actualM3) / 1000.0;
            }
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
            double capacity_m3 = InputModel.RingTankDepth * InputModel.RingTankArea * 10000.0;
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

            double storagevolume = StorageVolume + inputs_m3 - outputs_m3;
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
            double capacity_m3 = InputModel.RingTankDepth * InputModel.RingTankArea * 10000.0; //m^3
            return (potentialvolume < capacity_m3 ? potentialvolume : capacity_m3);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double CalcRunoffCaptureInflow()
        {
            double runoffinflow = Sim.SoilController.Runoff / 1000.0 * InputModel.CatchmentArea * 10000;  //m^3
            double runoffcapturerate_m3 = InputModel.RunoffCaptureRate * 1000.0;
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
                if (InputModel.AdditionalInflowFormat == (int)RingTankAdditionalInflowFormat.Constant)
                {
                    value = InputModel.AdditionalInflow * 1000.0;         //converting to m3
                }
                else
                {
                    value = InputModel.AdditionalInflowSequence.ValueAtDate(Sim.Today) * 1000.0;
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
                if (InputModel.UseRingTank)
                {
                    Sum.IrrigationLosses += RingTankIrrigationLosses;
                    Sum.EvaporationLosses += RingTankEvaporationLosses;
                    Sum.SeepageLosses += RingTankSeepageLosses;
                    Sum.OvertoppingLosses += RingTankOvertoppingLosses;
                    Sum.RunoffCaptureInflow += RingTankRunoffCaptureInflow;
                    Sum.RainfallInflow += RingTankRainfalInflow;
                    Sum.EffectiveAdditionalnflow += RingTankEffectiveAdditionalInflow;
                    Sum.TotalAdditionalInflow += RingTankTotalAdditionalInflow;
                    Sum.StorageLevel += RingTankStorageLevel;
                    if (RingTankOvertoppingLosses > 0)
                    {
                        ++NumDaysOverflow;
                        if (Sim.Year != LastOvertoppingYear)
                        {
                            ++NumYearOverflow;
                            LastOvertoppingYear = Sim.Year;
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
            return (InputModel.IrrigRunoffOptions > 0 &&
                    InputModel.IrrigCoverEffects > 0 &&
                    IrrigationAmount > 0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetCoverEffect(double crop_cover, double total_residue_cover)
        {
            double cover = 0;
            if (InputModel.IrrigCoverEffects == 0)
            {
                cover = Math.Min(100.0, (crop_cover + total_residue_cover * (1 - crop_cover)) * 100.0);
            }
            else if (InputModel.IrrigCoverEffects == 1)
            {
                cover = Math.Min(100.0, (0 + total_residue_cover * (1 - 0)) * 100.0);
            }
            else if (InputModel.IrrigCoverEffects == 2)
            {
                cover = 0;
            }
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
            if (InputModel.UseRingTank)
            {
                double simyears = Sim.NumberOfDaysInSimulation / 365.0;
                SO.RingTankIrrigationLosses = MathTools.Divide(Sum.IrrigationLosses, simyears);
                SO.RingTankIrrigationLossesDelivered = SO.RingTankIrrigationLosses * InputModel.IrrigDeliveryEfficiency / 100.0;
                SO.RingTankEvaporationLosses = MathTools.Divide(Sum.EvaporationLosses, simyears);
                SO.RingTankSeepageLosses = MathTools.Divide(Sum.SeepageLosses, simyears);
                SO.RingTankOvertoppingLosses = MathTools.Divide(Sum.OvertoppingLosses, simyears);
                SO.RingTankRunoffCaptureInflow = MathTools.Divide(Sum.RunoffCaptureInflow, simyears);
                SO.RingTankRainfallInflow = MathTools.Divide(Sum.RainfallInflow, simyears);
                SO.RingTankEffectiveAdditionalInflow = MathTools.Divide(Sum.EffectiveAdditionalnflow, simyears);
                SO.RingTankAdditionalInflow = MathTools.Divide(Sum.TotalAdditionalInflow, simyears);
                SO.RingTanksStorageLevel = MathTools.Divide(Sum.StorageLevel, Sim.NumberOfDaysInSimulation);
                SO.RingTankPropDaysOverflow = MathTools.Divide(NumDaysOverflow, Sim.NumberOfDaysInSimulation) * 100.0;
                SO.RingTankPropYearsOverflow = MathTools.Divide(NumYearOverflow, simyears) * 100.0;
            }
        }
    }
}



