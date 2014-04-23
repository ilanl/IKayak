using System;
using System.Data.SQLite;
using System.IO;
using AppKickStart.Common.Logging.Wrapper;
using Dapper;
using IKayak.Utils;

namespace IKayak.Persistency
{
    public class SqLiteBaseRepository
    {
        public static string DbFile
        {
            get
            {
                var dbPath = PathMap.Get("SqliteDb", @"IKayak.sqlite");
                return dbPath;
            }
        }

        public static SQLiteConnection SimpleDbConnection()
        {
            if (!File.Exists(DbFile))
            {
                CreateDatabase();
            }

            var cnn = new SQLiteConnection("Data Source=" + DbFile);
            
            return cnn;
        }

        public static void CreateDatabase()
        {
            try
            {

                Logger.Log(LoggingLevel.Debug, "Creating Database... [Start]");

                using (var cnn = new SQLiteConnection("Data Source=" + DbFile))
                {
                    cnn.Open();

                    cnn.Execute(
                        @"create table User
                      (
                         ID                             integer primary key AUTOINCREMENT,
                         Name                           nvarchar(50) not null,
                         Password                       varchar(20) not null,
                         IsFrozen                       bit null,
                         Session                        nvarchar(100) null,
                         Reminder                       integer not null,
                         DeviceToken                    varchar(64) null
                      )");

                    cnn.Execute(
                        @"create table Kayak
                      (
                         ID                             integer primary key AUTOINCREMENT,
                         Key                            varchar(10) not null,
                         Name                           nvarchar(50) not null,
                         Type                           integer not null
                      )");

                    cnn.Execute(
                        @"create table KayakPref
                      (
                         ID                             integer primary key AUTOINCREMENT,
                         Key                            varchar(10) not null,
                         UserId                         integer not null,
                         Weight                         integer not null
                      )");

                    cnn.Execute(
                        @"create table Trip
                      (
                         ID                             integer primary key AUTOINCREMENT,
                         TripKey                        varchar(10) not null,
                         OutingDate                     varchar(50) not null,
                         DayOfWeek                      varchar(20) not null,
                         Time                           integer not null,
                         Hour                           varchar(10) not null
                      )");

                    cnn.Execute(
                        @"create table Preference
                      (
                         ID                               integer primary key AUTOINCREMENT,
                         UserId                           integer not null,
                         Time                             integer not null,
                         DayOfWeek                        nvarchar(20) not null,
                         Type                             integer not null
                      )");

                    cnn.Execute(
                        @"create table Booking
                      (
                         ID                             integer primary key AUTOINCREMENT,
                         UserId                         integer not null,
                         TripKey                        varchar(10) not null,
                         KayakKey                       varchar(10) not null,
                         State                          integer not null,
                         Type                           integer not null,
                         OutingDate                     varchar(50) not null,
                         Day                            nvarchar(20) not null,
                         Time                           varchar(10) not null,
                         KayakName                      nvarchar(50) not null
                      )");

        
                    cnn.Execute(
                        @"create table Forecast
                      (
                         ID                             integer primary key AUTOINCREMENT,
                         Date                           nvarchar(50) not null,
                         Day                            varchar(20) not null,
                         TempC                          varchar(10) not null,
                         WaterTempC                     varchar(10) not null,
                         WaveH                          varchar(10) not null,
                         Hour                           varchar(10) not null,
                         Weather                        nvarchar(50) not null,
                         SwellSecs                      nvarchar(20) not null,
                         WindDir                        varchar(10) not null,
                         WindF                          nvarchar(10) not null
                      )");

                }
            }
            catch(Exception e)
            {
                Logger.Log(LoggingLevel.Fatal, "Could not complete Db creation: ", e);
            }
            finally
            {
                Logger.Log(LoggingLevel.Debug, "Creating Database... [End]");

            }
        }


    }
}