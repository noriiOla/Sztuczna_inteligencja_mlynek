
using MlynekV2.Models;
using MlynekV2.Services;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MlynekV2.Controllers
{
    public class JsController : Controller
    {
        // GET: Js
        public ActionResult Index()
        {
            return View();
        }

        public void inicjuj(int gracz1, int gracz2)
        {
            Gra.getInstance().init(gracz1, gracz2);
        }

        public Gracz getGracz(string kolor)
        {
            if(Convert.ToInt32(kolor) == 1)
            {
                return Gra.getInstance().gracz1;
            }else
            {
                return Gra.getInstance().gracz2;
            }
        }

        public string obsluzRuch(double xNowe, double yNowe, int kolorGracza, double xStare, double yStare,string nazwaPionka)
        {
            //0, 0- niepoprawny ruch
            //x,y - poprawny ruch- miejsce gdzie nalezy postawic pionek
            Punkt nowyPunkt = new Punkt(xNowe, yNowe);
            Punkt staryPunkt = new Punkt(xStare, yStare);
           WynikRuchu p = Gra.getInstance().obsluzRuch(nowyPunkt, getGracz(kolorGracza.ToString()), staryPunkt, nazwaPionka);
           return new JsonTransformService().SerializeToString(p);
        }

        public string usunPionek(double x, double y)
        {
            Gra.getInstance().usunZPlanszyInformacjeOStarymPunkcie(new Punkt(x, y));
            return  new JsonTransformService().SerializeToString(new Komunikat("c# usnuniete"));
        }

        public string znajdzRuchy(string kolor)
        {
            List<Ruch> listaRuchow=  Gra.getInstance().znajdzMiejscaDostepne(Convert.ToInt32(kolor), Gra.getInstance().pole);
            return new JsonTransformService().SerializeToString(listaRuchow);
        }

        public string kompZnajdzRuch(string kolor, bool jestMlynek)
        {
            ObiektZwracanyPrzezAlfaBeta ruchKomp = Gra.getInstance().kompZnajdzNajlepszeMiejsceIPionek(getGracz(kolor), jestMlynek);
            return new JsonTransformService().SerializeToString(ruchKomp);
        }
    }
}