//using Models.ReportQueryModels.InverterModels;
//using System.Data.SqlClient;

//namespace mvc.Models
//{
//    public class CalculatedDataDBmanager
//    {
//        private readonly string ConnectionString = "Data Source=DESKTOP-IN8B58B\\SQLEXPRESS;Initial Catalog=narrow;User ID={nowDBmanager.DB_UserID};Password={nowDBmanager.DB_Password};;";

//        public void CalculateAndInsert()
//        {


//            using (SqlConnection connection = new SqlConnection(ConnectionString))
//            {
//                connection.Open();

//                // Step 2: Retrieve data
//                SqlCommand retrieveRadCommand = new SqlCommand("SELECT value FROM ntest WHERE name = 'S052_001_SOLAR_RAD'", connection);
//                double radValue = Convert.ToDouble(retrieveRadCommand.ExecuteScalar());

//                SqlCommand retrievePreviousBCommand = new SqlCommand("SELECT TOP 1 value FROM Btable WHERE name = 'S052_001_SOLAR_RAD_DAY'", connection);
//                double previousBValue = Convert.ToDouble(retrievePreviousBCommand.ExecuteScalar());

//                // Step 3: Perform calculation
//                double calculatedValue = radValue / 60 + previousBValue;

//                // Step 4: Update Btable
//                SqlCommand updateBCommand = new SqlCommand("INSERT INTO Cal (drivertime, name, value) VALUES ( FORMAT(GETDATE(), 'yyyy-MM-dd HH:mm:') + CASE WHEN DATEPART(SECOND, GETDATE()) < 10 THEN '00' WHEN DATEPART(SECOND, GETDATE()) >= 10 AND DATEPART(SECOND, GETDATE()) < 20 THEN '10' WHEN DATEPART(SECOND, GETDATE()) >= 20 AND DATEPART(SECOND, GETDATE()) < 30 THEN '20' WHEN DATEPART(SECOND, GETDATE()) >= 30 AND DATEPART(SECOND, GETDATE()) < 40 THEN '30' WHEN DATEPART(SECOND, GETDATE()) >= 40 AND DATEPART(SECOND, GETDATE()) < 50 THEN '40' ELSE '50' END, @RadValue, 'S052_001_SOLAR_RAD_DAY' );", connection);
//                updateBCommand.Parameters.AddWithValue("@calculatedValue", calculatedValue);
//                updateBCommand.ExecuteNonQuery();
//            }
//        }




//        public void PostSQLDatas(double radValue)
//        {

//            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
//            {
//                sqlConnection.Open();
//                string sqlQuery = "INSERT INTO Cal (drivertime, name, value) VALUES ( FORMAT(GETDATE(), 'yyyy-MM-dd HH:mm:') + CASE WHEN DATEPART(SECOND, GETDATE()) < 10 THEN '00' WHEN DATEPART(SECOND, GETDATE()) >= 10 AND DATEPART(SECOND, GETDATE()) < 20 THEN '10' WHEN DATEPART(SECOND, GETDATE()) >= 20 AND DATEPART(SECOND, GETDATE()) < 30 THEN '20' WHEN DATEPART(SECOND, GETDATE()) >= 30 AND DATEPART(SECOND, GETDATE()) < 40 THEN '30' WHEN DATEPART(SECOND, GETDATE()) >= 40 AND DATEPART(SECOND, GETDATE()) < 50 THEN '40' ELSE '50' END, @RadValue, 'S052_001_SOLAR_RAD_DAY' );";

//                using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection))
//                {

//                    sqlCommand.Parameters.AddWithValue("@RadValue", radValue);

//                    sqlCommand.ExecuteNonQuery();
//                }

//            }

//        }






//    }
//}
