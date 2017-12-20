using HowLeaky.DataModels;
using HowLeaky.ModelControllers.Veg;
using HowLeaky.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowLeaky.ModelControllers.Tillage
{
    public class TillageObjectController : HLController
    {
        public TillageObjectDataModel DataModel { get; set; }
        public TillageController TillageController { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TillageObjectController() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sim"></param>
        public TillageObjectController(Simulation sim, TillageObjectDataModel dataModel) : base(sim)
        {
            TillageController = sim.TillageController;
            this.DataModel = dataModel;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Simulate()
        {
            if (CanTillToday())
            {
                TillageController.UpdateTillageParameters((ETillageType)DataModel.Type, DataModel.CropResidueMultiplier, DataModel.RoughnessRatio);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanTillToday()
        {
            switch (DataModel.Format)
            {
                case (int)DataModels.ETillageFormat.TillInWindow: return IsFallowAndInWindow();
                case (int)DataModels.ETillageFormat.FixedDate: return IsFallowAndDate();
                case (int)DataModels.ETillageFormat.AtPlantingAllCrop: return IsPlantDay();
                case (int)DataModels.ETillageFormat.AtPlantingCrop1: return IsPlantDayForCrop(0);
                case (int)DataModels.ETillageFormat.AtPlantingCrop2: return IsPlantDayForCrop(1);
                case (int)DataModels.ETillageFormat.AtPlantingCrop3: return IsPlantDayForCrop(2);
                case (int)DataModels.ETillageFormat.AtHarvestAllCrop: return IsHarvestDay();
                case (int)DataModels.ETillageFormat.AtHarvestCrop1: return IsHarvestDayForCrop(0);
                case (int)DataModels.ETillageFormat.AtHarvestCrop2: return IsHarvestDayForCrop(1);
                case (int)DataModels.ETillageFormat.AtHarvestCrop3: return IsHarvestDayForCrop(2);
                case (int)DataModels.ETillageFormat.FromSequenceFile: return IsFallowAndInSequence();
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsFallowAndInWindow()
        {
            bool check1 = Sim.VegetationController.InFallow();
            bool check2 = DateUtilities.isDateInWindow(Sim.Today, DataModel.StartTillWindow, DataModel.EndTillWindow);
            bool check3 = (TillageController.DaysSinceTillage >= DataModel.MinDaysBetweenTills || TillageController.DaysSinceTillage == -1);
            bool check4 = (Sim.ClimateController.SumRain(DataModel.NoDaysToTotalRain, 0) >= DataModel.RainForPrimaryTill);
            return check1 && check2 && check3 && check4;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        public void AdjustKeyDatesForYear(int year)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsFallowAndDate()
        {
            bool check1 = Sim.VegetationController.InFallow();
            bool check2 = (DataModel.PrimaryTillDate.MatchesDate(Sim.Today) || DataModel.SecondaryTillDate1.MatchesDate(Sim.Today) || DataModel.SecondaryTillDate2.MatchesDate(Sim.Today) || DataModel.SecondaryTillDate3.MatchesDate(Sim.Today));
            return check1 && check2;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsPlantDay()
        {
            VegObjectController CurrentCrop = Sim.VegetationController.CurrentCrop;
            if (CurrentCrop != null)
            {
                return CurrentCrop.TodayIsHarvestDay;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsPlantDayForCrop(int id)
        {
            VegObjectController CurrentCrop = Sim.VegetationController.CurrentCrop;
            if (CurrentCrop != null)
            {
                return CurrentCrop.CropIndex == id && CurrentCrop.TodayIsHarvestDay;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsHarvestDay()
        {
            VegObjectController CurrentCrop = Sim.VegetationController.CurrentCrop;
            if (CurrentCrop != null)
            {
                return CurrentCrop.TodayIsPlantDay;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsHarvestDayForCrop(int id)
        {
            VegObjectController CurrentCrop = Sim.VegetationController.CurrentCrop;
            if (CurrentCrop != null)
            {
                return CurrentCrop.CropIndex == id && CurrentCrop.TodayIsPlantDay;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsFallowAndInSequence()
        {
            return Sim.VegetationController.InFallow() && DataModel.PrimaryTillageDates.ContainsDate(Sim.Today);
        }
    }
}
