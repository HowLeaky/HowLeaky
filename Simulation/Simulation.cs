
using HowLeaky.ModelControllers;
using HowLeaky.SyncModels;
using HowLeaky.Tools;
using System;
using System.Collections.Generic;
using System.Xml;

namespace HowLeaky.Models
{
    public enum ManagementEvent { mePlanting, meHarvest, meTillage, mePesticide, meIrrigation, meCropGrowing, meInPlantingWindow, meMeetsSoilWaterPlantCritera, meMeetsDaysSinceHarvestPlantCritera, meMeetsRainfallPlantCritera, meNone };

    public class Simulation : CustomSyncModel
    {

        //public static int MISSING_DATA_VALUE = -32768;
        public static int ROBINSON_CN = 0;
        public static int PERFECT_CN = 1;
        public static int DEFAULT_CN = 2;

        public bool RunSilent { get; set; }
        public bool canlog { get; set; }
        public bool Use2008CurveNoFn { get; set; }
        public bool Force2011CurveNoFn { get; set; }


        //--------------------------------------------------------------------------
        // Submodel Controller
        //--------------------------------------------------------------------------
        public IrrigationController IrrigationController { get; set; }
        public VegetationController VegetationController { get; set; }
        public TillageController TillageController { get; set; }
        public PesticideController PesticideController { get; set; }
        public PhosphorusController PhosphorusController { get; set; }
        public NitrateController NitrateController { get; set; }
        public SolutesController SolutesController { get; set; }
        public ModelOptionsController ModelOptionsController { get; set; }
        public ClimateController ClimateController { get; set; }

        //public int LayerCount { get {return  }
        public double Latitude { get { return ClimateController.Latitude; } }
        public double Longitude { get { return ClimateController.Longitude; } }

        //--------------------------------------------------------------------------
        // Input Parameters
        //--------------------------------------------------------------------------
        public int in_LayerCount { get; set; }                          //  Number of soil layers used to define soil thicknesses ("Layer Depth (Cumulative)") and the soil hydraulic properties defined by "Air dry moisture", "Wilting point", "Field capacity", "Saturated water content" and "Maximum drainage from layer").
        public List<double> in_Depths { get; set; }                              //  Depth to the bottom of each soil layer defined by "Number of Horizons".  The properties of each layer are defined by "Air dry moisture", "Wilting point", "Field capacity", "Saturated water content" and "Maximum drainage from layer".
        public List<double> in_SoilLimitAirDry_pc { get; set; }                  //  This is the moisture content when the soil is air-dry (40o C).  It is usually much less than the lower limit of plant-available moisture.  A value is needed for each soil layer defined by "Number of Horizons" and "Layer Depth (Cumulative)".  However, values in deeper soil layers have no effect because evaporation only occurs in the top 10 to 30 cm of soil.
        public List<double> in_SoilLimitWiltingPoint_pc { get; set; }            //  Wilting point is the lower limit of soil moisture content for plant water use (the moisture content at which plants permanently wilted).  A value is needed for each soil layer defined by "Number of Horizons" and "Layer Depth (Cumulative)".
        public List<double> in_SoilLimitFieldCapacity_pc { get; set; }           //  Field capacity (or drained upper limit) is the water content in the soil after free water drains.  A value is needed for each soil layer defined by "Number of Horizons" and "Layer Depth (Cumulative)
        public List<double> in_SoilLimitSaturation_pc { get; set; }              //  Saturated water content (SAT) is the soil moisture content of the soil layer when saturated.  It is equal to total porosity (which can be calculated from bulk density) except where a small amount of air is entrapped in the soil (eg 0.0 � 0.05 v/v).  A value is needed for each soil layer defined by "Number of Horizons" and "Layer Depth (Cumulative)".
        public List<double> in_LayerDrainageLimit_mm_per_day { get; set; }       //  Controls the maximum rate of drainage downwards from each soil layer ("Layer Depth (Cumulative)") when it is saturated and the deep drainage below the deepest soil layer.  Drainage is also influenced by drainable porosity ("Saturated water content" minus "Field capacity").
        public List<double> in_BulkDensity_g_per_cm3 { get; set; }               //
        public double in_Cona_mm_per_sqrroot_day { get; set; }          //
        public double in_Stage1SoilEvapLimitU_mm { get; set; }          //
        public double in_RunoffCurveNumber { get; set; }                    //  The runoff Curve Number (CN) partitions rainfall into runoff and infiltration, using a modification of the USDA method that relates CN to soil moisture content each day (Williams and La Seur 1976, Williams et al. 1985), rather than to antecedent rainfall.  In PERFECT and HOWLEAKY, this is modified further to adjust CN for cover and for soil surface roughness caused by tillage (optional). The input parameter is the CN for bare soil at average antecedent moisture content (CN2bare).
        public double in_CurveNumberReduction { get; set; }                 //  Reduction in runoff Curve Number (CN2) below CN2bare (�Runoff curve number (bare soil)�) at 100% cover.  Used to calculate the effect of cover on runoff.
        public double in_MaxRedInCNDueToTill { get; set; }              //  Reduction in runoff Curve Number (CN2bare) when a tillage operation occurs (optional).  Used to model effects of soil surface roughness, cause by tillage, on runoff (if selected) in conjunction with �Rainfall to 0 roughness�, based on Littleboy et al. (1996a).
        public double in_RainToRemoveRoughness_mm { get; set; }             //
        public double in_USLE_K { get; set; }                               //  USLE K factor is the soil erodibility factor (K) of the Universal Soil Loss Equation (USLE, Renard et al 1993).  It defines the inherent susceptibility of a soil to erosion per unit of rainfall erosivity, and is defined for set cover and crop condition (bare soil, permanent fallow, C = 1), slope and length of slope (LS factor = 1) and practice factor (P=1).
        public double in_USLE_P { get; set; }                               //  USLE P factor is the practice factor (P) of the Universal Soil Loss Equation (USLE, Renard et al 1993).  It defines effects of conservation practices other than those related to cover and cropping/soil water use practices.  A value of 1.0 indicates no such practices and is considered the norm.
        public double in_FieldSlope_pc { get; set; }                    //
        public double in_SlopeLength_m { get; set; }                        //  Slope length is the distance down the slope, used to calculate the USLE slope-length factor (LS) using the algorithm from the Revised USLE (Renard et al. 1993).  It is converted from metres to feet in order to apply the RUSLE equations [Need this??].  It has no effect on other processes.
        public double in_RillRatio { get; set; }                        //
        public bool in_SoilCrackingSwitch { get; set; }                 //  A value of YES turns on the option for some rainfall (defined by \"Max crack infilt.\") to infiltrate below soil layer 2 directly via cracks.  Infiltration via crack will only occur when daily rainfall is greater than 10 mm and soil moisture content in the upper two soil layers is less than 30% of field capacity.  Cracks extend down through all layers where soil moisture is less than 30% of field capacity.  Infiltration occurs into the lowest �cracked� layer first and any layer can only fill to 50% of field capacity.  This option is affected by the number and thickness of layers used.
        public double in_MaxInfiltIntoCracks_mm { get; set; }           //
        public double in_SedDelivRatio { get; set; }                    //

        //--------------------------------------------------------------------------
        // Timeseries Inputs
        //--------------------------------------------------------------------------
        public List<double> MaxTemp { get; set; }                              //
        public List<double> MinTemp { get; set; }                              //
        public List<double> Rainfall { get; set; }                             //
        public List<double> Evaporation { get; set; }                          //
        public List<double> SolarRadiation { get; set; }                       //

        //--------------------------------------------------------------------------
        // Daily Outputs
        //--------------------------------------------------------------------------

        public double out_Rain_mm { get; set; }                             // Daily rainfall amount (mm) as read directly from the P51 file.
        public double out_MaxTemp_oC { get; set; }                          // Daily max temperature (oC) as read directly from the P51 file.
        public double out_MinTemp_oC { get; set; }                          // Daily min temperature (oC) as read directly from the P51 file.
        public double out_PanEvap_mm { get; set; }                          // Daily pan evaporation (mm) as read directly from the P51 file.
        public double out_SolarRad_MJ_per_m2_per_day { get; set; }          // Daily solar radition (mMJ/m^2/day) as read directly from the P51 file.
                                                                            //Water balance outputs
        public double out_WatBal_Irrigation_mm { get; set; }                // Irrigation amount (mm) as calcaulted in irrigation module.
        public double out_WatBal_Runoff_mm { get; set; }                    // Total Runoff amount (mm) - includes runoff from rainfall AND irrigation.
        public double out_WatBal_RunoffFromIrrigation_mm { get; set; }      // Runoff amount from irrigation (mm).
        public double out_WatBal_RunoffFromRainfall_mm { get; set; }        // Runoff amount from rainfall (mm).
        public double out_WatBal_SoilEvap_mm { get; set; }                  // Soil evaporation (mm).
        public double out_WatBal_PotSoilEvap_mm { get; set; }               // Potential soil evaporation (mm).
        public double out_WatBal_Transpiration_mm { get; set; }             // Transpiration (mm) calculated from current crop.
        public double out_WatBal_EvapoTransp_mm { get; set; }               // Evapo-transpiration (mm) is equal to the transpiration PLUS soil evaporation.
        public double out_WatBal_DeepDrainage_mm { get; set; }              // Deep drainage (mm) which is the amount of drainge out of the bottom layer.
        public double out_WatBal_Overflow_mm { get; set; }                  // Overflow (mm) ADD DEF HERE
        public double out_WatBal_LateralFlow_mm { get; set; }               // Lateral flow (mm) ADD DEF HERE
        public double out_WatBal_VBE_mm { get; set; }                       // Volume Balance Error
        public double out_WatBal_RunoffCurveNo { get; set; }                // Runoff curve number
        public double out_WatBal_RunoffRetentionNo { get; set; }            // Runoff retention number
                                                                            //Soil outputs
        public double out_Soil_HillSlopeErosion_t_per_ha { get; set; }      // Hillslope errorsion (t/ha)
        public double out_Soil_OffSiteSedDelivery_t_per_ha { get; set; }    // Offsite sediment deliver (t/ha)
        public double out_Soil_TotalSoilWater_mm { get; set; }              // Total soil water (mm) is the sum of soil water in all layers
        public double out_Soil_SoilWaterDeficit_mm { get; set; }            // Soil water deficit (mm)
        public double out_Soil_Layer1SatIndex { get; set; }                 // Layer 1 saturation index
        public double out_Soil_TotalCropResidue_kg_per_ha { get; set; }     // Total crop residue (kg/ha) - sum of all crops present
        public double out_Soil_TotalResidueCover_pc { get; set; }           // Total residue cover (%)  - based on all crops present
        public double out_Soil_TotalCover_pc { get; set; }                  // Total cover (%) - based on all crops present{get;set;}
        public List<double> out_Soil_SoilWater_mm { get; set; }                  // Soil water in each layer (mm)
        public List<double> out_Soil_Drainage_mm { get; set; }                   // Drainage in each layer

        //--------------------------------------------------------------------------
        // Monthly Outputs
        //--------------------------------------------------------------------------
        public List<double> mo_MthlyAvgRainfall_mm { get; set; }                 //
        public List<double> mo_MthlyAvgEvaporation_mm { get; set; }              //
        public List<double> mo_MthlyAvgTranspiration_mm { get; set; }            //
        public List<double> mo_MthlyAvgRunoff_mm { get; set; }                   //
        public List<double> mo_MthlyAvgDrainage_mm { get; set; }                 //
                                                                                 //--------------------------------------------------------------------------
                                                                                 // Summary Outputs
                                                                                 //--------------------------------------------------------------------------
        public double so_YrlyAvgRainfall_mm_per_yr { get; set; }           //
        public double so_YrlyAvgIrrigation_mm_per_yr { get; set; }         //
        public double so_YrlyAvgRunoff_mm_per_yr { get; set; }             //
        public double so_YrlyAvgSoilEvaporation_mm_per_yr { get; set; }    //
        public double so_YrlyAvgTranspiration_mm_per_yr { get; set; }      //
        public double so_YrlyAvgEvapotransp_mm_per_yr { get; set; }         //
        public double so_YrlyAvgOverflow_mm_per_yr { get; set; }           //
        public double so_YrlyAvgDrainage_mm_per_yr { get; set; }           //
        public double so_YrlyAvgLateralFlow_mm_per_yr { get; set; }        //
        public double so_YrlyAvgSoilErosion_T_per_ha_per_yr { get; set; }  //
        public double so_YrlyAvgOffsiteSedDel_T_per_ha_per_yr { get; set; }//
        public double so_TotalCropsPlanted { get; set; }                   //
        public double so_TotalCropsHarvested { get; set; }                 //
        public double so_TotalCropsKilled { get; set; }                    //
        public double so_AvgYieldPerHrvst_t_per_ha_per_hrvst { get; set; } //
        public double so_AvgYieldPerPlant_t_per_ha_per_plant { get; set; } //
        public double so_AvgYieldPerYr_t_per_ha_per_yr { get; set; }       //
        public double so_YrlyAvgCropRainfall_mm_per_yr { get; set; }       //
        public double so_YrlyAvgCropIrrigation_mm_per_yr { get; set; }     //
        public double so_YrlyAvgCropRunoff_mm_per_yr { get; set; }         //
        public double so_YrlyAvgCropSoilEvap_mm_per_yr { get; set; }        //
        public double so_YrlyAvgCropTransp_mm_per_yr { get; set; }          //
        public double so_YrlyAvgCropEvapotransp_mm_per_yr { get; set; }     //
        public double so_YrlyAvgCropOverflow_mm_per_yr { get; set; }       //
        public double so_YrlyAvgCropDrainage_mm_per_yr { get; set; }       //
        public double so_YrlyAvgCropLateralFlow_mm_per_yr { get; set; }    //
        public double so_YrlyAvgCropSoilErosion_T_per_ha_per_yr { get; set; }//
        public double so_YrlyAvgCropOffsiteSedDel_T_per_ha_per_yr { get; set; }//
        public double so_YrlyAvgFallowRainfall_mm_per_yr { get; set; }     //
        public double so_YrlyAvgFallowIrrigation_mm_per_yr { get; set; }   //
        public double so_YrlyAvgFallowRunoff_mm_per_yr { get; set; }       //
        public double so_YrlyAvgFallowSoilEvap_mm_per_yr { get; set; }      //
        public double so_YrlyAvgFallowTransp_mm_per_yr { get; set; }        //
        public double so_YrlyAvgFallowEvapotransp_mm_per_yr { get; set; }  //
        public double so_YrlyAvgFallowOverflow_mm_per_yr { get; set; }     //
        public double so_YrlyAvgFallowDrainage_mm_per_yr { get; set; }     //
        public double so_YrlyAvgFallowLateralFlow_mm_per_yr { get; set; }  //
        public double so_YrlyAvgFallowSoilErosion_T_per_ha_per_yr { get; set; }//
        public double so_YrlyAvgFallowOffsiteSedDel_T_per_ha_per_yr { get; set; }//
        public double so_YrlyAvgPotEvap_mm { get; set; }                   //

        public double so_YrlyAvgRunoffAsPercentOfInflow_pc { get; set; }   //
        public double so_YrlyAvgEvapAsPercentOfInflow_pc { get; set; }     //
        public double so_YrlyAvgTranspAsPercentOfInflow_pc { get; set; }   //
        public double so_YrlyAvgDrainageAsPercentOfInflow_pc { get; set; } //
        public double so_YrlyAvgPotEvapAsPercentOfInflow_pc { get; set; }  //
        public double so_YrlyAvgCropSedDel_t_per_ha_per_yr { get; set; }   //
        public double so_YrlyAvgFallowSedDel_t_per_ha_per_yr { get; set; } //
        public double so_RobinsonErrosionIndex { get; set; }                //
        public double so_YrlyAvgCover_pc { get; set; }                     //
        public double so_YrlyAvgFallowDaysWithMore50pcCov_days { get; set; }//
        public double so_AvgCoverBeforePlanting_pc { get; set; }           //
        public double so_SedimentEMCBeoreDR { get; set; }                  //
        public double so_SedimentEMCAfterDR { get; set; }                  //
        public double so_AvgSedConcInRunoff { get; set; }                   //


        //--------------------------------------------------------------------------
        // intermediate variables
        //--------------------------------------------------------------------------
        bool FReset { get; set; }
        public bool NeedToUpdateOutput { get; set; }
        public bool InRunoff { get; set; }
        public bool InRunoff2 { get; set; }
        public DateTime startdate { get; set; }
        public DateTime today { get; set; }
        public int seriesindex { get; set; }
        public int climateindex { get; set; }
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public int number_of_days_in_simulation { get; set; }
        public int RunoffEventCount2 { get; set; }
        public double previous_total_soil_water { get; set; }
        public double yesterdays_rain { get; set; }
        public double temperature { get; set; }
        public double total_cover { get; set; }
        public double total_cover_percent { get; set; }
        public double total_crop_residue { get; set; }
        public double total_residue_cover { get; set; }
        public double total_residue_cover_percent { get; set; }
        public double effective_rain { get; set; }
        public double total_soil_water { get; set; }
        public double crop_cover { get; set; }
        public double runoff { get; set; }
        public double sediment_conc { get; set; }
        public double erosion_t_per_ha { get; set; }
        public double offsite_sed_delivery { get; set; }
        public double cumSedConc { get; set; }
        public double peakSedConc { get; set; }
        public double swd { get; set; }
        public double satd { get; set; }
        public double sse1 { get; set; }
        public double sse2 { get; set; }
        public double se1 { get; set; }
        public double se2 { get; set; }
        public double se21 { get; set; }
        public double se22 { get; set; }
        public double dsr { get; set; }
        public double sed_catchmod { get; set; }
        public double saturationindex { get; set; }
        public double cn2 { get; set; }
        public double overflow_mm { get; set; }
        public double rain_since_tillage { get; set; }
        public double infiltration { get; set; }
        public double lateral_flow { get; set; }
        public double potential_soil_evaporation { get; set; }
        public double drainage { get; set; }
        public double runoff_retention_number { get; set; }
        public double usle_ls_factor { get; set; }
        public double PredRh { get; set; }
        public double accumulated_cover { get; set; }
        public double sum_rainfall { get; set; }
        public double sum_irrigation { get; set; }
        public double sum_runoff { get; set; }
        public double sum_potevap { get; set; }
        public double sum_soilevaporation { get; set; }
        public double sum_transpiration { get; set; }
        public double sum_evapotranspiration { get; set; }
        public double sum_overflow { get; set; }
        public double sum_drainage { get; set; }
        public double sum_lateralflow { get; set; }
        public double sum_soilerosion { get; set; }
        public double sum_crop_rainfall { get; set; }
        public double sum_crop_irrigation { get; set; }
        public double sum_crop_runoff { get; set; }
        public double sum_crop_soilevaporation { get; set; }
        public double sum_crop_transpiration { get; set; }
        public double sum_crop_evapotranspiration { get; set; }
        public double sum_crop_overflow { get; set; }
        public double sum_crop_drainage { get; set; }
        public double sum_crop_lateralflow { get; set; }
        public double sum_crop_soilerosion { get; set; }
        public double sum_fallow_rainfall { get; set; }
        public double sum_fallow_irrigation { get; set; }
        public double sum_fallow_overflow { get; set; }
        public double sum_fallow_lateralflow { get; set; }
        public double sum_fallow_runoff { get; set; }
        public double sum_fallow_soilevaporation { get; set; }
        public double sum_fallow_drainage { get; set; }
        public double sum_fallow_soilerosion { get; set; }
        public double fallow_efficiency { get; set; }
        public double sum_fallow_soilwater { get; set; }
        public double accumulate_cov_day_before_planting { get; set; }
        public double fallow_days_with_more_50pc_cov { get; set; }
        public double total_number_plantings { get; set; }
        public double accumulated_crop_sed_deliv { get; set; }
        public double accumulated_fallow_sed_deliv { get; set; }
        public List<double> mcfc { get; set; }
        public List<double> SoilWater_rel_wp { get; set; }
        public List<double> DrainUpperLimit_rel_wp { get; set; }
        public List<double> depth { get; set; }
        public List<double> layer_transpiration { get; set; }
        public List<double> red { get; set; }
        public List<double> wf { get; set; }
        public List<double> SaturationLimit_rel_wp { get; set; }
        public List<double> Wilting_Point_RelOD_mm { get; set; }
        public List<double> DUL_RelOD_mm { get; set; }
        public List<double> AirDryLimit_rel_wp { get; set; }
        public List<double> ksat { get; set; }
        public List<double> swcon { get; set; }
        public List<double> Seepage { get; set; }
        public List<double> MaxDrainage { get; set; }

        public string ControlError { get; set; }

        public ManagementEvent FManagementEvent { get; set; }
        //ProgramManager FProgramManager;
        public List<string> ErrorList { get; set; } = new List<string>();
        public List<string> ZerosList { get; set; } = new List<string>();


        //--------------------------------------------------------------------------
        //Progress housekeeping
        //--------------------------------------------------------------------------
        public int Progress { get; set; }
        public int FProgress { get; set; }
        public int FSimProgress { get; set; }
        public int FLastProgress { get; set; }
        public int FLastSimProgress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Simulation()
        {
            // FProgramManager = manager;

            ErrorList = new List<string>();
            ZerosList = new List<string>();

            canlog = false;
            //FOnUpdateOutput = 0;
            //FOnFinishSimulating = 0;
            //CurrentSimulationObject = 0;
            //FSimulationObjectsList = 0;

            RunSilent = false;

            Force2011CurveNoFn = false;
            //	InputDefinitions=0;//manager.PerfectInputModule();

            IrrigationController = new IrrigationController(this);
            VegetationController = new VegetationController(this);
            TillageController = new TillageController(this);
            PesticideController = new PesticideController(this);
            PhosphorusController = new PhosphorusController(this);
            NitrateController = new NitrateController(this);
            SolutesController = new SolutesController(this);
            ModelOptionsController = new ModelOptionsController(this);

        }
        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            //	if(Suspended)
            //		Resume();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            FReset = true;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Execute()
        {
            try
            {

            }
            catch (Exception e)
            {

                throw (new Exception("An error has occurred in the execution thread of the simulation.", new Exception(e.Message)));
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Simulate()
        {
            //	try
            //	{
            //		canlog=false;
            //
            //
            //		if(!RunSilent)Synchronize(UpdateStartMessages);
            //		FLastProgress=-1;
            //		int simcount=SimulationObjectsList.Count;
            //		SmartPointer<TList>ActiveSimList(new TList);
            //		for(int i=0;i<simcount;++i)
            //		{
            //			TScenarioInputObject*sim=(TScenarioInputObject*)SimulationObjectsList.Items[i];
            //			if(sim.NeedsToBeSimulated)
            //				ActiveSimList.Add(sim);
            //		}
            //		int activesimcount=ActiveSimList.Count;
            //		if(!FReset&&activesimcount>0)
            //		{
            //			for(int i=0;i<activesimcount;++i)
            //			{
            //				if(LoadSimulationObject((TScenarioInputObject*)ActiveSimList.Items[i]))
            //				{
            //					if(!FReset)Synchronize(UpdateOutput);  // trial this  on
            //					if(CurrentSimulationObject.NeedsToBeSimulated)
            //						if(!FReset)RunCurrentSimulation(i,activesimcount);
            //					if(!FReset)CurrentSimulationObject=0;
            //				}
            //			}
            //			if(!FReset)FProgress=100.0;
            //			if(!FReset&&!RunSilent)Synchronize(UpdateOutput); // trial this on
            //			if(!FReset&&!RunSilent)Synchronize(PostProgressMessage);
            //			if(!FReset&&!RunSilent)Synchronize(UpdateInputParametersForm);
            //			if(!FReset)Synchronize(RunOnFinishSimumulatingEvent);
            //		}
            //		else
            //		{
            //			if(!FReset)Synchronize(UpdateOutput);
            //		}
            //
            //	}
            //	catch(Exception e)
            //	{
            //		  //throw(new Exception("An error has occurred during simulation iterations.", mtError, TMsgDlgButtons() << mbOK, 0);
            //		  return false;
            //	}
            //	Synchronize(UpdateFinishMessages);
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="simulationindex"></param>
        /// <param name="simcount"></param>
        /// <returns></returns>
        public bool RunCurrentSimulation(int simulationindex, int simcount)
        {
            //	try
            //	{
            //		if(!FReset&&CurrentSimulationObject)
            //		{
            //			FLastSimProgress=-1;
            //			ZerosList.Clear();
            //			if(!RunSilent)
            //			{
            //
            //				if(!FReset)InitialiseSimulationParameters();
            //				if(!FReset)UpdateProgress(seriesindex,simulationindex,simcount,number_of_days_in_simulation,true);
            //			}
            //			else
            //			{
            //
            //				if(!FReset)InitialiseSimulationParameters();
            //			}
            //			for(int index=0;index<number_of_days_in_simulation;++index)
            //			{
            //				today=IncDay(startdate,index);
            //				if(!FReset&&SimulateDay())
            //				{
            //					if(!FReset&&!RunSilent)UpdateProgress(seriesindex+1,simulationindex,simcount,number_of_days_in_simulation,false);
            //					++seriesindex;
            //					++climateindex;
            //
            //				}
            //				else
            //				{
            //					index=number_of_days_in_simulation;
            //					if(!FReset)
            //					{
            //						throw;
            //					}
            //				}
            //			}
            //		//	NeedToUpdateOutput=true;
            //			if(!RunSilent)
            //			{
            //				if(!FReset)Synchronize(CalculateSummaryParameters);
            //				if(!FReset)Synchronize(EndUpdate);
            //				Synchronize(EndOutputObjectUpdate);
            //				//if(!FReset)Synchronize(UpdateOutput);   // trial this off
            //				UpdateProgress(1,simulationindex,simcount,1,true);
            //				if(FProgramManager.VerificationMode)
            //                	CurrentSimulationObject.SaveVerificationOutput();
            //			}
            //			else
            //			{
            //				CalculateSummaryParameters();
            //				EndUpdate();
            //				EndOutputObjectUpdate();
            //
            //			}
            //
            //
            //		}
            //		else
            //		{
            //
            //		  //	NeedToUpdateOutput=true;
            //			UpdateProgress(1,simulationindex,simcount,1,true);
            //		}
            //	}
            //	catch(Exception e)
            //	{
            //		ErrorList.Add("A serious error occurred while running the simulation");
            //		if(ZerosList.Count>0)
            //		{
            //			for(int i=0;i<ZerosList.Count;++i)
            //				ErrorList.Add(ZerosList.Strings[i]);
            //		}
            //		throw;
            //	}
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool SimulateDay()
        {
            bool result = true;
            try
            {
                ControlError = "";
                if (!FReset) InitialiseClimateData();
                if (!FReset) AdjustKeyDatesForYear();
                if (!FReset) SetStartOfDayParameters();
                if (!FReset) ApplyResetsIfAny();
                if (!FReset) TryModelIrrigation();
                if (!FReset) TryModelSoilCracking();
                if (!FReset) CalculateRunoff();
                if (!FReset) CalculatSoilEvaporation();
                if (!FReset) TryModelVegetation();
                if (!FReset) UpdateWaterBalance();
                if (!FReset) TryModelTillage();
                if (!FReset) CalculateResidue();
                if (!FReset) CalculateErosion();
                if (!FReset) TryModelRingTank();
                if (!FReset) TryModelPesticide();
                if (!FReset) TryModelPhosphorus();
                if (!FReset) TryModelNitrate();
                if (!FReset) TryModelSolutes();
                if (!FReset) TryModelLateralFlow();
                if (!FReset) UpdateCropWaterBalance();
                if (!FReset) UpdateFallowWaterBalance();
                if (!FReset) UpdateTotalWaterBalance();
                if (!FReset) TryUpdateRingTankWaterBalance();
                if (!FReset) UpdateMonthlyStatistics();
                if (!FReset) CalculateVolumeBalanceError();
                if (!FReset) ExportDailyOutputs();
                if (!FReset) ResetAnyParametersIfRequired();
            }
            catch (Exception e)
            {
                result = false;

                List<string> Text = new List<string>();
                if (today > new DateTime(1800, 1, 1) && today < new DateTime(2100, 1, 1))
                    Text.Add("There was an error in the simulation on day " + (seriesindex + 1).ToString() + " (" + today.ToString("dd/mm/yyyy") + ")");
                if (ControlError.Length > 0)
                    Text.Add("The error occurred in the function called " + ControlError);
                if (Text.Count > 0 && Text.Count < 3)
                    throw (new Exception(String.Join("\n", Text.ToArray(), e.Message))); //mtError
                else
                    throw (new Exception("Error Simulating Day", new Exception(e.Message)));

            }
            return result;

        }
        /// <summary>
        /// 
        /// </summary>
        public void InitialiseClimateData()
        {
            try
            {
                if (climateindex >= 0 && climateindex < Rainfall.Count)
                {
                    out_Rain_mm = (Rainfall)[climateindex];
                    if (climateindex > 0)
                        yesterdays_rain = (Rainfall)[climateindex - 1];
                    else
                        yesterdays_rain = 0;
                    out_MaxTemp_oC = (MaxTemp)[climateindex];
                    out_MinTemp_oC = (MinTemp)[climateindex];
                    temperature = ((MaxTemp)[climateindex] + (MinTemp)[climateindex]) / 2.0;
                    out_PanEvap_mm = (Evaporation)[climateindex];
                    out_SolarRad_MJ_per_m2_per_day = (SolarRadiation)[climateindex];
                }
                else

                    throw (new Exception("The climate time-series data has been accessed incorrectly during the simulation. Array bounds were exceeded.\nPlease let David McClymont know of this problem.\n\nmcclymon@mac.com")); //mtError
            }
            catch (Exception e)
            {
                ControlError = "InitialiseClimateData";
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void AdjustKeyDatesForYear()
        {
            try
            {
                day = today.Day;
                month = today.Month;
                year = today.Year;
            }
            catch (Exception e)
            {
                ControlError = "AdjustKeyDatesForYear";
                throw new Exception(e.Message);
            }

        }
        /// <summary>
        /// /
        /// </summary>
        public void SetStartOfDayParameters()
        {
            try
            {
                effective_rain = out_Rain_mm;
                swd = 0;
                satd = 0;
                for (int i = 0; i < in_LayerCount; ++i)
                {
                    satd = satd + (SaturationLimit_rel_wp[i] - SoilWater_rel_wp[i]);
                    swd = swd + (DrainUpperLimit_rel_wp[i] - SoilWater_rel_wp[i]);
                }
                IrrigationController.SetStartOfDayParameters();
                VegetationController.SetStartOfDayParameters();
                TillageController.SetStartOfDayParameters();
            }
            catch (Exception e)
            {
                ControlError = "SetStartOfDayParameters";
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void ApplyResetsIfAny()
        {
            ModelOptionsController.ApplyResetsIfAny(today);
        }
        /// <summary>
        /// 
        /// </summary>
        public void TryModelIrrigation()
        {
            IrrigationController.Simulate();
            out_WatBal_Irrigation_mm = IrrigationController.out_IrrigationApplied;
        }
        /// <summary>
        /// 
        /// </summary>
        public void TryModelSoilCracking()
        {
            try
            {
                if (in_SoilCrackingSwitch)
                {
                    //************************************************************************
                    //*                                                                      *
                    //*  This function allows for water to directly enter lower layers     *
                    //*  of the soil profile through cracks. For cracks to occur the top     *
                    //*  and second profile layers must be less than 30% and 50%             *
                    //*  respectively of field capacity. Cracks can extend down the          *
                    //*  profile using similar criteria. This subroutine assumes all         *
                    //*  cracks must exist at the surface. Water is placed into the          *
                    //*  lowest accessable layer first.                                      *
                    //*                                                                      *
                    //************************************************************************
                    int nod;
                    //  Initialise total water redistributed through cracks
                    double tred = 0;
                    for (int i = 0; i < in_LayerCount; ++i)
                    {
                        red[i] = 0;
                        if (!MathTools.DoublesAreEqual(DrainUpperLimit_rel_wp[i], 0))
                            mcfc[i] = SoilWater_rel_wp[i] / DrainUpperLimit_rel_wp[i];
                        else
                        {
                            mcfc[i] = 0;

                            LogDivideByZeroError("ModelSoilCracking", "DrainUpperLimit_rel_wp[i]", "mcfc[i]");
                        }
                        if (mcfc[i] < 0) mcfc[i] = 0;
                        else if (mcfc[i] > 1) mcfc[i] = 1;
                    }

                    //  Don't continue if rainfall is less than 10mm
                    if (effective_rain < 10)
                        return;

                    //  Check if profile is dry enough for cracking to occur.
                    if (mcfc[0] >= 0.3 || mcfc[1] >= 0.3)
                        return;

                    //  Calculate number of depths to which cracks extend
                    nod = 1;
                    for (int i = 1; i < in_LayerCount; ++i)
                    {
                        if (mcfc[i] >= 0.3)
                            i = in_LayerCount;
                        else
                            ++nod;
                    }
                    //  Fill cracks from lowest cracked layer first to a maximum of 50% of
                    //  field capacity.
                    tred = Math.Min(in_MaxInfiltIntoCracks_mm, effective_rain);
                    for (int i = nod - 1; i >= 0; --i)
                    {
                        red[i] = Math.Min(tred, DrainUpperLimit_rel_wp[i] / 2.0 - SoilWater_rel_wp[i]);
                        tred -= red[i];
                        if (tred <= 0)
                            i = -1;
                    }

                    //  calculate effective rainfall after infiltration into cracks.
                    //  Note that redistribution of water into layer 1 is ignored.
                    effective_rain = effective_rain + red[0] - Math.Min(in_MaxInfiltIntoCracks_mm, effective_rain);
                    red[0] = 0.0;

                    //  calculate total amount of water in cracks
                    for (int i = 0; i < in_LayerCount; ++i)
                        tred += red[i];
                }
            }
            catch (Exception e)
            {
                ControlError = "ModelSoilCracking";
                throw new Exception(e.Message);
            }
        }

        public void CalculateRunoff()
        {
            int progress = 0;
            try
            {
                //  *********************************************************************
                //  *  This subroutine calculates surface runoff using a modified form  *
                //  *  of USDA Curve numbers from CREAMS.  The input value of Curve     *
                //  *  Number for AMC II is adjusted to account for the effects of crop *
                //  *  and residue cover.  The magnitude of the reduction in CNII due   *
                //  *  to cover is governed by the user defined CNRED parameter.        *
                //  *                                                                   *
                //  *  Knisel, W.G. editor. CREAMS: A field-scale model for chemical,   *
                //  *  runoff and erosion from agricultural management systems.         *
                //  *  United States Department of Agriculture, Conservation Research   *
                //  *  Report no. 26.                                                   *
                //  *********************************************************************
                double sumh20;
                infiltration = 0.0;
                out_WatBal_Runoff_mm = 0.0;
                runoff_retention_number = 0;
                double cn1, smx;

                //  ***************************************************
                //  *  Calculate cover effect on curve number (cn2).  *
                //  ***************************************************}
                crop_cover = VegetationController.GetCropCoverIfLAIModel(crop_cover);  //LAI Model uses cover from the end of the previous day whereas Cover model predefines at the start of the day
                cn2 = in_RunoffCurveNumber - in_CurveNumberReduction * Math.Min(1.0, crop_cover + total_residue_cover * (1 - crop_cover));
                progress = 1;

                //this could need attention!!!! Danny Rattray
                //  *******************************************************
                //  *  Calculate roughness effect on curve number (cn2).  *
                //  *******************************************************

                rain_since_tillage += effective_rain;
                if (!MathTools.DoublesAreEqual(in_RainToRemoveRoughness_mm, 0))
                {
                    if (rain_since_tillage < in_RainToRemoveRoughness_mm)

                        cn2 += TillageController.roughness_ratio * in_MaxRedInCNDueToTill * (rain_since_tillage / in_RainToRemoveRoughness_mm - 1);
                }

                if (effective_rain < 0.1)
                {
                    out_WatBal_Runoff_mm = IrrigationController.out_IrrigationRunoff;
                    return;
                }
                progress = 2;
                if (ModelOptionsController.UsePerfectCurveNoFn())
                {
                    //  *******************************************************
                    //  *  Calculate smx (CREAMS p14, equations i-3 and i-4)  *
                    //  *******************************************************
                    cn1 = -16.91 + 1.348 * cn2 - 0.01379 * cn2 * cn2 + 0.0001177 * cn2 * cn2 * cn2;
                    if (!MathTools.DoublesAreEqual(cn1, 0))
                        smx = 254.0 * ((100.0 / cn1) - 1.0);
                    else
                    {
                        smx = 0;

                        LogDivideByZeroError("CalculateRunoff", "cn1", "smx");
                    }
                    progress = 3;
                    //  ***************************************
                    //  *  Calculate retention parameter,  runoff_retention_number  *
                    //  ***************************************
                    sumh20 = 0.0;
                    for (int i = 0; i < in_LayerCount - 1; ++i)
                    {
                        if (!MathTools.DoublesAreEqual(SaturationLimit_rel_wp[i], 0))
                            sumh20 += wf[i] * (Math.Max(SoilWater_rel_wp[i], 0) / SaturationLimit_rel_wp[i]);
                        else
                        {

                            LogDivideByZeroError("CalculateRunoff", "SaturationLimit_rel_wp[i]", "sumh20");
                        }
                    }
                    runoff_retention_number = (int)(smx * (1.0 - sumh20));
                    //REMOVE INT STATEMENT AFTER VALIDATION
                    progress = 4;
                }
                else
                {
                    // ******************************************************************
                    // *  MODIFIED Calculate smx (CREAMS p14, equations i-3 and i-4)  	*
                    // *  Fix" for oversize Smx at low CN                     			*
                    // *  e.g. >254mm for cn2<70                              			*
                    // *  Brett Robinson May 2011                             			*
                    // ******************************************************************
                    double temp = 265.0 + (Math.Exp(0.17 * (cn2 - 50)) + 1);
                    if (!MathTools.DoublesAreEqual(temp, 0))
                    {
                        if (cn2 > 83) // linear above cn2=83
                            smx = 6 + (100 - cn2) * 6.66;
                        else            // logistic for cn2<=83
                            smx = 254.0 - (265.0 * Math.Exp(0.17 * (cn2 - 50))) / temp;
                    }
                    else
                    {
                        smx = 0;

                        LogDivideByZeroError("CalculateRunoff", "(265.0+(exp(cn2)+1)", "smx");
                    }
                    progress = 3;
                    //  ***************************************
                    //  *  Calculate retention parameter,  runoff_retention_number  *
                    //  ***************************************
                    sumh20 = 0.0;
                    // * CREAMS and other model discount S for water content (linear from air dry to sat) *
                    // * old code = relative to WP, new code = rel to air dry                             *
                    // * Changes by Brett Robinson May 2011                                               *
                    for (int i = 0; i < in_LayerCount - 1; ++i)
                    {
                        double deno = SaturationLimit_rel_wp[i] + AirDryLimit_rel_wp[i];
                        if (!MathTools.DoublesAreEqual(deno, 0))
                            sumh20 = sumh20 + wf[i] * (SoilWater_rel_wp[i] + AirDryLimit_rel_wp[i]) / deno;
                        else

                            LogDivideByZeroError("CalculateRunoff", "SaturationLimit_rel_wp[i]+AirDryLimit_rel_wp[i]", "sumh20");
                    }
                    runoff_retention_number = (int)(smx * (1.0 - sumh20));
                    //REMOVE INT STATEMENT AFTER VALIDATION
                    progress = 4;
                }

                //  *************************************************
                //  *  Calculate runoff (creams p14, equation i-1)  *
                //  *************************************************
                double denom = effective_rain + 0.8 * runoff_retention_number;
                double bas = effective_rain - 0.2 * runoff_retention_number;
                if (!MathTools.DoublesAreEqual(denom, 0) && bas > 0)
                {
                    out_WatBal_Runoff_mm = Math.Pow(bas, 2.0) / denom;
                    infiltration = effective_rain - out_WatBal_Runoff_mm;
                }
                else
                {
                    out_WatBal_Runoff_mm = 0;
                    infiltration = effective_rain;
                }

                //add any runoff from irrigation.
                out_WatBal_Runoff_mm += IrrigationController.out_IrrigationRunoff;

            }
            catch (Exception e)
            {
                if (progress == 0) ControlError = "CalculateRunoff - Calculate initial roughness effect on curve number (cn2).";
                else if (progress == 1) ControlError = "CalculateRunoff - Updating roughness effect on curve number (cn2).";
                else if (progress == 2) ControlError = "CalculateRunoff - Calculate smx (CREAMS p14, equations i-3 and i-4).";
                else if (progress == 3) ControlError = "CalculateRunoff - Calculate retention parameter,  runoff_retention_number.";
                else if (progress == 4) ControlError = "CalculateRunoff - Calculate runoff (creams p14, equation i-1).";
                else ControlError = "CalculateRunoff";
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculatSoilEvaporation()
        {
            try
            {
                //  ********************************************************************
                //  *  This function calculates soil evaporation using the Ritchie     *
                //  *  model.                                                          *
                //  ********************************************************************

                //  Calculate potential soil evaporation
                //  From proportion of bare soil
                out_WatBal_PotSoilEvap_mm = VegetationController.GetPotentialSoilEvaporation();

                if (IrrigationController.PondingExists())
                {
                    out_WatBal_SoilEvap_mm = out_WatBal_PotSoilEvap_mm;
                }
                else
                {
                    //  Add crop residue effects
                    ////NOTE THAT THIS USED TO ONLY BE FOR THE LAI MODEL -  I"VE NOW MADE IT FOR EITHER

                    if (total_crop_residue > 1.0)
                        out_WatBal_PotSoilEvap_mm = out_WatBal_PotSoilEvap_mm * (Math.Exp(-0.22 * total_crop_residue / 1000.0));


                    //  *******************************
                    //  *  initialize daily variables
                    //  ******************************
                    se1 = 0.0;
                    out_WatBal_SoilEvap_mm = 0.0;
                    se2 = 0.0;
                    se21 = 0.0;
                    se22 = 0.0;
                    //  **************************************************
                    //  * If infiltration has occurred then reset sse1.  *
                    //  * Reset sse2 if infiltration exceeds sse1.       *
                    //  **************************************************
                    if (infiltration > 0.0)
                    {
                        sse2 = Math.Max(0, sse2 - Math.Max(0, infiltration - sse1));
                        sse1 = Math.Max(0, sse1 - infiltration);
                        if (!MathTools.DoublesAreEqual(in_Cona_mm_per_sqrroot_day, 0))
                            dsr = Math.Pow(sse2 / in_Cona_mm_per_sqrroot_day, 2);
                        else
                        {
                            dsr = 0;

                            LogDivideByZeroError("CalculatSoilEvaporation", "in_Cona_mm_per_sqrroot_day", "dsr");
                        }
                    }
                    //  ********************************
                    //  *  Test for 1st stage drying.  *
                    //  ********************************
                    if (sse1 < in_Stage1SoilEvapLimitU_mm)
                    {
                        //  *****************************************************************
                        //  *  1st stage evaporation for today. Set se1 equal to potential  *
                        //  *  soil evaporation but limited by U.                           *
                        //  *****************************************************************
                        se1 = Math.Min(out_WatBal_PotSoilEvap_mm, in_Stage1SoilEvapLimitU_mm - sse1);
                        se1 = Math.Max(0.0, Math.Min(se1, SoilWater_rel_wp[0] + AirDryLimit_rel_wp[0]));

                        //  *******************************
                        //  *  Accumulate stage 1 drying  *
                        //  *******************************
                        sse1 = sse1 + se1;
                        //  ******************************************************************
                        //  *  Check if potential soil evaporation is satisfied by 1st stage *
                        //  *  drying.  If not, calculate some stage 2 drying(se2).          *
                        //  ******************************************************************
                        if (out_WatBal_PotSoilEvap_mm > se1)
                        {
                            //  *****************************************************************************
                            //  * If infiltration on day, and potential_soil_evaporation.gt.se1 (ie. a deficit in evap) .and. sse2.gt.0 *
                            //  * than that portion of potential_soil_evaporation not satisfied by se1 should be 2nd stage. This *
                            //  * can be determined by Math.Sqrt(time)*in_Cona_mm_per_sqrroot_day with any remainder ignored.          *
                            //  * If sse2 is zero, then use Ritchie's empirical transition constant (0.6).  *
                            //  *****************************************************************************
                            if (sse2 > 0.0)
                                se2 = Math.Min(out_WatBal_PotSoilEvap_mm - se1, in_Cona_mm_per_sqrroot_day * Math.Pow(dsr, 0.5) - sse2);
                            else
                                se2 = 0.6 * (out_WatBal_PotSoilEvap_mm - se1);

                            //  **********************************************************
                            //  *  Calculate stage two evaporation from layers 1 and 2.  *
                            //  **********************************************************

                            //  Any 1st stage will equal infiltration and therefore no net change in
                            //  soil water for layer 1 (ie can use SoilWater_rel_wp(1)+AirDryLimit_rel_wp(1) to determine se21.
                            se21 = Math.Max(0.0, Math.Min(se2, SoilWater_rel_wp[0] + AirDryLimit_rel_wp[0]));
                            se22 = Math.Max(0.0, Math.Min(se2 - se21, SoilWater_rel_wp[1] + AirDryLimit_rel_wp[1]));
                            //  ********************************************************
                            //  *  Re-Calculate se2 for when se2-se21 > SoilWater_rel_wp(2)+AirDryLimit_rel_wp(2)  *
                            //  ********************************************************
                            se2 = se21 + se22;
                            //  ************************************************
                            //  *  Update 1st and 2nd stage soil evaporation.  *
                            //  ************************************************
                            sse1 = in_Stage1SoilEvapLimitU_mm;
                            sse2 += se2;
                            if (!MathTools.DoublesAreEqual(in_Cona_mm_per_sqrroot_day, 0))
                                dsr = Math.Pow(sse2 / in_Cona_mm_per_sqrroot_day, 2);
                            else
                            {
                                dsr = 0;

                                LogDivideByZeroError("CalculatSoilEvaporation", "in_Cona_mm_per_sqrroot_day", "dsr");
                            }
                        }
                        else
                            se2 = 0.0;
                    }
                    else
                    {
                        sse1 = in_Stage1SoilEvapLimitU_mm;
                        //  ************************************************************************
                        //  *  No 1st stage drying. Calc. 2nd stage and remove from layers 1 & 2.  *
                        //  ************************************************************************
                        dsr = dsr + 1.0;
                        se2 = Math.Min(out_WatBal_PotSoilEvap_mm, in_Cona_mm_per_sqrroot_day * Math.Pow(dsr, 0.5) - sse2);
                        se21 = Math.Max(0.0, Math.Min(se2, SoilWater_rel_wp[0] + AirDryLimit_rel_wp[0]));
                        se22 = Math.Max(0.0, Math.Min(se2 - se21, SoilWater_rel_wp[1] + AirDryLimit_rel_wp[1]));
                        //  ********************************************************
                        //  *  Re-calculate se2 for when se2-se21 > SoilWater_rel_wp(2)+AirDryLimit_rel_wp(2)  *
                        //  ********************************************************
                        se2 = se21 + se22;
                        //  *****************************************
                        //  *   Update 2nd stage soil evaporation.  *
                        //  *****************************************
                        sse2 = sse2 + se2;
                        //  **************************************
                        //  *  calculate total soil evaporation  *
                        //  **************************************
                    }
                    out_WatBal_SoilEvap_mm = se1 + se2;
                }
            }
            catch (Exception e)
            {
                ControlError = "CalculatSoilEvaporation";
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void TryModelVegetation()
        {
            VegetationController.Simulate();
        }
        /// <summary>
        /// 
        /// </summary>
        public void UpdateWaterBalance()
        {
            //***********************************************************************
            //*  This subroutine performs the water balance. New nested loop        *
            //*  algorithm infiltrates and redistributes water in one pass.  This   *
            //*  new algorithm has many advantages over the previous one.  Firstly, *
            //*  it is more biophysically realistic; secondly, it considers the     *
            //*  effects of a restricted Ksat on both infiltration and              *
            //*  redistribution.   Previously, only redistribution was considered.  *
            //*  It should also bettern explain water movemnet under saturated      *
            //*  conditions.                                                        *
            //***********************************************************************
            double oflow = 0.0;
            overflow_mm = 0;
            double drain = infiltration;

            //  1.  Add all infiltration/drainage and extract ET.
            //  2.  Cascade a proportion of all water greater than drained upper limit (FC)
            //  3.  If soil water content is still greater than upper limit (SWMAX), add
            //      all excess above upper limit to runoff

            for (int i = 0; i < in_LayerCount; ++i)
            {
                Seepage[i] = drain;
                if (i == 0) SoilWater_rel_wp[i] += Seepage[i] - (out_WatBal_SoilEvap_mm - se22) - layer_transpiration[i];
                else if (i == 1) SoilWater_rel_wp[i] += Seepage[i] - layer_transpiration[i] + red[i] - se22;
                else SoilWater_rel_wp[i] += Seepage[i] - layer_transpiration[i] + red[i];

                if (SoilWater_rel_wp[i] > DrainUpperLimit_rel_wp[i])
                {
                    drain = swcon[i] * (SoilWater_rel_wp[i] - DrainUpperLimit_rel_wp[i]);
                    //			if(drain>(ksat[i]*12.0))
                    //				drain=ksat[i]*12.0;
                    if (drain > ksat[i])
                        drain = ksat[i];
                    else if (drain < 0)

                        drain = 0;
                    SoilWater_rel_wp[i] -= drain;
                }
                else
                    drain = 0;

                if (SoilWater_rel_wp[i] > SaturationLimit_rel_wp[i])
                {
                    oflow = SoilWater_rel_wp[i] - SaturationLimit_rel_wp[i];
                    SoilWater_rel_wp[i] = SaturationLimit_rel_wp[i];
                }

                int j = 0;
                while (oflow > 0)
                {

                    if (i - j == 0)    //look at first layer
                    {
                        overflow_mm += oflow;
                        out_WatBal_Runoff_mm = out_WatBal_Runoff_mm + oflow;
                        infiltration -= oflow;
                        Seepage[0] -= oflow;         //drainage in first layer
                        oflow = 0;
                    }
                    else           //look at other layersException e
                    {
                        SoilWater_rel_wp[i - j] += oflow;
                        Seepage[i - j + 1] -= oflow;
                        if (SoilWater_rel_wp[i - j] > SaturationLimit_rel_wp[i - j])
                        {
                            oflow = SoilWater_rel_wp[i - j] - SaturationLimit_rel_wp[i - j];
                            SoilWater_rel_wp[i - j] = SaturationLimit_rel_wp[i - j];
                        }
                        else
                            oflow = 0;
                        ++j;

                    }
                }
            }
            double satrange = SaturationLimit_rel_wp[0] - DrainUpperLimit_rel_wp[0];
            double satamount = SoilWater_rel_wp[0] - DrainUpperLimit_rel_wp[0];
            if (satamount > 0 && satrange > 0)
                saturationindex = satamount / satrange;
            else
                saturationindex = 0;
            Seepage[in_LayerCount] = drain;
            out_WatBal_DeepDrainage_mm = drain;
            total_soil_water = 0;
            for (int i = 0; i < in_LayerCount; ++i)
                total_soil_water += SoilWater_rel_wp[i];

        }
        /// <summary>
        /// 
        /// </summary>
        public void TryModelTillage()
        {
            TillageController.Simulate();
        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateResidue()
        {
            try
            {
                VegetationController.CalculateResidue();
                //we already estimated these in the Runoff function- but will recalculate here.
                total_crop_residue = VegetationController.GetTotalCropResidue();
                total_residue_cover = VegetationController.GetTotalResidueCover();
                total_residue_cover_percent = VegetationController.GetTotalResidueCoverPercent();
                total_cover = VegetationController.GetTotalCover();
                crop_cover = VegetationController.GetCropCover();
                total_cover_percent = total_cover * 100.0;
                accumulated_cover += total_cover_percent;
            }
            catch (Exception e)
            {
                ControlError = "CalculateResidue";
                throw new Exception(e.Message);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateErosion()
        {
            try
            {
                //  ***********************************************************************
                //  *  This subroutine calculates sediment yield in tonnes/ha using the   *
                //  *  Dave Freebairn method                                              *
                //  ***********************************************************************

                erosion_t_per_ha = 0;
                sed_catchmod = 0;
                if (out_WatBal_Runoff_mm <= 1)
                {
                    sediment_conc = 0;
                }
                else
                {
                    double conc = 0;
                    double cover;
                    if (!IrrigationController.ConsiderCoverEffects())
                        cover = Math.Min(100.0, (crop_cover + total_residue_cover * (1 - crop_cover)) * 100.0);
                    else
                        cover = IrrigationController.GetCoverEffect(crop_cover, total_residue_cover);
                    if (cover < 50.0)

                        conc = 16.52 - 0.46 * cover + 0.0031 * cover * cover;  //% sediment concentration Exception e max g/l is 165.2 when cover =0;
                    else if (cover >= 50.0)
                        conc = -0.0254 * cover + 2.54;
                    conc = Math.Max(0.0, conc);
                    erosion_t_per_ha = conc * usle_ls_factor * in_USLE_K * in_USLE_P * out_WatBal_Runoff_mm / 10.0;
                    sed_catchmod = conc * in_USLE_K * in_USLE_P * out_WatBal_Runoff_mm / 10.0;
                }
                if (!MathTools.DoublesAreEqual(out_WatBal_Runoff_mm, 0))
                {
                    if (!InRunoff2)
                        ++RunoffEventCount2;
                    InRunoff2 = true;

                    sediment_conc = erosion_t_per_ha * 100.0 / out_WatBal_Runoff_mm * in_SedDelivRatio;    //sediment concentration in g/l
                    if (sediment_conc > peakSedConc)
                        peakSedConc = sediment_conc;
                }
                else
                {
                    // dont log a divide by zero error for this one
                    if (InRunoff2)
                        cumSedConc += peakSedConc;
                    peakSedConc = 0;
                    InRunoff2 = false;
                    sediment_conc = 0;
                }
                offsite_sed_delivery = erosion_t_per_ha * in_SedDelivRatio;
            }
            catch (Exception e)
            {
                ControlError = "CalculateErosion";
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void TryModelRingTank()
        {
            IrrigationController.ModelRingTank();
        }
        /// <summary>
        /// 
        /// </summary>
        public void TryModelPesticide()
        {
            PesticideController.Simulate();
        }
        /// <summary>
        /// 
        /// </summary>
        public void TryModelPhosphorus()
        {
            PhosphorusController.Simulate();
        }
        /// <summary>
        /// 
        /// </summary>
        public void TryModelNitrate()
        {
            NitrateController.Simulate();
        }
        /// <summary>
        /// 
        /// </summary>
        public void TryModelSolutes()
        {
            SolutesController.Simulate();
        }
        /// <summary>
        /// 
        /// </summary>
        public void TryModelLateralFlow()
        {
            try
            {
                if (ModelOptionsController.CanCalculateLateralFlow())
                {

                    // Calculate most limiting Kratio
                    double kr;
                    double kratio = 1.0;
                    for (int i = 1; i < in_LayerCount; ++i)
                    {
                        if (!MathTools.DoublesAreEqual(ksat[i], 0))
                            kr = ksat[i - 1] / ksat[i];
                        else
                            kr = 0;
                        if (kr > kratio)
                            kratio = kr;
                    }

                    //  Convert in_FieldSlope_pc from percent to degrees

                    double slopedeg = Math.Atan(in_FieldSlope_pc / 100.0) * 180.0 / 3.14159;

                    // Calculate PredRH - lateral flow partitioning

                    double LN_kratio = Math.Log(kratio);
                    double LN_angle = Math.Log(slopedeg);
                    double LN_kratio2 = LN_kratio * LN_kratio;
                    double LN_angle2 = LN_angle * LN_angle;
                    double LNK_lnang = LN_kratio * LN_angle;

                    double numer = 0.04487067 + (0.019797884 * LN_kratio) - (0.020606403 * LN_angle)
                           + (0.01010285 * LN_kratio2) + (0.01415831 * LN_angle2)
                           - (0.011046881 * LNK_lnang);
                    double denom = 1 - (0.11431376 * LN_kratio) - (0.35073561 * LN_angle)
                           + (0.013044911) * (LN_kratio2) + (0.040556192 * LN_angle2)
                           + (0.015858813 * LNK_lnang);
                    if (!MathTools.DoublesAreEqual(denom, 0))
                        PredRh = numer / denom;
                    else
                    {
                        PredRh = 0;

                    }
                    out_WatBal_LateralFlow_mm = Seepage[in_LayerCount] * PredRh;
                    Seepage[in_LayerCount] = Seepage[in_LayerCount] * (1 - PredRh);
                }
                else
                    out_WatBal_LateralFlow_mm = 0;
            }
            catch (Exception e)
            {
                ControlError = "CalculateLateralFlow";
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void UpdateCropWaterBalance()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public void UpdateFallowWaterBalance()
        {
            try
            {
                if (VegetationController.InFallow())
                {
                    sum_fallow_rainfall += out_Rain_mm;
                    sum_fallow_runoff += out_WatBal_Runoff_mm;
                    sum_fallow_soilevaporation += out_WatBal_SoilEvap_mm;
                    sum_fallow_drainage += out_WatBal_DeepDrainage_mm;
                    sum_fallow_soilerosion += erosion_t_per_ha;

                    if (total_cover > 0.5)
                        ++fallow_days_with_more_50pc_cov;

                }
                else if (VegetationController.IsPlanting())
                    sum_fallow_soilwater += VegetationController.CalcFallowSoilWater();
            }
            catch (Exception e)
            {
                ControlError = "UpdateFallowWaterBalance";
                throw new Exception (e.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void UpdateTotalWaterBalance()
        {
            try
            {
                sum_rainfall += out_Rain_mm;
                sum_irrigation += out_WatBal_Irrigation_mm;
                sum_runoff += out_WatBal_Runoff_mm;
                sum_potevap += out_WatBal_PotSoilEvap_mm;
                sum_soilevaporation += out_WatBal_SoilEvap_mm;
                sum_transpiration += VegetationController.GetTotalTranspiration();
                sum_evapotranspiration = sum_soilevaporation + sum_transpiration;
                sum_overflow += overflow_mm;
                sum_drainage += out_WatBal_DeepDrainage_mm;
                sum_lateralflow += out_WatBal_LateralFlow_mm;
                sum_soilerosion += erosion_t_per_ha;
            }
            catch (Exception e)
            {
                ControlError = "UpdateTotalWaterBalance";
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void TryUpdateRingTankWaterBalance()
        {
            IrrigationController.UpdateRingtankWaterBalance();
        }
        /// <summary>
        /// 
        /// </summary>
        public void UpdateMonthlyStatistics()
        {
            try
            {
                int monthindex = month - 1;
                mo_MthlyAvgRainfall_mm[monthindex] += out_Rain_mm;
                mo_MthlyAvgEvaporation_mm[monthindex] += out_WatBal_SoilEvap_mm;
                mo_MthlyAvgTranspiration_mm[monthindex] += VegetationController.GetTotalTranspiration();
                mo_MthlyAvgRunoff_mm[monthindex] += out_WatBal_Runoff_mm;
                mo_MthlyAvgDrainage_mm[monthindex] += out_WatBal_DeepDrainage_mm;
            }
            catch (Exception e)
            {
                ControlError = "UpdateMonthlyStatistics";
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateVolumeBalanceError()
        {
            try
            {
                double sse;
                double deltasw = total_soil_water - previous_total_soil_water;
                if (ModelOptionsController.CanCalculateLateralFlow())
                {
                    if (!MathTools.DoublesAreEqual(out_Rain_mm, MathTools.MISSING_DATA_VALUE))
                        sse = (out_WatBal_Irrigation_mm + out_Rain_mm) - (deltasw + out_WatBal_Runoff_mm + out_WatBal_SoilEvap_mm + VegetationController.GetTotalTranspiration() + Seepage[in_LayerCount] + out_WatBal_LateralFlow_mm);
                    else
                        sse = (out_WatBal_Irrigation_mm + 0) - (deltasw + out_WatBal_Runoff_mm + out_WatBal_SoilEvap_mm + VegetationController.GetTotalTranspiration() + Seepage[in_LayerCount] + out_WatBal_LateralFlow_mm);
                }
                else
                {
                    if (!MathTools.DoublesAreEqual(out_Rain_mm, MathTools.MISSING_DATA_VALUE))
                        sse = (out_WatBal_Irrigation_mm + out_Rain_mm) - (deltasw + out_WatBal_Runoff_mm + out_WatBal_SoilEvap_mm + VegetationController.GetTotalTranspiration() + Seepage[in_LayerCount]);
                    else
                        sse = (out_WatBal_Irrigation_mm + 0) - (deltasw + out_WatBal_Runoff_mm + out_WatBal_SoilEvap_mm + VegetationController.GetTotalTranspiration() + Seepage[in_LayerCount]);
                }

                out_WatBal_VBE_mm = (int)(sse * 1000000) / 100000.0;
            }
            catch (Exception e)
            {
                ControlError = "CalculateVolumeBalanceError";
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void ExportDailyOutputs()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public void ResetAnyParametersIfRequired()
        {
            try
            {
                VegetationController.ResetAnyParametersIfRequired();
            }
            catch (Exception e)
            {
                ControlError = "ResetAnyParametersIfRequired";
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ConnectParametersToModel()
        {
            //	bool cancontinue=CurrentSimulationObject.ConnectParametersToModel(this);
            //	if(cancontinue)
            //	{
            //		return true;
            //	}
            //
            //	ErrorList.Add("There was an error connecting parameters to the model from Simulation: "+CurrentSimulationObject.ColumnText[0]);
            //	throw;
            return false;
        }

        //bool LoadSimulationObject(TSimulationInputObject* object)
        //{
        //    //	if(object&&object.ReadyToSimulate)
        //    //	{
        //    //
        //    //		CurrentSimulationObject=object;
        //    //		CurrentSimulationObject.CreateOutputObject(); //new - didn't want to autocreate as I want it null in batch processing.
        //    //		if(!FReset)Synchronize(BeginUpdate);
        //    //		ResetSwitchesToDefault();
        //    //		Synchronize(SynchronizeModelToDataConnections);
        //    //		return(CurrentSimulationObject.IsInitialised);
        //    //	}
        //    return false;
        //}

        /// <summary>
        /// 
        /// </summary>
        public void SynchronizeModelToDataConnections()
        {
            //	try
            //	{
            //		CurrentSimulationObject.IsInitialised=false;
            //		if(CurrentSimulationObject)
            //		{
            //			if(CurrentSimulationObject.ConnectParametersToModel(this))
            //			{
            //				CurrentSimulationObject.InitialiseOutputObjectForSimulation();
            //				CurrentSimulationObject.IsInitialised=true;
            //			}
            //		}
            //
            //	}
            //	catch(Exception e)
            //	{
            //		if(CurrentSimulationObject)
            //			throw(new Exception("There was an error connecting "+CurrentSimulationObject.ColumnText[0]+" to the simulation engine.", mtError, TMsgDlgButtons() << mbOK, 0);
            //		else
            //			throw(new Exception("There was an error connecting parameters to the simulation engine.", mtError, TMsgDlgButtons() << mbOK, 0);
            //		throw;
            //	}
        }
        /// <summary>
        /// 
        /// </summary>
        public void FinaliseSimulationParameters()
        {
            //	if(CurrentSimulationObject)	CurrentSimulationObject.Modified=true;
            ////	if(LoadSimulationObject(CurrentSimulationObject))
            // //	{
            //
            //	//	if(!FReset)RunCurrentSimulation(1,1);
            ////		if(!FReset)FProgress=100.0;
            ////		if(!FReset)Synchronize(PostProgressMessage);
            ////		if(!FReset)Synchronize(RunOnFinishSimumulatingEvent);
            //   // }

        }
        /// <summary>
        /// 
        /// </summary>
        public void InitialiseSimulationParameters()
        {
            try
            {
                if (number_of_days_in_simulation > 0)
                {

                    //			seriesindex=0;
                    //			climateindex=CurrentSimulationObject.SeriesStartIndex;
                    //			if(CurrentOutputObject)
                    //				CurrentOutputObject.ManagementHistory.Reset();
                    //			InitialiseSoilParameters();
                    //			IrrigationController.Initialise();
                    //			VegetationController.Initialise();
                    //			TillageController.Initialise();
                    //			PesticideController.Initialise();
                    //			PhosphorusController.Initialise();
                    //			NitrateController.Initialise();
                    //			SolutesController.Initialise();
                    //
                    //			InitialiseMonthlyStatisticsParameters();
                    //			InitialiseSummaryVariables();
                    //			UpdateTimeSeriesNames();
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
        public void InitialiseSoilParameters()
        {

            //InitialiseArray(depth, in_LayerCount + 1);
            //InitialiseArray(red, in_LayerCount);
            //InitialiseArray(wf, in_LayerCount);
            //InitialiseArray(SoilWater_rel_wp, in_LayerCount);
            //InitialiseArray(DrainUpperLimit_rel_wp, in_LayerCount);
            //InitialiseArray(SaturationLimit_rel_wp, in_LayerCount);
            //InitialiseArray(AirDryLimit_rel_wp, in_LayerCount);
            //InitialiseArray(Wilting_Point_RelOD_mm, in_LayerCount);
            //InitialiseArray(DUL_RelOD_mm, in_LayerCount);
            //InitialiseArray(ksat, in_LayerCount);
            //InitialiseArray(swcon, in_LayerCount);
            //InitialiseArray(Seepage, in_LayerCount + 1);
            //InitialiseArray(layer_transpiration, in_LayerCount);
            //InitialiseArray(mcfc, in_LayerCount);

            depth = new List<double>(in_LayerCount + 1);
            red = new List<double>(in_LayerCount);
            wf = new List<double>(in_LayerCount);
            SoilWater_rel_wp = new List<double>(in_LayerCount);
            DrainUpperLimit_rel_wp = new List<double>(in_LayerCount);
            SaturationLimit_rel_wp = new List<double>(in_LayerCount);
            AirDryLimit_rel_wp = new List<double>(in_LayerCount);
            Wilting_Point_RelOD_mm = new List<double>(in_LayerCount);
            DUL_RelOD_mm = new List<double>(in_LayerCount);
            ksat = new List<double>(in_LayerCount);
            swcon = new List<double>(in_LayerCount);
            Seepage = new List<double>(in_LayerCount + 1);
            layer_transpiration = new List<double>(in_LayerCount);
            mcfc = new List<double>(in_LayerCount);

            swd = 0;
            sse1 = 0;
            sse2 = 0;
            se1 = 0;
            se2 = 0;
            se21 = 0;
            se22 = 0;
            dsr = 0;
            cumSedConc = 0;
            peakSedConc = 0;
            depth[0] = 0;
            for (int i = 0; i < in_LayerCount; ++i)
            {
                depth[i + 1] = (int)(in_Depths[i] + 0.5);
                red[i] = 0;
                ksat[i] = in_LayerDrainageLimit_mm_per_day[i];///12.0;
            }

            total_soil_water = 0.0;
            previous_total_soil_water = 0.0;
            for (int i = 0; i < in_LayerCount; ++i)
            {
                if (depth[i + 1] - depth[i] > 0)
                {
                    //PERFECT soil water alorithms relate all values to wilting point.
                    double deltadepth = (depth[i + 1] - depth[i]) * 0.01;
                    Wilting_Point_RelOD_mm[i] = in_SoilLimitWiltingPoint_pc[i] * deltadepth;
                    DUL_RelOD_mm[i] = in_SoilLimitFieldCapacity_pc[i] * deltadepth;
                    if (i == 0)
                        AirDryLimit_rel_wp[0] = Wilting_Point_RelOD_mm[i] - in_SoilLimitAirDry_pc[i] * deltadepth;
                    else if (i == 1)
                        AirDryLimit_rel_wp[1] = 0.5 * (Wilting_Point_RelOD_mm[i] - in_SoilLimitAirDry_pc[i] * deltadepth);
                    else
                        AirDryLimit_rel_wp[i] = 0;

                    DrainUpperLimit_rel_wp[i] = (in_SoilLimitFieldCapacity_pc[i] * deltadepth) - Wilting_Point_RelOD_mm[i];
                    SaturationLimit_rel_wp[i] = (in_SoilLimitSaturation_pc[i] * deltadepth) - Wilting_Point_RelOD_mm[i];
                }
                else
                {

                    DrainUpperLimit_rel_wp[i] = 0;
                    SaturationLimit_rel_wp[i] = 0;
                    AirDryLimit_rel_wp[0] = 0;
                }
            }

            for (int i = 0; i < in_LayerCount; ++i)
            {
                SoilWater_rel_wp[i] = ModelOptionsController.GetInitialPAW() * DrainUpperLimit_rel_wp[i];

                if (SoilWater_rel_wp[i] > SaturationLimit_rel_wp[i])
                    SoilWater_rel_wp[i] = SaturationLimit_rel_wp[i];
                else if (SoilWater_rel_wp[i] < 0)
                    SoilWater_rel_wp[i] = 0;
                total_soil_water += SoilWater_rel_wp[i];
            }

            total_crop_residue = 0;
            total_residue_cover = 0;//0.707*(1.0-exp(-1.0*total_crop_residue/1000.0));
            total_residue_cover_percent = 0;


            CalculateInitialValuesOfCumulativeSoilEvaporation();

            CalculateDepthRetentionWeightFactors();

            CalculateDrainageFactors();

            CalculateUSLE_LSFactor();
            RunoffEventCount2 = 0;
            PredRh = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateInitialValuesOfCumulativeSoilEvaporation()
        {
            //  Calculate initial values of cumulative soil evaporation
            if (DrainUpperLimit_rel_wp[0] - SoilWater_rel_wp[0] > in_Stage1SoilEvapLimitU_mm)
            {
                sse1 = in_Stage1SoilEvapLimitU_mm;
                sse2 = Math.Max(0.0, DrainUpperLimit_rel_wp[0] - SoilWater_rel_wp[0]) - in_Stage1SoilEvapLimitU_mm;
            }
            else
            {
                sse1 = Math.Max(0.0, DrainUpperLimit_rel_wp[0] - SoilWater_rel_wp[0]);
                sse2 = 0.0;
            }
            if (!MathTools.DoublesAreEqual(in_Cona_mm_per_sqrroot_day, 0))
                dsr = Math.Pow(sse2 / in_Cona_mm_per_sqrroot_day, 2.0);
            else
            {
                dsr = 0;

                LogDivideByZeroError("InitialiseSoilParameters", "in_Cona_mm_per_sqrroot_day", "dsr");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateUSLE_LSFactor()
        {
            if (ModelOptionsController.UsePerfectUSLELSFn())
            {
                double aht = in_FieldSlope_pc * in_SlopeLength_m / 100.0;
                double lambda = 3.281 * (Math.Sqrt(in_SlopeLength_m * in_SlopeLength_m + aht * aht));
                double theta;
                if (!MathTools.DoublesAreEqual(in_SlopeLength_m, 0))
                    theta = Math.Asin(aht / in_SlopeLength_m);
                else
                {
                    theta = 0;

                    LogDivideByZeroError("CalculateUSLE_LSFactor", "in_SlopeLength_m", "theta");
                }
                if (!MathTools.DoublesAreEqual(1.0 + in_RillRatio, 0))
                {
                    if (in_FieldSlope_pc < 9.0)

                        usle_ls_factor = Math.Pow(lambda / 72.6, in_RillRatio / (1.0 + in_RillRatio)) * (10.8 * Math.Sin(theta) + 0.03);
                    else
                        usle_ls_factor = Math.Pow(lambda / 72.6, in_RillRatio / (1.0 + in_RillRatio)) * (16.8 * Math.Sin(theta) - 0.5);
                }
                else
                {
                    usle_ls_factor = 0;

                    LogDivideByZeroError("CalculateUSLE_LSFactor", "1.0+in_RillRatio", "usle_ls_factor");
                }
            }
            else
            {
                if (!MathTools.DoublesAreEqual(1.0 + in_RillRatio, 0))
                {
                    usle_ls_factor = Math.Pow(in_SlopeLength_m / 22.1, in_RillRatio / (1.0 + in_RillRatio)) * (0.065 + 0.0456 * in_FieldSlope_pc + 0.006541 * Math.Pow(in_FieldSlope_pc, 2));
                }
                else
                {
                    usle_ls_factor = 0;

                    LogDivideByZeroError("CalculateUSLE_LSFactor", "1.0+in_RillRatio", "usle_ls_factor");
                }

            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateDepthRetentionWeightFactors()
        {
            double a, b;
            for (int i = 0; i < in_LayerCount - 1; ++i)
            {
                if (depth[in_LayerCount - 1] > 0)
                {
                    a = -4.16 * (depth[i] / depth[in_LayerCount - 1]);
                    b = -4.16 * (depth[i + 1] / depth[in_LayerCount - 1]);
                    wf[i] = 1.016 * (Math.Exp(a) - Math.Exp(b));
                }
                else
                {
                    wf[i] = 0;

                    LogDivideByZeroError("CalculateDepthRetentionWeightFactors", "depth[in_LayerCount-1]", "wf[i]");
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="s2"></param>
        /// <param name="s3"></param>
        public void LogDivideByZeroError(string s, string s2, string s3)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateDrainageFactors()
        {
            for (int i = 0; i < in_LayerCount; ++i)
            {
                if (ksat[i] > 0)
                {
                    // I've got rid of the old PERFECTism regarding Ksat
                    // the commented bits below was just my testing algorithms,
                    // to make sure the results are identical to the reworked equations.
                    //	double oldksat=ksat[i]/12.0;
                    //	double val1 = 48.0/(2.0*(SaturationLimit_rel_wp[i]-DrainUpperLimit_rel_wp[i])/oldksat+24.0);
                    //	double val2 = 2.0*ksat[i]/(SaturationLimit_rel_wp[i]-DrainUpperLimit_rel_wp[i]+ksat[i]);
                    //	if(int(val1*1000000)!=int(val2*1000000))
                    //		throw(new Exception("error!", mtError, TMsgDlgButtons() << mbOK, 0);
                    //	swcon[i]=val2;
                    double temp = (SaturationLimit_rel_wp[i] - DrainUpperLimit_rel_wp[i] + ksat[i]);
                    if (!MathTools.DoublesAreEqual(temp, 0))
                        swcon[i] = 2.0 * ksat[i] / temp;
                    else
                    {
                        swcon[i] = 0;

                        LogDivideByZeroError("CalculateDrainageFactors", "SaturationLimit_rel_wp[i]-DrainUpperLimit_rel_wp[i]+ksat[i]", "swcon[i]");
                    }
                }
                else
                    swcon[i] = 0;
                if (swcon[i] < 0)
                    swcon[i] = 0;
                else if (swcon[i] > 1) swcon[i] = 1;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public double SumRain(int n, int delay)
        {

            double sumrain = 0;
            int index;
            for (int i = 0; i < n; ++i)
            {
                index = climateindex - i - delay;
                if (index >= 0)
                    sumrain += Rainfall[index];
            }
            return sumrain;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="day"></param>
        /// <param name="sim"></param>
        /// <param name="simcount"></param>
        /// <param name="daycount"></param>
        /// <param name="forceupdate"></param>
        public void UpdateProgress(int day, int sim, int simcount, int daycount, bool forceupdate)
        {
            if (!FReset || forceupdate)
            {
                double totalcount = simcount * daycount;
                if (!MathTools.DoublesAreEqual(totalcount, 0))
                    FProgress = (int)((double)(sim * daycount + day) / totalcount * 100.0 + 0.5);
                else
                    FProgress = 0;
                if (daycount != 0)
                    FSimProgress = (int)((double)(day) / (double)(daycount) * 100.0 + 0.5);
                else
                    FSimProgress = 0;
                if (FLastProgress != FProgress && (forceupdate || FProgress % 5 == 0))
                {

                    UpdateTotalSimulationProgress();
                    FLastProgress = Progress;
                }

                if (FLastSimProgress != FSimProgress && (forceupdate || FSimProgress % 10 == 0))
                {

                    UpdateCurrentSimulationProgress();
                    FLastSimProgress = FSimProgress;
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateInputParametersForm()
        {
            //	#ifdef GUI
            //	SendMessage(_MainForm.Handle, WM_UPDATEINPUTPARAMETERSFORM, 0, 0);
            //	#endif
        }
        /// <summary>
        /// 
        /// </summary>
        public void UpdateCurrentSimulationProgress()
        {
            //	#ifdef GUI
            //	int id=CurrentSimulationObject.ID;
            //	SendMessage(_MainForm.Handle, WM_UPDATECURRENTSIMULATIONPROGRESS, id, FSimProgress);
            //	#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateTotalSimulationProgress()
        {
            //	#ifdef GUI
            //	if(_MainForm)
            //	{
            //		int id=CurrentSimulationObject.ID;
            //		SendMessage(_MainForm.Handle, WM_UPDATETOTALSIMULATIONPROGRESS, id, FProgress );
            //		//SendMessage(_MainForm.Handle, WM_UPDATETOTALSIMULATIONPROGRESS2, id, FProgress );
            //	}
            //
            ////	if(_MainForm)
            ////	{
            ////		_MainForm.ProgramManager.ActiveManager.CalculatingCaption="Simulating "+CurrentSimulationObject.ColumnText[0];
            ////		_MainForm.ProgramManager.ActiveManager.CalculationProgress=0;
            ////	}
            //	#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetProgress(int value)
        {
            //	if(value!=FProgress)
            //	{
            //		FProgress = value;
            //		if(value%5==0)
            //			Synchronize(PostProgressMessage);
            //
            //	}
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetProgress()
        {
            return FProgress;
        }
        /// <summary>
        /// 
        /// </summary>
        public void PostProgressMessage()
        {
            //if(FProgramManager.ActiveManager)
            //	{
            //        if(FProgress>0)
            //		{
            //            if(CurrentSimulationObject)
            //				FProgramManager.ActiveManager.CalculatingCaption="Simulating "+CurrentSimulationObject.ColumnText[0];
            //		}
            //        else
            //            FProgramManager.ActiveManager.CalculatingCaption="InitialisingException e ";
            //		FProgramManager.ActiveManager.CalculationProgress=FProgress;
            //
            //	}

        }
        /// <summary>
        /// 
        /// </summary>
        public void UpdateOutput()
        {
            //if (!FReset && FOnUpdateOutput)
            //{

            //    FOnUpdateOutput();

            //}

        }

        //TSimulateOutputObject* GetCurrentOutputObject()
        //{
        //    //	return CurrentSimulationObject.OutputObject;
        //}


        //void SetSimulationObjectsList(TList* value)
        //{
        //    FSimulationObjectsList = value;
        //}

        //TList* GetSimulationObjectsList()
        //{
        //    return FSimulationObjectsList;
        //}

        /// <summary>
        /// 
        /// </summary>
        public void BeginUpdate()
        {
            //	 if(CurrentOutputObject)CurrentOutputObject.BeginUpdate();
            //	 CurrentSimulationObject.ResetUsedState();
        }
        /// <summary>
        /// 
        /// </summary>
        public void EndUpdate()
        {
            //	if(CurrentSimulationObject&&CurrentOutputObject)
            //	{
            //		CurrentSimulationObject.Modified=false;
            //		for(int i=0;i<CropCount;++i)
            //			if(Crop[i])CurrentSimulationObject.VegetationDataTemplatePointer[i].IsUsedInSimulation=CurrentOutputObject.VegTimeSeriesHeader[i].toRootDepth.IsNonZero;
            //
            //		CurrentSimulationObject.IrrigationDataTemplatePointer.IsUsedInSimulation=CurrentOutputObject.toIrrigation.IsNonZero;
            //		for(int i=0;i<PesticideCount;++i)
            //			CurrentSimulationObject.PesticideDataTemplatePointer[i].IsUsedInSimulation=CurrentOutputObject.PesticideTimeSeriesHeader[i].toPestInSoil_g_per_ha.IsNonZero;;
            //		for(int i=0;i<TillageCount;++i)
            //			CurrentSimulationObject.TillageDataTemplatePointer[i].IsUsedInSimulation=CurrentOutputObject.toResidueReductionFromTillage.IsNonZero;;
            //
            //	}
        }
        /// <summary>
        /// 
        /// </summary>
        public void EndOutputObjectUpdate()
        {
            //	if(CurrentOutputObject)CurrentOutputObject.EndUpdate();
            //	if(CurrentSimulationObject&&ZerosList.Count>0)
            //	{
            //		ZerosList.Add("******* RECOMENDATIONS *********");
            //		ZerosList.Add("HowLeaky was able to continue simulating but you should be very wary of these results");
            //		if(ZerosList.Text>0&&ZerosList.Text<15)
            //			throw(new Exception(ZerosList.Text, mtWarning, TMsgDlgButtons() << mbOK, 0);
            //		else
            //		{
            //		   //	throw(new Exception("Many divide by zero errors ("+String(ZeroesList.Count)+")", mtWarning, TMsgDlgButtons() << mbOK, 0);
            //			SmartPointer<TStringList>list(new TStringList);
            //			for(int i=0;i<ZerosList.Count;++i)
            //			{
            //				if(i<15&&ZerosList.Strings[i].Length())
            //					list.Add(ZerosList.Strings[i]) ;
            //			}
            //			throw(new Exception(list.Text, mtWarning, TMsgDlgButtons() << mbOK, 0);
            //		}
            //	}
            ////		CurrentSimulationObject.Comments=ZerosList.Text;
        }
        /// <summary>
        /// 
        /// </summary>
        public void UpdateStartMessages()
        {
            //	#ifdef GUI
            //	Progress=0;
            //	if(FProgramManager&&FProgramManager.ActiveManager)
            //		FProgramManager.ActiveManager.IsCalculating=true;
            //	#endif
        }
        /// <summary>
        /// 
        /// </summary>
        public void UpdateFinishMessages()
        {
            //	#ifdef GUI
            //	FProgress=100.0;
            //	Synchronize(PostProgressMessage);
            //	if(FProgramManager&&FProgramManager.ActiveManager)
            //		FProgramManager.ActiveManager.IsCalculating=false;
            //	#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetParameters()
        {
            //	if(CropList)CropList.Clear();
            //	CurrentSimulationObject=0;
            //    FSimulationObjectsList=0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="me"></param>
        /// <param name="cropindex"></param>
        public void UpdateManagementEventHistory(ManagementEvent me, int cropindex)
        {
            //if (CurrentOutputObject)
            //{
            //    int index = me.GetHashCode();
            //    if (index > (int)ManagementEvent.meCropGrowing)

            //    {
            //        index = index - 6;
            //        index = 6 + (cropindex * 4) + index;
            //    }
            //    CurrentOutputObject.ManagementEvents[index]
            //    [seriesindex] = true;//GetManagementEventType();
            //    CurrentOutputObject.ManagementHistory.AddEvent(me, cropindex, year, month, day);
            //}
        }
        /// <summary>
        /// 
        /// </summary>
        public void RunOnFinishSimumulatingEvent()
        {
            //	if(FOnFinishSimulating)
            //		FOnFinishSimulating();
        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateMonthlyOutputs()
        {

            double simyears = number_of_days_in_simulation / 365.0;
            for (int i = 0; i < 12; ++i)
            {
                mo_MthlyAvgRainfall_mm[i] = Divide(mo_MthlyAvgRainfall_mm[i], simyears);
                mo_MthlyAvgEvaporation_mm[i] = Divide(mo_MthlyAvgEvaporation_mm[i], simyears);
                mo_MthlyAvgTranspiration_mm[i] = Divide(mo_MthlyAvgTranspiration_mm[i], simyears);
                mo_MthlyAvgRunoff_mm[i] = Divide(mo_MthlyAvgRunoff_mm[i], simyears);
                mo_MthlyAvgDrainage_mm[i] = Divide(mo_MthlyAvgDrainage_mm[i], simyears);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateSummaryOutputs()
        {
            double simyears = number_of_days_in_simulation / 365.0;

            so_YrlyAvgRainfall_mm_per_yr = Divide(sum_rainfall, simyears);
            so_YrlyAvgIrrigation_mm_per_yr = Divide(sum_irrigation, simyears);
            so_YrlyAvgRunoff_mm_per_yr = Divide(sum_runoff, simyears);
            so_YrlyAvgSoilEvaporation_mm_per_yr = Divide(sum_soilevaporation, simyears);
            so_YrlyAvgTranspiration_mm_per_yr = Divide(sum_transpiration, simyears);
            so_YrlyAvgEvapotransp_mm_per_yr = Divide(sum_evapotranspiration, simyears);
            so_YrlyAvgOverflow_mm_per_yr = Divide(sum_overflow, simyears);
            so_YrlyAvgDrainage_mm_per_yr = Divide(sum_drainage, simyears);
            so_YrlyAvgLateralFlow_mm_per_yr = Divide(sum_lateralflow, simyears);
            so_YrlyAvgSoilErosion_T_per_ha_per_yr = Divide(sum_soilerosion, simyears);
            so_YrlyAvgOffsiteSedDel_T_per_ha_per_yr = Divide(sum_soilerosion * in_SedDelivRatio, simyears);
            so_TotalCropsPlanted = VegetationController.GetTotalCropsPlanted();
            so_TotalCropsHarvested = VegetationController.GetTotalCropsHarvested();
            so_TotalCropsKilled = VegetationController.GetTotalCropsKilled();
            so_AvgYieldPerHrvst_t_per_ha_per_hrvst = VegetationController.GetAvgYieldPerHarvest();
            so_AvgYieldPerPlant_t_per_ha_per_plant = VegetationController.GetAvgYieldPerPlanting();
            so_AvgYieldPerYr_t_per_ha_per_yr = VegetationController.GetAvgYieldPerYear();
            so_YrlyAvgCropRainfall_mm_per_yr = Divide(sum_crop_rainfall, simyears);
            so_YrlyAvgCropIrrigation_mm_per_yr = Divide(sum_crop_irrigation, simyears);
            so_YrlyAvgCropRunoff_mm_per_yr = Divide(sum_crop_runoff, simyears);
            so_YrlyAvgCropSoilEvap_mm_per_yr = Divide(sum_crop_soilevaporation, simyears);
            so_YrlyAvgCropTransp_mm_per_yr = Divide(sum_crop_transpiration, simyears);
            so_YrlyAvgCropEvapotransp_mm_per_yr = Divide(sum_crop_evapotranspiration, simyears);
            so_YrlyAvgCropOverflow_mm_per_yr = Divide(sum_crop_overflow, simyears);
            so_YrlyAvgCropDrainage_mm_per_yr = Divide(sum_crop_drainage, simyears);
            so_YrlyAvgCropLateralFlow_mm_per_yr = Divide(sum_crop_lateralflow, simyears);
            so_YrlyAvgCropSoilErosion_T_per_ha_per_yr = Divide(sum_crop_soilerosion, simyears);
            so_YrlyAvgCropOffsiteSedDel_T_per_ha_per_yr = Divide(sum_crop_soilerosion * in_SedDelivRatio, simyears);
            so_YrlyAvgFallowRainfall_mm_per_yr = Divide(sum_fallow_rainfall, simyears);
            so_YrlyAvgFallowIrrigation_mm_per_yr = Divide(sum_fallow_irrigation, simyears);
            so_YrlyAvgFallowRunoff_mm_per_yr = Divide(sum_fallow_runoff, simyears);
            so_YrlyAvgFallowSoilEvap_mm_per_yr = Divide(sum_fallow_soilevaporation, simyears);
            so_YrlyAvgFallowOverflow_mm_per_yr = Divide(sum_fallow_overflow, simyears);
            so_YrlyAvgFallowDrainage_mm_per_yr = Divide(sum_fallow_drainage, simyears);
            so_YrlyAvgFallowLateralFlow_mm_per_yr = Divide(sum_fallow_lateralflow, simyears);
            so_YrlyAvgFallowSoilErosion_T_per_ha_per_yr = Divide(sum_fallow_soilerosion, simyears);
            so_YrlyAvgFallowOffsiteSedDel_T_per_ha_per_yr = Divide(sum_fallow_soilerosion * in_SedDelivRatio, simyears);
            fallow_efficiency = Divide(sum_fallow_soilwater * 100.0, sum_fallow_rainfall);
            so_YrlyAvgPotEvap_mm = Divide(sum_potevap, simyears);
            so_YrlyAvgRunoffAsPercentOfInflow_pc = Divide(so_YrlyAvgRunoff_mm_per_yr * 100.0, so_YrlyAvgRainfall_mm_per_yr + so_YrlyAvgIrrigation_mm_per_yr);
            so_YrlyAvgEvapAsPercentOfInflow_pc = Divide(so_YrlyAvgSoilEvaporation_mm_per_yr * 100.0, so_YrlyAvgRainfall_mm_per_yr + so_YrlyAvgIrrigation_mm_per_yr);
            so_YrlyAvgTranspAsPercentOfInflow_pc = Divide(so_YrlyAvgTranspiration_mm_per_yr * 100.0, so_YrlyAvgRainfall_mm_per_yr + so_YrlyAvgIrrigation_mm_per_yr);
            so_YrlyAvgDrainageAsPercentOfInflow_pc = Divide(so_YrlyAvgDrainage_mm_per_yr * 100.0, so_YrlyAvgRainfall_mm_per_yr + so_YrlyAvgIrrigation_mm_per_yr);
            so_YrlyAvgPotEvapAsPercentOfInflow_pc = Divide(so_YrlyAvgPotEvap_mm * 100.0, so_YrlyAvgRainfall_mm_per_yr + so_YrlyAvgIrrigation_mm_per_yr);
            so_YrlyAvgCropSedDel_t_per_ha_per_yr = Divide(accumulated_crop_sed_deliv, simyears);
            so_YrlyAvgFallowSedDel_t_per_ha_per_yr = Divide(accumulated_fallow_sed_deliv, simyears);
            so_RobinsonErrosionIndex = Divide(so_YrlyAvgSoilErosion_T_per_ha_per_yr * so_YrlyAvgCover_pc, in_FieldSlope_pc);
            so_YrlyAvgCover_pc = Divide(accumulated_cover, number_of_days_in_simulation);
            so_YrlyAvgFallowDaysWithMore50pcCov_days = Divide(fallow_days_with_more_50pc_cov * 100.0, number_of_days_in_simulation);
            so_AvgCoverBeforePlanting_pc = Divide(accumulate_cov_day_before_planting * 100.0, total_number_plantings);
            so_SedimentEMCBeoreDR = -32768;
            so_SedimentEMCAfterDR = -32768;
            so_AvgSedConcInRunoff = Divide(sum_soilerosion * in_SedDelivRatio * 100.0, sum_runoff);//for g/L
        }

        public double Divide(double num, double denom)

        {
            return num / denom;
        }
        //bool LoadInputParameters(TSimulationInputObject* inputobject)
        //{
        //    CurrentInputObject = inputobject;
        //    //	if(	LoadClimateData(inputobject.LocationData))
        //    {
        //        //	LoadSoilInputParameters(inputobject.SoilData);
        //        //
        //        //	IrrigationController.	LoadInputParameters(inputobject.IrrigationData);
        //        //	VegetationController.	LoadInputParameters(inputobject.VegetationData);
        //        //	TillageController.		LoadInputParameters(inputobject.TillageData);
        //        //	PesticideController.	LoadInputParameters(inputobject.PesticideData);
        //        //	PhosphorusController.	LoadInputParameters(inputobject.PhosphorusData);
        //        //	NitrateController.		LoadInputParameters(inputobject.NitrateData);
        //        //	SolutesController.		LoadInputParameters(inputobject.SoluteData);
        //        //	OptionsController.		LoadInputParameters(inputobject.ModelOptionsData);
        //        LoadStartEndDates(inputobject);
        //    }
        //}

        //bool LoadClimateData(TSimTemplateGroup<TTimeSeriesDataTemplate>* locationdata)
        //{
        //    try
        //    {
        //        //		TLocationObjectLock lock;
        //        //		if(DataFileObject)
        //        //		{
        //        //			LoadTemplateData(FileName);
        //        //			if(FDataFileObject.TimeSeriesCount>4&&FDataFileObject.DataIsLoaded)
        //        //			{
        //        //				Latitude		= Latitude;
        //        //				MaxTemp			= FDataFileObject.GetBaseValuesPtr(0,false);
        //        //				MinTemp			= FDataFileObject.GetBaseValuesPtr(1,false);
        //        //				Rainfall		= FDataFileObject.GetBaseValuesPtr(2,true);
        //        //				Evaporation		= FDataFileObject.GetBaseValuesPtr(3,true);
        //        //				SolarRadiation	= FDataFileObject.GetBaseValuesPtr(4,false);
        //        //				return true;
        //        //			 }
        //        //		}
        //    }
        //    catch (Exception e)
        //    {
        //    }
        //    return false;
        //}

        public bool LoadSoilInputParameters(Object soildata)
        {
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputobject"></param>
        /// <returns></returns>
        public bool LoadStartEndDates(Object inputobject)
        {
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        public void ResetToDefault()
        {
            ModelOptionsController.in_ResetSoilWaterAtDate = false;
            ModelOptionsController.in_ResetResidueAtDate = false;
            ModelOptionsController.in_ResetSoilWaterAtPlanting = false;
            ModelOptionsController.in_CanCalculateLateralFlow = false;
            ModelOptionsController.in_UsePERFECTGroundCovFn = false;
            ModelOptionsController.in_IgnoreCropKill = false;
            ModelOptionsController.in_UsePERFECTDryMatterFn = false;
            ModelOptionsController.in_UsePERFECTLeafAreaFn = false;
            ModelOptionsController.in_UsePERFECT_USLE_LS_Fn = true;
            ModelOptionsController.in_UsePERFECTResidueFn = false;
            ModelOptionsController.in_UsePERFECTSoilEvapFn = false;
            ModelOptionsController.in_UsePERFECTCurveNoFn = Simulation.DEFAULT_CN;
            ModelOptionsController.in_InitialPAW = 0.5;
        }


        /// <summary>
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// </summary>

        ////Ignore this for serialisation
        //public Project parent;

        ////Ignore this for serialisation
        //public List<string> modelPaths;

        ////Ignore this for serialisation
        //public bool active { get; set; } = false;

        //public List<DataModel> dataModels;

        ////Ignore this for serialisation
        ////public List<ModelControllers> modelControllers;

        //public Simulation()
        //{
        //    modelPaths = new List<string>();
        //    dataModels = new List<DataModel>();
        //}

        //public Simulation(Project parent) : this()
        //{
        //    this.parent = parent;
        //}

        //public Simulation(List<DataModel> dataModels) : this()
        //{
        //    //Assume how this is coing to come from Client/Server Comms doesn't neccessarily need a parent project

        //    //Work out which model is which by type - if possible after serialization

        //    this.dataModels = dataModels;
        //}

        //public void createFromXMLNode(XmlNode node)
        //{
        //    //Populate the DataModels

        //}
    }
}
