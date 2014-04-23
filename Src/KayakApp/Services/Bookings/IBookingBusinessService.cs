using System.Collections.Generic;
using AppKickStart.Common.Providers.Services;
using IKayak.Schemas.Contracts.Bookings;
using IKayak.Schemas.Models;

namespace IKayak.Services.Bookings
{
    public interface IBookingBusinessService : IBusinessHandler
    {
        IList<Booking> Execute(BookingRequest args);
    }
}