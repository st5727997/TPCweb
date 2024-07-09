using mvc.Models.IndexModels;
using mvc.Service;
using System.Data.SqlClient;
using System.Text;


namespace mvc.Models
{
    public class CalculatedDataDBmanager
    {

        NowDBmanager nowDBmanager = new NowDBmanager();


        private readonly string DeviceName;                         //電腦名稱
        private readonly string DB_Name;                           //站點名稱
        private readonly string dB_TblMin = "CalDataMin";
        private readonly string dB_TblHr = "CalDataHr";
        private string ConnectionString;

        public CalculatedDataDBmanager()
        {
            DeviceName = nowDBmanager.DeviceName;
            DB_Name = nowDBmanager.DB_Name;


            ConnectionString = GetConnectionString();
            //CheckAndCreateDailySolRadTable();
        }

        public string GetConnectionString()
        {

            return $"Data Source={DeviceName}\\SQLEXPRESS;Initial Catalog={DB_Name};User ID={nowDBmanager.DB_UserID};Password={nowDBmanager.DB_Password};";
        }



        private string QueryStringCaledData()
        {
            try
            {

                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("WITH CombinedData AS (");
                queryBuilder.Append($"SELECT top {nowDBmanager.countCaledDataMin} * FROM {dB_TblMin} ");
                queryBuilder.Append("order by drivertime desc");
                // Join the remaining tables using the first table's drivertime column
                queryBuilder.Append($" union all select top {nowDBmanager.countCaledDataHr} * from {dB_TblHr} order by drivertime desc ");
                queryBuilder.Append(") Select * from CombinedData");
                queryBuilder.Append($";");

                //Console.WriteLine(queryBuilder.ToString());

                return queryBuilder.ToString();


            }
            catch
            {
                Console.WriteLine("failed to retreive caleddata from SQL");
                return null;
            }

        }

        public List<CalIndexModel> GetCalIndexDatas()
        {
            List<CalIndexModel> calIndexModels = new List<CalIndexModel>();
            CalIndexModel calIndexModel = new CalIndexModel(); // Instantiate CalIndexModel outside the loop

            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            SqlCommand sqlCommand = new SqlCommand(QueryStringCaledData());
            Console.WriteLine($"{sqlCommand}");
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                calIndexModel.TotalPR_Now = -999;
                calIndexModel.TotalGeneratedPowerRatio = -999;
                calIndexModel.DailySolRad = -999;
                calIndexModel.DailykWh = -999;
                calIndexModel.MaxEfficiencyHrs = -999;
                while (reader.Read())
                {
                    string columnName = reader.GetString(reader.GetOrdinal("name"));
                    string columnValue = reader.GetString(reader.GetOrdinal("value"));
                    //Console.WriteLine($"columnName{columnName}, columnValue{columnValue} ");

                    double parsedValue;
                    if (!string.IsNullOrEmpty(columnValue) && double.TryParse(columnValue, out parsedValue))
                    {

                        if (columnName == nowDBmanager.totalGeneratedPowerRatio)
                        {
                            calIndexModel.TotalGeneratedPowerRatio = parsedValue;
                        }
                        else if (columnName == nowDBmanager.totalPR_Now)
                        {
                            calIndexModel.TotalPR_Now = parsedValue;
                        }
                        else if (columnName == nowDBmanager.dailySolRad)
                        {
                            calIndexModel.DailySolRad = parsedValue;
                        }
                        else if (columnName == nowDBmanager.dailykWh)
                        {
                            calIndexModel.DailykWh = parsedValue;
                        }
                        else if (columnName == nowDBmanager.maxEfficiencyHrs)
                        {
                            calIndexModel.MaxEfficiencyHrs = parsedValue;
                        }
                        else
                        {
                            Console.WriteLine("else error");
                        }
                    }
                }
                calIndexModels.Add(calIndexModel); // Add the single calIndexModel to the list after the loop
            }
            else
            {
                Console.WriteLine("資料庫為空！GetCalIndexDatas");
            }

            sqlConnection.Close();
            return calIndexModels;
        }


    }
}
