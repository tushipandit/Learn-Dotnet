﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class userML
    {
        public int AutoId { get; set; }
        public string username { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string emailId { get; set; }
        public string idtype { get; set; }
        public string idnumber { get; set; }
        public string phone_no { get; set; }
        public string password { get; set; }
        public Nullable<bool> isblocked { get; set; }
        public string website_link { get; set; }
        public string Location { get; set; }
        public string industry { get; set; }
        public string established_year { get; set; }
        public string GSTN { get; set; }
        public Nullable<int> total_students { get; set; }
        public Nullable<int> role { get; set; }
        public Nullable<int> type { get; set; }
        public Nullable<System.DateTime> createdon { get; set; }
        public string url { get; set; }
    }
}