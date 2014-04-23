using System;
using System.Collections.Generic;
using System.Linq;
using AppKickStart.Client;
using IKayak.Schemas.Models;
using NUnit.Framework;

namespace IKayak.Tests.IntegrationTests.Configure
{
    [TestFixture]
    [Category("Scenarios")]
    public class ScenarioShould : ShouldBase
    {
        //                        %D7%90%D7%99%D7%9C%D7%9F%20%D7%9C
        const string UserName = @"%D7%90%D7%99%D7%9C%D7%9F%20%D7%9C";
        private const string DeviceToken = "3bd026e55b59219bc06662bc548b5397100981ea26f7b9de7fae4a670d1effde";
        const string Password = "32371";
            
        public ScenarioShould()
        {
            Si = new ServiceInvoker();
        }

        [Test]
        public void GetBookings()
        {
            WakeUpServer();
            var response = GetBookings(UserName, Password, DeviceToken);
            Assert.IsTrue(response.Bookings.Any());
        }
        
        [Test]
        public void GetWeather()
        {
            var response = GetForecasts();
            Assert.IsTrue(response.Forecasts.Any());
        }

        [Test]
        public void Scenario()
        {
            WakeUpServer();

            Login(UserName, Password, DeviceToken);

            Wait(20);

            var preferenceResponse = GetPreferences(UserName, Password, DeviceToken);
            Assert.IsTrue(preferenceResponse.Set.KayakPrefs.Any());
            
            int i = 0;
            LightKayakPref oneKayak = null;
            foreach (var k in preferenceResponse.Set.KayakPrefs)
            {
                if (i == 0)
                    oneKayak = k;
                Console.WriteLine(String.Format(@"{0}. {1} {2}", ++i, k.Name, k.Weight));
            }

            short j= 0;
            foreach (var kp in preferenceResponse.Set.KayakPrefs)
            {
                kp.Weight = (short) (++j % 3);
            }
            
            var timingPrefs = new List<LightTimePref>
                                  {
                                      new LightTimePref
                                          {DayOfWeek = "Friday", Time = Timing.Afternoon, Type = KayakType.SurfSki},
                                      new LightTimePref { DayOfWeek = "Sunday", Time = Timing.Morning, Type = KayakType.Kayak },
                                      new LightTimePref { DayOfWeek = "Friday", Time = Timing.Late, Type = KayakType.Any },
                                  };
            
            SavePreferences(UserName, Password,DeviceToken, timingPrefs, preferenceResponse.Set.KayakPrefs);

            preferenceResponse = GetPreferences(UserName, Password, DeviceToken);
            Assert.IsTrue(preferenceResponse.Set.TimePrefs.Any(o=>o.DayOfWeek == "Friday" && o.Time == Timing.Afternoon && o.Type == KayakType.SurfSki));
            Assert.IsTrue(preferenceResponse.Set.KayakPrefs.Any(o => o.Type == oneKayak.Type && o.Weight == oneKayak.Weight && o.Key == oneKayak.Key));

            UpdateUserStateAndReminder(UserName, Password, DeviceToken, "1", 45); //freeze

            var bookings = GetBookings(UserName, Password, DeviceToken);

            Assert.IsTrue(bookings.Bookings.Count == 0);

            UpdateUserStateAndReminder(UserName, Password, DeviceToken, @"0", 20); //activate

            Wait(120);

            Assert.IsTrue(bookings.Bookings.Count >= 0);

        }

    }
}
