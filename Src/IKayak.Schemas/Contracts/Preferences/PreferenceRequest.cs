using AppKickStart.Schemas.Contracts;
using AppKickStart.Schemas.Contracts.Enums;
using IKayak.Schemas.Models;

namespace IKayak.Schemas.Contracts.Preferences
{
    public class PreferenceRequest : BaseRequest
    {
        
        public Action Action { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string IsFrozen { get; set; }
        public int Reminder { get; set; }
        public string DeviceToken { get; set; }
        
        public Set Set { get; set; }
    }
}