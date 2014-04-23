using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using AppKickStart.Common.Logging.Wrapper;
using AppKickStart.Common.Providers;
using AppKickStart.Schemas.Tools;
using IKayak.Persistency.Bookings;
using IKayak.Persistency.Kayaks;
using IKayak.Persistency.Trips;
using IKayak.Schemas.Models;

namespace IKayak.Algorithms.Book
{
    public class BookingAlgorithm : IBookingAlgorithm
    {
        private readonly IAppContext _appContext;

        public BookingAlgorithm(IAppContext appContext)
        {
            _appContext = appContext;
        }

        public bool DoBookings(Cookie cookie, User user)
        {
            var bookingQuery = _appContext.PersistencyProvider.Get<IBookingQuery>();
            var userBookings = bookingQuery.GetBookings(user);
            var tripContainers = _appContext.PersistencyProvider.Get<ITripQuery>().GetAllTrips();
            var cancelledBookings = bookingQuery.GetBookingsByState(user, BookingState.Cancelled);
            var pendingBookings = bookingQuery.GetBookingsByState(user, BookingState.PendingCancellation);

            foreach (TripContainer tripContainer in tripContainers)
            {
                foreach (var trip in tripContainer.Trips)
                {
                    //Check if already booked
                    Trip trip1 = trip;
                    if (userBookings.Any(o=>o.TripKey == trip1.Key))
                        continue;

                    if (cancelledBookings.Any(o => o.TripKey == trip1.Key) || pendingBookings.Any(o=>o.TripKey == trip1.Key))
                        continue;

                    //Check preferences for this day
                    var kayakPrefs = _appContext.PersistencyProvider.Get<IKayakPrefQuery>().GetByUserAndTime(user, tripContainer.DayOfWeek, trip.Time);
                    foreach (var kayakPref in kayakPrefs)
                    {
                        try
                        {
                            //Book with RegID, TripKey
                            var kayakId = kayakPref.Key;
                            var webReq =
                            (HttpWebRequest)
                            WebRequest.Create(@"http://www.drc.org.il/modules/reservation/outing.php?o=" + trip.Key +
                                  "&outing_date=" + trip.OutingDate + "&reg=" + kayakId);

                            webReq.UserAgent =
                                @"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.64 Safari/537.11";
                            webReq.ContentType = @"application/x-www-form-urlencoded";
                            webReq.Accept = @"text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                            webReq.Referer =
                                @"http://www.drc.org.il/modules/reservation/outing.php?o=" + trip.Key + "&outing_date=" +
                                trip.OutingDate;

                            webReq.Method = "GET";
                            var cookieContainer = new CookieContainer();
                            cookieContainer.Add(cookie);
                            webReq.CookieContainer = cookieContainer;

                            //Make the best reservation based on preferences
                            try
                            {
                                //Get the response handle, we have no true response yet
                                var webResp = (HttpWebResponse)webReq.GetResponse();

                                //read the response 
                                Stream webResponse = webResp.GetResponseStream();
                                if (webResponse != null)
                                {
                                    var response = new StreamReader(webResponse);
                                    string responseStr = response.ReadToEnd();

                                    if (responseStr.Contains("בטל הרשמה"))
                                    {
                                        string day = TimeTools.ToIsraelTime(trip.OutingDate).DayOfWeek.ToString();
                                        var kayak = _appContext.PersistencyProvider.Get<IKayakQuery>().GetKayakByKey(kayakId);
                                        var booking = new Booking
                                                          {
                                                              KayakKey = kayakId,
                                                              TripKey = trip.Key,
                                                              UserId = user.Id,
                                                              OutingDate = trip.OutingDate,
                                                              Day = day,
                                                              Time =  trip.Hour,
                                                              KayakName = kayak.Name,
                                                              Type = kayak.Type,
                                                              State = BookingState.Active
                                                          };
                                        if (!userBookings.Any(o => o.TripKey == trip1.Key))
                                        {
                                            userBookings.Add(booking);
                                            bookingQuery.SaveBookings(userBookings);
                                            break;
                                        }
                                    }
                                }

                            }
                            catch (WebException ex)
                            {
                                Logger.Log(LoggingLevel.Error, ex.Message, null, ex);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(LoggingLevel.Error, ex.Message, null, ex);
                        }
                    }
                }
            }

            return true;

        }

        public bool CancelBooking(Cookie cookie, Booking booking)
        {
            // modules/reservation/outing.php?o=174&outing_date=1381269600&remove=1 
            var bookingQuery = _appContext.PersistencyProvider.Get<IBookingQuery>();

            try
            {
                //Cancel with TripKey, OutingDate
                
                var webReq =
                    (HttpWebRequest)
                    WebRequest.Create(@"http://www.drc.org.il/modules/reservation/outing.php?o=" + booking.TripKey +
                                      "&outing_date=" + booking.OutingDate + "&remove=1");

                webReq.UserAgent =
                    @"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.64 Safari/537.11";
                webReq.ContentType = @"application/x-www-form-urlencoded";
                webReq.Accept = @"text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                webReq.Referer =
                    @"http://www.drc.org.il/modules/reservation/outing.php?o=" + booking.TripKey + "&outing_date=" +
                    booking.OutingDate;

                webReq.Method = "GET";
                var cookieContainer = new CookieContainer();
                cookieContainer.Add(cookie);
                webReq.CookieContainer = cookieContainer;

                //Make the best reservation based on preferences
                try
                {
                    //Get the response handle, we have no true response yet
                    var webResp = (HttpWebResponse) webReq.GetResponse();

                    //read the response 
                    Stream webResponse = webResp.GetResponseStream();
                    if (webResponse != null)
                    {
                        var response = new StreamReader(webResponse);
                        string responseStr = response.ReadToEnd();
                        var regexCancellation = new Regex(
                            "o=" + booking.TripKey + "&amp;outing_date=" + booking.OutingDate + "&amp;reg=" +
                            booking.KayakKey + "\">הרשם!</a>",
                            RegexOptions.CultureInvariant);

                        if (regexCancellation.IsMatch(responseStr))
                        {
                            bookingQuery.UpdateCancellation(booking, BookingState.Cancelled);
                            return true;
                        }
                    }

                }
                catch (WebException ex)
                {
                    Logger.Log(LoggingLevel.Error, ex.Message, null, ex);
                }
            }
            catch(Exception ex)
            {
                Logger.Log(LoggingLevel.Error, ex.Message, null, ex);
            }

            return false;
        }

        public IList<Booking> SyncBookings(Cookie cookie, User user,  IList<TripContainer> tripContainers)
        {
            var kayakQuery = _appContext.PersistencyProvider.Get<IKayakQuery>();
            var bookingQuery = _appContext.PersistencyProvider.Get<IBookingQuery>();
                                
            IList<Booking> bookings = bookingQuery.GetBookings(user);
            HttpWebRequest webReq;
            HttpWebResponse webResp;
            var dictionary = new StringDictionary();

            var cookieContainer = new CookieContainer();
            try
            {
                webReq =
                    (HttpWebRequest)
                    WebRequest.Create(@"http://www.drc.org.il/modules/reservation/view_outings.php");

                webReq.UserAgent =
                    @"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.64 Safari/537.11";
                webReq.ContentType = @"application/x-www-form-urlencoded";
                webReq.Accept = @"text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                webReq.Referer =
                    @"http://www.drc.org.il/modules/reservation/view_outings.php";

                webReq.Method = "GET";
                cookieContainer.Add(cookie);
                webReq.CookieContainer = cookieContainer;

                //Get the response handle, we have no true response yet
                webResp = (HttpWebResponse)webReq.GetResponse();

                //read the response 
                Stream webResponse = webResp.GetResponseStream();
                if (webResponse != null)
                {
                    var response = new StreamReader(webResponse);
                    string responseStr = response.ReadToEnd();

                    var regex = new Regex(
      "<a href=.outing.php.o=(?<KEY>.*?)&outing_date=(?<DATE>.*?).>(?<HH>\\d{2,2}):(?<MM>\\d{2,2})(.*" +
      "?)>נרשמת!</span>",
    RegexOptions.CultureInvariant
    );
                    var matches = regex.Matches(responseStr);
                    foreach (Match match in matches)
                    {
                        dictionary.Add(match.Groups["KEY"].Value, match.Groups["DATE"].Value +","+match.Groups["HH"].Value+":"+match.Groups["MM"].Value);
                    }
                }

                if (dictionary.Count == 0)
                    return bookings;

                foreach (DictionaryEntry pair in dictionary)
                {
                    try
                    {
                        var tripKey = (string)pair.Key;
                        var value = (string)pair.Value;
                        var strings = value.Split(new []{','}, StringSplitOptions.RemoveEmptyEntries);
                        var outingDate = strings[0];

                        if (bookingQuery.GetBookingByKey(user, tripKey) != null)
                            continue;

                        webReq =
                (HttpWebRequest)
                WebRequest.Create(String.Format(@"http://www.drc.org.il/modules/reservation/outing.php?o={0}&outing_date={1}", tripKey, outingDate));

                        webReq.UserAgent =
                            @"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.64 Safari/537.11";
                        webReq.ContentType = @"application/x-www-form-urlencoded";
                        webReq.Accept = @"text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                        webReq.Referer =
                            @"http://www.drc.org.il/modules/reservation/view_outings.php";

                        webReq.Method = "GET";
                        cookieContainer = new CookieContainer();
                        cookieContainer.Add(cookie);
                        webReq.CookieContainer = cookieContainer;

                        try
                        {
                            //Get the response handle, we have no true response yet
                            webResp = (HttpWebResponse)webReq.GetResponse();

                            //read the response 
                            webResponse = webResp.GetResponseStream();
                            if (webResponse != null)
                            {
                                var response = new StreamReader(webResponse);
                                string responseStr = response.ReadToEnd();

                                var regexBooked = new Regex(
      "<td\\s><b>(?<NAME>.*?)</b>.*<a\\shref=..o=(?<KEY>" + tripKey + ")&amp;ou" +
      "ting_date=(?<DATE>" + outingDate + ")&amp;remove=1\">",
    RegexOptions.Singleline
    | RegexOptions.CultureInvariant
    | RegexOptions.IgnorePatternWhitespace
    );
                                Match bookingMatch = null;
                                if ((bookingMatch = regexBooked.Match(responseStr)).Success)
                                {
                                    
                                    var kayakName = bookingMatch.Groups["NAME"].Value;
                                    Kayak kayak = kayakQuery.GetKayakByName(kayakName);
                                    string day = TimeTools.ToIsraelTime(outingDate).DayOfWeek.ToString();

                                    var booking = new Booking
                                    {
                                        KayakKey = kayak.Key,
                                        TripKey = tripKey,
                                        UserId = user.Id,
                                        OutingDate = outingDate,
                                        Day = day,
                                        Time = strings[1],
                                        KayakName = kayak.Name,
                                        Type = kayak.Type,
                                        State = BookingState.Active
                                    };
                                    bookings.Add(booking);
                                    bookingQuery.SaveBookings(bookings);
                                }
                            }

                        }
                        catch (WebException ex)
                        {
                            Logger.Log(LoggingLevel.Error, "Failed to sync bookings: \n" + ex.Message);

                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(LoggingLevel.Error, "Failed to sync bookings: \n" + ex.Message);
                    }

                }

                
            }
            catch (WebException ex)
            {
                Logger.Log(LoggingLevel.Error, "Failed to sync bookings: \n" + ex.Message);

            }
            return bookings;
        }
    }
}