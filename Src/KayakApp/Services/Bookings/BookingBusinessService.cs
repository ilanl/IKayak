using System.Collections.Generic;
using System.Net;
using System.Threading;
using AppKickStart.Common.Providers;
using AppKickStart.Common.Providers.Persistency;
using AppKickStart.Common.Providers.Services;
using AppKickStart.Schemas.Contracts.Enums;
using IKayak.Algorithms.Login;
using IKayak.Persistency.Bookings;
using IKayak.Schemas.Contracts.Bookings;
using IKayak.Schemas.Models;
using IKayak.Schemas.Models.Exceptions;

namespace IKayak.Services.Bookings
{
    public class BookingBusinessService :
        BusinessService<BookingRequest, IList<Booking>, BookingResponse>, IBookingBusinessService
    {
        private IPersistencyProvider _persistencyProvider;

        public BookingBusinessService()
        {
        }

        public BookingBusinessService(IAppContext appContext)
            : base(appContext)
        {
        }

        #region IBookingBusinessService Members

        public override string ServiceName
        {
            get { return "bookings"; }
        }

        public override string Request
        {
            get { return "BookingRequest"; }
        }

        public override string Response
        {
            get { return "BookingResponse"; }
        }

        public override IList<Booking> Execute(BookingRequest args)
        {
            IList<Booking> bookings = null;
            User user;

            _persistencyProvider = AppContext.PersistencyProvider;

            var algorithm = AppContext.AlgorithmProvider.Get<ILoginAlgorithm>();
            Cookie cookie = algorithm.Login(args.UserName, args.Password, args.DeviceToken, out user);
            if (cookie == null)
                throw new UserNotFoundBusinessException();

            var bookingQuery = _persistencyProvider.Get<IBookingQuery>();

            switch (args.Action)
            {
                case Action.Lookup:
                    bookings = bookingQuery.GetBookings(user);
                    break;

                case Action.Cancel:
                    new Thread(() =>
                    {

                        foreach (string key in args.Keys)
                        {
                            Booking booking = bookingQuery.GetBookingByKey(user, key);
                            bookingQuery.UpdateCancellation(booking, BookingState.PendingCancellation);
                        }

                    }).Start();
                    break;
            }

            return bookings;
        }

        #endregion

        public override BookingResponse BuildResponse(IList<Booking> bookings)
        {
            return new BookingResponse { Bookings = bookings, Status = "Success" };
        }
    }
}