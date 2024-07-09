//using System.Linq;


//using System.Data.SqlClient;
//using System.Data;
//using System.Reflection;



//namespace mvc.Models
//{
//    public class ChartDBmanager
//    {
//        private readonly string ConnectionString = "Data Source=DESKTOP-IN8B58B\\SQLEXPRESS;Initial Catalog=S26;Integrated Security=True";


//        public List<ChartModels> GetChartDatas( DateTime datetimefrom, DateTime datetimeto, string name = "S052_000_PR_N")
//        {
//            List<ChartModels> chartModels = new List<ChartModels>();
//            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
//            {
//                if (datetimefrom == default(DateTime))
//                    datetimefrom = new DateTime(2000, 1, 1);

//                if (datetimeto == default(DateTime))
//                    datetimeto = new DateTime(2000, 1, 1);


//                string datetimeFromFormatted = datetimefrom.ToString("yyyy-MM-dd HH:mm:ss");
//                string datetimeToFormatted = datetimeto.ToString("yyyy-MM-dd HH:mm:ss");
//                string selectQuery = $"SELECT * FROM ntest WHERE name IN ([{name}]) AND drivertime BETWEEN @DatetimeFrom AND @DatetimeTo ORDER BY drivertime ASC;";
//                //string selectQuery = $"SELECT drivertime, [{column}] FROM S26 WHERE drivertime BETWEEN @DatetimeFrom AND @DatetimeTo ORDER BY drivertime ASC;";
//                SqlCommand sqlCommand = new SqlCommand(selectQuery, sqlConnection);
//                sqlCommand.Parameters.AddWithValue("@DatetimeFrom", datetimefrom);
//                sqlCommand.Parameters.AddWithValue("@DatetimeTo", datetimeto);
//                sqlConnection.Open();

//                using (SqlDataReader reader = sqlCommand.ExecuteReader())
//                {
//                    if (reader.HasRows)
//                    {
//                        int drivertimeOrdinal = reader.GetOrdinal("drivertime");
//                        int columnOrdinal = reader.GetOrdinal(name);

//                        while (reader.Read())
//                        {
//                            ChartModels chartModel = new ChartModels
//                            {
//                                date = reader.GetDateTime(drivertimeOrdinal),
//                                close = (float)reader.GetDouble(columnOrdinal)
//                            };
//                            chartModels.Add(chartModel);
//                        }
//                    }
//                    else
//                    {
//                        Console.WriteLine("資料庫為空！");
//                    }
//                }
//            }

//            return chartModels;
//        }
//    }





//}

