using System;
using AppKickStart.Common.Providers;
using AppKickStart.Common.Providers.Algorithms;
using IKayak.Schemas.Models;

namespace IKayak.Algorithms
{
    public delegate void BookingUpdateHandler(Schemas.Models.Booking booking);

    public interface IAutoBookingAlgorithm : IAlgorithm
    {
        
        

    }
}