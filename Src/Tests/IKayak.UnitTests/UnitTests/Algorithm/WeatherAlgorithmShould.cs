using System.Linq;
using AppKickStart.Common.Providers;
using AppKickStart.Common.Providers.Persistency;
using IKayak.Algorithms.Login;
using IKayak.Persistency.Forecasts;
using Moq;
using NUnit.Framework;

namespace IKayak.Tests.UnitTests.Algorithm
{
    [TestFixture]
    public class WeatherAlgorithmShould
    {
        [Test]
        public void GetForecast()
        {
            var appContextMock = new Mock<IAppContext>();
            var forecastQuery = new ForecastQuery(appContextMock.Object);
            var mock = new Mock<IPersistencyProvider>();
            mock.Setup(o => o.Get<IForecastQuery>()).Returns(forecastQuery);
            appContextMock.Setup(o => o.PersistencyProvider).Returns(mock.Object);

            var algorithm = new WeatherAlgorithm(appContextMock.Object);
            algorithm.RetrieveLastForecasts("6q679s8xuvznesjj28aw2mpf", 32.103256, 34.77484);

            var forecasts = forecastQuery.GetNextDays(7, 600, 2000);

            Assert.IsTrue(forecasts.Any());
        }
    }
}
