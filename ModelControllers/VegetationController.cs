using HowLeaky.DataModels;
using HowLeaky.Tools;
using HowLeaky.Models;
using System;
using System.Collections.Generic;

namespace HowLeaky.ModelControllers
{
    public enum CropStatus { csInFallow, csPlanting, csGrowing, csHarvesting };

    public class VegetationController : HLObject
    {
        //TODO: Refactor these variables
        public int days_since_harvest { get; set; }             // Days since harvest
        public int days_since_planting { get; set; }            // Days since planting
        public int sum_crops_planted { get; set; }              // Total crops planted
        public int sum_crops_harvested { get; set; }            //
        public int sum_crops_killed { get; set; }               //

        //public struct total{

        //}

        //TODO: Refactor these variables

        public double total_transpiration { get; set; }         //
        public double total_evapotranspiration { get; set; }    //
        public double total_cover { get; set; }                 //
        public double total_cover_percent { get; set; }         //
        public double total_crop_residue { get; set; }          //
        public double total_residue_cover { get; set; }         //
        public double total_residue_cover_percent { get; set; } //

        public VegObjectController CurrentCrop { get; set; } = null;

        public List<VegObjectController> CropList { get; set; }
        public List<VegObjectController> SortedCropList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public VegetationController() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sim"></param>
        public VegetationController(Simulation sim) : base(sim)
        {

        }
        ////void RegisterInputs(TSimulationInputDefinitions* im)
        ////{

        ////}
        ////void RegisterOutputs(TSimulationInputDefinitions* im)
        ////{

        ////}
        /// <summary>
        /// 
        /// </summary>
        public override void Initialise()
        {
            int _crop_count = GetCropCount();
            CurrentCrop = GetCrop(0);
            //for (int i = 0; i<_crop_count; ++i)
            //	GetCrop(i).Initialise();
            //days_since_harvest=0;
            //total_transpiration=0;
            //total_evapotranspiration=0;
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Simulate()
        {
            try
            {
                if (CurrentCrop != null)
                {
                    if (UseLAIModel())
                    {
                        if (CurrentCrop.GetInFallow())
                            TryAndPlantNewCrop();
                        else if (CurrentCrop.IsGrowing())
                            CurrentCrop.Simulate();
                    }
                    else
                        CurrentCrop.Simulate();
                    ++days_since_harvest;
                    total_transpiration = CurrentCrop.total_transpiration;
                    total_evapotranspiration = CurrentCrop.total_transpiration + sim.out_WatBal_SoilEvap_mm;
                    UpdateCropWaterBalanceParameters();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void SetStartOfDayParameters()
        {
            SortCurrentCropList();   //resort crop list to put the current crop first.
        }
        /// <summary>
        /// 
        /// </summary>
        void SortCurrentCropList()
        {
            int _crop_count = CropList.Count;
            SortedCropList.Clear();
            int startindex = GetCropIndex(CurrentCrop);
            for (int i = 0; i < _crop_count; ++i)
            {
                int index = startindex + i;
                index = (index < _crop_count ? index : index - _crop_count);
                VegObjectController crop = GetCrop(index);
                SortedCropList.Add(crop);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="veg"></param>
        /// <returns></returns>
        public int GetCropIndex(VegObjectController veg)
        {
            return CropList.IndexOf(veg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        VegObjectController GetSortedCrop(int index)
        {

            return SortedCropList[index];
        }

        /// <summary>
        /// 
        /// </summary>
        void UpdateCropWaterBalanceParameters()
        {
            foreach (VegObjectController crop in CropList)
            {
                //TODO: Check these map to the correct variables
                if (crop == CurrentCrop)
                {
                    crop.out_CropRunoff_mm = sim.out_WatBal_Runoff_mm;
                    crop.out_CropDrainage_mm = sim.out_WatBal_DeepDrainage_mm; //As opposed to normal drainage
                    crop.out_SoilEvaporation_mm = sim.out_WatBal_SoilEvap_mm;
                    crop.total_evapotranspiration = total_evapotranspiration;
                }
                else
                {
                    crop.out_CropRunoff_mm = 0;
                    crop.out_CropDrainage_mm = 0;
                    crop.out_SoilEvaporation_mm = 0;
                    crop.total_evapotranspiration = 0;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void TryAndPlantNewCrop()
        {
            foreach (VegObjectController crop in SortedCropList)
            {
                if (CanPlantCrop(crop))
                {
                    crop.Plant();
                    sim.IrrigationController.firstIrrigation = true;
                    return;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="crop"></param>
        /// <returns></returns>
        public bool CanPlantCrop(VegObjectController crop)
        {
            // Here are a few notes to try and get your head around the logic here.
            // First of all check to see if we leave left sufficient gap between this new
            // rotation and the previous rotation of this crop.
            // Case 1:
            // First test to see if this is the CurrentCrop.
            // If so, then make sure that we HAVENT exceeded max rotation count. Then we must
            // see if actually meet the planting criteria. If any of these fail, then return
            // "false" so that we can test another crop. Otherwise return "true" to tell the
            // sim to replant the CurrentCrop
            // Case 2:
            // If this is NOT the CurrentCrop - we had better first check that the CurrentCrop
            // is still expected to be in rotation. Then check to see that the sowing criteria
            // has been met. Return "true" if all good, otherwise return "false" so we can test
            // another crop. If we run out of crops, then the CurrentCrop stays in fallow.
            if (crop.IsSequenceCorrect() && crop.DoesCropMeetSowingCriteria())
            {

                if (crop == CurrentCrop)
                    return crop.IsCropUnderMaxContinuousRotations();
                else if (CurrentCrop.HasCropHadSufficientContinuousRotations())
                    return crop.HasCropBeenAbsentForSufficientYears(sim.today);
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetCropCount()
        {
            return CropList.Count;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newcount"></param>
        public void SetCropCount(int newcount)
        {
            //int startcount;
            VegObjectController firstcrop = GetCrop(0);
            if (UseLAIModel())
            {
                if (firstcrop != null && firstcrop.GetType() == typeof(CoverVegObjectController))
                {
                    RemoveSomeCropObjects(0);
                }
                if (newcount > CropList.Count)
                {
                    AddSomeCropObjects(newcount);
                }
                else if (newcount < CropList.Count)
                {
                    RemoveSomeCropObjects(newcount);
                }
            }
            else
            {
                if (firstcrop != null && firstcrop.GetType() == typeof(LAIVegObjectController))
                {
                    RemoveSomeCropObjects(0);
                }
                AddSomeCropObjects(newcount);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newcount"></param>
        public void AddSomeCropObjects(int newcount)
        {
            VegObjectController firstcrop = GetCrop(0);

            for (int i = CropList.Count; i < newcount; ++i)
            {
                if (UseLAIModel())
                {
                    CropList.Add(new LAIVegObjectController(sim));
                }
                else
                {
                    CropList.Add(new CoverVegObjectController(sim));
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newcount"></param>
        public void RemoveSomeCropObjects(int newcount)
        {
            for (int i = CropList.Count - 1; i >= newcount; --i)
            {
                CropList.RemoveAt(i);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public VegObjectController GetCrop(int index)
        {
            if (index >= 0 && index < CropList.Count)
            {
                return CropList[index];
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cresmultiplier"></param>
        public void AdjustCropResidue(double cresmultiplier)
        {
            foreach (VegObjectController crop in CropList)
            {
                crop.crop_residue = crop.crop_residue * cresmultiplier;
            }
            CalculateTotalResidue();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void ResetCropResidue(double value)
        {
            int index = 0;
            int last = SortedCropList.Count - 1;
            foreach (VegObjectController crop in CropList)
            {
                if (index == last)
                {
                    crop.crop_residue = value;
                }
                else
                {
                    crop.crop_residue = 0;
                }
                ++index;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public double GetCropCoverIfLAIModel(double current)
        {
            if (CurrentCrop.GetType() == typeof(LAIVegObjectController))
            {
                return CurrentCrop.crop_cover;
                //LAI Model uses cover from the end of the previous day
                //whereas Cover model predefines at the start of the day
            }
            return current;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetPotentialSoilEvaporation()
        {
            if (CurrentCrop != null)
                return CurrentCrop.GetPotentialSoilEvaporation();
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetTotalCover()
        {
            if (CurrentCrop != null)
            {
                if (CurrentCrop.GetType() == typeof(LAIVegObjectController))
                {
                    return Math.Min(1.0, CurrentCrop.crop_cover + sim.total_residue_cover * (1 - CurrentCrop.crop_cover));
                }
                else
                {
                    return CurrentCrop.GetTotalCover();
                }
            }
            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetCropCover()
        {
            if (CurrentCrop != null)
                return CurrentCrop.crop_cover;
            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool InFallow()
        {
            if (CurrentCrop != null)
                return CurrentCrop.GetInFallow();
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsPlanting()
        {
            if (CurrentCrop != null)
                return CurrentCrop.GetIsPlanting();
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double CalcFallowSoilWater()
        {
            if (CurrentCrop != null)
                return CurrentCrop.CalcFallowSoilWater();
            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetTotalTranspiration()
        {
            if (CurrentCrop != null)
                return CurrentCrop.total_transpiration;
            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        public void ResetAnyParametersIfRequired()
        {
            try
            {
                if (CurrentCrop != null)
                    CurrentCrop.ResetCropParametersAfterHarvest();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateResidue()
        {
            foreach (VegObjectController crop in CropList)
            {
                crop.CalculateResidue();
            }
            CalculateTotalResidue();
        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateTotalResidue()
        {
            if (CurrentCrop != null && UseLAIModel())
            {
                total_crop_residue = 0;
                total_residue_cover = 0;
                LAIVegObjectController crop = (LAIVegObjectController)CurrentCrop;
                total_residue_cover = crop.residue_cover;
                int count = CropList.Count;
                for (int i = 1; i < count; ++i)
                {
                    int index = GetCropIndex(crop) + 1;
                    if (index == count)
                        index = 0;
                    crop = (LAIVegObjectController)CropList[index];
                    total_residue_cover = Math.Min(1.0, total_residue_cover + crop.residue_cover * (1 - total_residue_cover));
                }
                for (int i = 0; i < count; ++i)
                {
                    total_crop_residue += CropList[i].crop_residue;
                }

            }
            else
            {
                total_crop_residue = CropList[0].crop_residue;
                total_residue_cover = CropList[0].residue_cover;
            }
            total_residue_cover_percent = total_residue_cover * 100.0;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetTotalCropResidue()
        {
            return total_crop_residue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetTotalResidueCover()
        {
            return total_residue_cover;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetTotalResidueCoverPercent()
        {
            return total_residue_cover_percent;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetTotalCropsPlanted()
        {
            return sum_crops_planted;
        }
        public double GetTotalCropsHarvested()
        {
            return sum_crops_harvested;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetTotalCropsKilled()
        {
            return sum_crops_killed;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetAvgYieldPerHarvest()
        {
            return MathTools.Divide(CalculateTotalYield(), sum_crops_harvested);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetAvgYieldPerPlanting()
        {
            return MathTools.Divide(CalculateTotalYield(), sum_crops_planted);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetAvgYieldPerYear()
        {
            double simyears = sim.number_of_days_in_simulation / 365.0;
            return MathTools.Divide(CalculateTotalYield(), simyears);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double CalculateTotalYield()
        {
            double total = 0;
            foreach (VegObjectController crop in CropList)
            {
                total += crop.cumulative_yield;
            }

            return total;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetDaysSinceHarvest()
        {
            return days_since_harvest;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetDaysSincePlanting()
        {
            if (CurrentCrop != null)
                return CurrentCrop.days_since_planting;
            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool UseLAIModel()
        {
            return true; //FIXME
        }
    }
}
