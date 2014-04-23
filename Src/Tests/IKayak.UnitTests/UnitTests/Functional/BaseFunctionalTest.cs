using System.Configuration;
using System.IO;
using AppKickStart.Common.Providers;
using AppKickStart.Common.Providers.Persistency;
using IKayak.Algorithms.Book;
using IKayak.Persistency.Bookings;
using IKayak.Persistency.Kayaks;
using IKayak.Persistency.Preferences;
using IKayak.Persistency.Trips;
using IKayak.Persistency.Users;
using IKayak.Schemas.Models;
using Moq;
using NUnit.Framework;

namespace IKayak.Tests.UnitTests.Functional
{
    [TestFixture]
    public class BaseFunctionalTest
    {
        protected BookingAlgorithm BookingAlgorithm;
        protected Mock<IAppContext> AppMock;
        protected KayakQuery KayakQuery;
        protected UserQuery UserQuery;
        protected TripQuery TripQuery;
        protected PreferenceQuery TimingPrefQuery;
        protected KayakPrefQuery KayakPrefQuery;
        protected BookingQuery BookingQuery;
        protected User CurrentUser;
        protected const string Password = "32371";
        protected const string UserName = @"%D7%90%D7%99%D7%9C%D7%9F%20%D7%9C";
        
        [SetUp]
        protected void CleanDb()
        {
            File.Delete(ConfigurationManager.AppSettings["SqliteDb"]);

            AppMock = new Mock<IAppContext>();

            //Reals
            KayakQuery = new KayakQuery();
            UserQuery = new UserQuery();
            TripQuery = new TripQuery(AppMock.Object);
            TimingPrefQuery = new PreferenceQuery(AppMock.Object);
            KayakPrefQuery = new KayakPrefQuery(AppMock.Object);
            BookingQuery = new BookingQuery(AppMock.Object);

            //Save preferences
            var persistencyProviderMock = new Mock<IPersistencyProvider>();
            persistencyProviderMock.Setup(o => o.Get<ITripQuery>()).Returns(TripQuery);
            persistencyProviderMock.Setup(o => o.Get<IPreferenceQuery>()).Returns(TimingPrefQuery);
            persistencyProviderMock.Setup(o => o.Get<IKayakPrefQuery>()).Returns(KayakPrefQuery);
            persistencyProviderMock.Setup(o => o.Get<IKayakQuery>()).Returns(KayakQuery);
            persistencyProviderMock.Setup(o => o.Get<IUserQuery>()).Returns(UserQuery);
            persistencyProviderMock.Setup(o => o.Get<IBookingQuery>()).Returns(BookingQuery);

            AppMock.Setup(o => o.PersistencyProvider).Returns(persistencyProviderMock.Object);
        }
    }
}