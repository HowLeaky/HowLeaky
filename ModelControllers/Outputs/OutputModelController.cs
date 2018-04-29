using HowLeaky.OutputModels;
using HowLeaky.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowLeaky.ModelControllers
{
    public static class ExtMethods
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }

    public class OutputModelController : HLController
    {
        public List<OutputDataElement> Outputs = new List<OutputDataElement>();
        public List<List<double>> Values = new List<List<double>>();

        public List<OutputDataModel> OutputDataModels;

        public virtual bool DateIsOutput { get; set; } = true;

        public OutputModelController() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sim"></param>
        public OutputModelController(Simulation sim)
        {
            this.Sim = sim;

            //Create some stream writers from the simuation name
            OutputDataModels = new List<OutputDataModel>();

            //Create a list of output models
            foreach (HLController hlc in Sim.ActiveControlllers)
            {
                //Get the daily Ouputs and add to the list
                if (hlc.GetType().BaseType == typeof(HLObjectController))
                {
                    List<OutputDataModel> odms = ((HLObjectController)hlc).GetOutputModels();

                    if (odms != null)
                    {
                        OutputDataModels.AddRange(odms);
                    }
                }
                else
                {
                    List<OutputDataModel> odms = hlc.GetOutputModels();
                    if (odms != null)
                    {
                        OutputDataModels.AddRange(odms);
                    }
                }
            }
            if (!DateIsOutput)
            {
                Sim.Output.OutputDataElements.Where(h => h.PropertyInfo.Name == "Today").FirstOrDefault().IsSelected = false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void PrepareVariableNamesForOutput()
        {
            Outputs = new List<OutputDataElement>();

            foreach (OutputDataModel odm in OutputDataModels)
            {
                if (odm.HLController.GetType() != typeof(PesticideController))
                {
                    Outputs.AddRange(GetOutputs(odm, false));
                }
                else
                {
                    Outputs.AddRange(GetOutputs(odm, true));
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void WriteDailyData() { }
        /// <summary>
        /// 
        /// </summary>
        public virtual void Finalise() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="odm"></param>
        /// <param name="useSuffix"></param>
        /// <returns></returns>
        public List<string> GetHeaders(OutputDataModel odm, bool useSuffix = true)
        {
            return new List<string>(GetOutputs(odm, useSuffix).Select(x => x.Name));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="odm"></param>
        /// <param name="useSuffix"></param>
        /// <returns></returns>
        public List<OutputDataElement> GetOutputs(OutputDataModel odm, bool useSuffix = true)
        {
            List<OutputDataElement> OutputDataElements = new List<OutputDataElement>();

            string suffix = "";

            if (useSuffix == true)
            {
                suffix = odm.Suffix;
            }

            foreach (OutputDataElement v in odm.OutputDataElements)
            {
                if (v.IsSelected)
                {
                    if (v.PropertyInfo.PropertyType.IsGenericType)
                    {
                        int layerCount = 1;

                        foreach (double d in (IEnumerable<double>)v.PropertyInfo.GetValue(odm.HLController))
                        {
                            OutputDataElement o = Cloner.DeepClone(v);

                            o.Index = layerCount - 1;
                            o.Name = v.PropertyInfo.Name + "Layer" + layerCount.ToString() + (suffix == "" ? "" : ("-" + suffix));
                            o.HLController = odm.HLController;
                            o.PropertyInfo = v.PropertyInfo;

                            OutputDataElements.Add(o);
                            layerCount++;
                        }
                    }
                    else
                    {
                        OutputDataElements.Add(v);
                        v.Name = v.PropertyInfo.Name + (suffix == "" ? "" : ("-" + suffix));
                        v.HLController = odm.HLController;
                    }
                }
            }
            return OutputDataElements;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="odm"></param>
        /// <returns></returns>
        public List<double> GetData()
        {
            List<double> propertyValues = new List<double>();

            foreach (OutputDataElement v in Outputs)
            {
                propertyValues.Add(v.Value);
            }
            return propertyValues;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="odm"></param>
        /// <returns></returns>
        public List<double> GetData(OutputDataModel odm)
        {
            List<double> propertyValues = new List<double>();

            foreach (OutputDataElement v in odm.OutputDataElements)
            {
                if (v.PropertyInfo.PropertyType != typeof(DateTime))
                {
                    if (v.IsSelected)
                    {
                        if (v.PropertyInfo.PropertyType.IsGenericType)
                        {
                            List<double> arrayValues = new List<double>();

                            foreach (double d in (IEnumerable<double>)v.PropertyInfo.GetValue(odm.HLController))
                            {
                                //All of these properties have an output attribute
                                arrayValues.Add(d);
                            }

                            propertyValues.AddRange(arrayValues.ToArray());
                        }
                        else
                        {
                            object value = v.PropertyInfo.GetValue(odm.HLController);
                            if (v.Output.Scale != 1)
                            {
                                propertyValues.Add(((double)value * v.Output.Scale));
                            }
                            else
                            {
                                if (value.GetType() == typeof(double))
                                {
                                    propertyValues.Add((double)value);
                                }
                                else
                                {
                                    propertyValues.Add((int)value);
                                }
                            }
                        }
                    }
                }
            }
            return propertyValues;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Project"></param>
        /// <returns></returns>
        public static List<OutputDataElement> GetProjectOutputs(Project Project)
        {
            List<OutputDataElement> Outputs = new List<OutputDataElement>();

            //Get the outputs from the project

            //Firstly get a list of all of the inputs
            //This simulation will have a climate, soil and (1 for reef project) vegetaion controller

            //Find the soil with the most layers
            List<SoilController> SoilControllers = new List<SoilController>(Project.Simulations.Select(o => o.SoilController));
            AddOutputElements(Outputs, SoilControllers.Where(s => s.LayerCount == SoilControllers.Max(x => x.LayerCount)).FirstOrDefault(), false);

            //Climate
            AddOutputHeader(Outputs, typeof(ClimateController), Project.Simulations[0], false);
            //Vegetation
            //Find max VegetationController children to determine if suffix should be used
            int maxSimVegCount = Project.Simulations.Select(x => x.VegetationController).Max(y => y.ChildControllers.Count);
            AddOutputElements(Outputs, Project.Simulations[0].VegetationController.ChildControllers[0], false);

            //Optional Controllers
            //IrrigationController
            AddOutputOutputElements(Outputs, typeof(IrrigationController), Project.Simulations, false);
            //Nitrate
            AddOutputOutputElements(Outputs, typeof(NitrateController), Project.Simulations, false);
            AddOutputOutputElements(Outputs, typeof(DINNitrateController), Project.Simulations, false);
            //Phosphorus
            AddOutputOutputElements(Outputs, typeof(PhosphorusController), Project.Simulations, false);
            //Solutes
            AddOutputOutputElements(Outputs, typeof(SolutesController), Project.Simulations, false);
            //Tillage
            //Find max TillageController children to determine if suffix should be used
            AddOutputOutputElements(Outputs, typeof(TillageController), Project.Simulations, false);
            //Pesticides is handled differently
            //Always use the suffix for Pesticides
            AddOutputOutputElements(Outputs, typeof(PesticideController), Project.Simulations, true);

            return Outputs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="header"></param>
        public static void AddOutputHeader(List<OutputDataElement> Outputs, Type controllerType, Simulation simulation, bool useSuffix)
        {
            HLController controller = (HLController)simulation.ActiveControlllers.Where(x => x.GetType() == controllerType).FirstOrDefault();

            AddOutputElements(Outputs, controller, useSuffix);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="header"></param>
        public static void AddOutputOutputElements(List<OutputDataElement> Outputs, Type controllerType, List<Simulation> simulations, bool useSuffix)
        {
            if (!useSuffix)
            {
                HLController controller = null;

                if (controllerType.GetProperty("ChildControllers") == null)
                {
                    controller = (HLController)simulations.SelectMany(y => y.ActiveControlllers.Where(x => x.GetType() == controllerType)).FirstOrDefault();
                }
                else
                {
                    //List<HLObjectController> controllers = (List<HLObjectController>)simulations.SelectMany(s => s.ActiveControlllers.Where(x => x.GetType() == controllerType));
                    List<HLController> controllers = new List<HLController>(simulations.SelectMany(s => s.ActiveControlllers.Where(x => x.GetType() == controllerType)));
                    if (controllers != null && controllers.Count >= 1)
                    {
                        controller = ((HLObjectController)controllers[0]).ChildControllers[0];
                    }

                }
                AddOutputElements(Outputs, controller, useSuffix);
            }
            else
            {
                //Need to check for child controllers
                List<HLController> objectControllers = new List<HLController>(simulations.SelectMany(y => y.ActiveControlllers.Where(x => x.GetType() == controllerType)));

                List<HLController> controllers = new List<HLController>(objectControllers.SelectMany(o => ((HLObjectController)o).ChildControllers).DistinctBy(x => x.Name));

                foreach (HLController hlc in controllers)
                {
                    AddOutputElements(Outputs, hlc, useSuffix);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="useSuffix"></param>
        public static void AddOutputElements(List<OutputDataElement> Outputs, HLController controller, bool useSuffix)
        {
            if (controller == null)
            {
                return;
            }

            Outputs.AddRange(controller.Sim.OutputModelController.GetOutputs(controller.Output, useSuffix));
        }
    }
}
