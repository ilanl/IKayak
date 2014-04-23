using AppKickStart.Schemas.ErrorHandling;

namespace IKayak.Schemas.Models.Exceptions
{
    public class TripExpiredBusinessException : BusinessException
    {
        public TripExpiredBusinessException() : base("trip expired")
        {
        }
    }
}