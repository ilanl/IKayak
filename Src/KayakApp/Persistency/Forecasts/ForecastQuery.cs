using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Transactions;
using AppKickStart.Common.Providers;
using AppKickStart.Schemas.Tools;
using Dapper;
using IKayak.Schemas.Models;


namespace IKayak.Persistency.Forecasts
{
    public class ForecastQuery : SqLiteBaseRepository, IForecastQuery
    {
        private readonly IAppContext _appContext;

        public ForecastQuery(IAppContext appContext)
        {
            _appContext = appContext;
        }

        /*
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
         */
        public IList<Forecast> GetAll()
        {
            var forecasts = new List<Forecast>();

            using (SQLiteConnection cnn = SimpleDbConnection())
            {
                cnn.Open();

                List<dynamic> dynamic = cnn.Query<dynamic>(
                    @"SELECT Id,Date,Day,TempC, WaterTempC,WaveH,Hour,Weather, SwellSecs,WindDir,WindF
                    FROM Forecast").ToList();

                forecasts.AddRange(
                    dynamic.Select(o =>
                    {
                        var forecast = new Forecast { Date = o.Date, Day = o.Day , Hour = o.Hour, SwellSecs = o.SwellSecs, TempC = o.TempC, WaterTempC = o.WaterTempC, WaveH = o.WaveH, Weather = o.Weather, WindDir = o.WindDir, WindF = o.WindF };
                        return forecast;
                    }));
            }

            return forecasts;
        }

        public IEnumerable<Forecast> GetNextDays(int numberOfDays, int startHour, int endHour)
        {
            IList<Forecast> forecasts = GetAll();
            int nDays = 0;
            foreach (Forecast forecast in forecasts)
            {
                int hour24 = GetHour(forecast);
                if (nDays <= numberOfDays && hour24 <= endHour && hour24 >= startHour)
                {
                    nDays++;
                    yield return forecast;
                }
            }
        }

        private static int GetHour(Forecast forecast)
        {
            DateTime parsed = DateTime.ParseExact(forecast.Hour, "hh:mm tt",
                                      CultureInfo.InvariantCulture);

            // If you need a string
            var time = parsed.ToString("HHmm", CultureInfo.InvariantCulture);
            return int.Parse(time);
        }

        public bool SaveAll(IList<Forecast> forecasts)
        {
            if (!forecasts.Any())
                return true;

            using (SQLiteConnection cnn = SimpleDbConnection())
            {
                cnn.Open();
                using (var transactionScope = new TransactionScope())
                {
                    cnn.Query<long>(
                        @"delete from Forecast",
                        new {  });

                    foreach (var f in forecasts)
                    {
                        long id = cnn.Query<long>(
                            @"INSERT INTO Forecast 
                    ( Date,Day,TempC, WaterTempC,WaveH,Hour,Weather, SwellSecs,WindDir,WindF) VALUES 
                    ( @Date,@Day,@TempC, @WaterTempC,@WaveH,@Hour,@Weather, @SwellSecs,@WindDir,@WindF);
                    select last_insert_rowid()",
                            new
                            {
                                Date = f.Date, Day = f.Day, TempC = f.TempC, WaterTempC = f.WaterTempC, WaveH = f.WaveH, Hour = f.Hour, f.Weather, f.SwellSecs, f.WindDir, f.WindF
                            }).First();
                    }

                    transactionScope.Complete();
                }
                
                return true;
            }
        }

        public void CleanUpOlderDays(DateTime date)
        {
            IList<Forecast> forecasts = GetAll();

            foreach (Forecast forecast in forecasts)
            {
                if ((date - TimeTools.ToIsraelTime(forecast.Date)).Days <= 1)
                    continue;

                using (SQLiteConnection cnn = SimpleDbConnection())
                {
                    cnn.Open();
                    cnn.Query<long>(
                        @"delete from Forecast where Date = @date",
                        new { forecast.Date });
                }
            }
        }
    }
}