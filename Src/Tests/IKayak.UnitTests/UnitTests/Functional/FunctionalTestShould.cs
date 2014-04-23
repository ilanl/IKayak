using System;
using System.Linq;
using AppKickStart.Common.Providers.Algorithms;
using AppKickStart.Common.Providers.Notifications;
using IKayak.Algorithms.Book;
using IKayak.Algorithms.Login;
using IKayak.Algorithms.Lookup;
using IKayak.Notifiers;
using IKayak.Schemas.Models;
using IKayak.Tasks;
using Moq;
using NUnit.Framework;

namespace IKayak.Tests.UnitTests.Functional
{
    [TestFixture]
    public class FunctionalTestShould : BaseFunctionalTest
    {
        private const string DeviceToken = "MYTOKEN";

        [Test]
        public void Scenario()
        {


            //Login and Lookup scenarios
            var loginAlgorithm = new LoginAlgorithm(AppMock.Object);
            var cookie = loginAlgorithm.Login(UserName, Password, DeviceToken, out CurrentUser);

            var lookupAlgorithm = new LookupAlgorithm(AppMock.Object);
            var tripContainers = lookupAlgorithm.GetTripLookups(cookie);

            Assert.IsTrue(tripContainers.Any());
            foreach (var tripContainer in tripContainers)
            {
                foreach (var trip in tripContainer.Trips)
                {
                    Console.WriteLine(@"day:" + tripContainer.DayOfWeek + @" trip:" + trip.Key + @" time:" + Enum.GetName(trip.Time.GetType(), trip.Time));
                }
            }

            var kayaks = lookupAlgorithm.GetKayakLookups(cookie);
            Assert.IsTrue(kayaks.Any());

            foreach (Kayak k in kayaks)
            {
                Console.WriteLine(k.Key + @" -> " + k.Name);
            }

            //Get & Save kayak & timing preferences
            var prefs = TimingPrefQuery.GetUserPrefsById(CurrentUser.Id);

            int count = prefs.Count;
            prefs.Add(new LightTimePref { DayOfWeek = "Friday", Time = Timing.Morning, Type = KayakType.Kayak });
            prefs.Add(new LightTimePref { DayOfWeek = "Monday", Time = Timing.Morning, Type = KayakType.SurfSki });
            prefs.Add(new LightTimePref { DayOfWeek = "Tuesday", Time = Timing.Morning, Type = KayakType.Any });

            TimingPrefQuery.SaveUserPrefs(prefs, CurrentUser);
            prefs = TimingPrefQuery.GetUserPrefsById(CurrentUser.Id);
            Assert.AreEqual(count + 3, prefs.Count);

            var kayakPrefs = KayakPrefQuery.GetByUser(CurrentUser);
            count = kayakPrefs.Count;
            kayakPrefs.Add(new KayakPref { Key = "117", UserId = CurrentUser.Id, Weight = 3 });
            kayakPrefs.Add(new KayakPref { Key = "0", UserId = CurrentUser.Id, Weight = 1 });
            kayakPrefs.Add(new KayakPref { Key = "139", UserId = CurrentUser.Id, Weight = 1 });

            KayakPrefQuery.SaveUserKayaks(
                (from k in kayakPrefs select new LightKayakPref { Key = k.Key, Weight = k.Weight }).ToList(), CurrentUser.Id);

            kayakPrefs = KayakPrefQuery.GetByUser(CurrentUser);
            Assert.AreEqual(count + 3, kayakPrefs.Count);

            //Try to book
            BookingAlgorithm = new BookingAlgorithm(AppMock.Object);

            bool success = BookingAlgorithm.DoBookings(cookie, CurrentUser);
            Assert.IsTrue(success);

            var currentUserBookings = BookingQuery.GetBookings(CurrentUser);
            Assert.IsTrue(currentUserBookings.Count == 3);

            success = BookingAlgorithm.CancelBooking(cookie, currentUserBookings.Last());
            Assert.IsTrue(success);

            var bookingLeftToCancel = currentUserBookings.First();

            //Sync booking done manually
            var bookings = BookingAlgorithm.SyncBookings(cookie, CurrentUser, tripContainers);
            Assert.IsTrue(bookings.Any(o => o.TripKey == bookingLeftToCancel.TripKey && o.KayakKey == bookingLeftToCancel.KayakKey));
            success = BookingAlgorithm.CancelBooking(cookie, bookingLeftToCancel);
            Assert.IsTrue(success);

            currentUserBookings = BookingQuery.GetBookingsByState(CurrentUser, BookingState.Cancelled);
            Assert.IsTrue(currentUserBookings.Count == 2);
        }

        [Test]
        public void TaskScenario()
        {
            var lkUpAlgorithm = new LookupAlgorithm(AppMock.Object);
            var loginAlgorithm = new LoginAlgorithm(AppMock.Object);
            var bookingAlgorithm = new BookingAlgorithm(AppMock.Object);

            var algorithmerProviderMock = new Mock<IAlgorithmProvider>();
            var notificationProviderMock = new Mock<INotificationProvider>();

            algorithmerProviderMock.Setup(o => o.Get<ILookupAlgorithm>()).Returns(lkUpAlgorithm);
            algorithmerProviderMock.Setup(o => o.Get<ILoginAlgorithm>()).Returns(loginAlgorithm);
            algorithmerProviderMock.Setup(o => o.Get<IBookingAlgorithm>()).Returns(bookingAlgorithm);

            //Dummy notifier - need to add push
            var bookingNotifier = new BookingNotifier(AppMock.Object);

            notificationProviderMock.Setup(o => o.Get<IBookingNotifier>()).Returns(bookingNotifier);

            AppMock.Setup(o => o.AlgorithmProvider).Returns(algorithmerProviderMock.Object);
            AppMock.Setup(o => o.NotificationProvider).Returns(notificationProviderMock.Object);

            var user = new User { Name = UserName, Password = Password, DeviceToken = "MYTOKEN"};
            AddUserAndPrefs(user);

            var task = new KayakBookingTask(AppMock.Object);

            task.DoWork();

            //Check bookings and cancel
            var bookings = BookingQuery.GetBookings(user);
            Assert.IsTrue(bookings.Any());

            var cookie = loginAlgorithm.Login(user.Name, user.Password, DeviceToken, out user);
            int toCancel = 0;
            foreach (var booking in bookings)
            {
                if (bookingAlgorithm.CancelBooking(cookie, booking))
                    toCancel++;
            }

            Assert.AreEqual(bookings.Count, toCancel);

            //Run the task again
            task.DoWork();

            //See no new bookings
            bookings = BookingQuery.GetBookings(user);
            Assert.IsFalse(bookings.Any());

            //See that they are cancelled
            var cancelledBookings = BookingQuery.GetBookingsByState(user, BookingState.Cancelled);
            Assert.AreEqual(toCancel, cancelledBookings.Count);
        }

        private void AddUserAndPrefs(User user)
        {
            //Add user
            
            var newUser = UserQuery.Save(user);
            TimingPrefQuery.SaveUserPrefs(new[]
                                              {
                                                  new LightTimePref
                                                      {
                                                          DayOfWeek = "Friday",
                                                          Time = Timing.Morning,
                                                          Type = KayakType.SurfSki
                                                      },
                                                  new LightTimePref
                                                      {
                                                          DayOfWeek = "Monday",
                                                          Time = Timing.Morning,
                                                          Type = KayakType.Kayak
                                                      },
                                                  new LightTimePref
                                                      {
                                                          DayOfWeek = "Tuesday",
                                                          Time = Timing.Morning,
                                                          Type = KayakType.Any
                                                      }
                                              }, user);

            var timePrefs = TimingPrefQuery.GetUserPrefsById(newUser.Id);
            Assert.IsTrue(timePrefs.Any());

            KayakPrefQuery.SaveUserKayaks(
                new[]
                    {
                        new LightKayakPref { Key = "0", Type = KayakType.Kayak, Weight = 3 },
                        new LightKayakPref { Key = "69", Type = KayakType.Kayak, Weight = 1 },
                        new LightKayakPref { Key = "70", Type = KayakType.Kayak, Weight = 2 },
                        new LightKayakPref { Key = "137", Type = KayakType.SurfSki, Weight = 2 }
                    }, newUser.Id);

            

        }




    }
}
