
using Microsoft.AspNetCore.Razor.TagHelpers;
using Models.IndexModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using static NuGet.Client.ManagedCodeConventions;


namespace mvc.Models
{
    public class NowDBmanager
    {
        //-------------------------------V 3.2 Weather Temp -----------------------------------------------

        public readonly int PVBoxUnitCount = 0;
        public readonly int PVBoxCountPerUnit = 0;
        //--------------------------------V 3.1 Calculate Data---------------------------------------
        public readonly string dataNamePrefix;
        public readonly string totalkW = "S029_101_ACPM_TOT_KW";                        //毛發電功率 必填
        public readonly string totalGeneratedPowerRatio = "S029_000_PR_N";             //總發電比 -cal
        public readonly string totalPR_Now = "S029_000_PR";             //即時性能比(PR) -cal
        public readonly string dailykWh;                 //本日發電量 -cal
        public readonly string maxEfficiencyHrs;              //等效滿載發電時數 -cal
        public readonly string totalkWh = "S029_101_ACPM_TOT_KWH";   //總累積發電量 
        public readonly string solarRad = "S029_001_SOLAR_RAD";                      //日照強度 必填
        public readonly string airTemperature = "";                       //大氣溫度
        public readonly string shadetemperature = "S029_001_SHADOW_TEMP";              //遮陰溫度
        public readonly string arrayTemp = "S029_201_DC_PV01_TEMP";                            //陣列溫度
        public readonly string windSpeed = "";                      //風速
        public readonly string dailySolRad;                           //本日日照 -cal


        public readonly string DB_Password = "aa";
        public readonly string DB_UserID = "sa";


        public readonly List<string> caledOverviewDataNames;
        public readonly List<string> dataNamesToMinTbl;
        public readonly List<string> dataNamesToDailyTbl;
        public readonly List<string> dataNamesFromNowDB;
        public readonly List<string> dataNamesFromHistoryDB;

        public readonly int countCaledDataMin;
        public readonly int countCaledDataHr;

        //-----------------------------------V 3.0 History Data -----------------------
        public readonly List<string> overviewDataNames;
        //----------------------------------V 2.2 ----------------------------------


        public const string invKWH = "S029_3{i:D2}_INV_AC_TOT_KWH";             //變流器累積發電量命名規律

        public readonly string DCPMKWH = "";               //直流電表累積發電量命名規律

        public readonly string totalInvDCVolt = "";                             //全變流器直流電壓
        public readonly string totalInvDCAmp = "";                              //全變流器直流電流
        public readonly string totalInvDCkW = "";                               //全變流器直流功率
        public readonly string totalInvDCkWh = "";                              //全變流器直流發電量
        public readonly string totalInvDCRA = "";                               //全變流器直流發電比
        public readonly string totalInvACVolt = "";                             //全變流器交流電壓
        public readonly string totalInvACAmp = "";                              //全變流器交流電流
        public readonly string totalInvACkW = "";            //全變流器交流功率
        public readonly string totalInvACkVAR = "";                             //全變流器交流仟乏
        public readonly string totalInvACHz = "";                               //全變流器交流頻率
        public readonly string totalInvACPR = "";            //全變流器交流PR值
        public readonly string totalInvACDailykWh = "";                         //全變流器交流今日發電輛-不填
        public readonly string totalInvACkWh = "";          //全變流器交流發電量
        public readonly string totalSystemEffciency = "";              //總系統效率-不填
        public readonly string cO2 = "";


        public readonly string invACA = "";                                     //變流器電流名稱規律
        public readonly string invACV = "";                                     //變流器電壓名稱規律
        public readonly string invACkW = "";          //變流器功率名稱規律
        public readonly string invACkWh = invKWH;        //變流器發電量名稱規律-不須變更

        private readonly string columnNameFormat = "S029_3{0:D2}_INV_STA";  //變流器開關
        private readonly string propertyNameFormat = "CIInv{0:D2}Status";           //不須變更


        public readonly int ACPMCount = 1;                                        //電表數量
        public readonly int DCPMCount = 0;                                        //電表數量
        public readonly int ACBCount = 0;                                         //電表數量
        public readonly int GCBCount = 0;                                         //電表數量
        public readonly int MCCBCount = 0;                                        //電表數量
        public readonly int VCBCount = 0;                                         //電表數量
        public readonly int TRCount = 0;                                          //電表數量
        public readonly int LPCount = 0;                                          //電表數量
        public readonly int WPCount = 0;                                          //電表數量



        public readonly int weatherUnitCount = 1;                                    //氣象模組數量-取最大值
        public readonly int invCount = 10;                                            //變流器總數
        public readonly double InvCap = 258.00;                                          //裝置容量



        //開關名稱//
        public List<List<string>> switchNames;
        public readonly List<string> dsSwitchNames = new List<string>
        {

        };
        public readonly List<string> esSwitchNames = new List<string>
        {

        };
        public readonly List<string> acpmSwitchNames = new List<string>
        {


        };
        public readonly List<string> acbSwitchNames = new List<string>
        {


        };
        public readonly List<string> gcbSwitchNames = new List<string>
        {


        };
        public readonly List<string> vcbSwitchNames = new List<string>
        {

        };
        public readonly List<string> mccbSwitchNames = new List<string>
        {

        };
        public readonly List<string> wpSwitchNames = new List<string>
        {

        };




        //單線圖中顯示的各類電表數量(參考單線圖截圖)
        public List<List<List<string>>> powerMeterDataNames;
        public readonly int displayedACBCount = 1;
        public readonly int displayedGCBCount = 0;
        public readonly int displayedVCBCount = 0;
        public readonly int displayedMCCBCount = 0;
        public readonly int displayedWPCount = 0;


        public readonly string DeviceName = "DESKTOP-in8b58b";                         //電腦名稱
        public readonly string DB_Name = "S029";                                       //站點名稱
        public readonly int TableCount = 2;                                            //資料錶數量 
        private string[] Tables;
        private string ConnectionString;

        //全電表點位-使用string List
        private readonly string[] pesColumnNames = {

"S029_101_ACPM_P_AMP_R",
"S029_101_ACPM_P_AMP_S",
"S029_101_ACPM_P_AMP_T",
"S029_101_ACPM_TOT_AMP",
"S029_101_ACPM_P_VOLT_R",
"S029_101_ACPM_P_VOLT_S",
"S029_101_ACPM_P_VOLT_T",
"S029_101_ACPM_TOT_VOLT",
"S029_101_ACPM_P_KW_R",
"S029_101_ACPM_P_KW_S",
"S029_101_ACPM_P_KW_T",
"S029_101_ACPM_TOT_KW",
"S029_101_ACPM_TOT_HZ",
"S029_101_ACPM_P_KVAR_R",
"S029_101_ACPM_P_KVAR_S",
"S029_101_ACPM_P_KVAR_T",
"S029_101_ACPM_TOT_KVAR",
"S029_101_ACPM_TOT_PF",
"S029_101_ACPM_TOT_KVARH",
"S029_101_ACPM_TOT_KWH",
"S029_101_ACPM_TOT_KWH_PV",
"S029_101_AC_PR",
"S029_101_ACPM_TOT_KVAR_CC",
"S029_101_ACPM_TOT_KW_CC",
"S029_101_ACPM_TOT_KVARH_CC",
"S029_101_ACPM_TOT_KWH_CC",
"S029_101_ACPM_TOT_VOLT_CC",
"S029_101_ACPM_COMM_ERROR",
"S029_201_DC_PV01_TEMP"







                    };

        //全氣象溫度點位-使用string List
        private readonly string[] weatherTempNames =
        {

"S029_001_SOLAR_RAD",
"S029_001_SHADOW_TEMP",
"S029_201_DC_PV01_TEMP",






        };

        public List<string> BGDataNames;   //BGService-不須更改
        public List<string> GetKWHList(string kWHNames, int deviceCount)
        {
            List<string> KWHList = new List<string>();
            for (int i = 1; i <= deviceCount; i++)
            {
                string deviceId = "";
                string kwhName = "";
                if (kWHNames.Contains("{i:D2}"))
                {
                    deviceId = $"{i:D2}"; // Format i as string "{i:D2}", "02" etc
                    kwhName = kWHNames.Replace("{i:D2}", deviceId); // Replace {i} with deviceId

                }
                else
                {

                    deviceId = i.ToString(); // Format i as string "{i:D2}", "02" etc     
                    kwhName = kWHNames.Replace("{i}", deviceId); // Replace {i} with deviceId

                }

                KWHList.Add(kwhName);
            }
            return KWHList;
        }

        public Dictionary<string, double> TrimSolRadValues(Dictionary<string, double> solRads)
        {
            Dictionary<string, double> trimedValuesDict = new Dictionary<string, double>();

            foreach (KeyValuePair<string, double> solRad in solRads)
            {
                if (solRad.Value > 0)
                {
                    trimedValuesDict.Add(solRad.Key, solRad.Value);

                }
                else
                {
                    Console.WriteLine($"trimed solrad data off!!");
                }
            }
            return trimedValuesDict;
        }

        private readonly List<string> invColumnNames;

        public readonly List<string> eachInvCap = new List<string>          //個別變流器裝置容量//參考裝置容量單線圖
        {
        "24.0",
        "24.0",
        "24.0",
        "24.0",
        "24.0",
        "30.0",
        "30.0",
        "30.0",
        "24.0",
        "24.0",

        };

        public List<List<string>> GetDataKeys(double invCount)
        {
            List<List<string>> dataKeys = new List<List<string>>();

            for (int i = 1; i <= invCount; i++)
            {
                List<string> dataKey = new List<string>             //變流器資料名稱規律
            {
                $"", //"輸入端功率",
                $"",                   //"交流總實功率",
                $"",                   //"轉換效率",
                $"",                                                //"本日交流累積發電量",
                $"",                                                //"輸入端直流累積發電量",
                $"S029_3{i:D2}_INV_AC_TOT_KWH",                   //"交流總累積發電量",
                $"",                  //"輸入端直流電壓",
                $"",                    //"輸入端直流電流",
                $"S029_3{i:D2}_INV_DC_PV01_KW",    //"PV1-輸入端功率",
                $"",    //"PV1-輸入端直流累積發電量",
                $"S029_3{i:D2}_INV_DC_PV01_VOLT",    //"PV1-輸入端直流電壓",
                $"S029_3{i:D2}_INV_DC_PV01_AMP",    //"PV1-輸入端直流電流",
                $"S029_3{i:D2}_INV_DC_PV02_KW",    //"PV2-輸入端功率",
                $"",    //"PV2-輸入端直流累積發電量",
                $"S029_3{i:D2}_INV_DC_PV02_VOLT",    //"PV2-輸入端直流電壓",
                $"S029_3{i:D2}_INV_DC_PV02_AMP",    //"PV2-輸入端直流電流",
                $"",                              //"直流發電比",
                $"",                               //"交流相電壓",
                $"",                               //"交流相電流",
                $"",                                                //"交流總虛功率",
                $"",                    //"交流頻率",
                $"S029_3{i:D2}_INV_PR",                                               //"系統效能比PR",
            };
                dataKeys.Add(dataKey);
            }

            return dataKeys;

        }
        public List<string> DataNames
        {
            get
            {
                return new List<string>      //變流器資料名稱-有串(PV)才需變動
                {

                    "輸入端功率",
                    "交流總實功率",
                    "轉換效率",
                    "本日交流累積發電量",
                    "輸入端直流累積發電量",
                    "交流總累積發電量",
                    "輸入端直流電壓",
                    "輸入端直流電流",
                    "PV1-輸入端功率",
                    "PV1-輸入端直流累積發電量",
                    "PV1-輸入端直流電壓",
                    "PV1-輸入端直流電流",
                    "PV2-輸入端功率",
                    "PV2-輸入端直流累積發電量",
                    "PV2-輸入端直流電壓",
                    "PV2-輸入端直流電流",
                    "直流發電比",
                    "交流相電壓",
                    "交流相電流",
                    "交流總虛功率",
                    "交流頻率",
                    "系統效能比PR",
                };
            }
        }

        public List<string> Units
        {
            get
            {
                return new List<string>             //變流器資料單位-有串(PV)才需變動
                {
                    "kW",
                    "kW",
                    "%",
                    "kWh",
                    "kWh",
                    "kWh",
                    "V",
                    "A",
                    "kW",
                    "kWh",
                    "V",
                    "A",
                    "kW",
                    "kWh",
                    "V",
                    "A",
                    "Ra",
                    "V",
                    "A",
                    "kVAR",
                    "Hz",
                    "%",

                };
            }
        }


        //---------------------------------V 2.2-----------------------------------------------------------------------
        public List<List<List<string>>> pmNamesAndData = new List<List<List<string>>>();
        public List<string> PMDataNames
        {
            get
            {
                return new List<string>
                {

"總實功率",
"總虛功率",
"總視在功率",
"總累積發電量",
"負向總累積發電量",
"總仟乏小時",
"負向總仟乏小時",
"總視在電能",
"R相實功率",
"S相實功率",
"T相實功率",
"R相電壓",
"S相電壓",
"T相電壓",
"R-S線電壓",
"S-T線電壓",
"T-R線電壓",
"R相電流",
"S相電流",
"T相電流",
"R-S線電流",
"S-T線電流",
"T-R線電流",
"交流頻率",
"功率因數",

                };
            }
        }
        public List<string> PMUnits
        {
            get
            {
                return new List<string>
                {
"kW",
"kVAR",
"kVA",
"kWh",
"kWh",
"kVARh",
"kVARh",
"kVAh",
"kW",
"kW",
"kW",
"V",
"V",
"V",
"V",
"V",
"V",
"A",
"A",
"A",
"A",
"A",
"A",
"Hz",
    ""

                };
            }
        }

        public List<string> DCPMDataNames
        {
            get
            {
                return new List<string>
                {
                    $"發電量",
                    $"累積發電量",
                    $"直流電壓",
                    $"直流電流",
                    $"直流發電比",
                    $"陣列溫度",
                };
            }
        }
        public List<string> DCPMUnits
        {
            get
            {
                return new List<string>
                {
                    $"kW",
                    $"kWh",
                    $"A",
                    $"V",
                    $"RA",
                    $"°C",

                };
            }
        }

        //---------------------------------------------------分格線-----------------------------------------------------
        public NowDBmanager()
        {
            //----------------------------------V 3.1 Calculate Data ----------------------------------

            dataNamePrefix = $"{DB_Name}_000";
            dailykWh = $"{dataNamePrefix}_TODAYS_KWH";
            maxEfficiencyHrs = $"{dataNamePrefix}_MAX_EFF_HRS";
            dailySolRad = $"{dataNamePrefix}_TODAYS_SOLRAD";
            caledOverviewDataNames = new List<string>
            {
                dailykWh,
                maxEfficiencyHrs,
                dailySolRad,
                totalGeneratedPowerRatio,
                totalPR_Now
            };
            dataNamesFromHistoryDB = new List<string>
            {
                totalkWh,solarRad
            };
            dataNamesFromNowDB = new List<string>
            {
                totalkW,solarRad,
            };
            dataNamesToMinTbl = new List<string>
            {
                totalGeneratedPowerRatio,totalPR_Now
            };
            dataNamesToDailyTbl = new List<string>
            {
                dailykWh,maxEfficiencyHrs,dailySolRad
            };
            countCaledDataMin = dataNamesToMinTbl.Count;
            countCaledDataHr = dataNamesToDailyTbl.Count;
            //-----------------------------------V 3.0 Historical Data------------------------------------------------
            AlarmNames.Add(AllAlarmList);
            AlarmNames.Add(TrAlarmList);
            AlarmNames.Add(RelayAlarmList);
            AlarmNames.Add(InvAlarmList);
            AlarmNames.Add(OtherAlarmList);
            overviewDataNames = new List<string>
            {
                totalkW,
                totalGeneratedPowerRatio,
                totalPR_Now,
                totalkWh,
                solarRad,
                airTemperature,
                shadetemperature,
                arrayTemp,
                windSpeed,
                dailySolRad
            };
           
            //----------------------------------- V 2.2----------------------------------------------------------
            List<int> pmNames = new List<int>
            {
                ACPMCount,
                DCPMCount,
                ACBCount,
                GCBCount,
                MCCBCount,
                VCBCount,
                TRCount,
                LPCount,
                WPCount,
            };
            for (int j = 0; j < pmNames.Count; j++)
            {
                List<List<string>> dataKeys = new List<List<string>>();
                //ACPM
                if (j == 0)
                {
                    for (int i = 1; i <= pmNames[j]; i++)
                    {
                        List<string> acpmDataKey = new List<string>             //ACPM電表資料名稱規律
                        {
                           $"S029_1{i:D2}_ACPM_TOT_KW_CC",                                  //"總實功率",
                           $"S029_1{i:D2}_ACPM_TOT_KVAR_CC",                                //"總虛功率",
                           $"",                                    //"總視在功率",
                           $"S029_1{i:D2}_ACPM_TOT_KWH_PV",                             //"總累積發電量",
                           $"",                             //"負向總累積發電量",
                           $"S029_1{i:D2}_ACPM_TOT_KVARH_CC",                           //"總仟乏小時",
                           $"",                           //"負向總仟乏小時",
                           $"",                                   //"總視在電能",
                           $"S029_1{i:D2}_ACPM_P_KW_R",                                  //"R相實功率",
                           $"S029_1{i:D2}_ACPM_P_KW_S",                                  //"S相實功率",
                           $"S029_1{i:D2}_ACPM_P_KW_T",                                  //"T相實功率",
                           $"S029_1{i:D2}_ACPM_P_VOLT_R",                                     //"R相電壓",
                           $"S029_1{i:D2}_ACPM_P_VOLT_S",                                     //"S相電壓",
                           $"S029_1{i:D2}_ACPM_P_VOLT_T",                                     //"T相電壓",
                           $"",                               //"R-S線電壓",
                           $"",                               //"S-T線電壓",
                           $"",                               //"T-R線電壓",
                           $"S029_1{i:D2}_ACPM_P_AMP_R",                                 //"R相電流",
                           $"S029_1{i:D2}_ACPM_P_AMP_S",                                 //"S相電流",
                           $"S029_1{i:D2}_ACPM_P_AMP_T",                                 //"T相電流",
                           $"",                                    //"R-S線電流",
                           $"",                                    //"S-T線電流",
                           $"",                                    //"T-R線電流",
                           $"S029_1{i:D2}_ACPM_TOT_HZ",                                  //"交流頻率",
                           $"S029_1{i:D2}_ACPM_TOT_PF",                               //"功率因數",
                        };

                        dataKeys.Add(acpmDataKey);
                    }
                }
                //DCPM
                if (j == 1)
                {
                    for (int i = 1; i <= pmNames[j]; i++)
                    {
                        List<string> dcpmDataKey = new List<string>             //DCPM電表資料名稱規律
                        {
                            $"",                 //"發電量",
                            $"",                //"累積發電量",
                            $"",                   //"直流電壓",
                            $"",                 //"直流電流",
                            $"",                //"直流發電比",
                            $"S029_2{i:D2}_DC_PV01_TEMP",            //"陣列溫度",
                        };

                        dataKeys.Add(dcpmDataKey);
                    }
                }
                //ACB
                if (j == 2)
                {
                    for (int i = 1; i <= pmNames[j]; i++)
                    {
                        List<string> acbDataKey = new List<string>             //ACB電表資料名稱規律
                        {
                         $"",   //"總實功率",
                         $"",   //"總虛功率",
                         $"",   //"總視在功率",
                         $"",   //"總累積發電量",
                         $"",   //"負向總累積發電量",
                         $"",   //"總仟乏小時",
                         $"",   //"負向總仟乏小時",
                         $"",   //"總視在電能",
                         $"",   //"R相實功率",
                         $"",   //"S相實功率",
                         $"",   //"T相實功率",
                         $"",   //"R相電壓",
                         $"",   //"S相電壓",
                         $"",   //"T相電壓",
                         $"",   //"R-S線電壓",
                         $"",   //"S-T線電壓",
                         $"",   //"T-R線電壓",
                         $"",   //"R相電流",
                         $"",   //"S相電流",
                         $"",   //"T相電流",
                         $"",   //"R-S線電流",
                         $"",   //"S-T線電流",
                         $"",   //"T-R線電流",
                         $"",   //"交流頻率",
                         $"",   //"功率因數",
                        };

                        dataKeys.Add(acbDataKey);
                    }
                }
                //GCB
                if (j == 3)
                {
                    for (int i = 1; i <= pmNames[j]; i++)
                    {
                        List<string> gcbDataKey = new List<string>          //GCB電表資料名稱規律
                        {
                            $"", //"總實功率",
                            $"", //"總虛功率",
                            $"", //"總視在功率",
                            $"", //"總累積發電量",
                            $"", //"負向總累積發電量",
                            $"", //"總仟乏小時",
                            $"", //"負向總仟乏小時",
                            $"", //"總視在電能",
                            $"", //"R相實功率",
                            $"", //"S相實功率",
                            $"", //"T相實功率",
                            $"", //"R相電壓",
                            $"", //"S相電壓",
                            $"", //"T相電壓",
                            $"", //"R-S線電壓",
                            $"", //"S-T線電壓",
                            $"", //"T-R線電壓",
                            $"", //"R相電流",
                            $"", //"S相電流",
                            $"", //"T相電流",
                            $"", //"R-S線電流",
                            $"", //"S-T線電流",
                            $"", //"T-R線電流",
                            $"", //"交流頻率",
                            $"", //"功率因數",
                        };

                        dataKeys.Add(gcbDataKey);
                    }
                }
                //MCCB
                if (j == 4)
                {
                    for (int i = 1; i <= pmNames[j]; i++)
                    {
                        List<string> mccbDataKey = new List<string>
                        {
                            //總實功率
                            //總虛功率
                            //總視在功率
                            //總累積發電量
                            //負向總累積發電量
                            //總仟乏小時
                            //負向總仟乏小時
                            //總視在電能
                            //R相實功率
                            //S相實功率
                            //T相實功率
                            //R相電壓
                            //S相電壓
                            //T相電壓
                            //R-S線電壓
                            //S-T線電壓
                            //T-R線電壓
                            //R相電流
                            //S相電流
                            //T相電流
                            //R-S線電流
                            //S-T線電流
                            //T-R線電流
                            //交流頻率
                            //功率因數

                        };

                        dataKeys.Add(mccbDataKey);
                    }
                }
                //VCB
                if (j == 5)
                {
                    for (int i = 1; i <= pmNames[j]; i++)
                    {
                        List<string> vcbDataKey = new List<string>
                        {
                            //"總實功率",
                            //"總虛功率",
                            //"總視在功率",
                            //"總累積發電量",
                            //"負向總累積發電量",
                            //"總仟乏小時",
                            //"負向總仟乏小時",
                            //"總視在電能",
                            //"R相實功率",
                            //"S相實功率",
                            //"T相實功率",
                            //"R相電壓",
                            //"S相電壓",
                            //"T相電壓",
                            //"R-S線電壓",
                            //"S-T線電壓",
                            //"T-R線電壓",
                            //"R相電流",
                            //"S相電流",
                            //"T相電流",
                            //"R-S線電流",
                            //"S-T線電流",
                            //"T-R線電流",
                            //"交流頻率",
                            //"功率因數",
                        };

                        dataKeys.Add(vcbDataKey);
                    }
                }
                //TR
                if (j == 6)
                {
                    for (int i = 1; i <= pmNames[j]; i++)
                    {
                        List<string> trDataKey = new List<string>
                        {
                            //總實功率
                            //總虛功率
                            //總視在功率
                            //總累積發電量
                            //負向總累積發電量
                            //總仟乏小時
                            //負向總仟乏小時
                            //總視在電能
                            //R相實功率
                            //S相實功率
                            //T相實功率
                            //R相電壓
                            //S相電壓
                            //T相電壓
                            //R-S線電壓
                            //S-T線電壓
                            //T-R線電壓
                            //R相電流
                            //S相電流
                            //T相電流
                            //R-S線電流
                            //S-T線電流
                            //T-R線電流
                            //交流頻率
                            //功率因數
                        };

                        dataKeys.Add(trDataKey);
                    }
                }
                //LP
                if (j == 7)
                {
                    for (int i = 1; i <= pmNames[j]; i++)
                    {
                        List<string> lpDataKey = new List<string>
                        {
                            //總實功率
                            //總虛功率
                            //總視在功率
                            //總累積發電量
                            //負向總累積發電量
                            //總仟乏小時
                            //負向總仟乏小時
                            //總視在電能
                            //R相實功率
                            //S相實功率
                            //T相實功率
                            //R相電壓
                            //S相電壓
                            //T相電壓
                            //R-S線電壓
                            //S-T線電壓
                            //T-R線電壓
                            //R相電流
                            //S相電流
                            //T相電流
                            //R-S線電流
                            //S-T線電流
                            //T-R線電流
                            //交流頻率
                            //功率因數
                        };

                        dataKeys.Add(lpDataKey);
                    }
                }
                //WP
                if (j == 8)
                {
                    for (int i = 1; i <= pmNames[j]; i++)
                    {
                        List<string> wpDataKey = new List<string>
                        {
                            //總實功率
                            //總虛功率
                            //總視在功率
                            //總累積發電量
                            //負向總累積發電量
                            //總仟乏小時
                            //負向總仟乏小時
                            //總視在電能
                            //R相實功率
                            //S相實功率
                            //T相實功率
                            //R相電壓
                            //S相電壓
                            //T相電壓
                            //R-S線電壓
                            //S-T線電壓
                            //T-R線電壓
                            //R相電流
                            //S相電流
                            //T相電流
                            //R-S線電流
                            //S-T線電流
                            //T-R線電流
                            //交流頻率
                            //功率因數
                        };

                        dataKeys.Add(wpDataKey);
                    }
                }

                pmNamesAndData.Add(dataKeys);
            }

            //--------------------------------------V 2.0------------------------------------------------------------
            if (TableCount > 1)
            {
                Tables = new string[TableCount];
                for (int i = 1; i <= TableCount; i++)
                {
                    Tables[i - 1] = $"{DB_Name}P{i}_Instant_data";
                }
            }
            else
            {
                Tables = new string[TableCount];
                Tables[0] = $"{DB_Name}_Instant_data";
            }

            ConnectionString = GetConnectionString();

            invColumnNames = new List<string>();

            for (int i = 1; i <= invCount; i++)
            {
                //變流器名稱規律-使用NowDBmanager_Inv複製
                invColumnNames.Add($"S029_3{i:D2}_INV_DC_PV01_KW");
                invColumnNames.Add($"S029_3{i:D2}_INV_DC_PV02_KW");
                invColumnNames.Add($"S029_3{i:D2}_INV_AC_TOT_KWH");
                invColumnNames.Add($"S029_3{i:D2}_INV_DC_PV01_VOLT");
                invColumnNames.Add($"S029_3{i:D2}_INV_DC_PV02_VOLT");
                invColumnNames.Add($"S029_3{i:D2}_INV_DC_PV01_AMP");
                invColumnNames.Add($"S029_3{i:D2}_INV_DC_PV02_AMP");
                invColumnNames.Add($"S029_3{i:D2}_INV_PR");
                invColumnNames.Add($"S029_3{i:D2}_INV_AC_P_AMP_R");
                invColumnNames.Add($"S029_3{i:D2}_INV_AC_P_AMP_S");
                invColumnNames.Add($"S029_3{i:D2}_INV_AC_P_AMP_T");
                invColumnNames.Add($"S029_3{i:D2}_INV_AC_P_KW_R");
                invColumnNames.Add($"S029_3{i:D2}_INV_AC_P_KW_S");
                invColumnNames.Add($"S029_3{i:D2}_INV_AC_P_KW_T");
                invColumnNames.Add($"S029_3{i:D2}_INV_AC_P_VOLT_R");
                invColumnNames.Add($"S029_3{i:D2}_INV_AC_P_VOLT_S");
                invColumnNames.Add($"S029_3{i:D2}_INV_AC_P_VOLT_T");
                invColumnNames.Add($"S029_3{i:D2}_INV_AC_P_HZ_R");
                invColumnNames.Add($"S029_3{i:D2}_INV_AC_P_HZ_S");
                invColumnNames.Add($"S029_3{i:D2}_INV_AC_P_HZ_T");
                invColumnNames.Add($"S029_3{i:D2}_INV_STA");
                invColumnNames.Add($"S029_3{i:D2}_INV_MSG_FAULT_STA_00");
                invColumnNames.Add($"S029_3{i:D2}_INV_MSG_FAULT_STA_01");
                invColumnNames.Add($"S029_3{i:D2}_INV_MSG_FAULT_STA_02");
                invColumnNames.Add($"S029_3{i:D2}_INV_MSG_FAULT_STA_03");
                invColumnNames.Add($"S029_3{i:D2}_INV_MSG_FAULT_STA_04");
                invColumnNames.Add($"S029_3{i:D2}_INV_MSG_ERROR_STA_00");
                invColumnNames.Add($"S029_3{i:D2}_INV_MSG_ERROR_STA_01");
                invColumnNames.Add($"S029_3{i:D2}_INV_MSG_WARNING_STA_00");
                invColumnNames.Add($"S029_3{i:D2}_INV_MSG_WARNING_STA_01");
                invColumnNames.Add($"S029_3{i:D2}_INV_MSG_WARNING_STA_02");
                invColumnNames.Add($"S029_3{i:D2}_INV_COMM_ERROR");

            }
            //---------------------------------- V 3.1 Calculated Data ----------------------------------
            BGDataNames = new List<string>
            {
            solarRad,
            totalkWh,

            };
            BGDataNames.AddRange(GetKWHList(invKWH, invCount));
            BGDataNames.AddRange(GetKWHList(DCPMKWH, DCPMCount));

            //---------------------------------V 3.2 Weather Temp---------------------------------------------------------
            for (int i = 1; i <= weatherUnitCount; i++)
            {
                List<string> weatherdataKey = new List<string> //氣象模組點位名稱規律
                {
                     $"S029_0{i:D2}_SOLAR_RAD",     //        "日照強度",
                     $"",          //                        "氣溫",
                     $"S029_0{i:D2}_SHADOW_TEMP",   //        "遮陰溫度",
                     $"S029_2{i:D2}_DC_PV01_TEMP",          //        "模組溫度",
                     $""                        //           "變壓器溫度"
                };
                weatherdataKeys.Add(weatherdataKey);

            }

            for (int i = 1; i <= LPCount; i++)
            {
                List<string> LPDataKey = new List<string>//廠內用電點位名稱規律
                {
                    $"",                                    // 廠用電壓(V)
                    $"",                                    // 廠用電流(A)
                    $"",                                    // 即時廠用電功率(kW)
                    $"",                                    // 累計廠用電量(mWh)
                };
                LPDataKeys.Add(LPDataKey);
            }

            for (int i = 0; i < PVBoxUnitCount; i++)
            {
                List<string> PVBDataKey = new List<string>();//PVB點位名稱規律
                for (int j = 0; j < PVBoxCountPerUnit; j++)
                {
                    PVBDataKey.Add($""); //PVB室內溫度(°C)
                }
                PVBDataKeys.Add(PVBDataKey);
            }
            //--------------------------------- V 2.2 ------------------------------------------------
            switchNames = new List<List<string>>
            {
                dsSwitchNames,
                esSwitchNames,
                acpmSwitchNames,
                acbSwitchNames,
                gcbSwitchNames,
                vcbSwitchNames,
                mccbSwitchNames,
                wpSwitchNames
            };


            //電表 電壓,電流,功率,發電量 名稱//
            int[] deviceNameCounts = { displayedACBCount, displayedGCBCount, displayedVCBCount, displayedMCCBCount, displayedWPCount };
            powerMeterDataNames = new List<List<List<string>>>();
            for (int j = 0; j < deviceNameCounts.Length; j++)
            {
                List<List<string>>? acbLists = new List<List<string>>();
                for (int i = 1; i <= displayedACBCount; i++)
                {

                    List<string> acbList = new List<string>
                    {
                        //ACB電表點位名稱規律
                        $"S029_1{i:D2}_ACPM_P_VOLT_R",
                        $"S029_1{i:D2}_ACPM_P_VOLT_S",
                        $"S029_1{i:D2}_ACPM_P_VOLT_T",
                        $"S029_1{i:D2}_ACPM_P_AMP_R",
                        $"S029_1{i:D2}_ACPM_P_AMP_S",
                        $"S029_1{i:D2}_ACPM_P_AMP_T",
                        $"S029_1{i:D2}_ACPM_TOT_KW",
                        $"S029_1{i:D2}_ACPM_TOT_KWH",

                    };
                    acbLists.Add(acbList);
                }
                List<List<string>>? gcbLists = new List<List<string>>();
                for (int i = 1; i <= displayedGCBCount; i++)
                {
                    List<string>? gcbList = new List<string>
                    {
                        //GCB電表點位名稱規律
                        $"",
                        $"",
                        $"",
                        $"",
                        $"",
                        $"",
                        $"",
                        $"",


                    };
                    gcbLists.Add(gcbList);
                }
                List<List<string>>? vcbLists = new List<List<string>>();
                for (int i = 1; i <= displayedVCBCount; i++)
                {
                    List<string>? vcbList = new List<string>
                    {
                        //VCB電表點位名稱規律
                        $"",
                        $"",
                        $"",
                        $"",
                        $"",
                        $"",
                        $"",
                        $"",
                    };
                    vcbLists.Add(vcbList);
                }
                List<List<string>>? mccbLists = new List<List<string>>();
                for (int i = 1; i <= displayedMCCBCount; i++)
                {
                    List<string>? mccbList = new List<string>
                    {
                        //MCCB電表點位名稱規律
                        $"",
                        $"",
                        $"",
                        $"",
                        $"",
                        $"",
                        $"",
                        $"",
                    };
                    mccbLists.Add(mccbList);
                }
                List<List<string>>? wpLists = new List<List<string>>();
                for (int i = 1; i <= displayedWPCount; i++)
                {
                    List<string>? wpList = new List<string>
                    {
                        //WP電表點位名稱規律
                        $"",
                        $"",
                        $"",
                        $"",
                        $"",
                        $"",
                        $"",
                        $"",
                    };
                    wpLists.Add(wpList);
                }

                powerMeterDataNames.Add(acbLists);
                powerMeterDataNames.Add(gcbLists);
                powerMeterDataNames.Add(vcbLists);
                powerMeterDataNames.Add(mccbLists);
                powerMeterDataNames.Add(wpLists);
            }
        }







        //-----------------------------V 3.0 Historical Data-------------------------------------------
        public readonly List<string> AllAlarmList = new List<string>
        {
"S029_301_INV_MSG_FAULT_STA_00",
"S029_301_INV_MSG_FAULT_STA_01",
"S029_301_INV_MSG_FAULT_STA_02",
"S029_301_INV_MSG_FAULT_STA_03",
"S029_301_INV_MSG_FAULT_STA_04",
"S029_301_INV_MSG_ERROR_STA_01",
"S029_301_INV_MSG_WARNING_STA_00",
"S029_301_INV_MSG_WARNING_STA_01",
"S029_301_INV_MSG_WARNING_STA_02",
"S029_302_INV_MSG_FAULT_STA_00",
"S029_302_INV_MSG_FAULT_STA_01",
"S029_302_INV_MSG_FAULT_STA_02",
"S029_302_INV_MSG_FAULT_STA_03",
"S029_302_INV_MSG_FAULT_STA_04",
"S029_302_INV_MSG_ERROR_STA_00",
"S029_302_INV_MSG_ERROR_STA_01",
"S029_302_INV_MSG_WARNING_STA_00",
"S029_302_INV_MSG_WARNING_STA_01",
"S029_302_INV_MSG_WARNING_STA_02",
"S029_303_INV_MSG_FAULT_STA_00",
"S029_303_INV_MSG_FAULT_STA_01",
"S029_303_INV_MSG_FAULT_STA_02",
"S029_303_INV_MSG_FAULT_STA_03",
"S029_303_INV_MSG_FAULT_STA_04",
"S029_303_INV_MSG_ERROR_STA_00",
"S029_303_INV_MSG_ERROR_STA_01",
"S029_303_INV_MSG_WARNING_STA_00",
"S029_303_INV_MSG_WARNING_STA_01",
"S029_303_INV_MSG_WARNING_STA_02",
"S029_304_INV_MSG_FAULT_STA_00",
"S029_304_INV_MSG_FAULT_STA_01",
"S029_304_INV_MSG_FAULT_STA_02",
"S029_304_INV_MSG_FAULT_STA_03",
"S029_304_INV_MSG_FAULT_STA_04",
"S029_304_INV_MSG_ERROR_STA_00",
"S029_304_INV_MSG_ERROR_STA_01",
"S029_304_INV_MSG_WARNING_STA_00",
"S029_304_INV_MSG_WARNING_STA_01",
"S029_304_INV_MSG_WARNING_STA_02",
"S029_305_INV_MSG_FAULT_STA_00",
"S029_305_INV_MSG_FAULT_STA_01",
"S029_305_INV_MSG_FAULT_STA_02",
"S029_305_INV_MSG_FAULT_STA_03",
"S029_305_INV_MSG_FAULT_STA_04",
"S029_305_INV_MSG_ERROR_STA_00",
"S029_305_INV_MSG_ERROR_STA_01",
"S029_305_INV_MSG_WARNING_STA_00",
"S029_305_INV_MSG_WARNING_STA_01",
"S029_305_INV_MSG_WARNING_STA_02",
"S029_306_INV_MSG_FAULT_STA_00",
"S029_306_INV_MSG_FAULT_STA_01",
"S029_306_INV_MSG_FAULT_STA_02",
"S029_306_INV_MSG_FAULT_STA_03",
"S029_306_INV_MSG_FAULT_STA_04",
"S029_306_INV_MSG_ERROR_STA_00",
"S029_306_INV_MSG_ERROR_STA_01",
"S029_306_INV_MSG_WARNING_STA_00",
"S029_306_INV_MSG_WARNING_STA_01",
"S029_306_INV_MSG_WARNING_STA_02",
"S029_307_INV_MSG_FAULT_STA_00",
"S029_307_INV_MSG_FAULT_STA_01",
"S029_307_INV_MSG_FAULT_STA_02",
"S029_307_INV_MSG_FAULT_STA_03",
"S029_307_INV_MSG_FAULT_STA_04",
"S029_307_INV_MSG_ERROR_STA_00",
"S029_307_INV_MSG_ERROR_STA_01",
"S029_307_INV_MSG_WARNING_STA_00",
"S029_307_INV_MSG_WARNING_STA_01",
"S029_307_INV_MSG_WARNING_STA_02",
"S029_308_INV_MSG_FAULT_STA_00",
"S029_308_INV_MSG_FAULT_STA_01",
"S029_308_INV_MSG_FAULT_STA_02",
"S029_308_INV_MSG_FAULT_STA_03",
"S029_308_INV_MSG_FAULT_STA_04",
"S029_308_INV_MSG_ERROR_STA_00",
"S029_308_INV_MSG_ERROR_STA_01",
"S029_308_INV_MSG_WARNING_STA_00",
"S029_308_INV_MSG_WARNING_STA_01",
"S029_308_INV_MSG_WARNING_STA_02",
"S029_309_INV_MSG_FAULT_STA_00",
"S029_309_INV_MSG_FAULT_STA_01",
"S029_309_INV_MSG_FAULT_STA_02",
"S029_309_INV_MSG_FAULT_STA_03",
"S029_309_INV_MSG_FAULT_STA_04",
"S029_309_INV_MSG_ERROR_STA_00",
"S029_309_INV_MSG_ERROR_STA_01",
"S029_309_INV_MSG_WARNING_STA_00",
"S029_309_INV_MSG_WARNING_STA_01",
"S029_309_INV_MSG_WARNING_STA_02",
"S029_310_INV_MSG_FAULT_STA_00",
"S029_310_INV_MSG_FAULT_STA_01",
"S029_310_INV_MSG_FAULT_STA_02",
"S029_310_INV_MSG_FAULT_STA_03",
"S029_310_INV_MSG_FAULT_STA_04",
"S029_310_INV_MSG_ERROR_STA_00",
"S029_310_INV_MSG_ERROR_STA_01",
"S029_310_INV_MSG_WARNING_STA_00",
"S029_310_INV_MSG_WARNING_STA_01",
"S029_310_INV_MSG_WARNING_STA_02",

        };
        public readonly List<string> TrAlarmList = new List<string>
        {

        };
        public readonly List<string> RelayAlarmList = new List<string>
        {

        };
        public readonly List<string> InvAlarmList = new List<string>
        {

        };
        public readonly List<string> OtherAlarmList = new List<string>
        {

        };
        public readonly List<List<string>> AlarmNames = new List<List<string>>();
        public readonly Dictionary<string, string> replacementDictionary = new Dictionary<string, string>
        {
{"S029_101_ACPM_COMM_ERROR","彰林ES258kWp_AC電表連線狀態"},
{"S029_301_INV_COMM_ERROR","彰林ES258kWp_Inverter01連線狀態"},
{"S029_302_INV_COMM_ERROR","彰林ES258kWp_Inverter02連線狀態"},
{"S029_303_INV_COMM_ERROR","彰林ES258kWp_Inverter03連線狀態"},
{"S029_304_INV_COMM_ERROR","彰林ES258kWp_Inverter04連線狀態"},
{"S029_305_INV_COMM_ERROR","彰林ES258kWp_Inverter05連線狀態"},
{"S029_306_INV_COMM_ERROR","彰林ES258kWp_Inverter06連線狀態"},
{"S029_307_INV_COMM_ERROR","彰林ES258kWp_Inverter07連線狀態"},
{"S029_308_INV_COMM_ERROR","彰林ES258kWp_Inverter08連線狀態"},
{"S029_309_INV_COMM_ERROR","彰林ES258kWp_Inverter09連線狀態"},
{"S029_310_INV_COMM_ERROR","彰林ES258kWp_Inverter10連線狀態"},
{"S029_001_METEOR_COMM_ERROR","彰林ES258kWp_氣象模組連線狀態"},
{"S029_000_PR","彰林ES258kWp_總性能比"},
{"S029_000_PR_N","彰林ES258kWp_總發電比"},
{"S029_000_SYS_EFF","彰林ES258kWp_總系統效率"},
{"S029_001_SHADOW_TEMP","彰林ES258kWp_遮陰溫度值"},
{"S029_001_SOLAR_RAD","彰林ES258kWp_日照值"},
{"S029_101_ACPM_TOT_KVAR","彰林ES258kWp_AC交流集合式電錶01-乏"},
{"S029_101_ACPM_TOT_KVAR_CC","彰林ES258kWp_AC總虛功率"},
{"S029_101_ACPM_TOT_KW","彰林ES258kWp_AC交流集合式電錶01-瓦"},
{"S029_101_ACPM_TOT_KW_CC","彰林ES258kWp_AC總實功率"},
{"S029_101_ACPM_TOT_AMP","彰林ES258kWp_AC交流集合式電錶01-電流-總"},
{"S029_101_ACPM_P_AMP_R","彰林ES258kWp_AC交流集合式電錶01-R相電流"},
{"S029_101_ACPM_P_AMP_S","彰林ES258kWp_AC交流集合式電錶01-S相電流"},
{"S029_101_ACPM_P_AMP_T","彰林ES258kWp_AC交流集合式電錶01-T相電流"},
{"S029_101_ACPM_TOT_HZ","彰林ES258kWp_AC交流集合式電錶01-頻率"},
{"S029_101_ACPM_TOT_KVARH","彰林ES258kWp_AC交流集合式電錶01-負向瓦時"},
{"S029_101_ACPM_TOT_KVARH_CC","彰林ES258kWp_AC仟乏時"},
{"S029_101_ACPM_P_KVAR_R","彰林ES258kWp_AC交流集合式電錶01-R相虛功率"},
{"S029_101_ACPM_P_KVAR_S","彰林ES258kWp_AC交流集合式電錶01-S相虛功率"},
{"S029_101_ACPM_P_KVAR_T","彰林ES258kWp_AC交流集合式電錶01-T相虛功率"},
{"S029_101_ACPM_TOT_KWH","彰林ES258kWp_AC交流集合式電錶01-瓦時"},
{"S029_101_ACPM_TOT_KWH_CC","彰林ES258kWp_AC仟瓦時"},
{"S029_101_ACPM_TOT_KWH_PV","彰林ES258kWp_本日累計發電量"},
{"S029_101_ACPM_P_KW_R","彰林ES258kWp_AC交流集合式電錶01-R相實功率"},
{"S029_101_ACPM_P_KW_S","彰林ES258kWp_AC交流集合式電錶01-S相實功率"},
{"S029_101_ACPM_P_KW_T","彰林ES258kWp_AC交流集合式電錶01-T相實功率"},
{"S029_101_ACPM_TOT_PF","彰林ES258kWp_AC交流集合式電錶01-功因"},
{"S029_101_ACPM_TOT_VOLT","彰林ES258kWp_AC交流集合式電錶01-電壓-總"},
{"S029_101_ACPM_TOT_VOLT_CC","彰林ES258kWp_AC電壓"},
{"S029_101_ACPM_P_VOLT_R","彰林ES258kWp_AC交流集合式電錶01-R相電壓"},
{"S029_101_ACPM_P_VOLT_S","彰林ES258kWp_AC交流集合式電錶01-S相電壓"},
{"S029_101_ACPM_P_VOLT_T","彰林ES258kWp_AC交流集合式電錶01-T相電壓"},
{"S029_201_DC_PV01_TEMP","彰林ES258kWp_模組溫度值"},
{"S029_301_INV_AC_TOT_KWH","彰林ES258kWp_Inverter01_AC-瓩時"},
{"S029_301_INV_AC_P_AMP_R","彰林ES258kWp_Inverter01_AC-R相電流"},
{"S029_301_INV_AC_P_AMP_S","彰林ES258kWp_Inverter01_AC-S相電流"},
{"S029_301_INV_AC_P_AMP_T","彰林ES258kWp_Inverter01_AC-T相電流"},
{"S029_301_INV_AC_P_HZ_R","彰林ES258kWp_Inverter01_AC-R相頻率"},
{"S029_301_INV_AC_P_HZ_S","彰林ES258kWp_Inverter01_AC-S相頻率"},
{"S029_301_INV_AC_P_HZ_T","彰林ES258kWp_Inverter01_AC-T相頻率"},
{"S029_301_INV_AC_P_KW_R","彰林ES258kWp_Inverter01_AC-R相功率"},
{"S029_301_INV_AC_P_KW_S","彰林ES258kWp_Inverter01_AC-S相功率"},
{"S029_301_INV_AC_P_KW_T","彰林ES258kWp_Inverter01_AC-T相功率"},
{"S029_301_INV_AC_P_VOLT_R","彰林ES258kWp_Inverter01_AC-R相電壓"},
{"S029_301_INV_AC_P_VOLT_S","彰林ES258kWp_Inverter01_AC-S相電壓"},
{"S029_301_INV_AC_P_VOLT_T","彰林ES258kWp_Inverter01_AC-T相電壓"},
{"S029_301_INV_MSG_FAULT_STA_00","彰林ES258kWp_Inverter01故障點訊息0"},
{"S029_301_INV_MSG_FAULT_STA_01","彰林ES258kWp_Inverter01_故障點訊息1"},
{"S029_301_INV_MSG_FAULT_STA_02","彰林ES258kWp_Inverter01_故障點訊息2"},
{"S029_301_INV_MSG_FAULT_STA_03","彰林ES258kWp_Inverter01_故障點訊息3"},
{"S029_301_INV_MSG_FAULT_STA_04","彰林ES258kWp_Inverter01_故障點訊息4"},
{"S029_301_INV_DC_PV01_AMP","彰林ES258kWp_Inverter01_DC-電流_串列1"},
{"S029_301_INV_DC_PV02_AMP","彰林ES258kWp_Inverter01_DC-電流_串列2"},
{"S029_301_INV_DC_PV01_KW","彰林ES258kWp_Inverter01_DC-實功率_串列1"},
{"S029_301_INV_DC_PV02_KW","彰林ES258kWp_Inverter01_DC-實功率_串列2"},
{"S029_301_INV_DC_PV01_VOLT","彰林ES258kWp_Inverter01_DC-電壓_串列1"},
{"S029_301_INV_DC_PV02_VOLT","彰林ES258kWp_Inverter01_DC-電壓_串列2"},
{"S029_301_INV_MSG_ERROR_STA_00","彰林ES258kWp_Inverter01_錯誤點訊息0"},
{"S029_301_INV_MSG_ERROR_STA_01","彰林ES258kWp_Inverter01_錯誤點訊息1"},
{"S029_301_INV_STA","彰林ES258kWp_Inverter01狀態"},
{"S029_301_INV_MSG_WARNING_STA_00","彰林ES258kWp_Inverter01_警告點訊息0"},
{"S029_301_INV_MSG_WARNING_STA_01","彰林ES258kWp_Inverter01_警告點訊息1"},
{"S029_301_INV_MSG_WARNING_STA_02","彰林ES258kWp_Inverter01_警告點訊息2"},
{"S029_302_INV_AC_TOT_KWH","彰林ES258kWp_Inverter02_AC-瓩時"},
{"S029_302_INV_AC_P_AMP_R","彰林ES258kWp_Inverter02_AC-R相電流"},
{"S029_302_INV_AC_P_AMP_S","彰林ES258kWp_Inverter02_AC-S相電流"},
{"S029_302_INV_AC_P_AMP_T","彰林ES258kWp_Inverter02_AC-T相電流"},
{"S029_302_INV_AC_P_HZ_R","彰林ES258kWp_Inverter02_AC-R相頻率"},
{"S029_302_INV_AC_P_HZ_S","彰林ES258kWp_Inverter02_AC-S相頻率"},
{"S029_302_INV_AC_P_HZ_T","彰林ES258kWp_Inverter02_AC-T相頻率"},
{"S029_302_INV_AC_P_KW_R","彰林ES258kWp_Inverter02_AC-R相功率"},
{"S029_302_INV_AC_P_KW_S","彰林ES258kWp_Inverter02_AC-S相功率"},
{"S029_302_INV_AC_P_KW_T","彰林ES258kWp_Inverter02_AC-T相功率"},
{"S029_302_INV_AC_P_VOLT_R","彰林ES258kWp_Inverter02_AC-R相電壓"},
{"S029_302_INV_AC_P_VOLT_S","彰林ES258kWp_Inverter02_AC-S相電壓"},
{"S029_302_INV_AC_P_VOLT_T","彰林ES258kWp_Inverter02_AC-T相電壓"},
{"S029_302_INV_MSG_FAULT_STA_00","彰林ES258kWp_Inverter02_故障點訊息0"},
{"S029_302_INV_MSG_FAULT_STA_01","彰林ES258kWp_Inverter02_故障點訊息1"},
{"S029_302_INV_MSG_FAULT_STA_02","彰林ES258kWp_Inverter02_故障點訊息2"},
{"S029_302_INV_MSG_FAULT_STA_03","彰林ES258kWp_Inverter02_故障點訊息3"},
{"S029_302_INV_MSG_FAULT_STA_04","彰林ES258kWp_Inverter02_故障點訊息4"},
{"S029_302_INV_DC_PV01_AMP","彰林ES258kWp_Inverter02_DC-電流_串列1"},
{"S029_302_INV_DC_PV02_AMP","彰林ES258kWp_Inverter02_DC-電流_串列2"},
{"S029_302_INV_DC_PV01_KW","彰林ES258kWp_Inverter02_DC-實功率_串列1"},
{"S029_302_INV_DC_PV02_KW","彰林ES258kWp_Inverter02_DC-實功率_串列2"},
{"S029_302_INV_DC_PV01_VOLT","彰林ES258kWp_Inverter02_DC-電壓_串列1"},
{"S029_302_INV_DC_PV02_VOLT","彰林ES258kWp_Inverter02_DC-電壓_串列2"},
{"S029_302_INV_MSG_ERROR_STA_00","彰林ES258kWp_Inverter02_錯誤點訊息0"},
{"S029_302_INV_MSG_ERROR_STA_01","彰林ES258kWp_Inverter02_錯誤點訊息1"},
{"S029_302_INV_STA","彰林ES258kWp_Inverter02狀態"},
{"S029_302_INV_MSG_WARNING_STA_00","彰林ES258kWp_Inverter02_警告點訊息0"},
{"S029_302_INV_MSG_WARNING_STA_01","彰林ES258kWp_Inverter02_警告點訊息1"},
{"S029_302_INV_MSG_WARNING_STA_02","彰林ES258kWp_Inverter02_警告點訊息2"},
{"S029_303_INV_AC_TOT_KWH","彰林ES258kWp_Inverter03_AC-瓩時"},
{"S029_303_INV_AC_P_AMP_R","彰林ES258kWp_Inverter03_AC-R相電流"},
{"S029_303_INV_AC_P_AMP_S","彰林ES258kWp_Inverter03_AC-S相電流"},
{"S029_303_INV_AC_P_AMP_T","彰林ES258kWp_Inverter03_AC-T相電流"},
{"S029_303_INV_AC_P_HZ_R","彰林ES258kWp_Inverter03_AC-R相頻率"},
{"S029_303_INV_AC_P_HZ_S","彰林ES258kWp_Inverter03_AC-S相頻率"},
{"S029_303_INV_AC_P_HZ_T","彰林ES258kWp_Inverter03_AC-T相頻率"},
{"S029_303_INV_AC_P_KW_R","彰林ES258kWp_Inverter03_AC-R相功率"},
{"S029_303_INV_AC_P_KW_S","彰林ES258kWp_Inverter03_AC-S相功率"},
{"S029_303_INV_AC_P_KW_T","彰林ES258kWp_Inverter03_AC-T相功率"},
{"S029_303_INV_AC_P_VOLT_R","彰林ES258kWp_Inverter03_AC-R相電壓"},
{"S029_303_INV_AC_P_VOLT_S","彰林ES258kWp_Inverter03_AC-S相電壓"},
{"S029_303_INV_AC_P_VOLT_T","彰林ES258kWp_Inverter03_AC-T相電壓"},
{"S029_303_INV_MSG_FAULT_STA_00","彰林ES258kWp_Inverter03_故障點訊息0"},
{"S029_303_INV_MSG_FAULT_STA_01","彰林ES258kWp_Inverter03_故障點訊息1"},
{"S029_303_INV_MSG_FAULT_STA_02","彰林ES258kWp_Inverter03_故障點訊息2"},
{"S029_303_INV_MSG_FAULT_STA_03","彰林ES258kWp_Inverter03_故障點訊息3"},
{"S029_303_INV_MSG_FAULT_STA_04","彰林ES258kWp_Inverter03_故障點訊息4"},
{"S029_303_INV_DC_PV01_AMP","彰林ES258kWp_Inverter03_DC-電流_串列1"},
{"S029_303_INV_DC_PV02_AMP","彰林ES258kWp_Inverter03_DC-電流_串列2"},
{"S029_303_INV_DC_PV01_KW","彰林ES258kWp_Inverter03_DC-實功率_串列1"},
{"S029_303_INV_DC_PV02_KW","彰林ES258kWp_Inverter03_DC-實功率_串列2"},
{"S029_303_INV_DC_PV01_VOLT","彰林ES258kWp_Inverter03_DC-電壓_串列1"},
{"S029_303_INV_DC_PV02_VOLT","彰林ES258kWp_Inverter03_DC-電壓_串列2"},
{"S029_303_INV_MSG_ERROR_STA_00","彰林ES258kWp_Inverter03_錯誤點訊息0"},
{"S029_303_INV_MSG_ERROR_STA_01","彰林ES258kWp_Inverter03_錯誤點訊息1"},
{"S029_303_INV_STA","彰林ES258kWp_Inverter03狀態"},
{"S029_303_INV_MSG_WARNING_STA_00","彰林ES258kWp_Inverter03_警告點訊息0"},
{"S029_303_INV_MSG_WARNING_STA_01","彰林ES258kWp_Inverter03_警告點訊息1"},
{"S029_303_INV_MSG_WARNING_STA_02","彰林ES258kWp_Inverter03_警告點訊息2"},
{"S029_304_INV_AC_TOT_KWH","彰林ES258kWp_Inverter04_AC-瓩時"},
{"S029_304_INV_AC_P_AMP_R","彰林ES258kWp_Inverter04_AC-R相電流"},
{"S029_304_INV_AC_P_AMP_S","彰林ES258kWp_Inverter04_AC-S相電流"},
{"S029_304_INV_AC_P_AMP_T","彰林ES258kWp_Inverter04_AC-T相電流"},
{"S029_304_INV_AC_P_HZ_R","彰林ES258kWp_Inverter04_AC-R相頻率"},
{"S029_304_INV_AC_P_HZ_S","彰林ES258kWp_Inverter04_AC-S相頻率"},
{"S029_304_INV_AC_P_HZ_T","彰林ES258kWp_Inverter04_AC-T相頻率"},
{"S029_304_INV_AC_P_KW_R","彰林ES258kWp_Inverter04_AC-R相功率"},
{"S029_304_INV_AC_P_KW_S","彰林ES258kWp_Inverter04_AC-S相功率"},
{"S029_304_INV_AC_P_KW_T","彰林ES258kWp_Inverter04_AC-T相功率"},
{"S029_304_INV_AC_P_VOLT_R","彰林ES258kWp_Inverter04_AC-R相電壓"},
{"S029_304_INV_AC_P_VOLT_S","彰林ES258kWp_Inverter04_AC-S相電壓"},
{"S029_304_INV_AC_P_VOLT_T","彰林ES258kWp_Inverter04_AC-T相電壓"},
{"S029_304_INV_MSG_FAULT_STA_00","彰林ES258kWp_Inverter04_故障點訊息0"},
{"S029_304_INV_MSG_FAULT_STA_01","彰林ES258kWp_Inverter04_故障點訊息1"},
{"S029_304_INV_MSG_FAULT_STA_02","彰林ES258kWp_Inverter04_故障點訊息2"},
{"S029_304_INV_MSG_FAULT_STA_03","彰林ES258kWp_Inverter04_故障點訊息3"},
{"S029_304_INV_MSG_FAULT_STA_04","彰林ES258kWp_Inverter04_故障點訊息4"},
{"S029_304_INV_DC_PV01_AMP","彰林ES258kWp_Inverter04_DC-電流_串列1"},
{"S029_304_INV_DC_PV02_AMP","彰林ES258kWp_Inverter04_DC-電流_串列2"},
{"S029_304_INV_DC_PV01_KW","彰林ES258kWp_Inverter04_DC-實功率_串列1"},
{"S029_304_INV_DC_PV02_KW","彰林ES258kWp_Inverter04_DC-實功率_串列2"},
{"S029_304_INV_DC_PV01_VOLT","彰林ES258kWp_Inverter04_DC-電壓_串列1"},
{"S029_304_INV_DC_PV02_VOLT","彰林ES258kWp_Inverter04_DC-電壓_串列2"},
{"S029_304_INV_MSG_ERROR_STA_00","彰林ES258kWp_Inverter04_錯誤點訊息0"},
{"S029_304_INV_MSG_ERROR_STA_01","彰林ES258kWp_Inverter04_錯誤點訊息1"},
{"S029_304_INV_STA","彰林ES258kWp_Inverter04狀態"},
{"S029_304_INV_MSG_WARNING_STA_00","彰林ES258kWp_Inverter04_警告點訊息0"},
{"S029_304_INV_MSG_WARNING_STA_01","彰林ES258kWp_Inverter04_警告點訊息1"},
{"S029_304_INV_MSG_WARNING_STA_02","彰林ES258kWp_Inverter04_警告點訊息2"},
{"S029_305_INV_AC_TOT_KWH","彰林ES258kWp_Inverter05_AC-瓩時"},
{"S029_305_INV_AC_P_AMP_R","彰林ES258kWp_Inverter05_AC-R相電流"},
{"S029_305_INV_AC_P_AMP_S","彰林ES258kWp_Inverter05_AC-S相電流"},
{"S029_305_INV_AC_P_AMP_T","彰林ES258kWp_Inverter05_AC-T相電流"},
{"S029_305_INV_AC_P_HZ_R","彰林ES258kWp_Inverter05_AC-R相頻率"},
{"S029_305_INV_AC_P_HZ_S","彰林ES258kWp_Inverter05_AC-S相頻率"},
{"S029_305_INV_AC_P_HZ_T","彰林ES258kWp_Inverter05_AC-T相頻率"},
{"S029_305_INV_AC_P_KW_R","彰林ES258kWp_Inverter05_AC-R相功率"},
{"S029_305_INV_AC_P_KW_S","彰林ES258kWp_Inverter05_AC-S相功率"},
{"S029_305_INV_AC_P_KW_T","彰林ES258kWp_Inverter05_AC-T相功率"},
{"S029_305_INV_AC_P_VOLT_R","彰林ES258kWp_Inverter05_AC-R相電壓"},
{"S029_305_INV_AC_P_VOLT_S","彰林ES258kWp_Inverter05_AC-S相電壓"},
{"S029_305_INV_AC_P_VOLT_T","彰林ES258kWp_Inverter05_AC-T相電壓"},
{"S029_305_INV_MSG_FAULT_STA_00","彰林ES258kWp_Inverter05_故障點訊息0"},
{"S029_305_INV_MSG_FAULT_STA_01","彰林ES258kWp_Inverter05_故障點訊息1"},
{"S029_305_INV_MSG_FAULT_STA_02","彰林ES258kWp_Inverter05_故障點訊息2"},
{"S029_305_INV_MSG_FAULT_STA_03","彰林ES258kWp_Inverter05_故障點訊息3"},
{"S029_305_INV_MSG_FAULT_STA_04","彰林ES258kWp_Inverter05_故障點訊息4"},
{"S029_305_INV_DC_PV01_AMP","彰林ES258kWp_Inverter05_DC-電流_串列1"},
{"S029_305_INV_DC_PV02_AMP","彰林ES258kWp_Inverter05_DC-電流_串列2"},
{"S029_305_INV_DC_PV01_KW","彰林ES258kWp_Inverter05_DC-實功率_串列1"},
{"S029_305_INV_DC_PV02_KW","彰林ES258kWp_Inverter05_DC-實功率_串列2"},
{"S029_305_INV_DC_PV01_VOLT","彰林ES258kWp_Inverter05_DC-電壓_串列1"},
{"S029_305_INV_DC_PV02_VOLT","彰林ES258kWp_Inverter05_DC-電壓_串列2"},
{"S029_305_INV_MSG_ERROR_STA_00","彰林ES258kWp_Inverter05_錯誤點訊息0"},
{"S029_305_INV_MSG_ERROR_STA_01","彰林ES258kWp_Inverter05_錯誤點訊息1"},
{"S029_305_INV_STA","彰林ES258kWp_Inverter05狀態"},
{"S029_305_INV_MSG_WARNING_STA_00","彰林ES258kWp_Inverter05_警告點訊息0"},
{"S029_305_INV_MSG_WARNING_STA_01","彰林ES258kWp_Inverter05_警告點訊息1"},
{"S029_305_INV_MSG_WARNING_STA_02","彰林ES258kWp_Inverter05_警告點訊息2"},
{"S029_306_INV_AC_TOT_KWH","彰林ES258kWp_Inverter06_AC-瓩時"},
{"S029_306_INV_AC_P_AMP_R","彰林ES258kWp_Inverter06_AC-R相電流"},
{"S029_306_INV_AC_P_AMP_S","彰林ES258kWp_Inverter06_AC-S相電流"},
{"S029_306_INV_AC_P_AMP_T","彰林ES258kWp_Inverter06_AC-T相電流"},
{"S029_306_INV_AC_P_HZ_R","彰林ES258kWp_Inverter06_AC-R相頻率"},
{"S029_306_INV_AC_P_HZ_S","彰林ES258kWp_Inverter06_AC-S相頻率"},
{"S029_306_INV_AC_P_HZ_T","彰林ES258kWp_Inverter06_AC-T相頻率"},
{"S029_306_INV_AC_P_KW_R","彰林ES258kWp_Inverter06_AC-R相功率"},
{"S029_306_INV_AC_P_KW_S","彰林ES258kWp_Inverter06_AC-S相功率"},
{"S029_306_INV_AC_P_KW_T","彰林ES258kWp_Inverter06_AC-T相功率"},
{"S029_306_INV_AC_P_VOLT_R","彰林ES258kWp_Inverter06_AC-R相電壓"},
{"S029_306_INV_AC_P_VOLT_S","彰林ES258kWp_Inverter06_AC-S相電壓"},
{"S029_306_INV_AC_P_VOLT_T","彰林ES258kWp_Inverter06_AC-T相電壓"},
{"S029_306_INV_MSG_FAULT_STA_00","彰林ES258kWp_Inverter06_故障點訊息0"},
{"S029_306_INV_MSG_FAULT_STA_01","彰林ES258kWp_Inverter06_故障點訊息1"},
{"S029_306_INV_MSG_FAULT_STA_02","彰林ES258kWp_Inverter06_故障點訊息2"},
{"S029_306_INV_MSG_FAULT_STA_03","彰林ES258kWp_Inverter06_故障點訊息3"},
{"S029_306_INV_MSG_FAULT_STA_04","彰林ES258kWp_Inverter06_故障點訊息4"},
{"S029_306_INV_DC_PV01_AMP","彰林ES258kWp_Inverter06_DC-電流_串列1"},
{"S029_306_INV_DC_PV02_AMP","彰林ES258kWp_Inverter06_DC-電流_串列2"},
{"S029_306_INV_DC_PV01_KW","彰林ES258kWp_Inverter06_DC-實功率_串列1"},
{"S029_306_INV_DC_PV02_KW","彰林ES258kWp_Inverter06_DC-實功率_串列2"},
{"S029_306_INV_DC_PV01_VOLT","彰林ES258kWp_Inverter06_DC-電壓_串列1"},
{"S029_306_INV_DC_PV02_VOLT","彰林ES258kWp_Inverter06_DC-電壓_串列2"},
{"S029_306_INV_MSG_ERROR_STA_00","彰林ES258kWp_Inverter06_錯誤點訊息0"},
{"S029_306_INV_MSG_ERROR_STA_01","彰林ES258kWp_Inverter06_錯誤點訊息1"},
{"S029_306_INV_STA","彰林ES258kWp_Inverter06狀態"},
{"S029_306_INV_MSG_WARNING_STA_00","彰林ES258kWp_Inverter06_警告點訊息0"},
{"S029_306_INV_MSG_WARNING_STA_01","彰林ES258kWp_Inverter06_警告點訊息1"},
{"S029_306_INV_MSG_WARNING_STA_02","彰林ES258kWp_Inverter06_警告點訊息2"},
{"S029_307_INV_AC_TOT_KWH","彰林ES258kWp_Inverter07_AC-瓩時"},
{"S029_307_INV_AC_P_AMP_R","彰林ES258kWp_Inverter07_AC-R相電流"},
{"S029_307_INV_AC_P_AMP_S","彰林ES258kWp_Inverter07_AC-S相電流"},
{"S029_307_INV_AC_P_AMP_T","彰林ES258kWp_Inverter07_AC-T相電流"},
{"S029_307_INV_AC_P_HZ_R","彰林ES258kWp_Inverter07_AC-R相頻率"},
{"S029_307_INV_AC_P_HZ_S","彰林ES258kWp_Inverter07_AC-S相頻率"},
{"S029_307_INV_AC_P_HZ_T","彰林ES258kWp_Inverter07_AC-T相頻率"},
{"S029_307_INV_AC_P_KW_R","彰林ES258kWp_Inverter07_AC-R相功率"},
{"S029_307_INV_AC_P_KW_S","彰林ES258kWp_Inverter07_AC-S相功率"},
{"S029_307_INV_AC_P_KW_T","彰林ES258kWp_Inverter07_AC-T相功率"},
{"S029_307_INV_AC_P_VOLT_R","彰林ES258kWp_Inverter07_AC-R相電壓"},
{"S029_307_INV_AC_P_VOLT_S","彰林ES258kWp_Inverter07_AC-S相電壓"},
{"S029_307_INV_AC_P_VOLT_T","彰林ES258kWp_Inverter07_AC-T相電壓"},
{"S029_307_INV_MSG_FAULT_STA_00","彰林ES258kWp_Inverter07_故障點訊息0"},
{"S029_307_INV_MSG_FAULT_STA_01","彰林ES258kWp_Inverter07_故障點訊息1"},
{"S029_307_INV_MSG_FAULT_STA_02","彰林ES258kWp_Inverter07_故障點訊息2"},
{"S029_307_INV_MSG_FAULT_STA_03","彰林ES258kWp_Inverter07_故障點訊息3"},
{"S029_307_INV_MSG_FAULT_STA_04","彰林ES258kWp_Inverter07_故障點訊息4"},
{"S029_307_INV_DC_PV01_AMP","彰林ES258kWp_Inverter07_DC-電流_串列1"},
{"S029_307_INV_DC_PV02_AMP","彰林ES258kWp_Inverter07_DC-電流_串列2"},
{"S029_307_INV_DC_PV01_KW","彰林ES258kWp_Inverter07_DC-實功率_串列1"},
{"S029_307_INV_DC_PV02_KW","彰林ES258kWp_Inverter07_DC-實功率_串列2"},
{"S029_307_INV_DC_PV01_VOLT","彰林ES258kWp_Inverter07_DC-電壓_串列1"},
{"S029_307_INV_DC_PV02_VOLT","彰林ES258kWp_Inverter07_DC-電壓_串列2"},
{"S029_307_INV_MSG_ERROR_STA_00","彰林ES258kWp_Inverter07_錯誤點訊息0"},
{"S029_307_INV_MSG_ERROR_STA_01","彰林ES258kWp_Inverter07_錯誤點訊息1"},
{"S029_307_INV_STA","彰林ES258kWp_Inverter07狀態"},
{"S029_307_INV_MSG_WARNING_STA_00","彰林ES258kWp_Inverter07_警告點訊息0"},
{"S029_307_INV_MSG_WARNING_STA_01","彰林ES258kWp_Inverter07_警告點訊息1"},
{"S029_307_INV_MSG_WARNING_STA_02","彰林ES258kWp_Inverter07_警告點訊息2"},
{"S029_308_INV_AC_TOT_KWH","彰林ES258kWp_Inverter08_AC-瓩時"},
{"S029_308_INV_AC_P_AMP_R","彰林ES258kWp_Inverter08_AC-R相電流"},
{"S029_308_INV_AC_P_AMP_S","彰林ES258kWp_Inverter08_AC-S相電流"},
{"S029_308_INV_AC_P_AMP_T","彰林ES258kWp_Inverter08_AC-T相電流"},
{"S029_308_INV_AC_P_HZ_R","彰林ES258kWp_Inverter08_AC-R相頻率"},
{"S029_308_INV_AC_P_HZ_S","彰林ES258kWp_Inverter08_AC-S相頻率"},
{"S029_308_INV_AC_P_HZ_T","彰林ES258kWp_Inverter08_AC-T相頻率"},
{"S029_308_INV_AC_P_KW_R","彰林ES258kWp_Inverter08_AC-R相功率"},
{"S029_308_INV_AC_P_KW_S","彰林ES258kWp_Inverter08_AC-S相功率"},
{"S029_308_INV_AC_P_KW_T","彰林ES258kWp_Inverter08_AC-T相功率"},
{"S029_308_INV_AC_P_VOLT_R","彰林ES258kWp_Inverter08_AC-R相電壓"},
{"S029_308_INV_AC_P_VOLT_S","彰林ES258kWp_Inverter08_AC-S相電壓"},
{"S029_308_INV_AC_P_VOLT_T","彰林ES258kWp_Inverter08_AC-T相電壓"},
{"S029_308_INV_MSG_FAULT_STA_00","彰林ES258kWp_Inverter08_故障點訊息0"},
{"S029_308_INV_MSG_FAULT_STA_01","彰林ES258kWp_Inverter08_故障點訊息1"},
{"S029_308_INV_MSG_FAULT_STA_02","彰林ES258kWp_Inverter08_故障點訊息2"},
{"S029_308_INV_MSG_FAULT_STA_03","彰林ES258kWp_Inverter08_故障點訊息3"},
{"S029_308_INV_MSG_FAULT_STA_04","彰林ES258kWp_Inverter08_故障點訊息4"},
{"S029_308_INV_DC_PV01_AMP","彰林ES258kWp_Inverter08_DC-電流_串列1"},
{"S029_308_INV_DC_PV02_AMP","彰林ES258kWp_Inverter08_DC-電流_串列2"},
{"S029_308_INV_DC_PV01_KW","彰林ES258kWp_Inverter08_DC-實功率_串列1"},
{"S029_308_INV_DC_PV02_KW","彰林ES258kWp_Inverter08_DC-實功率_串列2"},
{"S029_308_INV_DC_PV01_VOLT","彰林ES258kWp_Inverter08_DC-電壓_串列1"},
{"S029_308_INV_DC_PV02_VOLT","彰林ES258kWp_Inverter08_DC-電壓_串列2"},
{"S029_308_INV_MSG_ERROR_STA_00","彰林ES258kWp_Inverter08_錯誤點訊息0"},
{"S029_308_INV_MSG_ERROR_STA_01","彰林ES258kWp_Inverter08_錯誤點訊息1"},
{"S029_308_INV_STA","彰林ES258kWp_Inverter08狀態"},
{"S029_308_INV_MSG_WARNING_STA_00","彰林ES258kWp_Inverter08_警告點訊息0"},
{"S029_308_INV_MSG_WARNING_STA_01","彰林ES258kWp_Inverter08_警告點訊息1"},
{"S029_308_INV_MSG_WARNING_STA_02","彰林ES258kWp_Inverter08_警告點訊息2"},
{"S029_309_INV_AC_TOT_KWH","彰林ES258kWp_Inverter09_AC-瓩時"},
{"S029_309_INV_AC_P_AMP_R","彰林ES258kWp_Inverter09_AC-R相電流"},
{"S029_309_INV_AC_P_AMP_S","彰林ES258kWp_Inverter09_AC-S相電流"},
{"S029_309_INV_AC_P_AMP_T","彰林ES258kWp_Inverter09_AC-T相電流"},
{"S029_309_INV_AC_P_HZ_R","彰林ES258kWp_Inverter09_AC-R相頻率"},
{"S029_309_INV_AC_P_HZ_S","彰林ES258kWp_Inverter09_AC-S相頻率"},
{"S029_309_INV_AC_P_HZ_T","彰林ES258kWp_Inverter09_AC-T相頻率"},
{"S029_309_INV_AC_P_KW_R","彰林ES258kWp_Inverter09_AC-R相功率"},
{"S029_309_INV_AC_P_KW_S","彰林ES258kWp_Inverter09_AC-S相功率"},
{"S029_309_INV_AC_P_KW_T","彰林ES258kWp_Inverter09_AC-T相功率"},
{"S029_309_INV_AC_P_VOLT_R","彰林ES258kWp_Inverter09_AC-R相電壓"},
{"S029_309_INV_AC_P_VOLT_S","彰林ES258kWp_Inverter09_AC-S相電壓"},
{"S029_309_INV_AC_P_VOLT_T","彰林ES258kWp_Inverter09_AC-T相電壓"},
{"S029_309_INV_MSG_FAULT_STA_00","彰林ES258kWp_Inverter09_故障點訊息0"},
{"S029_309_INV_MSG_FAULT_STA_01","彰林ES258kWp_Inverter09_故障點訊息1"},
{"S029_309_INV_MSG_FAULT_STA_02","彰林ES258kWp_Inverter09_故障點訊息2"},
{"S029_309_INV_MSG_FAULT_STA_03","彰林ES258kWp_Inverter09_故障點訊息3"},
{"S029_309_INV_MSG_FAULT_STA_04","彰林ES258kWp_Inverter09_故障點訊息4"},
{"S029_309_INV_DC_PV01_AMP","彰林ES258kWp_Inverter09_DC-電流_串列1"},
{"S029_309_INV_DC_PV02_AMP","彰林ES258kWp_Inverter09_DC-電流_串列2"},
{"S029_309_INV_DC_PV01_KW","彰林ES258kWp_Inverter09_DC-實功率_串列1"},
{"S029_309_INV_DC_PV02_KW","彰林ES258kWp_Inverter09_DC-實功率_串列2"},
{"S029_309_INV_DC_PV01_VOLT","彰林ES258kWp_Inverter09_DC-電壓_串列1"},
{"S029_309_INV_DC_PV02_VOLT","彰林ES258kWp_Inverter09_DC-電壓_串列2"},
{"S029_309_INV_MSG_ERROR_STA_00","彰林ES258kWp_Inverter09_錯誤點訊息0"},
{"S029_309_INV_MSG_ERROR_STA_01","彰林ES258kWp_Inverter09_錯誤點訊息1"},
{"S029_309_INV_STA","彰林ES258kWp_Inverter09狀態"},
{"S029_309_INV_MSG_WARNING_STA_00","彰林ES258kWp_Inverter09_警告點訊息0"},
{"S029_309_INV_MSG_WARNING_STA_01","彰林ES258kWp_Inverter09_警告點訊息1"},
{"S029_309_INV_MSG_WARNING_STA_02","彰林ES258kWp_Inverter09_警告點訊息2"},
{"S029_310_INV_AC_TOT_KWH","彰林ES258kWp_Inverter10_AC-瓩時"},
{"S029_310_INV_AC_P_AMP_R","彰林ES258kWp_Inverter10_AC-R相電流"},
{"S029_310_INV_AC_P_AMP_S","彰林ES258kWp_Inverter10_AC-S相電流"},
{"S029_310_INV_AC_P_AMP_T","彰林ES258kWp_Inverter10_AC-T相電流"},
{"S029_310_INV_AC_P_HZ_R","彰林ES258kWp_Inverter10_AC-R相頻率"},
{"S029_310_INV_AC_P_HZ_S","彰林ES258kWp_Inverter10_AC-S相頻率"},
{"S029_310_INV_AC_P_HZ_T","彰林ES258kWp_Inverter10_AC-T相頻率"},
{"S029_310_INV_AC_P_KW_R","彰林ES258kWp_Inverter10_AC-R相功率"},
{"S029_310_INV_AC_P_KW_S","彰林ES258kWp_Inverter10_AC-S相功率"},
{"S029_310_INV_AC_P_KW_T","彰林ES258kWp_Inverter10_AC-T相功率"},
{"S029_310_INV_AC_P_VOLT_R","彰林ES258kWp_Inverter10_AC-R相電壓"},
{"S029_310_INV_AC_P_VOLT_S","彰林ES258kWp_Inverter10_AC-S相電壓"},
{"S029_310_INV_AC_P_VOLT_T","彰林ES258kWp_Inverter10_AC-T相電壓"},
{"S029_310_INV_MSG_FAULT_STA_00","彰林ES258kWp_Inverter10_故障點訊息0"},
{"S029_310_INV_MSG_FAULT_STA_01","彰林ES258kWp_Inverter10_故障點訊息1"},
{"S029_310_INV_MSG_FAULT_STA_02","彰林ES258kWp_Inverter10_故障點訊息2"},
{"S029_310_INV_MSG_FAULT_STA_03","彰林ES258kWp_Inverter10_故障點訊息3"},
{"S029_310_INV_MSG_FAULT_STA_04","彰林ES258kWp_Inverter10_故障點訊息4"},
{"S029_310_INV_DC_PV01_AMP","彰林ES258kWp_Inverter10_DC-電流_串列1"},
{"S029_310_INV_DC_PV02_AMP","彰林ES258kWp_Inverter10_DC-電流_串列2"},
{"S029_310_INV_DC_PV01_KW","彰林ES258kWp_Inverter10_DC-實功率_串列1"},
{"S029_310_INV_DC_PV02_KW","彰林ES258kWp_Inverter10_DC-實功率_串列2"},
{"S029_310_INV_DC_PV01_VOLT","彰林ES258kWp_Inverter10_DC-電壓_串列1"},
{"S029_310_INV_DC_PV02_VOLT","彰林ES258kWp_Inverter10_DC-電壓_串列2"},
{"S029_310_INV_MSG_ERROR_STA_00","彰林ES258kWp_Inverter10_錯誤點訊息0"},
{"S029_310_INV_MSG_ERROR_STA_01","彰林ES258kWp_Inverter10_錯誤點訊息1"},
{"S029_310_INV_STA","彰林ES258kWp_Inverter10狀態"},
{"S029_310_INV_MSG_WARNING_STA_00","彰林ES258kWp_Inverter10_警告點訊息0"},
{"S029_310_INV_MSG_WARNING_STA_01","彰林ES258kWp_Inverter10_警告點訊息1"},
{"S029_310_INV_MSG_WARNING_STA_02","彰林ES258kWp_Inverter10_警告點訊息2"},
{"S029_401_DI_INV_27_TRIP","彰林ES258kWp_Inverter01_電驛_低電壓跳脫(27)"},
{"S029_401_DI_INV_59_TRIP","彰林ES258kWp_Inverter01_電驛_過電壓跳脫(59)"},
{"S029_401_DI_INV_81H_TRIP","彰林ES258kWp_Inverter01_電驛_過頻跳脫(81H)"},
{"S029_401_DI_INV_81L_TRIP","彰林ES258kWp_Inverter01_電驛_低頻跳脫(81L)"},
{"S029_402_DI_INV_27_TRIP","彰林ES258kWp_Inverter02_電驛跳脫27"},
{"S029_402_DI_INV_59_TRIP","彰林ES258kWp_Inverter02_電驛跳脫59"},
{"S029_402_DI_INV_81H_TRIP","彰林ES258kWp_Inverter02_電驛跳脫81H"},
{"S029_402_DI_INV_81L_TRIP","彰林ES258kWp_Inverter02_電驛跳脫81L"},
{"S029_403_DI_INV_27_TRIP","彰林ES258kWp_Inverter03_電驛跳脫27"},
{"S029_403_DI_INV_59_TRIP","彰林ES258kWp_Inverter03_電驛跳脫59"},
{"S029_403_DI_INV_81H_TRIP","彰林ES258kWp_Inverter03_電驛跳脫81H"},
{"S029_403_DI_INV_81L_TRIP","彰林ES258kWp_Inverter03_電驛跳脫81L"},
{"S029_404_DI_INV_27_TRIP","彰林ES258kWp_Inverter04_電驛跳脫27"},
{"S029_404_DI_INV_59_TRIP","彰林ES258kWp_Inverter04_電驛跳脫59"},
{"S029_404_DI_INV_81H_TRIP","彰林ES258kWp_Inverter04_電驛跳脫81H"},
{"S029_404_DI_INV_81L_TRIP","彰林ES258kWp_Inverter04_電驛跳脫81L"},
{"S029_405_DI_INV_27_TRIP","彰林ES258kWp_Inverter05_電驛跳脫27"},
{"S029_405_DI_INV_59_TRIP","彰林ES258kWp_Inverter05_電驛跳脫59"},
{"S029_405_DI_INV_81H_TRIP","彰林ES258kWp_Inverter05_電驛跳脫81H"},
{"S029_405_DI_INV_81L_TRIP","彰林ES258kWp_Inverter05_電驛跳脫81L"},
{"S029_406_DI_INV_27_TRIP","彰林ES258kWp_Inverter06_電驛跳脫27"},
{"S029_406_DI_INV_59_TRIP","彰林ES258kWp_Inverter06_電驛跳脫59"},
{"S029_406_DI_INV_81H_TRIP","彰林ES258kWp_Inverter06_電驛跳脫81H"},
{"S029_406_DI_INV_81L_TRIP","彰林ES258kWp_Inverter06_電驛跳脫81L"},
{"S029_407_DI_INV_27_TRIP","彰林ES258kWp_Inverter07_電驛跳脫27"},
{"S029_407_DI_INV_59_TRIP","彰林ES258kWp_Inverter07_電驛跳脫59"},
{"S029_407_DI_INV_81H_TRIP","彰林ES258kWp_Inverter07_電驛跳脫81H"},
{"S029_407_DI_INV_81L_TRIP","彰林ES258kWp_Inverter07_電驛跳脫81L"},
{"S029_408_DI_INV_27_TRIP","彰林ES258kWp_Inverter08_電驛跳脫27"},
{"S029_408_DI_INV_59_TRIP","彰林ES258kWp_Inverter08_電驛跳脫59"},
{"S029_408_DI_INV_81H_TRIP","彰林ES258kWp_Inverter08_電驛跳脫81H"},
{"S029_408_DI_INV_81L_TRIP","彰林ES258kWp_Inverter08_電驛跳脫81L"},
{"S029_409_DI_INV_27_TRIP","彰林ES258kWp_Inverter09_電驛跳脫27"},
{"S029_409_DI_INV_59_TRIP","彰林ES258kWp_Inverter09_電驛跳脫59"},
{"S029_409_DI_INV_81H_TRIP","彰林ES258kWp_Inverter09_電驛跳脫81H"},
{"S029_409_DI_INV_81L_TRIP","彰林ES258kWp_Inverter09_電驛跳脫81L"},
{"S029_410_DI_INV_27_TRIP","彰林ES258kWp_Inverter10_電驛跳脫27"},
{"S029_410_DI_INV_59_TRIP","彰林ES258kWp_Inverter10_電驛跳脫59"},
{"S029_410_DI_INV_81H_TRIP","彰林ES258kWp_Inverter10_電驛跳脫81H"},
{"S029_410_DI_INV_81L_TRIP","彰林ES258kWp_Inverter10_電驛跳脫81L"},
{"S029_101_AC_PR","彰林ES258kWp_AC01性能比PR"},
{"S029_100_KWH","彰林ES258kWp_等效滿載發電時數"},
{"S029_301_INV_PR","彰林ES258kWp_INV01性能比"},
{"S029_302_INV_PR","彰林ES258kWp_INV02性能比"},
{"S029_303_INV_PR","彰林ES258kWp_INV03性能比"},
{"S029_304_INV_PR","彰林ES258kWp_INV04性能比"},
{"S029_305_INV_PR","彰林ES258kWp_INV05性能比"},
{"S029_306_INV_PR","彰林ES258kWp_INV06性能比"},
{"S029_307_INV_PR","彰林ES258kWp_INV07性能比"},
{"S029_308_INV_PR","彰林ES258kWp_INV08性能比"},
{"S029_309_INV_PR","彰林ES258kWp_INV09性能比"},
{"S029_310_INV_PR","彰林ES258kWp_INV10性能比"},
{"S029_401_DI_PLC_AI_01","彰林ES258kWp_PLC_AI_01狀態"},
{"S029_401_DI_PLC_AI_02","彰林ES258kWp_PLC_AI_02狀態"},
{"S029_401_DI_PLC_AI_03","彰林ES258kWp_PLC_AI_03狀態"},
{"S029_401_DI_PLC_AI_04","彰林ES258kWp_PLC_AI_04狀態"},
{"S029_401_DI_PLC_AO_01","彰林ES258kWp_PLC_AO_01狀態"},
{"S029_401_DI_PLC_AO_02","彰林ES258kWp_PLC_AO_02狀態"},
{"S029_401_DI_PLC_AO_03","彰林ES258kWp_PLC_AO_03狀態"},
{"S029_401_DI_PLC_AO_04","彰林ES258kWp_PLC_AO_04狀態"},
{"S029_401_DI_PLC_DI_01","彰林ES258kWp_PLC_DI_01狀態"},
{"S029_401_DI_PLC_DI_02","彰林ES258kWp_PLC_DI_02狀態"},
{"S029_401_DI_PLC_DI_03","彰林ES258kWp_PLC_DI_03狀態"},
{"S029_401_DI_PLC_DI_04","彰林ES258kWp_PLC_DI_04狀態"},
{"S029_401_DI_PLC_DI_05","彰林ES258kWp_PLC_DI_05狀態"},
{"S029_401_DI_PLC_DI_06","彰林ES258kWp_PLC_DI_06狀態"},
{"S029_401_DI_PLC_DI_07","彰林ES258kWp_PLC_DI_07狀態"},
{"S029_401_DI_PLC_DI_08","彰林ES258kWp_PLC_DI_08狀態"},
{"S029_401_DI_PLC_DI_09","彰林ES258kWp_PLC_DI_09狀態"},
{"S029_401_DI_PLC_DI_10","彰林ES258kWp_PLC_DI_10狀態"},
{"S029_401_DI_PLC_DI_11","彰林ES258kWp_PLC_DI_11狀態"},
{"S029_401_DI_PLC_DI_12","彰林ES258kWp_PLC_DI_12狀態"},
{"S029_401_DI_PLC_DI_13","彰林ES258kWp_PLC_DI_13狀態"},
{"S029_401_DI_PLC_DI_14","彰林ES258kWp_PLC_DI_14狀態"},
{"S029_401_DI_PLC_DI_15","彰林ES258kWp_PLC_DI_15狀態"},
{"S029_401_DI_PLC_DI_16","彰林ES258kWp_PLC_DI_16狀態"},
{"S029_401_DI_PLC_DO_01","彰林ES258kWp_PLC_DO_01狀態"},
{"S029_401_DI_PLC_DO_02","彰林ES258kWp_PLC_DO_02狀態"},
{"S029_401_DI_PLC_DO_03","彰林ES258kWp_PLC_DO_03狀態"},
{"S029_401_DI_PLC_DO_04","彰林ES258kWp_PLC_DO_04狀態"},
{"S029_401_DI_PLC_DO_05","彰林ES258kWp_PLC_DO_05狀態"},
{"S029_401_DI_PLC_DO_06","彰林ES258kWp_PLC_DO_06狀態"},
{"S029_401_DI_PLC_DO_07","彰林ES258kWp_PLC_DO_07狀態"},
{"S029_401_DI_PLC_DO_08","彰林ES258kWp_PLC_DO_08狀態"},
{"S029_401_DI_PLC_DO_09","彰林ES258kWp_PLC_DO_09狀態"},
{"S029_401_DI_PLC_DO_10","彰林ES258kWp_PLC_DO_10狀態"},
{"S029_401_DI_PLC_DO_11","彰林ES258kWp_PLC_DO_11狀態"},
{"S029_401_DI_PLC_DO_12","彰林ES258kWp_PLC_DO_12狀態"},
{"S029_401_DI_PLC_DO_13","彰林ES258kWp_PLC_DO_13狀態"},
{"S029_401_DI_PLC_DO_14","彰林ES258kWp_PLC_DO_14狀態"},
{"S029_401_DI_PLC_DO_15","彰林ES258kWp_PLC_DO_15狀態"},
{"S029_401_DI_PLC_DO_16","彰林ES258kWp_PLC_DO_16狀態"},
{"S029_000_SOLAR_RAD_F","彰林ES258kWp_全覽頁_日照量"},
{"S029_000_SOLAR_RAD_DAY","彰林ES258kWp_今日累計日照量"},
        };

        //----------------------分隔線-----------------------------

        //----------------------------------------V 3.1 Calculated Data --------------------------------
        public string GetConnectionString()
        {
            return $"Data Source={DeviceName}\\SQLEXPRESS;Initial Catalog={DB_Name};User ID={DB_UserID};Password={DB_Password};";
        }
        //----------------------------------------V 3.0 ---------------- --------------------------------


        private string GenerateSQLWide()
        {

            if (Tables.Length == 1)
            {
                return $"select * from {Tables[0]};";

            }
            else
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append($"select * from {Tables[0]}");
                // Join the remaining tables using the first table's drivertime column
                for (int i = 1; i < Tables.Length; i++)
                {
                    queryBuilder.Append($" JOIN {Tables[i]} ON {Tables[0]}.drivertime = {Tables[i]}.drivertime ");

                }



                queryBuilder.Append(";");
                return queryBuilder.ToString();
            }
        }


        //---------------------------------V 3.0 Histroy Data ------------------------------
        private static string GenerateSQLNarrowAttN(string[] Tables, List<string> dataname)
        {
            if (Tables.Length == 1)
            {
                return $"select drivertime, {string.Join(", ", dataname)} from {Tables[0]} ;";

            }
            else
            {
                // If there are multiple tables, generate a JOIN query
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append($"select {Tables[0]}.drivertime, {string.Join(", ", dataname)} from {Tables[0]} ");
                // Join the remaining tables using the first table's drivertime column
                for (int i = 1; i < Tables.Length; i++)
                {
                    queryBuilder.Append($" JOIN {Tables[i]} ON {Tables[0]}.drivertime = {Tables[i]}.drivertime ");

                }
                //queryBuilder.Append($" ORDER BY drivertime;");

                Console.WriteLine($"query string done:{queryBuilder}  time:{DateTime.Now}");
                //queryBuilder.Append(";");
                return queryBuilder.ToString();
            }
            //if (Tables.Length == 1)
            //{
            //    return $"select * from {Tables[0]} where name in  ( '{string.Join("', '", dataname)}' )  ORDER BY drivertime;";

            //}
            //else
            //{
            //    // If there are multiple tables, generate a JOIN query
            //    StringBuilder queryBuilder = new StringBuilder();
            //    queryBuilder.Append($"select * from {Tables[0]} where name in ( '{string.Join("', '", dataname)}' )  ");
            //    // Join the remaining tables using the first table's drivertime column
            //    for (int i = 1; i < Tables.Length; i++)
            //    {
            //        queryBuilder.Append($" union all select * from {Tables[i]} where name in ( '{string.Join("', '", dataname)}' )");

            //    }
            //    queryBuilder.Append($" ORDER BY drivertime;");

            //    Console.WriteLine($"query string done:{queryBuilder}  time:{DateTime.Now}");
            //    //queryBuilder.Append(";");
            //    return queryBuilder.ToString();
            //}
        }
        public List<AttensionNeededModel> GetAttensionNeededs(string selectedCheckBox)
        {
            List<AttensionNeededModel> attensionNeededModels = new List<AttensionNeededModel>();

            List<string> selectedDevice = null;
            switch (selectedCheckBox)
            {
                case "All":
                    selectedDevice = AllAlarmList;
                    break;
                case "Tr":
                    selectedDevice = TrAlarmList;
                    break;
                case "Relay":
                    selectedDevice = RelayAlarmList;
                    break;
                case "Inv":
                    selectedDevice = InvAlarmList;
                    break;
                case "Other":
                    selectedDevice = OtherAlarmList;
                    break;
            }



            try
            {
                SqlConnection sqlConnection = new SqlConnection(ConnectionString);
                SqlCommand sqlCommand = new SqlCommand($"{GenerateSQLNarrowAttN(Tables, selectedDevice)}  ");
                sqlCommand.Connection = sqlConnection;
                // Increase command timeout
                sqlCommand.CommandTimeout = 6000; // seconds timeout 

                sqlConnection.Open();

                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        AttensionNeededModel attensionNeededModel = new AttensionNeededModel();
                        List<string> alarmNames = new List<string>();
                        List<string> alarmMessages = new List<string>();
                        List<double?> alarmValues = new List<double?>();

                        for (int i = 1; i < reader.FieldCount; i++)
                        {
                            string? columnName = reader.GetName(i);
                            double? columnValue = reader.GetDouble(i);

                            attensionNeededModel.EventTime = reader.GetDateTime(0);
                            alarmNames.Add(columnName);
                            alarmMessages.Add(columnName);
                            alarmValues.Add(columnValue);
                            attensionNeededModel.AlarmName = alarmNames;
                            attensionNeededModel.AlarmMessage = alarmMessages;
                            attensionNeededModel.AlarmValue = alarmValues;
                        }

                        attensionNeededModels.Add(attensionNeededModel);
                    }
                    //foreach (var model in attensionNeededModels)
                    //{
                    //    Console.WriteLine($"EventTime: {model.EventTime}");
                    //    Console.WriteLine($"AlarmName: {model.AlarmName}");
                    //    Console.WriteLine($"AlarmMessage: {model.AlarmMessage}");
                    //    Console.WriteLine($"AlarmValue: {model.AlarmValue}");
                    //    Console.WriteLine();
                    //}

                }
                else
                {
                    Console.WriteLine("資料庫為空！");
                }
                sqlConnection.Close();
                return attensionNeededModels;
            }
            catch (Exception ex)
            {


                return new List<AttensionNeededModel>();
            }

        }
        //--------------------------------V 2.2 -----------------------------------


        //-----------------------------V 3.1 Calculate Data-------------------------------
        public List<IndexModel> GetIndexDatas()
        {
            List<IndexModel> indexModels = new List<IndexModel>();
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            SqlCommand sqlCommand = new SqlCommand(GenerateSQLWide());
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    IndexModel indexModel = new IndexModel();
                    // Define an array of property names and corresponding column names
                    string[] propertyNames = {
                        "PESACTotalKW",
                        "TotalGeneratedPowerRatio",
                        "TotalPerformanceRatio",
                        "TodaysKWH",
                        "GeneratedHRS",
                        "PESACTotalKWH",
                        //"TotalSystemEffciency",
                        "SolarIntensity",
                        "AirTemperature",
                        "Shadetemperature",
                        "ArrayTemperature",
                        //"CO2Reduction",
                        "WindSpeed",
                        "CumulatedSolarRadiation"
                    };
                    string[] columnNames = {
                    totalkW,
                    totalGeneratedPowerRatio,
                    totalPR_Now,
                    dailykWh,
                    maxEfficiencyHrs,
                    totalkWh,
                    //totalSystemEffciency,
                    solarRad,
                    airTemperature,
                    shadetemperature,
                    arrayTemp,
                    //cO2,
                    windSpeed,
                    dailySolRad
                    };
                    for (int i = 0; i < propertyNames.Length; i++)
                    {
                        try
                        {
                            // Check if the database column value is DBNull before attempting to read it
                            if (reader.IsDBNull(reader.GetOrdinal(columnNames[i])))
                            {

                                // Handle the case when the data is null, e.g., set a default value or do something else
                                typeof(IndexModel).GetProperty(propertyNames[i]).SetValue(indexModel, -999);
                            }
                            else
                            {
                                // Read the non-null value from the database
                                double columnValue = reader.GetDouble(reader.GetOrdinal(columnNames[i]));
                                typeof(IndexModel)?.GetProperty(propertyNames[i])?.SetValue(indexModel, columnValue);
                            }
                        }
                        catch (IndexOutOfRangeException ex)
                        {
                            // Handle the exception here, e.g., by logging the error or providing a user-friendly message.
                            // Set a default value (-999) for the specific property
                            typeof(IndexModel).GetProperty(propertyNames[i]).SetValue(indexModel, -999);
                            if (columnNames[i] != "")
                            {
                                // Log the error or display a message to the user
                                Console.WriteLine($"An error occurred while reading data from the database for column ( {columnNames[i]} ): " + ex.Message);
                            }
                            else
                            {
                                Console.WriteLine($"ColumnNames[i] is empty, Index that caused the exception: {i} {propertyNames[i]}");
                            }
                        }
                    }
                    indexModels.Add(indexModel);
                }
            }
            else
            {
                Console.WriteLine("資料庫為空！GetIndexDatas");
            }
            sqlConnection.Close();
            return indexModels;
        }

        //------------------------------------V 3.2 Weather/Temp Page--------------------------------------------
        public readonly List<List<string?>> weatherdataKeys = new List<List<string?>>();
        public readonly List<List<string>> LPDataKeys = new List<List<string>>();
        public readonly List<List<string>> PVBDataKeys = new List<List<string>>();
        public List<WeatherTempModel> GetWeatherTempModels()
        {
            List<WeatherTempModel> weatherTempModels = new List<WeatherTempModel>();
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            SqlCommand sqlCommand = new SqlCommand(GenerateSQLWide());
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    // Create a dictionary to store property names and corresponding values
                    Dictionary<string, double?> weatherTempKeyValues = new Dictionary<string, double?>();

                    // Inside the loop where you retrieve data from the reader
                    // Populate the dictionary with property names and values
                    for (int i = 0; i < weatherTempNames.Length; i++)
                    {
                        double columnValue;
                        if (reader.IsDBNull(reader.GetOrdinal(weatherTempNames[i])))
                        {
                            // Handle null values, e.g., set a default value
                            columnValue = -999.0;
                        }
                        else
                        {
                            // Read non-null values from the database
                            columnValue = reader.GetDouble(reader.GetOrdinal(weatherTempNames[i]));
                        }

                        // Add the property name and its value to the dictionary
                        weatherTempKeyValues.Add(weatherTempNames[i], columnValue);
                    }

                    // Create a new WeatherTempModel object
                    WeatherTempModel weatherTempModel = new WeatherTempModel();

                    weatherTempModel.WeatherTempKeyValues = weatherTempKeyValues;
                    weatherTempModel.WeatherUnitCount = weatherUnitCount;
                    weatherTempModel.WeatherDataKeys = weatherdataKeys;
                    weatherTempModel.LPDataKeys = LPDataKeys;
                    weatherTempModel.PVBoxDataKeys = PVBDataKeys;
                    weatherTempModel.PVBoxUnitCount = PVBoxUnitCount;
                    weatherTempModel.PVBoxCountPerUnit = PVBoxCountPerUnit;
                    // Add the populated WeatherTempModel object to the list
                    weatherTempModels.Add(weatherTempModel);

                }
            }
            else
            {
                Console.WriteLine("資料庫為空！GetWeatherTempModels");
            }
            sqlConnection.Close();
            return weatherTempModels;

        }

        //-----------------------------------V 2.2 ---------------------------------------------
        public List<ConverterInfoModel> GetConverterInfoDatas()
        {
            List<ConverterInfoModel> converterInfoModels = new List<ConverterInfoModel>();
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            SqlCommand sqlCommand = new SqlCommand(GenerateSQLWide());
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();


            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ConverterInfoModel converterInfoModel = new ConverterInfoModel();
                    // Define an array of property names and corresponding column names

                    converterInfoModel.InvCap = InvCap;
                    converterInfoModel.invCount = invCount;
                    converterInfoModel.DB_Name = DB_Name;
                    converterInfoModel.totalkW = totalkW;
                    converterInfoModel.totalkWh = totalkWh;
                    converterInfoModel.totalPR_Now = totalPR_Now;
                    converterInfoModel.totalGeneratedPowerRatio = totalGeneratedPowerRatio;
                    converterInfoModel.totalSystemEffciency = totalSystemEffciency;

                    converterInfoModel.solarRad = solarRad;
                    converterInfoModel.airTemperature = airTemperature;
                    converterInfoModel.shadetemperature = shadetemperature;
                    converterInfoModel.arrayTemp = arrayTemp;
                    converterInfoModel.windSpeed = windSpeed;
                    converterInfoModel.SolRad = solarRad;


                    converterInfoModel.eachInvCap = eachInvCap;

                    converterInfoModel.invACA = invACA;
                    converterInfoModel.invACV = invACV;
                    converterInfoModel.invACkW = invACkW;
                    converterInfoModel.invACkWh = invACkWh;
                    //--------------------------------V 3.0 History data -------------------------
                    converterInfoModel.replacementDictionary = replacementDictionary;
                    //----------------------------------V 2.2 ---------------------------

                    for (int i = 1; i <= invCount; i++)
                    {

                        string propertyName = string.Format(propertyNameFormat, i);
                        string columnName = string.Format(columnNameFormat, i);
                        //string propertyName = $"CIInv{i:D2}Status";


                        try
                        {
                            // Check if the database column value is DBNull before attempting to read it
                            if (reader.IsDBNull(reader.GetOrdinal(columnName)))
                            {
                                // Handle the case when the data is null, e.g., set a default value or do something else
                                typeof(ConverterInfoModel).GetProperty(propertyName)?.SetValue(converterInfoModel, -999);
                            }
                            else
                            {
                                // Read the non-null value from the database
                                double columnValue = reader.GetDouble(reader.GetOrdinal(columnName));
                                typeof(ConverterInfoModel)?.GetProperty(propertyName)?.SetValue(converterInfoModel, columnValue);
                            }
                        }
                        catch (IndexOutOfRangeException ex)
                        {
                            // Handle the exception here, e.g., by logging the error or providing a user-friendly message.
                            // Set a default value (-999) for the specific property
                            typeof(ConverterInfoModel)?.GetProperty(propertyName)?.SetValue(converterInfoModel, -999.0);
                            // Log the error or display a message to the user
                            Console.WriteLine($"An error occurred while reading data from the database for column {columnName}: {ex.Message}");
                        }
                    }



                    for (int i = 0; i < invColumnNames.Count; i++)
                    {
                        try
                        {
                            // Check if the database column value is DBNull before attempting to read it
                            if (reader.IsDBNull(reader.GetOrdinal(invColumnNames[i])))
                            {
                                // Handle the case when the data is null, e.g., set a default value or do something else
                                typeof(ConverterInfoModel).GetProperty(invColumnNames[i]).SetValue(converterInfoModel, -999);
                            }
                            else
                            {
                                // Read the non-null value from the database
                                double columnValue = reader.GetDouble(reader.GetOrdinal(invColumnNames[i]));
                                typeof(ConverterInfoModel)?.GetProperty(invColumnNames[i])?.SetValue(converterInfoModel, columnValue);
                            }
                        }
                        catch (IndexOutOfRangeException ex)
                        {
                            // Handle the exception here, e.g., by logging the error or providing a user-friendly message.
                            // Set a default value (-999) for the specific property
                            typeof(ConverterInfoModel).GetProperty(invColumnNames[i]).SetValue(converterInfoModel, -999.0);
                            // Log the error or display a message to the user
                            Console.WriteLine("An error occurred while reading data from the database for column " + invColumnNames[i] + ": " + ex.Message);
                        }

                    }



                    // Populate new properties
                    converterInfoModel.totalInvDCVolt = string.IsNullOrEmpty(totalInvDCVolt) ? -999.0 : reader.GetDouble(reader.GetOrdinal(totalInvDCVolt));
                    converterInfoModel.totalInvDCAmp = string.IsNullOrEmpty(totalInvDCAmp) ? -999.0 : reader.GetDouble(reader.GetOrdinal(totalInvDCAmp));
                    converterInfoModel.totalInvDCkW = string.IsNullOrEmpty(totalInvDCkW) ? -999.0 : reader.GetDouble(reader.GetOrdinal(totalInvDCkW));
                    converterInfoModel.totalInvDCkWh = string.IsNullOrEmpty(totalInvDCkWh) ? -999.0 : reader.GetDouble(reader.GetOrdinal(totalInvDCkWh));
                    converterInfoModel.totalInvDCRA = string.IsNullOrEmpty(totalInvDCRA) ? -999.0 : reader.GetDouble(reader.GetOrdinal(totalInvDCRA));
                    converterInfoModel.totalInvACVolt = string.IsNullOrEmpty(totalInvACVolt) ? -999.0 : reader.GetDouble(reader.GetOrdinal(totalInvACVolt));
                    converterInfoModel.totalInvACAmp = string.IsNullOrEmpty(totalInvACAmp) ? -999.0 : reader.GetDouble(reader.GetOrdinal(totalInvACAmp));
                    converterInfoModel.totalInvACkW = string.IsNullOrEmpty(totalInvACkW) ? -999.0 : reader.GetDouble(reader.GetOrdinal(totalInvACkW));
                    converterInfoModel.totalInvACkVAR = string.IsNullOrEmpty(totalInvACkVAR) ? -999.0 : reader.GetDouble(reader.GetOrdinal(totalInvACkVAR));
                    converterInfoModel.totalInvACHz = string.IsNullOrEmpty(totalInvACHz) ? -999.0 : reader.GetDouble(reader.GetOrdinal(totalInvACHz));
                    converterInfoModel.totalInvACPR = string.IsNullOrEmpty(totalInvACPR) ? -999.0 : reader.GetDouble(reader.GetOrdinal(totalInvACPR));
                    converterInfoModel.totalInvACDailykWh = string.IsNullOrEmpty(totalInvACDailykWh) ? -999.0 : reader.GetDouble(reader.GetOrdinal(totalInvACDailykWh));
                    converterInfoModel.totalInvACkWh = string.IsNullOrEmpty(totalInvACkWh) ? -999.0 : reader.GetDouble(reader.GetOrdinal(totalInvACkWh));

                    converterInfoModels.Add(converterInfoModel);
                }
            }
            else
            {
                Console.WriteLine("資料庫為空！GetConverterInfoDatas");
            }
            sqlConnection.Close();
            return converterInfoModels;
        }




        public List<PowerEquipmentStatusModel> GetPowerEquipmentModels()
        {
            List<PowerEquipmentStatusModel> powerEquipmentModels = new List<PowerEquipmentStatusModel>();
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            SqlCommand sqlCommand = new SqlCommand($"{GenerateSQLWide()}");
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {

                    PowerEquipmentStatusModel powerEquipmentModel = new PowerEquipmentStatusModel();
                    // Define an array of property names and corresponding column names

                    powerEquipmentModel.ACPMCount = ACPMCount;
                    powerEquipmentModel.DCPMCount = DCPMCount;
                    powerEquipmentModel.ACBCount = ACBCount;
                    powerEquipmentModel.GCBCount = GCBCount;
                    powerEquipmentModel.MCCBCount = MCCBCount;
                    powerEquipmentModel.VCBCount = VCBCount;
                    powerEquipmentModel.TRCount = TRCount;
                    powerEquipmentModel.LPCount = LPCount;
                    powerEquipmentModel.SwitchNames = switchNames;
                    powerEquipmentModel.PowerMeterDataNames = powerMeterDataNames;


                    for (int i = 0; i < pesColumnNames.Length; i++)
                    {

                        try
                        {
                            // Check if the database column value is DBNull before attempting to read it
                            if (reader.IsDBNull(reader.GetOrdinal(pesColumnNames[i])))
                            {

                                // Handle the case when the data is null, e.g., set a default value or do something else
                                typeof(PowerEquipmentStatusModel).GetProperty(pesColumnNames[i]).SetValue(powerEquipmentModel, -999.0);
                            }
                            else
                            {
                                // Read the non-null value from the database
                                double columnValue = reader.GetDouble(reader.GetOrdinal(pesColumnNames[i]));
                                typeof(PowerEquipmentStatusModel)?.GetProperty(pesColumnNames[i])?.SetValue(powerEquipmentModel, columnValue);
                            }
                        }
                        catch (IndexOutOfRangeException ex)
                        {
                            // Handle the exception here, e.g., by logging the error or providing a user-friendly message.
                            // Set a default value (-999) for the specific property
                            typeof(PowerEquipmentStatusModel)?.GetProperty(pesColumnNames[i])?.SetValue(powerEquipmentModel, -999.0);
                            // Log the error or display a message to the user
                            Console.WriteLine("An error occurred while reading data from the database for column " + pesColumnNames[i] + ": " + ex.Message);
                        }







                    }


                    powerEquipmentModels.Add(powerEquipmentModel);
                }
            }
            else
            {
                Console.WriteLine("資料庫為空！GetPowerEquipmentModels");
            }
            sqlConnection.Close();
            return powerEquipmentModels;
        }






    }
}
