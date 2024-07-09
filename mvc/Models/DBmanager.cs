using mvc.Models.ReportQueryModels;
using mvc.Service;
using Microsoft.Extensions.Configuration; 
using System.Data.SqlClient;
using System.Text;


namespace mvc.Models
{
    public class DBmanager
    {
        NowDBmanager nowDBmanager = new NowDBmanager();
        private readonly string DB_Name;
        private readonly string DeviceName;
        private readonly int TableCount;
        private readonly int invCount;

        private readonly List<List<string>> dataKeys;
        private readonly List<string> overview;
        private readonly List<string> powerMeters;
        private readonly List<List<List<string>>> powerMeterDataNames;

        public readonly List<string> AllAlarmList = new List<string>();
        public readonly List<string> TrAlarmList = new List<string>();
        public readonly List<string> RelayAlarmList = new List<string>();
        public readonly List<string> InvAlarmList = new List<string>();
        public readonly List<string> OtherAlarmList = new List<string>();






        //------------------------------------------V 3.0---------------------------------------------------
        private string[] Tables;
        private string ConnectionString;

        public DBmanager()
        {
            DeviceName = nowDBmanager.DeviceName;
            DB_Name = nowDBmanager.DB_Name;
            TableCount = nowDBmanager.TableCount;
            invCount = (int)nowDBmanager.invCount;
            dataKeys = nowDBmanager.GetDataKeys(invCount);
            powerMeterDataNames = nowDBmanager.powerMeterDataNames;
            powerMeters = powerMeterDataNames.SelectMany(innerLists => innerLists.SelectMany(innerList => innerList)).ToList();
            //-------------------------------------V 3.1 Calculate Data----------------------------
            overview = nowDBmanager.overviewDataNames;
       
            //----------------------------------------V 3.0 ------------------------------------

            AllAlarmList       = nowDBmanager.AllAlarmList;
            TrAlarmList        = nowDBmanager.TrAlarmList;
            RelayAlarmList     = nowDBmanager.RelayAlarmList;
            InvAlarmList       = nowDBmanager.InvAlarmList;
            OtherAlarmList     = nowDBmanager.OtherAlarmList;


            if (TableCount > 1)
            {
                Tables = new string[TableCount];
                for (int i = 1; i <= TableCount; i++)
                {
                    Tables[i - 1] = $"{DB_Name}P{i}";
                }
            }
            else
            {
                Tables = new string[TableCount];
                Tables[0] = DB_Name;
            }

            ConnectionString = GetConnectionString();
        }



        private static string GenerateSQLNarrow(string[] Tables, List<string> dataname, DateTime startDate, DateTime endDate)
        {
            string startDateFormat = startDate.ToString("yyyy-MM-dd HH:mm:ss");
            string endDateFormat = endDate.ToString("yyyy-MM-dd HH:mm:ss");

            Console.WriteLine($" '{string.Join("', '", dataname)}' ");

            // Check if the 'values' parameter is null before using it
            if (dataname == null)
            {
                throw new ArgumentNullException(nameof(dataname), "dataname cannot be null.");
            }
            if (Tables.Length == 1)
            {
                return $"select * from {Tables[0]} where name in  ( '{string.Join("', '", dataname)}' )  AND drivertime >= '{startDateFormat}' AND drivertime <= '{endDateFormat}' ORDER BY drivertime;";

            }
            else
            {
                // If there are multiple tables, generate a JOIN query
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append($"select * from {Tables[0]} where name in ( '{string.Join("', '", dataname)}' )  AND drivertime >= '{startDateFormat}' AND drivertime <= '{endDateFormat}'");
                // Join the remaining tables using the first table's drivertime column
                for (int i = 1; i < Tables.Length; i++)
                {
                    queryBuilder.Append($" union all select * from {Tables[i]} where name in ( '{string.Join("', '", dataname)}' )  AND drivertime >= '{startDateFormat}' AND drivertime <= '{endDateFormat}'");

                }
                queryBuilder.Append($" ORDER BY drivertime desc;");

                Console.WriteLine($"query string done:{queryBuilder}  time:{DateTime.Now}");
                //queryBuilder.Append(";");
                return queryBuilder.ToString();
            }

        }
        
        //--------------------------------------V 3.1 Calculate Data-----------------------------------------------------
        private static string GenerateSQLNarrowOverview(string[] Tables, List<string> dataname, DateTime startDate, DateTime endDate)
        {
            string startDateFormat = startDate.ToString("yyyy-MM-dd HH:mm:ss");
            string endDateFormat = endDate.ToString("yyyy-MM-dd HH:mm:ss");

            Console.WriteLine($" '{string.Join("', '", dataname)}' ");

            // Check if the 'values' parameter is null before using it
            if (dataname == null)
            {
                throw new ArgumentNullException(nameof(dataname), "dataname cannot be null.");
            }
            if (Tables.Length == 1)
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append($"select * from {Tables[0]} where name in  ( '{string.Join("', '", dataname)}' )  AND drivertime >= '{startDateFormat}' AND drivertime <= '{endDateFormat}' ");
                queryBuilder.Append($" union all select * from CalDataMin where name in ( '{string.Join("', '", dataname)}' ) AND drivertime >= '{startDateFormat}' AND drivertime <= '{endDateFormat}'");
                queryBuilder.Append($" union all select * from CalDataHr where name in ( '{string.Join("', '", dataname)}' ) AND drivertime >= '{startDateFormat}' AND drivertime <= '{endDateFormat}'");
                queryBuilder.Append($" ORDER BY drivertime desc;");
                return queryBuilder.ToString();

            }
            else
            {
                // If there are multiple tables, generate a JOIN query
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append($"select * from {Tables[0]} where name in ( '{string.Join("', '", dataname)}' )  AND drivertime >= '{startDateFormat}' AND drivertime <= '{endDateFormat}'");
                // Join the remaining tables using the first table's drivertime column
                for (int i = 1; i < Tables.Length; i++)
                {
                    queryBuilder.Append($" union all select * from {Tables[i]} where name in ( '{string.Join("', '", dataname)}' )  AND drivertime >= '{startDateFormat}' AND drivertime <= '{endDateFormat}'");
                    queryBuilder.Append($" union all select * from CalDataMin where name in ( '{string.Join("', '", dataname)}' ) AND drivertime >= '{startDateFormat}' AND drivertime <= '{endDateFormat}'");
                    queryBuilder.Append($" union all select * from CalDataHr where name in ( '{string.Join("', '", dataname)}' ) AND drivertime >= '{startDateFormat}' AND drivertime <= '{endDateFormat}'");
                }
                queryBuilder.Append($" ORDER BY drivertime desc;");

                Console.WriteLine($"query string done:{queryBuilder}  time:{DateTime.Now}");
                //queryBuilder.Append(";");
                return queryBuilder.ToString();
            }

        }
        public string GetConnectionString()
        {
            return $"Data Source={DeviceName}\\SQLEXPRESS;Initial Catalog={DB_Name};User ID={nowDBmanager.DB_UserID};Password={nowDBmanager.DB_Password};;";
        }

        //---------------------------------------V 3.0------------------------------------------------------








        public List<QueryDataModel> GetHistoryDatas(DateTime startDate, DateTime endDate, string selectedCheckboxes)
        {
            List<QueryDataModel> QueryDataModels = new List<QueryDataModel>();
            
            List<string> selectedModel = null;
            if (selectedCheckboxes == "OV") 
            {
                selectedModel = overview;
            }
            else if (selectedCheckboxes == "AC") 
            {
                selectedModel = powerMeters;
            }
            else
            {
                for (int i = 1; i <= invCount; i++)
                {
                    if (selectedCheckboxes == $"Inv{i:D2}")
                    {
                        selectedModel = dataKeys[i-1];
                        break;
                    }
                }
            }

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                //----------------------------------------V 3.1 Calculate Data-------------------------------------------------------
                string sqlQuery;
                if (selectedModel == overview)
                {
                     sqlQuery = $"{GenerateSQLNarrowOverview(Tables, selectedModel, startDate, endDate)}  ";
                }
                else
                {
                     sqlQuery = $"{GenerateSQLNarrow(Tables, selectedModel, startDate, endDate)}  ";
                }
                //--------------------------------------V 3.0-------------------------------------------------------
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
                // Increase command timeout
                sqlCommand.CommandTimeout = 6000; // seconds timeout 
 
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            QueryDataModel queryDataModel = new QueryDataModel
                            {
                                Time = reader.GetDateTime(reader.GetOrdinal("drivertime")),
                                Name = reader.GetString(reader.GetOrdinal("name")),
                                Value = reader.GetString(reader.GetOrdinal("value")),
                            };
                            QueryDataModels.Add(queryDataModel);
                        }
                    }
                    else
                    {
                        Console.WriteLine("資料庫為空！");
                    }
                }
            }
            return QueryDataModels;
        }


        public List<AlarmHistroyModels> GetAlarmHistoryDatas(DateTime startDate, DateTime endDate, string selectedCheckboxes)
        {
            List<AlarmHistroyModels> alarmhistoryModels = new List<AlarmHistroyModels>();
            List<string> selectedModel = null;
            switch (selectedCheckboxes)
            {
                case "All":
                    selectedModel = AllAlarmList; 
                    break;
                case "Tr":
                    selectedModel = TrAlarmList;
                    break;
                case "Relay":
                    selectedModel = RelayAlarmList;
                    break;
                case "Inv":
                    selectedModel = InvAlarmList;
                    break;
                case "Other":
                    selectedModel = OtherAlarmList;
                    break;
            }

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {

                string selectQuery = $"{GenerateSQLNarrow(Tables,selectedModel,startDate,endDate)}  ";
                SqlCommand sqlCommand = new SqlCommand(selectQuery, sqlConnection);

                // Increase command timeout
                sqlCommand.CommandTimeout = 6000; // seconds timeout 

                sqlConnection.Open();

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            AlarmHistroyModels alarmhistoryModel = new AlarmHistroyModels
                            {
                                EventTime = reader.GetDateTime(reader.GetOrdinal("drivertime")),
                                AlarmMessage = reader.GetString(reader.GetOrdinal("name")),
                                AlarmName = reader.GetString(reader.GetOrdinal("name")),
                                AlarmStatus = reader.GetString(reader.GetOrdinal("value"))
                            };
                            alarmhistoryModels.Add(alarmhistoryModel);
                        }
                    }
                    else
                    {
                        Console.WriteLine("資料庫為空！");
                    }
                }
            }

            return alarmhistoryModels;
        }

        public List<ChartModels> GetChartDatas(DateTime startDate, DateTime endDate, List<string> name)
        {
            List<ChartModels> chartModels = new List<ChartModels>();
          
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {




                string selectQuery = $"{GenerateSQLNarrow(Tables, name, startDate, endDate)}  ORDER BY drivertime ASC;";
                SqlCommand sqlCommand = new SqlCommand(selectQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Name", name); // Use the provided name parameter
                sqlCommand.Parameters.AddWithValue("@StartDate", startDate);
                sqlCommand.Parameters.AddWithValue("@EndDate", endDate);
                // Increase command timeout
                sqlCommand.CommandTimeout = 6000; // seconds timeout 
                sqlConnection.Open();

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        int drivertimeOrdinal = reader.GetOrdinal("drivertime");
                        int valueOrdinal = reader.GetOrdinal("value");

                        while (reader.Read())
                        {
                            DateTime dateTime = reader.GetDateTime(drivertimeOrdinal);
                            string formattedDate = $"new Date('{dateTime.ToString("yyyy-MM-ddTHH:mm:ss")}')";
                            ChartModels chartModel = new ChartModels
                            {
                                //date = formattedDate,
                                date = dateTime,
                                close = Double.Parse(reader.GetString(valueOrdinal)),
                            };
                            chartModels.Add(chartModel);
                        }
                    }
                    else
                    {
                        Console.WriteLine("資料庫為空！");
                    }
                }
            }

            return chartModels;
        }

    }
}
