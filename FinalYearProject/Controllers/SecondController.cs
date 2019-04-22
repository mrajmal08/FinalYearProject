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
using Newtonsoft.Json;

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
                        try
                        {
                            oP.ProductPrice =item[1] == null ? "" : item[1].ToString();
                        }
                        catch(Exception e)
                        {
                            ViewBag.message = "";
                        }
                            oP.ProductImage = item[2] == null ? "" : item[2].ToString();
                            oP.ProductUrl = item[3] == null ? "" : item[3].ToString();
                            oP.ProductBrand = item[4] == null ? "" : item[4].ToString();
                            oP.ProductRating = item[10] == null ? "" : item[10].ToString();
                            oP.DiscountedPrice =item[8] == null ? "" : item[8].ToString();
                            oP.CategoryId = ORM.Category.Where(m => m.CategoryName.Equals(item[6])).ToList<Category>().FirstOrDefault().CategoryId;
                            ORM.Product.Add(oP);
                            ORM.SaveChanges();


                        }
                    }
                }





                ViewBag.Message = "All products categoreis and sub categories have been added in the sytem";
                return View();

            }

        // Code for sign in with facebook data store in data base
        [HttpPost]
        public JsonResult CheckIfAlreadyExist(string FaceBookID,string FirstName, string LastName, string Email, String ProfilePicture)
        {
            UserSystem OUsers = null;
            bool Result = false;
            
            OUsers = ORM.UserSystem.Where(m => m.FaceBookID == FaceBookID).FirstOrDefault();
            if (OUsers != null)
            {
                OUsers.UserSname = LastName;
                OUsers.UserEmail = Email;
                OUsers.UserImage = ProfilePicture;

                ORM.UserSystem.Update(OUsers);
                ORM.SaveChanges();

                Result = true;
                HttpContext.Session.SetString("LIUID",Convert.ToString(OUsers.Id));

            }
            else
            {
              
                
                OUsers = new UserSystem
                {
                    
                    UserFname = FirstName, FaceBookID = FaceBookID,
                    UserSname = LastName, UserEmail = Email,
                    UserImage = ProfilePicture,
                   
                };
                using (var tr = ORM.Database.BeginTransaction())
                {
                    try
                    {
                        ORM.UserSystem.Add(OUsers);
                        ORM.SaveChanges();
                        tr.Commit();
                        Result = true;
                        HttpContext.Session.SetString("LIUID", OUsers.Id.ToString());
                        TempData["GreetingMessage"] = "You are successfully registered";
                    }
                    catch (Exception ex)
                    {
                        string e = ex.Message;
                        tr.Rollback();
                    }
                }
            }

           
            return Json(Result);
        }

        [HttpGet]
        public IActionResult ContactUS()
        {

            return View();
        }
        [HttpPost]
        public IActionResult ContactUs(Contact C) {


            ORM.Contact.Add(C);
            ORM.SaveChanges();
            ViewBag.message = "You details has been sent to admin";

            return View(C);

        }

        public IActionResult AdToWishlist(int id)
        {
            HttpContext.Response.Cookies.Append("wishList", id.ToString());

            return RedirectToAction("ProductDetail", "First",new {id =id });
        }

    }
}

