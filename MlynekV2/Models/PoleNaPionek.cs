
namespace MlynekV2.Models
{
    public class PoleNaPionek
    {

        public bool dopuszczalne { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public bool zajete { get; set; }
        //public Pionek pionek { get; set; }
        public int kolorGracza {get;set;}

        public PoleNaPionek(bool dopuszczalne, double x, double y, bool zajete)
        {
            this.dopuszczalne = dopuszczalne;
            this.x = x;
            this.y = y;
            this.zajete = zajete;
        }
    }
}