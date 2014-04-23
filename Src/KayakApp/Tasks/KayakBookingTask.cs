using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using AppKickStart.Common.Logging.Wrapper;
using AppKickStart.Common.Providers;
using AppKickStart.Schemas.ErrorHandling;
using IKayak.Algorithms.Book;
using IKayak.Algorithms.Login;
using IKayak.Algorithms.Lookup;
using IKayak.Notifiers;
using IKayak.Persistency.Bookings;
using IKayak.Persistency.Users;
using IKayak.Schemas.Models;

namespace IKayak.Tasks
{
    public class KayakBookingTask : IKayakBookingTask
    {
        private readonly IAppContext _appContext;

        private IBookingNotifier _bookingNotifier;
        private readonly IBookingQuery _bookingQuery;
        private readonly IUserQuery _userQuery;
        private readonly ILookupAlgorithm _lkUpAlgorithm;
        private readonly ILoginAlgorithm _loginAlgorithm;
        private readonly IBookingAlgorithm _bookingAlgorithm;


        public KayakBookingTask(IAppContext appContext)
        {
            _isFirstTime = true;
            _appContext = appContext;
            StartAt = null;
            
            int intervalSecs = 300;
            Int32.TryParse(ConfigurationManager.AppSettings["BookingsIntervalSecs"], out intervalSecs);
            IntervalSeconds = intervalSecs;
            
            Name = "KayakBookingTask";

            _bookingQuery = _appContext.PersistencyProvider.Get<IBookingQuery>();
            _userQuery = _appContext.PersistencyProvider.Get<IUserQuery>();
            _lkUpAlgorithm = _appContext.AlgorithmProvider.Get<ILookupAlgorithm>();
            _loginAlgorithm = _appContext.AlgorithmProvider.Get<ILoginAlgorithm>();
            _bookingAlgorithm = _appContext.AlgorithmProvider.Get<IBookingAlgorithm>();
        }

        #region IKayakBookingTask Members

        public int? StartAt { get; set; }
        public string Name { get; set; }
        public int IntervalSeconds { get; set; }
        private bool _isFirstTime;
        public void DoWork()
        {
            try
            {
                var users = _userQuery.GetAll();
                
                foreach (var listedUser in users)
                {
                    //Use default user
                    User user;
                    Cookie cookie = _loginAlgorithm.Login(listedUser.Name, listedUser.Password, listedUser.DeviceToken, out user);
                    if (_isFirstTime)
                    {
                        _lkUpAlgorithm.GetKayakLookups(cookie);
                        _isFirstTime = false;
                    }
                    
                    var userBookings = _bookingQuery.GetBookings(user);
                    //check for cancellation pending
                    var bookingsToCancel = _bookingQuery.GetBookingsByState(user, BookingState.PendingCancellation);
                    foreach (var bookingToCancel in bookingsToCancel)
                    {
                        _bookingAlgorithm.CancelBooking(cookie, bookingToCancel);
                    }

                    if (user.IsFrozen)
                        continue;
                    
                    //get lookups
                    var tripContainers = _lkUpAlgorithm.GetTripLookups(cookie);
                    _bookingAlgorithm.SyncBookings(cookie, user, tripContainers);
                    _bookingAlgorithm.DoBookings(cookie, user);
                    var userBookings2 = _bookingQuery.GetBookings(user);

                    if (VerifyBookingChanges(userBookings, userBookings2))
                    {
                        _bookingNotifier = _appContext.NotificationProvider.Get<IBookingNotifier>();
                        _bookingNotifier.Send(userBookings2, user);
                    }
                }
            }
            catch (BusinessException ex)
            {
                Logger.Log(LoggingLevel.Error, "A bug has occured: " + ex.StackTrace, null, ex);
            }
            catch (Exception ex)
            {
                Logger.Log(LoggingLevel.Error, "An error occured: "+ ex.StackTrace, null, ex);
            }
        }

        private static bool VerifyBookingChanges(IEnumerable<Booking> bookingsBefore, IEnumerable<Booking> bookingsAfter)
        {
            List<string> query1 = (from t in bookingsBefore orderby t.OutingDate select t.KayakKey + t.TripKey).ToList();
            IEnumerable<string> query2 = from t in bookingsAfter orderby t.OutingDate select t.KayakKey + t.TripKey;
            if (string.Join(",", query1) != string.Join(",", query2))
            {
                Logger.Log(LoggingLevel.Info, "New bookings to notify");
                return true;
            }
            return false;
        }

        #endregion

    }
}