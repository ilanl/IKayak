using AppKickStart.Schemas.Contracts;
using IKayak.Schemas.Models;

namespace IKayak.Schemas.Contracts.Preferences
{
    public class PreferenceResponse : BaseResponse
    {
        public bool IsFrozen { get; set; }
        public Set Set { get; set; }

        public int Reminder { get; set; }
    }
}