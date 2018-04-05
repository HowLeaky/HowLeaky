using HowLeaky.CustomAttributes;
using HowLeaky.ModelControllers;
using HowLeaky.SyncModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HowLeaky.OutputModels
{
    public class OutputDataElement
    {
        [XmlIgnore]
        public PropertyInfo PropertyInfo { get; set; }
        public Output Output { get; set; }
       // public List<int> DBIndicies { get; set; }
        public bool IsSelected { get; set; } = true;
        public string Suffix { get; set; }
        public string Name { get; set; }
        public int Index { get; set; } = -1;
        public HLController HLController { get; set; } = null;

        public OutputDataElement() { }

        public OutputDataElement(PropertyInfo propertyInfo, Output output, string suffix)
        {
            PropertyInfo = propertyInfo;
            Output = output;
            this.Suffix = Suffix;

            //DBIndicies = new List<int>();
        }

        public double Value
        {
            get
            {
                if (Index > -1)
                {
                    return (((List<double>)PropertyInfo.GetValue(HLController))[Index]);
                }
                else
                {
                    object value = PropertyInfo.GetValue(HLController);
                    if (Output.Scale != 1)
                    {
                        return (((double)value * Output.Scale));
                    }
                    else
                    {
                        if (value.GetType() == typeof(double))
                        {
                            return((double)value);
                        }
                        else
                        {
                            return((int)value);
                        }
                    }
                }
            }
        }

        //public int DBIndex
        //{
        //    get { return DBIndicies[0]; }
        //    set
        //    {
        //        DBIndicies.Clear();
        //        DBIndicies.Add(value);
        //    }
        //}
    }

    /// <summary>
    /// 
    /// </summary>
    public class OutputDataModel
    {
        public HLController HLController;
        public List<OutputDataElement> OutputDataElements;
        public string Suffix = "";

        /// <summary>
        /// 
        /// </summary>
        public OutputDataModel()
        {
            //variables = new Dictionary<PropertyInfo, Output>();
            OutputDataElements = new List<OutputDataElement>();
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
                OutputDataElements.Add(new OutputDataElement(p, (Output)p.GetCustomAttribute(typeof(Output)), Suffix));
            }
        }
    }
}
