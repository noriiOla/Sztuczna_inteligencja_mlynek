using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MlynekV2.Models
{
    public class PionekDoZabrania
    {
        public PionekDoZabrania(string nazwaPionka, Punkt punkt)
        {
            this.nazwaPionka = nazwaPionka;
            this.punkt = punkt;
        }

        public PionekDoZabrania(Punkt punkt)
        {
            this.nazwaPionka = nazwaPionka;
            this.punkt = punkt;
        }

        public string nazwaPionka { get; set; }
        public Punkt punkt { get; set; }
    }
}