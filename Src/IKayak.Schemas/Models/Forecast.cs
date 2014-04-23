namespace IKayak.Schemas.Models
{
    public class Forecast
    {
        public string Date;
        public string Day;
        public string Hour;
        public string Weather;
        public string TempC;
        public string WaterTempC { get; set; }
        public string WaveH { get; set; }
        public string SwellSecs { get; set; }
        public string WindDir { get; set; }
        public string WindF { get; set; }
    }
}