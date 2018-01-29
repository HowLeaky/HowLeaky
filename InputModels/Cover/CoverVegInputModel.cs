using HowLeaky.Tools.DataObjects;
using System.Xml.Serialization;
using System.Xml;
using HowLeaky.XmlObjects;
using HowLeaky.CustomAttributes;

namespace HowLeaky.DataModels
{
    public class Data
    {
        [XmlAttribute("x")]
        public double X { get; set; }
        [XmlAttribute("y")]
        public double Y { get; set; }
        [XmlAttribute("z")]
        public double Z { get; set; }
        [XmlAttribute("a")]
        public double A { get; set; }

        public Data() { }
    }

    [XmlRoot("VegetationType")]
    public class CoverVegObjectDataModel : VegObjectInputDataModel
    {
        //Input Parameters
        public IndexData CoverInputOptions { get; set; }
        public IndexData ModelType { get; set; }
        public IndexData SourceData { get; set; }
        [XmlArray("CropFactorMatrix")]
        [XmlArrayItem("Data")]
        public Data[] CropFactorMatrix { get; set; }
        [XmlElement("PanPlantDay")]
        public int PlantDay { get; set; }
        [XmlIgnore]
        public int CoverDataType { get; set; } = 0; //for no time series
        [XmlIgnore]
        public ProfileData CoverProfile { get; set; }
        [XmlElement("LinkToGreenCover")]
        public TimeSeriesData GreenCoverTimeSeries { get; set; }
        [XmlElement("LinkToResidueCover")]
        public TimeSeriesData ResidueCoverTimeSeries { get; set; }
        [XmlElement("LinkToRootDepth")]
        public TimeSeriesData RootDepthTimeSeries { get; set; }
        [XmlElement("WaterUseEffic")]
        public double TranspirationEfficiency { get; set; }     // The ratio of grain production (kg/ha) to water supply (mm).
        [XmlElement("PanHarvestIndex")]
        public double HarvestIndex { get; set; }                // The grain biomass (kg/ha) divided by the above-ground biomass at flowering (kg/ha).
        [Unit("days")]
        public int DaysPlantingToHarvest { get; set; }          // The number of days between planting and harvest.
        [XmlElement("GreenBioMassToCoverFactor")]
        public double GreenCoverMultiplier { get; set; }        // Scaling factor for green cover
        [XmlElement("ResidueBioMassToCoverFactor")]
        public double ResidueCoverMultiplier { get; set; }      // Scaling factor for residue cover
        [XmlElement("RootBioMassToDepthFactor")]
        public double RootDepthMultiplier { get; set; }         // Scaling factor for root depth
        public double MaxAllowTotalCover { get; set; }          // Maximum allowable total cover
        [Unit("mm")]
        public double MaxRootDepth { get; set; }  // located in CustomVegObject - >The maximum depth of the roots from the soil surface.  For the LAI model, the model calculates daily root growth from the root depth increase parameter
        public double SWPropForNoStress { get; set; }

        public CoverVegObjectDataModel()
        {
            //InitialiseCoverProfile();
        }

        public void InitialiseCoverProfile()
        {
            CoverProfile = new ProfileData();

            foreach(Data d in CropFactorMatrix)
            {
                CoverProfile.AddDate((int)d.X);
                CoverProfile.AddValue("Green Cover", d.Y);
                CoverProfile.AddValue("Residue Cover", d.Z);
                CoverProfile.AddValue("Root Depth", d.A);

            }
        }
    }



}
