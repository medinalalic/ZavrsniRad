using System;
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
      




        //public ActionResult EmailPacijenti(int pacijentId, bool? message)
        //{
        //    var model = new EmailSendVM();

        //    if (message.HasValue)
        //    {
        //        var pacijentis = ctx.Pacijent.Where(x => x.Id == pacijentId).Include(x=>x.Korisnik).FirstOrDefault();
        //        model.Successed = true;

        //        model.To = pacijentis.Korisnik.Email;

        //        return View(model);
        //    }
        //    var pacijenti = ctx.Pacijent.Where(x => x.Id == pacijentId).Include(x => x.Korisnik).FirstOrDefault();

        //    if (pacijenti == null)
        //    {
        //        return View("Index");
        //    }

        //    model.To = pacijenti.Korisnik.Email;
        //    model.Id = pacijenti.Id;

        //    return PartialView("EmailPacijenti", model);

        //}
        //[HttpPost]
        //public ActionResult SnimiPacijenti(EmailSendVM model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return PartialView("EmailPacijenti", model);
        //    }
        //    var to = model.To;
        //    var body = model.Body;
        //    var subject = model.Subject;
        //    var aktiv = ctx.Pacijent.Include(c=>c.Korisnik).Where(c=>c.Id==model.Id).First();
        //    aktiv.Korisnik.Aktivan = true;
        //    ctx.SaveChanges();
        //    var sendEmail = SendMail(to, body, subject);

        //    if (sendEmail)
        //    {
        //        model.Successed = true;
        //        return RedirectToAction("EmailPacijenti",
        //            new
        //            {
        //                pacijentId = model.Id,
        //                message = true
        //            });
        //    }

        //    return PartialView("EmailPacijenti", model);

        //}



        //public bool SendMail(string email, string body, string subject)
        //{
        //    string emailSender = "omer_avdic@hotmail.com";
        //    MailMessage mailMessage = new MailMessage();
        //    mailMessage.From = new MailAddress(emailSender);
        //    mailMessage.To.Add(email);
        //    mailMessage.Subject = subject;
        //    mailMessage.Body = body;

        //    SmtpClient smtp = new SmtpClient();
        //    smtp.Host = "smtp.live.com";
        //    smtp.Port = 587;
        //    smtp.UseDefaultCredentials = false;
        //    smtp.Credentials = new System.Net.NetworkCredential
        //    ("omer_avdic@hotmail.com", "benzema");// Enter seders User name and password
        //    smtp.EnableSsl = true;
        //    try
        //    {
        //        smtp.Send(mailMessage);
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }

        //}

        //public ActionResult Metoda(int id)
        //{
        //    Pacijent p = ctx.Pacijent.Where(x=>x.Id == id).Include(x=>x.Korisnik).FirstOrDefault();
        //    p.Korisnik.Aktivan = !p.Korisnik.Aktivan;
        //    ctx.SaveChanges();
        //    return RedirectToAction("Index");
        //}
    }
}