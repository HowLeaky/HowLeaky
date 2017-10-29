using HowLeaky.CustomAttributes;
using HowLeaky.Models;
using HowLeaky.DataModels;
using System;
using System.Collections.Generic;

namespace HowLeaky.ModelControllers
{
    public class TillageController : HLObject
    {
        public List<TillageObjectController> Tillage { get; set; } = new List<TillageObjectController>();

        public int days_since_tillage { get; set; }
        public double roughness_ratio { get; set; }
        public double tillage_residue_reduction { get; set; }

        public int TillageCount
        {
            get
            {
                return Tillage.Count;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public TillageController() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sim"></param>
        public TillageController(Simulation sim) : base(sim) { }
        /// <summary>
        /// 
        /// </summary>
        public override void Simulate()
        {
            try
            {
                if (CanSimulateTillage())
                {
                    if (days_since_tillage != -1)
                        ++days_since_tillage;
                    for (int i = 0; i < TillageCount; ++i)
                        Tillage[i].Simulate();
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
        /// <returns></returns>
        public bool CanSimulateTillage()
        {
            return TillageCount > 0;
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Initialise()
        {
            days_since_tillage = -1;
        }
        /// <summary>
        /// 
        /// </summary>
        public void SetStartOfDayParameters()
        {
            roughness_ratio = 0.0;
            tillage_residue_reduction = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        public void AdjustKeyDatesForYear(int year)
        {
            try
            {
                if (CanSimulateTillage())
                {
                    for (int i = 0; i < TillageCount; ++i)
                    {
                        Tillage[i].AdjustKeyDatesForYear(year);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="cresmultipler"></param>
        /// <param name="roughnessratio"></param>
        public void UpdateTillageParameters(ETillageType type, double cresmultipler, double roughnessratio)
        {
            double initial_crop_residue = sim.total_crop_residue;
            roughness_ratio = roughnessratio;
            days_since_tillage = 0;
            sim.UpdateManagementEventHistory(ManagementEvent.meTillage, 0);

            sim.VegetationController.AdjustCropResidue(cresmultipler);

            tillage_residue_reduction = initial_crop_residue - sim.total_crop_residue;
            if (roughness_ratio > 0.0)
            {
                sim.rain_since_tillage = 0.0;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetTillageCount()
        {
            return Tillage.Count;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetTillageCount(int value)
        {
            if (value > Tillage.Count)
            {
                AddSomeTillageObjects(value);
            }
            else if (value < Tillage.Count)
            {
                RemoveSomeTillageObjects(value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void AddSomeTillageObjects(int value)
        {
            for (int i = Tillage.Count; i < value; ++i)
            {
                TillageObjectController pest = new TillageObjectController(sim);
                Tillage.Add(pest);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public void RemoveSomeTillageObjects(int index)
        {
            for (int i = Tillage.Count - 1; i >= index; --i)
            {
                Tillage.RemoveAt(i);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TillageObjectController GetTillage(int index)
        {
            if (index >= 0 && index < Tillage.Count)
            {
                return Tillage[index];
            }
            return null;
        }
    }
}
