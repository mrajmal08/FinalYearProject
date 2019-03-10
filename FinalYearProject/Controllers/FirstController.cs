using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalYearProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalYearProject.Controllers
{
    public class FirstController : Controller
    {
        private OPPCContext ORM = null;

        public FirstController(OPPCContext ORM)
        {
            this.ORM = ORM;
      

        }

        public IActionResult HomePage()
        {
            return View();
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
            var userWithSameEmail = ORM.UserSystem.Where(m => m.UserEmail == U.UserEmail).SingleOrDefault(); //checking if the emailid already exits for any user
            if (ModelState.IsValid)
            {
                if (userWithSameEmail == null)
                {
                    ORM.UserSystem.Add(U);
                    ORM.SaveChanges();
                    ViewBag.Message = "Registration Done";
                    return RedirectToAction("ProductDetail");


                }
                else
                {
                    ViewBag.Message = "User with this Email Already Exist";
                    return View("SignUp");
                }
            }

            else
            {
                return View("ProductDetail");
            }
        }


    }
}