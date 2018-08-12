using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mongodb_mvc.Properties;
using MongoDB.Driver;
using mongodb_mvc.Models;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;
using System.Threading.Tasks;
using System.Data;

namespace mongodb_mvc.Models
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/       
        connect cn = new connect();
        MongoDatabase mongodb = mongodb_mvc.connect.mongodb;
       static IMongoQuery q = mongodb_mvc.connect.query;
       static String product;

        public ProductController()
        {

        }

        public ActionResult List()
        {           
            var collection = mongodb.GetCollection<Product>("Product");
            return View(collection.FindAll().ToList<Product>());          
        }        
        public ActionResult Create()
        {            
            return View();
        }
        [HttpPost]
        public ActionResult Create(Product model)
        {
            var collection = mongodb.GetCollection<Product>("Product");
            collection.Insert(model);           
            return RedirectToAction("List");
        }
        public ActionResult Edit(string Id)
        {            
            var collection = mongodb.GetCollection<Product>("Product");
            q = Query<Product>.Where(s => s._id == ObjectId.Parse(Id));
            var model = collection.FindOne(q);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(Product model1)
        {
            var collection = mongodb.GetCollection<Product>("Product");           
            var update = Update.Set("branch_number", model1.branch_number).Set("product_code", model1.product_code).Set("brand", model1.brand).Set("product_type", model1.product_type).Set("purchase_price", model1.purchase_price).Set("purchase_date", model1.purchase_date).Set("stock", model1.stock);
            collection.FindAndModify(q, SortBy.Null, update);
            return RedirectToAction("List");
        }

        public ActionResult Delete(string Id)
        {
            var collection = mongodb.GetCollection<Product>("Product");
            q = Query<Product>.Where(s => s._id == ObjectId.Parse(Id));
            var model = collection.FindOne(q);
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(Product model)
        {
            var collection = mongodb.GetCollection<Product>("Product");
            collection.Remove(q);
            return RedirectToAction("List");
        }
       
        public ActionResult Excel()
        {
             var collection = mongodb.GetCollection<Product>("Product");
            try
            {
                Excel.Application application = new Excel.Application();
                Excel.Workbook workbook = application.Workbooks.Add(System.Reflection.Missing.Value);
                Excel.Worksheet worksheet = workbook.ActiveSheet;
                ProductModel p = new ProductModel();
                worksheet.Cells[1, 1] = "branch_number";
                worksheet.Cells[1, 2] = "product_code";
                worksheet.Cells[1, 3] = "brand";
                worksheet.Cells[1, 4] = "product_type";
                worksheet.Cells[1, 5] = "purchase_price";
                worksheet.Cells[1, 6] = "purchase_date";
                worksheet.Cells[1, 7] = "stock";
                int row = 2;
                List<Product> model = collection.FindAll().ToList<Product>();
                foreach (Product pr in p.findAll(model))
                {
                    worksheet.Cells[row, 1] = pr.branch_number;
                    worksheet.Cells[row, 2] = pr.product_code;
                    worksheet.Cells[row, 3] = pr.brand;
                    worksheet.Cells[row, 4] = pr.product_type;
                    worksheet.Cells[row, 5] = pr.purchase_price;
                    worksheet.Cells[row, 6] = pr.purchase_date;
                    worksheet.Cells[row, 7] = pr.stock;
                    row++;
                }

                worksheet.get_Range("A1", "E1").EntireColumn.AutoFit();

                //format heading
                var range_heading = worksheet.get_Range("A1", "E1");
                range_heading.Font.Bold = true;
                range_heading.Font.Color = System.Drawing.Color.Red;
                range_heading.Font.Size = 13;

                //format currency
                var range_currency = worksheet.get_Range("C2", "C4");
                range_currency.NumberFormat = "$ #,###,###.00";

                //format date
                //var range_date = worksheet.get_Range("F2", "F4");
                //range_date.NumberFormat = "mm/dd/yyyy";

                workbook.SaveAs("d:\\product.xls");
                workbook.Close();
                Marshal.ReleaseComObject(workbook);

                application.Quit();
                Marshal.FinalReleaseComObject(application);
                //ViewBag.Result = "Done";

                Response.Write("<script>alert('Excel dosyası oluşturuldu!')</script>");
               
            }
            catch (Exception ex)
            {
                ViewBag.Result = ex.Message;
            }
            return RedirectToAction("List");
            //return View("Succes");
        }

        //public ActionResult Search()
        //{        
        //    return View();   
        //}
        //[HttpPost]
        //public ActionResult Search()
        //{
        //    //var collection = mongodb.GetCollection<Product>("Product");
        //    //product = Request.Form["txtproduct"];
        //    //IMongoQuery query = Query<Product>.Where(f => f.product_code.ToLower().Contains(product));
        //    //var model = collection.Find(query).ToList<Product>();


        //    var collection = mongodb.GetCollection<Product>("Product");
        //    var query = Query<Product>.EQ(u => u.product_code, "edfg");
        //    Product pro = collection.FindOne(query);
        //    return View();
        //    //return RedirectToAction("List");
        //}
       
    }
}
