using AppKickStart.Schemas.ErrorHandling;

namespace IKayak.Schemas.Models.Exceptions
{
    public class UserGeneralBusinessException : BusinessException
    {
        public UserGeneralBusinessException()
            : base("user general error")
        {
        }
    }
}