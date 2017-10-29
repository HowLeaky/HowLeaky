using System.Collections.Generic;

namespace HowLeaky.Tools.DataObjects
{
    public class OutputParameter
    {
        List<double> values = new List<double>();
        int CurrentIndex;
        void Resize(int size) { }
        bool StoreValues;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public double this[int key]
        {
            get
            {
                if (key > 0 && key <= values.Count)
                {
                    return values[key];
                }
                return MathTools.MISSING_DATA_VALUE;
            }
            //set
            //{
            //    SetValue(key, value);
            //}
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double Value()
        {
            return this[CurrentIndex];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(double value)
        {
            values[CurrentIndex] = value;
        }
        /// <summary>
        /// 
        /// </summary>
        public void SetZero()
        {
            values[CurrentIndex] = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        public void SetMissing()
        {
            values[CurrentIndex] = MathTools.MISSING_DATA_VALUE;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Increment()
        {
            if (CurrentIndex > 0)
            {
                values[CurrentIndex] = values[CurrentIndex - 1] + 1;
            }
            values[CurrentIndex] = 1;
        }
    }
}
