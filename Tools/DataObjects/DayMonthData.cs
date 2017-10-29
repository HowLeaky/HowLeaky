using System;
using System.Xml.Serialization;

namespace HowLeaky.Tools.DataObjects
{
    public class DayMonthData
    {
        [XmlAttribute]
        public int Day { get; set; }
        [XmlAttribute]
        public int Month { get; set; }
        [XmlAttribute]
        public bool Enabled { get; set; } = true;
        /// <summary>
        /// 
        /// </summary>
        public DayMonthData()
        {
            Day = MathTools.MISSING_DATA_VALUE;
            Month = MathTools.MISSING_DATA_VALUE;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="day"></param>
        /// <param name="month"></param>
        public DayMonthData(int day, int month)
        {
            Day = day;
            Month = month;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool MatchesDate(DateTime date)
        {
            if(Day == MathTools.MISSING_DATA_VALUE || Month==MathTools.MISSING_DATA_VALUE || Enabled == false)
            {
                return false;
            }
            return (date == new DateTime(date.Year, Day, Month));
        }
    }
}
