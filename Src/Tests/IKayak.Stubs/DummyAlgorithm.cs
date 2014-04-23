//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using AppKickStart.Common.Providers;
//using AppKickStart.Common.Providers.Persistency;
//using AppKickStart.Schemas.Tools;
//using IKayak.Algorithms;
//using IKayak.Persistency.Bookings;
//using IKayak.Persistency.Kayaks;
//using IKayak.Persistency.Preferences;
//using IKayak.Persistency.Trips;
//using IKayak.Persistency.Users;
//using IKayak.Schemas.Models;
//using IKayak.Utils;

//namespace IKayak.Stubs
//{
//    public class DummyAlgorithm:IAutoBookingAlgorithm
//    {
//        private IKayakQuery _kayakQuery;
//        private ITripQuery _tripQuery;
//        private IUserQuery _userQuery;
//        private IPersistencyProvider _persistencyProvider;
//        private IBookingQuery _bookingQuery;

//        public void Initialize(IAppContext appContext)
//        {

//            File.Delete(PathMap.Get("SqliteDb",@"IKayak.sqlite"));
//            _persistencyProvider = appContext.PersistencyProvider;

//            var dateTime = DateTime.Now;

//            //Fill Db with kayaks
//            _kayakQuery = _persistencyProvider.Get<IKayakQuery>();
//            _kayakQuery.SaveAll(new List<Kayak>() { new Kayak("1", "12 קאיק", KayakType.SurfSki), new Kayak("2", "13", KayakType.Kayak), new Kayak("3", "24", KayakType.Kayak) });

//            //Fill trips
//            _tripQuery = _persistencyProvider.Get<ITripQuery>();
//            var tripContainerSunday = new TripContainer("Sunday");

//            tripContainerSunday.Add("123", TimeTools.ConvertToUnixTimestamp(dateTime.AddDays(3)));
//            tripContainerSunday.Add("456", TimeTools.ConvertToUnixTimestamp(dateTime.AddDays(3)));

//            var tripContainerMonday = new TripContainer("Monday");
//            tripContainerMonday.Add("789", TimeTools.ConvertToUnixTimestamp(dateTime.AddDays(4)));

//            var tripContainerWednesday = new TripContainer("Wednesday");
//            tripContainerWednesday.Add("890", TimeTools.ConvertToUnixTimestamp(dateTime.AddDays(6)));

//            _tripQuery.SaveTrips(new List<TripContainer> { tripContainerSunday, tripContainerMonday, tripContainerWednesday });

//            //Add user
//            _userQuery = _persistencyProvider.Get<IUserQuery>();
//            _userQuery.Save(new User {Name = "רמי", Password = "12345456"});
//        }

//        public void AttemptToBook()
//        {
//            var user = _userQuery.GetUser("רמי");

//            if (user == null)
//                return;

//            var prefQuery = _persistencyProvider.Get<IPreferenceQuery>();
//            var preferences = prefQuery.GetByUser(user);

//            if (preferences == null)
//                return;

//            var bookings = new List<Booking>();
//            foreach(var tripContainer in _tripQuery.GetAllTrips())
//            {
//                TripContainer container = tripContainer;
//                var prefsDay = preferences.Where(o => o.DayOfWeek == container.DayOfWeek);
//                if (prefsDay.Any())
//                {
//                    foreach (var trip in tripContainer.Trips)
//                    {
//                        foreach (var pref in prefsDay)
//                        {
//                            //If rule out continue
                            
//                            //Add booking if not already booked
//                            //var kayakNamePartial =
//                            //     .Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)[0];
                            
//                            //TODO: fix booking
                            
//                            var booking = new Booking
//                                              {
//                                                  KayakKey = 
//                                                      _kayakQuery.GetAll().FirstOrDefault(
//                                                          k => k.Name.Contains("BLA")).Key,
//                                                  TripKey = trip.Key,
//                                                  UserId = user.Id
//                                              };

//                            if (RuleOutMatches(booking))
//                                continue;

//                            bookings.Add(booking);
//                        }
//                    }
//                }
//            }

//            _bookingQuery = _persistencyProvider.Get<IBookingQuery>();
//            _bookingQuery.SaveBookings(bookings);
//        }

//        private bool RuleOutMatches(Booking booking)
//        {
//            //TODO: fix booking

//            //var ruleOuts = _bookingQuery.GetRuleOuts(booking.User.Name);
//            //foreach (var ruleOutBooking in ruleOuts)
//            //{
//            //    if (ruleOutBooking.Trip == booking.Trip)
//            //    {
//            //        return true;
//            //    }
//            //}
//            return false;
//        }

//        public event BookingUpdateHandler BookingUpdate;
//        public event EventHandler TripChanged;
//        public string Login(string user, string pwd)
//        {
//            return "cookie";
//        }

//        public void GetLookups(string userName, string password)
//        {
//            throw new NotImplementedException();
//        }

//        public bool AttemptToCancel(Booking cancellationBooking)
//        {
//            _bookingQuery.UpdateCancellation(cancellationBooking.Id, BookingState.Cancelled);

//            return true;
//        }
//    }
//}
