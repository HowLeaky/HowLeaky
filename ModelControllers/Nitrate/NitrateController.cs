using HowLeaky.CustomAttributes;
using HowLeaky.Tools.Helpers;
using System;
using HowLeaky.DataModels;
using HowLeaky.OutputModels;
using System.Collections.Generic;
using HowLeaky.Interfaces;

namespace HowLeaky.ModelControllers
{
    public class NitrateController : HLController
    {
        public NitrateInputModel InputModel { get; set; }
        //public NitrateOutputDataModel Output { get; set; }

        public int Nitratesdayindex1 { get; set; }
        public int Nitratesdayindex2 { get; set; }
        public int Nitratesdayindex3 { get; set; }

        public double LastNAppliedRate { get; set; } = 0;
        public double NitrateCumRain { get; set; } = 0;

        //Reportable Outputs
        [Output("Dissolved N03 N In Runoff", "mg/L")]
        public double N03NDissolvedInRunoff { get; set; }
        [Output("N03 Runoff Load", "kg/ha")]
        public double N03NRunoffLoad { get; set; }
        [Output("Dissolved N03 N In Leaching", "mg/L")]
        public double N03NDissolvedLeaching { get; set; }
        [Output("N03 N Leaching Load", "kg/ha")]
        public double N03NLeachingLoad { get; set; }
        [Output("Particulate N in Runoff", "kg/ha")]
        public double ParticNInRunoff { get; set; }
        [Output("N03 N Store in top layer", "kg/ha")]
        public double N03NStoreTopLayer { get; set; }
        [Output("N03 N Store in bot layer", "kg/ha")]
        public double N03NStoreBotLayer { get; set; }
        [Output("Total N Store in top layer", "kg/ha")]
        public double TotalNStoreTopLayer { get; set; }
        [Output("PNHLC", "kg/ha")]
        public double PNHLCa { get; set; }
        [Output("", "")]
        public double DrainageInN03Period { get; set; }
        [Output("", "")]
        public double RunoffInN03Period { get; set; }

        /// <summary>                                                     
        /// 
        /// </summary>
        public NitrateController(Simulation sim) : base(sim)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sim"></param>
        public NitrateController(Simulation sim, List<InputModel> inputModels) : this(sim)
        {
            InputModel = (NitrateInputModel)inputModels[0];
            InitOutputModel();
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
                bool CanSimulateNitrate = (InputModel.DissolvedNinRunoff.index != 0 || InputModel.DissolvedNinLeaching.index != 0 || InputModel.ParticulateNinRunoff.index != 0);
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
        /// <returns></returns>
        public override InputModel GetInputModel()
        {
            return InputModel;
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
            if (InputModel.DissolvedNinRunoff.index == 1)
            {
                return InputModel.DissolvedNinRunoff.NLoadInSurfaceLayerTimeSeries.GetCount() != 0;
            }
            else if (InputModel.DissolvedNinRunoff.index == 2 || InputModel.DissolvedNinRunoff.index == 3)
            {
                return true;
            }
            return false;
        }


        //        //From Howleaky developers (Brett Robinson), based on the concept that soil and runoff water mixing increases up to a maximum of k.
        //        // DN = Nsurface * K(1- exp(-cvQ)
        //        // where DN is the Nitrate conc in the runoff (mg/L)
        //        // k is the parameter that regulates mixing of soil and runoff water, (suggested 0.5)
        //        // cv is parameter that describes the curvature of change in the soil and water runoff at increasing runoff values (initial guess is 0.2)
        //        // Q is runoff (mm)
        //        // Nsurface (mg N/kg) is the soil nitrate concentrate in teh survace layer (0-2cm), which in our approach is derived from nitrate load (NLsoil in kg/ha) in surface layer from DairyMod
        //        // NSurface = alpha*100*NLsoil/(depth*soildensity)
        //        // Then dissolved N load (NL, kg/ha) in runoff is
        //        // DL=ND*Q/100.0;
        //        // NOTATION USE HERE IS TO BE CONSISTENT WITH THAT USED BY VIC DPI
        //        void CalculateDissolvedNInRunoff_VicDPI()
        //{
        //	try
        //	{
        //		double NL_kg_ha = GetN03_N_Store_TopLayer_kg_per_ha();  //Nitrate load in surface layer (From Dairymod)
        //		if(NL_kg_ha!=-32768)
        //		{
        //			NL_kg_ha=NL_kg_ha* SoilNitrateLoadWeighting1;
        //        double k = Nk; // INPUT parameter that regulates mixing of soil and runoff water
        //        double cv = Ncv;  //INPUT parameter that describes the curvature of change in soil and water runoff at increasing runoff values
        //        double Q = runoff;   //runoff amount

        //        double d = NDepthTopLayer1;    //depth of surface soil layer mm
        //        double phi = BulkDensity[0]; //soil density t/m3       ( BulkDensity is in g/cm3)

        //        //double kgs_soil_in_layer_1=BulkDensity[0]*1000.0*depth[1]*10000.0/1000.0;//per ha

        //        double NSoil = NAlpha * 100.0 * NL_kg_ha / (d * phi);     //mg/kg
        //        double DN = NSoil * k * (1 - exp(-cv * Q));
        //        double DL = DN * Q / 100.0;

        //        N03_N_Store_TopLayer_kg_per_ha=NL_kg_ha;
        //			N03_N_Dissolved_Runoff_mg_per_L=DN;
        //			N03_N_Load_Runoff_kg_per_ha=DL;
        //		}
        //		else
        //		{
        //			N03_N_Store_TopLayer_kg_per_ha	=-32768;
        //			N03_N_Dissolved_Runoff_mg_per_L	=-32768;
        //			N03_N_Load_Runoff_kg_per_ha		=-32768;
        //		}
        //	}
        //	catch(...)
        //	{
        //		ControlError="CalculateDissolvedNinRunoff";
        //		throw;
        //	}
        //}

        void CalculateDissolvedNInRunoffRattray()
        {
            try
            {
                double NLKgHa = GetN03NStoreTopLayerkgPerha();  //Nitrate load in surface layer (From Dairymod)
                if (NLKgHa != MathTools.MISSING_DATA_VALUE)
                {
                    double a = InputModel.DissolvedNinRunoff.N_DanRat_Alpha;
                    double b = -InputModel.DissolvedNinRunoff.N_DanRat_Beta;
                    double maxconc = InputModel.DissolvedNinRunoff.N_DanRat_MaxRunOffConc;
                    double minconc = InputModel.DissolvedNinRunoff.N_DanRat_MinRunOffConc;
                    double rate = InputModel.DissolvedNinRunoff.FertilizerInputDateSequences.ValueAtDate(Sim.Today);
                    if (rate > 0 && rate != MathTools.MISSING_DATA_VALUE)
                    {
                        NitrateCumRain = Sim.SoilController.EffectiveRain;
                        LastNAppliedRate = rate;
                    }
                    else
                    {
                        NitrateCumRain = NitrateCumRain + Sim.SoilController.EffectiveRain;
                    }


                    double DINMgPerL = 0;
                    if (Sim.SoilController.Runoff > 0)
                    {

                        if (NitrateCumRain > 0)
                        {
                            DINMgPerL = LastNAppliedRate / a * Math.Pow(NitrateCumRain, b);
                        }
                        else
                        {
                            DINMgPerL = 0;
                        }
                        DINMgPerL = Math.Min(maxconc, Math.Max(minconc, DINMgPerL));
                    }

                    N03NStoreTopLayer = MathTools.MISSING_DATA_VALUE;
                    N03NDissolvedInRunoff = DINMgPerL;
                    N03NRunoffLoad = MathTools.MISSING_DATA_VALUE;
                }
                else
                {
                    N03NStoreTopLayer = MathTools.MISSING_DATA_VALUE;
                    N03NDissolvedInRunoff = MathTools.MISSING_DATA_VALUE;
                    N03NRunoffLoad = MathTools.MISSING_DATA_VALUE;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CalculateDissolvedNInRunoffRattray", ex);
            }
        }

        //void __fastcall TPERFECTSimulation::CalculateDissolvedNInRunoff_Fraser(void)
        //{
        //	try
        //	{
        //		double NL_kg_ha = GetN03_N_Store_TopLayer_kg_per_ha();  //Nitrate load in surface layer (From Dairymod)
        //		if(NL_kg_ha!=-32768)
        //		{
        //			double invk = 1.0 / Nk;
        //double DL = N_GraFraz_DL;
        //double RL = N_GraFraz_RL;
        //double lowlimit = N_GraFraz_LowLimitDINConc;

        //double fertapp_kg_per_ha = GetFertilizerInputValue();
        //double DINinput = invk * fertapp_kg_per_ha;

        //double solid = DINinput + GetMaximum(lowlimit, solid_yesterday - GetMaximum(solid_yesterday * DL, effective_rain * RL));

        //N03_N_Store_TopLayer_kg_per_ha=0;
        //			N03_N_Dissolved_Runoff_mg_per_L=solid;
        //			N03_N_Load_Runoff_kg_per_ha=0;
        //			solid_yesterday=solid;
        //		}
        //		else
        //		{
        //			N03_N_Store_TopLayer_kg_per_ha	=-32768;
        //			N03_N_Dissolved_Runoff_mg_per_L	=-32768;
        //			N03_N_Load_Runoff_kg_per_ha		=-32768;
        //		}
        //	}
        //	catch(...)
        //	{
        //		ControlError="CalculateDissolvedNinRunoff";
        //		throw;
        //	}
        //}

        //double __fastcall TPERFECTSimulation::GetFertilizerInputValue(void)
        //{
        //	int index = IsDateInSequenceList(today, FertilizerDateList);
        //	if(index>=0&&index<FertilizerValueList.size())
        //		return FertilizerValueList[index];
        //	return 0;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanCalculateDissolvedNInLeaching()
        {
            if (InputModel.DissolvedNinLeaching.index == 1)
            {
                return InputModel.DissolvedNinLeaching.NLoadInLowerLayersTimeSeries.GetCount() != 0;
            }
            else if (InputModel.DissolvedNinLeaching.index == 2)
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
            if (InputModel.ParticulateNinRunoff.index == 1)
            {
                return (InputModel.ParticulateNinRunoff.NitrateSourceData.InorganicNitrateNTimeseries.GetCount() != 0 &&
                          InputModel.ParticulateNinRunoff.NitrateSourceData.InorganicAmmoniumNTimeseries.GetCount() != 0);
            }
            else if (InputModel.ParticulateNinRunoff.index == 2)
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
            if (InputModel.DissolvedNinRunoff.index == (int)NitrateModelSubType.RattrayEmpericalModel)
            {
                CalculateDissolvedNInRunoffRattray();
                return;
            }

            try
            {
                double NLKgHa = GetN03NStoreTopLayerkgPerha();  //Nitrate load in surface layer (From Dairymod)
                if (!MathTools.DoublesAreEqual(NLKgHa, MathTools.MISSING_DATA_VALUE))
                {
                    NLKgHa = NLKgHa * InputModel.DissolvedNinRunoff.SoilNitrateLoadWeighting1;
                    double k = InputModel.DissolvedNinRunoff.Nk;                               // INPUT parameter that regulates mixing of soil and runoff water
                    double cv = InputModel.DissolvedNinRunoff.Ncv;                             //INPUT parameter that describes the curvature of change in soil and water runoff at increasing runoff values
                    double Q = Sim.SoilController.Runoff;                   //runoff amount
                    double d = InputModel.DissolvedNinRunoff.NDepthTopLayer1;                  //depth of surface soil layer mm
                    double phi = Sim.SoilController.InputModel.BulkDensity.Values[0];   //soil density t/m3       ( BulkDensity is in g/cm3)
                    double NSoil = InputModel.DissolvedNinRunoff.NAlpha * 100.0 * NLKgHa / (d * phi);      //mg/kg
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
                    NSoilKgPerHa = NSoilKgPerHa * InputModel.DissolvedNinLeaching.SoilNitrateLoadWeighting2;
                    double deltadepth = InputModel.DissolvedNinLeaching.DepthBottomLayer;
                    if (deltadepth > 0)
                    {
                        double soilwater = (Sim.SoilController.InputModel.Saturation.Values[Sim.SoilController.LayerCount - 1] - Sim.SoilController.InputModel.AirDry.Values[Sim.SoilController.LayerCount - 1]) / 100.0 * deltadepth;
                        double LE = InputModel.DissolvedNinLeaching.NitrateLeachingEfficiency;                      //Leaching efficiency (INPUT)
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
                    TNSoilKgPerHa = TNSoilKgPerHa * InputModel.ParticulateNinRunoff.SoilNitrateLoadWeighting3;
                    double E = Sim.SoilController.HillSlopeErosion * 1000.0;// Gross erosion (kg/ha)
                    double SDR = Sim.SoilController.InputModel.SedDelivRatio;                         // Sediment delivery ratio.
                    double NER = InputModel.ParticulateNinRunoff.NEnrichmentRatio;                           // Nitrogen enrighment ratio
                    double d = InputModel.ParticulateNinRunoff.NDepthTopLayer2;                       // depth of surface soil layer mm
                    double phi = Sim.SoilController.InputModel.BulkDensity.Values[0];          // soil density t/m3       ( BulkDensity is in g/cm3)
                    double NSoil = InputModel.ParticulateNinRunoff.NAlpha * 100.0 * TNSoilKgPerHa / (d * phi);    // mg/kg
                    double PN = InputModel.ParticulateNinRunoff.NBeta * E * SDR * NSoil * NER / 1000000.0;

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
                if (InputModel.DissolvedNinRunoff.index == 1 && InputModel.DissolvedNinRunoff.NLoadInSurfaceLayerTimeSeries.GetCount() != 0)
                {
                    return InputModel.DissolvedNinRunoff.NLoadInSurfaceLayerTimeSeries.GetValueAtDate(Sim.Today);
                }
                else if (InputModel.DissolvedNinRunoff.index == 2)
                {
                    //return DataModel.SoilNLoadData1.GetValueForDayIndex("SoilNLoadData1", Nitratesdayindex1, Sim.Today);
                    return InputModel.DissolvedNinRunoff.SoilNLoadData1.GetValueForDayIndex("SoilNLoadData1", Sim.Today);
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
                if (InputModel.DissolvedNinLeaching.index == 1 && InputModel.DissolvedNinLeaching.NLoadInLowerLayersTimeSeries.GetCount() != 0)
                {
                    return InputModel.DissolvedNinLeaching.NLoadInLowerLayersTimeSeries.GetValueAtDate(Sim.Today);
                }
                else if (InputModel.DissolvedNinLeaching.index == 2)
                {
                    //return DataModel.SoilNLoadData2.GetValueForDayIndex("SoilNLoadData2", Nitratesdayindex2, Sim.Today);
                    return InputModel.DissolvedNinLeaching.SoilNLoadData2.GetValueForDayIndex("SoilNLoadData2", Sim.Today);
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
                if (InputModel.ParticulateNinRunoff.index == 1)
                {
                    double value1 = 0;
                    double value2 = 0;
                    double value3 = 0;
                    if (InputModel.ParticulateNinRunoff.NitrateSourceData.InorganicNitrateNTimeseries.GetCount() != 0)
                    {
                        value1 = InputModel.ParticulateNinRunoff.NitrateSourceData.InorganicNitrateNTimeseries.GetValueAtDate(Sim.Today);
                    }
                    if (InputModel.ParticulateNinRunoff.NitrateSourceData.InorganicAmmoniumNTimeseries.GetCount() != 0)
                    {
                        value2 = InputModel.ParticulateNinRunoff.NitrateSourceData.InorganicAmmoniumNTimeseries.GetValueAtDate(Sim.Today);
                    }
                    if (InputModel.ParticulateNinRunoff.NitrateSourceData.OrganicNTimeseries.GetCount() != 0)
                    {
                        value3 = InputModel.ParticulateNinRunoff.NitrateSourceData.OrganicNTimeseries.GetValueAtDate(Sim.Today);
                    }
                    if (MathTools.DoublesAreEqual(value1, MathTools.MISSING_DATA_VALUE) || MathTools.DoublesAreEqual(value2, MathTools.MISSING_DATA_VALUE) || MathTools.DoublesAreEqual(value3, MathTools.MISSING_DATA_VALUE))
                    {
                        return MathTools.MISSING_DATA_VALUE;
                    }
                    return value1 + value1 + value3;
                }
                else if (InputModel.ParticulateNinRunoff.index == 2)
                {
                    //return DataModel.SoilNLoadData3.GetValueForDayIndex("SoilNLoadData3", Nitratesdayindex3, Sim.Today);
                    return InputModel.ParticulateNinRunoff.SoilNLoadData3.GetValueForDayIndex("SoilNLoadData3", Sim.Today);
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
            return (InputModel.DissolvedNinRunoff.index != 0 || InputModel.DissolvedNinLeaching.index != 0 || InputModel.ParticulateNinRunoff.index != 0);
        }
    }
}
