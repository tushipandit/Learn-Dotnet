using LogoFieldDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogoFieldDb.Api_Data
{
    public class ProductData
    {
        LogoFieldDbEntities db = new LogoFieldDbEntities();

        public List<ProductML> GetProduct()
        {
            try
            {
                List<ProductML> pml = (from li in db.Products
                                       select new ProductML
                                       {
                                           Id = li.Id,
                                           Name = li.Name,
                                           Description = li.Description
                                       }
                                      ).ToList();

                return pml;
            }
            catch(Exception ex)
            {
                throw (ex);
            }
        }

        public List<ProductML> Getproductcoupon()
        {
            try
            {

                List<ProductML> val = (from li in db.Products.Where(x => x.IsAlive == true)
                                       join c in db.Coupons on li.couponid equals c.Couponid
                                       select new ProductML
                                       {
                                           Name = li.Name
                                       }
                                     ).ToList();
                return (val);
            }
            catch(Exception ex)
            {
                throw (ex);
            }
           
        }
    }
}