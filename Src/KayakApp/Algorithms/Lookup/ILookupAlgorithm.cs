using System.Collections.Generic;
using System.Net;
using AppKickStart.Common.Providers.Algorithms;
using IKayak.Schemas.Models;

namespace IKayak.Algorithms.Lookup
{
    public interface ILookupAlgorithm:IAlgorithm
    {
        IList<TripContainer> GetTripLookups(Cookie cookie);
        IList<Kayak> GetKayakLookups(Cookie cookie);
    }
}