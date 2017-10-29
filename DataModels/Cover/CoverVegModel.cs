using HowLeaky.Tools.DataObjects;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using HowLeaky.Tools;
using HowLeaky.XmlObjects;
using HowLeaky.Models;
using HowLeaky.CustomAttributes;

namespace HowLeaky.DataModels
{
    public class Data
    {
        [XmlAttribute]
        public double x { get; set; }
        [XmlAttribute]
        public double y { get; set; }
        [XmlAttribute]
        public double z { get; set; }
        [XmlAttribute]
        public double a { get; set; }

        public Data() { }
    }

    //public class CropFactorMatrix
    //{
    //    [XmlArrayItem("Data")]
    //    public List<Data> data { get; set; }

    //    public CropFactorMatrix()
    //    {
    //        data = new List<Data>();
    //    }
    //}

    [XmlRoot("VegetationType")]
    public class CoverVegObjectDataModel : DataModel
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

        public CoverVegObjectDataModel() { }
    }


    class CoverVegObjectController : VegObjectController
    {
        public CoverVegObjectDataModel dataModel { get; set; }

        public int pandayindex { get; set; }
        public int PanEvapFactor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="simulation"></param>
        public CoverVegObjectController(Simulation simulation) : base(simulation)
        {
            predefined_residue = true;
            VegetationController = simulation.VegetationController;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool StillRequiresIrrigating()
        {
            return (crop_cover > 0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool DoesCropMeetSowingCriteria()
        {
            return (pandayindex == dataModel.PlantDay);
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Simulate()
        {
            InitialisedMeasuredInputs();
            if (DoesCropMeetSowingCriteria())
                Plant();
            EtPanPhenology();
            //	CalculateRootGrowth();
            CalculateTranspiration();
            CalculateBiomass();
            if (days_since_planting == dataModel.DaysPlantingToHarvest)
                CalculateYield();
        }

        /// <summary>
        /// 
        /// </summary>
        public new void Plant()
        {
            base.Plant();
            sim.FManagementEvent = ManagementEvent.mePlanting;
            sim.UpdateManagementEventHistory(ManagementEvent.mePlanting, sim.VegetationController.GetCropIndex(this));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public new double GetTotalCover()
        {
            total_cover = residue_cover * (1 - green_cover) + green_cover;
            // was requested by VicDPI to account for animal trampling
            if (total_cover > dataModel.MaxAllowTotalCover)
            {
                total_cover = dataModel.MaxAllowTotalCover;
            }
            return total_cover;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public new double GetPotentialSoilEvaporation()
        {
            return sim.out_PanEvap_mm * (1.0 - total_cover * 0.87);
            // Dan Rattray: 0.87 comes from APSIM
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public new bool InitialisedMeasuredInputs()
        {
            if (dataModel.CoverDataType == 0)
            {
                //		UpdatePanDayIndex();
                green_cover = dataModel.CoverProfile.GetValueForDayIndex("Green Cover", pandayindex, sim.today) / 100.0 * dataModel.GreenCoverMultiplier;
                residue_cover = dataModel.CoverProfile.GetValueForDayIndex("Residue Cover", pandayindex, sim.today) / 100.0 * dataModel.ResidueCoverMultiplier;
                out_RootDepth_mm = dataModel.CoverProfile.GetValueForDayIndex("Root Depth", pandayindex, sim.today) * dataModel.RootDepthMultiplier;
                crop_cover = green_cover;
            }
            else
            {
                if (dataModel.GreenCoverTimeSeries.GetCount() > 0 && dataModel.ResidueCoverTimeSeries.GetCount() > 0 && dataModel.RootDepthTimeSeries.GetCount() > 0)
                {
                    double greenbiomass = dataModel.GreenCoverTimeSeries.GetValueAtDate(sim.today);
                    double residuebiomass = dataModel.ResidueCoverTimeSeries.GetValueAtDate(sim.today);
                    double rootbiomass = dataModel.RootDepthTimeSeries.GetValueAtDate(sim.today);
                    if (!MathTools.DoublesAreEqual(greenbiomass, MathTools.MISSING_DATA_VALUE))
                    {
                        green_cover = 1.0 * (1 - Math.Exp(-greenbiomass * dataModel.GreenCoverMultiplier));
                    }
                    else
                    {
                        green_cover = 0;
                    }

                    crop_cover = green_cover;

                    if (!MathTools.DoublesAreEqual(residuebiomass, MathTools.MISSING_DATA_VALUE))
                    {
                        residue_cover = 1.0 * (1 - Math.Exp(-residuebiomass * dataModel.ResidueCoverMultiplier));
                    }
                    else
                    {
                        residue_cover = 0;
                    }
                    if (!MathTools.DoublesAreEqual(rootbiomass, MathTools.MISSING_DATA_VALUE))
                    {
                        out_RootDepth_mm = dataModel.MaxRootDepth * (1 - Math.Exp(-rootbiomass * dataModel.RootDepthMultiplier));
                    }
                    else
                    {
                        out_RootDepth_mm = 0;
                    }
                }
                else
                {
                    green_cover = 0;
                    crop_cover = 0;
                    residue_cover = 0;
                    out_RootDepth_mm = 0;
                }
            }
            if (residue_cover > 1)
            {
                residue_cover = 1;
            }
            if (crop_cover > 1)
            {
                crop_cover = 1;
            }
            if (green_cover > 1)
            {
                green_cover = 1;
            }

            if (residue_cover < 0)
            {
                residue_cover = 0;
            }
            if (green_cover < 0)
            {
                green_cover = 0;
            }
            if (crop_cover < 0)
            {
                crop_cover = 0;
            }

            sim.total_residue_cover = residue_cover;
            sim.total_residue_cover_percent = residue_cover * 100.0;
            total_cover = GetTotalCover();
            yield = 0;
            out_Yield_t_per_ha = 0;

            out_GreenCover_pc = green_cover * 100.0;
            crop_cover_percent = crop_cover * 100.0;
            out_TotalCover_pc = total_cover * 100.0;
            out_ResidueCover_pc = residue_cover * 100.0;

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public new double CalculatePotentialTranspiration()
        {
            double cf = 1;
            if (out_RootDepth_mm > 0)
            {
                return Math.Min(green_cover * sim.out_PanEvap_mm * cf, sim.out_PanEvap_mm - sim.out_WatBal_SoilEvap_mm);
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

            if (CropStatus == ModelControllers.CropStatus.csGrowing)
            {
                ++days_since_planting;
                if (dataModel.DaysPlantingToHarvest != 0)
                {
                    crop_stage = 3.0 * days_since_planting / (double)(dataModel.DaysPlantingToHarvest);
                }
                else
                {
                    crop_stage = 0;
                    MathTools.LogDivideByZeroError("EtPanPhenology", "in_DaysPlantToHarvest_days", "crop_stage");
                }
                if (crop_stage >= 3.0)
                {
                    crop_stage = 3.0;
                    //CropHarvest=true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculateBiomass()
        {
            dry_matter += dataModel.TranspirationEfficiency * total_transpiration;
            out_DryMatter_kg_per_ha = dry_matter;   //this used to be multiplied by 10, removed 07/03/2013
        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateYield()
        {
            yield = dataModel.HarvestIndex * dry_matter; //this used to be multiplied by 10, removed
            out_Yield_t_per_ha = yield / 1000.0;
            cumulative_yield += yield;
            ++number_of_harvests;
            CropStatus = ModelControllers.CropStatus.csInFallow;
            days_since_planting = 0;
        }
    }
}
