//using System;
//using System.Collections;
//using System.Threading;
//using AppKickStart.Common.Templates;
using System;
using log4net.Config;
using NUnit.Framework;
//using System.Collections.Generic;

namespace IKayak.UnitTests
{
    [TestFixture]
    public class Should
    {
        [SetUp]
        public void SetUp()
        {
            XmlConfigurator.Configure();
        }

        [Test]
        public void RunBooker()
        {
            for (var i = 0; i < 10; i++)
            {

                var b = new AutoBookingAlgorithm();
                b.BookingUpdate += (o) =>
                                      {
                                          Console.WriteLine(o);
                                      };
                b.AttemptToBook();


            }
            Assert.IsTrue(true);

        }

        //[Test]
        //public void TemplateEmail()
        //{
        //    var templateView = new TemplateView(@"resources\templates\LastBookingSummary.vm");
            
        //    IDictionary context = new Hashtable
        //                              {
        //                                {"containers", new List<BookingContainer>() },
        //                                {"domain", "localhost/IKayak.Web"}
        //                              };

        //    string result = templateView.ProcessText(context);
        //}

        //[Test]
        //public void Cancel()
        //{
        //    //http://www.drc.org.il/modules/reservation/outing.php?o=165&outing_date=1360620000

        //    var booker = new Booker();
        //    booker.LoadPreferences();
        //    var isSuccess = booker.AttemptToCancel(new CancelledBooking("165", "1360620000"));

        //    Assert.IsTrue(isSuccess);
        //}

        //[Test]
        //public void RunService()
        //{
        //    var service = new EngineWrapper();
        //    service.OnStart(null);
        //    Thread.Sleep(60000);
        //    Assert.IsTrue(true);
        //}
    }
}