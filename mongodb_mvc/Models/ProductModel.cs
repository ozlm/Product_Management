using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mongodb_mvc.Models
{
    public class ProductModel
    {
        public List<Product> findAll(List<Product> modelList)
        {
            List<Product> listProducts = new List<Product>();
            foreach(Product model in modelList){
                 listProducts.Add(new Product { _id=model._id,branch_number=model.branch_number,product_code=model.product_code,brand=model.brand,product_type=model.product_type,purchase_price=model.purchase_price,purchase_date=model.purchase_date,stock=model.stock});  
            }
                   
            return listProducts;
        }
    }
}