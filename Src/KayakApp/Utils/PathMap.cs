using System;
using System.Configuration;
using System.IO;
using System.Web;
using AppKickStart.Common.Logging.Wrapper;

namespace IKayak.Utils
{
    public class PathMap
    {
        public const string BookingsToRemovePath = "toremove";

        public static string Get(string path)
        {
            try
            {
                string pathToReturn = HttpContext.Current != null
                                          ? HttpContext.Current.Server.MapPath(path)
                                          : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
                
                Logger.Log(LoggingLevel.Debug, "Path:"+ pathToReturn);

                return pathToReturn;
            }
            catch (HttpException)
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }
        }

        public static string Get(string dbKey, string dbFileName)
        {
            var setting = ConfigurationManager.AppSettings[dbKey];
            if (String.IsNullOrEmpty(setting))
                return Get(dbFileName);
            return setting;
        }
    }
}