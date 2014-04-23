using System.Collections.Generic;
using System.Linq;
using AppKickStart.Common.Providers;
using AppKickStart.Common.Providers.Services;
using IKayak.Persistency.Forecasts;
using IKayak.Schemas.Contracts.Forecasts;
using IKayak.Schemas.Models;

namespace IKayak.Services.Forecasts
{
    public class ForecastBusinessService :
        BusinessService<ForecastRequest, IEnumerable<Forecast>, ForecastResponse>, IForecastBusinessService
    {

        public ForecastBusinessService()
        {
        }

        public ForecastBusinessService(IAppContext appContext)
            : base(appContext)
        {
        }

        #region IForecastBusinessService Members

        public override string ServiceName
        {
            get { return "forecasts"; }
        }

        public override string Request
        {
            get { return "ForecastRequest"; }
        }

        public override string Response
        {
            get { return "ForecastResponse"; }
        }

        #endregion

        public override IEnumerable<Forecast> Execute(ForecastRequest args)
        {
            var forecastQuery = AppContext.PersistencyProvider.Get<IForecastQuery>();
            return forecastQuery.GetNextDays(7, 600, 2000);
        }

        public override ForecastResponse BuildResponse(IEnumerable<Forecast> results)
        {
            return new ForecastResponse{ Forecasts = results.ToList() };
        }
    }
}