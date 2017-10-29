using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HowLeaky.Tools.DataObjects
{
    public class Sequence
    {
        private String _Value;
        [XmlIgnore]
        public List<DateTime> Dates { get; set; }
        [XmlIgnore]
        public List<double> Values { get; set; }
        [XmlText]
        public String Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                parseStringValue();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Sequence()
        {
            Dates = new List<DateTime>();
            Values = new List<double>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool ContainsDate(DateTime date)
        {
            if (Dates.Contains(date))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public double ValueAtDate(DateTime date)
        {
            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        private void parseStringValue()
        {
            List<String> parts = new List<string>(_Value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));

            foreach (String s in parts)
            {
                List<String> seqParts = new List<string>(s.Split(new string[] { "\"", ",", " " }, StringSplitOptions.RemoveEmptyEntries));
                DateTime? thisDate = parseDate(seqParts[0]);

                if(thisDate.HasValue)
                {
                    Dates.Add(thisDate.Value);
                    double value = -1;
                    double.TryParse(seqParts[1], out value);
                    Values.Add(value);
                }

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            List<String> parts = new List<string>();

            for (int i = 0; i < Dates.Count; i++)
            {
                parts.Add("\"" + Dates[i].ToString("dd/mm/yyyy") + "," + Values[i].ToString() + "\"");
            }

            return String.Join(",", parts.ToArray());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        DateTime? parseDate(String dateString)
        {
            int year, month, day;
            List<string> dateParts = new List<string>(dateString.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries));

            if (int.TryParse(dateParts[0], out day) && int.TryParse(dateParts[1], out month) && int.TryParse(dateParts[2], out year))
            {
                return new DateTime(year, month, day);
            }

            return null;
        }
    }
}
