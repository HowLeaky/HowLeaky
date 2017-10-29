using HowLeaky.CustomAttributes;
using HowLeaky.Tools.DataObjects;
using System.Xml.Serialization;
using System.Xml;
using HowLeaky.XmlObjects;

namespace HowLeaky.DataModels
{
    public class IrrigationFormat : IndexData
    {
        public Sequence IrrigationDates { get; set; }
        public DayMonthData StartIrrigationWindow { get; set; }
        public DayMonthData EndIrrigationWindow { get; set; }

        public IrrigationFormat() { }
    }

    public class TargetAmountOptions : IndexData
    {
        public double IrrigationAmount { get; set; }

        public TargetAmountOptions() { }
    }

    public class IrrigationRunoffOptions : IndexData
    {
        public Sequence IrrigationRunoffSequence { get; set; }
        public double IrrigationRunoffProportion1 { get; set; }
        public double IrrigationRunoffProportion2 { get; set; }
        public IndexData tbIrrigationCoverEffects { get; set; }

        public IrrigationRunoffOptions() { }
    }

    public class IrrigationEvaporationOptions : IndexData
    {
        public double IrrigationEvaporationProportion { get; set; }

        public IrrigationEvaporationOptions() { }
    }

    public class RingTank : StateData
    {
        public double RingTankDepth { get; set; }
        public double RingTankArea { get; set; }
        public double CatchmentArea { get; set; }
        public double IrrigatedArea { get; set; }
        public AdditionalInflowFormat AdditionalInflowFormat { get; set; }
        public double RunoffCaptureRate { get; set; }
        public double RingTankSeepage { get; set; }
        public double RingTankEvapCoeficient { get; set; }
        public double IrrigationDeliveryEfficiency { get; set; }
        public ResetRingTank ResetRingTank { get; set; }

        public RingTank() { }
    }

    public class AdditionalInflowFormat : IndexData
    {
        public double AdditionalInflow { get; set; }
        public Sequence AdditionalInflowDateSequences { get; set; }

        public AdditionalInflowFormat() { }
    }

    public class ResetRingTank : StateData
    {
        public DayMonthData RingTankResetDate { get; set; }
        public double CapactityAtReset { get; set; }

        public ResetRingTank() { }
    }

    [XmlRoot("IrrigationType")]
    public class IrrigationDataModel : DataModel
    {
        //Input Parameters
        //Xml Elements
        [Unit("mm")]
        public double SWDToIrrigate { get; set; }                                                                                   // Soil water deficit amount for which to trigger an irrigation (mm).      
        [Unit("days")]
        public int IrrigationBufferPeriod { get; set; }                                                                             // Minimum days that should elapse between irrigations (days)        
        public IrrigationFormat IrrigationFormat { get; set; }
        public TargetAmountOptions TargetAmountOptions { get; set; }
        public IrrigationRunoffOptions IrrigationRunoffOptions { get; set; }
        public IrrigationEvaporationOptions IrrigationEvaporationOptions { get; set; }
        public StateData Ponding { get; set; }
        [XmlElement("UseRingTank")]
        public RingTank RingTank { get; set; }


        //Getters
        [Unit("mm")]
        public double FixedIrrigationAmount { get; set; }                                                                           // Irrigation amount to apply during each irrigation (mm).        

        //IrrigationFormat
        [XmlIgnore]
        public int IrrigFormat { get { return IrrigationFormat.index; } }                                                           // Option to choose to irrigate when Soil-Water Reqires it ( while crop is growing, or in a predefined window) or at predefined dates and amounts<       
        [XmlIgnore]
        public DayMonthData IrrigWindowStartDate { get { return IrrigationFormat.StartIrrigationWindow; } }                         // Start date of window in which to consider irrigating.        
        [XmlIgnore]
        public DayMonthData IrrigWindowEndDate { get { return IrrigationFormat.EndIrrigationWindow; } }                             // End date of window in which to consider irrigating.        
        [XmlIgnore]
        public Sequence IrrigSequence { get { return IrrigationFormat.IrrigationDates; } }                                          // Predefined irrigation dates and amounts.       
        //TargetAmountOption
        [XmlIgnore]
        public int TargetAmountOpt { get { return TargetAmountOptions.index; } set { TargetAmountOptions.index = value; } }                                        // Select how much water to apply from: Field Capacity (DUL), Saturation, Fixed amount, DUL+25%, DUL+50%, DUL+75%        
        //IrrigationRunofOptions
        [XmlIgnore]
        public int IrrigRunoffOptions { get { return IrrigationRunoffOptions.index; } }                                             // How runoff is calculated during an irrigation including "Ignore Runoff", "Proportial to Application" and "Predefined Sequence".       
        [XmlIgnore]
        public int IrrigCoverEffects { get { return IrrigationRunoffOptions.tbIrrigationCoverEffects.index; } }                     // Cover effects options for EROSION calculations - choose which components affect erosion including "Canopy and Stubble", "Stubble only" or "None".        
        [XmlIgnore]
        [Unit("pc")]
        public double IrrigRunoffProportion1 { get { return IrrigationRunoffOptions.IrrigationRunoffProportion1; } }                 // Runoff percentage (of inflow) for first irrigation.       
        [XmlIgnore]
        [Unit("pc")]
        public double IrrigRunoffProportion2 { get { return IrrigationRunoffOptions.IrrigationRunoffProportion2; } }                // Runoff percentage (of inflow) for later irrigations.       
        [XmlIgnore]
        public Sequence IrrigRunoffSequence { get { return IrrigationRunoffOptions.IrrigationRunoffSequence; } }                    // Predefined runoff dates and amounts.       
        //RingTank
        [XmlIgnore]
        public bool UseRingTank { get { return RingTank.state; } }                                                                  // Switch to simulate ring-tank component during irrigation to limit supply.        
        [XmlIgnore]
        public bool ResetRingTank { get { return RingTank.ResetRingTank.state; } }                                                  // Switch to allow ring-tank capacity to be reset a predefined date. NOTE that this introduces a volumebalance error into calculations, but is used for modelling "partial" years conditions.       
        [XmlIgnore]
        public int AdditionalInflowFormat { get { return RingTank.AdditionalInflowFormat.index; } }                                 // Allows an additional inflow other than catchment runoff (for example, from Coal Seam Gas). Options include "Constant Inflow" and "Predefined Sequence"       
        [XmlIgnore]
        public DayMonthData ResetRingTankDate { get { return RingTank.ResetRingTank.RingTankResetDate; } }                          // date to reset ringtank capacity.        
        [XmlIgnore]
        public double RingTankSeepageRate { get { return RingTank.RingTankSeepage; } }                                              // Ring-tank seepage rate (mm/day) losses.       
        [XmlIgnore]
        [Unit("m")]
        public double RingTankDepth { get { return RingTank.RingTankDepth; } }                                                      // Depth of ring-tank (m)      
        [XmlIgnore]
        [Unit("ha")]
        public double RingTankArea { get { return RingTank.RingTankArea; } }                                                        // Ring-tank surface area (ha).       
        [XmlIgnore]
        [Unit("ha")]
        public double CatchmentArea { get { return RingTank.CatchmentArea; } }                                                      // Runoff catchment area (ha) which supplies water for ring-tank.     
        [XmlIgnore]
        [Unit("ha")]
        public double IrrigatedArea { get { return RingTank.IrrigatedArea; } }                                                      // Irrigated (paddock) area (ha) used to calculate how much water drained from ring-tank during an irrigation.       
        [XmlIgnore]
        [Unit("ML_per_day")]
        public double AdditionalInflow { get { return RingTank.AdditionalInflowFormat.AdditionalInflow; } }                         // Constant additional inflow (ML/day) into ringtank other than catchment runoff.        
        [XmlIgnore]
        [Unit("_ML_per_day")]
        public double RunoffCaptureRate { get { return RingTank.RunoffCaptureRate; } }                                              // Pumping capacity (ML/day) for capturing catchment runoff as input into the ring-tank;       
        [XmlIgnore]
        [Unit("mm_per_day")]
        public double RingTankEvapCoefficient { get { return RingTank.RingTankEvapCoeficient; } }                                   // Surface water evaporation coeficient for ring-tank.       
        [XmlIgnore]
        [Unit("pc")]
        public double IrrigDeliveryEfficiency { get { return RingTank.IrrigationDeliveryEfficiency; } }                             // Efficiency for getting water from the ring-tank to the paddock.       
        [XmlIgnore]
        [Unit("pc")]
        public double CapactityAtReset { get { return RingTank.ResetRingTank.CapactityAtReset; } }                                  // Ring-tank capacity (forced) at reset date.
        [XmlIgnore]
        public Sequence AdditionalInflowSequence { get { return RingTank.AdditionalInflowFormat.AdditionalInflowDateSequences; } }  // Predefined sequence (values and dates) of additional ring-tank inflow.       
        //Ponding
        [XmlIgnore]
        public bool UsePonding { get { return Ponding.state; } }                                                                    // Switch to simulate ponding conditions which sets evaporation to potential evaporation.        


        /// <summary>
        /// 
        /// </summary>
        public IrrigationDataModel() { }
    }
}

