using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class blogML
    {

        public long Id { get; set; }
        public string title { get; set; }
        public string miniintro { get; set; }
        public string description { get; set; }
        public string seotitle { get; set; }
        public string seodiscription { get; set; }
        public string seokeywords { get; set; }
        public string language { get; set; }
        public Nullable<System.DateTime> createdon { get; set; }
        public Nullable<bool> isalive { get; set; }
        public string postedby { get; set; }
        public string photo { get; set; }
        public Nullable<long> position { get; set; }
        public string seourl { get; set; }


    }
}