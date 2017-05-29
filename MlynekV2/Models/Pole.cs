using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MlynekV2.Models
{
    public class Pole
    {
        public PoleNaPionek[,] plansza { get; set; }

        public Pole()
        {
            this.plansza = new PoleNaPionek[7, 7];
            double x = 244;
            double y = 227;
            int poczDopPole = 0;
            int dopuszczalnePole = 0;
            int skok = 3;
            int elemDodDoSkoku = -1;
            for(int i = 0; i<7; i++)
            {
                for(int j=0; j<7; j++)
                {
                    if ((i == 0 && (j == 0 || j == 3 || j == 6))
                        || (i == 1 && (j == 1 || j == 3 || j == 5))
                        || (i == 2 && (j == 2 || j == 3 || j == 4))
                        || (i == 3 && (j == 0 || j == 1 || j == 2 || j == 4 || j == 5 || j == 6))
                        || (i == 4 && (j == 2 || j == 3 || j == 4))
                        || (i == 5 && (j == 1 || j == 3 || j == 5))
                        || (i == 6 && (j == 0 || j == 3 || j == 6))){
                        this.plansza[i, j] = new PoleNaPionek(true, x, y, false);
                    } else {
                        this.plansza[i, j] = new PoleNaPionek(false, x, y, false);
                    }
                    x += 57;
                }
                y += 57;
                x = 244;
            }
        }

    }
}