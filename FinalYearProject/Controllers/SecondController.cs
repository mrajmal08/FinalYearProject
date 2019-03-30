using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
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
        public ActionResult UploadExcel(int SaveIt = 0, IFormFile FileUpload = null)
        {
            if (SaveIt == 0)
            {
                if (FileUpload != null)
                {
                    string extension = System.IO.Path.GetExtension(FileUpload.FileName).ToLower();
                    string query = null;
                    string connString = "";
                    string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                    String FilePath = ENV.WebRootPath + "/WebData/ExcelFile/";
                    String FileName = Guid.NewGuid().ToString();
                    String FileExtension = Path.GetExtension(FileUpload.FileName);

                    FileStream FS = new FileStream(FilePath + FileName + FileExtension, FileMode.Create);
                    FileUpload.CopyTo(FS);
                    FS.Close();
                    var filepath = "/WebData/ExcelFile/" + FileName + FileExtension;


                    //if (!Directory.Exists(filepath))
                    //{

                    //    Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));

                    //}
                    if (validFileTypes.Contains(extension))
                    {
                        DataTable dt = new DataTable();
                        if (System.IO.File.Exists(filepath))
                        {
                            System.IO.File.Delete(filepath);
                        }

                        if (extension.Trim() == ".csv")
                        {
                            dt = ExcelToIList.ConvertCSVtoDataTable(filepath);
                        }
                        else if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filepath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                            dt = ExcelToIList.ConvertXSLXtoDataTable(filepath, connString);

                        }
                        else if (extension.Trim() == ".xlsx")
                        {
                            connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                            dt = ExcelToIList.ConvertXSLXtoDataTable(filepath, connString);
                        }
                        //                        ViewBag.OverTimeList = ExcelToIList.ConvertDataTable<OVERTIME>(dt);
                        IList<Product> ListOvertime = ExcelToIList.ConvertDataTable<Product>(dt);
                        //   viewofMemOvertime

                            using (var transaction = ORM.Database.BeginTransaction())
                            {
                                try
                                {
                                    IList<Product> ot = ListOvertime;
                                    //MemList.Where(x => x.MEMBER_ID == Member.Key.MEMBER_ID).ToList().ForEach(x => x.SingleOverTimeHours = OverTimeHoursSingle.TotalHours);
                                    ot.ToList().ForEach(x => x.ProductCreatedDate = DateTime.Now);
                                ORM.Product.AddRange(ot);
                                ORM.SaveChanges();
                                    transaction.Commit();
                                    ViewBag.message = "Successfuly Added";
                                }
                                catch (Exception ex)
                                {
                                    ViewBag.ErrorMessage = "Some Error Occured";
                                }
                            }
                        

                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Please Upload Files in .xls, .xlsx or .csv format";
                    }

                }
            }
            return View();
        }
    }
}

