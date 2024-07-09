namespace mvc.Models
{
    public class PowerEquipmentStatusModel
    {


        public double? S029_101_ACPM_P_AMP_R { get; set; }
        public double? S029_101_ACPM_P_AMP_S { get; set; }
        public double? S029_101_ACPM_P_AMP_T { get; set; }
        public double? S029_101_ACPM_TOT_AMP { get; set; }
        public double? S029_101_ACPM_P_VOLT_R { get; set; }
        public double? S029_101_ACPM_P_VOLT_S { get; set; }
        public double? S029_101_ACPM_P_VOLT_T { get; set; }
        public double? S029_101_ACPM_TOT_VOLT { get; set; }
        public double? S029_101_ACPM_P_KW_R { get; set; }
        public double? S029_101_ACPM_P_KW_S { get; set; }
        public double? S029_101_ACPM_P_KW_T { get; set; }
        public double? S029_101_ACPM_TOT_KW { get; set; }
        public double? S029_101_ACPM_TOT_HZ { get; set; }
        public double? S029_101_ACPM_P_KVAR_R { get; set; }
        public double? S029_101_ACPM_P_KVAR_S { get; set; }
        public double? S029_101_ACPM_P_KVAR_T { get; set; }
        public double? S029_101_ACPM_TOT_KVAR { get; set; }
        public double? S029_101_ACPM_TOT_PF { get; set; }
        public double? S029_101_ACPM_TOT_KVARH { get; set; }
        public double? S029_101_ACPM_TOT_KWH { get; set; }
        public double? S029_101_ACPM_TOT_KWH_PV { get; set; }
        public double? S029_101_AC_PR { get; set; }
        public double? S029_101_ACPM_TOT_KVAR_CC { get; set; }
        public double? S029_101_ACPM_TOT_KW_CC { get; set; }
        public double? S029_101_ACPM_TOT_KVARH_CC { get; set; }
        public double? S029_101_ACPM_TOT_KWH_CC { get; set; }
        public double? S029_101_ACPM_TOT_VOLT_CC { get; set; }
        public double? S029_101_ACPM_COMM_ERROR { get; set; }

        public double? S029_201_DC_PV01_TEMP { get; set; }







        public List<List<string>>? SwitchNames { get; set; }
        public List<List<List<string>>>? PowerMeterDataNames { get; set; }

        public int? ACPMCount { get; set; }
        public int? DCPMCount {get;set;}
        public int? ACBCount {get;set;}
        public int? GCBCount {get;set;}
        public int? MCCBCount {get;set;}
        public int? VCBCount {get;set;}
        public int? TRCount {get;set;}
        public int? LPCount {get;set;}
    }
}
