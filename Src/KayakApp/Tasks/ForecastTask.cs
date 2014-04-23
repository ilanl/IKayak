using System;
using System.Configuration;
using AppKickStart.Common.Logging.Wrapper;
using AppKickStart.Common.Providers;
using AppKickStart.Schemas.ErrorHandling;
using IKayak.Algorithms.Login;
using IKayak.Persistency.Forecasts;

namespace IKayak.Tasks
{
    public class ForecastTask : IForecastTask
    {
        private readonly IAppContext _appContext;

        private readonly IWeatherAlgorithm _algorithm;

        public ForecastTask(IAppContext appContext)
        {
            _appContext = appContext;
            StartAt = null;
            
            int intervalSecs = 3600;
            Int32.TryParse(ConfigurationManager.AppSettings["WeatherIntervalSecs"], out intervalSecs);
            IntervalSeconds = intervalSecs;

            Name = "ForecastTask";

            _appContext.PersistencyProvider.Get<IForecastQuery>();
            _algorithm = _appContext.AlgorithmProvider.Get<IWeatherAlgorithm>();
        }

        #region IKayakBookingTask Members

        public int? StartAt { get; set; }
        public string Name { get; set; }
        public int IntervalSeconds { get; set; }
        public void DoWork()
        {
            try
            {
                _algorithm.RetrieveLastForecasts(ConfigurationManager.AppSettings["weatherApiKey"], 32.103256, 34.77484);
            }
            catch (BusinessException ex)
            {
                Logger.Log(LoggingLevel.Error, "A bug has occured: " + ex.StackTrace, null, ex);
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, "An error occured: "+ ex.StackTrace, null, ex);
            }
        }

        #endregion

    }
}