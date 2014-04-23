using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using AppKickStart.Common.Logging.Wrapper;
using AppKickStart.Common.Providers;
using AppKickStart.Common.Providers.Persistency;
using AppKickStart.Common.Providers.Services;
using IKayak.Algorithms.Login;
using IKayak.Algorithms.Lookup;
using IKayak.Persistency.Bookings;
using IKayak.Persistency.Kayaks;
using IKayak.Persistency.Preferences;
using IKayak.Persistency.Trips;
using IKayak.Persistency.Users;
using IKayak.Schemas.Contracts.Preferences;
using IKayak.Schemas.Models;
using IKayak.Schemas.Models.Exceptions;
using Action = AppKickStart.Schemas.Contracts.Enums.Action;

namespace IKayak.Services.Preferences
{
    public class PreferenceBusinessService :
        BusinessService<PreferenceRequest, PreferenceResponse, PreferenceResponse>, IPreferenceBusinessService
    {
        private IPersistencyProvider _persistencyProvider;

        public PreferenceBusinessService()
        {
        }

        public PreferenceBusinessService(IAppContext appContext)
            : base(appContext)
        {
        }

        #region IPreferenceBusinessService Members

        public override string ServiceName
        {
            get { return "preferences"; }
        }

        public override string Request
        {
            get { return "PreferenceRequest"; }
        }

        public override string Response
        {
            get { return "PreferenceResponse"; }
        }

        #endregion

        public override PreferenceResponse Execute(PreferenceRequest args)
        {
            User user;
            var result = new Set();
            _persistencyProvider = AppContext.PersistencyProvider;
            var prefQuery = _persistencyProvider.Get<IPreferenceQuery>();
            var kayakPrefsQuery = _persistencyProvider.Get<IKayakPrefQuery>();

            var algorithm = AppContext.AlgorithmProvider.Get<ILoginAlgorithm>();
            Cookie cookie;
            if ((cookie = algorithm.Login(args.UserName, args.Password, args.DeviceToken, out user)) == null)
                throw new UserNotFoundBusinessException();

            var kayakQuery = AppContext.PersistencyProvider.Get<IKayakQuery>();
            var userQuery = AppContext.PersistencyProvider.Get<IUserQuery>();
            var bookingQuery = AppContext.PersistencyProvider.Get<IBookingQuery>();

            var allKayaks = kayakQuery.GetAll();

            if (args.Action == Action.Save)
            {
                new Thread(() =>
                {

                    Logger.Log(LoggingLevel.Info, "Action.Save");

                    if (args.Reminder >= 0)
                        userQuery.UpdateReminder(args.UserName, args.Reminder);

                    switch (args.IsFrozen)
                    {
                        case "":
                            break;
                        case "1":
                            userQuery.UpdateStatus(args.UserName, UserStatus.Frozen);
                            foreach (var booking in bookingQuery.GetBookings(user))
                            {
                                bookingQuery.UpdateCancellation(booking, BookingState.PendingCancellation);
                            }
                            break;
                        case "0":
                            foreach (var booking in bookingQuery.GetAll(user))
                            {
                                bookingQuery.Delete(booking);
                            }
                            userQuery.UpdateStatus(args.UserName, UserStatus.Active);
                            break;
                    }
                    user = userQuery.GetUserById(user.Id);

                    if (args.Set != null)
                    {
                        prefQuery.SaveUserPrefs(args.Set.TimePrefs, user);
                        kayakPrefsQuery.SaveUserKayaks(args.Set.KayakPrefs, user.Id);
                    }
                }).Start();
                return new PreferenceResponse { Set = result, IsFrozen = args.IsFrozen == "1", Reminder = args.Reminder };

            }
            else
            {
                var lookupAlgorithm = AppContext.AlgorithmProvider.Get<ILookupAlgorithm>();
                var tripQuery = AppContext.PersistencyProvider.Get<ITripQuery>();
                
                if (!allKayaks.Any())
                {
                    lookupAlgorithm.GetKayakLookups(cookie);
                }

                var allTrips = tripQuery.GetAllTrips();
                if (!allTrips.Any())
                {
                    
                    lookupAlgorithm.GetTripLookups(cookie);
                }


                result.TimePrefs = prefQuery.GetUserPrefsById(user.Id);
                result.KayakPrefs = GetUserKayaksAndWeights(allKayaks, kayakPrefsQuery.GetByUser(user));

                return new PreferenceResponse { Set = result, IsFrozen = user.IsFrozen, Reminder = user.Reminder };
            }
            
        }

        private static IList<LightKayakPref> GetUserKayaksAndWeights(IEnumerable<Kayak> allKayaks, IEnumerable<KayakPref> getUserKayaks)
        {
            IList<LightKayakPref> results = new List<LightKayakPref>();

            foreach(var k in allKayaks)
            {
                short weight = 0;
                Kayak k1 = k;
                var lightKayakPref = getUserKayaks.Where(o=>o.Key == k1.Key).FirstOrDefault();
                if (lightKayakPref != null && lightKayakPref.Weight > 0)
                {
                    weight = lightKayakPref.Weight;
                }
                results.Add(new LightKayakPref{ Key  = k.Key, Name = k.Name, Type = k.Type, Weight = weight });
            }
            return results;
        }

        public override PreferenceResponse BuildResponse(PreferenceResponse response)
        {
            return response;
            
        }
    }
}