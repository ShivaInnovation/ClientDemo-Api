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
    public class ConfigsController : ApiController
    {
        private GetixAdminEntities db = new GetixAdminEntities();

        // GET: api/Configs
        public IQueryable<Config> GetConfigs()
        {
            return db.Configs;
        }

        [HttpGet]
        public IHttpActionResult GetConfigDetails(int Id)
        {
            var configs = db.Configs.Where(c => c.Project_Id == Id).ToList();
            return Ok(configs);
        }

        [HttpGet]
        public IHttpActionResult GetConfig(string userName)
        {
            var config = (from prj in db.Projects
                                join pconf in db.Project_Config on prj.id equals pconf.Project_ID
                                join usr in db.Users on pconf.User_Id equals usr.User_id
                                select new
                                {
                                    prj.name,
                                    prj.id
                                }).ToList();

            return Ok(config);
        }

        // PUT: api/Configs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutConfig(int id, Config config)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != config.Config_Id)
            {
                return BadRequest();
            }

            db.Entry(config).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConfigExists(id))
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

        // POST: api/Configs
        [ResponseType(typeof(Config))]
        public IHttpActionResult PostConfig(Configs config)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Config configs = new Config();
            configs.Config_Name = config.Config_Name;
            configs.Project_Id = config.Project_Id;
            configs.RowInsertBy = "shivappa";
            configs.RowInsertDate = Convert.ToDateTime(DateTime.Now.ToString("hh:mm:ss tt", System.Globalization.DateTimeFormatInfo.InvariantInfo));

            db.Configs.Add(configs);
            db.SaveChanges();

            return Ok();
        }

        // DELETE: api/Configs/5
        [ResponseType(typeof(Config))]
        public IHttpActionResult DeleteConfig(int id)
        {
            Config config = db.Configs.Find(id);
            if (config == null)
            {
                return NotFound();
            }

            db.Configs.Remove(config);
            db.SaveChanges();

            return Ok(config);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ConfigExists(int id)
        {
            return db.Configs.Count(e => e.Config_Id == id) > 0;
        }
    }
}