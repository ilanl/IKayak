namespace IKayak.Schemas.Models
{
    public class Trip
    {
        private readonly string _key;
        private readonly string _outingDate;

        public long Id { get; set; }

        public Trip(string key, string outingDate, string hour)
        {
            _key = key;
            _outingDate = outingDate;
            Hour = hour;
        }

        public string Key
        {
            get { return _key; }
        }

        public string OutingDate
        {
            get { return _outingDate; }
        }

        public Timing Time { get; set; }

        public string Hour { get; set; }

        public TripContainer Container { get; set; }
    }
}