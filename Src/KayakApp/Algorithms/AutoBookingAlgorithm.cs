//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Web;
//using AppKickStart.Common.Logging.Wrapper;
//using AppKickStart.Common.Providers;
//using AppKickStart.Common.Providers.Persistency;
//using AppKickStart.Schemas.Tools;
//using HtmlAgilityPack;
//using IKayak.Algorithms.Login;
//using IKayak.Persistency.Bookings;
//using IKayak.Persistency.Kayaks;
//using IKayak.Persistency.Preferences;
//using IKayak.Persistency.Trips;
//using IKayak.Persistency.Users;
//using IKayak.Schemas.Models;
//using IKayak.Schemas.Models.Exceptions;

//namespace IKayak.Algorithms
//{
//    public class AutoBookingAlgorithm : IAutoBookingAlgorithm
//    {
//        private readonly IAppContext _appContext;
//        private List<TripContainer> _allTripContainers;
//        private IBookingQuery _bookingQuery;
//        private IKayakQuery _kayakQuery;
//        private IList<LightTimePref> _listOfPrefs;

//        private string _password;
//        private IPreferenceQuery _preferenceQuery;
//        private ITripQuery _tripQuery;
//        private IUserQuery _userQuery;

//        private Cookie _sessionCookie;
//        private User _user;
//        private string _userName;
//        private IKayakPrefQuery _kayakPrefQuery;
//        private User _currentUser;
//        private IList<KayakPref> _kayakPrefs;

//        #region IAutoBookingAlgorithm Members

//        public event BookingUpdateHandler BookingUpdate;
//        public event EventHandler TripChanged;


//        public AutoBookingAlgorithm(IAppContext appContext)
//        {
//            _appContext = appContext;
//            IPersistencyProvider persistencyProvider = appContext.PersistencyProvider;
//            _preferenceQuery = persistencyProvider.Get<IPreferenceQuery>();
//            _bookingQuery = persistencyProvider.Get<IBookingQuery>();
//            _kayakQuery = persistencyProvider.Get<IKayakQuery>();
//            _userQuery = persistencyProvider.Get<IUserQuery>();
//            _tripQuery = persistencyProvider.Get<ITripQuery>();
//            _kayakPrefQuery = persistencyProvider.Get<IKayakPrefQuery>();

//            //PreferenceSet preferenceSet = _preferenceQuery.GetByUserName("אילן ל");
//            //if (preferenceSet == null)
//            //    return;

//            //_listOfPrefs = preferenceSet.Preferences;
//            //_userName = HttpUtility.UrlEncode(preferenceSet.User.Name);
//            //_password = preferenceSet.User.Password;
//            //_user = preferenceSet.User;

//            ////cancel all trips that need to be cancelled
//            //_allTripRulesOut = new List<RuleOutBooking>();

//            //if (File.Exists(PathMap.Get(PathMap.BookingsToRemovePath)))
//            //{
//            //    string[] toCancelLines = File.ReadAllLines(PathMap.Get(PathMap.BookingsToRemovePath));

//            //    foreach (string tripToCancelLine in toCancelLines)
//            //    {
//            //        try
//            //        {
//            //            var ruleOutBooking = new RuleOutBooking(tripToCancelLine);
//            //            ruleOutBooking.User = _user;

//            //            if (_allTripRulesOut.Any(o => o.Trip.OutingDate == ruleOutBooking.Trip.OutingDate))
//            //                continue;

//            //            if (AttemptToCancel(ruleOutBooking))
//            //            {
//            //                _allTripRulesOut.Add(ruleOutBooking);

//            //                _bookingQuery.AddRuleOut(ruleOutBooking);

//            //                BookingUpdate(ruleOutBooking);
//            //            }
//            //        }
//            //        catch (TripExpiredBusinessException)
//            //        {
//            //            continue;
//            //        }
//            //    }

//            //    File.Delete(PathMap.Get(PathMap.BookingsToRemovePath));
//            //}

//            //_allTripRulesOut = _bookingQuery.GetRuleOuts(_user.Name);
//        }

//        public bool AttemptToCancel(Booking cancellationBooking)
//        {
//            //Login(_userName, _password);

//            //var webReq =
//            //    (HttpWebRequest)
//            //    WebRequest.Create(
//            //        String.Format(
//            //            @"http://www.drc.org.il/modules/reservation/outing.php?o={0}&outing_date={1}&remove=1",
//            //            cancellationBooking.Trip.Key, cancellationBooking.Trip.OutingDate));
//            //webReq.KeepAlive = true;

//            //webReq.UserAgent =
//            //    @"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.17 (KHTML, like Gecko) Chrome/24.0.1312.57 Safari/537.17";


//            //webReq.Accept = @"text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
//            //webReq.Referer = String.Format(
//            //    @"http://www.drc.org.il/modules/reservation/outing.php?o={0}&outing_date={1}",
//            //    cancellationBooking.Trip.Key, cancellationBooking.Trip.OutingDate);
//            //webReq.Method = "GET";

//            //webReq.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
//            //webReq.Headers.Add("Accept-Language", "en-US,en;q=0.8");
//            //webReq.Headers.Add("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.3");

//            //var cookieContainer = new CookieContainer();
//            //cookieContainer.Add(_sessionCookie);

//            //webReq.CookieContainer = cookieContainer;
//            //var webResp = (HttpWebResponse)webReq.GetResponse();

//            //if (webResp.StatusCode == HttpStatusCode.OK)
//            //    return true;

//            ////read the response 
//            //Stream webResponse = webResp.GetResponseStream();
//            //if (webResponse != null)
//            //{
//            //    var response = new StreamReader(webResponse);
//            //    string responseStr = response.ReadToEnd();
//            //    //Logger.Log(LoggingLevel.Debug, responseStr);
//            //}
//            return false;
//        }

//        public void AttemptToBook()
//        {
//            foreach (var user in _userQuery.GetAll())
//            {

//                _currentUser = user;
//                _kayakPrefs = _kayakPrefQuery.GetByUser(_currentUser);

//                _sessionCookie = new LoginAlgorithm(_appContext).Login(user.Name, user.Password);

//                _listOfPrefs = _preferenceQuery.GetUserPrefs(user);

                

//                //Publish trips
//                if (TripChanged != null)
//                    TripChanged(this, new TripChangeEventArgs { TripContainers = _allTripContainers });

//                foreach (TripContainer tripContainer in _allTripContainers)
//                {
//                    Logger.Log(LoggingLevel.Debug, tripContainer.DayOfWeek);
//                    foreach (Trip trip in tripContainer.Trips)
//                    {
//                        TripContainer container = tripContainer;

//                        var trip1 = trip;
//                        if (!_listOfPrefs.Any(o => o.DayOfWeek == container.DayOfWeek && o.Time == trip1.Time))
//                            continue;

//                        InspectKayakList(container, trip, _listOfPrefs, _kayakPrefs);
//                    }
//                }
//            }
//        }

//        #endregion

//        // ReSharper disable UnusedParameter.Local
//        private void InspectKayakList(TripContainer container, Trip trip, IList<LightTimePref> preferences, IList<KayakPref> kayakPrefs // ReSharper restore UnusedParameter.Local
//            )
//        {

//            //Check the participants and kayaks

//            /*
//             GET http://www.drc.org.il/modules/reservation/outing.php?o=167&outing_date=1357423200 HTTP/1.1
//Host: www.drc.org.il
//Connection: keep-alive
//User-Agent: Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.97 Safari/537.11
//Accept: text/html,application/xhtml+xml,application/xml;q=0.9,;q=0.8
//Referer: http://www.drc.org.il/modules/reservation/view_outings.php
//Accept-Encoding: gzip,deflate,sdch
//Accept-Language: en-US,en;q=0.8
//Accept-Charset: ISO-8859-1,utf-8;q=0.7,*;q=0.3
//Cookie: drc_session_id=fa743955eb5aec499fba0416db620902; __utma=241674583.304110194.1344970777.1357319437.1357392895.30; __utmb=241674583.3.10.1357392895; __utmc=241674583; __utmz=241674583.1345699971.2.2.utmcsr=google|utmccn=(organic)|utmcmd=organic|utmctr=http://drc.org.il/
//             */

            
//        }

//        // ReSharper disable MemberCanBeMadeStatic.Local
//        private KayakPref MatchKayakInDayAndPreferences(IList<KayakPref> kayakPrefs, string kayak)
//        {
//            var kayakMatch = kayakPrefs.Where(o => o.Key == kayak).FirstOrDefault();
//            if (kayakMatch != null)
//            {
//                return kayakMatch;
//            }
//            return null;
//        }


//        private Booking BookKayak(string kayakName, string kayakId, Trip trip)
//        {
            
//        }


//    }




//}