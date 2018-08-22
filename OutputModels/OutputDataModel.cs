using HowLeaky.CustomAttributes;
using HowLeaky.ModelControllers;
using HowLeaky.SyncModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HowLeaky.OutputModels
{
    public class OutputDataElement : INotifyPropertyChanged
    {
        [XmlIgnore]
        public PropertyInfo PropertyInfo { get; set; }
        public Output Output { get; set; }
        // public List<int> DBIndicies { get; set; }
        private bool _IsSelected = true;

        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                _IsSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public string Suffix { get; set; }
        public string Name { get; set; }
        public int Index { get; set; } = -1;
        public HLController HLController { get; set; } = null;

        public event PropertyChangedEventHandler PropertyChanged;

        public OutputDataElement() { }

        public OutputDataElement(PropertyInfo propertyInfo, Output output, string suffix)
        {
            PropertyInfo = propertyInfo;
            Output = output;
            this.Suffix = Suffix;

            //DBIndicies = new List<int>();
        }


        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
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
                            return ((double)value);
                        }
                        else
                        {
                            return ((int)value);
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
                Suffix = ReplaceIllegalCharacters(HLController.GetInputModel().Name);
            }

            List<PropertyInfo> properties = new List<PropertyInfo>(HLController.GetType().GetProperties().Where(p => Attribute.IsDefined(p, typeof(Output))));
            foreach (PropertyInfo p in properties)
            {
                OutputDataElements.Add(new OutputDataElement(p, (Output)p.GetCustomAttribute(typeof(Output)), Suffix));
            }
        }

        //String.prototype.capitalize = function()
        //{
        //    return this.replace(/ (?:^|\s)\S / g, function(a) { return a.toUpperCase(); });
        //};


//        var myarr = ["color - black", "color - blue", "color - Red"]

//for(var i=0; i<myarr.length; i++) {
//    myarr[i] = myarr[i].capitalize().replace(/ -/g, ":");
//    }

    public string ReplaceIllegalCharacters(string Name)
        {
            char[] chars = { '-', '%',' ' };

            string[] temp = Name.Split(chars, StringSplitOptions.RemoveEmptyEntries);

            //for(int i = 0; i < temp.Length; i++)
            //{
            //    temp[i] = Char.ToUpper(temp[i][0]) + temp[i].Substring(1, temp[i].Length - 1);
            //}

            return(String.Join("_", temp));

        }
    }
}
