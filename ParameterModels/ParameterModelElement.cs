
//using Newtonsoft.Json;
//using System;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Collections.Generic;

//namespace HowLeaky.Parameters
//{

//    [Table("ParameterModelElements")]
//    Template <t> ParameterModelElement
//    {

//        public <T> ValueString { get; set; }
//        //public string Comments { get; set; }
//        public string UserName { get; set; }
//        public DateTime DateTimeStamp { get; set; }

//        [JsonIgnore]
//        public virtual ParameterModel Parent { get; set; }

//        public ParameterModelElement()
//        {

//        }

//        public ParameterModelElement(ParameterModel source, string valuestring, /*string comment,*/ string user, DateTime dateTime) : this()
//        {
//            Parent = source;
//            ValueString = valuestring;
//            //Comments = comment;
//            UserName = user;
//            DateTimeStamp = dateTime;
//        }

//        [Key]
//        [Required]
//        public Guid Id { get; set; }

//        [NotMapped]
//        [JsonIgnore]
//        public int IntValue
//        {
//            get
//            {
//                if (Parent.ParameterType == ParameterModelType.Int)
//                {
//                    int outvalue;
//                    if (Int32.TryParse(ValueString, out outvalue))
//                        return outvalue;
//                }
//                return 0;
//            }
//            set
//            {
//                if (Parent.ParameterType == ParameterModelType.Int)
//                {
//                    ValueString = String.Format("{0}", value);
//                }
//            }
//        }
//        [NotMapped]
//        [JsonIgnore]
//        public float FloatValue
//        {
//            get
//            {
//                if (Parent.ParameterType == ParameterModelType.Float)
//                {
//                    float outvalue;
//                    if (float.TryParse(ValueString, out outvalue))
//                        return outvalue;
//                }
//                return 0;
//            }
//            set
//            {

//                if (Parent.ParameterType == ParameterModelType.Float)
//                {
//                    ValueString = String.Format("{0:G}", value);
//                }
//            }
//        }
//        [NotMapped]
//        [JsonIgnore]
//        public bool BoolValue
//        {
//            get
//            {
//                if (Parent.ParameterType == ParameterModelType.Bool)
//                {
//                    bool outvalue;
//                    if (Boolean.TryParse(ValueString, out outvalue))
//                        return outvalue;
//                }
//                return false;
//            }
//            set
//            {

//                if (Parent.ParameterType == ParameterModelType.Bool)
//                {
//                    ValueString = value.ToString();
//                }
//            }
//        }
//        [NotMapped]
//        [JsonIgnore]
//        public float[] FloatVector
//        {
//            get
//            {
//                return ValueString.Split(',').Select(float.Parse).ToArray();
//            }
//            set
//            {

//                ValueString = String.Join(",", value.Select(p => p.ToString()).ToArray());
//            }
//        }
//        [NotMapped]
//        [JsonIgnore]
//        public int[] IntVector
//        {
//            get
//            {
//                return ValueString.Split(',').Select(int.Parse).ToArray();
//            }
//            set
//            {

//                ValueString = String.Join(",", value.Select(p => p.ToString("G")).ToArray());
//            }
//        }
//        [NotMapped]
//        public string lastChange
//        {
//            get
//            {
//                return ValueString;
//            }
//        }

//        internal bool IsEqualTo(ParameterModelElement source)
//        {
//            return String.Equals(lastChange, source.lastChange);
//        }

//        internal void RemoveFromDatabase(ApplicationDbContext.ApplicationDbContext db)
//        {

//            Parent = null;
//            db.Set<ParameterModelElement>().Remove(this);
//        }
//    }
//}