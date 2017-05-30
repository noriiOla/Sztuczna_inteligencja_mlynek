using MlynekV2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MlynekV2.Services
{
    public class AlphaBeta
    {
        int maxPoziom;
        public Pole pole;

        public AlphaBeta(Pole pole)
        {
            maxPoziom = 4;
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

        public ObiektZwracanyPrzezAlfaBeta play(Pole pole, string etapGry, int kolor, int iloscPionkowNieRozdanychCzarnych)     // u nas zawsze zaczynamy od max, a przeciwnik musi miec min
        {
            int poziom = 0;
            ObiektZwracanyPrzezAlfaBeta wynik = playMax(int.MinValue, int.MaxValue, this.pole, etapGry, poziom, kolor, iloscPionkowNieRozdanychCzarnych, null);
            return wynik;
        }

        public ObiektZwracanyPrzezAlfaBeta playMax(int alpha, int beta, Pole pole, string etapGry, int poziom, int kolor, int iloscPionkowNieRozdanychCzarnych, ObiektZwracanyPrzezAlfaBeta wczesniejszyRuch)
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
                            // ObiektZwracanyPrzezAlfaBeta wynikMax = playMax(alpha, beta, pole, "mlynek", poziom, kolor, iloscPionkowNieRozdanychCzarnych - 1, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, wczesniejszyRuch== null ? null: wczesniejszyRuch.miejscePionkaDoUsuniecia, ruch));
                            ObiektZwracanyPrzezAlfaBeta wynikMax = playMax(alpha, beta, pole, "mlynek", poziom, kolor, iloscPionkowNieRozdanychCzarnych - 1, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, wczesniejszyRuch == null ? null : wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch == null ? ruch : wczesniejszyRuch.miejscePionkaDoPostawienia));

                            if (value.maxValue < wynikMax.maxValue)
                            {
                                value = wynikMax;
                            }
                        }
                        else
                        {
                            if (iloscPionkowNieRozdanychCzarnych - 1 == 0)
                            {
                                ObiektZwracanyPrzezAlfaBeta wynikMax = playMin(alpha, beta, pole, "ruch", poziom, kolor == 1 ? 2 : 1, iloscPionkowNieRozdanychCzarnych, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, wczesniejszyRuch == null ? null : wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch == null ? ruch : wczesniejszyRuch.miejscePionkaDoPostawienia));
                                if (value.maxValue < wynikMax.maxValue)
                                {
                                    value = wynikMax;
                                }
                            }
                            else
                            {
                                ObiektZwracanyPrzezAlfaBeta wynikMax = playMin(alpha, beta, pole, etapGry, poziom, kolor == 1 ? 2 : 1, iloscPionkowNieRozdanychCzarnych - 1, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, wczesniejszyRuch == null ? null : wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch == null ? ruch : wczesniejszyRuch.miejscePionkaDoPostawienia));
                                if (value.maxValue < wynikMax.maxValue)
                                {
                                    value = wynikMax;
                                }
                            }
                        }

                        if (value.maxValue > beta)
                        {
                            return value;
                        }
                        if (value.maxValue > alpha)
                        {
                            alpha = value.maxValue;
                        }
                        pole.plansza[(int)ruch.x, (int)ruch.y].zajete = false;
                    }
                }
                else
                {
                    return new ObiektZwracanyPrzezAlfaBeta(funckjaH1(pole, kolor), wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch.miejscePionkaDoPostawienia);
                }
            }
            else
            {
                if (etapGry.Equals("ruch"))
                {
                    List<Ruch> listaRuchow = gra.znajdzMiejscaDostepne(kolor, pole);
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
                                    ObiektZwracanyPrzezAlfaBeta wynikMax = playMax(alpha, beta, pole, "mlynek", poziom, kolor, iloscPionkowNieRozdanychCzarnych - 1, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, (wczesniejszyRuch != null && wczesniejszyRuch.miejscePionkaDoUsuniecia != null) ? wczesniejszyRuch.miejscePionkaDoUsuniecia : ruch.miejscePionkaDoUsniecia, wczesniejszyRuch == null ? punkt : wczesniejszyRuch.miejscePionkaDoPostawienia));
                                    if (value.maxValue < wynikMax.maxValue)
                                    {
                                        value = wynikMax;
                                    }
                                }
                                else
                                {
                                    if (iloscPionkowNieRozdanychCzarnych - 1 == 0)
                                    {
                                        ObiektZwracanyPrzezAlfaBeta wynikMax = playMin(alpha, beta, pole, "ruch", poziom, kolor == 1 ? 2 : 1, iloscPionkowNieRozdanychCzarnych, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, (wczesniejszyRuch != null && wczesniejszyRuch.miejscePionkaDoUsuniecia != null) ? wczesniejszyRuch.miejscePionkaDoUsuniecia : ruch.miejscePionkaDoUsniecia, wczesniejszyRuch == null ? punkt : wczesniejszyRuch.miejscePionkaDoPostawienia));
                                        if (value.maxValue < wynikMax.maxValue)
                                        {
                                            value = wynikMax;
                                        }
                                    }
                                    else
                                    {
                                        ObiektZwracanyPrzezAlfaBeta wynikMax = playMin(alpha, beta, pole, etapGry, poziom, kolor == 1 ? 2 : 1, iloscPionkowNieRozdanychCzarnych - 1, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, (wczesniejszyRuch != null && wczesniejszyRuch.miejscePionkaDoUsuniecia != null) ? wczesniejszyRuch.miejscePionkaDoUsuniecia : ruch.miejscePionkaDoUsniecia, wczesniejszyRuch == null ? punkt : wczesniejszyRuch.miejscePionkaDoPostawienia));
                                        if (value.maxValue < wynikMax.maxValue)
                                        {
                                            value = wynikMax;
                                        }
                                    }
                                }

                                if (value.maxValue > beta)
                                {
                                    return value;
                                }
                                if (value.maxValue > alpha)
                                {
                                    alpha = value.maxValue;
                                }
                                pole.plansza[(int)punkt.x, (int)punkt.y].zajete = false;
                            }
                            pole.plansza[(int)ruch.miejscePionkaDoUsniecia.x, (int)ruch.miejscePionkaDoUsniecia.y].zajete = true;
                            // w js potem nadaj temu nazwe pionka i wyslij info do c#
                        }
                    }
                    else
                    {
                        return new ObiektZwracanyPrzezAlfaBeta(funckjaH1(pole, kolor), wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch.miejscePionkaDoPostawienia);
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
                                pole.plansza[(int)ruch.x, (int)ruch.y].zajete = false;          // w js potem nadaj temu nazwe pionka i wyslij info do c#

                                if (iloscPionkowNieRozdanychCzarnych - 1 == 0)
                                {                                                                                                                                                                                                                       //zmienam to ponizej
                                    ObiektZwracanyPrzezAlfaBeta wynikMax = playMin(alpha, beta, pole, "ruch", poziom, kolor == 1 ? 2 : 1, iloscPionkowNieRozdanychCzarnych, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, wczesniejszyRuch == null ? ruch : wczesniejszyRuch.miejscePionkaDoPostawienia, wczesniejszyRuch == null ? null : wczesniejszyRuch.miejscePionkaDoPostawienia));
                                    if (value.maxValue < wynikMax.maxValue)
                                    {
                                        value = wynikMax;
                                    }
                                }
                                else
                                {
                                    ObiektZwracanyPrzezAlfaBeta wynikMax = playMin(alpha, beta, pole, etapGry, poziom, kolor == 1 ? 2 : 1, iloscPionkowNieRozdanychCzarnych - 1, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, wczesniejszyRuch == null ? ruch : wczesniejszyRuch.miejscePionkaDoPostawienia, wczesniejszyRuch == null ? null : wczesniejszyRuch.miejscePionkaDoPostawienia));
                                    if (value.maxValue < wynikMax.maxValue)
                                    {
                                        value = wynikMax;
                                    }
                                }

                                if (value.maxValue > beta)
                                {
                                    return value;
                                }
                                if (value.maxValue > alpha)
                                {
                                    alpha = value.maxValue;
                                }
                                pole.plansza[(int)ruch.x, (int)ruch.y].zajete = true;          // w js potem nadaj temu nazwe pionka i wyslij info do c#
                            }
                        }
                        else
                        {
                            return new ObiektZwracanyPrzezAlfaBeta(funckjaH1(pole, kolor), wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch.miejscePionkaDoPostawienia);
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
                 return value;               //// usun potem
            }
        }

        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        /// <param name="pole"></param>
        /// <param name="etapGry"></param>
        /// <param name="poziom"></param>
        /// <param name="kolor"></param>
        /// <param name="iloscPionkowNieRozdanychCzarnych"></param>
        /// <param name="wczesniejszyRuch"></param>
        /// <returns></returns>
      //  https://github.com/ForbesLindesay/alpha-beta-pruning/blob/master/index.js
        public ObiektZwracanyPrzezAlfaBeta playMin(int alpha, int beta, Pole pole, string etapGry, int poziom, int kolor, int iloscPionkowNieRozdanychCzarnych, ObiektZwracanyPrzezAlfaBeta wczesniejszyRuch)
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
                        pole.plansza[(int)ruch.x, (int)ruch.y].zajete = true;          // w js potem nadaj temu nazwe pionka i wyslij info do c#
                        pole.plansza[(int)ruch.x, (int)ruch.y].kolorGracza = kolor;
                        if (Gra.getInstance().powstalMlynek((int)ruch.x, (int)ruch.y, kolor, pole))
                        {
                            ObiektZwracanyPrzezAlfaBeta wynikMax = playMin(alpha, beta, pole, "mlynek", poziom, kolor, iloscPionkowNieRozdanychCzarnych - 1, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, wczesniejszyRuch == null ? null : wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch == null ? ruch : wczesniejszyRuch.miejscePionkaDoPostawienia));
                            if (value.maxValue > wynikMax.maxValue)
                            {
                                value = wynikMax;
                            }
                        }
                        else
                        {
                            if (iloscPionkowNieRozdanychCzarnych - 1 == 0)
                            {
                                ObiektZwracanyPrzezAlfaBeta wynikMax = playMax(alpha, beta, pole, "ruch", poziom, kolor == 1 ? 2 : 1, iloscPionkowNieRozdanychCzarnych, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, wczesniejszyRuch == null ? null : wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch == null ? ruch : wczesniejszyRuch.miejscePionkaDoPostawienia));
                                if (value.maxValue > wynikMax.maxValue)
                                {
                                    value = wynikMax;
                                }
                            }
                            else
                            {
                                ObiektZwracanyPrzezAlfaBeta wynikMax = playMax(alpha, beta, pole, etapGry, poziom, kolor == 1 ? 2 : 1, iloscPionkowNieRozdanychCzarnych - 1, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, wczesniejszyRuch == null ? null : wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch == null ? ruch : wczesniejszyRuch.miejscePionkaDoPostawienia));
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

                        pole.plansza[(int)ruch.x, (int)ruch.y].zajete = false;          // w js potem nadaj temu nazwe pionka i wyslij info do c#
                    }
                }
                else
                {
                    return new ObiektZwracanyPrzezAlfaBeta(funckjaH1(pole, kolor), wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch.miejscePionkaDoPostawienia);
                }
            }
            else
            {
                if (etapGry.Equals("ruch"))
                {
                    List<Ruch> listaRuchow = gra.znajdzMiejscaDostepne(kolor, pole);
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
                                    ObiektZwracanyPrzezAlfaBeta wynikMax = playMax(alpha, beta, pole, "mlynek", poziom, kolor, iloscPionkowNieRozdanychCzarnych, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, (wczesniejszyRuch != null && wczesniejszyRuch.miejscePionkaDoUsuniecia != null) ? wczesniejszyRuch.miejscePionkaDoUsuniecia : ruch.miejscePionkaDoUsniecia, wczesniejszyRuch == null ? punkt : wczesniejszyRuch.miejscePionkaDoPostawienia));
                                    if (value.maxValue > wynikMax.maxValue) 
                                    {
                                        value = wynikMax;
                                    }
                                }
                                else
                                {
                                    if (iloscPionkowNieRozdanychCzarnych - 1 == 0)
                                    {
                                        ObiektZwracanyPrzezAlfaBeta wynikMax = playMin(alpha, beta, pole, "ruch", poziom, kolor == 1 ? 2 : 1, iloscPionkowNieRozdanychCzarnych, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, (wczesniejszyRuch != null && wczesniejszyRuch.miejscePionkaDoUsuniecia != null) ? wczesniejszyRuch.miejscePionkaDoUsuniecia : ruch.miejscePionkaDoUsniecia, wczesniejszyRuch == null ? punkt : wczesniejszyRuch.miejscePionkaDoPostawienia));
                                        if (value.maxValue > wynikMax.maxValue)
                                        {
                                            value = wynikMax;
                                        }
                                    }
                                    else
                                    {
                                        ObiektZwracanyPrzezAlfaBeta wynikMax = playMin(alpha, beta, pole, etapGry, poziom, kolor == 1 ? 2 : 1, iloscPionkowNieRozdanychCzarnych, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, (wczesniejszyRuch != null && wczesniejszyRuch.miejscePionkaDoUsuniecia != null) ? wczesniejszyRuch.miejscePionkaDoUsuniecia : ruch.miejscePionkaDoUsniecia, wczesniejszyRuch == null ? punkt : wczesniejszyRuch.miejscePionkaDoPostawienia));
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

                                pole.plansza[(int)punkt.x, (int)punkt.y].zajete = false;
                            }
                            pole.plansza[(int)ruch.miejscePionkaDoUsniecia.x, (int)ruch.miejscePionkaDoUsniecia.y].zajete = true;
                            // w js potem nadaj temu nazwe pionka i wyslij info do c#
                        }
                    }
                    else
                    {
                        return new ObiektZwracanyPrzezAlfaBeta(funckjaH1(pole, kolor), wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch.miejscePionkaDoPostawienia);
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
                                pole.plansza[(int)ruch.x, (int)ruch.y].zajete = false;          // w js potem nadaj temu nazwe pionka i wyslij info do c#

                                if (iloscPionkowNieRozdanychCzarnych - 1 == 0)
                                {
                                    ObiektZwracanyPrzezAlfaBeta wynikMax = playMin(alpha, beta, pole, "ruch", poziom, kolor == 1 ? 2 : 1, iloscPionkowNieRozdanychCzarnych, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, wczesniejszyRuch == null ? ruch : wczesniejszyRuch.miejscePionkaDoPostawienia, wczesniejszyRuch == null ? null : wczesniejszyRuch.miejscePionkaDoPostawienia));
                                    if (value.maxValue > wynikMax.maxValue)
                                    {
                                        value = wynikMax;
                                    }
                                }
                                else
                                {
                                    ObiektZwracanyPrzezAlfaBeta wynikMax = playMin(alpha, beta, pole, etapGry, poziom, kolor == 1 ? 2 : 1, iloscPionkowNieRozdanychCzarnych - 1, new ObiektZwracanyPrzezAlfaBeta(value.maxValue, wczesniejszyRuch == null ? ruch : wczesniejszyRuch.miejscePionkaDoPostawienia, wczesniejszyRuch == null ? null : wczesniejszyRuch.miejscePionkaDoPostawienia));
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
                                pole.plansza[(int)ruch.x, (int)ruch.y].zajete = true;          // w js potem nadaj temu nazwe pionka i wyslij info do c#
                            }
                        }
                        else
                        {
                            return new ObiektZwracanyPrzezAlfaBeta(funckjaH1(pole, kolor), wczesniejszyRuch.miejscePionkaDoUsuniecia, wczesniejszyRuch.miejscePionkaDoPostawienia);
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
                return value;               //// usun potem
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

        public int funckjaH1(Pole pole, int kolorGracza)
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
                            punkty += dodajPunktyH1(ilosc, iloscBialychWLini, iloscCzarnychWLini, kolorGracza);
                            ilosc = 0;
                            iloscBialychWLini = 0;
                            iloscCzarnychWLini = 0;
                        }
                    }
                }
                punkty += dodajPunktyH1(ilosc, iloscBialychWLini, iloscCzarnychWLini, kolorGracza);
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
                            punkty += dodajPunktyH1(ilosc, iloscBialychWLini, iloscCzarnychWLini, kolorGracza);
                            ilosc = 0;
                            iloscBialychWLini = 0;
                            iloscCzarnychWLini = 0;
                        }
                    }
                }
                punkty += dodajPunktyH1(ilosc, iloscBialychWLini, iloscCzarnychWLini, kolorGracza);
            }

            if (kolorGracza == 1)
            {
                punkty += iloscPionkowB;
            }
            else
            {
                punkty += iloscPionkowC;
            }
            if (punkty != 1)
            {
                int i = 2;
            }

            return punkty;
        }

        public int dodajPunktyH1(int ilosc, int iloscBialychWLini, int iloscCzarnychWLini, int kolorGracza)
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
            return ustalPunktyWZaleznosciOdGracza(mode, kolorGracza);
        }

        public int ustalPunktyWZaleznosciOdGracza(int mode, int kolorGracza)
        {
            switch (mode)
            {
                case 1:
                    if(kolorGracza == 1)
                    {
                        return 4;
                    }else
                    {
                        return -3;
                    }
                case 2:
                    if (kolorGracza == 1)
                    {
                        return -1;
                    }
                    else
                    {
                        return 10;
                    }
                    break;
                case 3:
                    if (kolorGracza == 1)
                    {
                        return 3;
                    }
                    else
                    {
                        return -1;
                    }
                    break;
                case 4:
                    if (kolorGracza == 1)
                    {
                        return -3;
                    }
                    else
                    {
                        return 4;
                    }
                    break;
                case 5:
                    if (kolorGracza == 1)
                    {
                        return 3;
                    }
                    else
                    {
                        return -3;
                    }
                    break;
                case 6:
                    if (kolorGracza == 1)
                    {
                        return -3;
                    }
                    else
                    {
                        return 3;
                    }
                    break;
                default:
                    return 0;

            }
        }
    }

}