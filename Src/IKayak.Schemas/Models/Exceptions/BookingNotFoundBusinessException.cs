using AppKickStart.Schemas.ErrorHandling;

namespace IKayak.Schemas.Models.Exceptions
{
    public class BookingNotFoundBusinessException : BusinessException
    {
        public BookingNotFoundBusinessException()
            : base("no booking")
        {
        }
    }
}