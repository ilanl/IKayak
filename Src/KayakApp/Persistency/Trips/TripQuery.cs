using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Transactions;
using AppKickStart.Common.Providers;
using AppKickStart.Schemas.Tools;
using Dapper;
using IKayak.Persistency.Bookings;
using IKayak.Schemas.Models;

namespace IKayak.Persistency.Trips
{
    public class TripQuery : SqLiteBaseRepository, ITripQuery
    {
        private readonly IAppContext _appContext;
        private IBookingQuery _bookingQuery;

        public TripQuery(IAppContext appContext)
        {
            _appContext = appContext;
        }

        #region ITripQuery Members

        public List<TripContainer> GetAllTrips()
        {
            var allTripContainers = new List<TripContainer>();

            using (SQLiteConnection cnn = SimpleDbConnection())
            {
                cnn.Open();

                List<dynamic> dynamic = cnn.Query<dynamic>(
                    @"SELECT Id, TripKey, DayOfWeek, OutingDate, Time, Hour
                    FROM Trip")
                    .ToList();
                
                foreach (dynamic o in dynamic)
                {
                    TripContainer tripContainerTemp;

                    dynamic o1 = o;
                    if (
                        (tripContainerTemp =
                         allTripContainers.Where(c => c.DayOfWeek.Equals(o1.DayOfWeek)).FirstOrDefault()) == null)
                    {
                        tripContainerTemp = new TripContainer(o.DayOfWeek);
                        allTripContainers.Add(tripContainerTemp);
                    }
                    tripContainerTemp.Add(o.TripKey, o.OutingDate, o.Hour);
                }
            }

            return allTripContainers;
        }


        /// <summary>
        /// Always override with the new trips, there no point keeping trips that are not on the original website
        /// </summary>
        /// <param name="tripContainers"></param>
        /// <returns></returns>
        public bool SaveTrips(IList<TripContainer> tripContainers)
        {
            DateTime? dateTimeStartTripsTemp = null;

            using (SQLiteConnection cnn = SimpleDbConnection())
            {
                cnn.Open();
                using (var transactionScope = new TransactionScope())
                {
                    cnn.Query<long>(
                   @"delete from Trip");

                    foreach (TripContainer tc in tripContainers)
                    {
                        foreach (Trip trip in tc.Trips)
                        {
                            DateTime tripDate = TimeTools.ToIsraelTime(trip.OutingDate);
                            dateTimeStartTripsTemp = !dateTimeStartTripsTemp.HasValue || tripDate < dateTimeStartTripsTemp.Value
                                                         ? tripDate
                                                         : dateTimeStartTripsTemp;

                            trip.Id = cnn.Query<long>(
                                @"INSERT INTO Trip 
                    ( TripKey, DayOfWeek, OutingDate, Time, Hour) VALUES 
                    ( @TripKey, @DayOfWeek, @OutingDate, @Time, @Hour);
                    select last_insert_rowid()",
                                new
                                {
                                    TripKey = trip.Key,
                                    trip.Container.DayOfWeek,
                                    trip.OutingDate,
                                    trip.Time,
                                    trip.Hour
                                }).First();
                        }

                    }

                    transactionScope.Complete();
                }
                
                //Call clean-up for bookings and rules
                _bookingQuery = _appContext.PersistencyProvider.Get<IBookingQuery>();

                if (dateTimeStartTripsTemp.HasValue)
                    _bookingQuery.CleanUp(dateTimeStartTripsTemp.Value);

            }

            return true;
        }

        #endregion
    }
}