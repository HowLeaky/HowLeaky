using HowLeaky.SyncModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace HowLeaky.DataModels
{
    /// <summary>
    /// This class is designed to map a usable interface to / from the How Leaky XML files 
    /// </summary>
    public class InputModel : CustomSyncModel
    {
        //This could be an attribute but may be out of date
        public string FileName { get; set; }
        [XmlAttribute("text")]
        public string Text { get; set; }
        public string Description { get; set; }
        [XmlIgnore]
        public Dictionary<string, object> Overrides { get; set; }
        public string LongName { get; set; }
        public override string Name
        {
            get
            {
                if (base.Name != null)
                {
                    return base.Name;
                }
                else
                {
                    string name = "";
                    try
                    {
                        name = new FileInfo(FileName).Name.Split(new char[] { '.' })[0];
                    }
                    catch (Exception e)
                    {
                        name = "";
                    }
                    return (name);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public InputModel()
        {
            Overrides = new Dictionary<string, object>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="fileName"></param>
        /// <param name="userName"></param>
        /// <param name="dt"></param>
        public InputModel(Guid guid, string fileName, string userName, DateTime dt) : base(guid, userName, dt)
        {
            FileName = fileName;
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void Init() { }
        /// <summary>
        /// 
        /// </summary>
        public void ApplyOverrides()
        {
            foreach (KeyValuePair<string, object> entry in Overrides)
            {
                SetPropertyValue(entry.Key, this, entry.Value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="target"></param>
        /// <param name="value"></param>
        private void SetPropertyValue(string propertyName, object target, object value)
        {
            PropertyInfo p = target.GetType().GetProperty(propertyName);

            int intVal;

            if(int.TryParse(value.ToString(), out intVal))
            {
                p.SetValue(target, intVal);
            }
            else
            {
                p.SetValue(target, double.Parse(value.ToString()));
            }
            //if (value.GetType() == typeof(double))
            //{
            //    val = double.Parse(value.ToString());
            //}
            //else

            //{
            //    val = int.Parse(value.ToString());
            //}

            //p.SetValue(target, val);

            return;


            //foreach (PropertyInfo p in target.GetType().GetProperties())
            //{
            //    if (p.Name == propertyName)
            //    {
            //        object val;

            //        try
            //        {
            //            val = int.Parse(value.ToString());
            //        }
            //        catch(Exception ex)
            //        {
            //            val = double.Parse(value.ToString());
            //        }

            //        //if (int.TryParse(value.ToString(), out intVal))
            //        //{
            //        //    p.SetValue(target, intVal);

            //        //}
            //        //else if (double.TryParse(value.ToString(), out doubleVal))
            //        //{
            //        //    p.SetValue(target, double.Parse(value.ToString()));
            //        //}

            //        p.SetValue(target, val);

            //        return;
            //    }

            //    if (p.PropertyType.GetProperties().Length > 0)
            //    {
            //        SetPropertyValue(propertyName, p.GetValue(target), value);
            //    }
            //}
        }
    }
}
