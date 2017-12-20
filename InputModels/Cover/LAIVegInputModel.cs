using HowLeaky.Tools.DataObjects;
using HowLeaky.XmlObjects;

namespace HowLeaky.DataModels
{
    public enum EPlantingRules { PlantInWindow, FixedAnualPlaning, PlantFromSequenceFile };

    public class PlantingFormat : IndexData
    {
        public DayMonthData StartPlantWindow { get; set; }
        public DayMonthData EndPlantWindow { get; set; }
        public DayMonthData PlantDate { get; set; }
        public StateData ForcePlanting { get; set; }
        public StateData MultiPlantInWindow { get; set; }
        public RotationOptions RotationOptions { get; set; }
        public Sequence PlantingDates { get; set; }
        public FallowSwitch FallowSwitch { get; set; }
        public RainfallSwitch RainfallSwitch { get; set; }
        public SoilWaterSwitch SoilWaterSwitch { get; set; }
        public RatoonCrop RatoonCrop { get; set; }

        public PlantingFormat() { }
    }

    public class RotationOptions : IndexData
    {
        public int MinContinuousRotations { get; set; }
        public int MaxContinuousRotations { get; set; }
        public int MinYearsBetweenSowing { get; set; }

        public RotationOptions() { }
    }

    public class FallowSwitch : StateData
    {
        public int MinFallowLength { get; set; }

        public FallowSwitch() { }
    }

    public class RainfallSwitch : StateData
    {
        public double PlantingRain { get; set; }
        public int DaysToTotalRain { get; set; }
        public int SowingDelay { get; set; }

        public RainfallSwitch() { }
    }

    public class SoilWaterSwitch : StateData
    {
        public double MinSoilWaterRatio { get; set; }
        public double MaxSoilWaterRatio { get; set; }
        public double AvailSWAtPlanting { get; set; }
        public double SoilDepthToSumPlantingSW { get; set; }

        public SoilWaterSwitch() { }
    }

    public class RatoonCrop : StateData
    {
        public int RatoonCount { get; set; }
        public double RatoonScaleFactor { get; set; }

        public RatoonCrop() { }

    }
    public class Waterlogging : StateData
    {
        public double WaterLoggingFactor1 { get; set; }
        public double WaterLoggingFactor2 { get; set; }

        public Waterlogging() { }
    }


   
}
