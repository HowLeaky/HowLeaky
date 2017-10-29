using HowLeaky.CustomAttributes;
using HowLeaky.Tools;
using HowLeaky.Models;
using HowLeaky.DataModels;
using System;


namespace HowLeaky.ModelControllers
{
    public class SolutesController : HLObject
    {
        public SolutesDataModel dataModel;
        /// <summary>
        /// 
        /// </summary>
        public SolutesController() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sim"></param>
        public SolutesController(Simulation sim) : base(sim) { }
        /// <summary>
        /// 
        /// </summary>
        public override void Initialise()
        {
            throw new NotImplementedException();
        }



        //        // *****************************************************************************
        //        // 							   RegisterInputs
        //        // This method is used to setup proxy objects used in connecting the input
        //        // parameter objects to their respective model variables. This method was used
        //        // so that the loading of input parameter values could be automated, and that
        //        // we didn't have to manually match inputs in the code, as we did in earlier
        //        // versions of HowLeaky.
        //        // *****************************************************************************
        //        void RegisterInputs(TSimulationInputDefinitions* im)
        //{

        //    RegisterInputVariable(in_StartConcOption, im);

        //    RegisterInputVariable(in_Layer1InitialConc_mg_per_kg, im);

        //    RegisterInputVariable(in_Layer2InitialConc_mg_per_kg, im);

        //    RegisterInputVariable(in_Layer3InitialConc_mg_per_kg, im);

        //    RegisterInputVariable(in_Layer4InitialConc_mg_per_kg, im);

        //    RegisterInputVariable(in_Layer5InitialConc_mg_per_kg, im);

        //    RegisterInputVariable(in_DefaultInitialConc_mg_per_kg, im);

        //    RegisterInputVariable(in_RainfallConcentration_mg_per_L, im);

        //    RegisterInputVariable(in_IrrigationConcentration_mg_per_L, im);

        //    RegisterInputVariable(in_MixingCoefficient, im);
        //    }

        //    void RegisterOutputs(TSimulationInputDefinitions* im)
        //{
        //    //Daily outputs
        //    RegisterDailyOutput(out_TotalSoilSolute_kg_per_ha, im);

        //    RegisterDailyOutput(out_LayerSoluteLoad_kg_per_ha, im);

        //    RegisterDailyOutput(out_LayerSoluteConc_mg_per_L, im);

        //    RegisterDailyOutput(out_LayerSoluteConc_mg_per_kg, im);

        //    RegisterDailyOutput(out_LeachateSoluteConc_mg_per_L, im);

        //    RegisterDailyOutput(out_LeachateSoluteLoad_kg_per_ha, im);
        //    //Monthly outputs
        //    RegisterDailyOutput(mo_SoluteLeachingLoad_kg_per_ha, im);

        //    RegisterDailyOutput(mo_SoluteLoadSoil_kg_per_ha, im);

        //    RegisterDailyOutput(mo_SoluteCount, im);
        //    //Yearly outputs
        //    RegisterDailyOutput(so_SoilWaterSoluteConc_mg_per_L, im);

        //    RegisterDailyOutput(so_SoilSoluteLoad_kg_per_ha, im);

        //    RegisterDailyOutput(so_LeachingSoluteExport_kg_per_ha, im);

        //    RegisterDailyOutput(so_LeachSoluateConc_mg_per_L, im);
        //}


        /// <summary>
        /// 
        /// </summary>
        public override void Simulate()
        {
            try
            {
                if (CanSimulateSolutes())
                {
                    double rain = sim.out_Rain_mm;
                    double runoff = sim.out_WatBal_Runoff_mm;
                    double irrigation_amount_mm = sim.out_WatBal_Irrigation_mm;
                    //calculte solute input loadings.
                    double kgs_soil_in_layer_1 = sim.in_BulkDensity_g_per_cm3[0] * 1000.0 * sim.depth[1] * 10000.0 / 1000.0;//per ha
                    if (rain > 0)
                    {
                        if (!MathTools.DoublesAreEqual(kgs_soil_in_layer_1, 0))
                        {
                            dataModel.out_LayerSoluteConc_mg_per_kg[0] += dataModel.RainfallConcentration * (rain - runoff) * 10000.0 / kgs_soil_in_layer_1;
                        }
                        else
                        {

                            MathTools.LogDivideByZeroError("CalculateSolutes", "kgs_soil_in_layer_1", "out_LayerSoluteConc_mg_per_kg[0]");
                        }
                    }
                    if (!MathTools.DoublesAreEqual(irrigation_amount_mm, 0))
                    {
                        if (!MathTools.DoublesAreEqual(kgs_soil_in_layer_1, 0))
                        {
                            dataModel.out_LayerSoluteConc_mg_per_kg[0] += dataModel.IrrigationConcentration * irrigation_amount_mm * 10000.0 / kgs_soil_in_layer_1;
                        }
                        else
                        {

                            MathTools.LogDivideByZeroError("CalculateSolutes", "kgs_soil_in_layer_1", "out_LayerSoluteConc_mg_per_kg[0]");
                        }
                    }
                    //initialise total solute count;
                    dataModel.out_TotalSoilSolute_kg_per_ha = 0;
                    //Route solutes down through layer.
                    for (int i = 0; i < sim.in_LayerCount; ++i)
                    {
                        double SW_rel_OD = sim.SoilWater_rel_wp[i] + sim.Wilting_Point_RelOD_mm[i];
                        double StartOfDay_SW_rel_OD = SW_rel_OD + sim.Seepage[i + 1];


                        if (!MathTools.DoublesAreEqual(StartOfDay_SW_rel_OD, 0) && !MathTools.DoublesAreEqual(SW_rel_OD, 0))
                        {
                            double kgs_soil_in_layer = sim.in_BulkDensity_g_per_cm3[i] * 1000.0 * (sim.in_Depths[i + 1] - sim.in_Depths[i]) * 10000.0 / 1000.0;

                            //calculate the potential drained loadings (doesn't take into account mixing effects)
                            double potential_drained_solute_mg = 0;
                            if (!MathTools.DoublesAreEqual(StartOfDay_SW_rel_OD, 0))
                                potential_drained_solute_mg = (dataModel.out_LayerSoluteConc_mg_per_kg[i] * kgs_soil_in_layer / StartOfDay_SW_rel_OD) * sim.Seepage[i + 1];
                            else
                            {

                                MathTools.LogDivideByZeroError("CalculateSolutes", "StartOfDay_SW_rel_OD", "potential_drained_solute_mg");
                            }
                            //calculate the actual drained loadings
                            double actual_drained_solute_mg = dataModel.MixingCoefficient * potential_drained_solute_mg;

                            //take the drained solute load away from the balance in the layer
                            if (!MathTools.DoublesAreEqual(kgs_soil_in_layer, 0))
                            {
                                /*OUTPUT*/
                                dataModel.out_LayerSoluteConc_mg_per_kg[i] -= actual_drained_solute_mg / kgs_soil_in_layer;
                            }
                            else
                            {
                                dataModel.out_LayerSoluteConc_mg_per_kg[i] = 0;

                                MathTools.LogDivideByZeroError("CalculateSolutes", "kgs_soil_in_layer", "out_LayerSoluteConc_mg_per_kg[i]");
                            }

                            //calculate the solute load in the layer
                            /*OUTPUT*/
                            dataModel.out_LayerSoluteLoad_kg_per_ha[i] = dataModel.out_LayerSoluteConc_mg_per_kg[i] * kgs_soil_in_layer / 1000000.0;

                            //keep track of total load
                            /*OUTPUT*/
                            dataModel.out_TotalSoilSolute_kg_per_ha += dataModel.out_LayerSoluteLoad_kg_per_ha[i];

                            //calculate solute concentration in layer
                            double SW_Volumetric;
                            if (!MathTools.DoublesAreEqual(sim.depth[i + 1], 0))
                                SW_Volumetric = SW_rel_OD / (double)(sim.depth[i + 1]);
                            else
                            {
                                SW_Volumetric = 0;

                                MathTools.LogDivideByZeroError("CalculateSolutes", "depth[i+1]", "SW_Volumetric");
                            }
                            if (!MathTools.DoublesAreEqual(SW_Volumetric, 0))
                                /*OUTPUT*/
                                dataModel.out_LayerSoluteConc_mg_per_L[i] = dataModel.out_LayerSoluteConc_mg_per_kg[i] * sim.in_BulkDensity_g_per_cm3[i] / SW_Volumetric;
                            else
                            {
                                dataModel.out_LayerSoluteConc_mg_per_L[i] = 0;

                                MathTools.LogDivideByZeroError("CalculateSolutes", "SW_Volumetric", "out_LayerSoluteConc_mg_per_L[i]");
                            }

                            //push solute into next layer OR calculate leaching (deep drainage) loadings
                            if (i + 1 < sim.in_LayerCount)
                            {
                                double kgs_soil_in_next_layer = sim.in_BulkDensity_g_per_cm3[i + 1] * 1000.0 * (sim.depth[i + 2] - sim.depth[i + 1]) * 10000.0 / 1000.0;
                                if (!MathTools.DoublesAreEqual(kgs_soil_in_next_layer, 0))
                                    dataModel.out_LayerSoluteConc_mg_per_kg[i + 1] += actual_drained_solute_mg / kgs_soil_in_next_layer;
                                else
                                {
                                    dataModel.out_LayerSoluteConc_mg_per_kg[i + 1] = 0;

                                    MathTools.LogDivideByZeroError("CalculateSolutes", "kgs_soil_in_next_layer", "out_LayerSoluteConc_mg_per_kg[i+1]");
                                }
                            }
                            else
                            {

                                /*OUTPUT*/
                                dataModel.out_LeachateSoluteLoad_kg_per_ha = dataModel.out_LayerSoluteConc_mg_per_L[i] / 1000000.0 * sim.out_Soil_Drainage_mm[i + 1] * 10000.0;
                                if (sim.out_Soil_Drainage_mm[i + 1] > 0)
                                    /*OUTPUT*/
                                    dataModel.out_LeachateSoluteConc_mg_per_L = dataModel.out_LayerSoluteConc_mg_per_L[i];      //CHECKTHIS
                                else
                                    dataModel.out_LeachateSoluteConc_mg_per_L = 0;
                            }
                        }
                        else
                        {
                            dataModel.out_LayerSoluteConc_mg_per_kg[i] = 0;
                            dataModel.out_LayerSoluteConc_mg_per_L[i] = 0;
                        }
                    }

                    UpdateSolutesSummaryValues();
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
        public void UpdateSolutesSummaryValues() { }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanSimulateSolutes()
        {
            return true;
        }
    }
}
