using HowLeaky.CustomAttributes;
using HowLeaky.Tools.DataObjects;
using HowLeaky.XmlObjects;
using System.Xml.Serialization;

namespace HowLeaky.DataModels
{
    public enum NitrateModelSubType { None, None2, UserDefinedProfile, RattrayEmpericalModel }

    [XmlRoot("NitratesType")]
    public class NitrateInputModel : InputModel
    {
        [XmlElement("DissolvedNinRunoffOptions")]
        [Input("DissolvedNinRunoff")]
        public DissolvedNinRunoffOptions DissolvedNinRunoff { get; set; }
        [XmlElement("DissolvedNinLeachingOptions")]
        [Input("DissolvedNinLeaching")]
        public DissolvedNinLeachingOptions DissolvedNinLeaching { get; set; }
        [XmlElement("ParticulateNinRunoffOptions")]
        [Input("ParticulateNinRunoff")]
        public ParticulateNinRunoffOptions ParticulateNinRunoff { get; set; }

        //public ProfileData SoilNLoadData2 { get; set; }                             //
        //public ProfileData SoilNLoadData1 { get; set; }                             //
        //public ProfileData SoilNLoadData3 { get; set; }                             //
        //public TimeSeriesData NLoadInLowerLayersTimeSeries { get; set; }            //
        //public TimeSeriesData NLoadInSurfaceLayerTimeSeries { get; set; }           //
        //public TimeSeriesData SoilInorganicNitrateNTimeSeries { get; set; }         //
        //public TimeSeriesData SoilInorganicAmmoniumNTimeSeries { get; set; }        //
        //public TimeSeriesData SoilOrganicNTimeSeries { get; set; }                  //

        //public int DissolvedNInRunoffInputOptions { get; set; }                     //
        //public int DissolvedNInLeachingInputOptions { get; set; }                   //
        //public int ParticulateNInRunoffInputOptions { get; set; }                   //

        //public double Nk { get; set; }                                              // A parameter that regulates mixing of soil and runoff water.
        //public double Ncv { get; set; }                                             // A parameter that describes the curvature of change in soil and water runoff at increasing runoff values.
        //public double NAlpha { get; set; }                                          // A dissolved N conversion factor that can be used for calibration.
        //[Unit("mm")]
        //public double NDepthTopLayer1 { get; set; }                                 // Layer depth for which Dissolved N in Runoff is effective.
        //public double SoilNLoadWeighting1 { get; set; }                             //
        //[Unit("mm")]
        //public double NDepthBottomLayer { get; set; }                               // Depth of bottom layer used to defined Nitrate soil Concentration.
        //[Unit("pc")]
        //public double NLeachingEfficiency { get; set; }                             //
        //public double SoilNiLoadWeighting2 { get; set; }                            //
        //[Unit("mm")]
        //public double NDepthTopLayer2 { get; set; }                                 // Layer depth for which Dissolved N in Runoff is effective.
        //public double NEnrichmentRatio { get; set; }                                //
        //public double NAlpha2 { get; set; }                                         // A dissolved N conversion factor that can be used for calibration.
        //public double NBeta { get; set; }                                           // A particulate N conversion factor to adjust units and that can be used as a calibration factor.
        //public double SoilNLoadWeighting3 { get; set; }                             //

        public NitrateInputModel() { }
    }
    /// <summary>
    /// 
    /// </summary>
    public class DissolvedNinRunoffOptions : IndexData
    {
        [Input("NDepthTopLayer1")]
        public Sequence FertilizerInputDateSequences { get; set; } = null;
        [Input("NDepthTopLayer1")]
        public TimeSeriesData NLoadInSurfaceLayerTimeSeries { get; set; } = null;

        [Input("NDepthTopLayer1")]
        public double NDepthTopLayer1 { get; set; }
        [Input("Nk")]
        public double Nk { get; set; }
        [Input("Ncv")]
        public double Ncv { get; set; }
        [Input("NAlpha")]
        public double NAlpha { get; set; }
        //public double NitrateSourceData { get; set; }  //Has child SoilNitrateTimeseries
        [Input("SoilNitrateLoadWeighting1")]
        public double SoilNitrateLoadWeighting1 { get; set; }
        [XmlArrayItem("Data")]
        [Input("SoilNitrateLevels")]
        public Data[] SoilNitrateLevels { get; set; } = null;
        [XmlIgnore]
        [Input("SoilNLoadData1")]
        public ProfileData SoilNLoadData1 { get; set; } = null;
        [Input("N_DanRat_Alpha")]
        public double N_DanRat_Alpha { get; set; }
        [Input("N_DanRat_Beta")]
        public double N_DanRat_Beta { get; set; }
        [Input("N_DanRat_MaxRunOffConc")]
        public double N_DanRat_MaxRunOffConc { get; set; }
        [Input("N_DanRat_MinRunOffConc")]
        public double N_DanRat_MinRunOffConc { get; set; }
       
        public DissolvedNinRunoffOptions() { }
    }
    /// <summary>
    /// 
    /// </summary>
    public class DissolvedNinLeachingOptions : IndexData
    {
        //       public NitrateSourceData NitrateSourceData { get; set; }
        [XmlArrayItem("Data")]
        [Input("SoilNitrateLevels")]
        public Data[] SoilNitrateLevels { get; set; } = null;
        [XmlIgnore]
        [Input("SoilNLoadData2")]
        public ProfileData SoilNLoadData2 { get; set; } = null;
        [Input("DepthBottomLayer")]
        public double DepthBottomLayer { get; set; }
        [Input("NitrateLeachingEfficiency")]
        public double NitrateLeachingEfficiency { get; set; }
        [Input("SoilNitrateLoadWeighting2")]
        public double SoilNitrateLoadWeighting2 { get; set; }
        //SoilNitrateLevels

        [Input("NLoadInLowerLayersTimeSeries")]
        public TimeSeriesData NLoadInLowerLayersTimeSeries { get; set; } = null;

        public DissolvedNinLeachingOptions() { }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ParticulateNinRunoffOptions : IndexData
    {
        public NitrateSourceData NitrateSourceData { get; set; }

        [Input("NDepthTopLayer2")]
        public double NDepthTopLayer2 { get; set; }
        [Input("NEnrichmentRatio")]
        public double NEnrichmentRatio { get; set; }
        [Input("NAlpha")]
        public double NAlpha { get; set; }
        [Input("NBeta")]
        public double NBeta { get; set; }
        [Input("SoilNitrateLoadWeighting3")]
        public double SoilNitrateLoadWeighting3 { get; set; }
        [XmlArrayItem("Data")]
        [Input("SoilNitrateLevels")]
        public Data[] SoilNitrateLevels { get; set; } = null;
        [XmlIgnore]
        [Input("SoilNLoadData3")]
        public ProfileData SoilNLoadData3 { get; set; } = null;

        public ParticulateNinRunoffOptions() { }
    }
    /// <summary>
    /// 
    /// </summary>
    public class NitrateSourceData : IndexData
    {
        [Input("InorganicNitrateNTimeseries")]
        public TimeSeriesData InorganicNitrateNTimeseries { get; set; }
        [Input("InorganicAmmoniumNTimeseries")]
        public TimeSeriesData InorganicAmmoniumNTimeseries { get; set; }
        [Input("OrganicNTimeseries")]
        public TimeSeriesData OrganicNTimeseries { get; set; }
        [Input("SoilNitrateTimeseries")]
        public TimeSeriesData SoilNitrateTimeseries { get; set; }

        public NitrateSourceData() { }
    }
}
//test commnet 3rd Jul19