using HowLeaky.CustomAttributes;
using HowLeaky.DataModels;
using HowLeaky.Tools.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowLeaky.InputModels
{
    public class ModelOptionsInputModel : InputModel
    {
        public DayMonthData ResetDateForResidue { get; set; }
        public DayMonthData ResetDateForSoilWater { get; set; }

        public bool ResetResidueAtDate { get; set; }
        [Unit("kg_per_ha")]
        public double ResetValueForResidue { get; set; }
        public bool ResetSoilWaterAtDate { get; set; }
        [Unit("pc")]
        public double ResetValueForSWAtDate { get; set; }
        public bool ResetSoilWaterAtPlanting { get; set; }
        [Unit("pc")]
        public double ResetValueForSWAtPlanting { get; set; }
        public bool CanCalculateLateralFlow { get; set; }
        public bool IgnoreCropKill { get; set; }
        public bool UsePERFECTDryMatterFn { get; set; }
        public bool UsePERFECTGroundCovFn { get; set; }
        public bool UsePERFECTSoilEvapFn { get; set; }
        public bool UsePERFECTLeafAreaFn { get; set; }
        public bool UsePERFECTResidueFn { get; set; }
        public bool UsePERFECT_USLE_LS_Fn { get; set; }
        public int UsePERFECTCurveNoFn { get; set; }
        public double InitialPAW { get; set; }

        public ModelOptionsInputModel() { }
    }
}
