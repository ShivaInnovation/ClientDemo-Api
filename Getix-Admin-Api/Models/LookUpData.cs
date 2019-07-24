using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Getix_Admin_Api.Models
{
    public class LookUpData
    {
        public List<ColumnDetails> columnDetails { get; set; }
        public string projectTableName { get; set; }
    }

    public class ColumnDetails
    {
        public string ColumnName { get; set; }
        public string ColumnData { get; set; }
        public string ProjectTableName { get; set; }
    }
}