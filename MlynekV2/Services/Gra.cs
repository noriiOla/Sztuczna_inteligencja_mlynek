
using MlynekV2.Models;
using System;
using System.Collections.Generic;

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
            //2 kolor Czarny; 1 kolor bialy
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


        public WynikRuchu obsluzRuch(Punkt nowyPunkt, int kolorGracza, Punkt staryPunkt, string nazwaPionka)
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
                this.pole.plansza[correctX, correctY].nazwaPionka = nazwaPionka;
                usunZPlanszyInformacjeOStarymPunkcie(staryPunkt);
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
                this.pole.plansza[correctX, correctY].nazwaPionka = "";
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
                if (x == 3 && i == 3)
                {
                    ilosc = 0;
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
                if (y == 3 && i == 3)
                {
                    ilosc = 0;
                }
            }

            return false;
        }

        public List<Ruch> znajdzMiejscaDostepne(string kolor)
        {
            List<Ruch> wolneMiejsca = new List<Ruch>();
            int intKolorGracza = Convert.ToInt32(kolor);
            for(int indexW = 0; indexW < 7; indexW++)
            {
                for (int indexKol = 0; indexKol < 7; indexKol++)
                {
                    if(this.pole.plansza[indexW, indexKol].dopuszczalne && this.pole.plansza[indexW, indexKol].zajete && this.pole.plansza[indexW, indexKol].kolorGracza == intKolorGracza)
                    {
                        wolneMiejsca.Add(new Ruch(this.pole.plansza[indexW, indexKol].nazwaPionka, szukajWolnychMiejc(indexW, indexKol, intKolorGracza)));
                    }
                }
            }
            return wolneMiejsca; 
        }

        public List<Punkt> szukajWolnychMiejc(int indexW, int indexKol, int intKolorGracza)
        {
            List<Punkt> listaPunktow = new List<Punkt>();
            
            int tempW = indexW+1;
            if(tempW < 7) { 
                while ((this.pole.plansza[tempW, indexKol] != null) && (!this.pole.plansza[tempW, indexKol].dopuszczalne) && (!(tempW == 3 && indexKol == 3)))
                {
                    tempW++;
                }

                if((this.pole.plansza[tempW, indexKol] != null) && (this.pole.plansza[tempW, indexKol].dopuszczalne) && (!(tempW == 3 && indexKol == 3)) && (!this.pole.plansza[tempW, indexKol].zajete))
                {
                    listaPunktow.Add(new Punkt(tempW, indexKol));
                }
            }

            tempW = indexW - 1;
            if(tempW > -1) { 
                while ((this.pole.plansza[tempW, indexKol] != null) && (!this.pole.plansza[tempW, indexKol].dopuszczalne) && (!(tempW == 3 && indexKol == 3)))
                {
                    tempW--;
                }

                if ((this.pole.plansza[tempW, indexKol] != null) && (this.pole.plansza[tempW, indexKol].dopuszczalne) && (!(tempW == 3 && indexKol == 3)) && (!this.pole.plansza[tempW, indexKol].zajete))
                {
                    listaPunktow.Add(new Punkt(tempW, indexKol));
                }
            }

            int tempK = indexKol- 1;
            if(tempK > -1) { 
                while ((this.pole.plansza[indexW, tempK] != null) && (!this.pole.plansza[indexW, tempK].dopuszczalne) && (!(indexW == 3 && tempK == 3)))
                {
                    tempK--;
                }

                if ((this.pole.plansza[indexW, tempK] != null) && (this.pole.plansza[indexW, tempK].dopuszczalne) && (!(indexW == 3 && tempK == 3)) && (!this.pole.plansza[indexW, tempK].zajete))
                {
                    listaPunktow.Add(new Punkt(indexW, tempK));
                }
            }

            tempK = indexKol + 1;

            if(tempK < 7) { 
                while ((this.pole.plansza[indexW, tempK] != null) && (!this.pole.plansza[indexW, tempK].dopuszczalne) && (!(indexW == 3 && tempK == 3)))
                {
                    tempK++;
                }

                if ((this.pole.plansza[indexW, tempK] != null) && (this.pole.plansza[indexW, tempK].dopuszczalne) && (!(indexW == 3 && tempK == 3)) && (!this.pole.plansza[indexW, tempK].zajete))
                {
                    listaPunktow.Add(new Punkt(indexW, tempK));
                }
            }
            return listaPunktow;
        }

        public List<PionekDoZabrania> znajdzPionkiDoZabrania(string kolor) //kolor pionka ktory chemy zabrac
        {
            List<PionekDoZabrania> pionkiDoZabrania = new List<PionekDoZabrania>();
            int intPionkaDoZabrania = Convert.ToInt32(kolor);
            for (int indexW = 0; indexW < 7; indexW++)
            {
                for (int indexKol = 0; indexKol < 7; indexKol++)
                {
                    if (this.pole.plansza[indexW, indexKol].dopuszczalne && this.pole.plansza[indexW, indexKol].zajete && this.pole.plansza[indexW, indexKol].kolorGracza == intPionkaDoZabrania)
                    {
                        pionkiDoZabrania.Add(new PionekDoZabrania(this.pole.plansza[indexW, indexKol].nazwaPionka, new Punkt(indexW, indexKol)));
                    }
                }
            }
            return pionkiDoZabrania;
        }

        public List<Punkt> znajdzWolneMiejsca() 
        {
            List<Punkt> wolneMiejsca = new List<Punkt>();
            for (int indexW = 0; indexW < 7; indexW++)
            {
                for (int indexKol = 0; indexKol < 7; indexKol++)
                {
                    if (this.pole.plansza[indexW, indexKol].dopuszczalne && (!this.pole.plansza[indexW, indexKol].zajete))
                    {
                        wolneMiejsca.Add(new Punkt(indexW, indexKol));
                    }
                }
            }
            return wolneMiejsca;
        }
    }
}