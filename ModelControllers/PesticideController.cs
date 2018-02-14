
using HowLeaky.DataModels;
using System;
using System.Collections.Generic;
using HowLeaky.ModelControllers.Pesticide;

namespace HowLeaky.ModelControllers
{
    public class PesticideController : HLObjectController
    {
        //public List<PesticideObjectController> ChildControllers { get; set; } = new List<PesticideObjectController>();

        /// <summary>
        /// 
        /// </summary>
        public PesticideController() { }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sim"></param>
        public PesticideController(Simulation sim, List<InputModel> inputModels) : base(sim)
        {
            foreach (InputModel im in inputModels)
            {
                ChildControllers.Add(new PesticideObjectController(sim, (PesticideInputModel)im));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Simulate()
        {
            try
            {
                if (CanSimulatePesticides())
                {
                    foreach (PesticideObjectController pesticide in ChildControllers)
                        pesticide.Simulate();

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
        bool CanSimulatePesticides()
        {
            return ChildControllers.Count > 0;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetPesticideCount()
        {
            return ChildControllers.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newcount"></param>
        public void SetPesticideCount(int newcount)
        {
            if (newcount > ChildControllers.Count)

            {
                AddSomePesticideObjects(newcount);
            }
            else if (newcount < ChildControllers.Count)
            {
                RemoveSomePesticideObjects(newcount);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newcount"></param>
        public void AddSomePesticideObjects(int newcount)
        {
            int startcount = ChildControllers.Count;
            for (int i = startcount; i < newcount; ++i)
            {
                PesticideObjectController pest = new PesticideObjectController(Sim, null);
                ChildControllers.Add(pest);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newcount"></param>
        public void RemoveSomePesticideObjects(int newcount)
        {
            int startcount = ChildControllers.Count;
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
        public PesticideObjectController GetPesticide(int index)
        {
            if (index >= 0 && index < ChildControllers.Count)
            {

                return (PesticideObjectController)ChildControllers[index];
            }
            return null;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pest"></param>
        /// <returns></returns>
        public int GetPesticideIndex(PesticideObjectController pest)
        {
            return ChildControllers.IndexOf(pest);
        }
    }
}
