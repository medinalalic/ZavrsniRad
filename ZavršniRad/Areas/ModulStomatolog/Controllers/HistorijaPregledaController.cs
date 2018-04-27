﻿using PagedList;
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

    public class HistorijaPregledaController : Controller
    {
        MojContext ctx = new MojContext();
        public ActionResult Index(int? page,int? pacijentid)
        {
            
            int o = pacijentid ?? 0;
            IPagedList<Pregled> op;
             op = ctx.Pregled.Where(c=>c.PacijentId==o).ToList().ToPagedList(page ?? 1, 3);
           
            HistorijaVM model = new HistorijaVM { };
            model.PregledList = op;
            model.pacijent = ctx.Pacijent.Include(c=>c.Korisnik).Where(c => c.Id == o).FirstOrDefault().Korisnik.Ime+" "+ ctx.Pacijent.Include(c => c.Korisnik).Where(c => c.Id == o).FirstOrDefault().Korisnik.Prezime;

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