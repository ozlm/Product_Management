using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace mongodb_mvc.Models
{
    public class Product
    {
        public ObjectId _id { get; set; }
        public int branch_number { get; set; }
        public string product_code { get; set; }
        public string brand { get; set; }
        public string product_type { get; set; }
        public double purchase_price { get; set; }
        public DateTime purchase_date { get; set; }
        public int stock { get; set; }
        
    }
}