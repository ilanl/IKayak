using System.Collections.Generic;
using AppKickStart.Common.Providers.Persistency;
using IKayak.Schemas.Models;

namespace IKayak.Persistency.Trips
{
    public interface ITripQuery : IQuery
    {
        List<TripContainer> GetAllTrips();

        bool SaveTrips(IList<TripContainer> tripContainers);
    }
}