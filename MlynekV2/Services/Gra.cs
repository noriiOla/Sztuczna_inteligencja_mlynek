
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


        public WynikRuchu obsluzRuch(Punkt nowyPunkt, int kolorGracza, Punkt staryPunkt)
        {
            int correctX = -1;
            int correctY = -1;

            for(int i=0; i<7; i++)
            {
                if(this.pole.plansza[0, i].x-17 < nowyPunkt.x && this.pole.plansza[0, i].x + 17 > nowyPunkt.x)
                {
                    correctY = i;
                }
            }

            for (int i = 0; i < 7; i++)
            {
                if (this.pole.plansza[i, 0].y - 17 < nowyPunkt.y && this.pole.plansza[i, 0].y + 17 > nowyPunkt.y)
                {
                    correctX = i;
                }
            }

            if(correctX == -1 || correctY == -1)
            {
                return new WynikRuchu(new Punkt(0,0), false);
            }else
            {
                this.pole.plansza[correctX, correctY].zajete = true;
                this.pole.plansza[correctX, correctY].kolorGracza = kolorGracza;
                usunZPlanszyInformacjeOStarymPunkcie(staryPunkt);
                // dodaj do planszy pionek/zmien nowe miejsce na zajete (zmien stary (miejsce na planszy na wolne)
                return new WynikRuchu(new Punkt(this.pole.plansza[correctX, correctY].x, this.pole.plansza[correctX, correctY].y), powstalMlynek(correctX, correctY, kolorGracza));
            }
        }

        public void usunZPlanszyInformacjeOStarymPunkcie(Punkt staryPunkt)
        {
            int correctX = -1;
            int correctY = -1;

            for (int i = 0; i < 7; i++)
            {
                if (this.pole.plansza[0, i].x - 17 < staryPunkt.x && this.pole.plansza[0, i].x + 17 > staryPunkt.x)
                {
                    correctY = i;
                }
            }

            for (int i = 0; i < 7; i++)
            {
                if (this.pole.plansza[i, 0].y - 17 < staryPunkt.y && this.pole.plansza[i, 0].y + 17 > staryPunkt.y)
                {
                    correctX = i;
                }
            }

            if (correctX != -1 && correctY != -1)
            {
                this.pole.plansza[correctX, correctY].zajete = false;
            }
        }

        public bool powstalMlynek(int x, int y, int kolorGracza)
        {
            int ilosc = 0;

            for(int i=0; i<7; i++)
            {
                if(this.pole.plansza[x, i].kolorGracza == kolorGracza)
                {
                    ilosc++;
                    if(ilosc == 3)
                    {
                        return true;
                    }
                }
            }

            ilosc = 0;

            for (int i = 0; i < 7; i++)
            {
                if (this.pole.plansza[i, y].kolorGracza == kolorGracza)
                {
                    ilosc++;
                    if (ilosc == 3)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}