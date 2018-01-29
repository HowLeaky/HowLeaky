using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowLeaky.CustomAttributes
{
    public class Alias:Attribute
    {
        public string Name { get; set; }
        
        public Alias(string Name)
        {
            this.Name = Name;
        }
    }
}
