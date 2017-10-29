using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowLeaky.Tools.DataObjects
{
    public class ProfileData
    {
        List<int> jdays;
        Dictionary<string, List<double>> values;
        /// <summary>
        /// 
        /// </summary>
        public ProfileData()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dates"></param>
        public void AddDateSeries(string dates)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddValuesSeries(string name, string value)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="datakey"></param>
        /// <param name="dayindex"></param>
        /// <param name="today"></param>
        /// <returns></returns>
        public double GetValueForDayIndex(string datakey, int dayindex, DateTime today)
        {

            UpdateDayIndex(dayindex, today);
            List<double> data = new List<double>(values[datakey]);
            int count = values.Count;
            //for (int i = 0; i < count; ++i)
            //{
            //    if (dayindex < jdays[0].days)
            //    {
            //        double m, c, denom;
            //        denom = (double)(jdays[0].days);
            //        if (denom != 0)
            //            m = data[0] / denom;
            //        else
            //            return 0;
            //        c = data[0] - m * jdays[0].days;
            //        return (m * dayindex + c);
            //    }
            //    else if (dayindex == jdays[i].days)
            //        return data[i];
            //    else if (dayindex < jdays[i].days)
            //    {
            //        double m, c, denom;
            //        denom = (jdays[i].days - jdays[i - 1].days);
            //        if (denom != 0)
            //            m = (data[i] - data[i - 1]) / denom;
            //        else
            //            return 0;
            //        c = data[i] - m * jdays[i].days;
            //        return (m * dayindex + c);
            //    }
            //}
            return data[count - 1];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dayindex"></param>
        /// <param name="today"></param>
        public void UpdateDayIndex(int dayindex, DateTime today)
        {
            try
            {
                int last = jdays.Count;
                //if (last >= 0)
                //{
                //    int dayno = today.DayOfYear;    //CHECK
                //    if (jdays[last] <= days(366))
                //        dayindex = dayno;
                //    else
                //    {
                //        int nolaps = (int)(jdays[last].days / 366) + 1;
                //        int resetvalue = nolaps * 365;
                //        if (dayindex >= resetvalue && dayno == 1)
                //            dayindex = 1;
                //        else
                //            dayindex++;
                //    }
                //}
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}
