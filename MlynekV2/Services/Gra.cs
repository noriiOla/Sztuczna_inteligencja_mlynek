
using MlynekV2.Models;
using System;
using System.Collections.Generic;

namespace MlynekV2.Services
{
    public class Gra
    {
        public Pole pole;
        public Gracz gracz1;
        public Gracz gracz2;
        public static Gra gra;
        public static List<ObiektZwracanyPrzezAlfaBeta> dwaOstPosunieciaB { get; set; }
        public static List<ObiektZwracanyPrzezAlfaBeta> dwaOstPosunieciaC { get; set; }

        private Gra()
        {
            this.pole = new Pole();
            //2 kolor Czarny; 1 kolor bialy
            //0 - czlowiek; 1- komputer
            this.gracz1 = new Gracz(1, 0, 0, 9);
            this.gracz2 = new Gracz(2, 0, 0, 9);
            dwaOstPosunieciaB = new List<ObiektZwracanyPrzezAlfaBeta>();
            dwaOstPosunieciaC = new List<ObiektZwracanyPrzezAlfaBeta>();
        }

        //public static bool wystepujeWLiscie(int kolor, string noweIdPionka, Punkt miejscePionkaDoUsuniecia, Punkt miejscePionkaDoPostawienia)
        //{
        //    if(kolor == 1)
        //    {
        //        return jestWLiscie(dwaOstPosunieciaB, kolor, noweIdPionka, miejscePionkaDoUsuniecia, miejscePionkaDoPostawienia);
        //    }
        //    else
        //    {
        //        return jestWLiscie(dwaOstPosunieciaC, kolor, noweIdPionka, miejscePionkaDoUsuniecia, miejscePionkaDoPostawienia);
        //    }
        //}

        public bool ruchSiePowtorzyl()
        {
            if(dwaOstPosunieciaB.Count == 2 && dwaOstPosunieciaC.Count == 2)
            {
                if(dwaOstPosunieciaB[0].miejscePionkaDoPostawienia.x == dwaOstPosunieciaB[1].miejscePionkaDoUsuniecia.x 
                    && dwaOstPosunieciaB[0].miejscePionkaDoPostawienia.y == dwaOstPosunieciaB[1].miejscePionkaDoUsuniecia.y
                    && dwaOstPosunieciaC[0].miejscePionkaDoPostawienia.x == dwaOstPosunieciaC[1].miejscePionkaDoUsuniecia.x
                    && dwaOstPosunieciaC[0].miejscePionkaDoPostawienia.y == dwaOstPosunieciaC[1].miejscePionkaDoUsuniecia.y)
                {
                    return true;
                }
            }
            return false;
        }

        //public static bool jestWLiscie(List<ObiektZwracanyPrzezAlfaBeta> lista, int kolor, string noweIdPionka, Punkt miejscePionkaDoUsuniecia, Punkt miejscePionkaDoPostawienia)
        //{
        //    foreach (ObiektZwracanyPrzezAlfaBeta ruch in lista)
        //    {
        //        if (ruch.nazwaPionka.Equals(noweIdPionka) && ruch.miejscePionkaDoUsuniecia.x == miejscePionkaDoUsuniecia.x && ruch.miejscePionkaDoUsuniecia.y == miejscePionkaDoUsuniecia.y && ruch.miejscePionkaDoPostawienia.x == miejscePionkaDoPostawienia.x && ruch.miejscePionkaDoPostawienia.y == miejscePionkaDoPostawienia.y)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        public static void createNewInstance()
        { 
              gra = new Gra();
        }

        public static Gra getInstance()
        {
            if (gra == null)
            {
                gra = new Gra();
            }
            return gra;
        }


        public WynikRuchu obsluzRuch(Punkt nowyPunkt, Gracz gracz, Punkt staryPunkt, string nazwaPionka)
        {
            int correctX = -1;
            int correctY = -1;

            if (gracz.nieUzytePionki.Contains(Convert.ToInt32(nazwaPionka[1])))
            {
                gracz.nieUzytePionki.Remove(Convert.ToInt32(nazwaPionka[1]));
            }
            
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
                this.pole.plansza[correctX, correctY].kolorGracza = gracz.kolor;
                this.pole.plansza[correctX, correctY].nazwaPionka = nazwaPionka;
                usunZPlanszyInformacjeOStarymPunkcie(staryPunkt);
                return new WynikRuchu(new Punkt(this.pole.plansza[correctX, correctY].x, this.pole.plansza[correctX, correctY].y), powstalMlynek(correctX, correctY, gracz.kolor, this.pole));
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

        public bool powstalMlynek(int x, int y, int kolorGracza, Pole poleDoSprawdzenia)
        {
            int ilosc = 0;

            for(int i=0; i<7; i++)
            {
                if(poleDoSprawdzenia.plansza[x, i].kolorGracza == kolorGracza && poleDoSprawdzenia.plansza[x, i].zajete && poleDoSprawdzenia.plansza[x, i].dopuszczalne)
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
                if (poleDoSprawdzenia.plansza[i, y].kolorGracza == kolorGracza && poleDoSprawdzenia.plansza[i, y].zajete && poleDoSprawdzenia.plansza[i, y].dopuszczalne)
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


        /// <summary>
        /// ////////////////////////////////////////////////meotdy heurystyczne
        /// </summary>
        /// <param name="gracz"></param>
        /// <returns></returns>
        public List<Ruch> znajdzMozliwePrzesuniecia(int graczKolor, Pole pole)
        {
            List<Ruch> wolneMiejsca = new List<Ruch>();
            for(int indexW = 0; indexW < 7; indexW++)
            {
                for (int indexKol = 0; indexKol < 7; indexKol++)
                {
                    if(pole.plansza[indexW, indexKol].dopuszczalne && pole.plansza[indexW, indexKol].zajete && pole.plansza[indexW, indexKol].kolorGracza == graczKolor)
                    {
                        List<Punkt> listaPunktow = szukajWolnychMiejc(indexW, indexKol, graczKolor, pole);
                        if(listaPunktow.Count != 0)
                        {
                            wolneMiejsca.Add(new Ruch(pole.plansza[indexW, indexKol].nazwaPionka, new Punkt(indexW, indexKol), listaPunktow));
                        }
                    }
                }
            }
            return wolneMiejsca; 
        }

        public List<Punkt> szukajWolnychMiejc(int indexW, int indexKol, int intKolorGracza, Pole pole)
        {
            List<Punkt> listaPunktow = new List<Punkt>();
            
            int tempW = indexW+1;
            if(tempW < 7) { 
                while ( tempW < 6 && (pole.plansza[tempW, indexKol] != null) && (!pole.plansza[tempW, indexKol].dopuszczalne) && (!(tempW == 3 && indexKol == 3)))
                {
                    tempW++;
                }

                if((pole.plansza[tempW, indexKol] != null) && (pole.plansza[tempW, indexKol].dopuszczalne) && (!(tempW == 3 && indexKol == 3)) && (!pole.plansza[tempW, indexKol].zajete))
                {
                    listaPunktow.Add(new Punkt(tempW, indexKol));
                }
            }

            tempW = indexW - 1;
            if(tempW > -1) { 
                while (tempW > 0 && (pole.plansza[tempW, indexKol] != null) && (!pole.plansza[tempW, indexKol].dopuszczalne) && (!(tempW == 3 && indexKol == 3)))
                {
                    tempW--;
                }

                if ((pole.plansza[tempW, indexKol] != null) && (pole.plansza[tempW, indexKol].dopuszczalne) && (!(tempW == 3 && indexKol == 3)) && (!pole.plansza[tempW, indexKol].zajete))
                {
                    listaPunktow.Add(new Punkt(tempW, indexKol));
                }
            }

            int tempK = indexKol- 1;
            if(tempK > -1) { 
                while (tempK > 0  && (pole.plansza[indexW, tempK] != null) && (!pole.plansza[indexW, tempK].dopuszczalne) && (!(indexW == 3 && tempK == 3)))
                {
                    tempK--;
                }

                if ((pole.plansza[indexW, tempK] != null) && (pole.plansza[indexW, tempK].dopuszczalne) && (!(indexW == 3 && tempK == 3)) && (!pole.plansza[indexW, tempK].zajete))
                {
                    listaPunktow.Add(new Punkt(indexW, tempK));
                }
            }

            tempK = indexKol + 1;

            if(tempK < 7) { 
                while (tempK < 6 && (pole.plansza[indexW, tempK] != null) && (!pole.plansza[indexW, tempK].dopuszczalne) && (!(indexW == 3 && tempK == 3)))
                {
                    tempK++;
                }

                if ((pole.plansza[indexW, tempK] != null) && (pole.plansza[indexW, tempK].dopuszczalne) && (!(indexW == 3 && tempK == 3)) && (!pole.plansza[indexW, tempK].zajete))
                {
                    listaPunktow.Add(new Punkt(indexW, tempK));
                }
            }
            return listaPunktow;
        }

        public List<Punkt> znajdzPionkiDoZabrania(Pole pole, int kolor) //kolor pionka ktory chemy zabrac
        {
            
            List<Punkt> pionkiDoZabrania = new List<Punkt>();
            for (int indexW = 0; indexW < 7; indexW++)
            {
                for (int indexKol = 0; indexKol < 7; indexKol++)
                {
                    if (pole.plansza[indexW, indexKol].dopuszczalne && pole.plansza[indexW, indexKol].zajete && (!(pole.plansza[indexW, indexKol].kolorGracza == kolor)))
                    {
                        pionkiDoZabrania.Add(new Punkt(indexW, indexKol));
                    }
                }
            }
            return pionkiDoZabrania;
        }

        public List<Punkt> znajdzWolneMiejsca(Pole pole) 
        {
            List<Punkt> wolneMiejsca = new List<Punkt>();
            for (int indexW = 0; indexW < 7; indexW++)
            {
                for (int indexKol = 0; indexKol < 7; indexKol++)
                {
                    if (pole.plansza[indexW, indexKol].dopuszczalne && (!pole.plansza[indexW, indexKol].zajete))
                    {
                        wolneMiejsca.Add(new Punkt(indexW, indexKol));
                    }
                }
            }
            return wolneMiejsca;
        }

        //////////////////metody komp//////////
        //ObiektZwracanyPrzezAlfaBeta
        public ObiektZwracanyPrzezAlfaBeta kompZnajdzNajlepszeMiejsceIPionek(Gracz gracz, bool jestMlynek)
        {
            string stanGry = jestMlynek ? "mlynek" : gracz.nieUzytePionki.Count != 0 ? "rozdawaniePionkow" : "ruch";
            AlphaBeta algorytm = new AlphaBeta(this.pole);
            ObiektZwracanyPrzezAlfaBeta ruch = algorytm.play(algorytm.pole, stanGry, gracz.kolor, gracz.iloscPionkowNieWylozonych, gracz.kolor == 1 ? gracz2.iloscPionkowNieWylozonych : this.gracz1.iloscPionkowNieWylozonych);

            if (stanGry.Equals("mlynek"))
            {
                dwaOstPosunieciaC.Clear();
                dwaOstPosunieciaB.Clear();
                ruch.nazwaPionka = pole.plansza[(int)ruch.miejscePionkaDoUsuniecia.x, (int)ruch.miejscePionkaDoUsuniecia.y].nazwaPionka;
                this.pole.plansza[(int)ruch.miejscePionkaDoUsuniecia.x, (int)ruch.miejscePionkaDoUsuniecia.y].zajete = false;
                int x = (int)ruch.miejscePionkaDoUsuniecia.x;
                int y = (int)ruch.miejscePionkaDoUsuniecia.y;
                ruch.miejscePionkaDoUsuniecia.x = pole.plansza[x, y].x;
                ruch.miejscePionkaDoUsuniecia.y = pole.plansza[x, y].y;
                ruch.jestMlynek = false;
                ruch.stanGry = "oblugujeMlynek";
                return ruch;
            }
                if (stanGry.Equals("rozdawaniePionkow"))
                {
                    ruch.nazwaPionka = gracz.kolor == 1 ? "b" + gracz.nieUzytePionki[0].ToString() : "c" + gracz.nieUzytePionki[0].ToString();
                    gracz.nieUzytePionki.Remove(Convert.ToInt32(gracz.nieUzytePionki[0]));
                    if (this.pole.plansza[(int)ruch.miejscePionkaDoPostawienia.x, (int)ruch.miejscePionkaDoPostawienia.y].zajete) {
                        bool s   = true;
                    }
                    this.pole.plansza[(int)ruch.miejscePionkaDoPostawienia.x, (int)ruch.miejscePionkaDoPostawienia.y].zajete = true;
                    this.pole.plansza[(int)ruch.miejscePionkaDoPostawienia.x, (int)ruch.miejscePionkaDoPostawienia.y].kolorGracza = gracz.kolor;
                    this.pole.plansza[(int)ruch.miejscePionkaDoPostawienia.x, (int)ruch.miejscePionkaDoPostawienia.y].nazwaPionka = ruch.nazwaPionka;
                    int x = (int)ruch.miejscePionkaDoPostawienia.x;
                    int y = (int)ruch.miejscePionkaDoPostawienia.y;
                    ruch.miejscePionkaDoPostawienia.x = pole.plansza[x, y].x;
                    ruch.miejscePionkaDoPostawienia.y = pole.plansza[x, y].y;
                    ruch.jestMlynek = powstalMlynek(x, y, gracz.kolor, this.pole);
                    ruch.stanGry = "rozdawaniePionkow";
                    gracz.iloscPionkowNieWylozonych--;
                    return ruch;
                }

            if (stanGry.Equals("ruch"))
            {
                if (ruchSiePowtorzyl())
                {
                    List<Ruch> listaMozliwychRuchow = znajdzMozliwePrzesuniecia(gracz.kolor, this.pole);
                    ObiektZwracanyPrzezAlfaBeta losowyObiekt = ruch;
                    string noweIdPionka = ruch.nazwaPionka;
                    Punkt nowyPunktDoPostawienia = ruch.miejscePionkaDoPostawienia;

                    if (listaMozliwychRuchow.Count == 0 || (listaMozliwychRuchow.Count == 1 && listaMozliwychRuchow[0].miejscaNaKtoreMozeSieRuszyc.Count < 2))
                    {
                        ruch.miejscePionkaDoPostawienia = null;
                        ruch.miejscePionkaDoUsuniecia = null;
                        ruch.nazwaPionka = "";
                        return ruch;            //w js sprawdz czy ruch juz byl
                        //przegrales //zwroc null(wtedybedzie info jednoczesnie)
                    }


                    Random rand = new Random();
                    rand.Next(0, listaMozliwychRuchow.Count);
                    int idElemntuDoUsuniecia = rand.Next(0, listaMozliwychRuchow.Count);
                    int idNowegoMiejsca = rand.Next(0, listaMozliwychRuchow[idElemntuDoUsuniecia].miejscaNaKtoreMozeSieRuszyc.Count);
                    ruch = new ObiektZwracanyPrzezAlfaBeta(0, listaMozliwychRuchow[idElemntuDoUsuniecia].miejscePionkaDoUsniecia, listaMozliwychRuchow[idElemntuDoUsuniecia].miejscaNaKtoreMozeSieRuszyc[idNowegoMiejsca]);

                }
            
                ruch.nazwaPionka = pole.plansza[(int)ruch.miejscePionkaDoUsuniecia.x, (int)ruch.miejscePionkaDoUsuniecia.y].nazwaPionka;
                if (gracz.kolor == 1)
                {
                    if (dwaOstPosunieciaB.Count == 2)
                    {
                        dwaOstPosunieciaB.RemoveAt(0);
                    }
                    dwaOstPosunieciaB.Add(new ObiektZwracanyPrzezAlfaBeta(0, new Punkt(ruch.miejscePionkaDoUsuniecia.x, ruch.miejscePionkaDoUsuniecia.y), new Punkt(ruch.miejscePionkaDoPostawienia.x, ruch.miejscePionkaDoPostawienia.y), ruch.nazwaPionka));
                } else
                {
                    if(dwaOstPosunieciaC.Count == 2)
                    {
                        dwaOstPosunieciaC.RemoveAt(0);
                    }
                    dwaOstPosunieciaC.Add(new ObiektZwracanyPrzezAlfaBeta(0, new Punkt(ruch.miejscePionkaDoUsuniecia.x, ruch.miejscePionkaDoUsuniecia.y), new Punkt(ruch.miejscePionkaDoPostawienia.x, ruch.miejscePionkaDoPostawienia.y), ruch.nazwaPionka));
                }

                pole.plansza[(int)ruch.miejscePionkaDoUsuniecia.x, (int)ruch.miejscePionkaDoUsuniecia.y].zajete = false;
                pole.plansza[(int)ruch.miejscePionkaDoPostawienia.x, (int)ruch.miejscePionkaDoPostawienia.y].zajete = true;
                pole.plansza[(int)ruch.miejscePionkaDoPostawienia.x, (int)ruch.miejscePionkaDoPostawienia.y].kolorGracza = gracz.kolor;
                pole.plansza[(int)ruch.miejscePionkaDoPostawienia.x, (int)ruch.miejscePionkaDoPostawienia.y].nazwaPionka = ruch.nazwaPionka;  
                int x = (int)ruch.miejscePionkaDoUsuniecia.x;
                int y = (int)ruch.miejscePionkaDoUsuniecia.y;
                ruch.miejscePionkaDoUsuniecia.x = pole.plansza[x, y].x;
                ruch.miejscePionkaDoUsuniecia.y = pole.plansza[x, y].y;
                x = (int)ruch.miejscePionkaDoPostawienia.x;
                y = (int)ruch.miejscePionkaDoPostawienia.y;
                ruch.jestMlynek = powstalMlynek(x, y, gracz.kolor, this.pole);
                ruch.miejscePionkaDoPostawienia.x = pole.plansza[x, y].x;
                ruch.miejscePionkaDoPostawienia.y = pole.plansza[x, y].y;
 
                ruch.stanGry = "ruch";
                return ruch;
            
            }
            return null;
        }
    }
}