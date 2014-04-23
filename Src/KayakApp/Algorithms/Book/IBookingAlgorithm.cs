using System.Collections.Generic;
using System.Net;
using AppKickStart.Common.Providers.Algorithms;
using IKayak.Schemas.Models;

namespace IKayak.Algorithms.Book
{
    public interface IBookingAlgorithm:IAlgorithm
    {
        bool DoBookings(Cookie cookie, User user);
        bool CancelBooking(Cookie cookie, Booking booking);
        IList<Booking> SyncBookings(Cookie cookie, User user,  IList<TripContainer> tripContainers);
    }
}