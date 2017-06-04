
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
        public List<ObiektZwracanyPrzezAlfaBeta> dwaOstPosunieciaB { get; set; }
        public List<ObiektZwracanyPrzezAlfaBeta> dwaOstPosunieciaC { get; set; }
        public string typBialych { get; set; }
        public string typCzarnych { get; set; }
        public string poziom { get; set; }
        public string kolejnoscWezlow { get; set; }
        public string algorytm { get; set; }
        public int iloscPowtElem = 0;

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

        public bool ruchSiePowtorzyl(ObiektZwracanyPrzezAlfaBeta ruch)                              
        {
            if (ruch != null)                                                   ////////////////////////////////////
            {
                if ((ruch.nazwaPionka[0].ToString()).Equals("b"))
                {
                    foreach (ObiektZwracanyPrzezAlfaBeta wczesnRuch in dwaOstPosunieciaB)
                    {
                        if (wczesnRuch.miejscePionkaDoPostawienia.x == ruch.miejscePionkaDoUsuniecia.x
                          && wczesnRuch.miejscePionkaDoPostawienia.y == ruch.miejscePionkaDoUsuniecia.y
                          || (wczesnRuch.miejscePionkaDoPostawienia.x == ruch.miejscePionkaDoPostawienia.x
                          && wczesnRuch.miejscePionkaDoPostawienia.y == ruch.miejscePionkaDoPostawienia.y
                          && wczesnRuch.miejscePionkaDoUsuniecia.x == ruch.miejscePionkaDoUsuniecia.x
                          && wczesnRuch.miejscePionkaDoUsuniecia.y == ruch.miejscePionkaDoUsuniecia.y
                          ))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    foreach (ObiektZwracanyPrzezAlfaBeta wczesnRuch in dwaOstPosunieciaC)
                    {
                        if (wczesnRuch.miejscePionkaDoPostawienia.x == ruch.miejscePionkaDoUsuniecia.x
                           && wczesnRuch.miejscePionkaDoPostawienia.y == ruch.miejscePionkaDoUsuniecia.y
                           || (wczesnRuch.miejscePionkaDoPostawienia.x == ruch.miejscePionkaDoPostawienia.x
                           && wczesnRuch.miejscePionkaDoPostawienia.y == ruch.miejscePionkaDoPostawienia.y
                           && wczesnRuch.miejscePionkaDoUsuniecia.x == ruch.miejscePionkaDoUsuniecia.x
                           && wczesnRuch.miejscePionkaDoUsuniecia.y == ruch.miejscePionkaDoUsuniecia.y
                           ))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void dodajZmienne(string typBialych, string typCzarnych, string poziom, string kolejnoscWezlow, string algorytm)
        {
            this.typBialych = typBialych;
            this.typCzarnych = typCzarnych;
            this.poziom = poziom;
            this.kolejnoscWezlow = kolejnoscWezlow;
            this.algorytm = algorytm;
        }

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
            gracz.iloscPionkowNieWylozonych--;
            int numerPionka = Convert.ToInt32(nazwaPionka[1].ToString());
            gracz.nieUzytePionki.Remove(numerPionka);
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

                if (poleDoSprawdzenia.plansza[x, i].kolorGracza == kolorGracza && poleDoSprawdzenia.plansza[x, i].zajete && poleDoSprawdzenia.plansza[x, i].dopuszczalne)
                {
                    ilosc++;

                    if (x == 3 && y > 3 && i < 3)
                    {
                        ilosc = 0;
                    }

                    if (ilosc == 3)
                    {       
                        return true;
                    }

                }
                //if (x == 3 && i == 3)
                //{
                //    ilosc = 0;
                //}
            }

            ilosc = 0;

            for (int i = 0; i < 7; i++)
            {
               
                if (poleDoSprawdzenia.plansza[i, y].kolorGracza == kolorGracza && poleDoSprawdzenia.plansza[i, y].zajete && poleDoSprawdzenia.plansza[i, y].dopuszczalne)
                {
                    ilosc++;
                    if (y == 3 && x > 3 && i < 3)
                    {
                        ilosc = 0;
                    }

                    if (ilosc == 3)
                    {              
                        return true; 
                    }

                }
                //if (y == 3 && i == 3)
                //{
                //    ilosc = 0;

                //}
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
                            if (this.kolejnoscWezlow.Equals("Od tylu"))
                            {
                                listaPunktow.Reverse();
                            }
                            wolneMiejsca.Add(new Ruch(pole.plansza[indexW, indexKol].nazwaPionka, new Punkt(indexW, indexKol), listaPunktow));
                        }
                    }
                }
            }
            if (this.kolejnoscWezlow.Equals("Od tylu"))
            {
                wolneMiejsca.Reverse();
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
            if (this.kolejnoscWezlow.Equals("Od tylu"))
            {
                listaPunktow.Reverse();
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
            if (this.kolejnoscWezlow.Equals("Od tylu"))
            {
                pionkiDoZabrania.Reverse();
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
            if (this.kolejnoscWezlow.Equals("Od tylu"))
            {
                wolneMiejsca.Reverse();
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
                this.pole.plansza[(int)ruch.miejscePionkaDoUsuniecia.x, (int)ruch.miejscePionkaDoUsuniecia.y].zajete = false;
                dwaOstPosunieciaC.Clear();
                dwaOstPosunieciaB.Clear();
                ruch.nazwaPionka = pole.plansza[(int)ruch.miejscePionkaDoUsuniecia.x, (int)ruch.miejscePionkaDoUsuniecia.y].nazwaPionka;    
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
                if(ruch == null)////////////////////////////////////////////////////////
                {
                    ruch.miejscePionkaDoPostawienia = null;
                    ruch.miejscePionkaDoUsuniecia = null;
                    ruch.nazwaPionka = "";
                    return ruch;
                }

                ruch.nazwaPionka = pole.plansza[(int)ruch.miejscePionkaDoUsuniecia.x, (int)ruch.miejscePionkaDoUsuniecia.y].nazwaPionka;
                if (ruchSiePowtorzyl(ruch))
                {
                    iloscPowtElem++;
                    if (iloscPowtElem >= 4)
                    {
                        List<Ruch> listaMozliwychRuchow = znajdzMozliwePrzesuniecia(gracz.kolor, this.pole);
                        ObiektZwracanyPrzezAlfaBeta losowyObiekt = ruch;
                        string noweIdPionka = ruch.nazwaPionka;
                        Punkt nowyPunktDoPostawienia = ruch.miejscePionkaDoPostawienia;

                        if (listaMozliwychRuchow.Count == 0)
                        {
                            ruch.miejscePionkaDoPostawienia = null;
                            ruch.miejscePionkaDoUsuniecia = null;
                            ruch.nazwaPionka = "";
                            return ruch;
                        }

                        Random rand = new Random();
                        rand.Next(0, listaMozliwychRuchow.Count);
                        int idElemntuDoUsuniecia = rand.Next(0, listaMozliwychRuchow.Count);
                        int idNowegoMiejsca = rand.Next(0, listaMozliwychRuchow[idElemntuDoUsuniecia].miejscaNaKtoreMozeSieRuszyc.Count);
                        ruch = new ObiektZwracanyPrzezAlfaBeta(0, listaMozliwychRuchow[idElemntuDoUsuniecia].miejscePionkaDoUsniecia, listaMozliwychRuchow[idElemntuDoUsuniecia].miejscaNaKtoreMozeSieRuszyc[idNowegoMiejsca]);
                    }
                }else
                {
                    iloscPowtElem = 0;
                }

                ruch.nazwaPionka = pole.plansza[(int)ruch.miejscePionkaDoUsuniecia.x, (int)ruch.miejscePionkaDoUsuniecia.y].nazwaPionka;
                if (gracz.kolor == 1)
                {
                    if (dwaOstPosunieciaB.Count == 4)
                    {
                        dwaOstPosunieciaB.RemoveAt(0);
                    }
                    dwaOstPosunieciaB.Add(new ObiektZwracanyPrzezAlfaBeta(0, new Punkt(ruch.miejscePionkaDoUsuniecia.x, ruch.miejscePionkaDoUsuniecia.y), new Punkt(ruch.miejscePionkaDoPostawienia.x, ruch.miejscePionkaDoPostawienia.y), ruch.nazwaPionka));
                } else
                {
                    if(dwaOstPosunieciaC.Count == 4)
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