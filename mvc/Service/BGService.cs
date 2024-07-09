using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Caching.Memory;
using mvc.Models;
using mvc.Models.IndexModels;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Timers;


namespace mvc.Service
{

    public class BGService : BackgroundService
    {
        private readonly IMemoryCache _cache;


        private readonly NowDBmanager nowDBmanager = new NowDBmanager();

        private readonly string DB_Name;
        private readonly string DeviceName;
        private readonly int TableCount;
        private readonly double InvCap; //kWp
        private readonly string kWName;
        private readonly string SolRadNowName;
        private readonly string dB_TblMin = "CalDataMin";
        private readonly string dB_TblHr = "CalDataHr";
        private readonly string dailykWh;
        private readonly string dailySolRad;
        private readonly string maxEfficiencyHrs;
        private readonly string totalGeneratedPowerRatio;
        private readonly string totalPR_Now;
        private readonly string totalKWHMidnightCacheKey = "totalKWHMidnightCacheKey";

        private readonly List<string> dataNamesToMinTbl;
        private readonly List<string> dataNamesToDailyTbl;
        private readonly List<string> dataNamesFromNowDB;
        private readonly List<string> dataNamesFromHistoryDB;

        private string ConnectionString;
        private string[] TablesNow;
        private string[] TablesHistory;
        private System.Timers.Timer? hourlyTimer;

        private List<string> BGDataNames;
        //private List<string> BGDataNamesInNewSQL;



        public BGService(IConfiguration configuration, IMemoryCache memoryCache)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            NowDBmanager nowDBmanager = new NowDBmanager();

            DeviceName = nowDBmanager.DeviceName;
            DB_Name = nowDBmanager.DB_Name;
            TableCount = nowDBmanager.TableCount;
            BGDataNames = nowDBmanager.BGDataNames;
            InvCap = nowDBmanager.InvCap;
            kWName = nowDBmanager.totalkW;
            SolRadNowName = nowDBmanager.solarRad;

            totalGeneratedPowerRatio = nowDBmanager.totalGeneratedPowerRatio;
            totalPR_Now = nowDBmanager.totalPR_Now;
            dailykWh = nowDBmanager.dailykWh;
            maxEfficiencyHrs = nowDBmanager.maxEfficiencyHrs;
            dailySolRad = nowDBmanager.dailySolRad;

            dataNamesFromNowDB = nowDBmanager.dataNamesFromNowDB;
            dataNamesToMinTbl = nowDBmanager.dataNamesToMinTbl;
            dataNamesToDailyTbl = nowDBmanager.dataNamesToDailyTbl;
            dataNamesFromHistoryDB = nowDBmanager.dataNamesFromHistoryDB;

            if (TableCount > 1)
            {
                TablesNow = new string[TableCount];
                TablesHistory = new string[TableCount];
                for (int i = 1; i <= TableCount; i++)
                {
                    TablesNow[i - 1] = $"{DB_Name}P{i}_instant_data";
                    TablesHistory[i - 1] = $"{DB_Name}P{i}";
                }
            }
            else
            {
                TablesNow = new string[TableCount];
                TablesHistory = new string[TableCount];
                TablesNow[0] = DB_Name + "_instant_data";
                TablesHistory[0] = DB_Name;
            }

            InitializeTimer();
            ConnectionString = GetConnectionString();
            CheckAndCreateTable(dB_TblHr);
            CheckAndCreateTable(dB_TblMin);

            _cache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            

        }
        private async Task AddKwhMidnightToCacheAsync()
        {
            Dictionary<string?,double?> dataValuesFromSQL = await GetTotalkWhAtMidnightAsync(dataNamesFromHistoryDB);
            double? totalKWHMidnightFromMainTbl = dataValuesFromSQL[nowDBmanager.totalkWh];
            // Cache the value for 24 hours
            _cache.Set(totalKWHMidnightCacheKey, totalKWHMidnightFromMainTbl, TimeSpan.FromMinutes((23 * 60 + 20)));
        }
        private void InitializeTimer()
        {
            hourlyTimer = new System.Timers.Timer();


            hourlyTimer.Interval = TimeSpan.FromMinutes(1).TotalMilliseconds;

            hourlyTimer.Elapsed += OnTimerElapsed;

        }
        public string GetConnectionString()
        {
            return $"Data Source={DeviceName}\\SQLEXPRESS;Initial Catalog={DB_Name};User ID={nowDBmanager.DB_UserID};Password={nowDBmanager.DB_Password};";
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Start the timer when the service starts
            hourlyTimer.Start();

            Console.WriteLine("BGService activated.");
            // Call AddKwhMidnightToCacheAsync here
            await AddKwhMidnightToCacheAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("BGService is waiting for the next hour.");
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            Console.WriteLine("Service closed.");
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)  //every min
        {
            var currentTime = DateTime.Now;
            Console.WriteLine($"b4 Hourly timer elapsed at {currentTime}");
            CalIndexModel calculatedModel = CalculateNowDataValues(dataNamesFromNowDB);
            List<double?> calculatedDataValues = new List<double?>
            {
                calculatedModel.TotalGeneratedPowerRatio,
                calculatedModel.TotalPR_Now,
            };
            InsertValuesIntoDB(calculatedDataValues, dataNamesToMinTbl, dB_TblMin);
            Console.WriteLine($"Inserted data into mintable {currentTime}");
            Console.WriteLine($"Hourly timer elapsed at {currentTime}");



            if (currentTime.Minute == 0)
            {
                Console.WriteLine($"mintime = 00 {currentTime}, hr{currentTime.Hour}");

                // Check if it's 00:00
                if (currentTime.Hour == 0)
                {
                    // At 00:00, insert 0 into Daily_Data
                    List<double?> ZeroReset = new List<double?>();
                    foreach (string countNames in dataNamesToDailyTbl)
                    {
                        ZeroReset.Add(0);
                    }
                    InsertValuesIntoDB(ZeroReset, dataNamesToDailyTbl, dB_TblHr);
                    Console.WriteLine("It's 00:00. Inserted 0s into dB_TblHr.");

                    AddKwhMidnightToCacheAsync();


                }
                else
                {

                    CalIndexModel calculatedHrModel = CalculateHrDataValues(dataNamesToDailyTbl, dataNamesFromHistoryDB);
                    List<double?> calculatedHrDataValues = new List<double?>
                    {
                        calculatedHrModel.DailykWh,
                        calculatedHrModel.MaxEfficiencyHrs,
                        calculatedHrModel.DailySolRad
                    };
                    InsertValuesIntoDB(calculatedHrDataValues, dataNamesToDailyTbl, dB_TblHr);

                }

            }
            else
            {


            }
        }

        static string IncrementTime(int minutes)
        {
            TimeSpan ts = TimeSpan.Zero;
            if (minutes > 0)
            {
                ts = ts.Add(new TimeSpan(0, minutes, 0));
            }
            else if (minutes < 0)
            {
                ts = ts.Subtract(new TimeSpan(0, Math.Abs(minutes), 0));
            }
            // Adjust if the result is negative
            if (ts.TotalMinutes < 0)
            {
                ts = ts.Add(new TimeSpan(24, 0, 0));
            }

            return ts.ToString("hh\\:mm");
        }

        private void CheckAndCreateTable(string tableName)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Check if the table "Daily_Data" exists
                var checkTableQuery = $"SELECT COUNT(*) FROM information_schema.tables WHERE table_name = '{tableName}'";

                using (var command = new SqlCommand(checkTableQuery, connection))
                {
                    int tableExists = Convert.ToInt32(command.ExecuteScalar());

                    if (tableExists == 0)
                    {
                        // The table does not exist, so create it
                        var createTableQuery = $"CREATE TABLE {tableName} (" +
                            "drivertime DATETIME, " +
                            "name NVARCHAR(50), " +
                            "value NVARCHAR(50))";

                        using (var createTableCommand = new SqlCommand(createTableQuery, connection))
                        {
                            createTableCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        private string QueryStringNowData(List<string> dataName)
        {

            try
            {

                if (TablesNow.Length == 1)
                {
                    return $"SELECT {string.Join(", ", dataName)}  FROM {TablesNow[0]} ;";

                }
                else
                {
                    StringBuilder queryBuilder = new StringBuilder();
                    queryBuilder.Append($"SELECT {string.Join(", ", dataName)} FROM {TablesNow[0]}");
                    // Join the remaining tables using the first table's drivertime column
                    for (int i = 1; i < TablesNow.Length; i++)
                    {
                        queryBuilder.Append($" JOIN {TablesNow[i]} ON {TablesNow[0]}.drivertime = {TablesNow[i]}.drivertime ");
                    }



                    queryBuilder.Append($";");
                    return queryBuilder.ToString();
                }
            }
            catch
            {
                Console.WriteLine("failed to retreive powerdata from SQL");
                return null;
            }


        }

        private List<double> GetNowDataValues(List<string> dataNames)
        {
            Console.WriteLine($"getting data :{string.Join("', '", dataNames)}");
            List<double> values = new List<double>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var query = QueryStringNowData(dataNames);
                Console.WriteLine($"GetNowDataValues query :{query}");
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            for (int i = 0; i < dataNames.Count; i++)
                            {

                                if (!reader.IsDBNull(0))
                                {

                                    values.Add(Convert.ToDouble(reader[$"{dataNames[i]}"]));
                                    Console.WriteLine($"added {dataNames[i]}  into data");
                                }
                                else
                                {
                                    // Handle DBNull case if needed
                                    // You may want to add a default value or skip adding the null value
                                    Console.WriteLine("SQL returned no value");
                                    values.Add(-999); // or any default value you choose
                                }
                            }


                        }
                    }
                }

            }
            Console.WriteLine($"GetNowDataValues done");
            return values;
        }

        private CalIndexModel CalculateNowDataValues(List<string> dataNames)
        {
            Console.WriteLine($"starting to calculate {string.Join(", ", dataNames)}");
            List<double> dataValuesFromSQL = GetNowDataValues(dataNames);
            if (dataNames != null)
            {
                double totalKW = dataValuesFromSQL[0];
                Console.WriteLine($"totalKW:{totalKW}");
                double solRad = dataValuesFromSQL[1];
                Console.WriteLine($"solRad:{solRad}");
                double PowerEfficiency = totalKW / InvCap * 100;
                Console.WriteLine($"PowerEfficiency:{totalKW} / {InvCap} * 100={PowerEfficiency}");
                double PRValue = 0;
                if (solRad != 0)
                {
                    PRValue = PowerEfficiency / (solRad / 1000); // take solradH not solrad
                }
                Console.WriteLine($"PRValue:{PowerEfficiency}/({solRad}/1000)={PRValue}");


                CalIndexModel calculatedValues = new CalIndexModel
                {
                    TotalGeneratedPowerRatio = PowerEfficiency,
                    TotalPR_Now = PRValue,
                    TotalKW = totalKW,
                    SolRad = solRad
                };

                return calculatedValues;
            }
            else
            {
                Console.WriteLine("CalculateNowDataValues.dataValues = null");
                return null;
            }
        }

        private CalIndexModel CalculateHrDataValues(List<string> dataNames, List<string> kwhAndSolRad)
        {
            Console.WriteLine($"starting to calculate {string.Join(", ", dataNames)}");

            List<double> dataValuesFromSQLNow = GetNowDataValues(kwhAndSolRad);
            List<string> solRad = new List<string>
            {
                dailySolRad
            };
            List<double?> solRadPreviousHr = GetLastSolRadFromHrTable(solRad);
            if (dataNames != null)
            {

                double? kWHNow = dataValuesFromSQLNow[0];
                Console.WriteLine($"kWHNow:{kWHNow}");
                double? solRadNow = dataValuesFromSQLNow[1];
                Console.WriteLine($"solRadNow:{solRadNow}");
                double? totalKWHMidnightFromMainTbl = _cache.Get<double?>(totalKWHMidnightCacheKey);
                Console.WriteLine($"CACHED totalKWHMidnight from maintabl:{totalKWHMidnightFromMainTbl}");

                double? dailySolRad = -999;
                if (solRadPreviousHr[0] != -999)
                {
                    double? solRadPreHr = solRadPreviousHr[0];
                    Console.WriteLine($"solRadPreHr:{solRadPreHr}");
                    dailySolRad = solRadPreHr + solRadNow;
                    Console.WriteLine($"dailySolRad:{dailySolRad}");
                }
                else
                {
                    Console.WriteLine($"failed to get dailySolRad:{dailySolRad}");
                }
                double? dailykWh = kWHNow - totalKWHMidnightFromMainTbl;
                Console.WriteLine($"dailykWh:{dailykWh}");
                double? maxEffHr = dailykWh / InvCap;
                Console.WriteLine($"maxEffHr:{maxEffHr}");
                if (totalKWHMidnightFromMainTbl == null)
                {
                    dailykWh = -999;
                    maxEffHr = -999;
                    Console.WriteLine("Cache is empty");
                }



                CalIndexModel calculatedValues = new CalIndexModel
                {
                    DailySolRad = dailySolRad,
                    DailykWh = dailykWh,
                    MaxEfficiencyHrs = maxEffHr,
                };

                return calculatedValues;
            }
            else
            {
                Console.WriteLine("CalculateNowDataValues.dataValues = null");
                return null;
            }
        }

        private string SQLQueryForTotalkWhAtMidnight(List<string> dataNames, string startTime, string endTime, string tableName)
        {
            if (tableName == "mainTable")
            {
                try
                {


                    if (TablesHistory.Length == 1)
                    {
                        return $"SELECT top {dataNames.Count} value,name FROM {TablesHistory[0]} WHERE name IN ('{string.Join("', '", dataNames)}') AND (drivertime BETWEEN '{startTime}' AND '{endTime}');";

                    }
                    else
                    {
                        StringBuilder queryBuilder = new StringBuilder();
                        queryBuilder.Append($"SELECT top {dataNames.Count} value,name FROM {TablesHistory[0]} WHERE name IN ('{string.Join("', '", dataNames)}') AND (drivertime BETWEEN '{startTime}' AND '{endTime}')");
                        // Join the remaining tables using the first table's drivertime column
                        for (int i = 1; i < TablesHistory.Length; i++)
                        {
                            queryBuilder.Append($"union all");
                            queryBuilder.Append($" SELECT top {dataNames.Count} value,name FROM {TablesHistory[i]}  ");
                            queryBuilder.Append($"WHERE name IN ('{string.Join("', '", dataNames)}') AND (drivertime BETWEEN '{startTime}' AND '{endTime}')");
                        }




                        Console.WriteLine($"SQLQueryForTotalkWhAtMidnight: {queryBuilder}");
                        return queryBuilder.ToString();
                    }

                }
                catch
                {
                    Console.WriteLine("failed to retreive data from SQL");
                    return null;
                }

            }
            else if (tableName == "hrTable")
            {
                try
                {



                    return $"SELECT top {dataNames.Count} value FROM {dB_TblHr} WHERE name IN ('{string.Join("', '", dataNames)}') AND (drivertime BETWEEN '{startTime}' AND '{endTime}');";



                }
                catch
                {
                    Console.WriteLine("failed to retreive data from SQL caught");
                    return null;
                }

            }
            else
            {
                Console.WriteLine("failed to retreive data from SQL else");
                return null;
            }
        }


        private void InsertValuesIntoDB(List<double?> values, List<string> dataNames, string tableName)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                DateTime lastMinute;
                if (values.All(x => x == 0) && dataNames == dataNamesToDailyTbl)
                {
                    lastMinute = DateTime.Now;
                    lastMinute = new DateTime(lastMinute.Year, lastMinute.Month, lastMinute.Day, lastMinute.Hour, lastMinute.Minute, 0);
                }
                else
                {
                    // Calculate the timestamp for the last exact hour
                    lastMinute = DateTime.Now.AddMinutes(-1);
                    lastMinute = new DateTime(lastMinute.Year, lastMinute.Month, lastMinute.Day, lastMinute.Hour, lastMinute.Minute, 0);
                }
                var query = $"INSERT INTO {tableName} (value, drivertime, name) VALUES ";

                for (int i = 0; i < values.Count; i++)
                {
                    query += $"({values[i]}, '{lastMinute:yyyy-MM-dd HH:mm:ss}', '{dataNames[i]}')";
                    if (i < values.Count - 1)
                        query += ",";
                }
                Console.WriteLine($"Insert query: {query}");
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private Dictionary<string?, double?> GetTotalkWhAtMidnight(List<string> dataNames)
        {
            Console.WriteLine($"execute GetTotalkWhAtMidnight");
            int attemptsPerHour = 59;
            int attemptsCount = 0;

            Dictionary<string?, double?> resultDictionary = new Dictionary<string?, double?>();

            DateTime currentDate = DateTime.Today;

            while (attemptsCount <= attemptsPerHour)
            {
                string startTime = currentDate.ToString("yyyy-MM-dd") + " " + IncrementTime(-attemptsCount);
                string endTime = currentDate.ToString("yyyy-MM-dd") + " " + IncrementTime(attemptsCount);

                if (attemptsCount > 0)
                {
                    startTime = currentDate.AddDays(-1).ToString("yyyy-MM-dd") + " " + IncrementTime(-attemptsCount);
                }

                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open(); 

                    var query = SQLQueryForTotalkWhAtMidnight(dataNames, startTime, endTime, "mainTable");

                    using (var command = new SqlCommand(query, connection))
                    {
                        // Set the command timeout (in seconds)
                        command.CommandTimeout = 6000;

                      
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var name = reader["name"].ToString();
                                var value = Convert.ToDouble(reader["value"]);

                                resultDictionary.Add(name, value);
                            }

                            if (resultDictionary.Count == 0)
                            {
                                Console.WriteLine($"GetTotalkWhAtMidnight no result within {attemptsCount} min");
                                attemptsCount++;
                                continue; // Continue to the next iteration
                            }
                            else
                            {
                                break; // Exit the while loop since we found the result
                            }
                        }
                    }
                }
            }

            if (resultDictionary.Count == 0)
            {
                for (int i = 0; i < dataNames.Count; i++)
                {
                    resultDictionary.Add(dataNames[i], -999);
                }
                Console.WriteLine($"Missing TotalkWh data at 00:00 so added -999s");
            }

            return resultDictionary;
        }

        private async Task<Dictionary<string?, double?>> GetTotalkWhAtMidnightAsync(List<string> dataNames)
        {
            Console.WriteLine($"execute GetTotalkWhAtMidnight");
            int attemptsPerHour = 59;
            int attemptsCount = 0;

            Dictionary<string?, double?> resultDictionary = new Dictionary<string?, double?>();

            DateTime currentDate = DateTime.Today;

            while (attemptsCount <= attemptsPerHour)
            {
                string startTime = currentDate.ToString("yyyy-MM-dd") + " " + IncrementTime(-attemptsCount);
                string endTime = currentDate.ToString("yyyy-MM-dd") + " " + IncrementTime(attemptsCount);

                if (attemptsCount > 0)
                {
                    startTime = currentDate.AddDays(-1).ToString("yyyy-MM-dd") + " " + IncrementTime(-attemptsCount);
                }

                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync(); // Open the connection asynchronously

                    var query = SQLQueryForTotalkWhAtMidnight(dataNames, startTime, endTime, "mainTable");

                    using (var command = new SqlCommand(query, connection))
                    {
                        // Set the command timeout (in seconds)
                        command.CommandTimeout = 6000;

                        // Execute the command asynchronously
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var name = reader["name"].ToString();
                                var value = Convert.ToDouble(reader["value"]);

                                resultDictionary.Add(name, value);
                            }

                            if (resultDictionary.Count == 0)
                            {
                                Console.WriteLine($"GetTotalkWhAtMidnight no result within {attemptsCount} min");
                                attemptsCount++;
                                continue; // Continue to the next iteration
                            }
                            else
                            {
                                break; // Exit the while loop since we found the result
                            }
                        }
                    }
                }
            }

            if (resultDictionary.Count == 0)
            {
                for (int i = 0; i < dataNames.Count; i++)
                {
                    resultDictionary.Add(dataNames[i], -999);
                }
                Console.WriteLine($"Missing TotalkWh data at 00:00 so added -999s");
            }

            return resultDictionary;
        }
        private List<double?> GetLastSolRadFromHrTable(List<string> dataNames)//-----------POTENTIAL　ISSUE if no data found
        {
            Console.WriteLine($"execute GetSolRadFromHrTable");
            int attemptsPerHour = 1;
            int attemptsCount = 0;
            //string baseTime = "00:00";
            List<double?> resultDictionary = new List<double?>();

            DateTime currentDate = DateTime.Today;

            while (attemptsCount <= attemptsPerHour)
            {
                // Decrement the start time by attemptsCount minutes from the current day's 00:00
                string startTime = currentDate.ToString("yyyy-MM-dd ");
                string endTime = currentDate.ToString("yyyy-MM-dd ");


                //Console.WriteLine($"startTime{startTime} ,   endTime{endTime}");

                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    var query = SQLQueryForTotalkWhAtMidnight(dataNames, startTime, endTime, "hrTable");

                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var value = reader["value"]; // Potential risk bc its unattached to a name

                                resultDictionary.Add(Convert.ToDouble(value));

                            }

                            if (resultDictionary.Count == 0)
                            {
                                Console.WriteLine($"GetLastSolRadFromHrTable no result within {attemptsCount} min");
                                attemptsCount++;
                                continue; // Continue to the next iteration
                            }
                            else
                            {
                                break; // Exit the while loop since we found the result
                            }

                        }

                    }
                }
            }
            if (resultDictionary.Count == 0)
            {
                for (int i = 0; i < dataNames.Count; i++)
                {
                    resultDictionary.Add(-999);
                }
                Console.WriteLine($"Missing LastSolRadFromHrTable data at {currentDate.ToString("yyyy-MM-dd ")} GetLastSolRadFromHrTable");
            }


            return resultDictionary;

        }





        private void InsertDayValuesIntoDB(double value, string dataName)
        {

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var query = $"INSERT INTO {dB_TblHr} (value, drivertime, name) VALUES (@value, @drivertime, {dataName})";
                Console.WriteLine($"insert command: {query}");
                using (var command = new SqlCommand(query, connection))
                {
                    // Calculate the timestamp for the last exact hour
                    DateTime lastHour = DateTime.Now.AddHours(-1);
                    lastHour = new DateTime(lastHour.Year, lastHour.Month, lastHour.Day, lastHour.Hour, 0, 0);
                    command.Parameters.AddWithValue("@value", value);
                    command.Parameters.AddWithValue("@drivertime", lastHour);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
