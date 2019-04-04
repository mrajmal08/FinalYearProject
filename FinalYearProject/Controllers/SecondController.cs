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

            var dsList = obj.getData("Sheet1", false);
            var oLObject = dsList.OfType<DataRow>().Select(x => x.ItemArray).ToList();
            oLObject.Remove(oLObject[0]);

            foreach (var item in oLObject)
            {
                if (item[5] != null)
                {
                    IList<Category> Catagory = ORM.Category.Where(m => m.CategoryName.Equals(item[5])).ToList();
                    if (Catagory.Count == 0)
                    {
                        Category Ca = new Category();
                        Ca.CategoryName = item[5] == null ? "" : item[5].ToString();
                        Ca.CategoryStatus = "Active";
                        Ca.CategoryCreatedDate = DateTime.Now;

                        ORM.Category.Add(Ca);
                        ORM.SaveChanges();
                    }


                    if (item[6] != null)
                    {
                        IList<Category> Subcata = ORM.Category.Where(m => m.CategoryName.Equals(item[6])).ToList();
                        if (Subcata.Count == 0)
                        {
                            Category SubCa = new Category();
                            SubCa.CategoryName = item[6] == null ? "" : item[6].ToString();
                            SubCa.CategoryStatus = "Active";
                            SubCa.CategoryCreatedDate = DateTime.Now;


                            SubCa.ParentCategory = ORM.Category.Where(m => m.CategoryName.Equals(item[5])).ToList<Category>().FirstOrDefault().CategoryId;


                            ORM.Category.Add(SubCa);
                            ORM.SaveChanges();
                        }

                        if (item[7] != null && item[9] != null)
                        {
                            IList<Website> Website = ORM.Website.Where(m => m.WebsiteName.Equals(item[7])).ToList();
                            if (Website.Count == 0)
                            {
                                Website web = new Website();
                                web.WebsiteName = item[7] == null ? "" : item[7].ToString();
                                web.WebsiteLogo = item[9] == null ? "" : item[9].ToString();


                                ORM.Website.Add(web);
                                ORM.SaveChanges();
                            }
                        }
                            Product oP = new Product();
                            oP.ProductName = item[0] == null ? "" : item[0].ToString();
                            oP.ProductPrice = Convert.ToDecimal(item[1] == null ? "" : item[1].ToString());
                            oP.ProductImage = item[2] == null ? "" : item[2].ToString();
                            oP.ProductUrl = item[3] == null ? "" : item[3].ToString();
                            oP.ProductBrand = item[4] == null ? "" : item[4].ToString();
                            oP.ProductRating = item[10] == null ? "" : item[10].ToString();
                            oP.DiscountedPrice = Convert.ToDecimal(item[8] == null ? "" : item[8].ToString()); ;
                            oP.CategoryId = ORM.Category.Where(m => m.CategoryName.Equals(item[6])).ToList<Category>().FirstOrDefault().CategoryId;
                            ORM.Product.Add(oP);
                            ORM.SaveChanges();


                        }
                    }
                }





                ViewBag.Message = "All products categoreis and sub categories have been added in the sytem";
                return View();

            }

        }
    }

