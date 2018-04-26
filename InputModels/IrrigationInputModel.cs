using HowLeaky.CustomAttributes;
using HowLeaky.Tools.DataObjects;
using System.Xml.Serialization;
using System.Xml;
using HowLeaky.XmlObjects;

namespace HowLeaky.DataModels
{
    

    [XmlRoot("IrrigationType")]
    public class IrrigationInputModel : InputModel
    {
        //Input Parameters
        //Xml Elements
        [Input("SWDToIrrigate","mm")]
        public double SWDToIrrigate { get; set; }                                                                                   // Soil water deficit amount for which to trigger an irrigation (mm).      
        [Input("FixedIrrigationAmount")]
        public double FixedIrrigationAmount { get; set; }                                                                           // Irrigation amount to apply during each irrigation (mm).        
        [Input("IrrigationBufferPeriod", "days")]
        public int IrrigationBufferPeriod { get; set; }                                                                             // Minimum days that should elapse between irrigations (days)        
        [Input("IrrigationFormat")]
        public IrrigationFormat IrrigationFormat { get; set; }
        [Input("TargetAmountOptions")]
        public TargetAmountOptions TargetAmountOptions { get; set; }
        [Input("IrrigationRunoffOptions")]
        public IrrigationRunoffOptions IrrigationRunoffOptions { get; set; }
        [Input("IrrigationEvaporationOptions")]
        public IrrigationEvaporationOptions IrrigationEvaporationOptions { get; set; }
        [Input("UseRingTank")]
        public StateData Ponding { get; set; }
        [XmlElement("UseRingTank")]
        [Input("RingTank")]
        public RingTank RingTank { get; set; }
        
        //Getters
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
        public int TargetAmountOpt { get { return TargetAmountOptions.index; } set { TargetAmountOptions.index = value; } }         // Select how much water to apply from: Field Capacity (DUL), Saturation, Fixed amount, DUL+25%, DUL+50%, DUL+75%        
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
        public bool UseRingTank { get { return RingTank.State; } }                                                                  // Switch to simulate ring-tank component during irrigation to limit supply.        
        [XmlIgnore]
        public bool ResetRingTank { get { return RingTank.ResetRingTank.State; } }                                                  // Switch to allow ring-tank capacity to be reset a predefined date. NOTE that this introduces a volumebalance error into calculations, but is used for modelling "partial" years conditions.       
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
        [Unit("ML_per_day")]
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
        public bool UsePonding { get { return Ponding.State; } }                                                                    // Switch to simulate ponding conditions which sets evaporation to potential evaporation.        

        /// <summary>
        /// 
        /// </summary>
        public IrrigationInputModel() { }
    }
    /// <summary>
    /// 
    /// </summary>
    public class IrrigationFormat : IndexData
    {
        [Input("IrrigationDates")]
        public Sequence IrrigationDates { get; set; }
        [Input("StartIrrigationWindow")]
        public DayMonthData StartIrrigationWindow { get; set; }
        [Input("EndIrrigationWindow")]
        public DayMonthData EndIrrigationWindow { get; set; }

        public IrrigationFormat() { }
    }
    /// <summary>
    /// 
    /// </summary>
    public class TargetAmountOptions : IndexData
    {
        [Input("IrrigationAmount")]
        public double IrrigationAmount { get; set; }

        public TargetAmountOptions() { }
    }
    /// <summary>
    /// 
    /// </summary>
    public class IrrigationRunoffOptions : IndexData
    {
        [Input("IrrigationRunoffSequence")]
        public Sequence IrrigationRunoffSequence { get; set; }
        [Input("tbIrrigationCoverEffects")]
        public IndexData tbIrrigationCoverEffects { get; set; }

        [Input("IrrigationRunoffProportion1")]
        public double IrrigationRunoffProportion1 { get; set; }
        [Input("IrrigationRunoffProportion2")]
        public double IrrigationRunoffProportion2 { get; set; }

        public IrrigationRunoffOptions() { }
    }
    /// <summary>
    /// 
    /// </summary>
    public class IrrigationEvaporationOptions : IndexData
    {
        [Input("IrrigationEvaporationProportion")]
        public double IrrigationEvaporationProportion { get; set; }

        public IrrigationEvaporationOptions() { }
    }
    /// <summary>
    /// 
    /// </summary>
    public class RingTank : StateData
    {
        [Input("AdditionalInflowFormat")]
        public AdditionalInflowFormat AdditionalInflowFormat { get; set; }
        [Input("ResetRingTank")]
        public ResetRingTank ResetRingTank { get; set; }

        [Input("RingTankDepth")]
        public double RingTankDepth { get; set; }
        [Input("RingTankArea")]
        public double RingTankArea { get; set; }
        [Input("CatchmentArea")]
        public double CatchmentArea { get; set; }
        [Input("IrrigatedArea")]
        public double IrrigatedArea { get; set; }
        [Input("RunoffCaptureRate")]
        public double RunoffCaptureRate { get; set; }
        [Input("RingTankSeepage")]
        public double RingTankSeepage { get; set; }
        [Input("RingTankEvapCoeficient")]
        public double RingTankEvapCoeficient { get; set; }
        [Input("IrrigationDeliveryEfficiency")]
        public double IrrigationDeliveryEfficiency { get; set; }

        public RingTank() { }
    }
    /// <summary>
    /// 
    /// </summary>
    public class AdditionalInflowFormat : IndexData
    {
        [Input("AdditionalInflow")]
        public double AdditionalInflow { get; set; }
        [Input("AdditionalInflowDateSequences")]
        public Sequence AdditionalInflowDateSequences { get; set; }

        public AdditionalInflowFormat() { }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ResetRingTank : StateData
    {
        [Input("RingTankResetDate")]
        public DayMonthData RingTankResetDate { get; set; }
        [Input("CapactityAtReset")]
        public double CapactityAtReset { get; set; }

        public ResetRingTank() { }
    }
}

