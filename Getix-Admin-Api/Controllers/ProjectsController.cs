using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Getix_Admin_Api.Models;

namespace Getix_Admin_Api.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProjectsController : ApiController
    {
        private GetixAdminEntities db = new GetixAdminEntities();

        // GET: api/Projects
        public IQueryable<Project> GetProjects()
        {
            return db.Projects
            .Include(c => c.TableColumns).OrderBy(c => c.id);
        }

        // GET: api/Projects/5
        [ResponseType(typeof(Project))]
        public IHttpActionResult GetProject(int id)
       {
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }

        // PUT: api/Projects/5
        [HttpPut]
        public IHttpActionResult PutProject(int id, Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != project.id)
            {
                return BadRequest();
            }

            db.Entry(project).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Projects
        [HttpPost]
        public IHttpActionResult PostProject(Projects project)
        {
            string connectionString = @"Data Source=NIBLP535;Initial Catalog=GetixAdminDb;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                int insertedID;               
                using (SqlCommand cmd = new SqlCommand("insert into Projects Values(@name, @description, @active); SELECT SCOPE_IDENTITY()   ", con))
                {
                    cmd.CommandType = CommandType.Text;
                    {
                        //Add parameter values
                        cmd.Parameters.AddWithValue("@name", project.name);
                        cmd.Parameters.AddWithValue("@description", project.description);                        
                        cmd.Parameters.AddWithValue("@active", project.active);                        

                        //Get the inserted query
                        insertedID = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                
            }
            return Ok();
        }

        // DELETE: api/Projects/5
        [ResponseType(typeof(Project))]
        public IHttpActionResult DeleteProject(int id)
        {
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            db.Projects.Remove(project);
            db.SaveChanges();

            return Ok(project);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProjectExists(int id)
        {
            return db.Projects.Count(e => e.id == id) > 0;
        }
    }
}