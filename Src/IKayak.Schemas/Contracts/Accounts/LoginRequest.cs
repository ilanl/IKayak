using AppKickStart.Schemas.Contracts;

namespace IKayak.Schemas.Contracts.Accounts
{
    public class LoginRequest : BaseRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public string DeviceToken { get; set; }
    }
}