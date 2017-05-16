
namespace MlynekV2.Models
{
    public class WynikRuchu
    {
        public WynikRuchu(Punkt punkt, bool czyJestMlynek)
        {
            this.punkt = punkt;
            this.czyJestMlynek = czyJestMlynek;
        }

        public Punkt punkt { get; set; }
        public bool czyJestMlynek { get; set; }
    }
}