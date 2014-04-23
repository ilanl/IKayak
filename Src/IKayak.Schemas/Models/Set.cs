using System.Collections.Generic;

namespace IKayak.Schemas.Models
{
    public class Set
    {
        public IList<LightTimePref> TimePrefs { get; set; }
        public IList<LightKayakPref> KayakPrefs { get; set; }         
    }
}