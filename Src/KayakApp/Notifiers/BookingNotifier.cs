using System.Collections.Generic;
using AppKickStart.Common.Notifications;
using AppKickStart.Common.Providers;
using AppKickStart.Common.Providers.Notifications;
using IKayak.Schemas.Models;

namespace IKayak.Notifiers
{

    
    public class BookingNotifier : NotifierBase, IBookingNotifier
    {
        protected readonly IAppContext AppContext;

        public BookingNotifier(IAppContext appContext)
        {
            AppContext = appContext;
        }

        #region IBookingNotifier Members

        public void Send(IList<Booking> allActiveBookings, User user)
        {
            if (!string.IsNullOrEmpty(user.DeviceToken))
                new PushNotificationService().SendPushNotification(user.DeviceToken, "Check out your new bookings!");

            /*
            //ThreadPool.QueueUserWorkItem(o =>
            //                                 {
                                                 try
                                                 {
                                                     var templateView =
                                                         new TemplateView(
                                                             PathMap.Get(@"resources\templates\LastBookingSummary.vm"));

                                                     var bookingContainer = new List<BookingContainer>();

                                                     foreach (var booking in allActiveBookings)
                                                     {
                                                         Booking booking1 = booking;
                                                         BookingContainer container =
                                                             bookingContainer.Where(
                                                                 c => c.OutingDate == booking1.OutingDate).
                                                                 SingleOrDefault();
                                                         if (container == null)
                                                         {

                                                             container = new BookingContainer
                                                                             {
                                                                                 DayOfWeek = TimeTools.ToIsraelTime(booking.OutingDate).DayOfWeek.ToString(),
                                                                                 OutingDate = booking.OutingDate
                                                                             };
                                                             bookingContainer.Add(container);
                                                         }
                                                         container.Bookings.Add(booking1);
                                                     }

                                                     IDictionary context = new Hashtable
                                                                               {
                                                                                   {"containers", bookingContainer},
                                                                                   {"user", user},
                                                                                   {"domain",ConfigurationManager.AppSettings["domain"]}
                                                                               };


                                                     var message =
                                                         new MailMessage(
                                                             new MailAddress("ilan.levy78@gmail.com", "Kayak Service"),
                                                             new MailAddress("ilan.levy78@gmail.com"))
                                                             {
                                                                 IsBodyHtml = true,
                                                                 Subject =
                                                                     "Kayak bookings " + DateTime.Now.ToLongTimeString(),
                                                             };


                                                     base.Send(context, templateView, message);
                                                 }
                                                 catch (Exception e)
                                                 {
                                                     Logger.Log(LoggingLevel.Error, "Failed to send email:", null, e);
                                                 }
                                             //});*/
        }

        #endregion
             
            
    }
}