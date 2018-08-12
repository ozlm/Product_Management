using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace mongodb_mvc.Models
{
    public class Sale
    {
        public ObjectId _id { get; set; }
        public DateTime sale_date { get; set; }
        public int count { get; set; }
        public double sale_price { get; set; }
        public ObjectId productId { get; set; }
    }
}