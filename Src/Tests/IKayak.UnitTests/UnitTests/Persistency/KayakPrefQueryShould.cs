using System.Collections.Generic;
using System.IO;
using AppKickStart.Common.Providers;
using AppKickStart.Common.Providers.Persistency;
using IKayak.Persistency;
using IKayak.Persistency.Kayaks;
using IKayak.Schemas.Models;
using Moq;
using NUnit.Framework;

namespace IKayak.Tests.UnitTests.Persistency
{
    [TestFixture]
    public class KayakPrefQueryShould
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
        public void SaveKayaksPrefsAndGet()
        {
            
            var appContextMock = new Mock<IAppContext>();
            var persistencyProviderMock = new Mock<IPersistencyProvider>();

            appContextMock.Setup(o => o.PersistencyProvider).Returns(persistencyProviderMock.Object);
            var query = new KayakPrefQuery(appContextMock.Object);

            const int userId = 100;

            var set = new List<LightKayakPref>
                          {
                              new LightKayakPref{ Key = "1", Weight = 1},
                              new LightKayakPref{ Key = "20", Weight = 3},
                              new LightKayakPref{ Key = "12", Weight = 3},
                              new LightKayakPref{ Key = "14", Weight = 2},
                              new LightKayakPref{ Key = "15", Weight = 1},
                          };

            bool b = query.SaveUserKayaks(set, userId);
            Assert.IsTrue(b);

            var saved = query.GetAll();

            Assert.AreEqual(saved[0].Key, set[0].Key);
            Assert.AreEqual(saved[1].UserId, userId);
            Assert.AreEqual(saved[4].Weight, set[4].Weight);
            
        }
    }
}