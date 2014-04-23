namespace IKayak.Schemas.Models
{
    public class Kayak
    {
        
        public Kayak(string key, string name, KayakType type)
        {
            Type = type;
            Key = key;
            Name = name;
        }

        public long Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public KayakType Type { get; set; }
    }
}
