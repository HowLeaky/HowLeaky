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
    public class PesticideOutputDataModel : OutputDataModel, IDailyOutput
    {
        [Output]
        [XmlIgnore]
        [Unit("g_per_ha")]
        public double AppliedPestOnVeg { get; set; }              // Applied pest on veg (g/ha)
        [Output]
        [XmlIgnore]
        [Unit("g_per_ha")]
        public double AppliedPestOnStubble { get; set; }          // Applied pest on stubble (g/ha
        [Output]
        [XmlIgnore]
        [Unit("g_per_ha")]
        public double AppliedPestOnSoil { get; set; }             // Applied pest on soil (g/ha)
        [Output]
        [XmlIgnore]
        [Unit("g_per_ha")]
        public double PestOnVeg { get; set; }                     // Pest on veg (g/ha)
        [Output]
        [XmlIgnore]
        [Unit("g_per_ha")]
        public double PestOnStubble { get; set; }                 // Pest on stubble (g/ha)
        [Output]
        [XmlIgnore]
        [Unit("g_per_ha")]
        public double PestInSoil { get; set; }                    // Pest in soil (g/ha)
        [Output]
        [XmlIgnore]
        [Unit("mg_per_kg")]
        public double PestSoilConc { get; set; }                 // Pest soil conc. (mg/kg)
        [Output]
        [XmlIgnore]
        [Unit("mg_per_kg")]
        public double PestSedPhaseConc { get; set; }             // Pest sediment phase conc. (mg/kg)
        [Output]
        [XmlIgnore]
        [Unit("ug_per_L")]
        public double PestWaterPhaseConc { get; set; }            // Pest water phase conc. (ug/L)
        [Output]
        [XmlIgnore]
        [Unit("ug_per_L")]
        public double PestRunoffConc { get; set; }                // Pest runoff conc. (water+sediment) (ug/L)
        [Output]
        [XmlIgnore]
        [Unit("g_per_L")]
        public double SedimentDelivered { get; set; }              // Sediment delivered (g/L)
        [Output]
        [XmlIgnore]
        [Unit("g_per_ha")]
        public double PestLostInRunoffWater { get; set; }         // Pest lost in runoff water (g/ha)
        [Output]
        [XmlIgnore]
        [Unit("g_per_ha")]
        public double PestLostInRunoffSediment { get; set; }      // Pest lost in runoff sediment (g/ha)
        [Output]
        [XmlIgnore]
        [Unit("g_per_ha")]
        public double TotalPestLostInRunoff { get; set; }         // Total pest lost in runoff (g/ha)
        [Output]
        [XmlIgnore]
        [Unit("g_per_ha")]
        public double PestLostInLeaching { get; set; }            // Pest lost in leaching (g/ha)
        [Output]
        [XmlIgnore]
        [Unit("pc")]
        public double PestLossesPercentOfInput { get; set; }            // Pest losses as percent of last input (%)
        [Output]
        [XmlIgnore]
        public double ApplicationCount { get; set; }                       // Number of applications
        [Output]
        [XmlIgnore]
        [Unit("g_per_ha")]
        public double ProductApplication { get; set; }            // Avg Product Application (g/ha/year)
        [Output]
        [XmlIgnore]
        [Unit("ug_per_L")]
        public double AvgBoundPestConcInRunoff { get; set; }      // Avg Bound Pest Conc in Runoff (ug/l)
        [Output]
        [XmlIgnore]
        [Unit("ug_per_L")]
        public double AvgUnboundPestConcInRunoff { get; set; }    // Avg Unbound Pest Conc in Runoff (ug/l)
        [Output]
        [XmlIgnore]
        [Unit("ug_per_L")]
        public double AvgCombinedPestConcInRunoff { get; set; }   // Avg Combined Pest Conc in Runoff (ug/l)
        [Output]
        [XmlIgnore]
        [Unit("g_per_ha")]
        public double AvgPestLoadWater { get; set; }              // Avg Pest Load (Water) (g/ha/yr)
        [Output]
        [XmlIgnore]
        [Unit("g_per_ha")]
        public double AvgPestLoadSediment { get; set; }           // Avg Pest Load (Sed) (g/ha/yr)
        [Output]
        [XmlIgnore]
        [Unit("g_per_ha")]
        public double AvgTotalPestLoad { get; set; }              // Avg Pest Load (Total) (g/ha/yr)
        [Output]
        [XmlIgnore]
        [Unit("pc")]
        public double ApplicationLossRatio { get; set; }                // Loss/Application Ratio (%)
        [Output]
        [XmlIgnore]
        [Unit("days")]
        public int DaysGreaterCrit1 { get; set; }                  // Avg Days Conc > Crit (days/yr)
        [Output]
        [XmlIgnore]
        [Unit("days")]
        public int DaysGreaterCrit2 { get; set; }                  // Avg Days Conc > 0.5 Crit (days/yr)
        [Output]
        [XmlIgnore]
        [Unit("days")]
        public int DaysGreaterCrit3 { get; set; }                  // Avg Days Conc > 2 Crit (days/yr)
        [Output]
        [XmlIgnore]
        [Unit("days")]
        public int DaysGreaterCrit4 { get; set; }                  // Avg Days Conc > 10 Crit (days/yr)
        [Output]
        [XmlIgnore]
        [Unit("ug_per_L")]
        public double PestEMCL { get; set; }                       // Pesticide EMC (ug/L)
    }

    public class PesticideObjectController : HLController
    {
        public PesticideObjectDataModel DataModel { get; set; }
        public PesticideOutputDataModel Output { get; set; }

        //Internals
        [Internal]
        [XmlIgnore]
        public int DaysSinceApplication { get; set; }
        [Internal]
        [XmlIgnore]
        public int PestApplicCount { get; set; }
        [Internal]
        [XmlIgnore]
        public int ApplicationIndex { get; set; }
        [Internal]
        [XmlIgnore]
        [Unit("L_per_ha")]
        public double ProductRateApplied { get; set; }
        [Internal]
        [XmlIgnore]
        [Unit("mg_per_kg")]//We set up this temp variable as there are many types of inputs that can be assigned
        public double ConcSoilAfterLeach { get; set; }
        [Internal]
        [XmlIgnore]
        [Unit("g_per_ha")]
        public double LastPestInput { get; set; }
        [Internal]
        [XmlIgnore]
        public int PesticideIndex
        {
            get
            {
                return Sim.PesticideController.GetPesticideIndex(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public PesticideObjectController(Simulation sim) : base(sim)
        {
            Output = new PesticideOutputDataModel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sim"></param>
        public PesticideObjectController(Simulation sim, PesticideObjectDataModel dataModel) : this(sim)
        {
            DataModel = dataModel;
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
        public void ApplyAnyNewPesticides()
        {
            ResetPesticideInputs();
            if (DataModel.ApplicationTiming == (int)EPestApplicationTiming.FixedDate)
            {
                if (Sim.Day == DataModel.ApplicationDate.Day && Sim.Month == DataModel.ApplicationDate.Month)
                {
                    ProductRateApplied = DataModel.ProductRate;
                }
            }
            else if (DataModel.ApplicationTiming == (int)EPestApplicationTiming.FromSequenceFile)
            {
                int index = DateUtilities.isDateInSequenceList(Sim.Today, DataModel.PestApplicationDateList);
                if (index >= 0 && index < DataModel.PestApplicationValueList.Count)
                {
                    ProductRateApplied = DataModel.PestApplicationValueList[index];
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
                        if (DataModel.ApplicationTiming == (int)EPestApplicationTiming.GDDCrop1 && crop == Sim.VegetationController.GetCrop(0))
                        {
                            CheckApplicationBasedOnGDD(crop);
                        }
                        else if (DataModel.ApplicationTiming == (int)EPestApplicationTiming.GDDCrop2 && crop == Sim.VegetationController.GetCrop(1))
                        {
                            CheckApplicationBasedOnGDD(crop);
                        }
                        else if (DataModel.ApplicationTiming == (int)EPestApplicationTiming.GDDCrop3 && crop == Sim.VegetationController.GetCrop(2))
                        {
                            CheckApplicationBasedOnGDD(crop);
                        }
                        else if (DataModel.ApplicationTiming == (int)EPestApplicationTiming.DASCrop1 && crop == Sim.VegetationController.GetCrop(0))
                        {
                            CheckApplicationBasedOnDAS(crop);
                        }
                        else if (DataModel.ApplicationTiming == (int)EPestApplicationTiming.DASCrop2 && crop == Sim.VegetationController.GetCrop(1))
                        {
                            CheckApplicationBasedOnDAS(crop);
                        }
                        else if (DataModel.ApplicationTiming == (int)EPestApplicationTiming.DASCrop3 && crop == Sim.VegetationController.GetCrop(2))
                        {
                            CheckApplicationBasedOnDAS(crop);
                        }
                    }
                    else if (crop.CropStatus == CropStatus.Fallow && DataModel.ApplicationTiming == (int)EPestApplicationTiming.DaysSinceFallow)
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
            if (ApplicationIndex == 0 && crop.HeatUnits >= DataModel.TriggerGGDFirst)
            {
                ProductRateApplied = DataModel.ProductRate;
                ++ApplicationIndex;
            }
            else if (ApplicationIndex > 0 && crop.HeatUnits >= DataModel.TriggerGGDFirst + DataModel.TriggerGGDSubsequent * ApplicationIndex)
            {
                ProductRateApplied = DataModel.SubsequentProductRate;
                ++ApplicationIndex;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="crop"></param>
        public void CheckApplicationBasedOnDAS(VegObjectController crop)
        {
            if (ApplicationIndex == 0 && crop.DaysSincePlanting >= DataModel.TriggerDaysFirst)
            {
                ProductRateApplied = DataModel.ProductRate;
                ++ApplicationIndex;
            }
            else if (ApplicationIndex > 0 && crop.DaysSincePlanting >= DataModel.TriggerDaysFirst + DataModel.TriggerDaysSubsequent * ApplicationIndex)
            {
                ProductRateApplied = DataModel.SubsequentProductRate;
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
            if (ApplicationIndex == 0 && days_since_harvest >= DataModel.TriggerDaysFirst)
            {
                ProductRateApplied = DataModel.ProductRate;
                ++ApplicationIndex;
            }
            else if (ApplicationIndex > 0 && days_since_harvest >= DataModel.TriggerDaysFirst + DataModel.TriggerDaysSubsequent * ApplicationIndex)
            {
                ProductRateApplied = DataModel.SubsequentProductRate;
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
            EPestApplicationPosition pos = (EPestApplicationPosition)DataModel.ApplicationPosition;
            double pest_application = DataModel.ConcActiveIngred * ProductRateApplied * DataModel.PestEfficiency / 100.0 * DataModel.BandSpraying / 100.0;
            LastPestInput = pest_application;

            if (pos == EPestApplicationPosition.ApplyToVegetationLayer)
            {
                Output.AppliedPestOnVeg = pest_application * Sim.SoilController.CropCover;
            }
            else
            {
                Output.AppliedPestOnVeg = 0;
            }
            if (pos == EPestApplicationPosition.ApplyToVegetationLayer || pos == EPestApplicationPosition.ApplyToStubbleLayer)
            {
                double stubble_cover = (1 - Sim.SoilController.CropCover) * Sim.SoilController.TotalResidueCover;
                Output.AppliedPestOnStubble = pest_application * stubble_cover;
            }
            else
            {
                Output.AppliedPestOnStubble = 0;
            }


            Output.AppliedPestOnSoil = pest_application - Output.AppliedPestOnVeg - Output.AppliedPestOnStubble;
            ++PestApplicCount;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void CalculateDegradingPestOnVeg()
        {
            double halfLifeVegAdjusted = 0;
            double universalGasConstant = 8.314472;
            double refAirTempVegKelvin = DataModel.RefTempHalfLifeVeg + 273.15;
            double airTempKelvin = ((Sim.ClimateController.MaxT + Sim.ClimateController.MinT) / 2.0) + 273.15;
            if (!MathTools.DoublesAreEqual(airTempKelvin, 0) && !MathTools.DoublesAreEqual(refAirTempVegKelvin, 0))
                halfLifeVegAdjusted = DataModel.HalfLifeVeg * Math.Exp((DataModel.DegradationActivationEnergy / universalGasConstant) * (1.0 / airTempKelvin - 1.0 / refAirTempVegKelvin));
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
                Output.PestOnVeg = Output.PestOnVeg * vegdegrate + Output.AppliedPestOnVeg;
                if (Sim.ClimateController.Rain >= 5) //rain over 5mm will wash part of pest of veg
                {
                    Output.PestOnVeg = Output.PestOnVeg * (1 - DataModel.CoverWashoffFraction);
                }
            }
            else
            {
                Output.PestOnVeg = 0;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void CalculateDegradingPestOnStubble()
        {
            double halfLifeStubbleAdjusted = 0;
            double universalGasConstant = 8.314472;
            double refAirTempStubbleKelvin = DataModel.RefTempHalfLifeStubble + 273.15;
            double airTempKelvin = ((Sim.ClimateController.MaxT + Sim.ClimateController.MinT) / 2.0) + 273.15;
            if (!MathTools.DoublesAreEqual(airTempKelvin, 0) && !MathTools.DoublesAreEqual(refAirTempStubbleKelvin, 0))
            {
                halfLifeStubbleAdjusted = DataModel.HalfLifeStubble * Math.Exp((DataModel.DegradationActivationEnergy / universalGasConstant) * (1.0 / airTempKelvin - 1.0 / refAirTempStubbleKelvin));
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
                Output.PestOnStubble = Output.PestOnStubble * stubdegrate + Output.AppliedPestOnStubble;
                if (Sim.ClimateController.Rain >= 5) //rain over 5mm will wash part of pest of stubble
                {
                    Output.PestOnStubble = Output.PestOnStubble * (1 - DataModel.CoverWashoffFraction);
                }
            }
            else
            {
                Output.PestOnStubble = 0;
            }
        }
       
        /// <summary>
        /// 
        /// </summary>
        public void CalculateDegradingPestInSoil()
        {
            double halflifesoil_adjusted = 0;
            double universalgasconstant = 8.314472;
            double ref_airtempsoil_kelvin = DataModel.RefTempHalfLifeSoil + 273.15;
            double airtemp_kelvin = ((Sim.ClimateController.MaxT + Sim.ClimateController.MinT) / 2.0) + 273.15;
            if (!MathTools.DoublesAreEqual(airtemp_kelvin, 0) && !MathTools.DoublesAreEqual(ref_airtempsoil_kelvin, 0))
            {
                halflifesoil_adjusted = DataModel.HalfLife * Math.Exp((DataModel.DegradationActivationEnergy / universalgasconstant) * (1.0 / airtemp_kelvin - 1.0 / ref_airtempsoil_kelvin));
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

            Output.PestInSoil = Output.PestInSoil * soildegrate + Output.AppliedPestOnSoil - Output.PestLostInLeaching - Output.TotalPestLostInRunoff;
            if (Sim.ClimateController.Rain >= 5.0)
            {
                Output.PestInSoil = Output.PestInSoil + (Output.PestOnStubble + Output.PestOnVeg) * DataModel.CoverWashoffFraction;
            }


            denom = (Sim.SoilController.DataModel.BulkDensity.Values[0] * DataModel.MixLayerThickness * 10);
            if (!MathTools.DoublesAreEqual(denom, 0))
            {
                Output.PestSoilConc = Output.PestInSoil / denom;
            }
            else
            {
                Output.PestSoilConc = 0;
                MathTools.LogDivideByZeroError("CalculateDegradingPestInSoil", "sim.in_BulkDensity_g_per_cm3[0]*in_MixLayerThickness_mm*10", "out_PestSoilConc_mg_per_kg");
            }


            double porosity = 1 - Sim.SoilController.DataModel.BulkDensity.Values[0] / 2.65;

            //calculate the denominator of the PestConcInSoilAfterLeaching Equation - need to test for denom=0
            denom = DataModel.MixLayerThickness * (DataModel.SorptionCoefficient * Sim.SoilController.DataModel.BulkDensity.Values[0] + porosity);

            double availwaterstorageinmixing;
            if (!MathTools.DoublesAreEqual(Sim.SoilController.Depth[1], 0))
            {
                availwaterstorageinmixing = (Sim.SoilController.DrainUpperLimitRelWP[0] - Sim.SoilController.SoilWaterRelWP[0]) * DataModel.MixLayerThickness / Sim.SoilController.Depth[1];
            }
            else
            {
                availwaterstorageinmixing = 0;
                MathTools.LogDivideByZeroError("CalculateDegradingPestInSoil", "sim.depth[1]", "availwaterstorageinmixing");
            }
            if (!MathTools.DoublesAreEqual(denom, 0))
            {
                double infiltration = Sim.ClimateController.Rain - Sim.SoilController.WatBal.Runoff - availwaterstorageinmixing;
                if (infiltration < 0)
                {
                    infiltration = 0;
                }
                ConcSoilAfterLeach = Output.PestSoilConc * Math.Exp(-infiltration / (denom));
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
            double sorpBYext = DataModel.SorptionCoefficient * DataModel.ExtractCoefficient;
            double denom1 = (1 + sorpBYext);

            if (Sim.SoilController.Runoff > 0 && Output.PestSoilConc > 0 && !MathTools.DoublesAreEqual(denom1, 0))
            {
                Output.PestWaterPhaseConc = ConcSoilAfterLeach * DataModel.ExtractCoefficient / denom1 * 1000.0;
                Output.PestSedPhaseConc = ConcSoilAfterLeach * sorpBYext / denom1;
                Output.PestRunoffConc = Output.PestWaterPhaseConc + Output.PestSedPhaseConc * Sim.SoilController.SedimentConc;
            }
            else
            {
                if (MathTools.DoublesAreEqual(1 + sorpBYext, 0))
                {
                    MathTools.LogDivideByZeroError("CalculatePesticideRunoffConcentrations", "1+sorpBYext", "3 x pest-concs");
                }
                Output.PestWaterPhaseConc = 0;
                Output.PestSedPhaseConc = 0;
                Output.PestRunoffConc = 0;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void CalculatePesticideLosses()
        {
            if (Sim.SoilController.Runoff > 0)
            {
                Output.PestLostInRunoffWater = Output.PestWaterPhaseConc * Sim.SoilController.Runoff * 0.01;
                Output.PestLostInRunoffSediment = Output.PestSedPhaseConc * Sim.SoilController.ErosionTPerHa * Sim.SoilController.DataModel.SedDelivRatio;// spreadsheet uses runoff instead of erosion*SelDelivRatio
                Output.TotalPestLostInRunoff = Output.PestLostInRunoffWater + Output.PestLostInRunoffSediment;

            }
            else
            {
                Output.PestLostInRunoffWater = 0;
                Output.PestLostInRunoffSediment = 0;
                Output.TotalPestLostInRunoff = 0;
            }
            Output.PestLostInLeaching = (Output.PestSoilConc - ConcSoilAfterLeach) * Sim.SoilController.DataModel.BulkDensity.Values[0] * DataModel.MixLayerThickness / 10.0;
            if (Output.PestLostInLeaching < 0)
            {
                Output.PestLostInLeaching = 0;
            }
            if (!MathTools.DoublesAreEqual(LastPestInput, 0))
            {
                Output.PestLossesPercentOfInput = (Output.TotalPestLostInRunoff + Output.PestLostInLeaching) / LastPestInput * 100.0;
            }
            else
            {
                Output.PestLossesPercentOfInput = 0;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void CalculatePesticideDaysAboveCritical()
        {
            if (Output.PestRunoffConc > DataModel.CritPestConc * 1)
            {
                Output.DaysGreaterCrit1++;
            }
            if (Output.PestRunoffConc > DataModel.CritPestConc * 0.5)
            {
                Output.DaysGreaterCrit2++;
            }
            if (Output.PestRunoffConc > DataModel.CritPestConc * 2)
            {
                Output.DaysGreaterCrit3++;
            }
            if (Output.PestRunoffConc > DataModel.CritPestConc * 10)
            {
                Output.DaysGreaterCrit4++;
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
