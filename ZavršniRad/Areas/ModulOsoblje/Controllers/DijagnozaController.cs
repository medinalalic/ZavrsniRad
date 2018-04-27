using PagedList;
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
    public class DijagnozaController : Controller
    {
        // GET: ModulOsoblje/Dijagnoza
        MojContext ctx = new MojContext();
        public ActionResult Index(int? page, string search, int? dijagnozaid)
        {

            string s;
            int o = dijagnozaid ?? 0;

            if (search == null) s = "";
            else s = search;

            IPagedList<Dijagnoza> op;
            if (s == "" && o == 0) op = ctx.Dijagnoza.ToList().ToPagedList(page ?? 1, 3);
            else if (s != "" && o == 0) op = ctx.Dijagnoza.Where(c => c.Naziv.ToLower().Contains(s.ToLower())).ToList().ToPagedList(page ?? 1, 3);
            else if (s == "" && o != 0) op = ctx.Dijagnoza.Where(c => c.Id == o).ToList().ToPagedList(page ?? 1, 3);
            else op = ctx.Dijagnoza.Where(c => c.Id == o && c.Naziv.ToLower().Contains(s.ToLower())).ToList().ToPagedList(page ?? 1, 3);

            DijagnozaPrikazVM model = new DijagnozaPrikazVM { };
            model.DijagnozaList = op;


            return View(model);
        }

        public ActionResult Dodaj()
        {
            DijagnozaDodajVM Model = new DijagnozaDodajVM();
            return View("Dodaj", Model);
        }
        public ActionResult Uredi(int id)
        {
            Dijagnoza l = ctx.Dijagnoza
             .Where(x => x.Id == id).SingleOrDefault();

            DijagnozaDodajVM model = new DijagnozaDodajVM
            {
                Id = l.Id,
                Naziv = l.Naziv

            };
            return View("Dodaj", model);
        }
        public ActionResult Obrisi(int id)
        {
            Dijagnoza d = ctx.Dijagnoza.Find(id);
            if (d != null)
            {
                ctx.Dijagnoza.Remove(d);
                ctx.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Snimi(DijagnozaDodajVM dijagnoza)
        {
            if (!ModelState.IsValid)
            {
                return View("Dodaj", dijagnoza);
            }
            else
            {
                Dijagnoza dijagnozaDB;
                if (dijagnoza.Id == 0)
                {
                    dijagnozaDB = new Dijagnoza();
                    ctx.Dijagnoza.Add(dijagnozaDB);
                }
                else
                {
                    dijagnozaDB = ctx.Dijagnoza.Where(s => s.Id == dijagnoza.Id).FirstOrDefault();
                }
                dijagnozaDB.Id = dijagnoza.Id;
                dijagnozaDB.Naziv = dijagnoza.Naziv;
               
                ctx.SaveChanges();
                return RedirectToAction("Index");
            }

        }

    }
}