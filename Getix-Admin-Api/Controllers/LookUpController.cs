using Getix_Admin_Api.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Getix_Admin_Api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LookUpController : ApiController
    {
        [HttpPost]
        public IHttpActionResult PostLookupData(LookUpData lookUpData)
        {
            string connectionString = @"Data Source=NIBLP535;Initial Catalog=GetixAdminDb;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                var commandStr = "INSERT INTO " + lookUpData.projectTableName + "(";               
                foreach (var columnData in lookUpData.columnDetails)
                {
                   commandStr += columnData.ColumnName + ",";
                }
                commandStr = commandStr.TrimEnd(',');
                commandStr += ")" + "VALUES" + "(";              
                foreach (var columnData in lookUpData.columnDetails)
                {
                    commandStr += "'"+ columnData.ColumnData + "',";
                }
                commandStr = commandStr.TrimEnd(',');
                commandStr += ")";
                try
                {
                    using (SqlCommand command = new SqlCommand(commandStr, con))
                    {
                        con.Open();

                        command.ExecuteNonQuery();

                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return Ok();
        }

    }
}
