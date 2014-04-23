namespace IKayak.Schemas.Models
{
    public class KayakPref
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Key { get; set; }
        public short Weight { get; set; }
    }

}