using HowLeaky.CustomAttributes;
using HowLeaky.ModelControllers;
using HowLeaky.Tools;
using HowLeaky.Tools.DataObjects;
using HowLeaky.Models;
using System;
using HowLeaky.DataModels;

namespace HowLeaky.ModelControllers
{
    public class NitrateController : HLObject
    {
        public NitrateDataModel data;

        //**************************************************************************
        //Outputs
        //**************************************************************************
        public OutputParameter out_N03NDissolvedInRunoff_mg_per_L { get; set; }        // Dissolved N03 N In Runoff (mg/L)
        public OutputParameter out_N03NRunoffLoad_kg_per_ha { get; set; }              // N03 Runoff Load (kg/ha)
        public OutputParameter out_N03NDissolvedLeaching_mg_per_L { get; set; }        // Dissolved N03 N In Leaching (mg/L)
        public OutputParameter out_N03NLeachingLoad_kg_per_ha { get; set; }            // N03 N Leaching Load (kg/ha)
        public OutputParameter out_ParticNInRunoff_kg_per_ha { get; set; }             // Particulate N in Runoff (kg/ha)
        public OutputParameter out_N03NStoreTopLayer_kg_per_ha { get; set; }           // N03 N Store in top layer (kg/ha)
        public OutputParameter out_N03NStoreBotLayer_kg_per_ha { get; set; }           // N03 N Store in bot layer (kg/ha)
        public OutputParameter out_TotalNStoreTopLayer_kg_per_ha { get; set; }         // Total N Store in top layer (kg/ha)
        public OutputParameter out_PNHLC_kg_per_ha { get; set; }                       // PNHLC (kg/ha)
        public OutputParameter out_DrainageInN03Period_mm { get; set; }                //
        public OutputParameter out_RunoffInN03Period_mm { get; set; }                  //

        //**************************************************************************
        //Internals
        //**************************************************************************
        public int nitratesdayindex1 { get; set; }
        public int nitratesdayindex2 { get; set; }
        public int nitratesdayindex3 { get; set; }
        //**************************************************************************
        /// <summary>
        /// 
        /// </summary>
        public NitrateController() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sim"></param>
        public NitrateController(Simulation sim) : base(sim) { }
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
        public void InitialiseNitrateParameters()
        {
            nitratesdayindex1 = 0;
            nitratesdayindex2 = 0;
            nitratesdayindex3 = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Simulate()
        {
            try
            {
                bool _CanSimulateNitrate = (data.DissolvedNInRunoffInputOptions != 0 || data.DissolvedNInLeachingInputOptions != 0 || data.ParticulateNInRunoffInputOptions != 0);
                if (_CanSimulateNitrate)
                {
                    if (CanCalculateDissolvedNInRunoff()) CalculateDissolvedNInRunoff();
                    if (CanCalculateDissolvedNInLeaching()) CalculateDissolvedNInLeaching();
                    if (CanCalculateParticulateNInRunoff()) CalculateParticulateNInRunoff();

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
            if (data.DissolvedNInRunoffInputOptions == 1)
                return data.NLoadInSurfaceLayerTimeSeries.GetCount() != 0;
            else if (data.DissolvedNInRunoffInputOptions == 2)
                return true;
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanCalculateDissolvedNInLeaching()
        {
            if (data.DissolvedNInLeachingInputOptions == 1)
                return data.NLoadInLowerLayersTimeSeries.GetCount() != 0;
            else if (data.DissolvedNInLeachingInputOptions == 2)
                return true;
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanCalculateParticulateNInRunoff()
        {
            if (data.ParticulateNInRunoffInputOptions == 1)
                return data.SoilInorganicNitrateNTimeSeries.GetCount() != 0 &&
                        data.SoilInorganicAmmoniumNTimeSeries.GetCount() != 0 &&
                        data.SoilInorganicAmmoniumNTimeSeries.GetCount() != 0;
            else if (data.ParticulateNInRunoffInputOptions == 2)
                return true;
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
                double NL_kg_ha = GetN03_N_Store_TopLayer_kg_per_ha();  //Nitrate load in surface layer (From Dairymod)
                if (!MathTools.DoublesAreEqual(NL_kg_ha, MathTools.MISSING_DATA_VALUE))
                {
                    NL_kg_ha = NL_kg_ha * data.SoilNLoadWeighting1;
                    double k = data.Nk;                           // INPUT parameter that regulates mixing of soil and runoff water
                    double cv = data.Ncv;                             //INPUT parameter that describes the curvature of change in soil and water runoff at increasing runoff values
                    double Q = sim.out_WatBal_Runoff_mm;    //runoff amount
                    double d = data.NDepthTopLayer1_mm;           //depth of surface soil layer mm
                    double phi = sim.in_BulkDensity_g_per_cm3[0]; //soil density t/m3       ( BulkDensity is in g/cm3)
                    double NSoil = data.NAlpha * 100.0 * NL_kg_ha / (d * phi);  //mg/kg
                    double DN = NSoil * k * (1 - Math.Exp(-cv * Q));
                    double DL = DN * Q / 100.0;

                    out_N03NStoreTopLayer_kg_per_ha.SetValue(NL_kg_ha);
                    out_N03NDissolvedInRunoff_mg_per_L.SetValue(DN);
                    out_N03NRunoffLoad_kg_per_ha.SetValue(DL);
                }
                else
                {
                    out_N03NStoreTopLayer_kg_per_ha.SetMissing();
                    out_N03NDissolvedInRunoff_mg_per_L.SetMissing();
                    out_N03NRunoffLoad_kg_per_ha.SetMissing();
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
                double Nsoil_kg_per_ha = GetN03_N_Store_BotLayer_kg_per_ha();       //nitrate concentrate in the soil (kg/ha)
                if (!MathTools.DoublesAreEqual(Nsoil_kg_per_ha, MathTools.MISSING_DATA_VALUE))
                {
                    Nsoil_kg_per_ha = Nsoil_kg_per_ha * data.SoilNiLoadWeighting2;
                    double deltadepth = data.NDepthBottomLayer_mm;
                    if (deltadepth > 0)
                    {
                        double soilwater = (sim.in_SoilLimitSaturation_pc[sim.in_LayerCount - 1] - sim.in_SoilLimitAirDry_pc[sim.in_LayerCount - 1]) / 100.0 * deltadepth;
                        double LE = data.NLeachingEfficiency_pc;                      //Leaching efficiency (INPUT)
                        double D = sim.out_WatBal_DeepDrainage_mm;                  //Drainage (mm)
                        double LN = Nsoil_kg_per_ha * 1000000.0 / (soilwater * 10000.0);
                        double LL = (LN / 1000000.0) * D * 10000.0 * LE;

                        out_N03NStoreBotLayer_kg_per_ha.SetValue(Nsoil_kg_per_ha);
                        out_N03NDissolvedLeaching_mg_per_L.SetValue(LN);
                        out_N03NLeachingLoad_kg_per_ha.SetValue(LL);
                    }
                    else
                    {
                        out_N03NStoreBotLayer_kg_per_ha.SetMissing();
                        out_N03NDissolvedLeaching_mg_per_L.SetMissing();
                        out_N03NLeachingLoad_kg_per_ha.SetMissing();
                    }
                }
                else
                {
                    out_N03NStoreBotLayer_kg_per_ha.SetMissing();
                    out_N03NDissolvedLeaching_mg_per_L.SetMissing();
                    out_N03NLeachingLoad_kg_per_ha.SetMissing();
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
                double TNSoil_kg_per_ha = GetTotalN_Store_TopLayer_kg_per_ha();     // Total N Concentration in soil (mg/kg) and is sum of organanic and inorgance conc at 0-2cm (Obtained from Dairymod)
                if (!MathTools.DoublesAreEqual(TNSoil_kg_per_ha, MathTools.MISSING_DATA_VALUE))
                {
                    TNSoil_kg_per_ha = TNSoil_kg_per_ha * data.SoilNLoadWeighting3;
                    double E = sim.out_Soil_HillSlopeErosion_t_per_ha * 1000.0;// Gross erosion (kg/ha)
                    double SDR = sim.in_SedDelivRatio;                         // Sediment delivery ratio.
                    double NER = data.NEnrichmentRatio;                           // Nitrogen enrighment ratio
                    double d = data.NDepthTopLayer2_mm;                       // depth of surface soil layer mm
                    double phi = sim.in_BulkDensity_g_per_cm3[0];          // soil density t/m3       ( BulkDensity is in g/cm3)
                    double NSoil = data.NAlpha2 * 100.0 * TNSoil_kg_per_ha / (d * phi);    // mg/kg
                    double PN = data.NBeta * E * SDR * NSoil * NER / 1000000.0;

                    out_ParticNInRunoff_kg_per_ha.SetValue(PN);
                    if (!MathTools.DoublesAreEqual(SDR, 0) && !MathTools.DoublesAreEqual(sim.usle_ls_factor, 0))
                        out_PNHLC_kg_per_ha.SetValue(PN / (SDR * sim.usle_ls_factor));
                    else
                        out_PNHLC_kg_per_ha.SetZero();
                    out_TotalNStoreTopLayer_kg_per_ha.SetValue(TNSoil_kg_per_ha);
                }
                else
                {
                    out_ParticNInRunoff_kg_per_ha.SetMissing();
                    out_PNHLC_kg_per_ha.SetMissing();
                    out_TotalNStoreTopLayer_kg_per_ha.SetMissing();
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
        public double GetN03_N_Store_TopLayer_kg_per_ha()
        {
            try
            {
                if (data.DissolvedNInRunoffInputOptions == 1 && data.NLoadInSurfaceLayerTimeSeries.GetCount() != 0)
                    return data.NLoadInSurfaceLayerTimeSeries.GetValueAtDate(sim.today);
                else if (data.DissolvedNInRunoffInputOptions == 2)
                    return data.SoilNLoadData1.GetValueForDayIndex("SoilNLoadData1", nitratesdayindex1, sim.today);
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
        public double GetN03_N_Store_BotLayer_kg_per_ha()
        {
            try
            {
                if (data.DissolvedNInLeachingInputOptions == 1 && data.NLoadInLowerLayersTimeSeries.GetCount() != 0)
                    return data.NLoadInLowerLayersTimeSeries.GetValueAtDate(sim.today);
                else if (data.DissolvedNInLeachingInputOptions == 2)
                {
                    return data.SoilNLoadData2.GetValueForDayIndex("SoilNLoadData2", nitratesdayindex2, sim.today);
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
        public double GetTotalN_Store_TopLayer_kg_per_ha()
        {
            try
            {
                if (data.ParticulateNInRunoffInputOptions == 1)
                {
                    double value1 = 0;
                    double value2 = 0;
                    double value3 = 0;
                    if (data.SoilInorganicNitrateNTimeSeries.GetCount() != 0)
                        value1 = data.SoilInorganicNitrateNTimeSeries.GetValueAtDate(sim.today);
                    if (data.SoilInorganicAmmoniumNTimeSeries.GetCount() != 0)
                        value2 = data.SoilInorganicAmmoniumNTimeSeries.GetValueAtDate(sim.today);
                    if (data.SoilOrganicNTimeSeries.GetCount() != 0)
                        value3 = data.SoilOrganicNTimeSeries.GetValueAtDate(sim.today);
                    if (MathTools.DoublesAreEqual(value1, MathTools.MISSING_DATA_VALUE) || MathTools.DoublesAreEqual(value2, MathTools.MISSING_DATA_VALUE) || MathTools.DoublesAreEqual(value3, MathTools.MISSING_DATA_VALUE))
                        return MathTools.MISSING_DATA_VALUE;
                    return value1 + value1 + value3;
                }
                else if (data.ParticulateNInRunoffInputOptions == 2)
                    return data.SoilNLoadData3.GetValueForDayIndex("SoilNLoadData3", nitratesdayindex3, sim.today);
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
            return (data.DissolvedNInRunoffInputOptions != 0 || data.DissolvedNInLeachingInputOptions != 0 || data.ParticulateNInRunoffInputOptions != 0);
        }


    }
}
