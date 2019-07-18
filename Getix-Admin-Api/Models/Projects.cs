using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Getix_Admin_Api.Models
{
    public class Projects
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }        
        public string active { get; set; }
    }
}