﻿using MlynekV2.Models;
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
                                ObiektZwracanyPrzezAlfaBeta wynikMax = playMin(alpha, beta, pole, etapGry, poziom, kolor == 1 ? 2 : 1, iloscPionkowNieRozdanychMoich - 1, iloscPionkowNieRozdanychPrzeciwnika, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, wczesniejszyRuch == null ? null : wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch == null ? ruch : wczesniejszyRuch.miejscePionkaDoPostawienia), iloscPowtorzenRuchu);
                                if (value.maxValue < wynikMax.maxValue)
                                {
                                    value = wynikMax;
                                }
                            }
                        }
                        pole.plansza[(int)ruch.x, (int)ruch.y].zajete = false;
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
                else
                {
                    ObiektZwracanyPrzezAlfaBeta obiektDoZwrocenia = new ObiektZwracanyPrzezAlfaBeta(funckjaPrzydzielaniaPunktow(pole, kolor), wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch.miejscePionkaDoPostawienia);
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
                                if (value.maxValue > beta)
                                {
                                    return value;
                                }
                                if (value.maxValue > alpha)
                                {
                                    alpha = value.maxValue;
                                }
                            }
                            pole.plansza[(int)ruch.miejscePionkaDoUsniecia.x, (int)ruch.miejscePionkaDoUsniecia.y].zajete = true;
                        }
                    }
                    else
                    {
                        ObiektZwracanyPrzezAlfaBeta obiekDoZwrocenia = new ObiektZwracanyPrzezAlfaBeta(funckjaPrzydzielaniaPunktow(pole, kolor), wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch.miejscePionkaDoPostawienia);
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
                                    ObiektZwracanyPrzezAlfaBeta wynikMax = playMin(alpha, beta, pole, etapGry, poziom, kolor == 1 ? 2 : 1, iloscPionkowNieRozdanychMoich - 1, iloscPionkowNieRozdanychPrzeciwnika, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, wczesniejszyRuch == null ? ruch : wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch == null ? null : wczesniejszyRuch.miejscePionkaDoPostawienia), iloscPowtorzenRuchu);
                                    if (value.maxValue < wynikMax.maxValue)
                                    {
                                        value = wynikMax;
                                    }
                                }
                                pole.plansza[(int)ruch.x, (int)ruch.y].zajete = true;
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
                        else
                        {
                            ObiektZwracanyPrzezAlfaBeta obiektDoZwrocenia = new ObiektZwracanyPrzezAlfaBeta(funckjaPrzydzielaniaPunktow(pole, kolor), wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch.miejscePionkaDoPostawienia);
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
                                ObiektZwracanyPrzezAlfaBeta wynikMax = playMax(alpha, beta, pole, etapGry, poziom, kolor == 1 ? 2 : 1, iloscPionkowNieRozdanychMoich, iloscPionkowNieRozdanychPrzeciwnika - 1, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, wczesniejszyRuch == null ? null : wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch == null ? ruch : wczesniejszyRuch.miejscePionkaDoPostawienia), iloscPowtorzenRuchu);
                                if (value.maxValue > wynikMax.maxValue)
                                {
                                    value = wynikMax;
                                }
                            }
                        }

                        if (value.maxValue < alpha)
                        {
                            return value;

                        }
                        if (value.maxValue < beta)
                        {
                            beta = value.maxValue;
                        }

                        pole.plansza[(int)ruch.x, (int)ruch.y].zajete = false;         
                    }
                }
                else
                {
                    ObiektZwracanyPrzezAlfaBeta obiektDoZwrocenia = new ObiektZwracanyPrzezAlfaBeta(funckjaPrzydzielaniaPunktow(pole, kolor), wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch.miejscePionkaDoPostawienia);
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
                                    ObiektZwracanyPrzezAlfaBeta wynikMax = playMax(alpha, beta, pole, "mlynek", poziom, kolor, iloscPionkowNieRozdanychMoich, iloscPionkowNieRozdanychPrzeciwnika, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, (wczesniejszyRuch != null && wczesniejszyRuch.miejscePionkaDoUsuniecia != null) ? wczesniejszyRuch.miejscePionkaDoUsuniecia : ruch.miejscePionkaDoUsniecia, wczesniejszyRuch == null ? punkt : wczesniejszyRuch.miejscePionkaDoPostawienia), iloscPowtorzenRuchu);
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

                                if (value.maxValue < alpha)
                                {
                                    return value;

                                }
                                if (value.maxValue < beta)
                                {
                                    beta = value.maxValue;
                                }

                                pole.plansza[(int)punkt.x, (int)punkt.y].zajete = false;
                            }
                            pole.plansza[(int)ruch.miejscePionkaDoUsniecia.x, (int)ruch.miejscePionkaDoUsniecia.y].zajete = true;
                        }
                    }
                    else
                    {
                        ObiektZwracanyPrzezAlfaBeta obiektDoZwrocenia =  new ObiektZwracanyPrzezAlfaBeta(funckjaPrzydzielaniaPunktow(pole, kolor), wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch.miejscePionkaDoPostawienia);
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
                                    ObiektZwracanyPrzezAlfaBeta wynikMax = playMin(alpha, beta, pole, "ruch", poziom, kolor == 1 ? 2 : 1, iloscPionkowNieRozdanychMoich, iloscPionkowNieRozdanychPrzeciwnika, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, wczesniejszyRuch == null ? ruch : wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch == null ? null : wczesniejszyRuch.miejscePionkaDoPostawienia), iloscPowtorzenRuchu);
                                    if (value.maxValue > wynikMax.maxValue)
                                    {
                                        value = wynikMax;
                                    }
                                }
                                else
                                {
                                    ObiektZwracanyPrzezAlfaBeta wynikMax = playMin(alpha, beta, pole, etapGry, poziom, kolor == 1 ? 2 : 1, iloscPionkowNieRozdanychMoich, iloscPionkowNieRozdanychPrzeciwnika - 1, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, wczesniejszyRuch == null ? ruch : wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch == null ? null : wczesniejszyRuch.miejscePionkaDoPostawienia), iloscPowtorzenRuchu);
                                    if (value.maxValue > wynikMax.maxValue)
                                    {
                                        value = wynikMax;
                                    }
                                }

                                if (value.maxValue < alpha)
                                {
                                    return value;

                                }
                                if (value.maxValue < beta)
                                {
                                    beta = value.maxValue;
                                }
                                pole.plansza[(int)ruch.x, (int)ruch.y].zajete = true;        
                            }
                        }
                        else
                        {
                            ObiektZwracanyPrzezAlfaBeta obiektDoZwrocenia = new ObiektZwracanyPrzezAlfaBeta(funckjaPrzydzielaniaPunktow(pole, kolor), wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch.miejscePionkaDoPostawienia);
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

        public int funckjaPrzydzielaniaPunktow(Pole pole, int kolorGracza)
        {
            return przeszukajPole(pole, kolorGracza);
        }

        public int przeszukajPole(Pole poleDoSprawdzenia, int kolorGracza)
        {
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
                            punkty += dodajPunkty(ilosc, iloscBialychWLini, iloscCzarnychWLini, kolorGracza);
                            ilosc = 0;
                            iloscBialychWLini = 0;
                            iloscCzarnychWLini = 0;
                        }
                    }
                }
                punkty += dodajPunkty(ilosc, iloscBialychWLini, iloscCzarnychWLini, kolorGracza);
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
                            punkty += dodajPunkty(ilosc, iloscBialychWLini, iloscCzarnychWLini, kolorGracza);
                            ilosc = 0;
                            iloscBialychWLini = 0;
                            iloscCzarnychWLini = 0;
                        }
                    }
                }
                punkty += dodajPunkty(ilosc, iloscBialychWLini, iloscCzarnychWLini, kolorGracza);
            }

            string typGry = "";
            if (kolorGracza == 1)
            {
                typGry = Gra.getInstance().typBialych;
            }
            else
            {
                typGry = Gra.getInstance().typCzarnych;
            }

            switch (typGry)
            {
                case "KomputerH1":
                    punkty += H1IloscRuchow(kolorGracza, iloscPionkowC, iloscPionkowB);
                    break;
                case "KomputerH2":
                    punkty += H2IloscRuchow(kolorGracza, iloscPionkowC, iloscPionkowB);
                    break;
                case "KomputerH3":
                    punkty += H3IloscRuchow(kolorGracza, iloscPionkowC, iloscPionkowB);
                    break;
                default:
                    punkty += 0;
                    break;
            }

            return punkty;
        }

        public int dodajPunkty(int ilosc, int iloscBialychWLini, int iloscCzarnychWLini, int kolorGracza)
        {
            int mode = 0;
            if (iloscBialychWLini == 3)
            {
                mode = 1;
            }
            else
            {
                if (iloscBialychWLini == 2 && iloscCzarnychWLini == 1)
                {
                    mode = 2;
                }
                else
                {
                    if (iloscBialychWLini == 1 && iloscCzarnychWLini == 2)
                    {
                        mode = 3;
                    }
                    else
                    {
                        if (iloscCzarnychWLini == 3)
                        {
                            mode = 4;
                        }else
                        {
                            if(iloscBialychWLini == 2 && iloscCzarnychWLini == 0)
                            {
                                mode = 5;
                            }else
                            {
                                if (iloscBialychWLini == 0 && iloscCzarnychWLini == 2)
                                {
                                    mode = 6;
                                }
                            }
                        }
                    }
                }     
            }

            string typGry = "";
            if (kolorGracza == 1)
            {
                 typGry = Gra.getInstance().typBialych;
            }
            else
            {
                 typGry = Gra.getInstance().typCzarnych;
            }

            switch (typGry)
            {
                case "KomputerH1":
                    return H1(mode, kolorGracza);
                case "KomputerH2":
                    return H2(mode, kolorGracza);
                case "KomputerH3":
                    return H2(mode, kolorGracza);
                default:
                    return 0;
            }

        }

        public int H1IloscRuchow(int kolorGracza, int iloscPionkowC, int iloscPionkowB)
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
            }
            return punkty;
        }

        public int H2IloscRuchow(int kolorGracza, int iloscPionkowC, int iloscPionkowB)
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
            }
            return punkty;
        }

        public int H3IloscRuchow(int kolorGracza, int iloscPionkowC, int iloscPionkowB)
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
            }
            return punkty;
        }

        public int H1(int mode, int kolorGracza)
        {
            var punkty = 0;
            switch (mode)
            {
                case 1:
                    if (kolorGracza == 1)
                    {
                        punkty = 6;
                    }
                    else
                    {
                        punkty = -5;
                    }
                    break;
                case 2:
                    if (kolorGracza == 1)
                    {
                        punkty = -1;
                    }
                    else
                    {
                        punkty = 10;
                    }
                    break;
                case 3:
                    if (kolorGracza == 1)
                    {
                        punkty = 10;
                    }
                    else
                    {
                        punkty = -1;
                    }
                    break;
                case 4:
                    if (kolorGracza == 1)
                    {
                        punkty = -5;
                    }
                    else
                    {
                        punkty = 6;
                    }
                    break;
                case 5:
                    if (kolorGracza == 1)
                    {
                        punkty = 3;
                    }
                    else
                    {
                        punkty = -3;
                    }
                    break;
                case 6:
                    if (kolorGracza == 1)
                    {
                        punkty = -3;
                    }
                    else
                    {
                        punkty = 3;
                    }
                    break;
                default:
                    return 0;
            }
            return punkty;
        }

        public int H2(int mode, int kolorGracza)
        {
            var punkty = 0;
            switch (mode)
            {
                case 1:
                    if (kolorGracza == 1)
                    {
                        punkty = 6;
                    }
                    else
                    {
                        punkty = -5;
                    }
                    break;
                case 2:
                    if (kolorGracza == 1)
                    {
                        punkty = -1;
                    }
                    else
                    {
                        punkty = 10;
                    }
                    break;
                case 3:
                    if (kolorGracza == 1)
                    {
                        punkty = 10;
                    }
                    else
                    {
                        punkty = -1;
                    }
                    break;
                case 4:
                    if (kolorGracza == 1)
                    {
                        punkty = -5;
                    }
                    else
                    {
                        punkty = 6;
                    }
                    break;
                case 5:
                    if (kolorGracza == 1)
                    {
                        punkty = 3;
                    }
                    else
                    {
                        punkty = -3;
                    }
                    break;
                case 6:
                    if (kolorGracza == 1)
                    {
                        punkty = -3;
                    }
                    else
                    {
                        punkty = 3;
                    }
                    break;
                default:
                    return 0;
            }
            return punkty;

        }

        public int H3(int mode, int kolorGracza)
        {
            var punkty = 0;
            switch (mode)
            {
                case 1:
                    if (kolorGracza == 1)
                    {
                        punkty = 6;
                    }
                    else
                    {
                        punkty = -5;
                    }
                    break;
                case 2:
                    if (kolorGracza == 1)
                    {
                        punkty = -1;
                    }
                    else
                    {
                        punkty = 10;
                    }
                    break;
                case 3:
                    if (kolorGracza == 1)
                    {
                        punkty = 10;
                    }
                    else
                    {
                        punkty = -1;
                    }
                    break;
                case 4:
                    if (kolorGracza == 1)
                    {
                        punkty = -5;
                    }
                    else
                    {
                        punkty = 6;
                    }
                    break;
                case 5:
                    if (kolorGracza == 1)
                    {
                        punkty = 3;
                    }
                    else
                    {
                        punkty = -3;
                    }
                    break;
                case 6:
                    if (kolorGracza == 1)
                    {
                        punkty = -3;
                    }
                    else
                    {
                        punkty = 3;
                    }
                    break;
                default:
                    return 0;
            }
            return punkty;

        }

    }

}