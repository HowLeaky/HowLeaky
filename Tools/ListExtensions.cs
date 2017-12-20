using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowLeaky.Tools.ListExtensions
{
    public static class ListExtensions
    {
        public static List<double> Fill(this List<double> list, double value)
        {
            for(int i = 0; i < list.Capacity; i++)
            {
                list.Add(value);
            }

            return list;
        }
    }
}
