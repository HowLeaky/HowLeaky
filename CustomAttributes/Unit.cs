using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowLeaky.CustomAttributes
{
    public enum UnitTypes { PerCent, Area, Volume }
    public class Unit : Attribute
    {
        public string unit { get; set; }

        public Unit(string unit)
        {
            this.unit = unit;
        }
    }
}
