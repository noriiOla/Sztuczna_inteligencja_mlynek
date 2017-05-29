
using System.Collections.Generic;

namespace MlynekV2.Models
{
    public class Gracz
    {
        public Gracz(int kolor, int typ, int iloscPkt, int iloscPionkow)
        {
            this.kolor = kolor;
            this.typ = typ;
            this.iloscPkt = iloscPkt;
            this.iloscPionkow = iloscPionkow;
            this.iloscPionkowNieWylozonych = 9;
            this.nieUzytePionki = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        }

        public int kolor { get; set; }
        public int typ { get;  set; }
        public int iloscPkt { get; set; }
        public int iloscPionkow { get; set; }
        public int iloscPionkowNieWylozonych { get; set; }
        public List<int> nieUzytePionki { get; set; }
    }
}