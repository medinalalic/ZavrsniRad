﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
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

    public class HomeSController : Controller
    {
        // GET: ModulStomatolog/Home
        MojContext ctx = new MojContext();
        public ActionResult Index()
        {
            var day1 = DateTime.Now.DayOfWeek;
            ZakazaniTermini Model = new ZakazaniTermini();
            Model.termin = ctx.Termin.ToList().Where(x => x.Datum.DayOfWeek == day1 && x.Vrijeme != null).OrderBy(x => x.Vrijeme)
               .Select(x => new ZakazaniTermini.TerminInfo
               {
                   Id = x.Id,
                   Datum = x.Datum,
                   Vrijeme = x.Vrijeme,
                   Razlog = x.RazlogPosjete,
                   Odobren = x.Odobren,
                   PacijentId = x.PacijentId,
                   Pacijent = ctx.Pacijent.Include(s => s.Korisnik).Where(s => s.Id == x.PacijentId).FirstOrDefault().Korisnik.Ime + " " + ctx.Pacijent.Include(s => s.Korisnik).Where(s => s.Id == x.PacijentId).FirstOrDefault().Korisnik.Prezime,
                   Obavljen = PregledObavljen(x.Id, x.PacijentId)
               }).ToList();
            
            return View(Model);
        }
        private bool PregledObavljen(int terminID,int pacijentID)
        {
            Pregled c = ctx.Pregled.Where(p => p.PacijentId==pacijentID && p.TerminId==terminID).FirstOrDefault();
            if (c !=null && c.IsObavljen==true)
                return true;
            return false;
        }
        public ActionResult Metoda(int id)
        {
            Termin p = ctx.Termin.Find(id);
            p.Odobren = true;
            ctx.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}