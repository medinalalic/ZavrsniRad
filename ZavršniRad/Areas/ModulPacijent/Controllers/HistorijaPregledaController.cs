using PagedList;
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

    public class HistorijaPregledaController : Controller
    {
        // GET: ModulPacijent/HistorijaPregleda
        MojContext ctx = new MojContext();
        public ActionResult Index(int? page,DateTime? from,DateTime? to)
        {
            Korisnik k = Autentifikacija.GetLogiraniKorisnik(HttpContext);
            

            int o = k.Pacijent.Id;
            IPagedList<Pregled> op;
            if (from !=null && to != null) op = ctx.Pregled.Where(c => c.PacijentId == o && c.DatumPregleda >= from && c.DatumPregleda <= to).ToList().ToPagedList(page ?? 1, 3);
            else if(from !=null || to!=null) op= ctx.Pregled.Where(c => c.PacijentId == o && (c.DatumPregleda >= from || c.DatumPregleda <= to)).ToList().ToPagedList(page ?? 1, 3);
            else op = ctx.Pregled.Where(c=>c.PacijentId==o).ToList().ToPagedList(page ?? 1, 3);
           
            HistorijaVM model = new HistorijaVM { };
            model.PregledList = op;


            return View(model);
        }

        public ActionResult Detalji(int pregledID)
        {
            HistorijaVM model = new HistorijaVM();
            model.IzvUsluga = ctx.IzvrsenaUsluga.Include(f => f.Usluga).Include(f => f.Zub).Where(v => v.PregledId == pregledID).
                 Select(o => new HistorijaVM.IzvrsenaUsluga
                 {
                     Naziv = ctx.Usluga.FirstOrDefault(u => u.Id == o.UslugaId).Vrsta,
                     Cijena=o.Cijena,
                     IzvrsenaId=o.Id,
                     NazivZuba=ctx.Zub.FirstOrDefault(u=>u.Id==o.ZubId).NazivZuba
                     
                 }).ToList();

            model.UspDijagnoza = ctx.UspostavljenaDijagnoza.Include(f => f.Dijagnoza).Where(v => v.PregledId == pregledID).
                Select(o => new HistorijaVM.UspostavljenaDijagnoza
                {
                    UspId=o.Id,
                    Naziv= ctx.Dijagnoza.FirstOrDefault(u => u.Id == o.DijagnozaId).Naziv,
                    Napomena=o.Napomena

                }).ToList();
            model.Lijek = ctx.Terapija.Include(f => f.Lijek).Where(v => v.PregledId == pregledID).
           Select(o => new HistorijaVM.Terapija
           {
               TerapijaId=o.Id,
               Lijek= ctx.Lijek.FirstOrDefault(u => u.Id == o.LijekId).Naziv,
               Kolicina=o.Količina

           }).ToList();

            return View("Detalji", model);
        }
    }
}