using MlynekV2.Models;
using System;
using System.Collections.Generic;

namespace MlynekV2.Services
{
    public class AlphaBeta
    {
        int maxPoziom;
        public Pole pole;

        public AlphaBeta(Pole pole)
        {
            maxPoziom = Convert.ToInt32(Gra.getInstance().poziom);
            this.pole = new Pole();
            for (int indexW = 0; indexW < 7; indexW++)
            {
                for (int indexKol = 0; indexKol < 7; indexKol++)
                {
                    this.pole.plansza[indexW, indexKol].dopuszczalne = pole.plansza[indexW, indexKol].dopuszczalne;
                    this.pole.plansza[indexW, indexKol].kolorGracza = pole.plansza[indexW, indexKol].kolorGracza;
                    this.pole.plansza[indexW, indexKol].nazwaPionka = pole.plansza[indexW, indexKol].nazwaPionka;
                    this.pole.plansza[indexW, indexKol].x = pole.plansza[indexW, indexKol].x;
                    this.pole.plansza[indexW, indexKol].y = pole.plansza[indexW, indexKol].y;
                    this.pole.plansza[indexW, indexKol].zajete = pole.plansza[indexW, indexKol].zajete;
                }
            }
        }

        public ObiektZwracanyPrzezAlfaBeta play(Pole pole, string etapGry, int kolor, int iloscPionkowNieRozdanychMoich, int iloscNieWylozonaychPrzeciwnika)     // u nas zawsze zaczynamy od max, a przeciwnik musi miec min
        {
            int poziom = 0;
            ObiektZwracanyPrzezAlfaBeta wynik = playMax(int.MinValue, int.MaxValue, this.pole, etapGry, poziom, kolor, iloscPionkowNieRozdanychMoich, iloscNieWylozonaychPrzeciwnika, null, 0);
            return wynik;
        }

        public ObiektZwracanyPrzezAlfaBeta playMax(int alpha, int beta, Pole pole, string etapGry, int poziom, int kolor, int iloscPionkowNieRozdanychMoich, int iloscPionkowNieRozdanychPrzeciwnika, ObiektZwracanyPrzezAlfaBeta wczesniejszyRuch, int iloscPowtorzenRuchu)
        {
            poziom++;
            ObiektZwracanyPrzezAlfaBeta value = new ObiektZwracanyPrzezAlfaBeta(int.MinValue, null, null);
            Gra gra = Gra.getInstance();
            if (etapGry.Equals("rozdawaniePionkow"))
            {
                List<Punkt> listaRuchow = gra.znajdzWolneMiejsca(pole);
                if (poziom < this.maxPoziom)
                {
                    foreach (Punkt ruch in listaRuchow)
                    {
                        pole.plansza[(int)ruch.x, (int)ruch.y].zajete = true;
                        pole.plansza[(int)ruch.x, (int)ruch.y].kolorGracza = kolor;
                        if (Gra.getInstance().powstalMlynek((int)ruch.x, (int)ruch.y, kolor, pole))
                        {
                            ObiektZwracanyPrzezAlfaBeta wynikMax = playMax(alpha, beta, pole, "mlynek", poziom, kolor, iloscPionkowNieRozdanychMoich - 1, iloscPionkowNieRozdanychPrzeciwnika, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, wczesniejszyRuch == null ? null : wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch == null ? ruch : wczesniejszyRuch.miejscePionkaDoPostawienia), iloscPowtorzenRuchu);

                            if (value.maxValue < wynikMax.maxValue)
                            {
                                value = wynikMax;
                            }
                        }
                        else
                        {
                            if (iloscPionkowNieRozdanychPrzeciwnika <= 0)
                            {
                                ObiektZwracanyPrzezAlfaBeta wynikMax = playMin(alpha, beta, pole, "ruch", poziom, kolor == 1 ? 2 : 1, iloscPionkowNieRozdanychMoich , iloscPionkowNieRozdanychPrzeciwnika, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, wczesniejszyRuch == null ? null : wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch == null ? ruch : wczesniejszyRuch.miejscePionkaDoPostawienia), iloscPowtorzenRuchu);
                                if (value.maxValue < wynikMax.maxValue)
                                {
                                    value = wynikMax;
                                }
                            }
                            else
                            {
                                ObiektZwracanyPrzezAlfaBeta wynikMax = playMin(alpha, beta, pole, "rozdawaniePionkow", poziom, kolor == 1 ? 2 : 1, iloscPionkowNieRozdanychMoich - 1, iloscPionkowNieRozdanychPrzeciwnika, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, wczesniejszyRuch == null ? null : wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch == null ? ruch : wczesniejszyRuch.miejscePionkaDoPostawienia), iloscPowtorzenRuchu);
                                if (value.maxValue < wynikMax.maxValue)
                                {
                                    value = wynikMax;
                                }
                            }
                        }
                        pole.plansza[(int)ruch.x, (int)ruch.y].zajete = false;
                        if (Gra.getInstance().algorytm.Equals("AlphaBeta"))
                        {
                            if (value.maxValue > beta)
                            {
                                return value;
                            }
                            if (value.maxValue > alpha)
                            {
                                alpha = value.maxValue;
                            }
                        }

                    }
                }
                else
                {
                    ObiektZwracanyPrzezAlfaBeta obiektDoZwrocenia = new ObiektZwracanyPrzezAlfaBeta(funckjaPrzydzielaniaPunktow(pole, kolor, listaRuchow.Count), wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch.miejscePionkaDoPostawienia);
                    obiektDoZwrocenia.iloscPowtorzenRuchu = iloscPowtorzenRuchu;
                    return obiektDoZwrocenia;
                }
            }
            else
            {
                if (etapGry.Equals("ruch"))
                {
                    List<Ruch> listaRuchow = gra.znajdzMozliwePrzesuniecia(kolor, pole);
                    if (poziom < this.maxPoziom)
                    {
                        foreach (Ruch ruch in listaRuchow)
                        {
                            pole.plansza[(int)ruch.miejscePionkaDoUsniecia.x, (int)ruch.miejscePionkaDoUsniecia.y].zajete = false;          // w js potem nadaj temu nazwe pionka i wyslij info do c#

                            foreach (Punkt punkt in ruch.miejscaNaKtoreMozeSieRuszyc)
                            {
                                pole.plansza[(int)punkt.x, (int)punkt.y].zajete = true;          // w js potem nadaj temu nazwe pionka i wyslij info do c#
                                pole.plansza[(int)punkt.x, (int)punkt.y].kolorGracza = kolor;

                                if (Gra.getInstance().powstalMlynek((int)punkt.x, (int)punkt.y, kolor, pole))
                                {
                                    ObiektZwracanyPrzezAlfaBeta wynikMax = playMax(alpha, beta, pole, "mlynek", poziom, kolor, iloscPionkowNieRozdanychMoich, iloscPionkowNieRozdanychPrzeciwnika, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, (wczesniejszyRuch != null && wczesniejszyRuch.miejscePionkaDoUsuniecia != null) ? wczesniejszyRuch.miejscePionkaDoUsuniecia : ruch.miejscePionkaDoUsniecia, wczesniejszyRuch == null ? punkt : wczesniejszyRuch.miejscePionkaDoPostawienia), iloscPowtorzenRuchu);
                                    if (value.maxValue < wynikMax.maxValue)
                                    {
                                        value = wynikMax;
                                    }
                                }
                                else
                                {
                                    ObiektZwracanyPrzezAlfaBeta wynikMax;
                                     wynikMax = playMin(alpha, beta, pole, "ruch", poziom, kolor == 1 ? 2 : 1, iloscPionkowNieRozdanychMoich, iloscPionkowNieRozdanychPrzeciwnika, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, (wczesniejszyRuch != null && wczesniejszyRuch.miejscePionkaDoUsuniecia != null) ? wczesniejszyRuch.miejscePionkaDoUsuniecia : ruch.miejscePionkaDoUsniecia, wczesniejszyRuch == null ? punkt : wczesniejszyRuch.miejscePionkaDoPostawienia), iloscPowtorzenRuchu); 
                                    if (value.maxValue < wynikMax.maxValue)
                                    {
                                        value = wynikMax;
                                    }
                                }
                                pole.plansza[(int)punkt.x, (int)punkt.y].zajete = false;
                                if (Gra.getInstance().algorytm.Equals("AlphaBeta"))
                                {
                                    if (value.maxValue > beta)
                                    {
                                        return value;
                                    }
                                    if (value.maxValue > alpha)
                                    {
                                        alpha = value.maxValue;
                                    }
                                }
                            }
                            pole.plansza[(int)ruch.miejscePionkaDoUsniecia.x, (int)ruch.miejscePionkaDoUsniecia.y].zajete = true;
                        }
                    }
                    else
                    {
                        ObiektZwracanyPrzezAlfaBeta obiekDoZwrocenia = new ObiektZwracanyPrzezAlfaBeta(funckjaPrzydzielaniaPunktow(pole, kolor, listaRuchow.Count), wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch.miejscePionkaDoPostawienia);
                        obiekDoZwrocenia.iloscPowtorzenRuchu = iloscPowtorzenRuchu;
                        return obiekDoZwrocenia;
                    }
                }
                else
                {
                    if (etapGry.Equals("mlynek"))
                    {
                        List<Punkt> listaRuchow = gra.znajdzPionkiDoZabrania(pole, kolor);
                        if (poziom < this.maxPoziom)
                        {
                            foreach (Punkt ruch in listaRuchow)
                            {
                                pole.plansza[(int)ruch.x, (int)ruch.y].zajete = false;          

                                if (iloscPionkowNieRozdanychPrzeciwnika <= 0)
                                {                                                                                                                                                                                                                       //zmienam to ponizej
                                    ObiektZwracanyPrzezAlfaBeta wynikMax = playMin(alpha, beta, pole, "ruch", poziom, kolor == 1 ? 2 : 1, iloscPionkowNieRozdanychMoich -1 , iloscPionkowNieRozdanychPrzeciwnika, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, wczesniejszyRuch == null ? ruch : wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch == null ? null : wczesniejszyRuch.miejscePionkaDoPostawienia), iloscPowtorzenRuchu);
                                    if (value.maxValue < wynikMax.maxValue)
                                    {
                                        value = wynikMax;
                                    }
                                }
                                else
                                {
                                    ObiektZwracanyPrzezAlfaBeta wynikMax = playMin(alpha, beta, pole, "rozdawaniePionkow", poziom, kolor == 1 ? 2 : 1, iloscPionkowNieRozdanychMoich - 1, iloscPionkowNieRozdanychPrzeciwnika, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, wczesniejszyRuch == null ? ruch : wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch == null ? null : wczesniejszyRuch.miejscePionkaDoPostawienia), iloscPowtorzenRuchu);
                                    if (value.maxValue < wynikMax.maxValue)
                                    {
                                        value = wynikMax;
                                    }
                                }
                                pole.plansza[(int)ruch.x, (int)ruch.y].zajete = true;
                                if (Gra.getInstance().algorytm.Equals("AlphaBeta"))
                                {
                                    if (value.maxValue > beta)
                                    {
                                        return value;
                                    }
                                    if (value.maxValue > alpha)
                                    {
                                        alpha = value.maxValue;
                                    }
                                }
                            }
                        }
                        else
                        {
                            ObiektZwracanyPrzezAlfaBeta obiektDoZwrocenia = new ObiektZwracanyPrzezAlfaBeta(funckjaPrzydzielaniaPunktow(pole, kolor, listaRuchow.Count), wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch.miejscePionkaDoPostawienia);
                            obiektDoZwrocenia.iloscPowtorzenRuchu = iloscPowtorzenRuchu;
                            return obiektDoZwrocenia;
                       }
                    }
                    else
                    {
                        throw new Exception("Bledny etap gry");
                    }
                }
            }
            if(value.miejscePionkaDoPostawienia == null && value.miejscePionkaDoUsuniecia == null)
            {
                return wczesniejszyRuch;
            }else { 
                 return value;              
            }
        }

        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////////////
        /// </summary>
      //  https://github.com/ForbesLindesay/alpha-beta-pruning/blob/master/index.js
        public ObiektZwracanyPrzezAlfaBeta playMin(int alpha, int beta, Pole pole, string etapGry, int poziom, int kolor, int iloscPionkowNieRozdanychMoich, int iloscPionkowNieRozdanychPrzeciwnika, ObiektZwracanyPrzezAlfaBeta wczesniejszyRuch, int iloscPowtorzenRuchu)
        {
            poziom++;
            ObiektZwracanyPrzezAlfaBeta value = new ObiektZwracanyPrzezAlfaBeta(int.MaxValue, null, null);
            List<int> listaWynikow = new List<int>();
            Gra gra = Gra.getInstance();
            if (etapGry.Equals("rozdawaniePionkow"))
            {
                List<Punkt> listaRuchow = gra.znajdzWolneMiejsca(pole);
                if (poziom < this.maxPoziom)
                {
                    foreach (Punkt ruch in listaRuchow)
                    {
                        pole.plansza[(int)ruch.x, (int)ruch.y].zajete = true;         
                        pole.plansza[(int)ruch.x, (int)ruch.y].kolorGracza = kolor;
                        if (Gra.getInstance().powstalMlynek((int)ruch.x, (int)ruch.y, kolor, pole))
                        {
                            ObiektZwracanyPrzezAlfaBeta wynikMax = playMin(alpha, beta, pole, "mlynek", poziom, kolor, iloscPionkowNieRozdanychMoich, iloscPionkowNieRozdanychPrzeciwnika -1, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, wczesniejszyRuch == null ? null : wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch == null ? ruch : wczesniejszyRuch.miejscePionkaDoPostawienia), iloscPowtorzenRuchu);
                            if (value.maxValue > wynikMax.maxValue)
                            {
                                value = wynikMax;
                            }
                        }
                        else
                        {
                            if (iloscPionkowNieRozdanychMoich <= 0)
                            {
                                ObiektZwracanyPrzezAlfaBeta wynikMax = playMax(alpha, beta, pole, "ruch", poziom, kolor == 1 ? 2 : 1, iloscPionkowNieRozdanychMoich, iloscPionkowNieRozdanychPrzeciwnika -1 , new ObiektZwracanyPrzezAlfaBeta(value.maxValue, wczesniejszyRuch == null ? null : wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch == null ? ruch : wczesniejszyRuch.miejscePionkaDoPostawienia), iloscPowtorzenRuchu);
                                if (value.maxValue > wynikMax.maxValue)
                                {
                                    value = wynikMax;
                                }
                            }
                            else
                            {
                                ObiektZwracanyPrzezAlfaBeta wynikMax = playMax(alpha, beta, pole, "rozdawaniePionkow", poziom, kolor == 1 ? 2 : 1, iloscPionkowNieRozdanychMoich, iloscPionkowNieRozdanychPrzeciwnika - 1, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, wczesniejszyRuch == null ? null : wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch == null ? ruch : wczesniejszyRuch.miejscePionkaDoPostawienia), iloscPowtorzenRuchu);
                                if (value.maxValue > wynikMax.maxValue)
                                {
                                    value = wynikMax;
                                }
                            }
                        }
                        pole.plansza[(int)ruch.x, (int)ruch.y].zajete = false;
                        if (Gra.getInstance().algorytm.Equals("AlphaBeta"))
                        {
                            if (value.maxValue < alpha)
                            {
                                return value;

                            }
                            if (value.maxValue < beta)
                            {
                                beta = value.maxValue;
                            }
                        }
                    }
                }
                else
                {
                    ObiektZwracanyPrzezAlfaBeta obiektDoZwrocenia = new ObiektZwracanyPrzezAlfaBeta(funckjaPrzydzielaniaPunktow(pole, kolor, listaRuchow.Count), wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch.miejscePionkaDoPostawienia);
                    obiektDoZwrocenia.iloscPowtorzenRuchu = iloscPowtorzenRuchu;
                    return obiektDoZwrocenia;
                }
            }
            else
            {
                if (etapGry.Equals("ruch"))
                {
                    List<Ruch> listaRuchow = gra.znajdzMozliwePrzesuniecia(kolor, pole);
                    if (poziom < this.maxPoziom)
                    {
                        foreach (Ruch ruch in listaRuchow)
                        {
                            pole.plansza[(int)ruch.miejscePionkaDoUsniecia.x, (int)ruch.miejscePionkaDoUsniecia.y].zajete = false;          

                            foreach (Punkt punkt in ruch.miejscaNaKtoreMozeSieRuszyc)
                            {
                                pole.plansza[(int)punkt.x, (int)punkt.y].zajete = true;          
                                pole.plansza[(int)punkt.x, (int)punkt.y].kolorGracza = kolor;

                                if (Gra.getInstance().powstalMlynek((int)punkt.x, (int)punkt.y, kolor, pole))
                                {
                                    ObiektZwracanyPrzezAlfaBeta wynikMax = playMin(alpha, beta, pole, "mlynek", poziom, kolor, iloscPionkowNieRozdanychMoich, iloscPionkowNieRozdanychPrzeciwnika, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, (wczesniejszyRuch != null && wczesniejszyRuch.miejscePionkaDoUsuniecia != null) ? wczesniejszyRuch.miejscePionkaDoUsuniecia : ruch.miejscePionkaDoUsniecia, wczesniejszyRuch == null ? punkt : wczesniejszyRuch.miejscePionkaDoPostawienia), iloscPowtorzenRuchu);
                                    if (value.maxValue > wynikMax.maxValue) 
                                    {
                                        value = wynikMax;
                                    }
                                }
                                else
                                {
                                    ObiektZwracanyPrzezAlfaBeta wynikMax;
                                    wynikMax = playMax(alpha, beta, pole, "ruch", poziom, kolor == 1 ? 2 : 1, iloscPionkowNieRozdanychMoich, iloscPionkowNieRozdanychPrzeciwnika, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, (wczesniejszyRuch != null && wczesniejszyRuch.miejscePionkaDoUsuniecia != null) ? wczesniejszyRuch.miejscePionkaDoUsuniecia : ruch.miejscePionkaDoUsniecia, wczesniejszyRuch == null ? punkt : wczesniejszyRuch.miejscePionkaDoPostawienia), iloscPowtorzenRuchu);

                                    if (value.maxValue > wynikMax.maxValue)
                                    {
                                        value = wynikMax;
                                    }
                                }
                                pole.plansza[(int)punkt.x, (int)punkt.y].zajete = false;
                                if (Gra.getInstance().algorytm.Equals("AlphaBeta"))
                                {
                                    if (value.maxValue < alpha)
                                    {
                                        return value;

                                    }
                                    if (value.maxValue < beta)
                                    {
                                        beta = value.maxValue;
                                    }
                                }
                            }
                            pole.plansza[(int)ruch.miejscePionkaDoUsniecia.x, (int)ruch.miejscePionkaDoUsniecia.y].zajete = true;
                        }
                    }
                    else
                    {
                        ObiektZwracanyPrzezAlfaBeta obiektDoZwrocenia =  new ObiektZwracanyPrzezAlfaBeta(funckjaPrzydzielaniaPunktow(pole, kolor, listaRuchow.Count), wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch.miejscePionkaDoPostawienia);
                        obiektDoZwrocenia.iloscPowtorzenRuchu = iloscPowtorzenRuchu;
                        return obiektDoZwrocenia;
                    }
                }
                else
                {
                    if (etapGry.Equals("mlynek"))
                    {
                        List<Punkt> listaRuchow = gra.znajdzPionkiDoZabrania(pole, kolor);
                        if (poziom < this.maxPoziom)
                        {
                            foreach (Punkt ruch in listaRuchow)
                            {
                                pole.plansza[(int)ruch.x, (int)ruch.y].zajete = false;       

                                if (iloscPionkowNieRozdanychMoich <= 0)
                                {
                                    ObiektZwracanyPrzezAlfaBeta wynikMax = playMax(alpha, beta, pole, "ruch", poziom, kolor == 1 ? 2 : 1, iloscPionkowNieRozdanychMoich, iloscPionkowNieRozdanychPrzeciwnika, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, wczesniejszyRuch == null ? ruch : wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch == null ? null : wczesniejszyRuch.miejscePionkaDoPostawienia), iloscPowtorzenRuchu);
                                    if (value.maxValue > wynikMax.maxValue)
                                    {
                                        value = wynikMax;
                                    }
                                }
                                else
                                {
                                    ObiektZwracanyPrzezAlfaBeta wynikMax = playMax(alpha, beta, pole, "rozdawaniePionkow", poziom, kolor == 1 ? 2 : 1, iloscPionkowNieRozdanychMoich, iloscPionkowNieRozdanychPrzeciwnika - 1, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, wczesniejszyRuch == null ? ruch : wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch == null ? null : wczesniejszyRuch.miejscePionkaDoPostawienia), iloscPowtorzenRuchu);
                                    if (value.maxValue > wynikMax.maxValue)
                                    {
                                        value = wynikMax;
                                    }
                                }
                                pole.plansza[(int)ruch.x, (int)ruch.y].zajete = true;
                                if (Gra.getInstance().algorytm.Equals("AlphaBeta"))
                                {
                                    if (value.maxValue < alpha)
                                    {
                                        return value;

                                    }
                                    if (value.maxValue < beta)
                                    {
                                        beta = value.maxValue;
                                    }
                                }
                            }
                        }
                        else
                        {
                            ObiektZwracanyPrzezAlfaBeta obiektDoZwrocenia = new ObiektZwracanyPrzezAlfaBeta(funckjaPrzydzielaniaPunktow(pole, kolor, listaRuchow.Count), wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch.miejscePionkaDoPostawienia);
                            obiektDoZwrocenia.iloscPowtorzenRuchu = iloscPowtorzenRuchu;
                            return obiektDoZwrocenia;
                        }
                    }
                    else
                    {
                        throw new Exception("Bledny etap gry");
                    }
                }
            }

            if (value.miejscePionkaDoPostawienia == null && value.miejscePionkaDoUsuniecia == null)
            {
                return wczesniejszyRuch;
            }
            else
            {
                return value;  
            }
        }

        public int getKolorPrzeciwnika(int kolor)
        {
            if (kolor == 1)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }

        public int funckjaPrzydzielaniaPunktow(Pole pole, int kolorGracza, int iloscMozliwychRuchow)
        {
            return przeszukajPole(pole, kolorGracza, iloscMozliwychRuchow);
        }

        public int przeszukajPole(Pole poleDoSprawdzenia, int kolorGracza, int iloscMozliwychRuchow)
        {
            string typGry = "";
            if (kolorGracza == 1)
            {
                typGry = Gra.getInstance().typBialych;
            }
            else
            {
                typGry = Gra.getInstance().typCzarnych;
            }

            int iloscPionkowB = 0;
            int iloscPionkowC = 0;

            int ilosc = 0;
            int punkty = 0;
            int iloscBialychWLini = 0;
            int iloscCzarnychWLini = 0;
            for (int indexW = 0; indexW < 7; indexW++)
            {
                ilosc = 0;
                iloscBialychWLini = 0;
                iloscCzarnychWLini = 0;
                for (int indexKol = 0; indexKol < 7; indexKol++)
                {
                    if (poleDoSprawdzenia.plansza[indexW, indexKol].dopuszczalne && poleDoSprawdzenia.plansza[indexW, indexKol].zajete)
                    {
                        if (poleDoSprawdzenia.plansza[indexW, indexKol].kolorGracza == 1)
                        {
                            iloscPionkowB++;
                            iloscBialychWLini++;
                        }
                        else
                        {
                            iloscPionkowC++;
                            iloscCzarnychWLini++;
                        }

                        ilosc++;
                        if (indexW == 3 && indexW == 3)
                        {
                            switch (typGry)
                            {
                                case "KomputerH1":
                                    punkty += dodajPunktyH1(ilosc, iloscBialychWLini, iloscCzarnychWLini, kolorGracza);
                                    break;
                                case "KomputerH2":
                                    punkty += dodajPunktyH2(ilosc, iloscBialychWLini, iloscCzarnychWLini, kolorGracza);
                                    break;
                                case "KomputerH3":
                                    punkty += dodajPunktyH3(ilosc, iloscBialychWLini, iloscCzarnychWLini, kolorGracza);
                                    break;
                                default:
                                    punkty += 0;
                                    break;
                            }
                            ilosc = 0;
                            iloscBialychWLini = 0;
                            iloscCzarnychWLini = 0;
                        }
                    }
                }
                switch (typGry)
                {
                    case "KomputerH1":
                        punkty += dodajPunktyH1(ilosc, iloscBialychWLini, iloscCzarnychWLini, kolorGracza);
                        break;
                    case "KomputerH2":
                        punkty += dodajPunktyH2(ilosc, iloscBialychWLini, iloscCzarnychWLini, kolorGracza);
                        break;
                    case "KomputerH3":
                        punkty += dodajPunktyH3(ilosc, iloscBialychWLini, iloscCzarnychWLini, kolorGracza);
                        break;
                    default:
                        punkty += 0;
                        break;
                }
            }

            for (int indexW = 0; indexW < 7; indexW++)
            {
                ilosc = 0;
                iloscBialychWLini = 0;
                iloscCzarnychWLini = 0;
                for (int indexKol = 0; indexKol < 7; indexKol++)
                {
                    if (poleDoSprawdzenia.plansza[indexKol, indexW].dopuszczalne && poleDoSprawdzenia.plansza[indexKol, indexW].zajete)
                    {
                        if (poleDoSprawdzenia.plansza[indexW, indexKol].kolorGracza == 1)
                        {
                            iloscBialychWLini++;
                        }
                        else
                        {
                            iloscCzarnychWLini++;
                        }
                        ilosc++;
                        if (indexW == 3 && indexW == 3)
                        {
                            switch (typGry)
                            {
                                case "KomputerH1":
                                    punkty += dodajPunktyH1(ilosc, iloscBialychWLini, iloscCzarnychWLini, kolorGracza);
                                    break;
                                case "KomputerH2":
                                    punkty += dodajPunktyH2(ilosc, iloscBialychWLini, iloscCzarnychWLini, kolorGracza);
                                    break;
                                case "KomputerH3":
                                    punkty += dodajPunktyH3(ilosc, iloscBialychWLini, iloscCzarnychWLini, kolorGracza);
                                    break;
                                default:
                                    punkty += 0;
                                    break;
                            }
                            ilosc = 0;
                            iloscBialychWLini = 0;
                            iloscCzarnychWLini = 0;
                        }
                    }
                }
                switch (typGry)
                {
                    case "KomputerH1":
                        punkty += dodajPunktyH1(ilosc, iloscBialychWLini, iloscCzarnychWLini, kolorGracza);
                        break;
                    case "KomputerH2":
                        punkty += dodajPunktyH2(ilosc, iloscBialychWLini, iloscCzarnychWLini, kolorGracza);
                        break;
                    case "KomputerH3":
                        punkty += dodajPunktyH3(ilosc, iloscBialychWLini, iloscCzarnychWLini, kolorGracza);
                        break;
                    default:
                        punkty += 0;
                        break;
                }
            }



            switch (typGry)
            {
                case "KomputerH1":
                    punkty += H1IloscRuchow(kolorGracza, iloscPionkowC, iloscPionkowB, iloscMozliwychRuchow, poleDoSprawdzenia);
                    break;
                case "KomputerH2":
                    punkty += H2IloscRuchow(kolorGracza, iloscPionkowC, iloscPionkowB, iloscMozliwychRuchow, poleDoSprawdzenia);
                    break;
                case "KomputerH3":
                    punkty += H3IloscRuchow(kolorGracza, iloscPionkowC, iloscPionkowB, iloscMozliwychRuchow, poleDoSprawdzenia);
                    break;
                default:
                    punkty += 0;
                    break;
            }

            return punkty;
        }

        public int dodajPunktyH1(int ilosc, int iloscBialychWLini, int iloscCzarnychWLini, int kolorGracza)
        {
            int punkty = 0;
            if (iloscBialychWLini == 3)
            {
                if (kolorGracza == 1)
                {
                    punkty += 6;
                }
                else
                {
                    punkty -= 6;
                }
            }

            if (iloscBialychWLini == 2 && iloscCzarnychWLini == 1)
            {
                if (kolorGracza == 1)
                {
                    punkty -= 1;
                }
                else
                {
                    punkty += 10;
                }
            }

            if (iloscBialychWLini == 1 && iloscCzarnychWLini == 2)
            {
                if (kolorGracza == 1)
                {
                    punkty += 10;
                }
                else
                {
                    punkty -= 1;
                }
            }

            if (iloscCzarnychWLini == 3)
            {
                if (kolorGracza == 1)
                {
                    punkty -= 6;
                }
                else
                {
                    punkty += 6;
                }
            }
            if (iloscBialychWLini == 2 && iloscCzarnychWLini == 0)
            {
                if (kolorGracza == 1)
                {
                    punkty += 3;
                }
                else
                {
                    punkty -= 3;
                }
            }
            if (iloscBialychWLini == 0 && iloscCzarnychWLini == 2)
            {
                if (kolorGracza == 1)
                {
                    punkty -= 3;
                }
                else
                {
                    punkty += 3;
                }
            }

            return punkty;
        }

        public int dodajPunktyH2(int ilosc, int iloscBialychWLini, int iloscCzarnychWLini, int kolorGracza)
        {
            int punkty = 0;
            if (iloscBialychWLini == 3)
            {
                if (kolorGracza == 1)
                {
                    punkty += 15;
                }
                else
                {
                    punkty -= 20;
                }
            }

            if (iloscBialychWLini == 2 && iloscCzarnychWLini == 1)
            {
                if (kolorGracza == 1)
                {
                    punkty -= 0;
                }
                else
                {
                    punkty += 25;
                }
            }

            if (iloscBialychWLini == 1 && iloscCzarnychWLini == 2)
            {
                if (kolorGracza == 1)
                {
                    punkty += 25;
                }
                else
                {
                    punkty -= 0;
                }
            }

            if (iloscCzarnychWLini == 3)
            {
                if (kolorGracza == 1)
                {
                    punkty -= 20;
                }
                else
                {
                    punkty += 15;
                }
            }
            if (iloscBialychWLini == 2 && iloscCzarnychWLini == 0)
            {
                if (kolorGracza == 1)
                {
                    punkty += 20;
                }
                else
                {
                    punkty -= -20;
                }
            }
            if (iloscBialychWLini == 0 && iloscCzarnychWLini == 2)
            {
                if (kolorGracza == 1)
                {
                    punkty -= 20;
                }
                else
                {
                    punkty += 20;
                }
            }

            return punkty;
        }

        public int dodajPunktyH3(int ilosc, int iloscBialychWLini, int iloscCzarnychWLini, int kolorGracza)
        {
            int punkty = 0;
            if (iloscBialychWLini == 3)
            {
                if (kolorGracza == 1)
                {
                    punkty += 15;
                }
                else
                {
                    punkty -= 5;
                }
            }

            if (iloscBialychWLini == 2 && iloscCzarnychWLini == 1)
            {
                if (kolorGracza == 1)
                {
                    punkty -= 25;
                }
                else
                {
                    punkty += 5;
                }
            }

            if (iloscBialychWLini == 1 && iloscCzarnychWLini == 2)
            {
                if (kolorGracza == 1)
                {
                    punkty += 25;
                }
                else
                {
                    punkty -= 5;
                }
            }

            if (iloscCzarnychWLini == 3)
            {
                if (kolorGracza == 1)
                {
                    punkty -= 15;
                }
                else
                {
                    punkty += 5;
                }
            }
            if (iloscBialychWLini == 2 && iloscCzarnychWLini == 0)
            {
                if (kolorGracza == 1)
                {
                    punkty += 20;
                }
                else
                {
                    punkty -= 3;
                }
            }
            if (iloscBialychWLini == 0 && iloscCzarnychWLini == 2)
            {
                if (kolorGracza == 1)
                {
                    punkty -= 20;
                }
                else
                {
                    punkty += 3;
                }
            }

            return punkty;
        }

        public int H1IloscRuchow(int kolorGracza, int iloscPionkowC, int iloscPionkowB, int iloscMozliwychRuchow, Pole poleDoSprawdzenia)
        {
            var punkty = 0;
            if (kolorGracza == 1)
            {
                if (iloscPionkowB <= 3)
                {
                    punkty -= 100;
                }
                punkty += iloscPionkowB;
                if (iloscPionkowC < 2)
                {
                    punkty += 100;
                }
                if((Gra.getInstance().znajdzMozliwePrzesuniecia(1, poleDoSprawdzenia)).Count < 2)
                {
                    punkty -= 40;
                }
                if ((Gra.getInstance().znajdzMozliwePrzesuniecia(2, poleDoSprawdzenia)).Count < 2)
                {
                    punkty += 40;
                }
            }
            else
            {
                if (iloscPionkowC <= 3)
                {
                    punkty -= 100;
                }
                punkty += iloscPionkowC;
                if (iloscPionkowB < 2)
                {
                    punkty += 100;
                }
                if ((Gra.getInstance().znajdzMozliwePrzesuniecia(1, poleDoSprawdzenia)).Count < 2)
                {
                    punkty += 40;
                }
                if ((Gra.getInstance().znajdzMozliwePrzesuniecia(2, poleDoSprawdzenia)).Count < 2)
                {
                    punkty -= 40;
                }
            }
            return punkty;
        }

        public int H2IloscRuchow(int kolorGracza, int iloscPionkowC, int iloscPionkowB, int iloscMozliwychRuchow, Pole poleDoSprawdzenia)
        {
            var punkty = 0;
            if (kolorGracza == 1)
            {
                if (iloscPionkowB < 3)
                {
                    punkty -= 100;
                }
                if (iloscPionkowB == 3)
                {
                    punkty -= 50;
                }
                punkty += iloscPionkowB;
                if (iloscPionkowC < 3)
                {
                    punkty += 100;
                }
                if (iloscPionkowC == 3)
                {
                    punkty += 50;
                }
                if ((Gra.getInstance().znajdzMozliwePrzesuniecia(1, poleDoSprawdzenia)).Count < 2)
                {
                    punkty -= 40;
                }
                if ((Gra.getInstance().znajdzMozliwePrzesuniecia(2, poleDoSprawdzenia)).Count < 2)
                {
                    punkty += 40;
                }
            }
            else
            {
                if (iloscPionkowB < 3)
                {
                    punkty += 100;
                }
                if (iloscPionkowB == 3)
                {
                    punkty += 50;
                }
                punkty += iloscPionkowB;
                if (iloscPionkowC < 3)
                {
                    punkty -= 100;
                }
                if (iloscPionkowC == 3)
                {
                    punkty -= 50;
                }
                if ((Gra.getInstance().znajdzMozliwePrzesuniecia(1, poleDoSprawdzenia)).Count < 2)
                {
                    punkty += 40;
                }
                if ((Gra.getInstance().znajdzMozliwePrzesuniecia(2, poleDoSprawdzenia)).Count < 2)
                {
                    punkty -= 40;
                }
            }
            return punkty;
        }

        public int H3IloscRuchow(int kolorGracza, int iloscPionkowC, int iloscPionkowB, int iloscMozliwychRuchow, Pole poleDoSprawdzenia)
        {
            var punkty = 0;
            if (kolorGracza == 1)
            {
                if (iloscPionkowB < 3)
                {
                    punkty -= 100;
                }
                if (iloscPionkowB == 3)
                {
                    punkty -= 50;
                }
                punkty += iloscPionkowB;
                if (iloscPionkowC < 3)
                {
                    punkty += 100;
                }
                if (iloscPionkowC == 3)
                {
                    punkty += 50;
                }
                if ((Gra.getInstance().znajdzMozliwePrzesuniecia(1, poleDoSprawdzenia)).Count < 2)
                {
                    punkty -= 40;
                }
                if ((Gra.getInstance().znajdzMozliwePrzesuniecia(2, poleDoSprawdzenia)).Count < 2)
                {
                    punkty += 40;
                }
            }
            else
            {
                if (iloscPionkowB < 3)
                {
                    punkty += 100;
                }
                if (iloscPionkowB == 3)
                {
                    punkty += 50;
                }
                punkty += iloscPionkowB;
                if (iloscPionkowC < 3)
                {
                    punkty -= 100;
                }
                if (iloscPionkowC == 3)
                {
                    punkty -= 50;
                }
                if ((Gra.getInstance().znajdzMozliwePrzesuniecia(1, poleDoSprawdzenia)).Count < 2)
                {
                    punkty += 40;
                }
                if ((Gra.getInstance().znajdzMozliwePrzesuniecia(2, poleDoSprawdzenia)).Count < 2)
                {
                    punkty -= 40;
                }
            }
            return punkty;
        }
    }
}