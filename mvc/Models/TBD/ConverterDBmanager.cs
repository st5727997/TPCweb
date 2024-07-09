//using System.Linq;


//using System.Data.SqlClient;
//using System.Data;
//using System.Reflection;


//namespace mvc.Models
//{
//    public class ConverterDBmanager
//    {

//        private readonly string ConnectionString = "Data Source=DESKTOP-IN8B58B\\SQLEXPRESS;Initial Catalog=test;Integrated Security=True";

//        public List<ConverterModel> GetConverterModels()
//        {
//            List<ConverterModel> converterModels = new List<ConverterModel>();
//            SqlConnection sqlConnection = new SqlConnection(ConnectionString);

//            SqlCommand sqlCommand = new SqlCommand("select*from testtable;");
//            sqlCommand.Connection = sqlConnection;
//            sqlConnection.Open();

//            SqlDataReader reader = sqlCommand.ExecuteReader();
//            if (reader.HasRows)
//            {
//                while (reader.Read())
//                {
//                    ConverterModel converterModel = new ConverterModel
//                    {
//                        C1_Converteractivationstatus = (float)reader.GetDouble(reader.GetOrdinal("v0")),
//                        C2_Converteractivationstatus = (float)reader.GetDouble(reader.GetOrdinal("v1")),
//                        C1_ConversionEfficiency = (float)reader.GetDouble(reader.GetOrdinal("v2")),
//                        C2_ConversionEfficiency = (float)reader.GetDouble(reader.GetOrdinal("v3")),
//                        C1_InverterTemperature = (float)reader.GetDouble(reader.GetOrdinal("v4")),
//                        C2_InverterTemperature = (float)reader.GetDouble(reader.GetOrdinal("v5")),
//                        C1_TotalACPower = (float)reader.GetDouble(reader.GetOrdinal("v6")),
//                        C2_TotalACPower = (float)reader.GetDouble(reader.GetOrdinal("v7")),
//                        C1_TotalDCPower = (float)reader.GetDouble(reader.GetOrdinal("v8")),
//                        C2_TotalDCPower = (float)reader.GetDouble(reader.GetOrdinal("v9")),
//                        C1_Frequency = (float)reader.GetDouble(reader.GetOrdinal("v10")),
//                        C2_Frequency = (float)reader.GetDouble(reader.GetOrdinal("v11")),
//                        C1_GenerationHour = (float)reader.GetDouble(reader.GetOrdinal("v12")),
//                        C2_GenerationHour = (float)reader.GetDouble(reader.GetOrdinal("v13")),
//                        C1_TotalPowerGenerated = (float)reader.GetDouble(reader.GetOrdinal("v14")),
//                        C2_TotalPowerGenerated = (float)reader.GetDouble(reader.GetOrdinal("v15")),
//                        //DateTime = reader.GetDateTime(reader.GetOrdinal("drivertime")),


//                    };
//                    converterModels.Add(converterModel);
//                }
//            }
//            else
//            {
//                Console.WriteLine("資料庫為空！");
//            }
//            sqlConnection.Close();
//            return converterModels;
//        }
//        public List<PowerEquipmentStatusModel> GetPowerEquipmentModels()
//        {
//            List<PowerEquipmentStatusModel> powerEquipmentModels = new List<PowerEquipmentStatusModel>();
//            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
//            SqlCommand sqlCommand = new SqlCommand("select * from ntest ; ");
//            sqlCommand.Connection = sqlConnection;
//            sqlConnection.Open();

//            SqlDataReader reader = sqlCommand.ExecuteReader();
//            if (reader.HasRows)
//            {
//                while (reader.Read())
//                {
//                    PowerEquipmentStatusModel powerEquipmentModel = new PowerEquipmentStatusModel
//                    {
//                        TotalPerformanceRatio = reader.GetDouble(reader.GetOrdinal("v0")),
//                        ArrayTemperature = reader.GetDouble(reader.GetOrdinal("v3")),
//                        SolarIntensity = reader.GetDouble(reader.GetOrdinal("v5")),
//                        PESACTotalKVAR = reader.GetDouble(reader.GetOrdinal("v7")),
//                        PESACTotalKW = reader.GetDouble(reader.GetOrdinal("v8")),
//                        PESACRKW = reader.GetDouble(reader.GetOrdinal("v9")),
//                        PESACSKW = reader.GetDouble(reader.GetOrdinal("v10")),
//                        PESACTKW = reader.GetDouble(reader.GetOrdinal("v11")),
//                        PESACTotalA = reader.GetDouble(reader.GetOrdinal("v12")),
//                        PESACRA = reader.GetDouble(reader.GetOrdinal("v13")),
//                        PESACSA = reader.GetDouble(reader.GetOrdinal("v14")),
//                        PESACTA = reader.GetDouble(reader.GetOrdinal("v15")),
//                        PESACTotalHz = reader.GetDouble(reader.GetOrdinal("v16")),
//                        PESACAVGKVARH = reader.GetDouble(reader.GetOrdinal("v17")),
//                        PESACTotalKVARH = reader.GetDouble(reader.GetOrdinal("v18")),
//                        PESACTotalKWH = reader.GetDouble(reader.GetOrdinal("v19")),
//                        PESACTotalKWHDM = reader.GetDouble(reader.GetOrdinal("v20")),
//                        PESACAVGKWH = reader.GetDouble(reader.GetOrdinal("v21")),
//                        PESACTotalPerformanceFactor = reader.GetDouble(reader.GetOrdinal("v22")),
//                        PESACTotalV = reader.GetDouble(reader.GetOrdinal("v23")),
//                        PESACRV = reader.GetDouble(reader.GetOrdinal("v24")),
//                        PESACSV = reader.GetDouble(reader.GetOrdinal("v25")),
//                        PESACTV = reader.GetDouble(reader.GetOrdinal("v26")),


//                    };
//                    powerEquipmentModels.Add(powerEquipmentModel);
//                }
//            }
//            else
//            {
//                Console.WriteLine("資料庫為空！");
//            }
//            sqlConnection.Close();
//            return powerEquipmentModels;
//        }

//    }
//}

