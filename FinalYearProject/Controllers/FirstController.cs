using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using FinalYearProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sakura.AspNetCore;

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



        [HttpGet]
        public IActionResult HomePage()
        {
            ViewBag.Categories = ORM.Category.ToList<Category>();
            IList<Product> SS = ORM.Product.ToList<Product>();
            return View(SS);
        }
        [HttpPost]
        public IActionResult HomePage(String Search)
        {
            ViewBag.Categories = ORM.Category.ToList<Category>();

            IList<Product> SearchData = ORM.Product.Where(m => m.ProductName.Contains(Search) || m.ProductMetaDisc.Contains(Search) ).ToList<Product>();


            return View(SearchData);
        }

        //List Of Products

        public IActionResult ListPage(int c= 0, int p=0,int page=1)

        {
           // var pageNumber = 1; 
            var pageSize = 5;
            
        //    return View(pagedData);


            IList<Product> S = null;

            if(p!=0)
            {
                IList<int> allsubcategories = ORM.Category.Where(cc => cc.ParentCategory == c).Select(m => m.CategoryId).ToList<int>();
                S = ORM.Product.Where(m => allsubcategories.Contains(m.CategoryId.Value)).ToList<Product>();
                
            }
                
            else if (c != 0)
            {


                S = ORM.Product.Where(pp => pp.CategoryId == c).ToList<Product>();


            }
            else
            {
                S = ORM.Product.ToList<Product>();
            }
            ViewBag.page = page;
            ViewBag.pageSize = pageSize;
            if (Request.Cookies["LIUID"] != null)
            {
                ViewBag.LIUID = Request.Cookies["LIUID"].ToString();
            }
            var pagedData = S.ToPagedList(pageSize, page);
            return View(pagedData);
        }



        //Detail of Products

        public IActionResult ProductDetail(int id)
        {

            Product P = ORM.Product.Where(m => m.ProductId == id).FirstOrDefault<Product>();
            if (P != null && P.CategoryId.HasValue)
            {
                ViewBag.Related = ORM.Product.Where(C => C.CategoryId == P.CategoryId).Take(10).ToList();
                
            }
            if(P!= null && P.WebsiteId.HasValue)
            {
                ViewBag.Web = ORM.Website.Where(C => C.WebsiteId == P.WebsiteId).FirstOrDefault();


            }
            if (Request.Cookies["LIUID"] != null) { 
            ViewBag.LIUID = Request.Cookies["LIUID"].ToString();
            }
            return View(P);
        }

        //Login Function
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
            Response.Cookies.Append("LIUID", DateTime.Now.ToString());

            return RedirectToAction("First","HomePage");

        }


        //Sign in Function 
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
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }



    }
}