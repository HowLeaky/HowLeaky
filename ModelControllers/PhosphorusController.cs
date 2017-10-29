using HowLeaky.CustomAttributes;
using HowLeaky.Tools;
using HowLeaky.Models;
using HowLeaky.DataModels;
using System;

namespace HowLeaky.ModelControllers
{
    public class PhosphorusController : HLObject
    {
        static int ENRICHMENT_RATIO = 0;
        static int ENRICHMENT_CLAY = 1;
        static int DISSOLVED_P_VICDPI = 0;
        static int DISSOLVED_P_QLDREEF = 1;

        //**************************************************************************
        //Outputs
        //**************************************************************************
        public double out_ParticulateConc_mg_per_l { get; set; }        // Particulate P Conc (mg/l)
        public double out_DissolvedConc_mg_per_l { get; set; }          // Dissolved P Conc (mg/l)
        public double out_BioAvailParticPConc_mg_per_l { get; set; }    // Bioavailable particulate P Conc(mg/l)
        public double out_BioAvailPConc_mg_per_l { get; set; }          // Bioavailable P Conc(mg/l)
        public double out_TotalPConc_mg_per_l { get; set; }             // Total P Conc (mg/l)
        public double out_ParticPExport_kg_per_ha { get; set; }         // Particulate P export(kg/ha)
        public double out_BioAvailParticPExport_kg_per_ha { get; set; } // Bioavailable particulate P export(kg/ha)
        public double out_TotalBioAvailExport_kg_per_ha { get; set; }   // Bioavailable P export(kg/ha)
        public double out_TotalP_kg_per_ha { get; set; }                // Total Phosphorus export(kg/ha)
        public double out_CKQ_t_per_ha { get; set; }
        public double out_PPHLC_kg_per_ha { get; set; }
        public double out_Phos_Export_Dissolve_kg_per_ha { get; set; }

        //public	double so_DissolvePExport_kg_per_ha{get;set;}		// Dissolved P export(kg/ha)
        //public	double so_EMC_mg_per_l{get;set;}						// Phosphorus EMC(mg/L)

        //**************************************************************************
        //Internals
        //**************************************************************************
        public double maxPhos_Conc_BioPartic_mg_per_L { get; set; }
        public double maxPhos_Conc_Bio_mg_per_L { get; set; }
        public double maxPhos_Conc_Partic_mg_per_L { get; set; }
        public double maxPhos_Conc_Total_mg_per_L { get; set; }
        public double maxPhos_Conc_Dissolve_mg_per_L { get; set; }

        public PhosphorusDataModel dataModel;
        /// <summary>
        /// 
        /// </summary>
        public PhosphorusController() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sim"></param>
        public PhosphorusController(Simulation sim) : base(sim) { }
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
        public void InitialisePhosphorusParameters()
        {
            maxPhos_Conc_BioPartic_mg_per_L = 0;
            maxPhos_Conc_Bio_mg_per_L = 0;
            maxPhos_Conc_Partic_mg_per_L = 0;
            maxPhos_Conc_Total_mg_per_L = 0;
            maxPhos_Conc_Dissolve_mg_per_L = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        public void UpdatePhosphorusSummaryValues() { }
        /// <summary>
        /// 
        /// </summary>
        public override void Simulate()
        {
            try
            {
                if (CansimulatePhosphorus())
                {
                    if (sim.out_WatBal_Runoff_mm > 0)
                    {
                        CalculateDissolvedPhosphorus();
                        CalculateParticulatePhosphorus();
                        CalculateTotalPhosphorus();
                        CalculateBioavailableParticulatePhosphorus();
                        CalculateBioavailablePhosphorus();
                        TestMaximumPhosphorusConcentrations();
                    }
                    else
                    {
                        ResetPhosphorusOutputParameters();

                    }
                    CalculateCATCHMODSOutputs();
                }
                UpdatePhosphorusSummaryValues();
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
        public bool CansimulatePhosphorus()
        {
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateDissolvedPhosphorus()
        {

            double phos_saturation_index = 0;
            double p_max_sorption = 0;
            if (dataModel.DissolvedPOpt == DISSOLVED_P_VICDPI)
            {
                p_max_sorption = 1447 * (1 - Math.Exp(-0.001 * dataModel.PBI));
            }
            else
            {
                p_max_sorption = 5.84 * dataModel.PBI - 0.0096 * Math.Pow(dataModel.PBI, 2);
                if (p_max_sorption < 50)
                    p_max_sorption = 50;
            }

            double p_enrich = CalculatePhosphorusEnrichmentRatio();
            if (!MathTools.DoublesAreEqual(p_max_sorption, 0))
                phos_saturation_index = (dataModel.ColwellP * p_enrich) / (p_max_sorption) * 100.0;
            else
            {
                phos_saturation_index = 0;
                MathTools.LogDivideByZeroError("CalculateDissolvedPhosphorus", "p_max_sorption", "phos_saturation_index");
            }

            if (dataModel.DissolvedPOpt == DISSOLVED_P_VICDPI)
            {
                //The following is the original fn published at MODSIM11:
                if (phos_saturation_index < 5)
                    out_DissolvedConc_mg_per_l = (10.0 * phos_saturation_index / 1000.0);
                else
                    out_DissolvedConc_mg_per_l = ((-100.0 + 30 * phos_saturation_index) / 1000.0);

            }
            else
            {
                if (phos_saturation_index < 10)
                    out_DissolvedConc_mg_per_l = (7.5 * phos_saturation_index / 1000.0);
                else
                    out_DissolvedConc_mg_per_l = ((-200.0 + 27.5 * phos_saturation_index) / 1000.0);
            }
            out_Phos_Export_Dissolve_kg_per_ha = (out_DissolvedConc_mg_per_l / 1000000.0 * sim.out_WatBal_Runoff_mm * 10000.0);//CHECK - this wasn't marked as an output parameter
        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateParticulatePhosphorus()
        {
            double p_enrich = CalculatePhosphorusEnrichmentRatio();
            double p_sed_conc_g_per_l = 0;
            if (!MathTools.DoublesAreEqual(sim.out_WatBal_Runoff_mm, 0))//&&sim.in_SedDelivRatio!=0)
            {
                // convert t/ha to g/ha and sim.out_WatBal_Runoff_mm(mm) to L/ha.  Then the division yields g/L of sediment.
                p_sed_conc_g_per_l = sim.out_Soil_HillSlopeErosion_t_per_ha * 1000000.0 / (sim.out_WatBal_Runoff_mm * 10000.0) * sim.in_SedDelivRatio;
            }
            // convert sed conc from g/L to mg/L and totalPconc from mg/kg (I assume it is in mg/kg) to g/g
            out_ParticulateConc_mg_per_l = (p_sed_conc_g_per_l * 1000.0 * dataModel.TotalPConc / 1000000.0 * p_enrich);

            out_ParticPExport_kg_per_ha = (out_ParticulateConc_mg_per_l / 1000000.0 * sim.out_WatBal_Runoff_mm * 10000.0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double CalculatePhosphorusEnrichmentRatio()
        {
            if (dataModel.PEnrichmentOpt == ENRICHMENT_RATIO)
            {
                //input a constant (range = 1 to ~10).
                return dataModel.EnrichmentRatio;
            }
            else if (dataModel.PEnrichmentOpt == ENRICHMENT_CLAY)
            {
                //Clay%.  This function is based on a few data from Qld field experiments.
                //Penrichment=MIN(10,MAX(1,15-0.33*clay)). The results of this funtion are:
                return Math.Min(10.0, Math.Max(1, 15 - 0.33 * dataModel.ClayPercentage));
            }
            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateTotalPhosphorus()
        {
            out_TotalPConc_mg_per_l = (out_DissolvedConc_mg_per_l + out_ParticulateConc_mg_per_l);
            out_TotalP_kg_per_ha = (out_Phos_Export_Dissolve_kg_per_ha + out_ParticPExport_kg_per_ha); //CHECK Phos_Export_Dissolve wasn't marked as out;

        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateBioavailableParticulatePhosphorus()
        {
            double pA;
            if (!MathTools.DoublesAreEqual(dataModel.TotalPConc, 0))
            {
                pA = dataModel.ColwellP * 1.2 / dataModel.TotalPConc;
            }
            else
            {
                pA = 0;
                //	LogDivideByZeroError("CalculateBioavailableParticulatePhosphorus","in_TotalPConc_mg_per_kg","pA");
            }
            out_BioAvailParticPConc_mg_per_l = (out_ParticulateConc_mg_per_l * pA);
            out_BioAvailParticPExport_kg_per_ha = (out_ParticPExport_kg_per_ha * pA);
        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateBioavailablePhosphorus()
        {
           out_BioAvailPConc_mg_per_l = (0.8 * out_DissolvedConc_mg_per_l + out_BioAvailParticPConc_mg_per_l);
           out_TotalBioAvailExport_kg_per_ha = (0.8 * out_Phos_Export_Dissolve_kg_per_ha + out_BioAvailParticPExport_kg_per_ha);
        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateCATCHMODSOutputs()
        {
            if (sim.out_WatBal_Runoff_mm > 0 && sim.out_Soil_HillSlopeErosion_t_per_ha > 0)
            {
                if (!MathTools.DoublesAreEqual(sim.in_SedDelivRatio, 0) && !MathTools.DoublesAreEqual(sim.usle_ls_factor, 0))
                {
                    out_PPHLC_kg_per_ha = (out_ParticPExport_kg_per_ha / (sim.in_SedDelivRatio * sim.usle_ls_factor));
                }
                else
                {
                    out_PPHLC_kg_per_ha = 0;
                }
            }
            else
            {
                out_PPHLC_kg_per_ha = 0;
            }
            out_CKQ_t_per_ha = (sim.sed_catchmod);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetPhosphorusOutputParameters()
        {
            maxPhos_Conc_BioPartic_mg_per_L = 0;
            maxPhos_Conc_Bio_mg_per_L = 0;
            maxPhos_Conc_Partic_mg_per_L = 0;
            maxPhos_Conc_Total_mg_per_L = 0;
            maxPhos_Conc_Dissolve_mg_per_L = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        public void TestMaximumPhosphorusConcentrations()
        {
            if (maxPhos_Conc_Partic_mg_per_L < out_ParticulateConc_mg_per_l)
                maxPhos_Conc_Partic_mg_per_L = out_ParticulateConc_mg_per_l;
            if (maxPhos_Conc_Total_mg_per_L < out_TotalPConc_mg_per_l)
                maxPhos_Conc_Total_mg_per_L = out_TotalPConc_mg_per_l;
            if (maxPhos_Conc_Dissolve_mg_per_L < out_DissolvedConc_mg_per_l)
                maxPhos_Conc_Dissolve_mg_per_L = out_DissolvedConc_mg_per_l;
            if (maxPhos_Conc_BioPartic_mg_per_L < out_BioAvailParticPConc_mg_per_l)
                maxPhos_Conc_BioPartic_mg_per_L = out_BioAvailParticPConc_mg_per_l;
            if (maxPhos_Conc_Bio_mg_per_L < out_TotalPConc_mg_per_l)
                maxPhos_Conc_Bio_mg_per_L = out_BioAvailPConc_mg_per_l;
        }
    }
}
