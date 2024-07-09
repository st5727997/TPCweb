//using System.Data.SqlClient;

//namespace mvc.Models
//{
//    public class AlarmHistoryDBmanager
//    {
//        private readonly string ConnectionString = "Data Source=DESKTOP-IN8B58B\\SQLEXPRESS;Initial Catalog=S26;Integrated Security=True";


//        public List<AlarmHistroyModels> GetAlarmHistoryDatas(DateTime datetimefrom, DateTime datetimeto, string alarmstatus = " ")
//        {
//            List<AlarmHistroyModels> alarmhistoryModels = new List<AlarmHistroyModels>();
//            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
//            {
//                if (datetimefrom == default(DateTime))
//                    datetimefrom = new DateTime(2000, 1, 1);

//                if (datetimeto == default(DateTime))
//                    datetimeto = new DateTime(2000, 1, 1);

//                if (alarmstatus == null)
//                    alarmstatus= " ";



//                string datetimeFromFormatted = datetimefrom.ToString("yyyy-MM-dd HH:mm:ss");
//                string datetimeToFormatted = datetimeto.ToString("yyyy-MM-dd HH:mm:ss");
//                string selectQuery = $"SELECT drivertime, [v0], [v1],[v2] FROM S26 WHERE drivertime BETWEEN @DatetimeFrom AND @DatetimeTo {alarmstatus} ORDER BY drivertime ASC;";
//                SqlCommand sqlCommand = new SqlCommand(selectQuery, sqlConnection);
//                sqlCommand.Parameters.AddWithValue("@DatetimeFrom", datetimefrom);
//                sqlCommand.Parameters.AddWithValue("@DatetimeTo", datetimeto);


//                sqlConnection.Open();

//                using (SqlDataReader reader = sqlCommand.ExecuteReader())
//                {
//                    if (reader.HasRows)
//                    {
//                        int drivertimeOrdinal = reader.GetOrdinal("drivertime");
//                        int alarmmessageOrdinal = reader.GetOrdinal("v1");
//                        int alarmnameOrdinal = reader.GetOrdinal("v2");
//                        int alarmstatusOrdinal = reader.GetOrdinal("v0");
//                        while (reader.Read())
//                        {
//                            AlarmHistroyModels alarmhistoryModel = new AlarmHistroyModels
//                            {
//                                EventTime = reader.GetDateTime(drivertimeOrdinal),
//                                AlarmMessage = (float)reader.GetDouble(alarmmessageOrdinal),
//                                AlarmName = (float)reader.GetDouble(alarmnameOrdinal),
//                                AlarmStatus = (float)reader.GetDouble(alarmstatusOrdinal),
//                            };
//                            alarmhistoryModels.Add(alarmhistoryModel);
//                        }
//                    }
//                    else
//                    {
//                        Console.WriteLine("資料庫為空！");
//                    }
//                }
//            }

//            return alarmhistoryModels;
//        }
//    }
//}
