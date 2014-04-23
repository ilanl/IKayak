using System.Collections.Generic;
using AppKickStart.Common.Providers.Persistency;
using IKayak.Schemas.Models;

namespace IKayak.Persistency.Forecasts
{
    public interface IForecastQuery : IQuery
    {
        IEnumerable<Forecast> GetNextDays(int numberOfDays, int startHour, int endHour);
        bool SaveAll(IList<Forecast> forecast);
    }
}