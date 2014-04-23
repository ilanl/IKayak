using System.Collections.Generic;
using AppKickStart.Schemas.Contracts;
using IKayak.Schemas.Models;

namespace IKayak.Schemas.Contracts.Bookings
{
    public class BookingResponse : BaseResponse
    {
        public IList<Booking> Bookings { get; set; }
    }
}