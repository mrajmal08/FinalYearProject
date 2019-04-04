using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using FinalYearProject.Models;
using LinqToExcel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalYearProject.Controllers
{
    public class SecondController : Controller
    {
        private OPPCContext ORM = null;
        private IHostingEnvironment ENV = null;

        public SecondController(OPPCContext ORM, IHostingEnvironment ENV)
        {
            this.ORM = ORM;
            this.ENV = ENV;


        }
        [HttpGet]
        public ActionResult UploadExcel()
        {

            return View();
        }
        [HttpPost]
        public ActionResult UploadExcel(int SaveIt = 0, IFormFile upload = null)
        {
            String FilePath = ENV.WebRootPath + "/WebData/ExcelFile/";
            String FileName = Guid.NewGuid().ToString();
            String FileExtension = Path.GetExtension(upload.FileName);

            FileStream FS = new FileStream(FilePath + FileName + FileExtension, FileMode.Create);
            upload.CopyTo(FS);
            FS.Close();
            var SaveFN = FilePath + "" + FileName + "" + FileExtension;
          ExcelData obj = new ExcelData(SaveFN);

            var dsList = obj.getData("Sheet1",false);
            var oLObject = dsList.OfType<DataRow>().Select(x => x.ItemArray).ToList();
            oLObject.Remove(oLObject[0]);
            foreach (var item in oLObject)
            {
                Product oP = new Product();
                oP.ProductName = item[0].ToString();
                oP.ProductPrice =Convert.ToDecimal(item[1].ToString());
                oP.ProductImage = item[2].ToString();
                oP.ProductUrl = item[3].ToString();
                oP.ProductBrand = item[4].ToString();
                oP.ProductRating = item[10].ToString();
                oP.DiscountedPrice = Convert.ToDecimal(item[8].ToString()); ;

                ORM.Product.Add(oP);
                ORM.SaveChanges();
            }
            
            return View();

        }

    }
}

