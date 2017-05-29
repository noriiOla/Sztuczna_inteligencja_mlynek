using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MlynekV2.Models
{
    public class Ruch
    {
        public Ruch(string idPionka, Punkt miejscePionkaDoUsniecia, List<Punkt> miejscaNaKtoreMozeSieRuszyc)
        {
            this.idPionka = idPionka;
            this.miejscaNaKtoreMozeSieRuszyc = miejscaNaKtoreMozeSieRuszyc;
        }

        public string idPionka { get; set; }
        public Punkt miejscePionkaDoUsniecia { get; set; }
        public List<Punkt> miejscaNaKtoreMozeSieRuszyc { get; set; }
    }
}