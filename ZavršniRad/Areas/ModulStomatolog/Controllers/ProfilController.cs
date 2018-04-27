using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZavršniRad.Areas.ModulStomatolog.Models;
using ZavršniRad.DAL;
using ZavršniRad.Helper;
using ZavršniRad.Models;

namespace ZavršniRad.Areas.ModulStomatolog.Controllers
{
    [Autorizacija(KorisnickeUloge.Stomatolog)]

    public class ProfilController : Controller
    {
        // GET: ModulStomatolog/Profil
        MojContext ctx = new MojContext();


        public ActionResult IzmjenaPristupnihPodataka()
        {
            Korisnik lp = (Korisnik)ControllerContext.HttpContext.Session["logirani_korisnik"];
            Stomatolog p = ctx.Stomatolog.Where(x => x.Id == lp.Id).FirstOrDefault();

            var Model = new UrediProfilVM();

            Model.Lozinka = lp.Lozinka;

            return View(Model);
        }

        public ActionResult SnimiPristupniPodaci(UrediProfilVM model)
        {

            Korisnik lp = (Korisnik)ControllerContext.HttpContext.Session["logirani_korisnik"];
            Stomatolog DBStomatolog = ctx.Stomatolog.Where(x => x.Id == lp.Id).Include(x => x.Korisnik).FirstOrDefault();

            DBStomatolog.Korisnik.Lozinka = model.Lozinka;
            DBStomatolog.Korisnik.LozinkaSalt = UIHelper.GenerateSalt();
            DBStomatolog.Korisnik.LozinkaHash = UIHelper.GenerateHash(model.Lozinka, DBStomatolog.Korisnik.LozinkaSalt);

            ctx.SaveChanges();
            TempData["Success"] = "Uspješno sačuvano !";
            return RedirectToAction("IzmjenaPristupnihPodataka");
        }
    }
}