
using ZavršniRad.Models;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using ZavršniRad.Helper;
using ZavršniRad.DAL;

namespace Si_zadatak5.Controllers
{
    public class LoginController : Controller
    {
        private MojContext ctx = new MojContext();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
       

        public ActionResult Provjera(string KorisnickoIme, string Lozinka, string zapamti)
        {

            Korisnik k = ctx.Korisnik.Include(x => x.Stomatolog)
                .Include(x => x.Pacijent)
                .Include(x => x.Osoblje)
                .SingleOrDefault(x => x.KorisnickoIme == KorisnickoIme && x.Lozinka == Lozinka && x.Aktivan == true);
            if (k == null)//zbog ovoga stalno ucitava login a ne pocetnu
            {
                
                return Redirect("/Login");
            }
            //// if(k.isPacijent)
            //{
            //    Autentifikacija.PokreniNovuSesiju(k, HttpContext, (zapamti == "on"));
            //    return Redirect("/HomePacijent");
            //}
            //if (k.IsAdmin)
            //{
            //    Autentifikacija.PokreniNovuSesiju(k, HttpContext, (zapamti == "on"));
            //    return Redirect("/HomeAdmin");
            //}
            Autentifikacija.PokreniNovuSesiju(k, HttpContext, (zapamti == "on"));
            return Redirect("/Home/Pocetna");
        }

        public ActionResult Logout()
        {
            Autentifikacija.PokreniNovuSesiju(null, HttpContext, true);
            return Redirect("/Home/Index");
        }
       
    }
}