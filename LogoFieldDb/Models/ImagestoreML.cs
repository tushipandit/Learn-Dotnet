using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogoFieldDb.Models
{
    public class ImagestoreML
    {


        public long Id { get; set; }
        public string Name { get; set; }
        public Nullable<bool> Isalive { get; set; }
        public Nullable<System.DateTime> createdon { get; set; }
        public Nullable<System.DateTime> modifiedon { get; set; }
        public string photourl { get; set; }
        public Nullable<long> CategoryId { get; set; }
        public Nullable<long> SubcateogoryId { get; set; }
        public Nullable<long> ProductId { get; set; }

        public List<Sub_CategoryML> Sub_CategoryML { get; set; }
     
    }
}