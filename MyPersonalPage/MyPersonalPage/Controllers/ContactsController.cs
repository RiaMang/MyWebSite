using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyPersonalPage.Models;
using Microsoft.AspNet.Identity;
using System.Configuration;

namespace MyPersonalPage.Controllers
{
    [RequireHttps]
    public class ContactsController : Controller
    {
        // GET: Contacts
        public ActionResult Index()
        {
            ViewBag.MessageSent = TempData["MessageSent"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ContactMessage ContactForm)
        {
            if(ModelState.IsValid)
            {
                var Emailer = new EmailService();
                var mail = new IdentityMessage {
                    Subject = "Message",
                    Destination = ConfigurationManager.AppSettings["ContactEmail"],
                    Body = "You have received a new Contact Form Submission from: "+ContactForm.Name+
                        "( "+ContactForm.Email+") with the following contents. \n\n"+ContactForm.Message };
                Emailer.SendAsync(mail);

                TempData["MessageSent"]="Your message has been delivered succesfully.";
                return RedirectToAction("Index");
                }
            
            return View(ContactForm);
        }

       
    }
}

