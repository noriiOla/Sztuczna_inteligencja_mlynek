
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

        public string obsluzRuch(double x, double y, int kolorGracza)
        {
            //0, 0- niepoprawny ruch
            //x,y - poprawny ruch- miejsce gdzie nalezy postawic pionek
           Punkt p = Gra.getInstance().obsluzRuch(x, y, kolorGracza);
           return new JsonTransformService().SerializeToString(p);
        }
    }
}