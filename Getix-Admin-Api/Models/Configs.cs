using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Getix_Admin_Api.Models
{
    public class Configs
    {
        public int Config_Id { get; set; }
        public string Config_Name { get; set; }
        public int Project_Id { get; set; }
        public string RowInsertDate { get; set; }
        public string RowInsertBy { get; set; }
    }
}