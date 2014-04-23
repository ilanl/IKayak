using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using AppKickStart.Common.Logging.Wrapper;
using AppKickStart.Common.Providers;
using AppKickStart.Schemas.Tools;
using HtmlAgilityPack;
using IKayak.Persistency.Kayaks;
using IKayak.Persistency.Trips;
using IKayak.Schemas.Models;

namespace IKayak.Algorithms.Lookup
{
    public class LookupAlgorithm: ILookupAlgorithm
    {

        private readonly IAppContext _appContext;

        public LookupAlgorithm(IAppContext appContext)
        {
            _appContext = appContext;
        }

        public IList<TripContainer> GetTripLookups(Cookie cookie)
        {
            var webReq =
                    (HttpWebRequest)WebRequest.Create(@"http://www.drc.org.il/modules/reservation/view_outings.php");
            webReq.UserAgent =
                @"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.64 Safari/537.11";
            webReq.ContentType = @"application/x-www-form-urlencoded";
            webReq.Accept = @"text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            webReq.Referer = @"http://www.drc.org.il/pages/abrit/qiaqim/hzmnot.php";
            webReq.Method = "GET";
            var cookieContainer = new CookieContainer();
            cookieContainer.Add(cookie);
            webReq.CookieContainer = cookieContainer;
            
            //Get the response handle, we have no true response yet
            var webResp = (HttpWebResponse)webReq.GetResponse();

            //read the response 
            Stream webResponse = webResp.GetResponseStream();
            var allTripContainers = new List<TripContainer>();
            if (webResponse != null)
            {
                var response = new StreamReader(webResponse);
                string responseStr = response.ReadToEnd();

                var regTrip = new Regex(@"outing\.php\?o=(?<REG>\d*?)&outing_date=(?<DATE>\d+)\""\>" +
                                                                                                    @"(?<TIME>(?<HH>\d{2,2}):(?<MM>\d{2,2}).*?)\<",
                                        RegexOptions.IgnoreCase | RegexOptions.Compiled |
                                        RegexOptions.ExplicitCapture);

                foreach (Match matchTrip in regTrip.Matches(responseStr))
                {
                    if (matchTrip.Success)
                    {
                        string outdate = matchTrip.Groups["DATE"].Value;
                        DateTime localTime = TimeTools.ToIsraelTime(outdate);
                        string dayOfWeek = localTime.DayOfWeek.ToString();
                        if (!allTripContainers.Any(o => o.DayOfWeek == dayOfWeek))
                            allTripContainers.Add(new TripContainer(dayOfWeek));

                        string tripId = matchTrip.Groups["REG"].Value;
                        string timeOfDay = matchTrip.Groups["TIME"].Value;
                        int hhOfDay = int.Parse(matchTrip.Groups["HH"].Value);
                        
                        TripContainer tripContainer = allTripContainers.Where(o => o.DayOfWeek == dayOfWeek).First();
                        var trip = tripContainer.Add(tripId, outdate, String.Format("{0}:{1}", matchTrip.Groups["HH"].Value, matchTrip.Groups["MM"].Value));

                        if (timeOfDay.Contains("בוקר") || hhOfDay < 12)
                        {
                            trip.Time = Timing.Morning;
                        }
                        else if (timeOfDay.Contains("אחר") || (hhOfDay >= 12 && hhOfDay < 16))
                        {
                            trip.Time = Timing.Afternoon;
                        }
                        else if (timeOfDay.Contains("ערב") || (hhOfDay >= 16))
                        {
                            trip.Time = Timing.Late;
                        }
                    }
                }

                var tripQuery = _appContext.PersistencyProvider.Get<ITripQuery>();
                tripQuery.SaveTrips(allTripContainers);

            }
            return allTripContainers;

        }

        public IList<Kayak> GetKayakLookups(Cookie cookie)
        {
            var allKayaks = new List<Kayak>();
            
            var tripKey = "150";
            var outingDate = "1357423200";
            
            var webReq =
                (HttpWebRequest)
                WebRequest.Create(String.Format(@"http://www.drc.org.il/modules/reservation/outing.php?o={0}&outing_date={1}", tripKey, outingDate ));

            webReq.UserAgent =
                @"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.64 Safari/537.11";
            webReq.ContentType = @"application/x-www-form-urlencoded";
            webReq.Accept = @"text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            webReq.Referer =
                @"http://www.drc.org.il/modules/reservation/view_outings.php";

            webReq.Method = "GET";
            var cookieContainer = new CookieContainer();
            cookieContainer.Add(cookie);
            webReq.CookieContainer = cookieContainer;

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

                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(responseStr);
                    HtmlNode table = htmlDoc.GetElementbyId("outings");

                    //Get all nodes
                    foreach (HtmlNode row in table.SelectNodes("tr"))
                    {
                        if (row.Element("td") == null)
                            continue;

                        if (!row.Elements("td").Any() ||
                            row.Elements("td").ElementAtOrDefault(1).InnerText.Contains("לא זמין"))
                            continue;

                        if (row.Elements("td").ElementAtOrDefault(1).InnerText.Contains("תפוס") ||
                            row.Elements("td").ElementAtOrDefault(1).Element("a") == null)
                            continue;

                        string kayakEntry = row.Element("td").Element("b").InnerText;
                        string target = row.Elements("td").ElementAtOrDefault(1).Element("a").Attributes["href"].Value;
                        

                        var kayakReg = new Regex(@"o=&amp;outing_date=(?<DATE>.*?)&amp;reg=(?<REG>\d+)",
                                                   RegexOptions.IgnoreCase |
                                                   RegexOptions.Compiled |
                                                   RegexOptions.ExplicitCapture);

                        Match matchKayak = kayakReg.Match(target);
                        if (matchKayak.Success && (!allKayaks.Any(o => o.Name.Equals(kayakEntry))))
                        {
                            allKayaks.Add(new Kayak(matchKayak.Groups["REG"].Value, kayakEntry, kayakEntry.Contains("סקי") ? KayakType.SurfSki : KayakType.Kayak) );
                        }
                    }
                    var kayakQuery = _appContext.PersistencyProvider.Get<IKayakQuery>();
                    kayakQuery.SaveAll(allKayaks);
                }

            }
            catch (WebException ex)
            {
                Logger.Log( LoggingLevel.Error, "Failed to get kayak list: \n" +ex.Message);
            }

            return allKayaks;
        }


    }
}