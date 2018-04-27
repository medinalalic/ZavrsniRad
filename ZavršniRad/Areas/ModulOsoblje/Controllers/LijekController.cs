﻿using PagedList;
using System;
using System.Collections.Generic;
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

    public class LijekController : Controller
    {
        // GET: ModulOsoblje/Lijek
        MojContext ctx = new MojContext();
        public ActionResult Index(int? page, string search, int? lijekid)
        {

            string s;
            int o = lijekid ?? 0;

            if (search == null) s = "";
            else s = search;

            IPagedList<Lijek> op;
            if (s == "" && o == 0) op = ctx.Lijek.ToList().ToPagedList(page ?? 1, 3);
            else if (s != "" && o == 0) op = ctx.Lijek.Where(c => c.Naziv.ToLower().Contains(s.ToLower())).ToList().ToPagedList(page ?? 1, 3);
            else if (s == "" && o != 0) op = ctx.Lijek.Where(c => c.Id == o).ToList().ToPagedList(page ?? 1, 3);
            else op = ctx.Lijek.Where(c => c.Id == o && c.Naziv.ToLower().Contains(s.ToLower())).ToList().ToPagedList(page ?? 1, 3);

            LijekPrikazVM model = new LijekPrikazVM { };
            model.LijekList = op;


            return View(model);
        }
        public ActionResult Obrisi(int id)
        {
            Lijek d = ctx.Lijek.Find(id);
            if (d != null)
            {
                ctx.Lijek.Remove(d);
                ctx.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Dodaj()
        {
            LijekDodajVM Model = new LijekDodajVM();
            return View("Dodaj", Model);
        }
        public ActionResult Uredi(int id)
        {
            Lijek l = ctx.Lijek
              .Where(x => x.Id == id).SingleOrDefault();

            LijekDodajVM model = new LijekDodajVM
            {
                Id = l.Id,
                Naziv=l.Naziv

            };
            return View("Dodaj", model);
        }
        public ActionResult Snimi(LijekDodajVM lijek)
        {
            if (!ModelState.IsValid)
            {
                return View("Dodaj", lijek);
            }
            else
            {
                Lijek lijekDB;
                if (lijek.Id == 0)
                {
                    lijekDB = new Lijek();
                    ctx.Lijek.Add(lijekDB);
                }
                else
                {
                    lijekDB = ctx.Lijek.Where(s => s.Id == lijek.Id).FirstOrDefault();
                }
                lijekDB.Id = lijek.Id;
                lijekDB.Naziv = lijek.Naziv;

                ctx.SaveChanges();
                return RedirectToAction("Index");
            }

        }
    }
}