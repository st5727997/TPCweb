//using System.Data.SqlClient;

//namespace Models.IndexModels
//{
//    public class IndexDBmanager
//    {
//        private readonly string ConnectionString = "Data Source=DESKTOP-IN8B58B\\SQLEXPRESS;Initial Catalog=test;Integrated Security=True";

//        public List<IndexModel> GetIndexDatas()
//        {
//            List<IndexModel> indexModels = new List<IndexModel>();
//            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
//            SqlCommand sqlCommand = new SqlCommand("select top 1 * from testtable; ");
//            sqlCommand.Connection = sqlConnection;
//            sqlConnection.Open();

//            SqlDataReader reader = sqlCommand.ExecuteReader();
//            if (reader.HasRows)
//            {
//                while (reader.Read())
//                {
//                    IndexModel indexModel = new IndexModel
//                    {
//                        PESACTotalKW = reader.GetDouble(reader.GetOrdinal("v8")),
//                        PESACTotalKWH = reader.GetDouble(reader.GetOrdinal("v19")),
//                        AirTemperature = reader.GetDouble(reader.GetOrdinal("v6")),
//                        SolarIntensity = reader.GetDouble(reader.GetOrdinal("v5")),
//                        Shadetemperature = reader.GetDouble(reader.GetOrdinal("v4")),
//                        TotalPerformanceRatio = reader.GetDouble(reader.GetOrdinal("v0")),
//                        TotalGeneratedPowerRatio = reader.GetDouble(reader.GetOrdinal("v1")),
//                        TotalSystemEffciency = reader.GetDouble(reader.GetOrdinal("v2")),
//                        ArrayTemperature = reader.GetDouble(reader.GetOrdinal("v3")),
//                        CumulatedSolarRadiation = reader.GetDouble(reader.GetOrdinal("v143")),

//                    };
//                    indexModels.Add(indexModel);
//                }
//            }
//            else
//            {
//                Console.WriteLine("資料庫為空！");
//            }
//            sqlConnection.Close();
//            return indexModels;
//        }
//    }





//}


