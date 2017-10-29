using HowLeaky.Tools.DataObjects;
using HowLeaky.XmlObjects;
using System.Xml.Serialization;

namespace HowLeaky.DataModels
{

    public class DissolvedNinRunoffOptions : IndexData
    {
        public double NDepthTopLayer1 { get; set; }
        public double Nk { get; set; }
        public double Ncv { get; set; }
        public double NAlpha { get; set; }
        public double NitrateSourceData { get; set; }
        public double SoilNitrateLoadWeighting1 { get; set; }
        public double N_DanRat_Alpha { get; set; } 
        public double N_DanRat_Beta { get; set; }
        public double N_DanRat_MaxRunOffConc { get; set; }
        public double N_DanRat_MinRunOffConc { get; set; }
        public Sequence FertilizerInputDateSequences { get; set; }

        public DissolvedNinRunoffOptions() { }
    }

    public class DissolvedNinLeachingOptions : IndexData
    {
        public double DepthBottomLayer { get; set; }
        public double NitrateLeachingEfficiency { get; set; }
        public NitrateSourceData NitrateSourceData { get; set; }
        public double SoilNitrateLoadWeighting2 { get; set; }

        public DissolvedNinLeachingOptions() { }
    }

    public class ParticulateNinRunoffOptions : IndexData
    {
        public double NDepthTopLayer2 { get; set; }
        public double NEnrichmentRatio { get; set; }
        public double NAlpha { get; set; }
        public double NBeta { get; set; }
        public NitrateSourceData NitrateSourceData { get; set; }
        public double SoilNitrateLoadWeighting3 { get; set; }

        public ParticulateNinRunoffOptions() { }
    }

    public class NitrateSourceData : IndexData
    {
        public TimeSeriesData InorganicNitrateNTimeseries { get; set; }
        public TimeSeriesData InorganicAmmoniumNTimeseries { get; set; }
        public TimeSeriesData OrganicNTimeseries { get; set; }
        public TimeSeriesData SoilNitrateTimeseries { get; set; }

        public NitrateSourceData() { }
    }

    [XmlRoot("NitratesType")]
    public class NitrateDataModel : DataModel
    {
        [XmlElement("DissolvedNinRunoffOptions")]
        public DissolvedNinRunoffOptions DissolvedNinRunoff { get; set; }
        [XmlElement("DissolvedNinLeachingOptions")]
        public DissolvedNinLeachingOptions DissolvedNinLeaching { get; set; }
        [XmlElement("ParticulateNinRunoffOptions")]
        public ParticulateNinRunoffOptions ParticulateNinRunoff { get; set; } 


        public int DissolvedNInRunoffInputOptions { get; set; }              //
        public double Nk { get; set; }                                       // A parameter that regulates mixing of soil and runoff water.
        public double Ncv { get; set; }                                  // A parameter that describes the curvature of change in soil and water runoff at increasing runoff values.
        public double NAlpha { get; set; }                                   // A dissolved N conversion factor that can be used for calibration.
        public double NDepthTopLayer1_mm { get; set; }                       // Layer depth for which Dissolved N in Runoff is effective.
        public ProfileData SoilNLoadData1 { get; set; }                            //
        public TimeSeriesData NLoadInSurfaceLayerTimeSeries { get; set; }          //
        public double SoilNLoadWeighting1 { get; set; }                  //
        public int DissolvedNInLeachingInputOptions { get; set; }        //
        public double NDepthBottomLayer_mm { get; set; }                     // Depth of bottom layer used to defined Nitrate soil Concentration.
        public double NLeachingEfficiency_pc { get; set; }               //
        public ProfileData SoilNLoadData2 { get; set; }                            //
        public TimeSeriesData NLoadInLowerLayersTimeSeries { get; set; }           //
        public double SoilNiLoadWeighting2 { get; set; }                 //
        public int ParticulateNInRunoffInputOptions { get; set; }        //
        public double NDepthTopLayer2_mm { get; set; }                       // Layer depth for which Dissolved N in Runoff is effective.
        public double NEnrichmentRatio { get; set; }                     //
        public double NAlpha2 { get; set; }                              // A dissolved N conversion factor that can be used for calibration.
        public double NBeta { get; set; }                                    // A particulate N conversion factor to adjust units and that can be used as a calibration factor.
        public ProfileData SoilNLoadData3 { get; set; }                            //
        public TimeSeriesData SoilInorganicNitrateNTimeSeries { get; set; }        //
        public TimeSeriesData SoilInorganicAmmoniumNTimeSeries { get; set; }       //
        public TimeSeriesData SoilOrganicNTimeSeries { get; set; }                 //
        public double SoilNLoadWeighting3 { get; set; }                  //
    }
}
