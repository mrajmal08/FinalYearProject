using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using FinalYearProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalYearProject.Controllers
{
    public class FirstController : Controller
    {
        private OPPCContext ORM = null;
        private IHostingEnvironment ENV = null;

        public FirstController(OPPCContext ORM, IHostingEnvironment ENV)
        {
            this.ORM = ORM;
            this.ENV = ENV;
      

        }

        public IActionResult HomePage()
        {
            return View();
        }


        public IActionResult ListPage(int Id)
        {
            Product S = ORM.Product.Where(m => m.ProductId == Id).FirstOrDefault<Product>();

            return View(S);

        }

        public IActionResult ProductDetail(int id)
        {

            Product P = ORM.Product.Where(m => m.ProductId == id).FirstOrDefault<Product>();

            return View(P);
        }
        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Login(UserSystem U)
        {
            UserSystem LU = ORM.UserSystem.Where(m => m.UserEmail == U.UserEmail && m.UserPassword == U.UserPassword).FirstOrDefault<UserSystem>();
            if (LU == null)
            {
                ViewBag.Message = "Invalid User Name or Password";
                return View();
            }
            HttpContext.Session.SetString("LIUID", LU.Id.ToString());
            return RedirectToAction("ProductDetail");

        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUp(UserSystem U)
        {
            var userWithSameEmail = ORM.UserSystem.Where(m => m.UserEmail == U.UserEmail).FirstOrDefault(); //checking if the emailid already exits for any user
            if (ModelState.IsValid)
            {
                if (userWithSameEmail == null)
                {
               
                    ORM.UserSystem.Add(U);
                    ORM.SaveChanges();

                    MailMessage Obj = new MailMessage();
                    Obj.From = new MailAddress("meharsalman073@gmail.com");
                    Obj.To.Add(new MailAddress(U.UserEmail));
                    Obj.Subject = "Welcome to theta Solution:";
                    Obj.Body = "Dear" + "" + "Mr " + " " + U.UserFname + "<br ><br >" +
                    "Well Come to Online Product Price Comparison " + "<br><br>" +
                    "Reguards OPPC Team...";
                    Obj.IsBodyHtml = true;
                   

                    //


                    //

                    SmtpClient SMTP = new SmtpClient();
                    SMTP.Host = "smtp.gmail.com";
                    SMTP.Port = 587;
                    SMTP.EnableSsl = true;
                    SMTP.Credentials = new System.Net.NetworkCredential("meharsalman073@gmail.com", "salman123");

                    try
                    {
                        SMTP.Send(Obj);
                    }
                    catch (Exception)
                    {
                        ViewBag.Message = "Mail has sent successfully";
                    }


                    ViewBag.Message = "You are signed in now";
                    return View();
                }
                ViewBag.Message = "Registration Done";
                    return RedirectToAction("HomePage");
                
                    

                }
                else
                {
                    ViewBag.Message = "User with this Email Already Exist";
                    return View("SignUp");
                }
        }


    }
}