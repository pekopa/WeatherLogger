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

        public List<WeatherMeasurement> GetWeatherMeasurements()

        {
            List<WeatherMeasurement> weatherMeasumentList = new List<WeatherMeasurement>();

            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))

            {

                string command = "SELECT * FROM WeatherMeasurements";

                databaseConnection.Open();

                SqlCommand selectCommand = new SqlCommand(command, databaseConnection);

                var reader = selectCommand.ExecuteReader();

                while (reader.Read())
                {
                    weatherMeasumentList.Add(new WeatherMeasurement()
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
            return weatherMeasumentList;
        }

        public List<WeatherMeasurement> GetWeatherMeasurement(string id)

        {
            List<WeatherMeasurement> weatherMeasurementList = new List<WeatherMeasurement>();

            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))

            {

                string command = $"SELECT * FROM WeatherMeasurements where Id='{id}'";

                databaseConnection.Open();

                SqlCommand selectCommand = new SqlCommand(command, databaseConnection);

                var reader = selectCommand.ExecuteReader();
               
                    while (reader.Read())
                    {
                        weatherMeasurementList.Add(new WeatherMeasurement()
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
            return weatherMeasurementList;
        }

        public string AddWeatherMeasurement(WeatherMeasurement weatherMeasurement)
        {
            string insertStudent = $"insert into WeatherMeasurements (Temperature, Pressure, Humidity, WindSpeed, TimeStamp) values ('{weatherMeasurement.Temperature}','{weatherMeasurement.Pressure}','{weatherMeasurement.Humidity}','{weatherMeasurement.WindSpeed}','{weatherMeasurement.TimeStamp}')" + "Select Scope_Identity()";
            SqlConnection databaseConnection = new SqlConnection(ConnectionString);
            databaseConnection.Open();
            SqlCommand insertCommand = new SqlCommand(insertStudent, databaseConnection);
            var rows = insertCommand.ExecuteScalar();
            return " ID: " + rows;
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
        public string UpdateWeatherMeasurement(WeatherMeasurement weatherMeasurement, string id)
        {
            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {
                string command = $"UPDATE WeatherMeasurements SET Temperature = '{weatherMeasurement.Temperature}', Pressure = '{weatherMeasurement.Pressure}', Humidity = '{weatherMeasurement.Humidity}', WindSpeed = '{weatherMeasurement.WindSpeed}', TimeStamp = '{weatherMeasurement.TimeStamp}'  WHERE Id = '{id}'";
                databaseConnection.Open();
                SqlCommand selectCommand = new SqlCommand(command, databaseConnection);
                int rows = selectCommand.ExecuteNonQuery();
                string response = "Rows affected: " + rows;
                return response;
            }
        }
    }
}
