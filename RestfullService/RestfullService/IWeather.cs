using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace RestfullService
{
    [ServiceContract]
    public interface IWeather
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "WeatherMeasurements/")]
        List<WeatherMeasument> GetWeatherMeasurements();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "WeatherMeasurements/{id}")]
        List<WeatherMeasument> GetWeatherMeasurement(string id);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "WeatherMeasurements/")]
        string AddWeatherMeasurement(WeatherMeasument weatherMeasument);

        [OperationContract]
        [WebInvoke(Method = "DELETE", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "WeatherMeasurements/{id}")]
        string DeleteWeatherMeasurement(string id);

        [OperationContract]
        [WebInvoke(Method = "PUT", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "WeatherMeasurements/{id}")]
        string UpdateWeatherMeasurement(WeatherMeasument weatherMeasument, string id);
    }

    [DataContract]
    public class WeatherMeasument
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public double Temperature { get; set; }
        [DataMember]
        public double Pressure { get; set; }
        [DataMember]
        public double Humidity { get; set; }
        [DataMember]
        public double WindSpeed { get; set; }
        [DataMember]
        public string TimeStamp { get; set; }
    }
}

