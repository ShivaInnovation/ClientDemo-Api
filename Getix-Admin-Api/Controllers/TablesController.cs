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
        public IQueryable<ProjectTable> GetTables()
        {
            return db.ProjectTables.AsQueryable();
        }

        [HttpGet]
        public IHttpActionResult GetTable(int id)
        {
            var employees = (from t in db.ProjectTables
                             join p in db.Projects on t.ProjectID equals p.id
                             where t.ProjectID == id
                             select new
                             {
                                 t.ProjectTableName,
                                 t.TableType,
                                 t.ProjectTableID,
                                 t.ProjectID,
                                 p.name,
                                 p.description
                             }).ToList();




            //var tables = db.ProjectTables.Where(t => t.ProjectID == id || t.ProjectTableID == id).ToList(); // do inner join with projects
            return Ok(employees);
        }

        [HttpGet]
        public IHttpActionResult GetTableColumns(int tblId)
        {
            var tableColumns = (from t in db.ProjectTables
                                join p in db.ProjectColumns on t.ProjectTableID equals p.ProjectTableID
                                where t.ProjectTableID == tblId
                                select new
                                {
                                    t.ProjectTableName,
                                    t.TableType,
                                    t.ProjectTableID,
                                    p.CoulmnName,
                                    p.Datatype,
                                    p.IsDisplay,
                                    t.ProjectID
                                }).ToList();

            return Ok(tableColumns);
        }

        [HttpPost]
        public IHttpActionResult PostTable(Table tables)
        {
            try
            {
                var existingTable = checkTableName(tables.name);
                var checkTableType = checkMainTable(tables.tableType, tables.tableType);
                if (!existingTable)
                {
                    string message = "Table already existed";
                    return BadRequest(message);
                }
                else if (!checkTableType)
                {
                    string message = "One main table for project";
                    return BadRequest(message);
                }
                else
                {
                    string connectionString = @"Data Source=NIBLP535;Initial Catalog=GetixAdminDb;Integrated Security=True";
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        var commandStr = "CREATE TABLE " + tables.name + "(" + " PRJ_TASK_ID" + " int identity(1,1) not null, ";
                        bool isFirstCol = true;
                        foreach (var item in tables.columns)
                        {
                            if (!isFirstCol) commandStr += ",";
                            commandStr += "[" + item.columnName + "]" + item.dataType;
                            isFirstCol = false;
                        }
                        commandStr += ", constraint " + "[PK_" + tables.name + "]" + " primary key clustered(PRJ_TASK_ID) " + ")";
                        using (SqlCommand command = new SqlCommand(commandStr, con))
                            command.ExecuteNonQuery();

                        con.Close();
                    }

                    ProjectTable projectTable = new ProjectTable();
                    projectTable.ProjectTableName = tables.name;
                    projectTable.ProjectID = tables.projectId;
                    projectTable.TableType = tables.tableType;
                    projectTable.RowInsertBy = "shivappa";
                    projectTable.RowInsertDate = Convert.ToDateTime(DateTime.Now.ToString("hh:mm:ss tt", System.Globalization.DateTimeFormatInfo.InvariantInfo));

                    db.ProjectTables.Add(projectTable);
                    db.SaveChanges();

                    foreach (var column in tables.columns)
                    {
                        ProjectColumn projectColumn = new ProjectColumn();
                        projectColumn.CoulmnName = column.columnName;
                        projectColumn.Datatype = column.dataType;
                        projectColumn.IsDisplay = column.isDisplay;
                        projectColumn.ProjectId = tables.projectId;
                        projectColumn.ProjectTableID = projectTable.ProjectTableID;
                        projectColumn.RowInsertDate = Convert.ToDateTime(DateTime.Now.ToString("hh:mm:ss tt", System.Globalization.DateTimeFormatInfo.InvariantInfo));
                        db.ProjectColumns.Add(projectColumn);
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
            }

            return Ok();
        }




        private bool checkTableName(string name)
        {
            var lstResult = (from table in db.ProjectTables.AsEnumerable()
                             where table.ProjectTableName == name
                             select table.ProjectTableName).ToList();
            if (lstResult.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool checkMainTable(string name, string tableType)
        {
            var lstResult = (from table in db.ProjectTables.AsEnumerable()
                             where table.TableType == name && table.TableType.ToUpper() == tableType.ToUpper()
                             select table.TableType).ToList();
            if (lstResult.Contains("Main Table"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
