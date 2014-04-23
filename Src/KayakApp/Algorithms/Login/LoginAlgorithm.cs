using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using AppKickStart.Common.Logging.Wrapper;
using AppKickStart.Common.Providers;
using IKayak.Persistency.Users;
using IKayak.Schemas.Models;

namespace IKayak.Algorithms.Login
{
    public class LoginAlgorithm : ILoginAlgorithm
    {
        private readonly IAppContext _appContext;

        public LoginAlgorithm(IAppContext appContext)
        {
            _appContext = appContext;
        }

        public Cookie Login(string userName, string password, string deviceToken, out User user)
        {
            var format = string.Format(
                @"username_fieldname=username&password_fieldname=password&redirect=http%3A%2F%2Fwww.drc.org.il%2Fpages%2Fabrit%2Fqiaqim%2Fhzmnot.php&username={0}&password={1}&submit=Login",
                userName, password);

            byte[] buffer =
                Encoding.ASCII.GetBytes(
                    format);

            var webReq = (HttpWebRequest)WebRequest.Create(@"http://www.drc.org.il/account/login.php");
            webReq.UserAgent =
                @"Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.64 Safari/537.11";
            webReq.ContentType = @"application/x-www-form-urlencoded";
            webReq.Accept = @"text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            webReq.Referer =
                @"http://www.drc.org.il/account/login.php?redirect=http://www.drc.org.il/pages/abrit/qiaqim/hzmnot.php";
            var cookieContainer = new CookieContainer();
            webReq.Method = "POST";

            webReq.CookieContainer = cookieContainer;
            Cookie cookie = null;
            webReq.ContentLength = buffer.Length;
            Stream postData = webReq.GetRequestStream();
            postData.Write(buffer, 0, buffer.Length);
            //Closing is always important
            postData.Close();

            //Get the response handle, we have no true response yet
            var webResp = (HttpWebResponse)webReq.GetResponse();

            //read the response 
            Stream webResponse = webResp.GetResponseStream();
            if (webResponse != null)
            {
                var response = new StreamReader(webResponse);
                if (webResp.Cookies.Count > 0)
                {
                    cookie = webResp.Cookies[0];
                }
                else
                {
                    var table = (Hashtable)cookieContainer.GetType().InvokeMember("m_domainTable",
                                                                         BindingFlags.NonPublic |
                                                                         BindingFlags.GetField |
                                                                         BindingFlags.Instance,
                                                                         null,
                                                                         cookieContainer,
                                                                         new object[] { });



                    foreach (var key in table.Keys)
                    {
                        foreach (Cookie c in cookieContainer.GetCookies(new Uri(string.Format("http://{0}/", key))))
                        {
                            Debug.WriteLine(@"Name = {0} ; Value = {1} ; Domain = {2}", c.Name, c.Value,
                                              cookie.Domain);
                            cookie = c;
                            break;
                        }
                    }

                }
                
                string session = cookie.Value;

                var userQuery = _appContext.PersistencyProvider.Get<IUserQuery>();
                
                user = userQuery.Save(new User { Name = userName, Password = password, Session = session, DeviceToken = deviceToken});
                return cookie;
            }

            user = null;
            Logger.Log(LoggingLevel.Error, "Login failed");
            return null;
        }
    }
}