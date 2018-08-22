using HowLeaky.CustomAttributes;
using HowLeaky.Tools.DataObjects;
using HowLeaky.XmlObjects;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace HowLeaky.DataModels
{
    public enum EPestApplicationTiming { FixedDate, FromSequenceFile, GDDCrop1, GDDCrop2, GDDCrop3, DASCrop1, DASCrop2, DASCrop3, DaysSinceFallow };

    public enum EPestApplicationPosition { ApplyToVegetationLayer, ApplyToStubbleLayer, ApplyToSoilLayer };

    //Class for XML Deserialisation
    public class PestApplicationTiming : IndexData
    {
        [Input("tbPestVegIndex1")]
        public StateData tbPestVegIndex1 { get; set; }
        [Input("tbPestVegIndex2")]
        public StateData tbPestVegIndex2 { get; set; }
        [Input("tbPestVegIndex3")]
        public StateData tbPestVegIndex3 { get; set; }
        [Input("tbPestVegIndex4")]
        public StateData tbPestVegIndex4 { get; set; }
        [Input("tbPestVegIndex5")]
        public StateData tbPestVegIndex5 { get; set; }
        [Input("tbPestVegIndex6")]
        public StateData tbPestVegIndex6 { get; set; }
        [Input("tbPestVegIndex7")]
        public StateData tbPestVegIndex7 { get; set; }
        [Input("tbPestVegIndex8")]
        public StateData tbPestVegIndex8 { get; set; }
        [Input("tbPestVegIndex9")]
        public StateData tbPestVegIndex9 { get; set; }
        [Input("tbPestVegIndex10")]
        public StateData tbPestVegIndex10 { get; set; }

        [Input("ApplicationDate")]
        public DayMonthData ApplicationDate { get; set; }

        //  public PesticideDatesAndRates
        [Input("TriggerGGDFirst")]
        public int TriggerGGDFirst { get; set; }
        [Input("TriggerGGDSubsequent")]
        public int TriggerGGDSubsequent { get; set; }
        [Input("TriggerDaysFirst")]
        public int TriggerDaysFirst { get; set; }
        [Input("TriggerDaysSubsequent")]
        public int TriggerDaysSubsequent { get; set; }
        [Input("ProductRate")]
        public double ProductRate { get; set; }
        [Input("SubsequentProductRate")]
        public double SubsequentProductRate { get; set; }
        [Input("PesticideDatesAndRates")]
        public Sequence PesticideDatesAndRates { get; set; }



        public PestApplicationTiming() { }
    }

    [XmlRoot("PesticideType")]
    public class PesticideInputModel : InputModel
    {
        //Input Parameters
        [Input("")]
        public PestApplicationTiming PestApplicationTiming { get; set; }
        [Input("")]
        public IndexData PestApplicationPosition { get; set; }          // Describes where the pesticide is applied relative to the crop. It is used to determine the fraction of the applied pesticide that is assumed to have been intercepted by the vegetation and/or stubble rather than entering the soil.        
        [Input("","days")]
        public double HalfLifeVeg { get; set; }                         // The time required (days) for a pesticide to under go dissipation or degradation to half of the initial concentration on the vegetation.       
        [Input("", "°C")]
        public double RefTempHalfLifeVeg { get; set; }                  // The mean air temperature at which the Half-life (Veg) was determined (oC).      
        [Input("","days")]
        public double HalfLifeStubble { get; set; }                     // The time required (days) for a pesticide to under go dissipation or degradation to half of the initial concentration in the stubble.    
        [Input("", "°C")]
        public double RefTempHalfLifeStubble { get; set; }              // The mean air temperature at which the Half-life (Stubble) was determined (oC).      
        [Input("","days")]
        public double HalfLife { get; set; }                            // The time required (days) for a pesticide to under go dissipation or degradation to half of the initial concentration in the soil.       
        [Input("", "°C")]
        public double RefTempHalfLifeSoil { get; set; }                 // The mean air temperature at which the Half-life (Soil) was determined (oC).      
        [Input("","ug/L")]
        public double CritPestConc { get; set; }                        // Concentration of a pesticide that should not be exceeded in runoff (ug/L).      
        [Input("","g/L")]
        public double ConcActiveIngred { get; set; }                    // The concentration of the pesticide active ingredient (e.g. glyphosate) in the applied product (e.g. Roundup) (g/L). This value is multiplied by the application rate (L/ha) to calculate the amount of active ingredient applied (kg/ha).     
        [Input("","%")]
        public double PestEfficiency { get; set; }                      // The percent of total applied pesticide (concentration of active ingredient x application rate) that is retained in the paddock (on the vegetation, stubble or soil) immediately following application. This percent may be less than 100 if there is significant spray drift or other losses between the point of application and the vegetation, stubble and soil.      
        [Input("","J/mol")]
        public double DegradationActivationEnergy { get; set; }         // The energetic threshold for thermal decomposition reactions (J/mol).   
        [Input("","mm")]
        public int MixLayerThickness { get; set; }                      // Depth of the surface soil layer into which an applied pesticide is mixed (mm). This depth is used to calculate a pesticide concentration in the soil following application.        
        [Input("")]
        public double SorptionCoefficient { get; set; }                 // The sorption coefficient is the ratio of the amount of pesticide bound to soil/sediment versus the amount in the water phase (Kd). Kd values can be derived empirically or estimated from published organic carbon sorption coefficients (Koc) where Kd=Koc x fraction of organic carbon.      
        [Input("")]
        public double ExtractCoefficient { get; set; }                  // The fraction of pesticide present in soil that will be extracted into runoff. This includes pesticides present in runoff in both the sorbed and dissolved phase. The value of 0.02 has been derived empirically (Silburn, DM, 2003. Characterising pesticide runoff from soil on cotton farms using a rainfall simulator. PhD Thesis, University of Sydney.) and was found to be relevant to data from a range of published studies.        
        [Input("")]
        public double CoverWashoffFraction { get; set; }                // The fraction of a pesticide that will move off the surface of the vegetation or stubble and into the soil following a rainfall event of greater than 5mm.        
        [Input("","%")]
        public double BandSpraying { get; set; }                        // The percent area of a paddock to which a pesticide is applied.        

        [Input("")]
        public Sequence PestApplicationDateList { get; set; }
        [Input("")]
        public List<double> PestApplicationValueList { get; set; } = new List<double>();

        [XmlIgnore]
        public int ApplicationTiming { get { return PestApplicationTiming.index; } }
        [XmlIgnore]
        public DayMonthData ApplicationDate { get { return PestApplicationTiming.ApplicationDate; } }
        [Input("","L/ha")]
        [XmlIgnore]
        public double ProductRate { get { return PestApplicationTiming.ProductRate; } }
        [Input("","L/ha")]
        [XmlIgnore]
        public double SubsequentProductRate { get { return PestApplicationTiming.SubsequentProductRate; } }
        //[XmlIgnore]
        //public double in_PestApplicationSequence { get; set; }          //        
        [XmlIgnore]
        public int TriggerGGDFirst { get { return PestApplicationTiming.TriggerGGDFirst; } }
        [XmlIgnore]
        public int TriggerGGDSubsequent { get { return PestApplicationTiming.TriggerGGDSubsequent; } }
        [XmlIgnore]
        public int TriggerDaysFirst { get { return PestApplicationTiming.TriggerDaysFirst; } }
        [XmlIgnore]
        public int TriggerDaysSubsequent { get { return PestApplicationTiming.TriggerDaysSubsequent; } }
        [XmlIgnore]
        public int ApplicationPosition { get { return PestApplicationPosition.index; } }

        /// <summary>
        /// 
        /// </summary>
        public PesticideInputModel() { }
    }
}
