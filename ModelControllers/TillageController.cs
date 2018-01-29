using HowLeaky.DataModels;
using System;
using System.Collections.Generic;
using HowLeaky.ModelControllers.Tillage;

namespace HowLeaky.ModelControllers
{
    /// <summary>
    /// 
    /// </summary>
    public class TillageController : HLObjectController
    {
        public int DaysSinceTillage { get; set; }
        public double RoughnessRatio { get; set; }
        public double TillageResidueReduction { get; set; }

        public int TillageCount
        {
            get
            {
                return ChildControllers.Count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TillageController(Simulation sim) : base(sim)
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sim"></param>
        public TillageController(Simulation sim, List<InputModel> inputModels) : this(sim)
        {
            foreach (InputModel im in inputModels)
            {
                ChildControllers.Add(new TillageObjectController(sim, (TillageObjectDataModel)im));
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public override void Simulate()
        {
            try
            {
                if (CanSimulateTillage())
                {
                    if (DaysSinceTillage != -1)
                    {
                        ++DaysSinceTillage;
                    }
                    for (int i = 0; i < TillageCount; ++i)
                    {
                        ChildControllers[i].Simulate();
                    }
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
            DaysSinceTillage = -1;
        }
       
        /// <summary>
        /// 
        /// </summary>
        public override void SetStartOfDayParameters()
        {
            RoughnessRatio = 0.0;
            TillageResidueReduction = 0;
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
                        ((TillageObjectController)ChildControllers[i]).AdjustKeyDatesForYear(year);
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
            double initialCropResidue = Sim.SoilController.TotalCropResidue;
            RoughnessRatio = roughnessratio;
            DaysSinceTillage = 0;
            Sim.UpdateManagementEventHistory(ManagementEvent.Tillage, 0);

            Sim.VegetationController.AdjustCropResidue(cresmultipler);

            TillageResidueReduction = initialCropResidue - Sim.SoilController.TotalCropResidue;
            if (RoughnessRatio > 0.0)
            {
                Sim.SoilController.RainSinceTillage = 0.0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetTillageCount()
        {
            return ChildControllers.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetTillageCount(int value)
        {
            if (value > ChildControllers.Count)
            {
                AddSomeTillageObjects(value);
            }
            else if (value < ChildControllers.Count)
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
            for (int i = ChildControllers.Count; i < value; ++i)
            {
                TillageObjectController till = new TillageObjectController(Sim, null);
                ChildControllers.Add(till);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public void RemoveSomeTillageObjects(int index)
        {
            for (int i = ChildControllers.Count - 1; i >= index; --i)
            {
                ChildControllers.RemoveAt(i);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TillageObjectController GetTillage(int index)
        {
            if (index >= 0 && index < ChildControllers.Count)
            {
                return (TillageObjectController)ChildControllers[index];
            }
            return null;
        }
    }
}
