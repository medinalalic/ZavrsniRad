using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZavršniRad.Areas.ModulAdministrator.Models;
using ZavršniRad.DAL;
using ZavršniRad.Helper;
using ZavršniRad.Models;

namespace ZavršniRad.Areas.ModulAdministrator.Controllers
{
    [Autorizacija(KorisnickeUloge.Administrator)]
    public class ProfilController : Controller
    {
        // GET: ModulAdministrator/Profil
        MojContext ctx = new MojContext();
        public ActionResult IzmjenaPristupnihPodataka()
        {
            Korisnik lp = (Korisnik)ControllerContext.HttpContext.Session["logirani_korisnik"];
            Osoblje p = ctx.Osoblje.Where(x => x.Id == lp.Id && lp.IsAdmin==true).FirstOrDefault();

            var Model = new UrediProfilVM();

            Model.Lozinka = lp.Lozinka;

            return View(Model);
        }

        public ActionResult SnimiPristupniPodaci(UrediProfilVM model)
        {

            Korisnik lp = (Korisnik)ControllerContext.HttpContext.Session["logirani_korisnik"];
            Osoblje DBAdmin = ctx.Osoblje.Where(x => x.Id == lp.Id && lp.IsAdmin==true).Include(x => x.Korisnik).FirstOrDefault();

            DBAdmin.Korisnik.Lozinka = model.Lozinka;
            DBAdmin.Korisnik.LozinkaSalt = UIHelper.GenerateSalt();
            DBAdmin.Korisnik.LozinkaHash = UIHelper.GenerateHash(model.Lozinka, DBAdmin.Korisnik.LozinkaSalt);

            ctx.SaveChanges();
            TempData["Success"] = "Uspješno sačuvano !";
            return RedirectToAction("IzmjenaPristupnihPodataka");
        }
    }
}