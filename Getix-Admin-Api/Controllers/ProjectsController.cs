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
        public IEnumerable<Project> GetProjects()
        {
            return db.Projects.ToList();           
        }

        // GET: api/Projects/5
        [HttpGet]
        public IHttpActionResult GetProject(int id)
       {
            var projectColumns = db.ProjectColumns.Where(p => p.ProjectTableID == id).ToList();
            if (projectColumns == null)
            {
                return NotFound();
            }
            return Ok(projectColumns);
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Project projectData = new Project();
            projectData.name = project.name;
            projectData.description = project.description;
            projectData.active = project.active;
            projectData.ModifiedBy = "shivappa"; //project.modifiedBy;
            projectData.ModifiedDate = Convert.ToDateTime(DateTime.Now.ToString("hh:mm:ss tt", System.Globalization.DateTimeFormatInfo.InvariantInfo));

            db.Projects.Add(projectData);
            db.SaveChanges();
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