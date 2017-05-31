
namespace MlynekV2.Models
{
    public class Pionek
    {
        public Pionek(string idPionka, Punkt miejscePolozenia)
        {
            this.idPionka = idPionka;
            this.miejscePolozenia = miejscePolozenia;
        }

        public string idPionka { get; set; }
        public int kolor { get; set; }
        public bool zbity { get; set; }
        public Punkt miejscePolozenia { get; set; }
    }
}