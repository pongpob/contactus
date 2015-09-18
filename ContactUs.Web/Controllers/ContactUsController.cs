using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ContactUs.Services;
using ContactUs.Models;
using ContactUs.Web.ViewModels;
namespace ContactUs.Web.Controllers
{
    public class ContactUsController : Controller
    {
        private App app = new App();

        protected override void Dispose(bool disposing)
        {
            if (disposing) { app.Dispose(); }
            base.Dispose(disposing);
        }
        // GET: ContactUs
        public ActionResult Index()
        {
            ViewBag.Count = app.Tickets.All().Count();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitTicket(ContactUsIndexVM item)
        {
            if (ModelState.IsValid)
            {
                var t = new Ticket();
                t.Title = item.Title;
                t.Body = item.Body;
                t.LastActivityDate = DateTime.Now;

                app.Tickets.Add(t);
                app.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        [HttpPost]
        public ActionResult AddSampleTicket()
        {
            var t = new Ticket();
            t.Title = "Test Ticket";
            t.Body = "Blah blah";
            t.LastActivityDate = DateTime.Now;

            app.Tickets.Add(t);
            app.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Browse()
        {
            var q = from t in app.Tickets.All()
                    orderby t.LastActivityDate descending
                    select t;

            var tickets = q.ToList();
            return View(tickets);
        }

        [HttpPost]
        public ActionResult ChangeStatus(string id, string toStatus)
        {
            var ticket = app.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            if (toStatus == "Accepted")
            {
                ticket.Accept();
            }
            else if (toStatus == "Closed")
            {
                ticket.Close();
            }
            else if (toStatus == "Rejected")
            {
                ticket.Reject("N/A");
            }
            app.SaveChanges();
            return RedirectToAction("Browse");
        }
    }
}