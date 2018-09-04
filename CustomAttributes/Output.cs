using System;

namespace HowLeaky.CustomAttributes
{

    public enum AggregationTypeEnum { Mean, Sum, Current,  Max , InCropMean, InCropSum, InCropCurrent}
    public enum AggregationSequenceEnum { InCrop, Fallow, Always }


    public class Output : Attribute
    {
        public String Description { get; set; }
        public String Unit { get; set;}
        public double Scale { get; set; } = 1;

        public AggregationTypeEnum AggregationType { get; set; }
        public AggregationSequenceEnum AggregationSequence{ get; set; }

        public Output() { }

        public Output(string Description ="", string Unit = "", double Scale = 1, AggregationTypeEnum AggregationType = AggregationTypeEnum.Mean, AggregationSequenceEnum AggregationSequence = AggregationSequenceEnum.Always)
        {
            this.Description = Description;
            this.Unit = Unit;
            this.Scale = Scale;

            this.AggregationType = AggregationType;
            this.AggregationSequence = AggregationSequence;
        }
    }
}
