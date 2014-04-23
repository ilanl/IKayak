namespace IKayak.Schemas.Models
{
   
    public class Booking
    {
        public Booking()
        {
        }

        public Booking(long id, long userId, string tripKey, string kayakKey, string outingDate, string tripDay, string tripTime, string kayakName, KayakType kayakType)
        {
            Id = id;
            UserId = userId;
            TripKey = tripKey;
            KayakKey = kayakKey;
            State = BookingState.Active;
            OutingDate = outingDate;
            Day = tripDay;
            Time = tripTime;
            KayakName = kayakName;
            Type = kayakType;
        }

        public long Id { get; set; }

        public string KayakKey { get; set; }
        
        public KayakType Type { get; set; }

        public string TripKey { get; set; }

        public long UserId { get; set; }

        public BookingState State { get; set; }

        public string OutingDate { get; set; }

        public string Day { get; set; }

        public string Time { get; set; }
        
        public string KayakName { get; set; }
    }
}