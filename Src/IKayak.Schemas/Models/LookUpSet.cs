using System.Collections.Generic;

namespace IKayak.Schemas.Models
{
    public class LookUpSet
    {
        public List<Kayak> AllKayaks { get; set; }
        public List<TripContainer> AllTripsByDay { get; set; }
    }
}