using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Transactions;
using AppKickStart.Common.Providers;
using Dapper;
using IKayak.Persistency.Users;
using IKayak.Schemas.Models;

namespace IKayak.Persistency.Preferences
{
    public class PreferenceQuery : SqLiteBaseRepository, IPreferenceQuery
    {
        private readonly IAppContext _appContext;
        private IUserQuery _userQuery;

        public PreferenceQuery(IAppContext appContext)
        {
            _appContext = appContext;
        }

        #region IPreferenceQuery Members

        public IList<Preference> GetByUser(User user)
        {
            _userQuery = _appContext.PersistencyProvider.Get<IUserQuery>();

            user = _userQuery.GetUser(user.Name);

            List<Preference> prefs;
            using (SQLiteConnection cnn = SimpleDbConnection())
            {
                cnn.Open();

                prefs = cnn.Query<dynamic>(
                    @"SELECT Id, UserId, DayOfWeek, Type, Time
                    FROM Preference WHERE UserId = @UserId",
                    new { UserId = user.Id })
                    .Select(o => new Preference
                                     {
                                         DayOfWeek = o.DayOfWeek,
                                         Time = (Timing)o.Time,
                                         Type = (KayakType)o.Type,
                                         UserId = o.UserId
                                     }).ToList();
            }

            return prefs;
        }

        public IList<LightTimePref> GetUserPrefsById(long userId)
        {
            List<LightTimePref> userPrefs;

            using (SQLiteConnection cnn = SimpleDbConnection())
            {
                cnn.Open();

                userPrefs = cnn.Query<dynamic>(
                        @"SELECT DayOfWeek, Time, Type
                    FROM Preference AS P WHERE UserId = @UserId", new { UserId = userId })
                    .Select(o => new LightTimePref { DayOfWeek = o.DayOfWeek, Time = (Timing)o.Time, Type = (KayakType)o.Type }).ToList();

            }
            return userPrefs;
        }

        public bool SaveUserPrefs(IList<LightTimePref> preferences, User u)
        {
            _userQuery = _appContext.PersistencyProvider.Get<IUserQuery>();


            var actualUser = _userQuery.GetUser(u.Name) ?? new User
                                                {
                                                    Id = _userQuery.Save(new User { Name = u.Name, Password = u.Password }).Id
                                                };

            using (SQLiteConnection cnn = SimpleDbConnection())
            {
                cnn.Open();

                using (var transactionScope = new TransactionScope())
                {
                    cnn.Query<long>(
                        @"delete from Preference where UserId = @id",
                        new { actualUser.Id });

                    foreach (var p in preferences)
                    {
                        cnn.Query<long>(
                            @"INSERT INTO Preference 
                    ( UserId, Time, DayOfWeek, Type) VALUES 
                    ( @UserId, @Time, @DayOfWeek, @Type );
                    select last_insert_rowid()", new { UserId = actualUser.Id, p.Time, p.DayOfWeek, p.Type }
                            );
                    }

                    transactionScope.Complete();
                }
            }

            return true;
        }

        #endregion
    }
}