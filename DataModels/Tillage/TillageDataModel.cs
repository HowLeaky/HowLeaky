using HowLeaky.CustomAttributes;
using HowLeaky.ModelControllers;
using HowLeaky.Tools.DataObjects;
using HowLeaky.Models;
using System.Xml.Serialization;
using HowLeakyWebsite.Tools.DHMCoreLib.Helpers;
using HowLeaky.XmlObjects;

namespace HowLeaky.DataModels
{
    public enum ETillageType { ttNone, ttBurn, ttDisc, ttScarify, ttSweep, ttChisel, ttPlant, ttBlade, ttRodWeeder, ttZeroTill, ttUser1, ttUser2 };
    public enum ETillageFormat { tfTillInWindow, tfFixedDate, tfAtPlantingAllCrop, tfAtPlantingCrop1, tfAtPlantingCrop2, tfAtPlantingCrop3, tfAtHarvestAllCrop, tfAtHarvestCrop1, tfAtHarvestCrop2, tfAtHarvestCrop3, tfFromSequenceFile };

    //Class for XML Serialisation
    public class TillageTypeData : IndexData
    {
        public double PrimaryCropResMultiplier { get; set; }
        public double PrimaryRoughnessRatio { get; set; }

        public TillageTypeData() { }
    }

    [XmlRoot("TillageType")]
    public class TillageObjectDataModel : DataModel
    {
        public DayMonthData StartTillWindow { get; set; }
        public DayMonthData EndTillWindow { get; set; }
        public DayMonthData PrimaryTillDate { get; set; }
        public DayMonthData SecondaryTillDate1 { get; set; }
        public DayMonthData SecondaryTillDate2 { get; set; }
        public DayMonthData SecondaryTillDate3 { get; set; }
        public Sequence PrimaryTillageDates { get; set; }

        public double CropResidueMultiplier { get; set; }
        public double RoughnessRatio { get; set; }
        public double RainForPrimaryTill { get; set; }
        public int NoDaysToTotalRain { get; set; }
        public int MinDaysBetweenTills { get; set; }

        public TillageTypeData PrimaryTillType { get; set; }
        public IndexData TillageFormat { get; set; }

        [XmlIgnore]
        public int Format { get { return TillageFormat.index; } }
        [XmlIgnore]
        public int Type { get { return PrimaryTillType.index; } }

        public TillageObjectDataModel() { }
    }

    public class TillageObjectController:HLObject
    {
        public TillageObjectDataModel dataModel { get; set; }
       
        public TillageController TillageController { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TillageObjectController() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sim"></param>
        public TillageObjectController(Simulation sim) : base(sim)
        {
            TillageController = sim.TillageController;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Simulate()
        {
            if (CanTillToday())
            {
                TillageController.UpdateTillageParameters((ETillageType)dataModel.Type, dataModel.CropResidueMultiplier, dataModel.RoughnessRatio);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CanTillToday()
        {
            switch (dataModel.Format)
            {
                case (int)DataModels.ETillageFormat.tfTillInWindow: return IsFallowAndInWindow();
                case (int)DataModels.ETillageFormat.tfFixedDate: return IsFallowAndDate();
                case (int)DataModels.ETillageFormat.tfAtPlantingAllCrop: return IsPlantDay();
                case (int)DataModels.ETillageFormat.tfAtPlantingCrop1: return IsPlantDayForCrop(0);
                case (int)DataModels.ETillageFormat.tfAtPlantingCrop2: return IsPlantDayForCrop(1);
                case (int)DataModels.ETillageFormat.tfAtPlantingCrop3: return IsPlantDayForCrop(2);
                case (int)DataModels.ETillageFormat.tfAtHarvestAllCrop: return IsHarvestDay();
                case (int)DataModels.ETillageFormat.tfAtHarvestCrop1: return IsHarvestDayForCrop(0);
                case (int)DataModels.ETillageFormat.tfAtHarvestCrop2: return IsHarvestDayForCrop(1);
                case (int)DataModels.ETillageFormat.tfAtHarvestCrop3: return IsHarvestDayForCrop(2);
                case (int)DataModels.ETillageFormat.tfFromSequenceFile: return IsFallowAndInSequence();
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsFallowAndInWindow()
        {
            bool check1 = sim.VegetationController.InFallow();
            bool check2 = DateUtilities.isDateInWindow(sim.today, dataModel.StartTillWindow, dataModel.EndTillWindow);
            bool check3 = (TillageController.days_since_tillage >= dataModel.MinDaysBetweenTills || TillageController.days_since_tillage == -1);
            bool check4 = (sim.SumRain(dataModel.NoDaysToTotalRain, 0) >= dataModel.RainForPrimaryTill);
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
            bool check1 = sim.VegetationController.InFallow();
            bool check2 = (dataModel.PrimaryTillDate.MatchesDate(sim.today) || dataModel.SecondaryTillDate1.MatchesDate(sim.today) || dataModel.SecondaryTillDate2.MatchesDate(sim.today) || dataModel.SecondaryTillDate3.MatchesDate(sim.today));
            return check1 && check2;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsPlantDay()
        {
            VegObjectController CurrentCrop = sim.VegetationController.CurrentCrop;
            if (CurrentCrop != null)
                return CurrentCrop.today_is_harvest_day;
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsPlantDayForCrop(int id)
        {
            VegObjectController CurrentCrop = sim.VegetationController.CurrentCrop;
            if (CurrentCrop != null)
                return CurrentCrop.CropIndex == id && CurrentCrop.today_is_harvest_day;
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsHarvestDay()
        {
            VegObjectController CurrentCrop = sim.VegetationController.CurrentCrop;
            if (CurrentCrop != null)
                return CurrentCrop.TodayIsPlantDay;
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsHarvestDayForCrop(int id)
        {
            VegObjectController CurrentCrop = sim.VegetationController.CurrentCrop;
            if (CurrentCrop != null)
                return CurrentCrop.CropIndex == id && CurrentCrop.TodayIsPlantDay;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsFallowAndInSequence()
        {
            return sim.VegetationController.InFallow() && dataModel.PrimaryTillageDates.ContainsDate(sim.today);
        }
    }
}

