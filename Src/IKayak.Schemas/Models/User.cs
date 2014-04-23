
namespace IKayak.Schemas.Models
{
    public class User
    {
        public User()
        {
            
        }

        public User(long userId)
        {
            Id = userId;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsFrozen { get; set; }
        public string Session { get; set; }
        public string DeviceToken { get; set; }
        public int Reminder { get; set; }

    }
}