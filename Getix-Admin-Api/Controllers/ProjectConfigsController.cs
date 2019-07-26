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
        public IQueryable<Project_Config> GetProjectConfigs()
        {
            return db.Project_Config;
        }

        // GET: api/ProjectConfigs/5
       
        public IHttpActionResult GetProjectConfig(int id)
        {
            Project_Config projectConfig = db.Project_Config.Find(id);
            if (projectConfig == null)
            {
                return NotFound();
            }

            return Ok(projectConfig);
        }

        // PUT: api/ProjectConfigs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProjectConfig(int id, Project_Config projectConfig)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != projectConfig.Project_Config_Id)
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

            Project_Config projectConfig = new Project_Config();
            //projectConfig.projects = string.Join(",", configModel.Projects);
            //projectConfig.users = string.Join(",", configModel.Users);

            //db.ProjectConfigs.Add(projectConfig);
            //db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = projectConfig.Project_ID }, projectConfig);
        }

        // DELETE: api/ProjectConfigs/5
      
        public IHttpActionResult DeleteProjectConfig(int id)
        {
            Project_Config projectConfig = db.Project_Config.Find(id);
            if (projectConfig == null)
            {
                return NotFound();
            }

            db.Project_Config.Remove(projectConfig);
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
            return db.Project_Config.Count(e => e.Project_Config_Id == id) > 0;
        }
    }
}