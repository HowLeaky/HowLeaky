using HowLeaky.CustomAttributes;
using HowLeaky.ModelControllers;
using HowLeaky.Models;
using HowLeaky.DataModels;
using System;
using System.Collections.Generic;

namespace HowLeaky.ModelControllers
{
    public class PesticideController : HLObject
    {
        public List<PesticideObjectController> Pesticides { get; set; } = new List<PesticideObjectController>();

        /// <summary>
        /// 
        /// </summary>
        public PesticideController() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sim"></param>
        public PesticideController(Simulation sim) : base(sim) { }

        /// <summary>
        /// 
        /// </summary>
        public override void Simulate()

        {
            try
            {
                if (CanSimulatePesticides())
                {
                    foreach (PesticideObjectController pesticide in Pesticides)
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
            return Pesticides.Count > 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetPesticideCount()
        {
            return Pesticides.Count;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="newcount"></param>
        public void SetPesticideCount(int newcount)
        {
            if (newcount > Pesticides.Count)

                AddSomePesticideObjects(newcount);
            else if (newcount < Pesticides.Count)

                RemoveSomePesticideObjects(newcount);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newcount"></param>
        public void AddSomePesticideObjects(int newcount)
        {
            int startcount = Pesticides.Count;
            for (int i = startcount; i < newcount; ++i)
            {
                PesticideObjectController pest = new PesticideObjectController(sim);
                Pesticides.Add(pest);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newcount"></param>
        public void RemoveSomePesticideObjects(int newcount)
        {
            int startcount = Pesticides.Count;
            for (int i = Pesticides.Count - 1; i >= newcount; --i)
            {
                Pesticides.RemoveAt(i);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public PesticideObjectController GetPesticide(int index)
        {
            if (index >= 0 && index < Pesticides.Count)
            {

                return Pesticides[index];
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
            return Pesticides.IndexOf(pest);
        }
    }
}
