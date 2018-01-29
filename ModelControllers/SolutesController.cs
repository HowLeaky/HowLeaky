using HowLeaky.CustomAttributes;
using HowLeaky.Tools.Helpers;
using HowLeaky.DataModels;
using System;
using System.Collections.Generic;
using HowLeaky.OutputModels;
using HowLeaky.Interfaces;

namespace HowLeaky.ModelControllers
{
    public class SolutesOutputDataModel : OutputDataModel, IDailyOutput
    {

    }

    public class SolutesMonthlyOutputDataModel : OutputDataModel
    {
        [Unit("kg_per_ha")]
        public List<double> SoluteLeachingLoad { get; set; }
        [Unit("kg_per_ha")]
        public List<double> SoluteLoadSoil { get; set; }
        public List<double> SoluteCount { get; set; }
    }

    public class SolutesAnnualOutputDataModel : OutputDataModel
    {
        [Unit("mg_per_L")]
        public double SoilWaterSoluteConc { get; set; }         // Soil Water Solute Conc (mg/l).
        [Unit("kg_per_ha")]
        public double SoilSoluteLoad { get; set; }              // Soil Solute Load (kg/ha).
        [Unit("kg_per_ha")]
        public double LeachingSoluteExport { get; set; }        // Leaching Solute Export (kg/ha).
        [Unit("mg_per_L")]
        public double LeachSoluateConc { get; set; }            // Leaching Solute Conc (mg/l).
        //**************************************************************************
    }

    public class SolutesController : HLController
    {
        public SolutesInputModel DataModel { get; set; }
        //public SolutesOutputDataModel Output { get; set; }

        //Reportable Outputs
        [Output("Total Soil Solute(Load)","kg/ha")]
        public double TotalSoilSolute { get; set; }
        [Output("Layer Solute (Load)", "kg/ha")]
        public List<double> LayerSoluteLoad { get; set; }          
        [Output("Layer Solute (Conc)","mg/L")]
        public List<double> LayerSoluteConcmgPerL { get; set; }    
        [Output("Layer Solute (Conc)", "mg/kg")]
        public List<double> LayerSoluteConcmgPerkg { get; set; }   
        [Output("Leachate Solute Concentration", "mg/L")]
        public double LeachateSoluteConcmgPerL { get; set; }       
        [Output("Leachate Solute Load", "kg/ha")]
        public double LeachateSoluteLoadkgPerha { get; set; }      
                                                                                    
        /// <summary>
        /// 
        /// </summary>
        public SolutesController() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sim"></param>
        public SolutesController(Simulation sim, List<InputModel> inputModels) : base(sim)
        {
            DataModel = (SolutesInputModel)inputModels[0];
        }

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
        public override void Simulate()
        {
            try
            {
                if (CanSimulateSolutes())
                {
                    double rain = Sim.ClimateController.Rain;
                    double runoff = Sim.SoilController.Runoff;
                    double irrigationAmountmm = Sim.SoilController.Irrigation;
                    //calculte solute input loadings.
                    double kgsSoilinLayer1 = Sim.SoilController.DataModel.BulkDensity.Values[0] * 1000.0 * Sim.SoilController.Depth[1] * 10000.0 / 1000.0;//per ha
                    if (rain > 0)
                    {
                        if (!MathTools.DoublesAreEqual(kgsSoilinLayer1, 0))
                        {
                            LayerSoluteConcmgPerkg[0] += DataModel.RainfallConcentration * (rain - runoff) * 10000.0 / kgsSoilinLayer1;
                        }
                        else
                        {

                            MathTools.LogDivideByZeroError("CalculateSolutes", "kgs_soil_in_layer_1", "out_LayerSoluteConc_mg_per_kg[0]");
                        }
                    }
                    if (!MathTools.DoublesAreEqual(irrigationAmountmm, 0))
                    {
                        if (!MathTools.DoublesAreEqual(kgsSoilinLayer1, 0))
                        {
                            LayerSoluteConcmgPerkg[0] += DataModel.IrrigationConcentration * irrigationAmountmm * 10000.0 / kgsSoilinLayer1;
                        }
                        else
                        {
                            MathTools.LogDivideByZeroError("CalculateSolutes", "kgs_soil_in_layer_1", "out_LayerSoluteConc_mg_per_kg[0]");
                        }
                    }
                    //initialise total solute count;
                    TotalSoilSolute = 0;
                    //Route solutes down through layer.
                    for (int i = 0; i < Sim.SoilController.LayerCount; ++i)
                    {
                        double SWRelOD = Sim.SoilController.SoilWaterRelWP[i] + Sim.SoilController.WiltingPointRelOD[i];
                        double StartOfDaySWRelOD = SWRelOD + Sim.SoilController.Seepage[i + 1];

                        if (!MathTools.DoublesAreEqual(StartOfDaySWRelOD, 0) && !MathTools.DoublesAreEqual(SWRelOD, 0))
                        {
                            double kgsSoilInLayer = Sim.SoilController.DataModel.BulkDensity.Values[i] * 1000.0 * (Sim.SoilController.DataModel.Depths.Values[i + 1] - Sim.SoilController.DataModel.Depths.Values[i]) * 10000.0 / 1000.0;

                            //calculate the potential drained loadings (doesn't take into account mixing effects)
                            double potentialDrainedSolutemg = 0;
                            if (!MathTools.DoublesAreEqual(StartOfDaySWRelOD, 0))
                                potentialDrainedSolutemg = (LayerSoluteConcmgPerkg[i] * kgsSoilInLayer / StartOfDaySWRelOD) * Sim.SoilController.Seepage[i + 1];
                            else
                            {
                                MathTools.LogDivideByZeroError("CalculateSolutes", "StartOfDay_SW_rel_OD", "potential_drained_solute_mg");
                            }
                            //calculate the actual drained loadings
                            double actualDrainedSolutemg = DataModel.MixingCoefficient * potentialDrainedSolutemg;

                            //take the drained solute load away from the balance in the layer
                            if (!MathTools.DoublesAreEqual(kgsSoilInLayer, 0))
                            {
                                /*OUTPUT*/
                                LayerSoluteConcmgPerkg[i] -= actualDrainedSolutemg / kgsSoilInLayer;
                            }
                            else
                            {
                                LayerSoluteConcmgPerkg[i] = 0;
                                MathTools.LogDivideByZeroError("CalculateSolutes", "kgs_soil_in_layer", "out_LayerSoluteConc_mg_per_kg[i]");
                            }

                            //calculate the solute load in the layer
                            /*OUTPUT*/
                            LayerSoluteLoad[i] = LayerSoluteConcmgPerkg[i] * kgsSoilInLayer / 1000000.0;

                            //keep track of total load
                            /*OUTPUT*/
                            TotalSoilSolute += LayerSoluteLoad[i];

                            //calculate solute concentration in layer
                            double SWVolumetric;
                            if (!MathTools.DoublesAreEqual(Sim.SoilController.Depth[i + 1], 0))
                                SWVolumetric = SWRelOD / (double)(Sim.SoilController.Depth[i + 1]);
                            else
                            {
                                SWVolumetric = 0;

                                MathTools.LogDivideByZeroError("CalculateSolutes", "depth[i+1]", "SW_Volumetric");
                            }
                            if (!MathTools.DoublesAreEqual(SWVolumetric, 0))
                                /*OUTPUT*/
                                LayerSoluteConcmgPerL[i] = LayerSoluteConcmgPerkg[i] * Sim.SoilController.DataModel.BulkDensity.Values[i] / SWVolumetric;
                            else
                            {
                                LayerSoluteConcmgPerL[i] = 0;

                                MathTools.LogDivideByZeroError("CalculateSolutes", "SW_Volumetric", "out_LayerSoluteConc_mg_per_L[i]");
                            }

                            //push solute into next layer OR calculate leaching (deep drainage) loadings
                            if (i + 1 < Sim.SoilController.LayerCount)
                            {
                                double kgsSoilInNextLayer = Sim.SoilController.DataModel.BulkDensity.Values[i + 1] * 1000.0 * (Sim.SoilController.Depth[i + 2] - Sim.SoilController.Depth[i + 1]) * 10000.0 / 1000.0;
                                if (!MathTools.DoublesAreEqual(kgsSoilInNextLayer, 0))
                                {
                                    LayerSoluteConcmgPerkg[i + 1] += actualDrainedSolutemg / kgsSoilInNextLayer;
                                }
                                else
                                {
                                    LayerSoluteConcmgPerkg[i + 1] = 0;
                                    MathTools.LogDivideByZeroError("CalculateSolutes", "kgs_soil_in_next_layer", "out_LayerSoluteConc_mg_per_kg[i+1]");
                                }
                            }
                            else
                            {

                                /*OUTPUT*/
                                LeachateSoluteLoadkgPerha = LayerSoluteConcmgPerL[i] / 1000000.0 * Sim.SoilController.Drainage[i + 1] * 10000.0;
                                if (Sim.SoilController.Drainage[i + 1] > 0)
                                /*OUTPUT*/
                                {
                                    LeachateSoluteConcmgPerL = LayerSoluteConcmgPerL[i];      //CHECKTHIS
                                }
                                else
                                {
                                    LeachateSoluteConcmgPerL = 0;
                                }
                            }
                        }
                        else
                        {
                            LayerSoluteConcmgPerkg[i] = 0;
                            LayerSoluteConcmgPerL[i] = 0;
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
        public void UpdateSolutesSummaryValues()
        {
        }

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
