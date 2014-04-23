using System;
using System.Web.UI;
using AppKickStart.Common.Logging.Wrapper;
using AppKickStart.Schemas.Tools;

namespace IKayak
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TimeZone localZone = TimeZone.CurrentTimeZone;
            DateTime currentDate = DateTime.Now;
            
            TimeSpan currentOffset =
                localZone.GetUtcOffset(currentDate);

            txtDateTime.InnerText =
                String.Format("Local: " + DateTime.Now.ToLocalTime() + ", Utc: " + DateTime.Now.ToUniversalTime() +
                              ", Mine:" + TimeTools.GetIsraelTime() + ",Offset:" + currentOffset.TotalHours);

            Logger.Log(LoggingLevel.Info, "Default page test");
        }
    }
}