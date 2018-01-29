using HowLeaky.CustomAttributes;
using HowLeaky.Tools.Helpers;
using System;
using HowLeaky.DataModels;
using HowLeaky.OutputModels;
using System.Collections.Generic;
using HowLeaky.Interfaces;

namespace HowLeaky.ModelControllers
{

    //public class NitrateOutputDataModel : OutputDataModel, IDailyOutput
    //{
       
    //}
    public class NitrateController : HLController
    {
        public NitrateInputDataModel DataModel { get; set; }
        //public NitrateOutputDataModel Output { get; set; }

        public int Nitratesdayindex1 { get; set; }
        public int Nitratesdayindex2 { get; set; }
        public int Nitratesdayindex3 { get; set; }


        //Reportable Outputs
        [Output]
        [Unit("mg_per_L")]
        public double N03NDissolvedInRunoff { get; set; }        // Dissolved N03 N In Runoff (mg/L)
        [Output]
        [Unit("kg_per_ha")]
        public double N03NRunoffLoad { get; set; }              // N03 Runoff Load (kg/ha)
        [Output]
        [Unit("mg_per_L")]
        public double N03NDissolvedLeaching { get; set; }       // Dissolved N03 N In Leaching (mg/L)
        [Output]
        [Unit("kg_per_ha")]
        public double N03NLeachingLoad { get; set; }            // N03 N Leaching Load (kg/ha)
        [Output]
        [Unit("kg_per_ha")]
        public double ParticNInRunoff { get; set; }             // Particulate N in Runoff (kg/ha)
        [Output]
        [Unit("kg_per_ha")]
        public double N03NStoreTopLayer { get; set; }           // N03 N Store in top layer (kg/ha)
        [Output]
        [Unit("kg_per_ha")]
        public double N03NStoreBotLayer { get; set; }           // N03 N Store in bot layer (kg/ha)
        [Output]
        [Unit("kg_per_ha")]
        public double TotalNStoreTopLayer { get; set; }         // Total N Store in top layer (kg/ha)
        [Output]
        [Unit("kg_per_ha")]
        public double PNHLCa { get; set; }                      // PNHLC (kg/ha)
        [Output]
        [Unit("mm")]
        public double DrainageInN03Period { get; set; }         //
        [Output]
        [Unit("mm")]
        public double RunoffInN03Period { get; set; }           //

        /// <summary>
        /// 
        /// </summary>
        public NitrateController(Simulation sim) : base(sim)
        {
           // Output = new NitrateOutputDataModel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sim"></param>
        public NitrateController(Simulation sim, List<InputModel> inputModels) : this(sim)
        {
            DataModel = (NitrateInputDataModel)inputModels[0];
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Initialise()
        {
            //Do nothing
        }
        /// <summary>
        /// 
        /// </summary>
        public void InitialiseNitrateParameters()
        {
            //Nitratesdayindex1 = 0;
            //Nitratesdayindex2 = 0;
            //Nitratesdayindex3 = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Simulate()
        {
            try
            {
                bool CanSimulateNitrate = (DataModel.DissolvedNInRunoffInputOptions != 0 || DataModel.DissolvedNInLeachingInputOptions != 0 || DataModel.ParticulateNInRunoffInputOptions != 0);
                if (CanSimulateNitrate)
                {
                    if (CanCalculateDissolvedNInRunoff())
                    {
                        CalculateDissolvedNInRunoff();
                    }
                    if (CanCalculateDissolvedNInLeaching())
                    {
                        CalculateDissolvedNInLeaching();
                    }
                    if (CanCalculateParticulateNInRunoff())
                    {
                        CalculateParticulateNInRunoff();
                    }
                    UpdateNitrateSummaryValues();
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
        public void UpdateNitrateSummaryValues() { }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanCalculateDissolvedNInRunoff()
        {
            if (DataModel.DissolvedNInRunoffInputOptions == 1)
            {
                return DataModel.NLoadInSurfaceLayerTimeSeries.GetCount() != 0;
            }
            else if (DataModel.DissolvedNInRunoffInputOptions == 2)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanCalculateDissolvedNInLeaching()
        {
            if (DataModel.DissolvedNInLeachingInputOptions == 1)
            {
                return DataModel.NLoadInLowerLayersTimeSeries.GetCount() != 0;
            }
            else if (DataModel.DissolvedNInLeachingInputOptions == 2)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanCalculateParticulateNInRunoff()
        {
            if (DataModel.ParticulateNInRunoffInputOptions == 1)
            {
                return (DataModel.SoilInorganicNitrateNTimeSeries.GetCount() != 0 &&
                          DataModel.SoilInorganicAmmoniumNTimeSeries.GetCount() != 0 &&
                          DataModel.SoilInorganicAmmoniumNTimeSeries.GetCount() != 0);
            }
            else if (DataModel.ParticulateNInRunoffInputOptions == 2)
            {
                return true;
            }
            return false;
        }

        //From Howleaky developers (Brett Robinson), based on the concept that soil and runoff water mixing increases up to a maximum of k.
        // DN = Nsurface * K(1- Math.Exp(-cvQ)
        // where DN is the Nitrate conc in the runoff (mg/L)
        // k is the parameter that regulates mixing of soil and runoff water, (suggested 0.5)
        // cv is parameter that describes the curvature of change in the soil and water runoff at increasing runoff values (initial guess is 0.2)
        // Q is runoff (mm)
        // Nsurface (mg N/kg) is the soil nitrate concentrate in teh survace layer (0-2cm), which in our approach is derived from nitrate load (NLsoil in kg/ha) in surface layer from DairyMod
        // NSurface = alpha*100*NLsoil/(depth*soildensity)
        // Then dissolved N load (NL, kg/ha) in runoff is
        // DL=ND*Q/100.0;
        // NOTATION USE HERE IS TO BE CONSISTENT WITH THAT USED BY VIC DPI
        /// <summary>
        /// 
        /// </summary>
        public void CalculateDissolvedNInRunoff()
        {
            try
            {
                double NLKgHa = GetN03NStoreTopLayerkgPerha();  //Nitrate load in surface layer (From Dairymod)
                if (!MathTools.DoublesAreEqual(NLKgHa, MathTools.MISSING_DATA_VALUE))
                {
                    NLKgHa = NLKgHa * DataModel.SoilNLoadWeighting1;
                    double k = DataModel.Nk;                           // INPUT parameter that regulates mixing of soil and runoff water
                    double cv = DataModel.Ncv;                             //INPUT parameter that describes the curvature of change in soil and water runoff at increasing runoff values
                    double Q = Sim.SoilController.Runoff;    //runoff amount
                    double d = DataModel.NDepthTopLayer1;           //depth of surface soil layer mm
                    double phi = Sim.SoilController.DataModel.BulkDensity.Values[0]; //soil density t/m3       ( BulkDensity is in g/cm3)
                    double NSoil = DataModel.NAlpha * 100.0 * NLKgHa / (d * phi);  //mg/kg
                    double DN = NSoil * k * (1 - Math.Exp(-cv * Q));
                    double DL = DN * Q / 100.0;

                    N03NStoreTopLayer = NLKgHa;
                    N03NDissolvedInRunoff = DN;
                    N03NRunoffLoad = DL;
                }
                else
                {
                    N03NStoreTopLayer = MathTools.MISSING_DATA_VALUE;
                    N03NDissolvedInRunoff = MathTools.MISSING_DATA_VALUE;
                    N03NRunoffLoad = MathTools.MISSING_DATA_VALUE;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        // The nitrate concentrate in soil water contributing to leaching (mg/l) is
        // LN= NSoil/(totalsoilwater)
        // where NSoil is the nigrate concentration in the soil (kg/ha) and
        // totalsoilwater  is the soil water between air dry water content and saturated water content (mm) of the soil profile or the layer.
        // Nitrate concentration in soil can either be for the soil profile or for the deepest soil layer.
        // Concentration for the soil profile can be obtained from Math.Experiments or Math.Expert knowledge and soil nitrate concentration in the deepest soil layer can be informed by other nitrogen biophysical models (eg. DairyMod).
        // Nitrate leaching load LL (kg /ha) is then calcualted as
        // LL = LN*LE*D/100.0
        // Where LE is the leaching efficiency parameter portioning soil water nitrate concentration into various pathways (often taken as 0.5)
        // D is the daily drainage
        // NOTATION USE HERE IS TO BE CONSISTENT WITH THAT USED BY VIC DPI
        /// <summary>
        /// 
        /// </summary>
        public void CalculateDissolvedNInLeaching()
        {
            try
            {
                double NSoilKgPerHa = GetN03NStoreBotLayerkgPerha();       //nitrate concentrate in the soil (kg/ha)
                if (!MathTools.DoublesAreEqual(NSoilKgPerHa, MathTools.MISSING_DATA_VALUE))
                {
                    NSoilKgPerHa = NSoilKgPerHa * DataModel.SoilNiLoadWeighting2;
                    double deltadepth = DataModel.NDepthBottomLayer;
                    if (deltadepth > 0)
                    {
                        double soilwater = (Sim.SoilController.DataModel.Saturation.Values[Sim.SoilController.LayerCount - 1] - Sim.SoilController.DataModel.AirDry.Values[Sim.SoilController.LayerCount - 1]) / 100.0 * deltadepth;
                        double LE = DataModel.NLeachingEfficiency;                      //Leaching efficiency (INPUT)
                        double D = Sim.SoilController.DeepDrainage;                  //Drainage (mm)
                        double LN = NSoilKgPerHa * 1000000.0 / (soilwater * 10000.0);
                        double LL = (LN / 1000000.0) * D * 10000.0 * LE;

                        N03NStoreBotLayer = NSoilKgPerHa;
                        N03NDissolvedLeaching = LN;
                        N03NLeachingLoad = LL;
                    }
                    else
                    {
                        N03NStoreBotLayer = MathTools.MISSING_DATA_VALUE;
                        N03NDissolvedLeaching = MathTools.MISSING_DATA_VALUE;
                        N03NLeachingLoad = MathTools.MISSING_DATA_VALUE;
                    }
                }
                else
                {
                    N03NStoreBotLayer = MathTools.MISSING_DATA_VALUE;
                    N03NDissolvedLeaching = MathTools.MISSING_DATA_VALUE;
                    N03NLeachingLoad = MathTools.MISSING_DATA_VALUE;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        // Particulate N Losses in runoff are modelled in a similar way to particulate P
        // PN = beta*E*SDR*TNsoil*NER
        // where PN is the particulate N load (kg/ha)
        // TNsoil is the total N concentrate of the soil (mg/kg) and is the sum of the organica and inorganic N concentrations at 0-2cm from DairyMod.
        // As TNsoil will be derived from DairyMod in kg/ha, we need to convert this to mg.kg.
        // E is the gross errosion (kg/ha)
        // SDR is the sediment delivery ratio
        // NER is the Nitrogen enrichment ratio, which is unitless and defined similary to PER (for P)
        // Beta is a conversion factor to adjust units and that can be used as a calibration factor.
        // NOTATION USE HERE IS TO BE CONSISTENT WITH THAT USED BY VIC DPI
        /// <summary>
        /// 
        /// </summary>
        public void CalculateParticulateNInRunoff()
        {
            try
            {
                double TNSoilKgPerHa = GetTotalNStoreTopLayerkgPerha();     // Total N Concentration in soil (mg/kg) and is sum of organanic and inorgance conc at 0-2cm (Obtained from Dairymod)
                if (!MathTools.DoublesAreEqual(TNSoilKgPerHa, MathTools.MISSING_DATA_VALUE))
                {
                    TNSoilKgPerHa = TNSoilKgPerHa * DataModel.SoilNLoadWeighting3;
                    double E = Sim.SoilController.HillSlopeErosion * 1000.0;// Gross erosion (kg/ha)
                    double SDR = Sim.SoilController.DataModel.SedDelivRatio;                         // Sediment delivery ratio.
                    double NER = DataModel.NEnrichmentRatio;                           // Nitrogen enrighment ratio
                    double d = DataModel.NDepthTopLayer2;                       // depth of surface soil layer mm
                    double phi = Sim.SoilController.DataModel.BulkDensity.Values[0];          // soil density t/m3       ( BulkDensity is in g/cm3)
                    double NSoil = DataModel.NAlpha2 * 100.0 * TNSoilKgPerHa / (d * phi);    // mg/kg
                    double PN = DataModel.NBeta * E * SDR * NSoil * NER / 1000000.0;

                    ParticNInRunoff = PN;
                    if (!MathTools.DoublesAreEqual(SDR, 0) && !MathTools.DoublesAreEqual(Sim.SoilController.UsleLsFactor, 0))
                    {
                        PNHLCa = PN / (SDR * Sim.SoilController.UsleLsFactor);
                    }
                    else
                    {
                        PNHLCa = 0;
                    }
                    TotalNStoreTopLayer = TNSoilKgPerHa;
                }
                else
                {
                    ParticNInRunoff = MathTools.MISSING_DATA_VALUE;
                    PNHLCa = MathTools.MISSING_DATA_VALUE;
                    TotalNStoreTopLayer = MathTools.MISSING_DATA_VALUE;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // Called from  CalculateDissolvedNInRunoff
        // extracts value directly from input time-series;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetN03NStoreTopLayerkgPerha()
        {
            try
            {
                if (DataModel.DissolvedNInRunoffInputOptions == 1 && DataModel.NLoadInSurfaceLayerTimeSeries.GetCount() != 0)
                {
                    return DataModel.NLoadInSurfaceLayerTimeSeries.GetValueAtDate(Sim.Today);
                }
                else if (DataModel.DissolvedNInRunoffInputOptions == 2)
                {
                    //return DataModel.SoilNLoadData1.GetValueForDayIndex("SoilNLoadData1", Nitratesdayindex1, Sim.Today);
                    return DataModel.SoilNLoadData1.GetValueForDayIndex("SoilNLoadData1", Sim.Today);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return 0;
        }


        // Called from  CalculateDissolvedNInLeaching
        // extracts value directly from input time-series OR can interpolate from user-defined values;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetN03NStoreBotLayerkgPerha()
        {
            try
            {
                if (DataModel.DissolvedNInLeachingInputOptions == 1 && DataModel.NLoadInLowerLayersTimeSeries.GetCount() != 0)
                {
                    return DataModel.NLoadInLowerLayersTimeSeries.GetValueAtDate(Sim.Today);
                }
                else if (DataModel.DissolvedNInLeachingInputOptions == 2)
                {
                    //return DataModel.SoilNLoadData2.GetValueForDayIndex("SoilNLoadData2", Nitratesdayindex2, Sim.Today);
                    return DataModel.SoilNLoadData2.GetValueForDayIndex("SoilNLoadData2", Sim.Today);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return 0;
        }

        // Called from  CalculateParticulateNInRunoff
        // extracts value directly from input time-series;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetTotalNStoreTopLayerkgPerha()
        {
            try
            {
                if (DataModel.ParticulateNInRunoffInputOptions == 1)
                {
                    double value1 = 0;
                    double value2 = 0;
                    double value3 = 0;
                    if (DataModel.SoilInorganicNitrateNTimeSeries.GetCount() != 0)
                    {
                        value1 = DataModel.SoilInorganicNitrateNTimeSeries.GetValueAtDate(Sim.Today);
                    }
                    if (DataModel.SoilInorganicAmmoniumNTimeSeries.GetCount() != 0)
                    {
                        value2 = DataModel.SoilInorganicAmmoniumNTimeSeries.GetValueAtDate(Sim.Today);
                    }
                    if (DataModel.SoilOrganicNTimeSeries.GetCount() != 0)
                    {
                        value3 = DataModel.SoilOrganicNTimeSeries.GetValueAtDate(Sim.Today);
                    }
                    if (MathTools.DoublesAreEqual(value1, MathTools.MISSING_DATA_VALUE) || MathTools.DoublesAreEqual(value2, MathTools.MISSING_DATA_VALUE) || MathTools.DoublesAreEqual(value3, MathTools.MISSING_DATA_VALUE))
                    {
                        return MathTools.MISSING_DATA_VALUE;
                    }
                    return value1 + value1 + value3;
                }
                else if (DataModel.ParticulateNInRunoffInputOptions == 2)
                {
                    //return DataModel.SoilNLoadData3.GetValueForDayIndex("SoilNLoadData3", Nitratesdayindex3, Sim.Today);
                    return DataModel.SoilNLoadData3.GetValueForDayIndex("SoilNLoadData3", Sim.Today);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanSimulateNitrate()
        {
            return (DataModel.DissolvedNInRunoffInputOptions != 0 || DataModel.DissolvedNInLeachingInputOptions != 0 || DataModel.ParticulateNInRunoffInputOptions != 0);
        }
    }
}
