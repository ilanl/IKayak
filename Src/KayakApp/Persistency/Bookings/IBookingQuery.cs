using System;
using System.Collections.Generic;
using AppKickStart.Common.Providers.Persistency;
using IKayak.Schemas.Models;

namespace IKayak.Persistency.Bookings
{
    public interface IBookingQuery : IQuery
    {
        IList<Booking> GetBookings(User user);
        Booking GetBookingByKey(User user, string tripKey);

        bool SaveBookings(IList<Booking> bookings);
        void CleanUp(DateTime tripDate);
        bool UpdateCancellation(Booking booking, BookingState state);
        IList<Booking> GetBookingsByState(User user, BookingState bookingState);
        IList<Booking> GetAll(User user);
        void Delete(Booking booking);
    }
}