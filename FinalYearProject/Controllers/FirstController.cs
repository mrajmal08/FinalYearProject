using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalYearProject.Models;
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

        public IActionResult ProductDetail(int id)
        {

            Product P = ORM.Product.Where(m => m.ProductId == id).FirstOrDefault<Product>();

            return View(P);
        }
        
       
    }
}