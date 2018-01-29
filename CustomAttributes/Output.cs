using System;

namespace HowLeaky.CustomAttributes
{
    public class Output : Attribute
    {
        public String Description { get; set; }
        public String Unit { get; set;}
        public double Scale { get; set; } = 1;

        public Output(string Description ="", string Unit = "", double Scale = 1)
        {
            this.Description = Description;
            this.Unit = Unit;
            this.Scale = Scale;
        }
    }
}
