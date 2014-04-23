using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Transactions;
using Dapper;
using IKayak.Schemas.Models;

namespace IKayak.Persistency.Kayaks
{
    public class KayakQuery : SqLiteBaseRepository, IKayakQuery
    {
        
        public IList<Kayak> GetAll()
        {
            var kayaks = new List<Kayak>();

            using (SQLiteConnection cnn = SimpleDbConnection())
            {
                cnn.Open();

                List<dynamic> dynamic = cnn.Query<dynamic>(
                    @"SELECT Id, Key, Name, Type
                    FROM Kayak")
                    .ToList();
                
                foreach (dynamic o in dynamic)
                {
                    var k = new Kayak(o.Key, o.Name, (KayakType)o.Type){ Id = o.ID};
                    kayaks.Add(k);
                }

            }
            return kayaks;
        }

        public bool SaveAll(IList<Kayak> kayaks)
        {
            using (SQLiteConnection cnn = SimpleDbConnection())
            {
                cnn.Open();

                using (var transactionScope = new TransactionScope())
                {
                    cnn.Query<long>(
                    @"delete from Kayak");

                    foreach (var k in kayaks)
                    {
                        k.Id = cnn.Query<long>(
                            @"INSERT INTO Kayak 
                    ( Key, Name, Type) VALUES 
                    ( @Key, @Name, @Type );
                    select last_insert_rowid()",
                            new
                            {
                                k.Key,
                                k.Name,
                                k.Type
                            }).First();
                    }

                    transactionScope.Complete();
                }
            }
            
            return true;
        }

        public Kayak GetKayakByName(string kayakName)
        {

            Kayak kayak = null;

            using (SQLiteConnection cnn = SimpleDbConnection())
            {
                cnn.Open();

                List<dynamic> dynamic = cnn.Query<dynamic>(
                    @"SELECT Id, Key, Name, Type
                    FROM Kayak WHERE Name = @Name", new {Name = kayakName})
                    .ToList();

                foreach (dynamic o in dynamic)
                {
                    kayak = new Kayak(o.Key, o.Name, (KayakType)o.Type){Id = o.ID};
                    break;
                }

            }
            return kayak;
        }

        public Kayak GetKayakByKey(string kayakKey)
        {
            Kayak kayak = null;

            using (SQLiteConnection cnn = SimpleDbConnection())
            {
                cnn.Open();

                List<dynamic> dynamic = cnn.Query<dynamic>(
                    @"SELECT Id, Key, Name, Type
                    FROM Kayak WHERE Key = @Key", new { Key = kayakKey })
                    .ToList();

                foreach (dynamic o in dynamic)
                {
                    kayak = new Kayak(o.Key, o.Name, (KayakType)o.Type) { Id = o.ID };
                    break;
                }

            }
            return kayak;
        }
    }
}