using System.Collections.Generic;
using AppKickStart.Schemas.Contracts;
using Action = AppKickStart.Schemas.Contracts.Enums.Action;

namespace IKayak.Schemas.Contracts.Bookings
{
    public class BookingRequest : BaseRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public Action Action { get; set; }
        public IList<string> Keys { get; set; }

        public string DeviceToken { get; set; }
    }
}