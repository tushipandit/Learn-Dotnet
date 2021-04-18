﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogoFieldDb.Models
{
    public class Sub_CategoryML
    {

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SEOKeyword { get; set; }
        public string SEOUrl { get; set; }
        public Nullable<bool> IsAlive { get; set; }
        public Nullable<bool> showinmenu { get; set; }
        public Nullable<System.DateTime> createdon { get; set; }
        public Nullable<System.DateTime> modifiedon { get; set; }
        public Nullable<long> ImageId { get; set; }
        public Nullable<long> CategoryId { get; set; }
        public string SEOTitle { get; set; }

        public List<servicesML> ServicesML { get; set; }
    }
}