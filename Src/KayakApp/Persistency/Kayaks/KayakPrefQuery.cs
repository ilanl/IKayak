using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Transactions;
using AppKickStart.Common.Providers;
using Dapper;
using IKayak.Schemas.Models;

namespace IKayak.Persistency.Kayaks
{
    public class KayakPrefQuery : SqLiteBaseRepository, IKayakPrefQuery
    {
        private readonly IAppContext _appContext;
        
        public KayakPrefQuery(IAppContext appContext)
        {
            _appContext = appContext;
        }

        public List<KayakPref> GetAll()
        {
            List<KayakPref> kayakPrefs;

            using (SQLiteConnection cnn = SimpleDbConnection())
            {
                cnn.Open();

                kayakPrefs = cnn.Query<KayakPref>(
                    @"SELECT Id, Key, UserId, Weight
                    FROM KayakPref").ToList();
            }
            return kayakPrefs;
        }

        public IList<KayakPref> GetByUser(User user)
        {
            List<KayakPref> kayakPrefs;

            using (SQLiteConnection cnn = SimpleDbConnection())
            {
                cnn.Open();

                kayakPrefs = cnn.Query<KayakPref>(
                    @"SELECT Id, Key, UserId, Weight
                    FROM KayakPref WHERE UserId = @UserId", new { UserId = user.Id }).ToList();
            }
            return kayakPrefs;
        }

        public bool SaveUserKayaks(IList<LightKayakPref> kayakPrefs, long userId)
        {
            if (!kayakPrefs.Any())
                return false;
            
            int i = 0;
            using (SQLiteConnection cnn = SimpleDbConnection())
            {
                cnn.Open();
                    
                using (var transactionScope = new TransactionScope())
                {
                    cnn.Query<long>(
                        @"delete from KayakPref WHERE UserId = @UserId", new { UserId = userId });

                    foreach (var k in kayakPrefs)
                    {
                        if (k.Weight == 0)
                            continue;

                        long kp = cnn.Query<long>(
                            @"INSERT INTO KayakPref 
                    ( Key, UserId, Weight) VALUES 
                    ( @Key, @UserId, @Weight );
                    select last_insert_rowid()",
                            new
                                {
                                    k.Key,
                                    UserId = userId,
                                    k.Weight
                                }).First();
                        i++;
                    }
                    
                    transactionScope.Complete();
                    
                }
            }
            
            return i > 0;
        }

        public IList<KayakPref> GetByUserAndTime(User user, string dayOfWeek, Timing time)
        {
            List<KayakPref> kayakPrefs;

            using (SQLiteConnection cnn = SimpleDbConnection())
            {
                cnn.Open();

                kayakPrefs = cnn.Query<KayakPref>(
                    @"SELECT KP.Id, KP.Key, KP.UserId, KP.Weight
                    FROM KayakPref KP
                    INNER JOIN Kayak K ON KP.Key = K.Key
                    INNER JOIN Preference P ON P.UserId = KP.UserId
                    WHERE KP.UserId = @UserId AND P.DayOfWeek = @DayOfWeek AND P.Time = @Time AND (P.Type = K.Type OR P.Type == 3) ORDER BY KP.Weight, K.Type ASC", 
                                    new { UserId = user.Id, DayOfWeek = dayOfWeek, Time = time }).ToList();
            }
            return kayakPrefs;
        }
    }
}