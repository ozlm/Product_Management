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

namespace mongodb_mvc.Models
{
    public class SaleController : Controller
    {
        //
        // GET: /Sale/
        static IMongoQuery query;
        connect cn = new connect();
        MongoDatabase mongodb = mongodb_mvc.connect.mongodb;   
       static IMongoQuery queryDel = mongodb_mvc.connect.query;
       static IMongoQuery query_pro = mongodb_mvc.connect.query;

        public SaleController()
        {

        }

        public ActionResult SaleProduct(string ID)
        {
            var collec_pro = mongodb.GetCollection<Product>("Product");
            query_pro = Query<Product>.Where(s => s._id == ObjectId.Parse(ID));
            return View();
        }
        [HttpPost]
        public ActionResult SaleProduct(Sale model)
        {
            var collec_pro = mongodb.GetCollection<Product>("Product");
            var model_pro = collec_pro.FindOne(query_pro);
            var collec_sale = mongodb.GetCollection<Sale>("Sale");          
            
            int a = model_pro.stock;
            int b = model.count;
            int c = a - b;
            if (c >= 0)
            {
                var document = new BsonDocument{
               {"sale_date",model.sale_date},
               {"count", model.count},
               {"sale_price",model.sale_price},
               {"productId",model_pro._id},
           };
                collec_sale.Insert(document);
                var update = Update.Set("stock", c);
                collec_pro.FindAndModify(query_pro, SortBy.Null, update);                
            }
            else
            {
                Response.Write("<script>alert('Yeterli sayıda ürün bulunmamaktadır!')</script>");
                return View();
            }
            
            return RedirectToAction("List", "Product");
        }

        public ActionResult SaleList(Sale model)
        {

            var collec_sale = mongodb.GetCollection<Sale>("Sale");
            return View(collec_sale.FindAll().ToList<Sale>());
        }

        public ActionResult SaleDelete(string Id)
        {
            var collection = mongodb.GetCollection<Sale>("Sale");
            queryDel = Query<Sale>.Where(s => s._id == ObjectId.Parse(Id));
            var model = collection.FindOne(queryDel);
            return View(model);
        }
        [HttpPost]
        public ActionResult SaleDelete(Sale model)
        {
            var collection = mongodb.GetCollection<Sale>("Sale");
            collection.Remove(queryDel);
            return RedirectToAction("SaleList");
        }


        public ActionResult ExcelSale()
        {
            var collection = mongodb.GetCollection<Sale>("Sale");
            try
            {
                Excel.Application application = new Excel.Application();
                Excel.Workbook workbook = application.Workbooks.Add(System.Reflection.Missing.Value);
                Excel.Worksheet worksheet = workbook.ActiveSheet;
                SaleModel p = new SaleModel();
                worksheet.Cells[1, 1] = "sale date";
                worksheet.Cells[1, 2] = "count";
                worksheet.Cells[1, 3] = "sale price";
                worksheet.Cells[1, 4] = "productId";
              
                int row = 2;
                List<Sale> model = collection.FindAll().ToList<Sale>();
                foreach (Sale pr in p.findAll(model))
                {
                    worksheet.Cells[row, 1] = pr.sale_date;
                    worksheet.Cells[row, 2] = pr.count;
                    worksheet.Cells[row, 3] = pr.sale_price;
                    worksheet.Cells[row, 4] = pr.productId.ToString();                   
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

                workbook.SaveAs("d:\\sale.xls");
                workbook.Close();
                Marshal.ReleaseComObject(workbook);
               
                application.Quit();
                Marshal.FinalReleaseComObject(application);
               
            }
            catch (Exception ex)
            {
                ViewBag.Result = ex.Message;
            }

            Response.Write("<script>alert('Excel dosyası oluşturuldu!')</script>");
            return RedirectToAction("SaleList");
        }

        public ActionResult Index()
        {
            return View();
        }


        public JsonResult SaleCounts()
        { 
           string [] months={"1","2","3","4","5","6","7","8","9","10","11","12"};
           
            int[] counts = new int[12];

            var collection = mongodb.GetCollection<Sale>("Sale");
            List<SaleCount> items = new List<SaleCount>();
            List<Sale> model = collection.FindAll().ToList<Sale>();
            Sale s = new Sale();
                        
            for (int i = 0; i < model.Count; i++)
            {
                for (int j = 0; j < months.Length; j++)
                {
                    if (months[j].Equals(model[i].sale_date.Month.ToString()))
                    {
                        counts[j] += model[i].count;
                    }
                }
            }
            
            for (int i = 0; i < months.Length; i++)
            {
                items.Add(new SaleCount { month = months[i], count = counts[i] });
            }            
            return (Json(items, JsonRequestBehavior.AllowGet));
        }

        
    }
}
