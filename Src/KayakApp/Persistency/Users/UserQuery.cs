using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using AppKickStart.Common.Logging.Wrapper;
using Dapper;
using IKayak.Schemas.Models;
using IKayak.Schemas.Models.Exceptions;

namespace IKayak.Persistency.Users
{
    public class UserQuery : SqLiteBaseRepository, IUserQuery
    {
        #region IUserQuery Members

        public User GetUser(string userName)
        {
            User user;
            using (SQLiteConnection cnn = SimpleDbConnection())
            {
                cnn.Open();
                user = cnn.Query<User>(
                    @"SELECT Id, Name, Password, Session, IsFrozen, Reminder, DeviceToken
                    FROM User
                    WHERE Name = @userName",
                    new {userName}).FirstOrDefault();
            }

            if (user == null)
                throw new UserNotFoundBusinessException();

            return user;
        }

        public User GetUserById(long userId)
        {
            User user;
            using (SQLiteConnection cnn = SimpleDbConnection())
            {
                cnn.Open();
                user = cnn.Query<User>(
                    @"SELECT Id, Name, Password, Session, IsFrozen,Reminder, DeviceToken
                    FROM User
                    WHERE Id = @userId",
                    new { userId }).FirstOrDefault();
            }

            return user;
        }

        public User Save(User user)
        {
            User userNew;
            try
            {
                if ((userNew = GetUser(user.Name)) != null)
                {
                    using (SQLiteConnection cnn = SimpleDbConnection())
                    {
                        cnn.Open();

                        cnn.Execute(
                            @"UPDATE User
                                  SET Name = @Name,
                                  Password = @Password,
                                  Session = @Session,
                                  DeviceToken = @DeviceToken
                                  WHERE Name = @Name  
                                 ",
                            new { Id = userNew.Id, Name = user.Name, Password = user.Password, Session = user.Session, DeviceToken = user.DeviceToken });
                    }
                }
            }
            catch(UserNotFoundBusinessException)
            {
                using (SQLiteConnection cnn = SimpleDbConnection())
                {
                    cnn.Open();

                    userNew = new User
                    {
                        Name = user.Name,
                        Password = user.Password,
                        DeviceToken = user.DeviceToken,
                        Id = cnn.Query<long>(
                            @"INSERT INTO User 
                    ( Name, Password, Session, IsFrozen, Reminder, DeviceToken) VALUES 
                    ( @Name, @Password, @Session, @IsFrozen, 0, @DeviceToken);
                    select last_insert_rowid()",
                            new { Name = user.Name, Password = user.Password, Session = user.Session, IsFrozen = false, DeviceToken = user.DeviceToken }).First()
                    };
                }
            }

            if (userNew == null)
                throw new UserGeneralBusinessException();

            user = GetUserById(userNew.Id);
            
            return user;
        }

        public IList<User> GetAll()
        {
            List<User> users;

            using (SQLiteConnection cnn = SimpleDbConnection())
            {
                cnn.Open();

                users = cnn.Query<User>(
                    @"SELECT Id, Name, Password, Session, IsFrozen, Reminder, DeviceToken
                    FROM User").ToList();
            }
            return users;
        }

        public void UpdateStatus(string userName,  UserStatus state)
        {
            try
            {
                using (SQLiteConnection cnn = SimpleDbConnection())
                {
                    cnn.Open();

                    cnn.Execute(
                        @"UPDATE User
                                  SET IsFrozen = @IsFrozen
                                  WHERE Name = @Name 
                                 ",
                        new { Name = userName, IsFrozen = state == UserStatus.Frozen });
                }
            }
            catch (UserNotFoundBusinessException ex)
            {
                Logger.Log(LoggingLevel.Error, ex.Message);
                throw;
            }
        }

        public void UpdateReminder(string userName, int reminder)
        {
            try
            {
                using (SQLiteConnection cnn = SimpleDbConnection())
                {
                    cnn.Open();

                    cnn.Execute(
                        @"UPDATE User
                                  SET Reminder = @Reminder
                                  WHERE Name = @Name 
                                 ",
                        new { Name = userName, Reminder = reminder });
                }
            }
            catch (UserNotFoundBusinessException ex)
            {
                Logger.Log(LoggingLevel.Error, ex.Message);
                throw;
            }
        }

        #endregion
    }
}