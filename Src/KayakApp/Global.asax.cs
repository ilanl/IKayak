using System;
using AppKickStart.Application;

namespace IKayak
{
    public class Global : System.Web.HttpApplication
    {
        private AppContainer _appContainer;

        protected void Application_Start(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            _appContainer = new AppContainer(this.Application);
            _appContainer.AppStart();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (_appContainer != null)
                _appContainer.Beat();
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (_appContainer!=null)
                _appContainer.AppError(sender, e);
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            if (_appContainer != null)
                _appContainer.AppStop();
        }
    }
}