using System.Collections.Generic;

namespace IKayak.Schemas.Models
{
    public class BookingContainer
    {
        public BookingContainer()
        {
            Bookings = new List<Booking>();
        }

        public string OutingDate { get; set; }

        public IList<Booking> Bookings { get; set; }

        public string DayOfWeek { get; set; }
    }

}