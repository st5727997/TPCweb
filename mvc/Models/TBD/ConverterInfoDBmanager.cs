//using System.Data.SqlClient;

//namespace mvc.Models
//{
//    public class ConverterInfoDBmanager
//    {
//        private readonly string ConnectionString = "Data Source=DESKTOP-IN8B58B\\SQLEXPRESS;Initial Catalog=test;Integrated Security=True";

//        public List<ConverterInfoModel> GetConverterInfoDatas()
//        {
//            List<ConverterInfoModel> converterInfoModels = new List<ConverterInfoModel>();
//            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
//            SqlCommand sqlCommand = new SqlCommand("select top 1 * from testtable ; ");
//            sqlCommand.Connection = sqlConnection;
//            sqlConnection.Open();

//            SqlDataReader reader = sqlCommand.ExecuteReader();
//            if (reader.HasRows)
//            {
//                while (reader.Read())
//                {
//                    ConverterInfoModel converterInfoModel = new ConverterInfoModel
//                    {
//                        CIInv01Status = reader.GetDouble(reader.GetOrdinal("v49")),
//                        CIInv02Status = reader.GetDouble(reader.GetOrdinal("v72")),
//                        CIInv03Status = reader.GetDouble(reader.GetOrdinal("v95")),
//                        CIInv04Status = reader.GetDouble(reader.GetOrdinal("v118")),
//                        CIInv05Status = reader.GetDouble(reader.GetOrdinal("v141")),



//                        CIInv01CommStatus = reader.GetDouble(reader.GetOrdinal("v35")),
//                        CIInv02CommStatus = reader.GetDouble(reader.GetOrdinal("v58")),
//                        CIInv03CommStatus = reader.GetDouble(reader.GetOrdinal("v81")),
//                        CIInv04CommStatus = reader.GetDouble(reader.GetOrdinal("v104")),
//                        CIInv05CommStatus = reader.GetDouble(reader.GetOrdinal("v127")),







//                        CIInv01ACRA = reader.GetDouble(reader.GetOrdinal("v27")),
//                        CIInv01ACSA = reader.GetDouble(reader.GetOrdinal("v28")),
//                        CIInv01ACTA = reader.GetDouble(reader.GetOrdinal("v29")),
//                        CIInv01ACKW = reader.GetDouble(reader.GetOrdinal("v30")),
//                        CIInv01ACkWh = reader.GetDouble(reader.GetOrdinal("v31")),
//                        CIInv01ACRV = reader.GetDouble(reader.GetOrdinal("v32")),
//                        CIInv01ACSV = reader.GetDouble(reader.GetOrdinal("v33")),
//                        CIInv01ACTV = reader.GetDouble(reader.GetOrdinal("v34")),

//                        CIInv01DCKW = reader.GetDouble(reader.GetOrdinal("v36")),
//                        CIInv01DCPV01KW = reader.GetDouble(reader.GetOrdinal("v37")),
//                        CIInv01DCPV02KW = reader.GetDouble(reader.GetOrdinal("v38")),
//                        CIInv01DCPV03KW = reader.GetDouble(reader.GetOrdinal("v39")),
//                        CIInv01DCPV04KW = reader.GetDouble(reader.GetOrdinal("v40")),
//                        CIInv01DCPV01AMP = reader.GetDouble(reader.GetOrdinal("v41")),
//                        CIInv01DCPV02AMP = reader.GetDouble(reader.GetOrdinal("v42")),
//                        CIInv01DCPV03AMP = reader.GetDouble(reader.GetOrdinal("v43")),
//                        CIInv01DCPV04AMP = reader.GetDouble(reader.GetOrdinal("v44")),
//                        CIInv01DCPV01VOLT = reader.GetDouble(reader.GetOrdinal("v45")),
//                        CIInv01DCPV02VOLT = reader.GetDouble(reader.GetOrdinal("v46")),
//                        CIInv01DCPV03VOLT = reader.GetDouble(reader.GetOrdinal("v47")),
//                        CIInv01DCPV04VOLT = reader.GetDouble(reader.GetOrdinal("v48")),




//                        CIInv02ACRA = reader.GetDouble(reader.GetOrdinal("v50")),
//                        CIInv02ACSA = reader.GetDouble(reader.GetOrdinal("v51")),
//                        CIInv02ACTA = reader.GetDouble(reader.GetOrdinal("v52")),
//                        CIInv02ACKW = reader.GetDouble(reader.GetOrdinal("v53")),
//                        CIInv02ACkWh = reader.GetDouble(reader.GetOrdinal("v54")),
//                        CIInv02ACRV = reader.GetDouble(reader.GetOrdinal("v55")),
//                        CIInv02ACSV = reader.GetDouble(reader.GetOrdinal("v56")),
//                        CIInv02ACTV = reader.GetDouble(reader.GetOrdinal("v57")),

//                        CIInv02DCKW = reader.GetDouble(reader.GetOrdinal("v59")),
//                        CIInv02DCPV01KW = reader.GetDouble(reader.GetOrdinal("v60")),
//                        CIInv02DCPV02KW = reader.GetDouble(reader.GetOrdinal("v61")),
//                        CIInv02DCPV03KW = reader.GetDouble(reader.GetOrdinal("v62")),
//                        CIInv02DCPV04KW = reader.GetDouble(reader.GetOrdinal("v63")),
//                        CIInv02DCPV01AMP = reader.GetDouble(reader.GetOrdinal("v64")),
//                        CIInv02DCPV02AMP = reader.GetDouble(reader.GetOrdinal("v65")),
//                        CIInv02DCPV03AMP = reader.GetDouble(reader.GetOrdinal("v66")),
//                        CIInv02DCPV04AMP = reader.GetDouble(reader.GetOrdinal("v67")),
//                        CIInv02DCPV01VOLT = reader.GetDouble(reader.GetOrdinal("v68")),
//                        CIInv02DCPV02VOLT = reader.GetDouble(reader.GetOrdinal("v69")),
//                        CIInv02DCPV03VOLT = reader.GetDouble(reader.GetOrdinal("v70")),
//                        CIInv02DCPV04VOLT = reader.GetDouble(reader.GetOrdinal("v71")),


//                        CIInv03ACRA = reader.GetDouble(reader.GetOrdinal("v73")),
//                        CIInv03ACSA = reader.GetDouble(reader.GetOrdinal("v74")),
//                        CIInv03ACTA = reader.GetDouble(reader.GetOrdinal("v75")),
//                        CIInv03ACKW = reader.GetDouble(reader.GetOrdinal("v76")),
//                        CIInv03ACkWh = reader.GetDouble(reader.GetOrdinal("v77")),
//                        CIInv03ACRV = reader.GetDouble(reader.GetOrdinal("v78")),
//                        CIInv03ACSV = reader.GetDouble(reader.GetOrdinal("v79")),
//                        CIInv03ACTV = reader.GetDouble(reader.GetOrdinal("v80")),

//                        CIInv03DCKW = reader.GetDouble(reader.GetOrdinal("v82")),
//                        CIInv03DCPV01KW = reader.GetDouble(reader.GetOrdinal("v83")),
//                        CIInv03DCPV02KW = reader.GetDouble(reader.GetOrdinal("v84")),
//                        CIInv03DCPV03KW = reader.GetDouble(reader.GetOrdinal("v85")),
//                        CIInv03DCPV04KW = reader.GetDouble(reader.GetOrdinal("v86")),
//                        CIInv03DCPV01AMP = reader.GetDouble(reader.GetOrdinal("v87")),
//                        CIInv03DCPV02AMP = reader.GetDouble(reader.GetOrdinal("v88")),
//                        CIInv03DCPV03AMP = reader.GetDouble(reader.GetOrdinal("v89")),
//                        CIInv03DCPV04AMP = reader.GetDouble(reader.GetOrdinal("v90")),
//                        CIInv03DCPV01VOLT = reader.GetDouble(reader.GetOrdinal("v91")),
//                        CIInv03DCPV02VOLT = reader.GetDouble(reader.GetOrdinal("v92")),
//                        CIInv03DCPV03VOLT = reader.GetDouble(reader.GetOrdinal("v93")),
//                        CIInv03DCPV04VOLT = reader.GetDouble(reader.GetOrdinal("v94")),





//                        CIInv04ACRA = reader.GetDouble(reader.GetOrdinal("v96")),
//                        CIInv04ACSA = reader.GetDouble(reader.GetOrdinal("v97")),
//                        CIInv04ACTA = reader.GetDouble(reader.GetOrdinal("v98")),
//                        CIInv04ACKW = reader.GetDouble(reader.GetOrdinal("v99")),
//                        CIInv04ACkWh = reader.GetDouble(reader.GetOrdinal("v100")),
//                        CIInv04ACRV = reader.GetDouble(reader.GetOrdinal("v101")),
//                        CIInv04ACSV = reader.GetDouble(reader.GetOrdinal("v102")),
//                        CIInv04ACTV = reader.GetDouble(reader.GetOrdinal("v103")),

//                        CIInv04DCKW = reader.GetDouble(reader.GetOrdinal("v105")),
//                        CIInv04DCPV01KW = reader.GetDouble(reader.GetOrdinal("v106")),
//                        CIInv04DCPV02KW = reader.GetDouble(reader.GetOrdinal("v107")),
//                        CIInv04DCPV03KW = reader.GetDouble(reader.GetOrdinal("v108")),
//                        CIInv04DCPV04KW = reader.GetDouble(reader.GetOrdinal("v109")),
//                        CIInv04DCPV01AMP = reader.GetDouble(reader.GetOrdinal("v110")),
//                        CIInv04DCPV02AMP = reader.GetDouble(reader.GetOrdinal("v111")),
//                        CIInv04DCPV03AMP = reader.GetDouble(reader.GetOrdinal("v112")),
//                        CIInv04DCPV04AMP = reader.GetDouble(reader.GetOrdinal("v113")),
//                        CIInv04DCPV01VOLT = reader.GetDouble(reader.GetOrdinal("v114")),
//                        CIInv04DCPV02VOLT = reader.GetDouble(reader.GetOrdinal("v115")),
//                        CIInv04DCPV03VOLT = reader.GetDouble(reader.GetOrdinal("v116")),
//                        CIInv04DCPV04VOLT = reader.GetDouble(reader.GetOrdinal("v117")),





//                        CIInv05ACRA = reader.GetDouble(reader.GetOrdinal("v119")),
//                        CIInv05ACSA = reader.GetDouble(reader.GetOrdinal("v120")),
//                        CIInv05ACTA = reader.GetDouble(reader.GetOrdinal("v121")),
//                        CIInv05ACKW = reader.GetDouble(reader.GetOrdinal("v122")),
//                        CIInv05ACkWh = reader.GetDouble(reader.GetOrdinal("v123")),
//                        CIInv05ACRV = reader.GetDouble(reader.GetOrdinal("v124")),
//                        CIInv05ACSV = reader.GetDouble(reader.GetOrdinal("v125")),
//                        CIInv05ACTV = reader.GetDouble(reader.GetOrdinal("v126")),

//                        CIInv05DCKW = reader.GetDouble(reader.GetOrdinal("v128")),
//                        CIInv05DCPV01KW = reader.GetDouble(reader.GetOrdinal("v129")),
//                        CIInv05DCPV02KW = reader.GetDouble(reader.GetOrdinal("v130")),
//                        CIInv05DCPV03KW = reader.GetDouble(reader.GetOrdinal("v131")),
//                        CIInv05DCPV04KW = reader.GetDouble(reader.GetOrdinal("v132")),
//                        CIInv05DCPV01AMP = reader.GetDouble(reader.GetOrdinal("v133")),
//                        CIInv05DCPV02AMP = reader.GetDouble(reader.GetOrdinal("v134")),
//                        CIInv05DCPV03AMP = reader.GetDouble(reader.GetOrdinal("v135")),
//                        CIInv05DCPV04AMP = reader.GetDouble(reader.GetOrdinal("v136")),
//                        CIInv05DCPV01VOLT = reader.GetDouble(reader.GetOrdinal("v137")),
//                        CIInv05DCPV02VOLT = reader.GetDouble(reader.GetOrdinal("v138")),
//                        CIInv05DCPV03VOLT = reader.GetDouble(reader.GetOrdinal("v139")),
//                        CIInv05DCPV04VOLT = reader.GetDouble(reader.GetOrdinal("v140")),



//                    };
//                    converterInfoModels.Add(converterInfoModel);
//                }
//            }
//            else
//            {
//                Console.WriteLine("資料庫為空！");
//            }
//            sqlConnection.Close();
//            return converterInfoModels;
//        }
//    }
//}
