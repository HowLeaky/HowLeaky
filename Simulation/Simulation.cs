
using HowLeaky.CustomAttributes;
using HowLeaky.DataModels;
using HowLeaky.InputModels;
using HowLeaky.Interfaces;
using HowLeaky.ModelControllers;
using HowLeaky.OutputModels;
using HowLeaky.SyncModels;
using HowLeaky.Tools;
using HowLeaky.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace HowLeaky
{
    public enum ManagementEvent { Planting, Harvest, Tillage, Pesticide, Irrigation, CropGrowing, InPlantingWindow, MeetsSoilWaterPlantCritera, MeetsDaysSinceHarvestPlantCritera, MeetsRainfallPlantCritera, None };

    public class SimulationOutputModel : OutputDataModel, IDailyOutput
    {
        public SimulationOutputModel() : base() { }

        public DateTime Date { get; set; }
        [Unit("mm")]
        public double Rain { get; set; }            // Daily rainfall amount (mm) as read directly from the P51 file.
        [Unit("oC")]
        public double MaxTemp { get; set; }         // Daily max temperature (oC) as read directly from the P51 file.
        [Unit("oC")]
        public double MinTemp { get; set; }         // Daily min temperature (oC) as read directly from the P51 file.
        [Unit("mm")]
        public double PanEvap { get; set; }         // Daily pan evaporation (mm) as read directly from the P51 file.
        [Unit("MJ_per_m2_per_day")]
        public double SolarRad { get; set; }        // Daily solar radition (mMJ/m^2/day) as read directly from the P51 file.
    }

    public class Simulation : HLController
    {
        public static int ROBINSON_CN = 0;
        public static int PERFECT_CN = 1;
        public static int DEFAULT_CN = 2;

        public bool RunSilent { get; set; }
        public bool CanLog { get; set; }
        public bool Use2008CurveNoFn { get; set; }
        public bool Force2011CurveNoFn { get; set; }

        //--------------------------------------------------------------------------
        // Submodel Controllers
        //--------------------------------------------------------------------------
        public IrrigationController IrrigationController { get; set; } = null;
        public VegetationController VegetationController { get; set; } = null;
        public TillageController TillageController { get; set; } = null;
        public PesticideController PesticideController { get; set; } = null;
        public PhosphorusController PhosphorusController { get; set; } = null;
        public NitrateController NitrateController { get; set; } = null;
        public SolutesController SolutesController { get; set; } = null;
        public ModelOptionsController ModelOptionsController { get; set; } = null;
        public ClimateController ClimateController { get; set; } = null;
        public SoilController SoilController { get; set; } = null;
        public OutputModelController OutputModelController { get; set; } = null;

        public List<HLController> ActiveControlllers { get; set; }

        public SimulationOutputModel Out { get; set; } = new SimulationOutputModel();

        public int NumberOfDaysInSimulation { get; set; }
        public DateTime Today { get; set; }

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
        public bool FReset { get; set; }

        public bool NeedToUpdateOutput { get; set; }

        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public int StartYear { get; set; }
        public int EndYear { get; set; }

        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public int Seriesindex { get; set; }
        public int Climateindex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Simulation()
        {
            // FProgramManager = manager;

            ErrorList = new List<string>();
            ZerosList = new List<string>();

            CanLog = false;

            RunSilent = false;

            Force2011CurveNoFn = false;

            IrrigationController = null;
            VegetationController = null;
            TillageController = null;
            PesticideController = null;
            PhosphorusController = null;
            NitrateController = null;
            SolutesController = null;
            ModelOptionsController = null;
            OutputModelController = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputDataModels"></param>
        public Simulation(List<InputModel> inputDataModels, int startYear = 0, int endYear = 0) : this()
        {
            StartYear = startYear;
            EndYear = EndYear;

            ErrorList = new List<string>();
            ZerosList = new List<string>();

            CanLog = false;

            RunSilent = false;
            Force2011CurveNoFn = false;

            //Simulation has to have a Climate, Soil, Vegetion Controllers/Models
            ClimateController = new ClimateController(this, new List<InputModel>(inputDataModels.Where(x => x.GetType() == typeof(ClimateInputModel))));
            VegetationController = new VegetationController(this, new List<InputModel>(inputDataModels.Where(x => x.GetType().BaseType == (typeof(VegObjectInputDataModel)))));
            SoilController = new SoilController(this, new List<InputModel>(inputDataModels.Where(x => x.GetType() == typeof(SoilInputModel))));

            //Optional Controllers/Models
            IrrigationController = FindInputModels(inputDataModels, typeof(IrrigationInputModel)) == null ? null : new IrrigationController(this, FindInputModels(inputDataModels, typeof(IrrigationInputModel)));
            TillageController = FindInputModels(inputDataModels, typeof(TillageObjectDataModel)) == null ? null : new TillageController(this, FindInputModels(inputDataModels, typeof(TillageObjectDataModel)));
            PesticideController = FindInputModels(inputDataModels, typeof(PesticideObjectDataModel)) == null ? null : new PesticideController(this, FindInputModels(inputDataModels, typeof(PesticideObjectDataModel)));
            PhosphorusController = FindInputModels(inputDataModels, typeof(PhosphorusInputModel)) == null ? null : new PhosphorusController(this, FindInputModels(inputDataModels, typeof(PhosphorusInputModel)));
            NitrateController = FindInputModels(inputDataModels, typeof(NitrateInputDataModel)) == null ? null : new NitrateController(this, FindInputModels(inputDataModels, typeof(NitrateInputDataModel)));
            SolutesController = FindInputModels(inputDataModels, typeof(SolutesInputModel)) == null ? null : new SolutesController(this, FindInputModels(inputDataModels, typeof(SolutesInputModel)));
            //ModelOptionsController = FindInputModels(inputDataModels, typeof(ModelOptionsInputModel)) == null ? null : new ModelOptionsController(this, FindInputModels(inputDataModels, typeof(ModelOptionsInputModel)));
            //There is no XML definition found yet
            ModelOptionsController = new ModelOptionsController(this);

            //Add the non-null controllers to the activecontroller list
            List<PropertyInfo> controllers = new List<PropertyInfo>(this.GetType().GetProperties().Where(
                x => x.PropertyType.BaseType == typeof(HLController) || x.PropertyType.BaseType == typeof(HLObjectController)));

            ActiveControlllers = new List<HLController>();

            ActiveControlllers.Add(this);

            foreach (PropertyInfo p in controllers)
            {
                if (p.GetValue(this) != null)
                {
                    ActiveControlllers.Add((HLController)p.GetValue(this));
                }
            }

            //Instantiate the output controller
            OutputModelController = new OutputModelController(this);

            //Set the start date and end dates
            if (StartYear == 0)
            {
                StartDate = new DateTime(ClimateController.DataModel.StartDate.Value.Ticks);
            }
            else
            {
                StartDate = new DateTime(StartYear, 1, 1);
            }

            if (EndYear == 0)
            {
                EndDate = new DateTime(ClimateController.DataModel.EndDate.Value.Ticks);
            }
            else
            {
                EndDate = new DateTime(EndYear, 12, 31);
            }

            Today = StartDate;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Run()
        {
            ResetToDefault();

            DateTime start = DateTime.Now;

            while (Today <= EndDate)
            {
                this.SimulateDay();
               
                //TODO: Tidy this up - reporting
                Out.Date = Today;
                Out.Rain = ClimateController.Rain;
                Out.MaxTemp = ClimateController.MaxT;
                Out.MinTemp = ClimateController.MinT;
                Out.PanEvap = ClimateController.PanEvap;
                Out.SolarRad = ClimateController.Radiation;

                //Write output and go to next day
                OutputModelController.WriteData();
                Today += new TimeSpan(1, 0, 0, 0);
            }

            //Output all of the summary data
            //OutputModelController.WriteToFile(false);

            OutputModelController.Finalise();

            DateTime end = DateTime.Now;

            TimeSpan ts = end - start;

            Console.WriteLine(ts);

            //Do any necessary cleaup or output
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<InputModel> FindInputModels(List<InputModel> models, Type type)
        {
            List<InputModel> foundModels = new List<InputModel>(models.Where(x => x.GetType() == type));
            if (foundModels.Count == 0)
            {
                return null;
            }
            return foundModels;
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
                if (!FReset) ClimateController.Simulate();
                if (!FReset) AdjustKeyDatesForYear();
                if (!FReset) SetStartOfDayParameters();
                if (!FReset) ApplyResetsIfAny();
                if (!FReset) TryModelIrrigation();
                if (!FReset) SoilController.TryModelSoilCracking();
                if (!FReset) SoilController.CalculateRunoff();
                if (!FReset) SoilController.CalculatSoilEvaporation();
                if (!FReset) TryModelVegetation();
                if (!FReset) SoilController.UpdateWaterBalance();
                if (!FReset) TryModelTillage();
                if (!FReset) SoilController.CalculateResidue();
                if (!FReset) SoilController.CalculateErosion();
                if (!FReset) TryModelRingTank();
                if (!FReset) TryModelPesticide();
                if (!FReset) TryModelPhosphorus();
                if (!FReset) TryModelNitrate();
                if (!FReset) TryModelSolutes();
                if (!FReset) SoilController.TryModelLateralFlow();
                if (!FReset) UpdateCropWaterBalance();
                if (!FReset) SoilController.UpdateFallowWaterBalance();
                if (!FReset) SoilController.UpdateTotalWaterBalance();
                if (!FReset) TryUpdateRingTankWaterBalance();
                if (!FReset) SoilController.UpdateMonthlyStatistics();
                if (!FReset) SoilController.CalculateVolumeBalanceError();
                if (!FReset) ExportDailyOutputs();
                if (!FReset) ResetAnyParametersIfRequired();
            }
            catch (Exception e)
            {
                result = false;

                List<string> Text = new List<string>();
                if (Today > new DateTime(1800, 1, 1) && Today < new DateTime(2100, 1, 1))
                {
                    //Text.Add("There was an error in the simulation on day " + (seriesindex + 1).ToString() + " (" + Today.ToString("dd/mm/yyyy") + ")");
                    Text.Add("There was an error in the simulation on day " + " (" + Today.ToString("dd/mm/yyyy") + ")");
                }
                if (ControlError.Length > 0)
                {
                    Text.Add("The error occurred in the function called " + ControlError);
                }
                if (Text.Count > 0 && Text.Count < 3)
                {
                    //throw (new Exception(String.Join("\n", Text.ToArray(), e.Message))); //mtError
                }
                else
                {
                    throw (new Exception("Error Simulating Day", new Exception(e.Message)));
                }
            }
            return result;

        }

        /// <summary>
        /// 
        /// </summary>
        public void AdjustKeyDatesForYear()
        {
            try
            {
                Day = Today.Day;
                Month = Today.Month;
                Year = Today.Year;
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
                if (SoilController != null)
                {
                    SoilController.SetStartOfDayParameters();
                }
                if (IrrigationController != null)
                {
                    IrrigationController.SetStartOfDayParameters();
                }
                if (VegetationController != null)
                {
                    VegetationController.SetStartOfDayParameters();
                }
                if (TillageController != null)
                {
                    TillageController.SetStartOfDayParameters();
                }
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
            ModelOptionsController.ApplyResetsIfAny(Today);
        }

        /// <summary>
        /// 
        /// </summary>
        public void TryModelIrrigation()
        {
            if (IrrigationController != null)
            {
                IrrigationController.Simulate();
                SoilController.WatBal.Irrigation = IrrigationController.Output.IrrigationApplied;
            }
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
            ModelOptionsController.DataModel.ResetSoilWaterAtDate = false;
            ModelOptionsController.DataModel.ResetResidueAtDate = false;
            ModelOptionsController.DataModel.ResetSoilWaterAtPlanting = false;
            ModelOptionsController.DataModel.CanCalculateLateralFlow = false;
            ModelOptionsController.DataModel.UsePERFECTGroundCovFn = false;
            ModelOptionsController.DataModel.IgnoreCropKill = false;
            ModelOptionsController.DataModel.UsePERFECTDryMatterFn = false;
            ModelOptionsController.DataModel.UsePERFECTLeafAreaFn = false;
            ModelOptionsController.DataModel.UsePERFECT_USLE_LS_Fn = true;
            ModelOptionsController.DataModel.UsePERFECTResidueFn = false;
            ModelOptionsController.DataModel.UsePERFECTSoilEvapFn = false;
            ModelOptionsController.DataModel.UsePERFECTCurveNoFn = Simulation.DEFAULT_CN;
            ModelOptionsController.DataModel.InitialPAW = 0.5;
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
                {
                    FProgress = (int)((double)(sim * daycount + day) / totalcount * 100.0 + 0.5);
                }
                else
                {
                    FProgress = 0;
                }
                if (daycount != 0)
                {
                    FSimProgress = (int)((double)(day) / (double)(daycount) * 100.0 + 0.5);
                }
                else
                {
                    FSimProgress = 0;
                }
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
        public void TryModelVegetation()
        {
            if (VegetationController != null)
            {
                VegetationController.Simulate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void TryModelTillage()
        {
            if (TillageController != null)
            {
                TillageController.Simulate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void TryModelRingTank()
        {
            if (IrrigationController != null)
            {
                IrrigationController.ModelRingTank();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void TryModelPesticide()
        {
            if (PesticideController != null)
            {
                PesticideController.Simulate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void TryModelPhosphorus()
        {
            if (PhosphorusController != null)
            {
                PhosphorusController.Simulate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void TryModelNitrate()
        {
            if (NitrateController != null)
            {
                NitrateController.Simulate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void TryModelSolutes()
        {
            if (SolutesController != null)
            {
                SolutesController.Simulate();
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
        public void TryUpdateRingTankWaterBalance()
        {
            if (IrrigationController != null)
            {
                IrrigationController.UpdateRingtankWaterBalance();
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
            //		}
            //		else
            //		{
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
                if (NumberOfDaysInSimulation > 0)
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

        public bool LoadSoilInputParameters(Object soildata)
        {
            return false;
        }

    }
}
