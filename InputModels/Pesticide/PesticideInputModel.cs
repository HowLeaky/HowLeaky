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
        public StateData tbPestVegIndex1 { get; set; }
        public StateData tbPestVegIndex2 { get; set; }
        public StateData tbPestVegIndex3 { get; set; }
        public StateData tbPestVegIndex4 { get; set; }
        public StateData tbPestVegIndex5 { get; set; }
        public StateData tbPestVegIndex6 { get; set; }
        public StateData tbPestVegIndex7 { get; set; }
        public StateData tbPestVegIndex8 { get; set; }
        public StateData tbPestVegIndex9 { get; set; }
        public StateData tbPestVegIndex10 { get; set; }

        public DayMonthData ApplicationDate { get; set; }

        //  public PesticideDatesAndRates
        public int TriggerGGDFirst { get; set; }
        public int TriggerGGDSubsequent { get; set; }
        public int TriggerDaysFirst { get; set; }
        public int TriggerDaysSubsequent { get; set; }
        public double ProductRate { get; set; }
        public double SubsequentProductRate { get; set; }

        public PestApplicationTiming() { }
    }

    [XmlRoot("PesticideType")]
    public class PesticideObjectDataModel : InputModel
    {
        //Input Parameters
        public PestApplicationTiming PestApplicationTiming { get; set; }
        public IndexData PestApplicationPosition { get; set; }          // Describes where the pesticide is applied relative to the crop. It is used to determine the fraction of the applied pesticide that is assumed to have been intercepted by the vegetation and/or stubble rather than entering the soil.        
        [Unit("days")]
        public double HalfLifeVeg { get; set; }                         // The time required (days) for a pesticide to under go dissipation or degradation to half of the initial concentration on the vegetation.       
        [Unit("oC")]
        public double RefTempHalfLifeVeg { get; set; }                  // The mean air temperature at which the Half-life (Veg) was determined (oC).      
        [Unit("days")]
        public double HalfLifeStubble { get; set; }                     // The time required (days) for a pesticide to under go dissipation or degradation to half of the initial concentration in the stubble.    
        [Unit("oC")]
        public double RefTempHalfLifeStubble { get; set; }              // The mean air temperature at which the Half-life (Stubble) was determined (oC).      
        [Unit("days")]
        public double HalfLife { get; set; }                            // The time required (days) for a pesticide to under go dissipation or degradation to half of the initial concentration in the soil.       
        [Unit("oC")]
        public double RefTempHalfLifeSoil { get; set; }                 // The mean air temperature at which the Half-life (Soil) was determined (oC).      
        [Unit("ug_per_L")]
        public double CritPestConc { get; set; }                        // Concentration of a pesticide that should not be exceeded in runoff (ug/L).      
        [Unit("g_per_L")]
        public double ConcActiveIngred { get; set; }                    // The concentration of the pesticide active ingredient (e.g. glyphosate) in the applied product (e.g. Roundup) (g/L). This value is multiplied by the application rate (L/ha) to calculate the amount of active ingredient applied (kg/ha).     
        [Unit("pc")]
        public double PestEfficiency { get; set; }                      // The percent of total applied pesticide (concentration of active ingredient x application rate) that is retained in the paddock (on the vegetation, stubble or soil) immediately following application. This percent may be less than 100 if there is significant spray drift or other losses between the point of application and the vegetation, stubble and soil.      
        [Unit("J_per_mol")]
        public double DegradationActivationEnergy { get; set; }         // The energetic threshold for thermal decomposition reactions (J/mol).   
        [Unit("mm")]
        public int MixLayerThickness { get; set; }                      // Depth of the surface soil layer into which an applied pesticide is mixed (mm). This depth is used to calculate a pesticide concentration in the soil following application.        
        public double SorptionCoefficient { get; set; }                 // The sorption coefficient is the ratio of the amount of pesticide bound to soil/sediment versus the amount in the water phase (Kd). Kd values can be derived empirically or estimated from published organic carbon sorption coefficients (Koc) where Kd=Koc x fraction of organic carbon.      
        public double ExtractCoefficient { get; set; }                  // The fraction of pesticide present in soil that will be extracted into runoff. This includes pesticides present in runoff in both the sorbed and dissolved phase. The value of 0.02 has been derived empirically (Silburn, DM, 2003. Characterising pesticide runoff from soil on cotton farms using a rainfall simulator. PhD Thesis, University of Sydney.) and was found to be relevant to data from a range of published studies.        
        public double CoverWashoffFraction { get; set; }                // The fraction of a pesticide that will move off the surface of the vegetation or stubble and into the soil following a rainfall event of greater than 5mm.        
        [Unit("pc")]
        public double BandSpraying { get; set; }                        // The percent area of a paddock to which a pesticide is applied.        

        public Sequence PestApplicationDateList { get; set; }
        public List<double> PestApplicationValueList { get; set; } = new List<double>();

        [XmlIgnore]
        public int ApplicationTiming { get { return PestApplicationTiming.index; } }
        [XmlIgnore]
        public DayMonthData ApplicationDate { get { return PestApplicationTiming.ApplicationDate; } }
        [Unit("L_per_ha")]
        [XmlIgnore]
        public double ProductRate { get { return PestApplicationTiming.ProductRate; } }
        [Unit("L_per_ha")]
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
        public PesticideObjectDataModel() { }
    }
}
