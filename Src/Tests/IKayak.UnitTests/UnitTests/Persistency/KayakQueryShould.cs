using AppKickStart.Common.Providers;
using AppKickStart.Common.Providers.Persistency;
using IKayak.Persistency.Kayaks;
using Moq;
using NUnit.Framework;

namespace IKayak.Tests.UnitTests.Persistency
{
    [TestFixture]
    public class KayakQueryShould
    {
        [SetUp]
        public void SetUp()
        {
            //Clean old sqlite file
            //if (File.Exists(SqLiteBaseRepository.DbFile))
            //    File.Delete(SqLiteBaseRepository.DbFile);

            //SqLiteBaseRepository.CreateDatabase();
        }

        [Test]
        public void SaveKayaksAndGet()
        {
            
            var appContextMock = new Mock<IAppContext>();
            var persistencyProviderMock = new Mock<IPersistencyProvider>();

            appContextMock.Setup(o => o.PersistencyProvider).Returns(persistencyProviderMock.Object);
            var query = new KayakQuery();
            
            //var set = new List<Kayak>
            //              {
            //                  new Kayak("1", "my kayak 1", KayakType.Kayak),
            //                  new Kayak("2", "XT", KayakType.SurfSki),
            //              };

            //bool b = query.SaveAll(set);
            //Assert.IsTrue(b);

            var saved = query.GetAll();

            //Assert.AreEqual(saved[0].Key, set[0].Key);
            //Assert.AreEqual(saved[1].Name, set[1].Name);
            //Assert.AreEqual(saved[1].Type, set[1].Type);
            
        }
    }
}