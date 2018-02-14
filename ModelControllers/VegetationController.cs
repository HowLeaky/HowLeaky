using HowLeaky.DataModels;
using System;
using System.Collections.Generic;
using HowLeaky.Tools.Helpers;
using HowLeaky.ModelControllers.Veg;

namespace HowLeaky.ModelControllers
{
    public enum CropStatus { Fallow, Planting, Growing, Harvesting };

    public class VegetationController : HLObjectController
    {
        public int DaysSinceHarvest { get; set; }             // Days since harvest
        public int DaysSincePlanting { get; set; }            // Days since planting
        public int SumCropsPlanted { get; set; }              // Total crops planted
        public int SumCropsHarvested { get; set; }            //
        public int SumCropsKilled { get; set; }               //

        public double TotalTranspiration { get; set; }
        public double TotalEvapotranspiration { get; set; }
        public double TotalCover { get; set; }
        public double TotalCoverPercent { get; set; }
        public double TotalCropResidue { get; set; }
        public double TotalResidueCover { get; set; }
        public double TotalResidueCoverPercent { get; set; }

        public VegObjectController CurrentCrop { get; set; } = null;

        //public List<VegObjectController> ChildControllers { get; set; }
        public List<VegObjectController> SortedCropList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public VegetationController(Simulation sim) : base(sim)
        {
            SortedCropList = new List<VegObjectController>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sim"></param>
        /// <param name="inputModels"></param>
        public VegetationController(Simulation sim, List<InputModel> inputModels) : this(sim)
        {

            foreach (InputModel im in inputModels)
            {
                if (im.GetType() == typeof(LAIVegInputModel))
                {
                    ChildControllers.Add(new LAIVegObjectController(sim, (LAIVegInputModel)im));
                }
                else
                {
                    ChildControllers.Add(new CoverVegObjectController(sim, (CoverVegInputModel)im));
                }
            }
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
            DaysSinceHarvest = 0;
            TotalTranspiration = 0;
            TotalEvapotranspiration = 0;
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
                        {
                            TryAndPlantNewCrop();
                        }
                        else if (CurrentCrop.IsGrowing())
                        {
                            CurrentCrop.Simulate();
                        }
                    }
                    else
                    {
                        CurrentCrop.Simulate();
                    }
                    ++DaysSinceHarvest;
                    TotalTranspiration = CurrentCrop.TotalTranspiration;
                    TotalEvapotranspiration = CurrentCrop.TotalTranspiration + Sim.SoilController.SoilEvap;
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
        public override void SetStartOfDayParameters()
        {
            SortCurrentCropList();   //resort crop list to put the current crop first.
        }

        /// <summary>
        /// 
        /// </summary>
        void SortCurrentCropList()
        {
            int cropCount = ChildControllers.Count;
            SortedCropList.Clear();
            int startindex = GetCropIndex(CurrentCrop);
            for (int i = 0; i < cropCount; ++i)
            {
                int index = startindex + i;
                index = (index < cropCount ? index : index - cropCount);
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
            return ChildControllers.IndexOf(veg);
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
            foreach (VegObjectController crop in ChildControllers)
            {
                //TODO: Check these map to the correct variables
                if (crop == CurrentCrop)
                {
                    crop.CropRunoff = Sim.SoilController.Runoff;
                    crop.CropDrainage = Sim.SoilController.DeepDrainage; //As opposed to normal drainage
                    crop.SoilEvaporation = Sim.SoilController.SoilEvap;
                    crop.TotalEvapotranspiration = TotalEvapotranspiration;
                }
                else
                {
                    crop.CropRunoff = 0;
                    crop.CropDrainage = 0;
                    crop.SoilEvaporation = 0;
                    crop.TotalEvapotranspiration = 0;
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
                    if (Sim.IrrigationController != null)
                    {
                        Sim.IrrigationController.FirstIrrigation = true;
                    }
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
                {
                    return crop.IsCropUnderMaxContinuousRotations();
                }
                else if (CurrentCrop.HasCropHadSufficientContinuousRotations())
                {
                    return crop.HasCropBeenAbsentForSufficientYears(Sim.Today);
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetCropCount()
        {
            return ChildControllers.Count;
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
                if (newcount > ChildControllers.Count)
                {
                    AddSomeCropObjects(newcount);
                }
                else if (newcount < ChildControllers.Count)
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

            for (int i = ChildControllers.Count; i < newcount; ++i)
            {
                if (UseLAIModel())
                {
                    ChildControllers.Add(new LAIVegObjectController(Sim));
                }
                else
                {
                    ChildControllers.Add(new CoverVegObjectController(Sim));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newcount"></param>
        public void RemoveSomeCropObjects(int newcount)
        {
            for (int i = ChildControllers.Count - 1; i >= newcount; --i)
            {
                ChildControllers.RemoveAt(i);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public VegObjectController GetCrop(int index)
        {
            if (index >= 0 && index < ChildControllers.Count)
            {
                return (VegObjectController)ChildControllers[index];
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cresmultiplier"></param>
        public void AdjustCropResidue(double cresmultiplier)
        {
            foreach (VegObjectController crop in ChildControllers)
            {
                crop.CropResidue = crop.CropResidue * cresmultiplier;
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
            foreach (VegObjectController crop in ChildControllers)
            {
                if (index == last)
                {
                    crop.CropResidue = value;
                }
                else
                {
                    crop.CropResidue = 0;
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
            if (CurrentCrop != null)
            {
                if (CurrentCrop.GetType() == typeof(LAIVegObjectController))
                {
                    return CurrentCrop.CropCover;
                    //LAI Model uses cover from the end of the previous day
                    //whereas Cover model predefines at the start of the day
                }
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
            {
                return CurrentCrop.GetPotentialSoilEvaporation();
            }
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
                    return Math.Min(1.0, CurrentCrop.CropCover + Sim.SoilController.TotalResidueCover * (1 - CurrentCrop.CropCover));
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
            {
                return CurrentCrop.CropCover;
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool InFallow()
        {
            if (CurrentCrop != null)
            {
                return CurrentCrop.GetInFallow();
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsPlanting()
        {
            if (CurrentCrop != null)
            {
                return CurrentCrop.GetIsPlanting();
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double CalcFallowSoilWater()
        {
            if (CurrentCrop != null)
            {
                return CurrentCrop.CalcFallowSoilWater();
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetTotalTranspiration()
        {
            if (CurrentCrop != null)
            {
                return CurrentCrop.TotalTranspiration;
            }
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
                {
                    CurrentCrop.ResetCropParametersAfterHarvest();
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
        public void CalculateResidue()
        {
            foreach (VegObjectController crop in ChildControllers)
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
            //TODO: Make the crop residue function more generic -- remove type condition
            if (CurrentCrop != null && CurrentCrop.GetType() == typeof(LAIVegObjectController))
            {
                TotalCropResidue = 0;
                TotalResidueCover = 0;
                LAIVegObjectController crop = (LAIVegObjectController)CurrentCrop;
                TotalResidueCover = crop.ResidueCover;
                int count = ChildControllers.Count;
                for (int i = 1; i < count; ++i)
                {
                    int index = GetCropIndex(crop) + 1;
                    if (index == count)
                    {
                        index = 0;
                    }
                    crop = (LAIVegObjectController)ChildControllers[index];
                    TotalResidueCover = Math.Min(1.0, TotalResidueCover + crop.ResidueCover * (1 - TotalResidueCover));
                }
                for (int i = 0; i < count; ++i)
                {
                    TotalCropResidue += ((VegObjectController)ChildControllers[i]).CropResidue;
                }

            }
            else
            {
                TotalCropResidue = ((VegObjectController)ChildControllers[0]).CropResidue;
                TotalResidueCover = ((VegObjectController)ChildControllers[0]).ResidueCover;
            }
            TotalResidueCoverPercent = TotalResidueCover * 100.0;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetTotalCropResidue()
        {
            return TotalCropResidue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetTotalResidueCover()
        {
            return TotalResidueCover;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetTotalResidueCoverPercent()
        {
            return TotalResidueCoverPercent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetTotalCropsPlanted()
        {
            return SumCropsPlanted;
        }
        public double GetTotalCropsHarvested()
        {
            return SumCropsHarvested;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetTotalCropsKilled()
        {
            return SumCropsKilled;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetAvgYieldPerHarvest()
        {
            return MathTools.Divide(CalculateTotalYield(), SumCropsHarvested);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetAvgYieldPerPlanting()
        {
            return MathTools.Divide(CalculateTotalYield(), SumCropsPlanted);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetAvgYieldPerYear()
        {
            double simyears = Sim.NumberOfDaysInSimulation / 365.0;
            return MathTools.Divide(CalculateTotalYield(), simyears);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double CalculateTotalYield()
        {
            double total = 0;
            foreach (VegObjectController crop in ChildControllers)
            {
                total += crop.CumulativeYield;
            }

            return total;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetDaysSinceHarvest()
        {
            return DaysSinceHarvest;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetDaysSincePlanting()
        {
            if (CurrentCrop != null)
            {
                return CurrentCrop.DaysSincePlanting;
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool UseLAIModel()
        {
            return CurrentCrop.GetType() == typeof(LAIVegObjectController); //FIXME
        }
    }
}
