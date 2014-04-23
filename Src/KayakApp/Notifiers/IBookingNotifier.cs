using System.Collections.Generic;
using AppKickStart.Common.Providers.Notifications;
using IKayak.Schemas.Models;

namespace IKayak.Notifiers
{
    public interface IBookingNotifier : INotifier
    {
        void Send(IList<Booking> allActiveBookings, User user);
    }
}