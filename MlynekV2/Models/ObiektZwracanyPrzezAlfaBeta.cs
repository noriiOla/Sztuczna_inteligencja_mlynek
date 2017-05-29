
namespace MlynekV2.Models
{
    public class ObiektZwracanyPrzezAlfaBeta
    {
        public ObiektZwracanyPrzezAlfaBeta(int maxValue, Punkt miejscePionkaDoUsuniecia, Punkt miejscePionkaDoPostawienia)
        {
            this.maxValue = maxValue;
            this.miejscePionkaDoUsuniecia = miejscePionkaDoUsuniecia;
            this.miejscePionkaDoPostawienia = miejscePionkaDoPostawienia;
            this.nazwaPionka = "";
        }

        public int maxValue { get; set; } 
        public Punkt miejscePionkaDoUsuniecia { get; set; }
        public Punkt miejscePionkaDoPostawienia { get; set; }
        public string nazwaPionka { get; set; }
        public bool jestMlynek { get; set; }
    }
}