using HowLeaky.SyncModels;
using System;
using System.Collections.Generic;
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
        foreach(KeyValuePair<string, object> entry in Overrides)
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
            foreach (PropertyInfo p in target.GetType().GetProperties())
            {
                if(p.Name == propertyName)
                {
                    p.SetValue(target, value);
                }

                if(p.ReflectedType.IsClass)
                {
                    SetPropertyValue(propertyName, p.GetValue(target), value) ;
                }
            }
        }
    }
}
