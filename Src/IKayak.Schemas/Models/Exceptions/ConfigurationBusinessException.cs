using AppKickStart.Schemas.ErrorHandling;

namespace IKayak.Schemas.Models.Exceptions
{
    public class UserNotFoundBusinessException : BusinessException
    {
        public UserNotFoundBusinessException() : base("no user")
        {
        }
    }
}