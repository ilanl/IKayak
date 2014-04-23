using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Transactions;
using AppKickStart.Common.Providers;
using AppKickStart.Schemas.Tools;
using Dapper;
using IKayak.Schemas.Models;

namespace IKayak.Persistency.Bookings
{
    public class BookingQuery : SqLiteBaseRepository, IBookingQuery
    {

        
        public BookingQuery(IAppContext appContext)
        {
        }

        #region IBookingQuery Members

        public IList<Booking> GetBookings(User user)
        {
            var bookings = new List<Booking>();

            using (SQLiteConnection cnn = SimpleDbConnection())
            {
                cnn.Open();

                List<dynamic> dynamic = cnn.Query<dynamic>(
                    @"SELECT Id, UserId, TripKey, KayakKey, State, OutingDate, Type, Day,Time, KayakName 
                    FROM Booking WHERE State = @State", new { State = BookingState.Active }).Where
                    (o => user == null || o.UserId.Equals(user.Id)).ToList();

                bookings.AddRange(
                    dynamic.Select(o =>
                                       {
                                           var booking = new Booking(o.ID, o.UserId, o.TripKey, o.KayakKey, o.OutingDate, o.Day, o.Time, o.KayakName, (KayakType)o.Type) { State = (BookingState)o.State };
                                           return booking;
                                       }));
            }

            return bookings;
        }

        public IList<Booking> GetAll(User user)
        {
            var bookings = new List<Booking>();

            using (SQLiteConnection cnn = SimpleDbConnection())
            {
                cnn.Open();

                List<dynamic> dynamic = cnn.Query<dynamic>(
                    @"SELECT Id, UserId, TripKey, KayakKey, State, OutingDate, Type, Day,Time, KayakName
                    FROM Booking").Where
                    (o => user == null || o.UserId.Equals(user.Id)).ToList();

                bookings.AddRange(
                    dynamic.Select(o =>
                    {
                        var booking = new Booking(o.ID, o.UserId, o.TripKey, o.KayakKey, o.OutingDate, o.Day, o.Time, o.KayakName, (KayakType)o.Type) { State = (BookingState)o.State };
                        return booking;
                    }));
            }

            return bookings;
        }

        public void Delete(Booking booking)
        {
            using (SQLiteConnection cnn = SimpleDbConnection())
            {
                cnn.Open();

                cnn.Query<long>(
                    @"delete from Booking where Id = @Id",
                    new { booking.Id });
            }
        }

        public Booking GetBookingByKey(User user, string tripKey)
        {
            Booking booking = null;

            using (SQLiteConnection cnn = SimpleDbConnection())
            {
                cnn.Open();

                List<dynamic> dynamic = cnn.Query<dynamic>(
                    @"SELECT Id, UserId, TripKey, KayakKey, State, OutingDate, Type, Day,Time, KayakName
                    FROM Booking WHERE State = @State AND TripKey = @TripKey", new { State = BookingState.Active, TripKey = tripKey }).Where
                    (o => user == null || o.UserId.Equals(user.Id))
                    .ToList();

                foreach (dynamic o in dynamic)
                {
                    booking = new Booking(o.ID, o.UserId, o.TripKey, o.KayakKey, o.OutingDate, o.Day, o.Time, o.KayakName, (KayakType)o.Type);
                    break;
                }
            }

            return booking;
        }

        public bool SaveBookings(IList<Booking> bookings)
        {
            if (!bookings.Any())
                return true;

            var userId = bookings.First().UserId;

            using (SQLiteConnection cnn = SimpleDbConnection())
            {
                cnn.Open();
                using (var transactionScope = new TransactionScope())
                {
                    cnn.Query<long>(
                        @"delete from Booking where UserId = @UserId AND State = @State",
                        new { UserId = userId, State = BookingState.Active });

                    foreach (Booking b in bookings)
                    {
                        b.Id = cnn.Query<long>(
                            @"INSERT INTO Booking 
                    ( UserId, TripKey, KayakKey, State, OutingDate, Type, Day,Time, KayakName) VALUES 
                    ( @UserId, @TripKey, @KayakKey, @State, @OutingDate, @Type, @Day, @Time, @KayakName);
                    select last_insert_rowid()",
                            new
                                {
                                    UserId = userId,
                                    b.TripKey,
                                    b.KayakKey,
                                    State = (int)b.State,
                                    b.OutingDate,
                                    b.Type,
                                    b.Day,
                                    b.Time,
                                    b.KayakName
                                }).First();
                    }

                    transactionScope.Complete();
                }
            }
            return true;
        }

        public void CleanUp(DateTime tripDate)
        {
            IList<Booking> allBookings = GetBookings(null);

            foreach (Booking booking in allBookings)
            {
                if ((tripDate - TimeTools.ToIsraelTime(booking.OutingDate)).Days <= 1)
                    continue;

                using (SQLiteConnection cnn = SimpleDbConnection())
                {
                    cnn.Open();

                    cnn.Query<long>(
                        @"delete from Booking where OutingDate = @outingDate",
                        new { booking.OutingDate });
                }
            }
        }

        public bool UpdateCancellation(Booking booking, BookingState state)
        {
            if (booking == null)
                return false;

            using (SQLiteConnection cnn = SimpleDbConnection())
            {
                cnn.Open();

                cnn.Execute(
                    @"UPDATE Booking
                      SET State = @State
                      WHERE TripKey = @TripKey AND UserId = @UserId AND KayakKey = @KayakKey",
                    new { State = (int)state, booking.TripKey, booking.KayakKey, booking.UserId });
            }

            return true;
        }

        #endregion

        public IList<Booking> GetBookingsByState(User user, BookingState bookingState)
        {
            var bookings = new List<Booking>();

            using (SQLiteConnection cnn = SimpleDbConnection())
            {
                cnn.Open();

                List<dynamic> dynamic = cnn.Query<dynamic>(
                    @"SELECT Id, UserId, TripKey, KayakKey, State, OutingDate, Type, Day,Time, KayakName
                    FROM Booking WHERE State = @State", new { State = bookingState }).Where
                    (o => user == null || o.UserId.Equals(user.Id)).ToList();

                bookings.AddRange(
                    dynamic.Select(o =>
                    {
                        var booking = new Booking(o.ID, o.UserId, o.TripKey, o.KayakKey, o.OutingDate, o.Day, o.Time, o.KayakName, (KayakType)o.Type) { State = (BookingState)o.State };
                        return booking;
                    }));
            }

            return bookings;
        }
    }
}