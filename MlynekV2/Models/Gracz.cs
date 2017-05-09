
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
        }

        int kolor { get; set; }
        int typ { get;  set; }
        int iloscPkt { get; set; }
        int iloscPionkow { get; set; }
    }
}