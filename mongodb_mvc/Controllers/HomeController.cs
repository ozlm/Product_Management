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

namespace mongodb_mvc.Models
{
    public class HomeController : Controller
    {
        static IMongoQuery query;
        connect cn = new connect();
        MongoDatabase mongodb = mongodb_mvc.connect.mongodb;
        IMongoQuery q = mongodb_mvc.connect.query;
    
        //// GET: /Home/
        
        public HomeController()
        {
           
        }


        public ActionResult Index()
        {
            return View();
        }

    }
}
