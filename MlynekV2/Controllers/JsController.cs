
using MlynekV2.Models;
using MlynekV2.Services;
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

        public string obsluzRuch(double xNowe, double yNowe, int kolorGracza, double xStare, double yStare,string nazwaPionka)
        {
            //0, 0- niepoprawny ruch
            //x,y - poprawny ruch- miejsce gdzie nalezy postawic pionek
            Punkt nowyPunkt = new Punkt(xNowe, yNowe);
            Punkt staryPunkt = new Punkt(xStare, yStare);
           WynikRuchu p = Gra.getInstance().obsluzRuch(nowyPunkt, kolorGracza, staryPunkt, nazwaPionka);
           return new JsonTransformService().SerializeToString(p);
        }

        public string usunPionek(double x, double y)
        {
            Gra.getInstance().usunZPlanszyInformacjeOStarymPunkcie(new Punkt(x, y));
            return  new JsonTransformService().SerializeToString(new Komunikat("c# usnuniete"));
        }
    }
}