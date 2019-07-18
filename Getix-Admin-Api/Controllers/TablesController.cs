using Getix_Admin_Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Getix_Admin_Api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TablesController : ApiController
    {
        private GetixAdminEntities db = new GetixAdminEntities();

        [HttpGet]
        public IQueryable<TableColumn> GetTables()
        {
            return db.TableColumns;
        }

        [HttpGet]
        public IHttpActionResult GetTable(int id)
        {
            var query = from tables in db.TableColumns
                        where tables.projectId == id
                        join projects in db.Projects on
                        tables.projectId equals projects.id
                        select new { tables.name, tables.tableType };
            return Ok(query);
        }

        [HttpGet]
        [Route("api/Tables/GetTableNameById")]
        public IHttpActionResult GetTableNameById(int id)
        {
            TableColumn table = db.TableColumns.Where(t => t.id == id).FirstOrDefault();
            return Ok(table);
        }


        [HttpPost]
        public IHttpActionResult PostTable(Table table)
        {
            try
            {
                var existingTable = checkTableName(table.name);
                if (!existingTable)
                {
                    string message = "Table already created";
                    return BadRequest(message);
                }
                else
                {
                    string connectionString = @"Data Source=NIBLP535;Initial Catalog=GetixAdminDb;Integrated Security=True";
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        int insertedID;
                        var commandStr = "CREATE TABLE " + table.name + "(" + " Id" + " int identity(1,1) not null, ";
                        bool isFirstCol = true;
                        foreach (var item in table.columns)
                        {
                            if (!isFirstCol) commandStr += ",";
                            commandStr += "[" + item + "] varchar(500)";
                            isFirstCol = false;
                        }
                        commandStr += ", constraint " + "[PK_" + table.name + "]" + " primary key clustered(id) " + ")";
                        using (SqlCommand command = new SqlCommand(commandStr, con))
                            command.ExecuteNonQuery();

                        string columns = string.Join(",", table.columns);
                        //Insert QUery with Scope_Identity
                        using (SqlCommand cmd = new SqlCommand("insert into TableColumn Values(@name, @tableType, @columnNames, @projectId); SELECT SCOPE_IDENTITY()   ", con))
                        {
                            cmd.CommandType = CommandType.Text;
                            {
                                //Add parameter values
                                cmd.Parameters.AddWithValue("@name", table.name);
                                cmd.Parameters.AddWithValue("@tableType", table.tableType);
                                cmd.Parameters.AddWithValue("@columnNames", columns);
                                cmd.Parameters.AddWithValue("@projectId", table.projectId);

                                //Get the inserted query
                                insertedID = Convert.ToInt32(cmd.ExecuteScalar());
                            }
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return Ok();

        }
        private bool checkTableName(string name)
        {

            var lstResult = (from table in db.TableColumns.AsEnumerable()
                             where table.name == name
                             select table.name).ToList();
            if (lstResult.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
