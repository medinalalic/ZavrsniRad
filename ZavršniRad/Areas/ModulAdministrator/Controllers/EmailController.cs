﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using ZavršniRad.Areas.ModulAdministrator.Models;
using ZavršniRad.DAL;
using ZavršniRad.Helper;
using ZavršniRad.Models;

namespace ZavršniRad.Areas.ModulAdministrator.Controllers
{
    [Autorizacija(KorisnickeUloge.Administrator)]
    public class EmailController : Controller
    {
        // GET: ModulAdministrator/Email
        MojContext ctx = new MojContext();

        public ActionResult Index()
        {
            List<PacijentPrikazVM.PacijentInfo> pacijenti = (ctx.Pacijent.Where(c=>c.Korisnik.Aktivan==false)
             .Select(x => new PacijentPrikazVM.PacijentInfo
             {
                 Id = x.Id,
                 Ime = x.Korisnik.Ime,
                 Prezime = x.Korisnik.Prezime,
                 Email = x.Korisnik.Email,
                 Mobitel = x.Korisnik.Mobitel,
                 Adresa = x.Korisnik.Adresa,
                 KorisnickoIme = x.Korisnik.KorisnickoIme,
                 Lozinka = x.Korisnik.Lozinka,
                 JMBG = x.JMBG,
                 IsAdmin = x.Korisnik.IsAdmin,
                 Aktivan = x.Korisnik.Aktivan
             }))
             .ToList();

            PacijentPrikazVM model = new PacijentPrikazVM
            {
                pacijent = pacijenti
            };

            return View(model);
        }
      




       
    }
}
