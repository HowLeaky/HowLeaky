using HowLeaky.CustomAttributes;
using HowLeaky.ModelControllers;
using HowLeaky.SyncModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HowLeaky.OutputModels
{
    public class OutputDataModel
    {
        public HLController HLController;
        public Dictionary<PropertyInfo, Output> variables;

        public OutputDataModel()
        {
            variables = new Dictionary<PropertyInfo, Output>();
        }

        public OutputDataModel(HLController hLController) : this()
        {
            HLController = hLController;
            List<PropertyInfo> properties = new List<PropertyInfo>(HLController.GetType().GetProperties().Where(p => Attribute.IsDefined(p, typeof(Output))));
            foreach (PropertyInfo p in properties)
            {
                variables.Add(p, (Output)p.GetCustomAttribute(typeof(Output)));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string WriteHeaders()
        {
            List<string> propertyNames = new List<string>();

            foreach (KeyValuePair<PropertyInfo, Output> v in variables)
            {
                if (v.Key.GetType().IsGenericType)
                {
                    List<string> arrayNames = new List<string>();

                    int layerCount = 1;

                    foreach (double d in (IEnumerable<double>)v.Key.GetValue(this))
                    {
                        arrayNames.Add(v.Key.Name + "Layer" + layerCount.ToString());
                        layerCount++;
                    }

                    propertyNames.Add(String.Join(",", arrayNames.ToArray()));
                }
                else
                {
                    propertyNames.Add(v.Key.Name);
                }
            }

            return String.Join(",", propertyNames.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string WriteData()
        {
            List<string> propertyValues = new List<string>();

            foreach (KeyValuePair<PropertyInfo, Output> v in variables)
            {
                if (v.Key.GetType().IsGenericType)
                {
                    List<string> arrayValues = new List<string>();

                    foreach (double d in (IEnumerable<double>)v.Key.GetValue(HLController))
                    {
                        //All of these properties have an output attribute
                        //Output attr = p.Attributes.Where(a => a.GetType() == typeof(Output));
                        //double scale = p.Attributes.Where(a => a.);
                        arrayValues.Add(d.ToString());
                    }

                    propertyValues.Add(String.Join(",", arrayValues.ToArray()));
                }
                else
                {
                    object value = v.Key.GetValue(HLController);
                    if (v.Value.Scale != 1)
                    {
                        propertyValues.Add(((double)value * v.Value.Scale).ToString());
                    }
                    else
                    {
                        propertyValues.Add(value.ToString());
                    }
                }
            }

            return String.Join(",", propertyValues.ToArray());
        }
    }
}
