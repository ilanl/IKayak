using System;
using System.Collections.Generic;
using AppKickStart.Schemas.Tools;

namespace IKayak.Schemas.Models
{
    public class TripContainer
    {
        private readonly string _dayOfWeek;

        public TripContainer(string dayOfWeek)
        {
            _dayOfWeek = dayOfWeek;
        }

        public string DayOfWeek
        {
            get { return _dayOfWeek; }
        }

        public List<Trip> Trips { get; private set; }

        public Trip Add(string key, string outingDate, string hour)
        {
            if (Trips == null)
                Trips = new List<Trip>();
            var trip = new Trip(key, outingDate, hour) { Container = this };

            DateTime localTime = TimeTools.ToIsraelTime(outingDate);
            //Check time using outing date
            if (localTime.Hour == 1)
            {
                //morning
                trip.Time = Timing.Morning;
            }

            Trips.Add(trip);
            return trip;
        }

    }

}