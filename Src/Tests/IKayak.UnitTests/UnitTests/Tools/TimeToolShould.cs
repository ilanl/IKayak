using System;
using System.Globalization;
using NUnit.Framework;

namespace IKayak.Tests.UnitTests.Tools
{
    [TestFixture]
    public class TimeToolShould
    {
        [Test]
        public void ConvertIntTimeTo12H()
        {
            DateTime parsed = DateTime.ParseExact("06:00 PM", "hh:mm tt",
                                      CultureInfo.InvariantCulture);

            // If you need a string
            var time = parsed.ToString("HHmm", CultureInfo.InvariantCulture);
            int t = int.Parse(time);
            Assert.IsNotNull(time);

        }
    }
}
