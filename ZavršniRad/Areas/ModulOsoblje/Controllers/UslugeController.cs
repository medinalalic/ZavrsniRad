﻿using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZavršniRad.Areas.ModulOsoblje.Models;
using ZavršniRad.DAL;
using ZavršniRad.Helper;
using ZavršniRad.Models;

namespace ZavršniRad.Areas.ModulOsoblje.Controllers
{
    [Autorizacija(KorisnickeUloge.Osoblje)]

    public class UslugeController : Controller
    {
        MojContext ctx = new MojContext();

        
        public ActionResult Index(int? page, string search, int? uslugaid)
        {

            string s;
            int o = uslugaid ?? 0;

            if (search == null) s = "";
            else s = search;

            IPagedList<Usluga> op;
            if (s == "" && o == 0) op = ctx.Usluga.ToList().ToPagedList(page ?? 1, 3);
            else if (s != "" && o == 0) op = ctx.Usluga.Where(c => c.Vrsta.ToLower().Contains(s.ToLower())).ToList().ToPagedList(page ?? 1, 3);
            else if (s == "" && o != 0) op = ctx.Usluga.Where(c => c.Id == o).ToList().ToPagedList(page ?? 1, 3);
            else op = ctx.Usluga.Where(c => c.Id == o && c.Vrsta.ToLower().Contains(s.ToLower())).ToList().ToPagedList(page ?? 1, 3);

            UslugePagingVM model = new UslugePagingVM { };
            model.UslugeList = op;


            return View(model);
        }

        public ActionResult Dodaj()
        {
           UslugeUrediVM Model = new UslugeUrediVM();
            return View("Dodaj", Model);
        }


        public ActionResult Obrisi(int UslugaID)
        {
            Usluga usluge = ctx.Usluga.Find(UslugaID);
            if (usluge != null)
            {
                ctx.Usluga.Remove(usluge);
                ctx.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        public ActionResult Uredi(int UslugaID)
        {
            UslugeUrediVM model = ctx.Usluga
                 .Where(x => x.Id == UslugaID)
                 .Select(x => new UslugeUrediVM
                 {
                     Id = x.Id,
                     Vrsta = x.Vrsta
                 }).Single();
            return View(model);
        }

        public ActionResult Snimi(UslugeUrediVM usluge)
        {
            if (!ModelState.IsValid)
            {
                return View("Uredi", usluge);
            }
            else
            {
                Usluga uslugeDB;
                if (usluge.Id == 0)
                {
                    uslugeDB = new Usluga();
                    ctx.Usluga.Add(uslugeDB);
                }
                else
                {
                    uslugeDB = ctx.Usluga.Where(s => s.Id == usluge.Id).FirstOrDefault();
                }
                uslugeDB.Id = usluge.Id;
                uslugeDB.Vrsta = usluge.Vrsta;
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];

                    if (file != null && file.ContentLength > 0)
                    {
                        uslugeDB.Slika = new byte[file.ContentLength];
                        file.InputStream.Read(uslugeDB.Slika, 0, file.ContentLength);
                    }
                }
                ctx.SaveChanges();
                return RedirectToAction("Index");
            }

        }
      

        public ActionResult Show(int id)
        {
            var imageData = ctx.Usluga.Where(x => x.Id == id).Select(x => x.Slika).FirstOrDefault();

            return File(imageData, "image/jpg");
        }
    }
}