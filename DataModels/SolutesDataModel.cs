using HowLeaky.CustomAttributes;
using System.Collections.Generic;


namespace HowLeaky.DataModels
{
    public class SolutesDataModel : DataModel
    {
        //Input Parameters
        public int StartConcOption { get; set; }
        [Unit("mg_per_kg")]
        public double Layer1InitialConc { get; set; }
        [Unit("mg_per_kg")]
        public double Layer2InitialConc { get; set; }
        [Unit("mg_per_kg")]
        public double Layer3InitialConc { get; set; }
        [Unit("mg_per_kg")]
        public double Layer4InitialConc { get; set; }
        [Unit("mg_per_kg")]
        public double Layer5InitialConc { get; set; }
        [Unit("mg_per_kg")]
        public double DefaultInitialConc { get; set; }
        [Unit("mg_per_L")]
        public double RainfallConcentration { get; set; }
        [Unit("mg_per_L")]
        public double IrrigationConcentration { get; set; }
        public double MixingCoefficient { get; set; }                   

        //**************************************************************************
        //Daily outputs
        //**************************************************************************
        public double out_TotalSoilSolute_kg_per_ha { get; set; }       // Total Soil Solute (Load) (kg/ha).
        public List<double> out_LayerSoluteLoad_kg_per_ha { get; set; }      // Layer Solute (Load) (kg/ha).
        public List<double> out_LayerSoluteConc_mg_per_L { get; set; }       // Layer Solute (Conc) (mg/L).
        public List<double> out_LayerSoluteConc_mg_per_kg { get; set; }      // Layer Solute (Conc) (mg/kg).
        public double out_LeachateSoluteConc_mg_per_L { get; set; }     // Leachate Solute Concentration (mg/L).
        public double out_LeachateSoluteLoad_kg_per_ha { get; set; }    // Leachate Solute Load (kg/ha).

        //**************************************************************************
        //Monthly outputs
        //**************************************************************************
        public List<double> mo_SoluteLeachingLoad_kg_per_ha { get; set; }    //
        public List<double> mo_SoluteLoadSoil_kg_per_ha { get; set; }        //
        public List<double> mo_SoluteCount { get; set; }                     //

        //**************************************************************************
        //Summary outputs
        //**************************************************************************
        public double so_SoilWaterSoluteConc_mg_per_L { get; set; }     // Soil Water Solute Conc (mg/l).
        public double so_SoilSoluteLoad_kg_per_ha { get; set; }     // Soil Solute Load (kg/ha).
        public double so_LeachingSoluteExport_kg_per_ha { get; set; }   // Leaching Solute Export (kg/ha).
        public double so_LeachSoluateConc_mg_per_L { get; set; }        // Leaching Solute Conc (mg/l).
                                                                        //**************************************************************************

        //Output Parameters


        public double sum_solute_load_soil_kg_per_ha { get; set; }
        public double sum_solute_leaching_load_kg_per_ha { get; set; }

        public double annual_solute_leaching_conc_mg_per_L { get; set; }
        public double annual_solute_leaching_load_kg_per_ha { get; set; }
        public double annual_solute_load_soil_kg_per_ha { get; set; }
    }
}

