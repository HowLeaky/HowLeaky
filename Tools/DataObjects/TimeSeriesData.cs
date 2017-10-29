using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowLeaky.Tools.DataObjects
{
    public class TimeSeriesData
    {
        List<DateTime> Dates { get; set; } = new List<DateTime>();
        List<double> Values { get; set; } = new List<double>();

        /// <summary>
        /// 
        /// </summary>
        public TimeSeriesData()
        {
            Dates = new List<DateTime>();
            Values = new List<double>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dates"></param>
        /// <param name="values"></param>
        public TimeSeriesData(List<DateTime> dates, List<double> values)
        {
            dates.CopyTo(Dates.ToArray());
            values.CopyTo(Values.ToArray());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return Dates.Count;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
       public double GetValueAtDate(DateTime date)
        {
            int index = Dates.IndexOf(date);

            if (index >= 0)
            {
                return Values[index];
            }
            return MathTools.MISSING_DATA_VALUE;
        }
    }
}
