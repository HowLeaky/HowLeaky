using HowLeaky.CustomAttributes;
using HowLeaky.DataModels;
using HowLeaky.Interfaces;
using HowLeaky.ModelControllers.Veg;
using HowLeaky.OutputModels;
using HowLeaky.Tools.Helpers;
using System;
using System.Xml.Serialization;

namespace HowLeaky.ModelControllers.Pesticide
{
   
    public class PesticideObjectController : HLController, IChildController
    {
        public PesticideInputModel InputModel { get; set; }

        //Internals
        [XmlIgnore]
        public int DaysSinceApplication { get; set; }
        [XmlIgnore]
        public int PestApplicCount { get; set; }
        [XmlIgnore]
        public int ApplicationIndex { get; set; }
        [XmlIgnore]
        [Unit("L/ha")]
        public double ProductRateApplied { get; set; }
        [XmlIgnore]
        [Unit("mg/kg")]
        public double ConcSoilAfterLeach { get; set; }
        [XmlIgnore]
        [Unit("g/ha")]
        public double LastPestInput { get; set; }
        [XmlIgnore]
        public int PesticideIndex
        {
            get
            {
                return Sim.PesticideController.GetPesticideIndex(this);
            }
        }

        //Reportable Outputs
        [Output("Applied pest on veg", "g/ha")]
        public double AppliedPestOnVeg { get; set; }
        [Output("Applied pest on stubble", "g/ha")]
        public double AppliedPestOnStubble { get; set; }
        [Output("Applied pest on soil", "(g/ha")]
        public double AppliedPestOnSoil { get; set; }
        [Output("Pest on veg", "g/ha")]
        public double PestOnVeg { get; set; }
        [Output("Pest on stubble", "g/ha")]
        public double PestOnStubble { get; set; }
        [Output("Pest in soil", "g/ha")]
        public double PestInSoil { get; set; }
        [Output("Pest soil conc.", "mg/kg")]
        public double PestSoilConc { get; set; }
        [Output("Pest sediment phase conc.", "mg/kg")]
        public double PestSedPhaseConc { get; set; }
        [Output("Pest water phase conc.", "ug/L")]
        public double PestWaterPhaseConc { get; set; }
        [Output("Pest runoff conc. (water+sediment)", "ug/L")]
        public double PestRunoffConc { get; set; }
        //[Output("Sediment delivered", "g/L")]
        //public double SedimentDelivered { get; set; }
        [Output("Pest lost in runoff water", "g/ha")]
        public double PestLostInRunoffWater { get; set; }
        [Output("Pest lost in runoff sediment", "g/ha")]
        public double PestLostInRunoffSediment { get; set; }
        [Output("Total pest lost in runoff", "g/ha")]
        public double TotalPestLostInRunoff { get; set; }
        [Output("Pest lost in leaching", "g/ha")]
        public double PestLostInLeaching { get; set; }
        [Output("Pest losses as percent of last input", "%")]
        public double PestLossesPercentOfInput { get; set; }
        [Output("Number of applications", "")]
        public double ApplicationCount { get; set; }
        [Output("Avg Product Application", "g/ha/year")]
        public double ProductApplication { get; set; }
        [Output("Avg Bound Pest Conc in Runoff", "ug/l")]
        public double AvgBoundPestConcInRunoff { get; set; }
        [Output("Avg Unbound Pest Conc in Runoff", "ug/l")]
        public double AvgUnboundPestConcInRunoff { get; set; }
        [Output("Avg Combined Pest Conc in Runoff", "ug/l")]
        public double AvgCombinedPestConcInRunoff { get; set; }
        [Output("Avg Pest Load (Water)", "g/ha/yr")]
        public double AvgPestLoadWater { get; set; }
        [Output("Avg Pest Load (Sed)", "g/ha/yr")]
        public double AvgPestLoadSediment { get; set; }
        [Output("Avg Pest Load (Total)", "g/ha/yr")]
        public double AvgTotalPestLoad { get; set; }
        [Output("Loss/Application Ratio", "%")]
        public double ApplicationLossRatio { get; set; }
        [Output("Avg Days Conc > Crit", "days/yr")]
        public int DaysGreaterCrit1 { get; set; }
        [Output("Avg Days Conc > 0.5 Crit", "days/yr")]
        public int DaysGreaterCrit2 { get; set; }
        [Output("Avg Days Conc > 2 Crit", "days/yr")]
        public int DaysGreaterCrit3 { get; set; }
        [Output("Avg Days Conc > 10 Crit", "days/yr")]
        public int DaysGreaterCrit4 { get; set; }
        [Output("Pesticide EMC", "ug/L")]
        public double PestEMCL { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PesticideObjectController(Simulation sim) : base(sim)
        {
  
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sim"></param>
        public PesticideObjectController(Simulation sim, PesticideInputModel dataModel) : this(sim)
        {
            InputModel = dataModel;

            InitOutputModel();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Simulate()
        {
            if (PestApplicCount > 0)
            {
                ++DaysSinceApplication;
            }
            //check inputs 
            ApplyAnyNewPesticides();

            //calculate pest store
            CalculateDegradingPestOnVeg();
            CalculateDegradingPestOnStubble();
            CalculateDegradingPestInSoil();

            //generate output values
            CalculatePesticideRunoffConcentrations();
            CalculatePesticideLosses();
            CalculatePesticideDaysAboveCritical();
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
        public void ApplyAnyNewPesticides()
        {

            AppliedPestOnVeg = 0;
            AppliedPestOnStubble = 0;
            AppliedPestOnSoil = 0;

            ResetPesticideInputs();
            if (InputModel.ApplicationTiming == (int)EPestApplicationTiming.FixedDate)
            {
                if (Sim.Day == InputModel.ApplicationDate.Day && Sim.Month == InputModel.ApplicationDate.Month)
                {
                    ProductRateApplied = InputModel.ProductRate;
                }
            }
            else if (InputModel.ApplicationTiming == (int)EPestApplicationTiming.FromSequenceFile)
            {
                //int index = DateUtilities.isDateInSequenceList(Sim.Today, InputModel.PestApplicationTiming.PesticideDatesAndRates);
                int index = InputModel.PestApplicationTiming.PesticideDatesAndRates.Dates.IndexOf(Sim.Today);
                if (index >= 0 && index < InputModel.PestApplicationTiming.PesticideDatesAndRates.Values.Count)
                {
                    ProductRateApplied = InputModel.PestApplicationTiming.PesticideDatesAndRates.Values[index];
                }
            }
            else
            {
                LAIVegObjectController crop = (LAIVegObjectController)Sim.VegetationController.CurrentCrop;
                if (crop != null)
                {
                    if (crop.CropStatus != CropStatus.Fallow)
                    {
                        if (MathTools.DoublesAreEqual(crop.HeatUnitIndex, 0))
                        {
                            ApplicationIndex = 0;
                        }
                        if (InputModel.ApplicationTiming == (int)EPestApplicationTiming.GDDCrop1 && crop == Sim.VegetationController.GetCrop(0))
                        {
                            CheckApplicationBasedOnGDD(crop);
                        }
                        else if (InputModel.ApplicationTiming == (int)EPestApplicationTiming.GDDCrop2 && crop == Sim.VegetationController.GetCrop(1))
                        {
                            CheckApplicationBasedOnGDD(crop);
                        }
                        else if (InputModel.ApplicationTiming == (int)EPestApplicationTiming.GDDCrop3 && crop == Sim.VegetationController.GetCrop(2))
                        {
                            CheckApplicationBasedOnGDD(crop);
                        }
                        else if (InputModel.ApplicationTiming == (int)EPestApplicationTiming.DASCrop1 && crop == Sim.VegetationController.GetCrop(0))
                        {
                            CheckApplicationBasedOnDAS(crop);
                        }
                        else if (InputModel.ApplicationTiming == (int)EPestApplicationTiming.DASCrop2 && crop == Sim.VegetationController.GetCrop(1))
                        {
                            CheckApplicationBasedOnDAS(crop);
                        }
                        else if (InputModel.ApplicationTiming == (int)EPestApplicationTiming.DASCrop3 && crop == Sim.VegetationController.GetCrop(2))
                        {
                            CheckApplicationBasedOnDAS(crop);
                        }
                    }
                    else if (crop.CropStatus == CropStatus.Fallow && InputModel.ApplicationTiming == (int)EPestApplicationTiming.DaysSinceFallow)
                    {
                        CheckApplicationBasedOnDAH();
                    }
                }
            }
            if (ProductRateApplied > 0)
                ApplyPesticide();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="crop"></param>
        public void CheckApplicationBasedOnGDD(LAIVegObjectController crop)
        {
            if (ApplicationIndex == 0 && crop.HeatUnits >= InputModel.TriggerGGDFirst)
            {
                ProductRateApplied = InputModel.ProductRate;
                ++ApplicationIndex;
            }
            else if (ApplicationIndex > 0 && crop.HeatUnits >= InputModel.TriggerGGDFirst + InputModel.TriggerGGDSubsequent * ApplicationIndex)
            {
                ProductRateApplied = InputModel.SubsequentProductRate;
                ++ApplicationIndex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="crop"></param>
        public void CheckApplicationBasedOnDAS(VegObjectController crop)
        {
            if (ApplicationIndex == 0 && crop.DaysSincePlanting >= InputModel.TriggerDaysFirst)
            {
                ProductRateApplied = InputModel.ProductRate;
                ++ApplicationIndex;
            }
            else if (ApplicationIndex > 0 && crop.DaysSincePlanting >= InputModel.TriggerDaysFirst + InputModel.TriggerDaysSubsequent * ApplicationIndex)
            {
                ProductRateApplied = InputModel.SubsequentProductRate;
                ++ApplicationIndex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CheckApplicationBasedOnDAH()
        {
            int days_since_harvest = Sim.VegetationController.GetDaysSinceHarvest();
            if (days_since_harvest == 0) ApplicationIndex = 0;
            if (ApplicationIndex == 0 && days_since_harvest >= InputModel.TriggerDaysFirst)
            {
                ProductRateApplied = InputModel.ProductRate;
                ++ApplicationIndex;
            }
            else if (ApplicationIndex > 0 && days_since_harvest >= InputModel.TriggerDaysFirst + InputModel.TriggerDaysSubsequent * ApplicationIndex)
            {
                ProductRateApplied = InputModel.SubsequentProductRate;
                ++ApplicationIndex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetPesticideInputs()
        {
            ProductRateApplied = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ApplyPesticide()
        {

            Sim.UpdateManagementEventHistory(ManagementEvent.Pesticide, PesticideIndex);
            DaysSinceApplication = 0;
            EPestApplicationPosition pos = (EPestApplicationPosition)InputModel.ApplicationPosition;
            double pest_application = InputModel.ConcActiveIngred * ProductRateApplied * InputModel.PestEfficiency / 100.0 * InputModel.BandSpraying / 100.0;
            LastPestInput = pest_application;

            if (pos == EPestApplicationPosition.ApplyToVegetationLayer)
            {
                AppliedPestOnVeg = pest_application * Sim.SoilController.CropCover;
            }
            else
            {
                AppliedPestOnVeg = 0;
            }
            if (pos == EPestApplicationPosition.ApplyToVegetationLayer || pos == EPestApplicationPosition.ApplyToStubbleLayer)
            {
                double stubble_cover = (1 - Sim.SoilController.CropCover) * Sim.SoilController.TotalResidueCover;
                AppliedPestOnStubble = pest_application * stubble_cover;
            }
            else
            {
                AppliedPestOnStubble = 0;
            }


            AppliedPestOnSoil = pest_application - AppliedPestOnVeg - AppliedPestOnStubble;
            ++PestApplicCount;
        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculateDegradingPestOnVeg()
        {
            double halfLifeVegAdjusted = 0;
            double universalGasConstant = 8.314472;
            double refAirTempVegKelvin = InputModel.RefTempHalfLifeVeg + 273.15;
            double airTempKelvin = ((Sim.ClimateController.MaxTemp + Sim.ClimateController.MinTemp) / 2.0) + 273.15;
            if (!MathTools.DoublesAreEqual(airTempKelvin, 0) && !MathTools.DoublesAreEqual(refAirTempVegKelvin, 0))
                halfLifeVegAdjusted = InputModel.HalfLifeVeg * Math.Exp((InputModel.DegradationActivationEnergy / universalGasConstant) * (1.0 / airTempKelvin - 1.0 / refAirTempVegKelvin));
            else
            {
                MathTools.LogDivideByZeroError("CalculateDegradingPestOnVeg", "AirTemperature_kelvin!=0 or Ref_AirTemperatureVeg_kelvin", "HalfLifeVeg_adjusted");
            }
            double vegdegrate;
            if (!MathTools.DoublesAreEqual(halfLifeVegAdjusted, 0))
            {
                vegdegrate = Math.Exp(-0.693 / halfLifeVegAdjusted);
            }
            else
            {
                vegdegrate = 0;
                MathTools.LogDivideByZeroError("CalculateDegradingPestOnVeg", "HalfLifeVeg_adjusted", "vegdegrate");
            }
            if (Sim.ClimateController.YesterdaysRain < 5.0)
            {
                PestOnVeg = PestOnVeg * vegdegrate + AppliedPestOnVeg;
                if (Sim.ClimateController.Rain >= 5) //rain over 5mm will wash part of pest of veg
                {
                    PestOnVeg = PestOnVeg * (1 - InputModel.CoverWashoffFraction);
                }
            }
            else
            {
                PestOnVeg = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculateDegradingPestOnStubble()
        {
            double halfLifeStubbleAdjusted = 0;
            double universalGasConstant = 8.314472;
            double refAirTempStubbleKelvin = InputModel.RefTempHalfLifeStubble + 273.15;
            double airTempKelvin = ((Sim.ClimateController.MaxTemp + Sim.ClimateController.MinTemp) / 2.0) + 273.15;
            if (!MathTools.DoublesAreEqual(airTempKelvin, 0) && !MathTools.DoublesAreEqual(refAirTempStubbleKelvin, 0))
            {
                halfLifeStubbleAdjusted = InputModel.HalfLifeStubble * Math.Exp((InputModel.DegradationActivationEnergy / universalGasConstant) * (1.0 / airTempKelvin - 1.0 / refAirTempStubbleKelvin));
            }
            else
            {
                MathTools.LogDivideByZeroError("CalculateDegradingPestOnStubble", "AirTemperature_kelvin or Ref_AirTemperatureStubble_kelvin", "HalfLifeStubble_adjusted");
            }

            double stubdegrate;
            if (!MathTools.DoublesAreEqual(halfLifeStubbleAdjusted, 0))
            {
                stubdegrate = Math.Exp(-0.693 / halfLifeStubbleAdjusted);
            }
            else
            {
                stubdegrate = 0;
                MathTools.LogDivideByZeroError("CalculateDegradingPestOnStubble", "HalfLifeStubble_adjusted", "stubdegrate");
            }
            if (Sim.ClimateController.YesterdaysRain < 5.0)
            {
                PestOnStubble = PestOnStubble * stubdegrate + AppliedPestOnStubble;
                if (Sim.ClimateController.Rain >= 5) //rain over 5mm will wash part of pest of stubble
                {
                    PestOnStubble = PestOnStubble * (1 - InputModel.CoverWashoffFraction);
                }
            }
            else
            {
                PestOnStubble = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculateDegradingPestInSoil()
        {
            double halflifesoil_adjusted = 0;
            double universalgasconstant = 8.314472;
            double ref_airtempsoil_kelvin = InputModel.RefTempHalfLifeSoil + 273.15;
            double airtemp_kelvin = ((Sim.ClimateController.MaxTemp + Sim.ClimateController.MinTemp) / 2.0) + 273.15;
            if (!MathTools.DoublesAreEqual(airtemp_kelvin, 0) && !MathTools.DoublesAreEqual(ref_airtempsoil_kelvin, 0))
            {
                halflifesoil_adjusted = InputModel.HalfLife * Math.Exp((InputModel.DegradationActivationEnergy / universalgasconstant) * (1.0 / airtemp_kelvin - 1.0 / ref_airtempsoil_kelvin));
            }
            else
            {
                MathTools.LogDivideByZeroError("CalculateDegradingPestInSoil", "AirTemperature_kelvin or Ref_AirTemperatureSoil_kelvin", "HalfLifeSoil_adjusted");
            }
            double denom;
            double soildegrate;
            if (halflifesoil_adjusted > 0)
            {
                soildegrate = Math.Exp(-0.693 / halflifesoil_adjusted);
            }
            else
            {
                soildegrate = 0;
                MathTools.LogDivideByZeroError("CalculateDegradingPestInSoil", "HalfLifeSoil_adjusted", "soildegrate");
            }

            PestInSoil = PestInSoil * soildegrate + AppliedPestOnSoil - PestLostInLeaching - TotalPestLostInRunoff;
            if (Sim.ClimateController.Rain >= 5.0)
            {
                PestInSoil = PestInSoil + (PestOnStubble + PestOnVeg) * InputModel.CoverWashoffFraction;
            }


            denom = (Sim.SoilController.InputModel.BulkDensity.Values[0] * InputModel.MixLayerThickness * 10);
            if (!MathTools.DoublesAreEqual(denom, 0))
            {
                PestSoilConc = PestInSoil / denom;
            }
            else
            {
                PestSoilConc = 0;
                MathTools.LogDivideByZeroError("CalculateDegradingPestInSoil", "sim.in_BulkDensity_g_per_cm3[0]*in_MixLayerThickness_mm*10", "out_PestSoilConc_mg_per_kg");
            }


            double porosity = 1 - Sim.SoilController.InputModel.BulkDensity.Values[0] / 2.65;

            //calculate the denominator of the PestConcInSoilAfterLeaching Equation - need to test for denom=0
            denom = InputModel.MixLayerThickness * (InputModel.SorptionCoefficient * Sim.SoilController.InputModel.BulkDensity.Values[0] + porosity);

            double availwaterstorageinmixing;
            if (!MathTools.DoublesAreEqual(Sim.SoilController.Depth[1], 0))
            {
                availwaterstorageinmixing = (Sim.SoilController.DrainUpperLimitRelWP[0] - Sim.SoilController.SoilWaterRelWP[0]) * InputModel.MixLayerThickness / Sim.SoilController.Depth[1];
            }
            else
            {
                availwaterstorageinmixing = 0;
                MathTools.LogDivideByZeroError("CalculateDegradingPestInSoil", "sim.depth[1]", "availwaterstorageinmixing");
            }
            if (!MathTools.DoublesAreEqual(denom, 0))
            {
                double infiltration = Sim.ClimateController.Rain - Sim.SoilController.Runoff - availwaterstorageinmixing;
                if (infiltration < 0)
                {
                    infiltration = 0;
                }
                ConcSoilAfterLeach = PestSoilConc * Math.Exp(-infiltration / (denom));
            }
            else
            {
                ConcSoilAfterLeach = 0;
                MathTools.LogDivideByZeroError("CalculateDegradingPestInSoil", "sim.in_BulkDensity_g_per_cm3[0]*in_MixLayerThickness_mm*10", "conc_in_soil_after_leach_mg_per_kg");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculatePesticideRunoffConcentrations()
        {
            double sorpBYext = InputModel.SorptionCoefficient * InputModel.ExtractCoefficient;
            double denom1 = (1 + sorpBYext);

            if (Sim.SoilController.Runoff > 0 && PestSoilConc > 0 && !MathTools.DoublesAreEqual(denom1, 0))
            {
                PestWaterPhaseConc = ConcSoilAfterLeach * InputModel.ExtractCoefficient / denom1 * 1000.0;
                PestSedPhaseConc = ConcSoilAfterLeach * sorpBYext / denom1;
                PestRunoffConc = PestWaterPhaseConc + PestSedPhaseConc * Sim.SoilController.SedimentConc;
            }
            else
            {
                if (MathTools.DoublesAreEqual(1 + sorpBYext, 0))
                {
                    MathTools.LogDivideByZeroError("CalculatePesticideRunoffConcentrations", "1+sorpBYext", "3 x pest-concs");
                }
                PestWaterPhaseConc = 0;
                PestSedPhaseConc = 0;
                PestRunoffConc = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculatePesticideLosses()
        {
            if (Sim.SoilController.Runoff > 0)
            {
                PestLostInRunoffWater = PestWaterPhaseConc * Sim.SoilController.Runoff * 0.01;
                PestLostInRunoffSediment = PestSedPhaseConc * Sim.SoilController.HillSlopeErosion * Sim.SoilController.InputModel.SedDelivRatio;// spreadsheet uses runoff instead of erosion*SelDelivRatio
                TotalPestLostInRunoff = PestLostInRunoffWater + PestLostInRunoffSediment;

            }
            else
            {
                PestLostInRunoffWater = 0;
                PestLostInRunoffSediment = 0;
                TotalPestLostInRunoff = 0;
            }
            PestLostInLeaching = (PestSoilConc - ConcSoilAfterLeach) * Sim.SoilController.InputModel.BulkDensity.Values[0] * InputModel.MixLayerThickness / 10.0;
            if (PestLostInLeaching < 0)
            {
                PestLostInLeaching = 0;
            }
            if (!MathTools.DoublesAreEqual(LastPestInput, 0))
            {
                PestLossesPercentOfInput = (TotalPestLostInRunoff + PestLostInLeaching) / LastPestInput * 100.0;
            }
            else
            {
                PestLossesPercentOfInput = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculatePesticideDaysAboveCritical()
        {
            if (PestRunoffConc > InputModel.CritPestConc * 1)
            {
                DaysGreaterCrit1++;
            }
            if (PestRunoffConc > InputModel.CritPestConc * 0.5)
            {
                DaysGreaterCrit2++;
            }
            if (PestRunoffConc > InputModel.CritPestConc * 2)
            {
                DaysGreaterCrit3++;
            }
            if (PestRunoffConc > InputModel.CritPestConc * 10)
            {
                DaysGreaterCrit4++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetPesticideIndex()
        {
            return Sim.PesticideController.GetPesticideIndex(this);
        }
    }
}
