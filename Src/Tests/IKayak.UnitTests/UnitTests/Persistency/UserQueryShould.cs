using System.IO;
using IKayak.Persistency;
using IKayak.Persistency.Users;
using IKayak.Schemas.Models;
using NUnit.Framework;

namespace IKayak.Tests.UnitTests.Persistency
{
    [TestFixture]
    public class UserQueryShould
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            //Clean old sqlite file
            if (File.Exists(SqLiteBaseRepository.DbFile))
                File.Delete(SqLiteBaseRepository.DbFile);

            SqLiteBaseRepository.CreateDatabase();
        }

        #endregion

        [Test]
        public void SaveUser()
        {
            var query = new UserQuery();
            var userName = "אילן ל";
            var expected = new User {Name = userName, Password = "32371", DeviceToken = "MYTOKEN"};

            var actualUser = query.Save(expected);

            User actual = query.GetUser(expected.Name);
            Assert.AreEqual(false, actual.IsFrozen);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Password, actual.Password);
            Assert.GreaterOrEqual(actualUser.Id, 1);

            query.UpdateStatus(userName,UserStatus.Frozen);
            actual = query.GetUser(expected.Name);
            Assert.AreEqual(true, actual.IsFrozen);
        }
    }
}