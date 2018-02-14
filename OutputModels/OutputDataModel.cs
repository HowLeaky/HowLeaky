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
    public class OutputDataElement
    {
        public PropertyInfo PropertyInfo { get; set; }
        public Output Output { get; set; }
        public List<int> DBIndicies { get; set; }
        public bool IsSelected { get; set; }

        public OutputDataElement(PropertyInfo propertyInfo, Output output)
        {
            PropertyInfo = propertyInfo;
            Output = output;
            DBIndicies = new List<int>();
        }

        public int DBIndex
        {
            get { return DBIndicies[0]; }
            set
            {
                DBIndicies.Clear();
                DBIndicies.Add(value);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class OutputDataModel
    {
        public HLController HLController;
        public List<OutputDataElement> variables;
        public string Suffix = "";

        /// <summary>
        /// 
        /// </summary>
        public OutputDataModel()
        {
            //variables = new Dictionary<PropertyInfo, Output>();
            variables = new List<OutputDataElement>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hLController"></param>
        public OutputDataModel(HLController hLController) : this()
        {
            HLController = hLController;

            if (HLController.GetType().GetInterface("IChildController") != null)
            {
                Suffix = HLController.GetInputModel().Name;
            }

            List<PropertyInfo> properties = new List<PropertyInfo>(HLController.GetType().GetProperties().Where(p => Attribute.IsDefined(p, typeof(Output))));
            foreach (PropertyInfo p in properties)
            {
                variables.Add(new OutputDataElement(p, (Output)p.GetCustomAttribute(typeof(Output))));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string WriteHeaders()
        {
            List<string> propertyNames = new List<string>();

            foreach (OutputDataElement v in variables)
            {
                if (v.PropertyInfo.PropertyType.IsGenericType)
                {
                    List<string> arrayNames = new List<string>();

                    int layerCount = 1;

                    foreach (double d in (IEnumerable<double>)v.PropertyInfo.GetValue(HLController))
                    {
                        ; arrayNames.Add(v.PropertyInfo.Name + "Layer" + layerCount.ToString() + (Suffix == "" ? "" : ("-" + Suffix)));
                        layerCount++;
                    }

                    propertyNames.Add(String.Join(",", arrayNames.ToArray()));
                }
                else
                {
                    propertyNames.Add(v.PropertyInfo.Name + (Suffix == "" ? "" : ("-" + Suffix)));
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

            foreach (OutputDataElement v in variables)
            {
                if (v.PropertyInfo.PropertyType.IsGenericType)
                {
                    List<string> arrayValues = new List<string>();

                    foreach (double d in (IEnumerable<double>)v.PropertyInfo.GetValue(HLController))
                    {
                        //All of these properties have an output attribute
                        arrayValues.Add(d.ToString());
                    }

                    propertyValues.Add(String.Join(",", arrayValues.ToArray()));
                }
                else
                {
                    object value = v.PropertyInfo.GetValue(HLController);
                    if (v.Output.Scale != 1)
                    {
                        propertyValues.Add(((double)value * v.Output.Scale).ToString());
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
