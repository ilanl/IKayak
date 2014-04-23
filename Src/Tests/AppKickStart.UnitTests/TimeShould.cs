using AppKickStart.Schemas.Tools;
using NUnit.Framework;

namespace AppKickStart.UnitTests
{
    [TestFixture]
    public class TimeShould
    {
        [Test]
        public void TimeConverter()
        {
            //http://www.drc.org.il/modules/reservation/outing.php?o=185&outing_date=1363298400
            var date = TimeTools.ToIsraelTime("1363298400");
            Assert.IsTrue(true);
        }
    }
}
