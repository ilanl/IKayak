using System;
using System.Collections.Generic;
using System.IO;
using AppKickStart.Common.Providers;
using AppKickStart.Common.Providers.Persistency;
using AppKickStart.Schemas.Tools;
using IKayak.Persistency;
using IKayak.Persistency.Bookings;
using IKayak.Persistency.Trips;
using IKayak.Schemas.Models;
using Moq;
using NUnit.Framework;

namespace IKayak.Tests.UnitTests.Persistency
{
    [TestFixture]
    public class TripQueryShould
    {
        [SetUp]
        public void SetUp()
        {
            //Clean old sqlite file
            if (File.Exists(SqLiteBaseRepository.DbFile))
                File.Delete(SqLiteBaseRepository.DbFile);

            SqLiteBaseRepository.CreateDatabase();
        }

        [Test]
        public void SaveTripsAndCleanUpOldBookingsAndRules()
        {
            var dateTime = DateTime.Now;
            var appContextMock = new Mock<IAppContext>();
            var persistencyProviderMock = new Mock<IPersistencyProvider>();

            var bookingQueryMock = new Mock<IBookingQuery>();
            bookingQueryMock.Setup(o => o.CleanUp(dateTime.AddDays(-1)));

            persistencyProviderMock.Setup(o => o.Get<IBookingQuery>()).Returns(bookingQueryMock.Object);
            appContextMock.Setup(o => o.PersistencyProvider).Returns(persistencyProviderMock.Object);
            var query = new TripQuery(appContextMock.Object);

            var outingDate1 = TimeTools.ConvertToUnixTimestamp(dateTime.AddDays(-1));
            var containerDay1 = new TripContainer("Saturday");
            const string hour = "06:00";
            containerDay1.Add("trip_1_1", outingDate1, hour);
            containerDay1.Add("trip_1_2", outingDate1, hour);

            var outingDate2 = TimeTools.ConvertToUnixTimestamp(dateTime);
            var containerDay2 = new TripContainer("Sunday");
            containerDay2.Add("trip_2_1", outingDate2, hour);
            
            var outingDate3 = TimeTools.ConvertToUnixTimestamp(dateTime.AddDays(1));
            var containerDay3 = new TripContainer("Monday");
            containerDay3.Add("trip_3_1", outingDate3, hour);
            containerDay3.Add("trip_3_2", outingDate3, hour);

            var set = new List<TripContainer>
                          {
                              containerDay1,
                              containerDay2,
                              containerDay3
                          };

            bool b = query.SaveTrips(set);
            Assert.IsTrue(b);

            var tripsSaved = query.GetAllTrips();
            
            Assert.AreEqual(tripsSaved[0].DayOfWeek, set[0].DayOfWeek);
            Assert.AreEqual(tripsSaved[0].Trips[0].Time, set[0].Trips[0].Time);
            Assert.AreEqual(tripsSaved[0].Trips[0].OutingDate, set[0].Trips[0].OutingDate);
            Assert.AreEqual(tripsSaved[0].Trips[0].Hour, set[0].Trips[0].Hour);

            Assert.AreEqual(tripsSaved[1].DayOfWeek, set[1].DayOfWeek);
            Assert.AreEqual(tripsSaved[2].DayOfWeek, set[2].DayOfWeek);
            
            bookingQueryMock.Verify(o=>o.CleanUp(It.IsAny<DateTime>()), Times.Exactly(1));
        }
    }
}