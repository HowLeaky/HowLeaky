using HowLeaky.Tools.DataObjects;
using System.Xml.Serialization;
using System.Xml;
using HowLeaky.XmlObjects;
using HowLeaky.CustomAttributes;
using System.Collections.Generic;

namespace HowLeaky.DataModels
{
    public class Data
    {
        [XmlAttribute("x")]
        [Input("x")]
        public double X { get; set; }
        [XmlAttribute("y")]
        [Input("y")]
        public double Y { get; set; }
        [XmlAttribute("z")]
        [Input("z")]
        public double Z { get; set; }
        [XmlAttribute("a")]
        [Input("a")]
        public double A { get; set; }

        public Data() { }
    }

    [XmlRoot("VegetationType")]
    public class CoverVegInputModel : VegInputModel
    {
        //Input Parameters
        [Input("CoverInputOptions")]
        public IndexData CoverInputOptions { get; set; }
        [Input("ModelType")]
        public IndexData ModelType { get; set; }
        [Input("SourceData")]
        public IndexData SourceData { get; set; }
        [XmlArray("CropFactorMatrix")]
        [XmlArrayItem("Data")]
        [Input("CropFactorMatrix")]
        public Data[] CropFactorMatrix { get; set; }
        [XmlElement("PanPlantDay")]
        [Input("PlantDay")]
        public int PlantDay { get; set; } = 1;
        [XmlIgnore]
        [Input("CoverDataType")]
        public int CoverDataType { get; set; } = 0;             //for no time series
        [XmlIgnore]
        [Input("CoverProfile")]
        public ProfileData CoverProfile { get; set; }
        [XmlElement("LinkToGreenCover")]
        [Input("GreenCoverTimeSeries")]
        public TimeSeriesData GreenCoverTimeSeries { get; set; }
        [XmlElement("LinkToResidueCover")]
        [Input("ResidueCoverTimeSeries")]
        public TimeSeriesData ResidueCoverTimeSeries { get; set; }
        [XmlElement("LinkToRootDepth")]
        [Input("RootDepthTimeSeries")]
        public TimeSeriesData RootDepthTimeSeries { get; set; }
        [XmlElement("WaterUseEffic")]
        [Input("TranspirationEfficiency")]
        public double TranspirationEfficiency { get; set; }     // The ratio of grain production (kg/ha) to water supply (mm).
        [XmlElement("PanHarvestIndex")]
        [Input("HarvestIndex")]
        public double HarvestIndex { get; set; }                // The grain biomass (kg/ha) divided by the above-ground biomass at flowering (kg/ha).
        [Input("DaysPlantingToHarvest","days")]
        public int DaysPlantingToHarvest { get; set; }          // The number of days between planting and harvest.
        [XmlElement("GreenBioMassToCoverFactor")]
        [Input("GreenCoverMultiplier")]
        public double GreenCoverMultiplier { get; set; } = 1;   // Scaling factor for green cover
        [XmlElement("ResidueBioMassToCoverFactor")]
        [Input("ResidueCoverMultiplier")]
        public double ResidueCoverMultiplier { get; set; } = 1; // Scaling factor for residue cover
        [XmlElement("RootBioMassToDepthFactor")]
        [Input("RootDepthMultiplier")]
        public double RootDepthMultiplier { get; set; } = 1;    // Scaling factor for root depth
        [Input("MaxAllowTotalCover")]
        public double MaxAllowTotalCover { get; set; } = 1;     // Maximum allowable total cover
        [Input("MaxRootDepth","mm")]
        public double MaxRootDepth { get; set; }                // located in CustomVegObject - >The maximum depth of the roots from the soil surface.  For the LAI model, the model calculates daily root growth from the root depth increase parameter
        [Input("SWPropForNoStress")]
        public double SWPropForNoStress { get; set; } = 0.3;

        /// <summary>
        /// 
        /// </summary>
        public CoverVegInputModel()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitialiseCoverProfile()
        {
            CoverProfile = new ProfileData(new List<string> (new string[] { "Green Cover", "Residue Cover","Root Depth" }));

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
