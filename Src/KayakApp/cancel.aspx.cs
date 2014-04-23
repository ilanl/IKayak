using System;
using AppKickStart.Common.Logging.Wrapper;
using AppKickStart.Common.Providers;
using IKayak.Schemas.Contracts.Bookings;
using IKayak.Services.Bookings;


namespace IKayak
{
    public partial class cancel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //process request
                string userName = @"%D7%90%D7%99%D7%9C%D7%9F%20%D7%9C";
                string pwd = Request["p"];
                string tripKey = Request["t"];

                var appContext = (IAppContext) this.Application["Context"];
                var service = appContext.BusinessServiceProvider.Get<IBookingBusinessService>();
                var method = new Action(() =>
                                            {
// ReSharper disable ConvertToLambdaExpression
                                                service.Execute(new BookingRequest
                                                                    {
                                                                        Action =
                                                                            AppKickStart.Schemas.Contracts.Enums.Action.
                                                                            Cancel,
                                                                        UserName = userName,
                                                                        Password = pwd,
                                                                        Keys = new[] {tripKey}
                                                                    });
// ReSharper restore ConvertToLambdaExpression
                                            });

                method.BeginInvoke((ar) =>
                                       {
// ReSharper disable ConvertToLambdaExpression
                                           Logger.Log(LoggingLevel.Info, @"processing cancellation request");
// ReSharper restore ConvertToLambdaExpression
                                       }, null);



            }
        }
    }
}