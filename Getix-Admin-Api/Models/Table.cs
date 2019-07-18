using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Getix_Admin_Api.Models
{
    public class Table
    {
        public int id { get; set; }
        public string name { get; set; }
        public string tableType { get; set; }
        public List<string> columns { get; set; }
        public int projectId { get; set; }
    }
}