using System;
using System.Collections.Generic;
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

    public class PregledController : Controller
    {
        // GET: ModulStomatolog/Pregled
        MojContext ctx = new MojContext();
        public ActionResult Index(int id,int pacijentId)
        {
            PregledVM model = new PregledVM();
            Pregled p = ctx.Pregled.Where(c => c.TerminId == id && c.PacijentId == pacijentId).FirstOrDefault();
            model.DatumPregleda = p.DatumPregleda;
            model.VrijemePregleda = p.VrijemePregleda;
            model.Id = p.Id;
            IzvrsenaUsluga u = ctx.IzvrsenaUsluga.Where(c => c.PregledId == p.Id).FirstOrDefault();
            model.CijenaUsluge = u.Cijena;
            Usluga uu = ctx.Usluga.Where(c => c.Id == u.UslugaId).FirstOrDefault();
            model.NazivUsluge = uu.Vrsta;
            Zub z = ctx.Zub.Where(c => c.Id == u.ZubId).FirstOrDefault();
            model.NazivZuba = z.NazivZuba;
            UspostavljenaDijagnoza d = ctx.UspostavljenaDijagnoza.Where(c => c.PregledId == p.Id).FirstOrDefault();
            model.Napomena = d.Napomena;
            Dijagnoza dd = ctx.Dijagnoza.Where(c => c.Id == d.DijagnozaId).FirstOrDefault();
            model.Dijagnoza=dd.Naziv;
            Terapija t = ctx.Terapija.Where(c => c.PregledId == p.Id).FirstOrDefault();
            Lijek l = ctx.Lijek.Where(c => c.Id == t.LijekId).FirstOrDefault();
            model.Lijek = l.Naziv;
            model.Kolicina = t.Količina;
            return View(model);
        }
        //private bool PregledObavljen(int id)
        //{
        //    int pregledId = ctx.Pregled.Where(p => p.Id == id).Select(p => p.Id).FirstOrDefault();
        //    if (pregledId != 0)
        //        return true;
        //    return false;
        //}

        public ActionResult PregledNovi(int id,int pacijentId)
        {
           
            Korisnik k = Autentifikacija.GetLogiraniKorisnik(HttpContext);
            NoviPregledVM Model = new NoviPregledVM();
            Model.DatumPregleda =DateTime.Now;
            Model.VrijemePregleda = DateTime.Now;
            Model.PacijentId = pacijentId;
            Model.StomatologId = k.Stomatolog.Id;
            Model.TerminId = id;
            Model._Zub = UcitajZub();
          // Model.zubID= Convert.ToInt32(Model._Zub.FirstOrDefault().Selected);
            Model._Dijagnoza = UcitajDijagnozu();
            Model._Usluga = UcitajUslugu();
            Model._Lijek = UcitajLijek();
            Session["Model"] = Model;
            return View("UnesiPregled", Model);

        }





        //id je termin 
        public ActionResult SnimiPregled(NoviPregledVM Model)
        {
            var p= (NoviPregledVM)Session["Model"];
            Pregled pregledDB;
                pregledDB = new Pregled();
                ctx.Pregled.Add(pregledDB);
           

               pregledDB.DatumPregleda =p.DatumPregleda;
                pregledDB.VrijemePregleda = p.VrijemePregleda;
                pregledDB.PacijentId = p.PacijentId;
                pregledDB.StomatologId =p.StomatologId;
            pregledDB.TerminId = p.TerminId;
            pregledDB.IsObavljen = true;
           


          ctx.SaveChanges();
            //ViewData["pregled"] = pregledDB;

            IzvrsenaUsluga usl = new IzvrsenaUsluga();
            usl.UslugaId = Model.uslugaID.Value;
            usl.ZubId = Model.zubID.Value;
            usl.Cijena = Model.Cijena;
            usl.PregledId = ctx.Pregled.Where(c=>c.PacijentId==p.PacijentId && c.TerminId==p.TerminId).FirstOrDefault().Id;
            ctx.IzvrsenaUsluga.Add(usl);
            ctx.SaveChanges();

            UspostavljenaDijagnoza dij = new UspostavljenaDijagnoza();
            dij.DijagnozaId = Model.dijagnozaID.Value;
            dij.ZubId = Model.zubID.Value;
            dij.Intenzitet = Model.Intenzitet;
            dij.Napomena = Model.Napomena;
            dij.PregledId = ctx.Pregled.Where(c => c.PacijentId == p.PacijentId && c.TerminId == p.TerminId).FirstOrDefault().Id;
            ctx.UspostavljenaDijagnoza.Add(dij);
            ctx.SaveChanges();

            Terapija t = new Terapija();
            t.LijekId = Model.lijekID.Value;
            t.Količina = Model.Kolicina;
            t.Vrsta = Model.Vrsta;
            t.PregledId = ctx.Pregled.Where(c => c.PacijentId == p.PacijentId && c.TerminId == p.TerminId).FirstOrDefault().Id;
            ctx.Terapija.Add(t);

             ctx.SaveChanges();

            return RedirectToAction("Index",new {id=p.TerminId,pacijentId=p.PacijentId });
         
            //TempData["Uspjeh"] = "Datum: " + pregledDB.DatumPregleda.ToString("dd/MM/yyyy") 
            //    +"/"
            //    + "Vrijeme: " + pregledDB.VrijemePregleda.ToShortTimeString();

        }

        private List<SelectListItem> UcitajLijek()
        {
            List<SelectListItem> lista = new List<SelectListItem>();
            lista.Add(new SelectListItem { Value = 0.ToString(), Text = "Odaberite lijek" });

            lista.AddRange(ctx.Lijek.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Naziv }).ToList());
            return lista;
        }

        private List<SelectListItem> UcitajUslugu()
        {
            List<SelectListItem> lista = new List<SelectListItem>();
            lista.Add(new SelectListItem { Value = 0.ToString(), Text = "Odaberite uslugu" });

            lista.AddRange(ctx.Usluga.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Vrsta }).ToList());
            return lista;
        }

        private List<SelectListItem> UcitajDijagnozu()
        {
            List<SelectListItem> lista = new List<SelectListItem>();
            lista.Add(new SelectListItem { Value = 0.ToString(), Text = "Odaberite dijagnozu" });

            lista.AddRange(ctx.Dijagnoza.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Naziv }).ToList());
            return lista;
        }

        private List<SelectListItem> UcitajZub()
        {
            List<SelectListItem> lista = new List<SelectListItem>();
            lista.Add(new SelectListItem { Value = 0.ToString(), Text = "Odaberite zub" });

            lista.AddRange(ctx.Zub.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.BrojZuba.ToString() + "-" + x.NazivZuba }).ToList());
            return lista;
        }

       
    }
}