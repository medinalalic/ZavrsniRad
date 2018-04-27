using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZavršniRad.Areas.ModulPacijent.Models;
using ZavršniRad.DAL;
using ZavršniRad.Helper;
using ZavršniRad.Models;

namespace ZavršniRad.Areas.ModulPacijent.Controllers
{
    [Autorizacija(KorisnickeUloge.Pacijent)]

    public class ProfilController : Controller
    {
        // GET: ModulPacijent/Profil
        MojContext ctx = new MojContext();
        
       
        public ActionResult IzmjenaPristupnihPodataka()
        {
            Korisnik lp = (Korisnik)ControllerContext.HttpContext.Session["logirani_korisnik"];
            Pacijent p = ctx.Pacijent.Where(x => x.Id == lp.Id).FirstOrDefault();

            var Model = new UrediProfilVM();

            Model.Lozinka = lp.Lozinka;

            return View(Model);
        }

        public ActionResult SnimiPristupniPodaci(UrediProfilVM model)
        {

            Korisnik lp = (Korisnik)ControllerContext.HttpContext.Session["logirani_korisnik"];
            Pacijent DBPacijent = ctx.Pacijent.Where(x => x.Id == lp.Id).Include(x=>x.Korisnik).FirstOrDefault();

            DBPacijent.Korisnik.Lozinka = model.Lozinka;
            DBPacijent.Korisnik.LozinkaSalt = UIHelper.GenerateSalt();
            DBPacijent.Korisnik.LozinkaHash = UIHelper.GenerateHash(model.Lozinka, DBPacijent.Korisnik.LozinkaSalt);

            ctx.SaveChanges();
            TempData["Success"] = "Uspijesno sačuvano !";
            return RedirectToAction("IzmjenaPristupnihPodataka");
        }
    }
}