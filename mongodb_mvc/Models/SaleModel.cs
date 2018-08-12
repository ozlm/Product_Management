using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mongodb_mvc.Models
{
    public class SaleModel
    {
        public List<Sale> findAll(List<Sale> modelList)
        {
            List<Sale> listSale = new List<Sale>();
            foreach (Sale model in modelList)
            {
                listSale.Add(new Sale { _id = model._id,sale_date=model.sale_date, count=model.count, sale_price=model.sale_price ,productId=model.productId});
            }

            return listSale;
        }
    }
}