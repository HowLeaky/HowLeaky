using HowLeaky.DataModels;
using HowLeaky.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HowLeaky.ModelControllers.Veg
{
    public class CoverVegObjectController : VegObjectController
    {
        public CoverVegInputModel InputModel { get; set; }

        public int PanDayindex { get; set; }
        public int PanEvapFactor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="simulation"></param>
        public CoverVegObjectController(Simulation simulation) : base(simulation)
        {
            PredefinedResidue = true;
            VegetationController = simulation.VegetationController;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="simulation"></param>
        /// <param name="dataModel"></param>
        public CoverVegObjectController(Simulation simulation, CoverVegInputModel dataModel) : this(simulation)
        {
            this.InputModel = dataModel;

            InitOutputModel();

            InputModel.InitialiseCoverProfile();

            CalculateMaxRootDepth();
        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculateMaxRootDepth()
        {
            //int max = 0;

            List<double> rootDepths = InputModel.CoverProfile.values["Root Depth"];

            if (rootDepths != null)
            {
                MaximumRootDepth = rootDepths.Max();
            }
            //else
            //{
            //	int count = RootDepthData->NumData;
            //	for(int i=0;i<count;++i)
            //	{
            //                   if (RootDepthData->DataY[i] > max)
            //                   { max = RootDepthData->DataY[i] };
            //	}
            //}
            //MaximumRootDepth = max;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool StillRequiresIrrigating()
        {
            return (CropCover > 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool DoesCropMeetSowingCriteria()
        {
            return (Sim.Today.DayOfYear == InputModel.PlantDay && CropStatus != CropStatus.Growing);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Simulate()
        {
            //InitialisedMeasuredInputs();
            if (DoesCropMeetSowingCriteria())
            {
                Plant();

            }
            InitialisedMeasuredInputs();
            EtPanPhenology();
            //	CalculateRootGrowth();
            CalculateTranspiration();
            CalculateBiomass();
            if (DaysSincePlanting == InputModel.DaysPlantingToHarvest)
            {
                CalculateYield();
            }

            UpdateCropSummary();
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
        public override void Plant()
        {
            base.Plant();
            Sim.FManagementEvent = ManagementEvent.Planting;
            Sim.UpdateManagementEventHistory(ManagementEvent.Planting, Sim.VegetationController.GetCropIndex(this));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override double GetTotalCover()
        {
            TotalCover = ResidueCover * (1 - GreenCover) + GreenCover;
            // was requested by VicDPI to account for animal trampling
            if (TotalCover > InputModel.MaxAllowTotalCover)
            {
                TotalCover = InputModel.MaxAllowTotalCover;
            }
            return TotalCover;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override double GetPotentialSoilEvaporation()
        {
            return Sim.ClimateController.PanEvap * (1.0 - TotalCover * 0.87);
            // Dan Rattray: 0.87 comes from APSIM
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool InitialisedMeasuredInputs()
        {
            if (InputModel.CoverDataType == 0)
            {
                //		UpdatePanDayIndex();
                //GreenCover = DataModel.CoverProfile.GetValueForDayIndex("Green Cover", PanDayindex, Sim.Today) / 100.0 * DataModel.GreenCoverMultiplier;
                //ResidueCover = DataModel.CoverProfile.GetValueForDayIndex("Residue Cover", PanDayindex, Sim.Today) / 100.0 * DataModel.ResidueCoverMultiplier;
                //Output.RootDepth = DataModel.CoverProfile.GetValueForDayIndex("Root Depth", PanDayindex, Sim.Today) * DataModel.RootDepthMultiplier;
                InputModel.CoverProfile.UpdateDayIndex(Sim.Today);
                GreenCover = InputModel.CoverProfile.GetValueForDayIndex("Green Cover", Sim.Today) / 100.0 * InputModel.GreenCoverMultiplier;
                ResidueCover = InputModel.CoverProfile.GetValueForDayIndex("Residue Cover", Sim.Today) / 100.0 * InputModel.ResidueCoverMultiplier;
                RootDepth = InputModel.CoverProfile.GetValueForDayIndex("Root Depth", Sim.Today) * InputModel.RootDepthMultiplier;

                CropCover = GreenCover;
            }
            else
            {
                if (InputModel.GreenCoverTimeSeries.GetCount() > 0 && InputModel.ResidueCoverTimeSeries.GetCount() > 0 && InputModel.RootDepthTimeSeries.GetCount() > 0)
                {
                    double greenbiomass = InputModel.GreenCoverTimeSeries.GetValueAtDate(Sim.Today);
                    double residuebiomass = InputModel.ResidueCoverTimeSeries.GetValueAtDate(Sim.Today);
                    double rootbiomass = InputModel.RootDepthTimeSeries.GetValueAtDate(Sim.Today);
                    if (!MathTools.DoublesAreEqual(greenbiomass, MathTools.MISSING_DATA_VALUE))
                    {
                        GreenCover = 1.0 * (1 - Math.Exp(-greenbiomass * InputModel.GreenCoverMultiplier));
                    }
                    else
                    {
                        GreenCover = 0;
                    }

                    CropCover = GreenCover;

                    if (!MathTools.DoublesAreEqual(residuebiomass, MathTools.MISSING_DATA_VALUE))
                    {
                        ResidueCover = 1.0 * (1 - Math.Exp(-residuebiomass * InputModel.ResidueCoverMultiplier));
                    }
                    else
                    {
                        ResidueCover = 0;
                    }
                    if (!MathTools.DoublesAreEqual(rootbiomass, MathTools.MISSING_DATA_VALUE))
                    {
                        RootDepth = InputModel.MaxRootDepth * (1 - Math.Exp(-rootbiomass * InputModel.RootDepthMultiplier));
                    }
                    else
                    {
                        RootDepth = 0;
                    }
                }
                else
                {
                    GreenCover = 0;
                    CropCover = 0;
                    ResidueCover = 0;
                    RootDepth = 0;
                }
            }
            if (ResidueCover > 1)
            {
                ResidueCover = 1;
            }
            if (CropCover > 1)
            {
                CropCover = 1;
            }
            if (GreenCover > 1)
            {
                GreenCover = 1;
            }

            if (ResidueCover < 0)
            {
                ResidueCover = 0;
            }
            if (GreenCover < 0)
            {
                GreenCover = 0;
            }
            if (CropCover < 0)
            {
                CropCover = 0;
            }

            Sim.SoilController.TotalResidueCover = ResidueCover;
            Sim.SoilController.TotalResidueCoverPercent = ResidueCover * 100.0;
            TotalCover = GetTotalCover();
            Yield = 0;

            //REVIEW
            //Output.GreenCover = GreenCover * 100.0;
            //CropCoverPercent = CropCover * 100.0;
            //Output.TotalCover = TotalCover * 100.0;
            //Output.ResidueCover = ResidueCover * 100.0;

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override double CalculatePotentialTranspiration()
        {
            double cf = 1;
            if (RootDepth > 0)
            {
                return Math.Min(GreenCover * Sim.ClimateController.PanEvap * cf, Sim.ClimateController.PanEvap - Sim.SoilController.SoilEvap);
            }
            return 0;
        }

        //void  UpdatePanDayIndex()
        //{
        //	std::vector<int> jdays=in_CoverProfile.jdays;
        //	std::vector<double> residuedata=in_CoverProfile.Values["Residue Cover"];
        //	int i=residuedata.size()-1;
        //	if(i>=0)
        //	{
        //		int dayno=sim.today-TDateTime(YearOf(sim.today),1,1)+1;
        //		if(jdays[i]<=366)
        //		{
        //			pandayindex=dayno;
        //		}
        //		else
        //		{
        //			int nolaps= int(residuedata[i]/366)+1;
        //			int resetvalue=nolaps*365;
        //			if(pandayindex>=resetvalue&&dayno==1)
        //				pandayindex=1;
        //			else
        //				pandayindex++;
        //		}
        //	}
        //}


        //double  GetRootDepth(const int&  dayindex)
        //{
        //	return in_CoverProfile.GetValueForDayIndex("Root Depth",pandayindex,sim.today);
        //}

        /// <summary>
        /// 
        /// </summary>
        public void EtPanPhenology()
        {

            if (CropStatus == ModelControllers.CropStatus.Growing)
            {
                ++DaysSincePlanting;
                if (InputModel.DaysPlantingToHarvest != 0)
                {
                    CropStage = 3.0 * DaysSincePlanting / (double)(InputModel.DaysPlantingToHarvest);
                }
                else
                {
                    CropStage = 0;
                    MathTools.LogDivideByZeroError("EtPanPhenology", "in_DaysPlantToHarvest_days", "crop_stage");
                }
                if (CropStage >= 3.0)
                {
                    CropStage = 3.0;
                    //CropHarvest=true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculateBiomass()
        {
            DryMatter += InputModel.TranspirationEfficiency * TotalTranspiration;
            //Output.DryMatter = DryMatter;   //this used to be multiplied by 10, removed 07/03/2013
        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateYield()
        {
            Yield = InputModel.HarvestIndex * DryMatter; //this used to be multiplied by 10, removed
            //REVIEW
            //Output.Yield = Yield / 1000.0;
            CumulativeYield += Yield;
            ++HarvestCount;
            CropStatus = ModelControllers.CropStatus.Fallow;
            DaysSincePlanting = 0;
        }
    }
}
