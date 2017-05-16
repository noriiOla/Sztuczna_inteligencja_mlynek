using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MlynekV2.Models
{
    public class Komunikat
    {
        public string komunikat { get; set; }
        public Komunikat(string komunikat)
        {
            this.komunikat = komunikat;
        }
    }
}