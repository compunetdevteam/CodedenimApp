using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CodedenimWebApp.Controllers.Api.ApiViewModel;
using CodedenimWebApp.Models;
using CodeninModel;

namespace CodedenimWebApp.Controllers.Api
{
    [System.Web.Http.RoutePrefix("api/Modules")]
    public class ModulesController : ApiController
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: api/Modules
        [Route("GetModules")]
        public IQueryable GetModules()
        {
            // return db.Modules.Include(m => m.Topics).ToList();
            var topicVm = new List<TopicVm>();

            return _db.Modules.Include(x => x.Topics).Select(x => new ModuleVm
            {
                CourseId = x.CourseId,
                ExpectedTime = x.ExpectedTime,
                ModuleDescription = x.ModuleDescription,
                ModuleId = x.ModuleId,
                ModuleName = x.ModuleName,
               Topics = x.Topics.Select(t => new TopicVm {
                   TopicId = t.TopicId,
                   ModuleId = t.ModuleId,
                   TopicName = t.TopicName,
                   ExpectedTime = t.ExpectedTime
               })
            });
        }


/// <summary>
/// these method takes the id of a course from the android app
/// and select the specific modules connected to that course
/// </summary>
/// <param name="id"></param>
/// <returns></returns>
        // GET: api/Modules/5
        [ResponseType(typeof(Module))]
        public async Task<IHttpActionResult> GetModule(int id)
        {
            //Module module = await db.Modules.FindAsync(id);
            var module = await _db.Modules.Where(c => c.CourseId.Equals(id))
                                        .Select(m => new ModuleVm
                                        {
                                            ModuleId = m.ModuleId,
                                            ModuleName = m.ModuleName,
                                            ModuleDescription = m.ModuleDescription,
                                            ExpectedTime = m.ExpectedTime,
                                            CourseId = m.CourseId,
                                            Topics = m.Topics.Select(t => new TopicVm
                                            {
                                                TopicId = t.TopicId,
                                                ModuleId = t.ModuleId,
                                                TopicName = t.TopicName,
                                                ExpectedTime = t.ExpectedTime
                                            })
                                        }).ToListAsync();
            //var module = await _db.Topics.Where(t => t.ModuleId.Equals(id))
            //                                    .Select(t => new
            //                                    {
            //                                        t.ModuleId,
            //                                        t.Module.ModuleName,
            //                                        t.TopicId,
            //                                        t.TopicName,
            //                                        t.ExpectedTime,
            //                                        t.StudentQuestions.Count
            //                                    }).ToListAsync();
            //                            }).ToListAsync();
            if (module == null)
            {
                return NotFound();
            }

            return Ok(module);
        }



        // PUT: api/Modules/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutModule(int id, Module module)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != module.ModuleId)
            {
                return BadRequest();
            }

            _db.Entry(module).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModuleExists(id))
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

        // POST: api/Modules
        [ResponseType(typeof(Module))]
        public async Task<IHttpActionResult> PostModule(Module module)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Modules.Add(module);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = module.ModuleId }, module);
        }

        // DELETE: api/Modules/5
        [ResponseType(typeof(Module))]
        public async Task<IHttpActionResult> DeleteModule(int id)
        {
            Module module = await _db.Modules.FindAsync(id);
            if (module == null)
            {
                return NotFound();
            }

            _db.Modules.Remove(module);
            await _db.SaveChangesAsync();

            return Ok(module);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ModuleExists(int id)
        {
            return _db.Modules.Count(e => e.ModuleId == id) > 0;
        }
    }
}