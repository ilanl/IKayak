namespace IKayak.Schemas.Models
{
    public class LightTimePref
    {
        public string DayOfWeek { get; set; }
        public Timing Time { get; set; }
        public KayakType Type { get; set; }
    }
}