using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using AppKickStart.Common.Logging.Wrapper;
using AppKickStart.Common.Providers;
using AppKickStart.Schemas.Tools;
using IKayak.Persistency.Forecasts;
using IKayak.Schemas.Contracts.Forecasts;
using IKayak.Schemas.Models;
using IKayak.Utils;


namespace IKayak.Algorithms.Login
{
    public class WeatherAlgorithm  : IWeatherAlgorithm
    {
        private readonly IAppContext _appContext;
        private static StringDictionary _weathers = new StringDictionary();
        private IForecastQuery _forecastQuery;
 
        public WeatherAlgorithm(IAppContext appContext)
        {
            _appContext = appContext;
            _forecastQuery = _appContext.PersistencyProvider.Get<IForecastQuery>();
        }

        public void RetrieveLastForecasts(string appkey, double latitude, double longitude)
        {
            var url = string.Format(
                @"http://api.worldweatheronline.com/free/v1/marine.ashx?q={0}%2C{1}&format=json&key={2}",
                latitude, longitude, appkey);

            var webReq = (HttpWebRequest)WebRequest.Create(url);
            webReq.UserAgent =
                @"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.64 Safari/537.11";
            webReq.ContentType = @"application/x-www-form-urlencoded";
            webReq.Accept = @"text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            
            webReq.Method = "GET";

            //Get the response handle, we have no true response yet
            var webResp = (HttpWebResponse)webReq.GetResponse();
            Stream webResponse = webResp.GetResponseStream();
            
            if (webResponse != null)
            {
                string response = new StreamReader(webResponse).ReadToEnd();
                Logger.Log(LoggingLevel.Info, @"forecast:\n" + response);
                JSON obj = Newtonsoft.Json.JsonConvert.DeserializeObject<JSON>(response);
                string date;
                var forecasts = new List<Forecast>();
                foreach (Weather weather in obj.data.weather)
                {
                    date = weather.date;
                    foreach (var h in weather.hourly)
                    {
                        var dateTime = DateTime.Parse(date);
                        string dayOfWeek = dateTime.DayOfWeek.ToString();
                        var forecast = new Forecast { Date = TimeTools.ConvertToUnixTimestamp(dateTime), Day = dayOfWeek, Hour = GetTime(h.time), Weather = h.weatherCode, SwellSecs = h.swellPeriod_secs, TempC = h.tempC, WaterTempC = h.waterTemp_C, WaveH = h.sigHeight_m, WindDir = h.winddir16Point, WindF = h.windspeedKmph };
                        forecasts.Add(forecast);
                    }
                }
                _forecastQuery.SaveAll(forecasts);
            }
        }

        private static string GetTime(string input) //"0", "600", "2100"
        {
            
            input = input.PadLeft(4, '0');
            var timeFromInput = DateTime.ParseExact(input, "HHmm", null, DateTimeStyles.None);

            string timeIn12HourFormatForDisplay = timeFromInput.ToString(
                "hh:mm tt",
                CultureInfo.InvariantCulture);
            return timeIn12HourFormatForDisplay;
        }

        private static string GetWeather(string weatherCode)
        {
            if (_weathers.Count == 0)
            {
                //open file
                var lines = File.ReadAllLines(PathMap.Get(@"resources\WeatherCodes.csv"));
                foreach (var line in lines)
                {
                    var strings = line.Split(new []{","}, StringSplitOptions.RemoveEmptyEntries);
                    string code = strings[0];
                    string weather = strings[1];
                    if (!_weathers.ContainsKey(weather))
                        _weathers.Add(code, weather);
                }
            }

            return _weathers[weatherCode];
        }

    }
}