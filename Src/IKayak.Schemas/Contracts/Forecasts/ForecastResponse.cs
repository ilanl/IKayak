using System.Collections.Generic;
using AppKickStart.Schemas.Contracts;
using IKayak.Schemas.Models;

namespace IKayak.Schemas.Contracts.Forecasts
{
    /*
     
     { "data": { "nearest_area": [ {"distance_miles": "24.4", "latitude": "45.000", "longitude": "-1.500" } ],  "request": [ {"query": "Lat 45.00 and Lon -2.00", "type": "LatLon" } ],  "weather": [ {"date": "2013-10-16",  "hourly": [ {"cloudcover": "100", "humidity": "94", "precipMM": "0.1", "pressure": "1017", "sigHeight_m": "2.0", "swellDir": "280", "swellHeight_m": "2.0", "swellPeriod_secs": "8.6", "tempC": "19", "tempF": "66", "time": "0", "visibility": "10", "waterTemp_C": "19", "waterTemp_F": "67", "weatherCode": "122",  "weatherIconUrl": [ {"value": "http:\/\/cdn.worldweatheronline.net\/images\/wsymbols01_png_64\/wsymbol_0004_black_low_cloud.png" } ], "winddir16Point": "WSW", "winddirDegree": "236", "windspeedKmph": "20", "windspeedMiles": "12" }, {"cloudcover": "97", "humidity": "96", "precipMM": "1.3", "pressure": "1016", "sigHeight_m": "2.1", "swellDir": "280", "swellHeight_m": "1.9", "swellPeriod_secs": "12.4", "tempC": "19", "tempF": "66", "time": "300", "visibility": "9", "waterTemp_C": "19", "waterTemp_F": "67", "weatherCode": "296",  "weatherIconUrl": [ {"value": "http:\/\/cdn.worldweatheronline.net\/images\/wsymbols01_png_64\/wsymbol_0017_cloudy_with_light_rain.png" } ], "winddir16Point": "SW", "winddirDegree": "232", "windspeedKmph": "28", "windspeedMiles": "17" }, {"cloudcover": "0", "humidity": "95", "precipMM": "0.2", "pressure": "1016", "sigHeight_m": "2.2", "swellDir": "280", "swellHeight_m": "2.0", "swellPeriod_secs": "10.5", "tempC": "20", "tempF": "67", "time": "600", "visibility": "10", "waterTemp_C": "19", "waterTemp_F": "67", "weatherCode": "113",  "weatherIconUrl": [ {"value": "http:\/\/cdn.worldweatheronline.net\/images\/wsymbols01_png_64\/wsymbol_0001_sunny.png" } ], "winddir16Point": "W", "winddirDegree": "264", "windspeedKmph": "32", "windspeedMiles": "20" }, {"cloudcover": "0", "humidity": "91", "precipMM": "0.0", "pressure": "1017", "sigHeight_m": "2.0", "swellDir": "280", "swellHeight_m": "1.9", "swellPeriod_secs": "11.5", "tempC": "20", "tempF": "69", "time": "900", "visibility": "10", "waterTemp_C": "19", "waterTemp_F": "67", "weatherCode": "113",  "weatherIconUrl": [ {"value": "http:\/\/cdn.worldweatheronline.net\/images\/wsymbols01_png_64\/wsymbol_0001_sunny.png" } ], "winddir16Point": "WSW", "winddirDegree": "237", "windspeedKmph": "26", "windspeedMiles": "16" }, {"cloudcover": "100", "humidity": "84", "precipMM": "1.5", "pressure": "1017", "sigHeight_m": "2.1", "swellDir": "280", "swellHeight_m": "1.9", "swellPeriod_secs": "10.9", "tempC": "21", "tempF": "70", "time": "1200", "visibility": "10", "waterTemp_C": "19", "waterTemp_F": "67", "weatherCode": "353",  "weatherIconUrl": [ {"value": "http:\/\/cdn.worldweatheronline.net\/images\/wsymbols01_png_64\/wsymbol_0009_light_rain_showers.png" } ], "winddir16Point": "SW", "winddirDegree": "216", "windspeedKmph": "30", "windspeedMiles": "18" }, {"cloudcover": "28", "humidity": "90", "precipMM": "1.4", "pressure": "1017", "sigHeight_m": "2.3", "swellDir": "280", "swellHeight_m": "2.1", "swellPeriod_secs": "10.9", "tempC": "22", "tempF": "71", "time": "1500", "visibility": "9", "waterTemp_C": "19", "waterTemp_F": "67", "weatherCode": "176",  "weatherIconUrl": [ {"value": "http:\/\/cdn.worldweatheronline.net\/images\/wsymbols01_png_64\/wsymbol_0009_light_rain_showers.png" } ], "winddir16Point": "WSW", "winddirDegree": "254", "windspeedKmph": "36", "windspeedMiles": "22" }, {"cloudcover": "0", "humidity": "88", "precipMM": "0.0", "pressure": "1019", "sigHeight_m": "2.4", "swellDir": "280", "swellHeight_m": "2.3", "swellPeriod_secs": "10.4", "tempC": "20", "tempF": "68", "time": "1800", "visibility": "10", "waterTemp_C": "19", "waterTemp_F": "67", "weatherCode": "113",  "weatherIconUrl": [ {"value": "http:\/\/cdn.worldweatheronline.net\/images\/wsymbols01_png_64\/wsymbol_0001_sunny.png" } ], "winddir16Point": "NW", "winddirDegree": "310", "windspeedKmph": "30", "windspeedMiles": "19" }, {"cloudcover": "0", "humidity": "80", "precipMM": "0.0", "pressure": "1020", "sigHeight_m": "2.3", "swellDir": "280", "swellHeight_m": "2.2", "swellPeriod_secs": "10.5", "tempC": "19", "tempF": "67", "time": "2100", "visibility": "10", "waterTemp_C": "19", "waterTemp_F": "67", "weatherCode": "113",  "weatherIconUrl": [ {"value": "http:\/\/cdn.worldweatheronline.net\/images\/wsymbols01_png_64\/wsymbol_0001_sunny.png" } ], "winddir16Point": "NW", "winddirDegree": "308", "windspeedKmph": "20", "windspeedMiles": "13" } ], "maxtempC": "22", "mintempC": "19" } ] }}
     */

    // ReSharper disable InconsistentNaming

    public class ForecastResponse : BaseResponse
    {
        public IList<Forecast> Forecasts { get; set; }
    }

    public class JSON
    {
        public Data data { get; set; }
    }

    public class Data
    {
        public List<Request> request { get; set; }
        public List<NearestArea> nearest_area { get; set; }
        public List<Weather> weather { get; set; }
    }

    public class Weather
    {
        public string date { get; set; }
        public string maxtempC { get; set; }
        public string mintempC { get; set; }
        public List<Hourly> hourly { get; set; }
    }

    public class Hourly
    {
        public string cloudcover,
                      humidity,
                      precipMM,
                      pressure,
                      sigHeight_m,
                      swellDir,
                      swellHeight_m,
                      swellPeriod_secs,
                      tempC,
                      tempF,
                      time,
                      visibility,
                      waterTemp_C,
                      waterTemp_F,
                      weatherCode,
                      winddir16Point,
                      winddirDegree,
                      windspeedKmph,
                      windspeedMiles;

        public List<WeatherIconUrl> weatherIconUrl;

    }

    public class WeatherIconUrl
    {
        public string value;
    }

    public class NearestArea
    {
        public string distance_miles { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
    }

    public class Request
    {
        public string type { get; set; }
        public string query { get; set; }
    }


    // ReSharper restore InconsistentNaming
}