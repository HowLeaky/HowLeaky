using HowLeaky.Tools.DataObjects;
using System.Xml.Serialization;
using HowLeaky.XmlObjects;

namespace HowLeaky.DataModels
{
    public enum ETillageType { None, Burn, Disc, Scarify, Sweep, Chisel, Plant, Blade, RodWeeder, ZeroTill, User1, User2 };
    public enum ETillageFormat { TillInWindow, FixedDate, AtPlantingAllCrop, AtPlantingCrop1, AtPlantingCrop2, AtPlantingCrop3, AtHarvestAllCrop, AtHarvestCrop1, AtHarvestCrop2, AtHarvestCrop3, FromSequenceFile };

    //Class for XML Serialisation
    public class TillageTypeData : IndexData
    {
        public double PrimaryCropResMultiplier { get; set; }
        public double PrimaryRoughnessRatio { get; set; }

        public TillageTypeData() { }
    }

    [XmlRoot("TillageType")]
    public class TillageInputModel : InputModel
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

        public TillageInputModel() { }
    }
}

