using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Getix_Admin_Api.Models
{
    public class ConfigModel
    {
        public int Id { get; set; }
        public List<string> Projects { get; set; }
        public List<string> Users { get; set; }

    }
}