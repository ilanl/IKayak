using AppKickStart.Schemas.ErrorHandling;

namespace IKayak.Schemas.Models.Exceptions
{
    public class ActionNotSupportedBusinessException : BusinessException
    {
        public ActionNotSupportedBusinessException()
            : base("action not supported")
        {
        }
    }
}