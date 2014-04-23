using System;
using System.Collections.Generic;
using System.IO;
using AppKickStart.Common.Providers;
using AppKickStart.Common.Providers.Persistency;
using AppKickStart.Schemas.Tools;
using IKayak.Persistency;
using IKayak.Persistency.Bookings;
using IKayak.Persistency.Users;
using IKayak.Schemas.Models;
using Moq;
using NUnit.Framework;

namespace IKayak.Tests.UnitTests.Persistency
{
    [TestFixture]
    public class BookingQueryShould
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
        public void SaveBookingAndRules()
        {
            var appContextMock = new Mock<IAppContext>();
            var persistencyProviderMock = new Mock<IPersistencyProvider>();
            var userQueryMock = new Mock<IUserQuery>();
            var user = new User{ Id = 1, Name = "אילן ל", Password = "32371"};
            userQueryMock.Setup(o => o.GetUser("אילן ל")).Returns(user);
            userQueryMock.Setup(o => o.GetUserById(1)).Returns(user);

            persistencyProviderMock.Setup(o => o.Get<IUserQuery>()).Returns(userQueryMock.Object);
            
            appContextMock.Setup(o => o.PersistencyProvider).Returns(persistencyProviderMock.Object);
            
            var query = new BookingQuery(appContextMock.Object);
            var tripContainer = new TripContainer("Sunday");
            const string tripKey = "1";
            tripContainer.Add(tripKey, "123456712455", "06:30");

            var booking = new Booking { UserId = user.Id, KayakKey = "nn", TripKey = tripKey, OutingDate = TimeTools.ConvertToUnixTimestamp(DateTime.UtcNow), State = BookingState.Active, Day = "שני", KayakName = "קאיק 14" , Time = "06:30", Type = KayakType.Kayak };
            bool p = query.SaveBookings(new List<Booking>{ booking});
            Assert.IsTrue(p);
            var bookingsSaved = query.GetBookings(user);

            Assert.AreEqual(booking.UserId, bookingsSaved[0].UserId);
            Assert.AreEqual(booking.KayakKey, bookingsSaved[0].KayakKey);
            Assert.AreEqual(booking.TripKey, bookingsSaved[0].TripKey);
            Assert.AreEqual(booking.OutingDate, bookingsSaved[0].OutingDate);
            Assert.AreEqual(booking.Time, bookingsSaved[0].Time);

        }
    }
}