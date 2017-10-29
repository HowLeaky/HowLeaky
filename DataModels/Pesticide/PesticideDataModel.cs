using HowLeaky.CustomAttributes;
using HowLeaky.ModelControllers;
using HowLeaky.Models;
using HowLeaky.Tools;
using HowLeaky.Tools.DataObjects;
using HowLeaky.XmlObjects;
using HowLeakyWebsite.Tools.DHMCoreLib.Helpers;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace HowLeaky.DataModels
{
    public enum EPestApplicationTiming { patFixedDate, patFromSequenceFile, patGDDCrop1, patGDDCrop2, patGDDCrop3, patDASCrop1, patDASCrop2, patDASCrop3, patDaysSinceFallow };

    public enum EPestApplicationPosition { apApplyToVegetationLayer, apApplyToStubbleLayer, apApplyToSoilLayer };

    //Class for XML Deserialisation
    public class PestApplicationTiming : IndexData
    {
        public StateData tbPestVegIndex1 { get; set; }
        public StateData tbPestVegIndex2 { get; set; }
        public StateData tbPestVegIndex3 { get; set; }
        public StateData tbPestVegIndex4 { get; set; }
        public StateData tbPestVegIndex5 { get; set; }
        public StateData tbPestVegIndex6 { get; set; }
        public StateData tbPestVegIndex7 { get; set; }
        public StateData tbPestVegIndex8 { get; set; }
        public StateData tbPestVegIndex9 { get; set; }
        public StateData tbPestVegIndex10 { get; set; }
        //  public PesticideDatesAndRates
        public int TriggerGGDFirst { get; set; }
        public int TriggerGGDSubsequent { get; set; }
        public int TriggerDaysFirst { get; set; }
        public int TriggerDaysSubsequent { get; set; }
        public DayMonthData ApplicationDate { get; set; }
        public double ProductRate { get; set; }
        public double SubsequentProductRate { get; set; }

        public PestApplicationTiming() { }
    }

    [XmlRoot("PesticideType")]
    public class PesticideObjectDataModel : DataModel
    {

        //Input Parameters
        public PestApplicationTiming PestApplicationTiming { get; set; }
        public IndexData PestApplicationPosition { get; set; }        // Describes where the pesticide is applied relative to the crop. It is used to determine the fraction of the applied pesticide that is assumed to have been intercepted by the vegetation and/or stubble rather than entering the soil.        
        [Unit("days")]
        public double HalfLifeVeg { get; set; }                         // The time required (days) for a pesticide to under go dissipation or degradation to half of the initial concentration on the vegetation.       
        [Unit("oC")]
        public double RefTempHalfLifeVeg { get; set; }                  // The mean air temperature at which the Half-life (Veg) was determined (oC).      
        [Unit("days")]
        public double HalfLifeStubble { get; set; }                     // The time required (days) for a pesticide to under go dissipation or degradation to half of the initial concentration in the stubble.    
        [Unit("oC")]
        public double RefTempHalfLifeStubble { get; set; }              // The mean air temperature at which the Half-life (Stubble) was determined (oC).      
        [Unit("days")]
        public double HalfLife { get; set; }                            // The time required (days) for a pesticide to under go dissipation or degradation to half of the initial concentration in the soil.       
        [Unit("oC")]
        public double RefTempHalfLifeSoil { get; set; }                 // The mean air temperature at which the Half-life (Soil) was determined (oC).      
        [Unit("ug_per_L")]
        public double CritPestConc { get; set; }                        // Concentration of a pesticide that should not be exceeded in runoff (ug/L).      
        [Unit("g_per_L")]
        public double ConcActiveIngred { get; set; }                    // The concentration of the pesticide active ingredient (e.g. glyphosate) in the applied product (e.g. Roundup) (g/L). This value is multiplied by the application rate (L/ha) to calculate the amount of active ingredient applied (kg/ha).     
        [Unit("pc")]
        public double PestEfficiency { get; set; }                      // The percent of total applied pesticide (concentration of active ingredient x application rate) that is retained in the paddock (on the vegetation, stubble or soil) immediately following application. This percent may be less than 100 if there is significant spray drift or other losses between the point of application and the vegetation, stubble and soil.      
        [Unit("J_per_mol")]
        public double DegradationActivationEnergy { get; set; }         // The energetic threshold for thermal decomposition reactions (J/mol).   
        [Unit("mm")]
        public int MixLayerThickness { get; set; }                      // Depth of the surface soil layer into which an applied pesticide is mixed (mm). This depth is used to calculate a pesticide concentration in the soil following application.        
        public double SorptionCoefficient { get; set; }                 // The sorption coefficient is the ratio of the amount of pesticide bound to soil/sediment versus the amount in the water phase (Kd). Kd values can be derived empirically or estimated from published organic carbon sorption coefficients (Koc) where Kd=Koc x fraction of organic carbon.      
        public double ExtractCoefficient { get; set; }                  // The fraction of pesticide present in soil that will be extracted into runoff. This includes pesticides present in runoff in both the sorbed and dissolved phase. The value of 0.02 has been derived empirically (Silburn, DM, 2003. Characterising pesticide runoff from soil on cotton farms using a rainfall simulator. PhD Thesis, University of Sydney.) and was found to be relevant to data from a range of published studies.        
        public double CoverWashoffFraction { get; set; }                // The fraction of a pesticide that will move off the surface of the vegetation or stubble and into the soil following a rainfall event of greater than 5mm.        
        [Unit("pc")]
        public double BandSpraying { get; set; }                        // The percent area of a paddock to which a pesticide is applied.        

        public Sequence PestApplicationDateList { get; set; }
        public List<double> PestApplicationValueList { get; set; } = new List<double>();

        [XmlIgnore]
        public int ApplicationTiming { get { return PestApplicationTiming.index; } }
        [XmlIgnore]
        public DayMonthData ApplicationDate { get { return PestApplicationTiming.ApplicationDate; } }
        [Unit("L_per_ha")]
        [XmlIgnore]
        public double ProductRate { get { return PestApplicationTiming.ProductRate; } }
        [Unit("L_per_ha")]
        [XmlIgnore]
        public double SubsequentProductRate { get { return PestApplicationTiming.SubsequentProductRate; } }
        //[XmlIgnore]
        //public double in_PestApplicationSequence { get; set; }          //        
        [XmlIgnore]
        public int TriggerGGDFirst { get { return PestApplicationTiming.TriggerGGDFirst; } }
        [XmlIgnore]
        public int TriggerGGDSubsequent { get { return PestApplicationTiming.TriggerGGDSubsequent; } }
        [XmlIgnore]
        public int TriggerDaysFirst { get { return PestApplicationTiming.TriggerDaysFirst; } }
        [XmlIgnore]
        public int TriggerDaysSubsequent { get { return PestApplicationTiming.TriggerDaysSubsequent; } }
        [XmlIgnore]
        public int ApplicationPosition { get { return PestApplicationPosition.index; } }

    }

    public class PesticideObjectController : HLObject
    {

        public PesticideObjectDataModel dataModel { get; set; }
        //**************************************************************************
        //Outputs
        //**************************************************************************
        [Output]
        [XmlIgnore]
        [Unit("g_per_ha")]
        public OutputParameter out_AppliedPestOnVeg_g_per_ha { get; set; }              // Applied pest on veg (g/ha)
        [Output]
        [XmlIgnore]
        [Unit("g_per_ha")]
        public OutputParameter out_AppliedPestOnStubble_g_per_ha { get; set; }          // Applied pest on stubble (g/ha
        [Output]
        [XmlIgnore]
        [Unit("g_per_ha")]
        public OutputParameter out_AppliedPestOnSoil_g_per_ha { get; set; }             // Applied pest on soil (g/ha)
        [Output]
        [XmlIgnore]
        [Unit("g_per_ha")]
        public OutputParameter out_PestOnVeg_g_per_ha { get; set; }                     // Pest on veg (g/ha)
        [Output]
        [XmlIgnore]
        [Unit("g_per_ha")]
        public OutputParameter out_PestOnStubble_g_per_ha { get; set; }                 // Pest on stubble (g/ha)
        [Output]
        [XmlIgnore]
        [Unit("g_per_ha")]
        public OutputParameter out_PestInSoil_g_per_ha { get; set; }                    // Pest in soil (g/ha)
        [Output]
        [XmlIgnore]
        [Unit("mg_per_kg")]
        public OutputParameter out_PestSoilConc_mg_per_kg { get; set; }                 // Pest soil conc. (mg/kg)
        [Output]
        [XmlIgnore]
        [Unit("mg_per_kg")]
        public OutputParameter out_PestSedPhaseConc_mg_per_kg { get; set; }             // Pest sediment phase conc. (mg/kg)
        [Output]
        [XmlIgnore]
        [Unit("ug_per_L")]
        public OutputParameter out_PestWaterPhaseConc_ug_per_L { get; set; }            // Pest water phase conc. (ug/L)
        [Output]
        [XmlIgnore]
        [Unit("ug_per_L")]
        public OutputParameter out_PestRunoffConc_ug_per_L { get; set; }                // Pest runoff conc. (water+sediment) (ug/L)
        [Output]
        [XmlIgnore]
        [Unit("g_per_L")]
        public OutputParameter out_SedimentDelivered_g_per_L { get; set; }              // Sediment delivered (g/L)
        [Output]
        [XmlIgnore]
        [Unit("g_per_ha")]
        public OutputParameter out_PestLostInRunoffWater_g_per_ha { get; set; }         // Pest lost in runoff water (g/ha)
        [Output]
        [XmlIgnore]
        [Unit("g_per_ha")]
        public OutputParameter out_PestLostInRunoffSediment_g_per_ha { get; set; }      // Pest lost in runoff sediment (g/ha)
        [Output]
        [XmlIgnore]
        [Unit("g_per_ha")]
        public OutputParameter out_TotalPestLostInRunoff_g_per_ha { get; set; }         // Total pest lost in runoff (g/ha)
        [Output]
        [XmlIgnore]
        [Unit("g_per_ha")]
        public OutputParameter out_PestLostInLeaching_g_per_ha { get; set; }            // Pest lost in leaching (g/ha)
        [Output]
        [XmlIgnore]
        [Unit("pc")]
        public OutputParameter out_PestLossesPercentOfInput_pc { get; set; }            // Pest losses as percent of last input (%)
        [Output]
        [XmlIgnore]
        public OutputParameter out_ApplicationCount { get; set; }                       // Number of applications
        [Output]
        [XmlIgnore]
        [Unit("g_per_ha")]
        public OutputParameter out_ProductApplication_g_per_ha { get; set; }            // Avg Product Application (g/ha/year)
        [Output]
        [XmlIgnore]
        [Unit("ug_per_L")]
        public OutputParameter out_AvgBoundPestConcInRunoff_ug_per_L { get; set; }      // Avg Bound Pest Conc in Runoff (ug/l)
        [Output]
        [XmlIgnore]
        [Unit("ug_per_L")]
        public OutputParameter out_AvgUnboundPestConcInRunoff_ug_per_L { get; set; }    // Avg Unbound Pest Conc in Runoff (ug/l)
        [Output]
        [XmlIgnore]
        [Unit("ug_per_L")]
        public OutputParameter out_AvgCombinedPestConcInRunoff_ug_per_L { get; set; }   // Avg Combined Pest Conc in Runoff (ug/l)
        [Output]
        [XmlIgnore]
        [Unit("g_per_ha")]
        public OutputParameter out_AvgPestLoadWater_g_per_ha { get; set; }              // Avg Pest Load (Water) (g/ha/yr)
        [Output]
        [XmlIgnore]
        [Unit("g_per_ha")]
        public OutputParameter out_AvgPestLoadSediment_g_per_ha { get; set; }           // Avg Pest Load (Sed) (g/ha/yr)
        [Output]
        [XmlIgnore]
        [Unit("g_per_ha")]
        public OutputParameter out_AvgTotalPestLoad_g_per_ha { get; set; }              // Avg Pest Load (Total) (g/ha/yr)
        [Output]
        [XmlIgnore]
        [Unit("pc")]
        public OutputParameter out_ApplicationLossRatio_pc { get; set; }                // Loss/Application Ratio (%)
        [Output]
        [XmlIgnore]
        [Unit("days")]
        public OutputParameter out_DaysGreaterCrit1_days { get; set; }                  // Avg Days Conc > Crit (days/yr)
        [Output]
        [XmlIgnore]
        [Unit("days")]
        public OutputParameter out_DaysGreaterCrit2_days { get; set; }                  // Avg Days Conc > 0.5 Crit (days/yr)
        [Output]
        [XmlIgnore]
        [Unit("days")]
        public OutputParameter out_DaysGreaterCrit3_days { get; set; }                  // Avg Days Conc > 2 Crit (days/yr)
        [Output]
        [XmlIgnore]
        [Unit("days")]
        public OutputParameter out_DaysGreaterCrit4_days { get; set; }                  // Avg Days Conc > 10 Crit (days/yr)
        [Output]
        [XmlIgnore]
        [Unit("ug_per_L")]
        public OutputParameter out_PestEMC_ug_per_L { get; set; }                       // Pesticide EMC (ug/L)

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
                return sim.PesticideController.GetPesticideIndex(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public PesticideObjectController() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sim"></param>
        public PesticideObjectController(Simulation sim) : base(sim)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public override void Simulate()
        {
            if (PestApplicCount > 0)
                ++DaysSinceApplication;
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
            if (dataModel.ApplicationTiming == (int)EPestApplicationTiming.patFixedDate)
            {
                if (sim.day == dataModel.ApplicationDate.Day && sim.month == dataModel.ApplicationDate.Month)
                    ProductRateApplied = dataModel.ProductRate;
            }
            else if (dataModel.ApplicationTiming == (int)EPestApplicationTiming.patFromSequenceFile)
            {
                int index = DateUtilities.isDateInSequenceList(sim.today, dataModel.PestApplicationDateList);
                if (index >= 0 && index < dataModel.PestApplicationValueList.Count)
                    ProductRateApplied = dataModel.PestApplicationValueList[index];
            }
            else
            {
                LAIVegObjectController crop = (LAIVegObjectController)sim.VegetationController.CurrentCrop;
                if (crop != null)
                {
                    if (crop.CropStatus != CropStatus.csInFallow)
                    {
                        if (MathTools.DoublesAreEqual(crop.heat_unit_index, 0))
                        {
                            ApplicationIndex = 0;
                        }
                        if (dataModel.ApplicationTiming == (int)EPestApplicationTiming.patGDDCrop1 && crop == sim.VegetationController.GetCrop(0))
                        {
                            CheckApplicationBasedOnGDD(crop);
                        }
                        else if (dataModel.ApplicationTiming == (int)EPestApplicationTiming.patGDDCrop2 && crop == sim.VegetationController.GetCrop(1))
                        {
                            CheckApplicationBasedOnGDD(crop);
                        }
                        else if (dataModel.ApplicationTiming == (int)EPestApplicationTiming.patGDDCrop3 && crop == sim.VegetationController.GetCrop(2))
                        {
                            CheckApplicationBasedOnGDD(crop);
                        }
                        else if (dataModel.ApplicationTiming == (int)EPestApplicationTiming.patDASCrop1 && crop == sim.VegetationController.GetCrop(0))
                        {
                            CheckApplicationBasedOnDAS(crop);
                        }
                        else if (dataModel.ApplicationTiming == (int)EPestApplicationTiming.patDASCrop2 && crop == sim.VegetationController.GetCrop(1))
                        {
                            CheckApplicationBasedOnDAS(crop);
                        }
                        else if (dataModel.ApplicationTiming == (int)EPestApplicationTiming.patDASCrop3 && crop == sim.VegetationController.GetCrop(2))
                        {
                            CheckApplicationBasedOnDAS(crop);
                        }
                    }
                    else if (crop.CropStatus == CropStatus.csInFallow && dataModel.ApplicationTiming == (int)EPestApplicationTiming.patDaysSinceFallow)
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
            if (ApplicationIndex == 0 && crop.heat_units >= dataModel.TriggerGGDFirst)
            {
                ProductRateApplied = dataModel.ProductRate;
                ++ApplicationIndex;
            }
            else if (ApplicationIndex > 0 && crop.heat_units >= dataModel.TriggerGGDFirst + dataModel.TriggerGGDSubsequent * ApplicationIndex)
            {
                ProductRateApplied = dataModel.SubsequentProductRate;
                ++ApplicationIndex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="crop"></param>
        public void CheckApplicationBasedOnDAS(VegObjectController crop)
        {
            if (ApplicationIndex == 0 && crop.days_since_planting >= dataModel.TriggerDaysFirst)
            {
                ProductRateApplied = dataModel.ProductRate;
                ++ApplicationIndex;
            }
            else if (ApplicationIndex > 0 && crop.days_since_planting >= dataModel.TriggerDaysFirst + dataModel.TriggerDaysSubsequent * ApplicationIndex)
            {
                ProductRateApplied = dataModel.SubsequentProductRate;
                ++ApplicationIndex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void CheckApplicationBasedOnDAH()
        {
            int days_since_harvest = sim.VegetationController.GetDaysSinceHarvest();
            if (days_since_harvest == 0) ApplicationIndex = 0;
            if (ApplicationIndex == 0 && days_since_harvest >= dataModel.TriggerDaysFirst)
            {
                ProductRateApplied = dataModel.ProductRate;
                ++ApplicationIndex;
            }
            else if (ApplicationIndex > 0 && days_since_harvest >= dataModel.TriggerDaysFirst + dataModel.TriggerDaysSubsequent * ApplicationIndex)
            {
                ProductRateApplied = dataModel.SubsequentProductRate;
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

            sim.UpdateManagementEventHistory(ManagementEvent.mePesticide, PesticideIndex);
            DaysSinceApplication = 0;
            EPestApplicationPosition pos = (EPestApplicationPosition)dataModel.ApplicationPosition;
            double pest_application = dataModel.ConcActiveIngred * ProductRateApplied * dataModel.PestEfficiency / 100.0 * dataModel.BandSpraying / 100.0;
            LastPestInput = pest_application;

            if (pos == EPestApplicationPosition.apApplyToVegetationLayer)
            {
                out_AppliedPestOnVeg_g_per_ha.SetValue(pest_application * sim.crop_cover);
            }
            else
            {
                out_AppliedPestOnVeg_g_per_ha.SetZero();
            }
            if (pos == EPestApplicationPosition.apApplyToVegetationLayer || pos == EPestApplicationPosition.apApplyToStubbleLayer)
            {
                double stubble_cover = (1 - sim.crop_cover) * sim.total_residue_cover;
                out_AppliedPestOnStubble_g_per_ha.SetValue(pest_application * stubble_cover);
            }
            else
            {
                out_AppliedPestOnStubble_g_per_ha.SetZero();
            }


            out_AppliedPestOnSoil_g_per_ha.SetValue(pest_application - out_AppliedPestOnVeg_g_per_ha.Value() - out_AppliedPestOnStubble_g_per_ha.Value());
            ++PestApplicCount;
        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateDegradingPestOnVeg()
        {
            double halflifeveg_adjusted = 0;
            double universalgasconstant = 8.314472;
            double ref_airtempveg_kelvin = dataModel.RefTempHalfLifeVeg + 273.15;
            double airtemp_kelvin = ((sim.out_MaxTemp_oC + sim.out_MinTemp_oC) / 2.0) + 273.15;
            if (!MathTools.DoublesAreEqual(airtemp_kelvin, 0) && !MathTools.DoublesAreEqual(ref_airtempveg_kelvin, 0))
                halflifeveg_adjusted = dataModel.HalfLifeVeg * Math.Exp((dataModel.DegradationActivationEnergy / universalgasconstant) * (1.0 / airtemp_kelvin - 1.0 / ref_airtempveg_kelvin));
            else
            {
                MathTools.LogDivideByZeroError("CalculateDegradingPestOnVeg", "AirTemperature_kelvin!=0 or Ref_AirTemperatureVeg_kelvin", "HalfLifeVeg_adjusted");
            }
            double vegdegrate;
            if (!MathTools.DoublesAreEqual(halflifeveg_adjusted, 0))
            {
                vegdegrate = Math.Exp(-0.693 / halflifeveg_adjusted);
            }
            else
            {
                vegdegrate = 0;
                MathTools.LogDivideByZeroError("CalculateDegradingPestOnVeg", "HalfLifeVeg_adjusted", "vegdegrate");
            }
            if (sim.yesterdays_rain < 5.0)
            {
                out_PestOnVeg_g_per_ha.SetValue(out_PestOnVeg_g_per_ha.Value() * vegdegrate + out_AppliedPestOnVeg_g_per_ha.Value());
                if (sim.out_Rain_mm >= 5) //rain over 5mm will wash part of pest of veg
                {
                    out_PestOnVeg_g_per_ha.SetValue(out_PestOnVeg_g_per_ha.Value() * (1 - dataModel.CoverWashoffFraction));
                }
            }
            else
            {
                out_PestOnVeg_g_per_ha.SetValue(0);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateDegradingPestOnStubble()
        {
            double halflifestubble_adjusted = 0;
            double universalgasconstant = 8.314472;
            double ref_airtempstubble_kelvin = dataModel.RefTempHalfLifeStubble + 273.15;
            double airtemp_kelvin = ((sim.out_MaxTemp_oC + sim.out_MinTemp_oC) / 2.0) + 273.15;
            if (!MathTools.DoublesAreEqual(airtemp_kelvin, 0) && !MathTools.DoublesAreEqual(ref_airtempstubble_kelvin, 0))
            {
                halflifestubble_adjusted = dataModel.HalfLifeStubble * Math.Exp((dataModel.DegradationActivationEnergy / universalgasconstant) * (1.0 / airtemp_kelvin - 1.0 / ref_airtempstubble_kelvin));
            }
            else
            {
                MathTools.LogDivideByZeroError("CalculateDegradingPestOnStubble", "AirTemperature_kelvin or Ref_AirTemperatureStubble_kelvin", "HalfLifeStubble_adjusted");
            }

            double stubdegrate;
            if (!MathTools.DoublesAreEqual(halflifestubble_adjusted, 0))
            {
                stubdegrate = Math.Exp(-0.693 / halflifestubble_adjusted);
            }
            else
            {
                stubdegrate = 0;
                MathTools.LogDivideByZeroError("CalculateDegradingPestOnStubble", "HalfLifeStubble_adjusted", "stubdegrate");
            }
            if (sim.yesterdays_rain < 5.0)
            {
                out_PestOnStubble_g_per_ha.SetValue(out_PestOnStubble_g_per_ha.Value() * stubdegrate + out_AppliedPestOnStubble_g_per_ha.Value());
                if (sim.out_Rain_mm >= 5) //rain over 5mm will wash part of pest of stubble
                {
                    out_PestOnStubble_g_per_ha.SetValue(out_PestOnStubble_g_per_ha.Value() * (1 - dataModel.CoverWashoffFraction));
                }
            }
            else
            {
                out_PestOnStubble_g_per_ha.SetZero();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateDegradingPestInSoil()
        {
            double halflifesoil_adjusted = 0;
            double universalgasconstant = 8.314472;
            double ref_airtempsoil_kelvin = dataModel.RefTempHalfLifeSoil + 273.15;
            double airtemp_kelvin = ((sim.out_MaxTemp_oC + sim.out_MinTemp_oC) / 2.0) + 273.15;
            if (!MathTools.DoublesAreEqual(airtemp_kelvin, 0) && !MathTools.DoublesAreEqual(ref_airtempsoil_kelvin, 0))
            {
                halflifesoil_adjusted = dataModel.HalfLife * Math.Exp((dataModel.DegradationActivationEnergy / universalgasconstant) * (1.0 / airtemp_kelvin - 1.0 / ref_airtempsoil_kelvin));
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

            out_PestInSoil_g_per_ha.SetValue(out_PestInSoil_g_per_ha.Value() * soildegrate + out_AppliedPestOnSoil_g_per_ha.Value() - out_PestLostInLeaching_g_per_ha.Value() - out_TotalPestLostInRunoff_g_per_ha.Value());
            if (sim.out_Rain_mm >= 5.0)
            {
                out_PestInSoil_g_per_ha.SetValue(out_PestInSoil_g_per_ha.Value() + (out_PestOnStubble_g_per_ha.Value() + out_PestOnVeg_g_per_ha.Value()) * dataModel.CoverWashoffFraction);
            }


            denom = (sim.in_BulkDensity_g_per_cm3[0] * dataModel.MixLayerThickness * 10);
            if (!MathTools.DoublesAreEqual(denom, 0))
            {
                out_PestSoilConc_mg_per_kg.SetValue(out_PestInSoil_g_per_ha.Value() / denom);
            }
            else
            {
                out_PestSoilConc_mg_per_kg.SetZero();
                MathTools.LogDivideByZeroError("CalculateDegradingPestInSoil", "sim.in_BulkDensity_g_per_cm3[0]*in_MixLayerThickness_mm*10", "out_PestSoilConc_mg_per_kg");
            }


            double porosity = 1 - sim.in_BulkDensity_g_per_cm3[0] / 2.65;

            //calculate the denominator of the PestConcInSoilAfterLeaching Equation - need to test for denom=0
            denom = dataModel.MixLayerThickness * (dataModel.SorptionCoefficient * sim.in_BulkDensity_g_per_cm3[0] + porosity);

            double availwaterstorageinmixing;
            if (!MathTools.DoublesAreEqual(sim.depth[1], 0))
            {
                availwaterstorageinmixing = (sim.DrainUpperLimit_rel_wp[0] - sim.SoilWater_rel_wp[0]) * dataModel.MixLayerThickness / sim.depth[1];
            }
            else
            {
                availwaterstorageinmixing = 0;
                MathTools.LogDivideByZeroError("CalculateDegradingPestInSoil", "sim.depth[1]", "availwaterstorageinmixing");
            }
            if (!MathTools.DoublesAreEqual(denom, 0))
            {
                double infiltration = sim.out_Rain_mm - sim.out_WatBal_Runoff_mm - availwaterstorageinmixing;
                if (infiltration < 0)
                {
                    infiltration = 0;
                }
                ConcSoilAfterLeach = out_PestSoilConc_mg_per_kg.Value() * Math.Exp(-infiltration / (denom));
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
            double sorpBYext = dataModel.SorptionCoefficient * dataModel.ExtractCoefficient;
            double denom1 = (1 + sorpBYext);

            if (sim.runoff > 0 && out_PestSoilConc_mg_per_kg.Value() > 0 && !MathTools.DoublesAreEqual(denom1, 0))
            {
                out_PestWaterPhaseConc_ug_per_L.SetValue(ConcSoilAfterLeach * dataModel.ExtractCoefficient / denom1 * 1000.0);
                out_PestSedPhaseConc_mg_per_kg.SetValue(ConcSoilAfterLeach * sorpBYext / denom1);
                out_PestRunoffConc_ug_per_L.SetValue(out_PestWaterPhaseConc_ug_per_L.Value() + out_PestSedPhaseConc_mg_per_kg.Value() * sim.sediment_conc);
            }
            else
            {
                if (MathTools.DoublesAreEqual(1 + sorpBYext, 0))
                {
                    MathTools.LogDivideByZeroError("CalculatePesticideRunoffConcentrations", "1+sorpBYext", "3 x pest-concs");
                }
                out_PestWaterPhaseConc_ug_per_L.SetZero();
                out_PestSedPhaseConc_mg_per_kg.SetZero();
                out_PestRunoffConc_ug_per_L.SetZero();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculatePesticideLosses()
        {
            if (sim.runoff > 0)
            {
                out_PestLostInRunoffWater_g_per_ha.SetValue(out_PestWaterPhaseConc_ug_per_L.Value() * sim.runoff * 0.01);
                out_PestLostInRunoffSediment_g_per_ha.SetValue(out_PestSedPhaseConc_mg_per_kg.Value() * sim.erosion_t_per_ha * sim.in_SedDelivRatio);// spreadsheet uses runoff instead of erosion*SelDelivRatio
                out_TotalPestLostInRunoff_g_per_ha.SetValue(out_PestLostInRunoffWater_g_per_ha.Value() + out_PestLostInRunoffSediment_g_per_ha.Value());

            }
            else
            {
                out_PestLostInRunoffWater_g_per_ha.SetZero();
                out_PestLostInRunoffSediment_g_per_ha.SetZero();
                out_TotalPestLostInRunoff_g_per_ha.SetZero();
            }
            out_PestLostInLeaching_g_per_ha.SetValue((out_PestSoilConc_mg_per_kg.Value() - ConcSoilAfterLeach) * sim.in_BulkDensity_g_per_cm3[0] * dataModel.MixLayerThickness / 10.0);
            if (out_PestLostInLeaching_g_per_ha.Value() < 0)
            {
                out_PestLostInLeaching_g_per_ha.SetZero();
            }
            if (!MathTools.DoublesAreEqual(LastPestInput, 0))
            {
                out_PestLossesPercentOfInput_pc.SetValue((out_TotalPestLostInRunoff_g_per_ha.Value() + out_PestLostInLeaching_g_per_ha.Value()) / LastPestInput * 100.0);
            }
            else
            {
                out_PestLossesPercentOfInput_pc.SetZero();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculatePesticideDaysAboveCritical()
        {
            if (out_PestRunoffConc_ug_per_L.Value() > dataModel.CritPestConc * 1)
            {
                out_DaysGreaterCrit1_days.Increment();
            }
            if (out_PestRunoffConc_ug_per_L.Value() > dataModel.CritPestConc * 0.5)
            {
                out_DaysGreaterCrit2_days.Increment();
            }
            if (out_PestRunoffConc_ug_per_L.Value() > dataModel.CritPestConc * 2)
            {
                out_DaysGreaterCrit3_days.Increment();
            }
            if (out_PestRunoffConc_ug_per_L.Value() > dataModel.CritPestConc * 10)
            {
                out_DaysGreaterCrit4_days.Increment();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetPesticideIndex()
        {
            return sim.PesticideController.GetPesticideIndex(this);
        }

    }
}
