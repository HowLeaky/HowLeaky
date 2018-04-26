using HowLeaky.Tools.DataObjects;
using System.Xml.Serialization;
using HowLeaky.XmlObjects;
using HowLeaky.CustomAttributes;

namespace HowLeaky.DataModels
{
    public enum ETillageType { None, Burn, Disc, Scarify, Sweep, Chisel, Plant, Blade, RodWeeder, ZeroTill, User1, User2 };
    public enum ETillageFormat { TillInWindow, FixedDate, AtPlantingAllCrop, AtPlantingCrop1, AtPlantingCrop2, AtPlantingCrop3, AtHarvestAllCrop, AtHarvestCrop1, AtHarvestCrop2, AtHarvestCrop3, FromSequenceFile };

    //Class for XML Serialisation
    public class TillageTypeData : IndexData
    {
        [Input("PrimaryCropResMultiplier")]
        public double PrimaryCropResMultiplier { get; set; }
        [Input("PrimaryRoughnessRatio")]
        public double PrimaryRoughnessRatio { get; set; }

        public TillageTypeData() { }
    }

    [XmlRoot("TillageType")]
    public class TillageInputModel : InputModel
    {
        [Input("StartTillWindow")]
        public DayMonthData StartTillWindow { get; set; }
        [Input("EndTillWindow")]
        public DayMonthData EndTillWindow { get; set; }
        [Input("PrimaryTillDate")]
        public DayMonthData PrimaryTillDate { get; set; }
        [Input("SecondaryTillDate1")]
        public DayMonthData SecondaryTillDate1 { get; set; }
        [Input("SecondaryTillDate2")]
        public DayMonthData SecondaryTillDate2 { get; set; }
        [Input("SecondaryTillDate3")]
        public DayMonthData SecondaryTillDate3 { get; set; }
        [Input("PrimaryTillageDates")]
        public Sequence PrimaryTillageDates { get; set; }

        [Input("CropResidueMultiplier")]
        public double CropResidueMultiplier { get; set; }
        [Input("RoughnessRatio")]
        public double RoughnessRatio { get; set; }
        [Input("RainForPrimaryTill")]
        public double RainForPrimaryTill { get; set; }
        [Input("NoDaysToTotalRain")]
        public int NoDaysToTotalRain { get; set; }
        [Input("MinDaysBetweenTills")]
        public int MinDaysBetweenTills { get; set; }

        [Input("PrimaryTillType")]
        public TillageTypeData PrimaryTillType { get; set; }
        [Input("TillageFormat")]
        public IndexData TillageFormat { get; set; }

        [XmlIgnore]
        public int Format { get { return TillageFormat.index; } }
        [XmlIgnore]
        public int Type { get { return PrimaryTillType.index; } }

        public TillageInputModel() { }
    }
}

