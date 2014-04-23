using System;
using System.Collections.Generic;

namespace IKayak.Schemas.Models
{
    public class TripChangeEventArgs : EventArgs
    {
        public List<TripContainer> TripContainers { get; set; }
    }
}
