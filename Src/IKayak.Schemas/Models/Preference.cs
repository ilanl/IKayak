namespace IKayak.Schemas.Models
{
    public class Preference
    {
        public long Id { get; set; }
        public long UserId { get; set; }

        public string DayOfWeek { get; set; }
        public Timing Time { get; set; }
        public KayakType Type { get; set; }

    }
}