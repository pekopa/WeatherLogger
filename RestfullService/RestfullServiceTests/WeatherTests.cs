using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestfullService;

namespace RestfullServiceTests
{

    [TestClass()]
    public class WeatherTests
    {
        
        [TestMethod()]
        public void GetWeatherMeasurementsTest()
        {
            Weather weather = new Weather();
            var list = weather.GetWeatherMeasurements();
            Assert.AreNotEqual(0,list.Count);
        }

        [TestMethod()]
        public void GetWeatherMeasurementsByIdTest()
        {
            Weather weather = new Weather();
            var list = weather.GetWeatherMeasurement("2");
            Assert.AreEqual(1, list.Count);
        }

        [TestMethod()]
        public void GetWeatherMeasurementsBydateTest()
        {
            Weather weather = new Weather();
            string dateFrom = "1511784647";
            string dateTo = "1511784647";
            var list = weather.GetWeatherMeasurementByDate(dateFrom, dateTo);
            Assert.AreNotEqual(0, list.Count);
        }

        [TestMethod()]
        public void PostAndDeleteWeatherMeasurements()
        {
            WeatherMeasurement measurement = new WeatherMeasurement()
            {
                Humidity = 20,
                Pressure = 15,
                Temperature = 11,
                WindSpeed = 5,
                TimeStamp = 1511784647
            };
            Weather weather = new Weather();
            string result = weather.AddWeatherMeasurement(measurement);
            int id = Int32.Parse(result.Split(' ')[1]);
            if (id == 0 && id == null)
            {
                Assert.Fail("Id is 0");
            }
            var rowsAffected = weather.DeleteWeatherMeasurement(id.ToString());
            if (rowsAffected == "Rows affected: 0")
            {
                Assert.Fail("No rows deleted");
            }                      
        }
    }
}