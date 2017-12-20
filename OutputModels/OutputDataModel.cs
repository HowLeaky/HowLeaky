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
        public List<PropertyInfo> variables;

        public OutputDataModel()
        {
            variables = new List<PropertyInfo>(this.GetType().GetProperties());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string WriteHeaders()
        {
            List<string> propertyNames = new List<string>();

            foreach (PropertyInfo p in variables)
            {
                if (p.GetType().IsGenericType)
                {
                    List<string> arrayNames = new List<string>();

                    int layerCount = 1;

                    foreach (double d in (IEnumerable<double>)p.GetValue(this))
                    {
                        arrayNames.Add(p.Name + "Layer" + layerCount.ToString());
                        layerCount++;
                    }

                    propertyNames.Add(String.Join(",", arrayNames.ToArray()));
                }
                else
                {
                    propertyNames.Add(p.Name);
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

            foreach (PropertyInfo p in variables)
            {
                if (p.GetType().IsGenericType)
                {
                    List<string> arrayValues = new List<string>();

                    foreach(double d in (IEnumerable<double>)p.GetValue(this) )
                    {
                        arrayValues.Add(d.ToString());
                    }

                    propertyValues.Add(String.Join(",", arrayValues.ToArray()));
                }
                else
                {
                    propertyValues.Add(p.GetValue(this).ToString());
                }
            }

            return String.Join(",", propertyValues.ToArray());
        }
    }
}
