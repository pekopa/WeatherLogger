using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace RestfullService
{
    public class Weather : IWeather
    {
        private const string ConnectionString = "Server=tcp:boxvalueserver.database.windows.net,1433;Initial Catalog=Weather;Persist Security Info=False;User ID=value;Password=Darmaedas1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public List<WeatherMeasument> GetWeatherMeasurements()

        {
            List<WeatherMeasument> WeatherMeasurementsList = new List<WeatherMeasument>();

            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))

            {

                string command = "SELECT * FROM WeatherMeasurements";

                databaseConnection.Open();

                SqlCommand selectCommand = new SqlCommand(command, databaseConnection);

                var reader = selectCommand.ExecuteReader();

                while (reader.Read())
                {
                    WeatherMeasurementsList.Add(new WeatherMeasument()
                    {
                        Id = reader.GetInt32(0),
                        Temperature = reader.GetDouble(1),
                        Pressure = reader.GetDouble(2),
                        Humidity = reader.GetDouble(3),
                        WindSpeed = reader.GetDouble(4),
                        TimeStamp = reader.GetDateTime(5).ToString(),
                    });
                }
            }
            return WeatherMeasurementsList;
        }

        public List<WeatherMeasument> GetWeatherMeasurement(string id)

        {
            List<WeatherMeasument> WeatherMeasurementsList = new List<WeatherMeasument>();

            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))

            {

                string command = $"SELECT * FROM WeatherMeasurements where id='{id}'";

                databaseConnection.Open();

                SqlCommand selectCommand = new SqlCommand(command, databaseConnection);

                var reader = selectCommand.ExecuteReader();

                while (reader.Read())
                {
                    while (reader.Read())
                    {
                        WeatherMeasurementsList.Add(new WeatherMeasument()
                        {
                            Id = reader.GetInt32(0),
                            Temperature = reader.GetDouble(1),
                            Pressure = reader.GetDouble(2),
                            Humidity = reader.GetDouble(3),
                            WindSpeed = reader.GetDouble(4),
                            TimeStamp = reader.GetDouble(5).ToString(),
                        });
                    }
                }
            }
            return WeatherMeasurementsList;
        }

        public string AddWeatherMeasurement(WeatherMeasument weatherMeasument)
        {
            string insertStudent = $"insert into WeatherMeasurements (Temperature, Pressure, Humidity, WindSpeed, TimeStamp) values ('{weatherMeasument.Temperature}','{weatherMeasument.Pressure}','{weatherMeasument.Humidity}','{weatherMeasument.WindSpeed}','{weatherMeasument.TimeStamp}')" + "Select Scope_Identity()";
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand insertCommand = new SqlCommand(insertStudent, databaseConnection))
                {
                    databaseConnection.Open();
                    int rows = insertCommand.ExecuteNonQuery();
                    return "Rows Affected: " + rows + " ID: " + insertCommand.ExecuteScalar();
                }
            }
        }

        public string DeleteWeatherMeasurement(string id)
        {
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {
                string command = $"DELETE FROM WeatherMeasurements WHERE id = '{id}'";
                databaseConnection.Open();
                SqlCommand selectCommand = new SqlCommand(command, databaseConnection);
                string rows = selectCommand.ExecuteNonQuery().ToString();
                string response = "Rows affected: " + rows;
                return response;
            }
        }
        public string UpdateWeatherMeasurement(WeatherMeasument weatherMeasument, string id)
        {
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {
                string command = $"UPDATE WeatherMeasurements SET Temperature = '{weatherMeasument.Temperature}', Pressure = '{weatherMeasument.Pressure}', Humidity = '{weatherMeasument.Humidity}', WindSpeed = '{weatherMeasument.WindSpeed}', TimeStamp = '{weatherMeasument.TimeStamp}'  WHERE Id = '{id}'";
                databaseConnection.Open();
                SqlCommand selectCommand = new SqlCommand(command, databaseConnection);
                int rows = selectCommand.ExecuteNonQuery();
                string response = "Rows affected: " + rows;
                return response;
            }
        }
    }
}
