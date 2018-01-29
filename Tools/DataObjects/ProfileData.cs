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

        int dayIndex = 0;
        /// <summary>
        /// 
        /// </summary>
        public ProfileData()
        {
            jdays = new List<int>();
            values = new Dictionary<string, List<double>>
            {
                { "Green Cover", new List<double>() },
                { "Residue Cover", new List<double>() },
                { "Root Depth", new List<double>() }
            };

            dayIndex = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dates"></param>
        public void AddDate(int jday)
        {
            jdays.Add(jday);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddValue(string name, double value)
        {
            List<double> data = values[name];

            data.Add(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="datakey"></param>
        /// <param name="dayindex"></param>
        /// <param name="today"></param>
        /// <returns></returns>
        //public double GetValueForDayIndex(string datakey, int dayindex, DateTime today)
        public double GetValueForDayIndex(string datakey, DateTime today)
        {

            //UpdateDayIndex(dayindex, today);
            //UpdateDayIndex(today);
            List<double> data = values[datakey];
            int count = values.Count;
            for (int i = 0; i < count; ++i)
            {
                if (dayIndex < jdays[0])
                {
                    //double m, c, denom;
                    //denom = (double)(jdays[0]);
                    //if (denom != 0)
                    //{
                    //    //TODO: fix this
                    //    //m = data[0] / denom; //This is not correct
                    //}
                    //else
                    //{
                    //    return 0;
                    //}
                    //c = data[0] - m * jdays[0];
                    //return (m * dayindex + c);

                    //return 0;

                    //After discussion with Dan Rattray...
                    return data[0];

                }
                else if (dayIndex == jdays[i])
                {
                    return data[i];
                }
                else if (dayIndex < jdays[i])
                {
                    double m, c, denom;
                    denom = (jdays[i] - jdays[i - 1]);
                    if (denom != 0)
                    {
                        m = (data[i] - data[i - 1]) / denom;
                    }
                    else
                    {
                        return 0;
                    }
                    c = data[i] - m * jdays[i];
                    return (m * dayIndex + c);
                }
            }
            return data[count - 1];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dayindex"></param>
        /// <param name="today"></param>
        //public void UpdateDayIndex(int dayindex, DateTime today)
        public void UpdateDayIndex(DateTime today)
        {
            try
            {
                int last = jdays.Count;
                if (last >= 0)
                {
                    int dayno = today.DayOfYear;    //CHECK
                    if (jdays[last - 1] <= 366)
                    {
                        dayIndex = dayno;
                    }
                    else
                    {
                        int nolaps = (int)(jdays[last - 1] / 366) + 1;
                        int resetvalue = nolaps * 365;
                        if (dayIndex >= resetvalue && dayno == 1)
                        {
                            dayIndex = 1;
                        }
                        else
                        {
                            dayIndex++;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
