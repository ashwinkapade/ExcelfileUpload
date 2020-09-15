using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExcelFileUpload.Models;
using ExcelFileUpload.ViewModel;
namespace ExcelFileUpload.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            return View();
        }


        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            string filePath = string.Empty;
            if (postedFile != null)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(postedFile.FileName);
                string extension = Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(filePath);

                string conString = string.Empty;
                switch (extension)
                {
                    case ".xls": //Excel 97-03.
                        conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07 and above.
                        conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                        break;
                }

                DataTable dt = new DataTable();
                conString = string.Format(conString, filePath);

                using (OleDbConnection connExcel = new OleDbConnection(conString))
                {
                    using (OleDbCommand cmdExcel = new OleDbCommand())
                    {
                        using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                        {
                            cmdExcel.Connection = connExcel;

                            //Get the name of First Sheet.
                            connExcel.Open();
                            DataTable dtExcelSchema;
                            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            connExcel.Close();

                            //Read Data from First Sheet.
                            connExcel.Open();
                            cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                            odaExcel.SelectCommand = cmdExcel;
                            odaExcel.Fill(dt);
                            connExcel.Close();
                        }
                    }
                }

                conString = ConfigurationManager.ConnectionStrings["Constring"].ConnectionString;
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        //Set the database table name.
                        sqlBulkCopy.DestinationTableName = "dbo.customer1";

                        //[OPTIONAL]: Map the Excel columns with that of the database table
                        sqlBulkCopy.ColumnMappings.Add("CompanyName", "CompanyName");
                        sqlBulkCopy.ColumnMappings.Add("GSTIN", "GSTIN");
                        sqlBulkCopy.ColumnMappings.Add("StartDate", "StartDate");

                        sqlBulkCopy.ColumnMappings.Add("EndDate", "EndDate");
                        sqlBulkCopy.ColumnMappings.Add("TurnoveAmount", "TurnoveAmount");
                        sqlBulkCopy.ColumnMappings.Add("ContactEmail", "ContactEmail");
                        sqlBulkCopy.ColumnMappings.Add("ContactNumber", "ContactNumber");


                        con.Open();
                        sqlBulkCopy.WriteToServer(dt);
                        con.Close();
                    }
                }
            }
            return RedirectToAction(nameof(details));
            // return View();
        }

        public ActionResult details()
        {
            testtEntities pe = new testtEntities();
            IList<customer1> er = pe.customer1.ToList();
            List<CustomerIndexViewModel> evm = new List<CustomerIndexViewModel>();
            foreach (var item in er)
            {
                CustomerIndexViewModel obj = new CustomerIndexViewModel();
                obj.sr = item.sr;
                obj.CompanyName = item.CompanyName;
                obj.GSTIN = item.GSTIN;
                obj.StartDate = item.StartDate;

                obj.StartDate = item.StartDate;
                obj.EndDate = item.EndDate;
                obj.TurnoveAmount = item.TurnoveAmount;
                obj.ContactEmail = item.ContactEmail;
                obj.ContactNumber = item.ContactNumber;

                evm.Add(obj);


            }
            return View(evm);

        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            testtEntities pe = new testtEntities();
            customer1 item = pe.customer1.Where(x => x.sr == id).FirstOrDefault();
            CustomerEditViewModel obj = new CustomerEditViewModel();

            obj.sr = item.sr;
            obj.CompanyName = item.CompanyName;
            obj.GSTIN = item.GSTIN;
            obj.StartDate = item.StartDate;

            obj.StartDate = item.StartDate;
            obj.EndDate = item.EndDate;
            obj.TurnoveAmount = item.TurnoveAmount;
            obj.ContactEmail = item.ContactEmail;
            obj.ContactNumber = item.ContactNumber;



            return View(obj);
        }

        [HttpPost]
        public ActionResult Edit(CustomerEditViewModel model)
        {
            if(ModelState.IsValid)
            {
                testtEntities pe = new testtEntities();
                customer1 obj = pe.customer1.Where(x => x.sr == model.sr).FirstOrDefault();

                obj.CompanyName = model.CompanyName;
                obj.GSTIN = model.GSTIN;
                obj.StartDate = model.StartDate;

                obj.StartDate = model.StartDate;
                obj.EndDate = model.EndDate;
                obj.TurnoveAmount = model.TurnoveAmount;
                obj.ContactEmail = model.ContactEmail;
                obj.ContactNumber = model.ContactNumber;

                pe.SaveChanges();
                return RedirectToAction(nameof(details));
            }
            else
            {
                return View(model);
            }
            
        }
    }
}