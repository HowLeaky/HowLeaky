using HowLeaky.CustomAttributes;
using HowLeaky.DataModels;
using HowLeaky.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HowLeaky.DataModels
{
    public class CropClassCoefficients
    {
        [Input("a")]
        [XmlElement("a")]
        public double A { get; set; }
        [Input("b")]
        [XmlElement("b")]
        public double B { get; set; }
        [XmlIgnore]
        public double Daily { get; set; }
        [Input("Annual uptake")]
        [Unit("kg/ha")]
        [XmlElement("annual")]
        public double Annual { get; set; }

        public CropClassCoefficients() { }

        public CropClassCoefficients(double a, double b, double annual)
        {
            this.A = a;
            this.B = b;
            this.Annual = annual;
            this.Daily = annual / 365;
        }

        public void CalcDaily()
        {
            this.Daily = this.Annual / 365;
        }
    }

    public enum StageType { Fallow, Plant, Ratoon }

    [XmlRoot("DINNitratesType")]
    public class DINNitrateInputModel : NitrateInputModel
    {

        [Input("Main stem crop coefficients")]
        [XmlElement("plant")]
        public CropClassCoefficients Plant { get; set; } = new CropClassCoefficients(135, 0.035, 285);

        [Input("Ratoon crop coefficients")]
        [XmlElement("ratoon")]
        public CropClassCoefficients Ratoon { get; set; } = new CropClassCoefficients(92, 0.042, 285);

        [XmlIgnore]
        public CropClassCoefficients Current { get; set; }

        [Input("Denitrification")]
        [XmlElement("denitrification")]
        public double Denitrification { get; set; } = 0.03;

        [Input("Nitrate drainage retention")]
        [XmlElement("nitrateDrainageRetention")]
        public double NitrateDrainageRetention { get; set; } = 0.05;

        [Input("Mineralisation", "per annum")]
        [XmlElement("mineralisation")]
        public double Mineralisation { get; set; } = 60;

        [Input("Saturated water capacity")]
        [XmlElement("volSat")]
        public double VolSat { get; set; } = 300;

        [Input("Nitrogen added per year")]
        [XmlElement("nitrogenApplication")]
        public double NitrogenApplication { get; set; } = 475;

        [Input("Nitrogen Application Frequency", "days")]
        [XmlElement("nitrogenFrequency")]
        public double NitrogenFrequency { get; set; } = 14;

        [Input("Soil Carbon")]
        [XmlElement("soilCarbon")]
        public double SoilCarbon { get; set; } = 475;

        [Input("Initial Excess N", "kg/ha")]
        [XmlElement("initialExcessN")]
        public double InitialExcessN { get; set; } = 20;

        [Input("Main Stem Duration", "days")]
        [XmlElement("mainStemDuration")]
        public int MainStemDuration { get; set; } = 305;
    }
}

