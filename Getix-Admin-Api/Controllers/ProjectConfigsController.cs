using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
    public class ProjectConfigsController : ApiController
    {
        private GetixAdminEntities db = new GetixAdminEntities();

        // GET: api/ProjectConfigs
        public IQueryable<ProjectConfig> GetProjectConfigs()
        {
            return db.ProjectConfigs;
        }

        // GET: api/ProjectConfigs/5
        [ResponseType(typeof(ProjectConfig))]
        public IHttpActionResult GetProjectConfig(int id)
        {
            ProjectConfig projectConfig = db.ProjectConfigs.Find(id);
            if (projectConfig == null)
            {
                return NotFound();
            }

            return Ok(projectConfig);
        }

        // PUT: api/ProjectConfigs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProjectConfig(int id, ProjectConfig projectConfig)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != projectConfig.id)
            {
                return BadRequest();
            }

            db.Entry(projectConfig).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectConfigExists(id))
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

        // POST: api/ProjectConfigs
        [HttpPost]
        public IHttpActionResult PostProjectConfig(ConfigModel configModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ProjectConfig projectConfig = new ProjectConfig();
            projectConfig.projects = string.Join(",", configModel.Projects);
            projectConfig.users = string.Join(",", configModel.Users);

            db.ProjectConfigs.Add(projectConfig);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = projectConfig.id }, projectConfig);
        }

        // DELETE: api/ProjectConfigs/5
        [ResponseType(typeof(ProjectConfig))]
        public IHttpActionResult DeleteProjectConfig(int id)
        {
            ProjectConfig projectConfig = db.ProjectConfigs.Find(id);
            if (projectConfig == null)
            {
                return NotFound();
            }

            db.ProjectConfigs.Remove(projectConfig);
            db.SaveChanges();

            return Ok(projectConfig);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProjectConfigExists(int id)
        {
            return db.ProjectConfigs.Count(e => e.id == id) > 0;
        }
    }
}