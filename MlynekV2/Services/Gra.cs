
using MlynekV2.Models;
using System;

namespace MlynekV2.Services
{
    public class Gra
    {
        Pole pole;
        Gracz gracz1;
        Gracz gracz2;
        public static Gra gra;

        public Gra()
        {

        }

        public void init(int typGracza1, int typGracza2)
        {
            this.pole = new Pole();
            //0 kolor bialy; 1 kolor czarny
            //0 - czlowiek; 1- komputer
            this.gracz1 = new Gracz(0, typGracza1, 0, 9);
            this.gracz2 = new Gracz(1, typGracza2, 0, 9);  
        }

        public static Gra getInstance()
        {
            if (gra == null)
            {
                gra = new Gra();
            }
            return gra;
        }


        public Punkt obsluzRuch(double x, double y, int kolorGracza)
        {
            int correctX = -1;
            int correctY = -1;
            int valueToReturn = 0;

            for(int i=0; i<7; i++)
            {
                if(this.pole.plansza[0, i].x-17 < x && this.pole.plansza[0, i].x + 17 > x)
                {
                    correctY = i;
                }
            }

            for (int i = 0; i < 7; i++)
            {
                if (this.pole.plansza[i, 0].y - 17 < y && this.pole.plansza[i, 0].y + 17 > y)
                {
                    correctX = i;
                }
            }

            if(correctX == -1 || correctY == -1)
            {
                return (new Punkt(0,0));
            }else
            {
                return new Punkt(this.pole.plansza[correctX, correctY].x, this.pole.plansza[correctX, correctY].y);
            }
        }
    }
}