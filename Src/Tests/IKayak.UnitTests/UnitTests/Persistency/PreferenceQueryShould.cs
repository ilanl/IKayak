using System.Collections.Generic;
using System.IO;
using AppKickStart.Common.Providers;
using AppKickStart.Common.Providers.Persistency;
using IKayak.Persistency;
using IKayak.Persistency.Preferences;
using IKayak.Persistency.Users;
using IKayak.Schemas.Models;
using Moq;
using NUnit.Framework;

namespace IKayak.Tests.UnitTests.Persistency
{
    [TestFixture]
    public class PreferenceQueryShould
    {
        private const string UserName = "אילן ל";

        [SetUp]
        public void SetUp()
        {
            //Clean old sqlite file
            if (File.Exists(SqLiteBaseRepository.DbFile))
                File.Delete(SqLiteBaseRepository.DbFile);

            SqLiteBaseRepository.CreateDatabase();
        }
        [Test]
        public void SaveUserAndPrefs()
        {
            var appContextMock = new Mock<IAppContext>();
            var persistencyProviderMock = new Mock<IPersistencyProvider>();
            var userQueryMock = new Mock<IUserQuery>();

            var stamUser = new User { Id = 1, Name = UserName };
            userQueryMock.Setup(o => o.GetUser(UserName)).Returns(stamUser);
            persistencyProviderMock.Setup(o => o.Get<IUserQuery>()).Returns(userQueryMock.Object);
            appContextMock.Setup(o => o.PersistencyProvider).Returns(persistencyProviderMock.Object);

            var query = new PreferenceQuery(appContextMock.Object);
            
            var prefs = new List<LightTimePref>
                            {
                                new LightTimePref
                                    {
                                        DayOfWeek = "Friday",
                                        Time = Timing.Morning,
                                        Type = KayakType.Kayak
                                    },
                                new LightTimePref
                                    {
                                        DayOfWeek = "Friday",
                                        Time = Timing.Afternoon,
                                        Type = KayakType.SurfSki
                                    },
                                new LightTimePref
                                    {
                                        DayOfWeek = "Sunday",
                                        Time = Timing.Late,
                                        Type = KayakType.Any
                                    }
                            };

            bool b = query.SaveUserPrefs(prefs, stamUser);
            Assert.IsTrue(b);

            var prefs2 = query.GetUserPrefsById(stamUser.Id);
            
            Assert.AreEqual(prefs[0].DayOfWeek, prefs2[0].DayOfWeek);
            Assert.AreEqual(prefs[1].Time, prefs2[1].Time);
            Assert.AreEqual(prefs[1].Type, prefs2[1].Type);
            Assert.AreEqual((int)prefs[2].Type, 3);
        }
    }
}