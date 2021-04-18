using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogoFieldDb.Models
{
    public class ProductML
    {

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SEOKeyword { get; set; }
        public string SEOUrl { get; set; }
        public Nullable<bool> IsAlive { get; set; }
        public Nullable<System.DateTime> createon { get; set; }
        public Nullable<System.DateTime> modifiedon { get; set; }
        public string skucode { get; set; }
        public string itemcode { get; set; }
        public string brandname { get; set; }
        public string features { get; set; }
        public Nullable<bool> stockstaus { get; set; }
        public Nullable<long> stockquantity { get; set; }
        public Nullable<long> minquantity { get; set; }
        public Nullable<decimal> price_mrp { get; set; }
        public Nullable<bool> price_mrp_display { get; set; }
        public Nullable<bool> showonhome { get; set; }
        public Nullable<decimal> tax_percentage { get; set; }
        public Nullable<long> ImageId { get; set; }
        public Nullable<long> SubCategoryId { get; set; }
        public string SEOTitle { get; set; }

    }
}