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
using CodedenimWebApp.Models;
using CodeninModel.Quiz;

namespace CodedenimWebApp.Controllers.Api
{
    public class TopicsController : ApiController
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: api/Topics
        public IQueryable GetTopics()
        {
            //return db.Topics.Include(t => t.MaterialUploads).ToList();
            return _db.Topics.Select(x => new
            {
                x.ExpectedTime,
                x.ModuleId,
                x.Id,
                x.TopicName,
               
             
            });
        }

        // GET: api/Topics/5
        [ResponseType(typeof(Topic))]

        public async Task<IHttpActionResult> GetTopic(int id)
        {
            //Topic topic = await db.Topics.FindAsync(id);
            var topic = await _db.Topics.Where(t => t.ModuleId.Equals(id))
                                                .Select(t => new
                                                {
                                                    t.Id,
                                                    t.TopicName,
                                                    t.ExpectedTime,
                                                }).ToListAsync();
            if (topic == null)
            {
                return NotFound();
            }

            return Ok(topic);
        }

        // PUT: api/Topics/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTopic(int id, Topic topic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != topic.Id)
            {
                return BadRequest();
            }

            _db.Entry(topic).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TopicExists(id))
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

        // POST: api/Topics
        [ResponseType(typeof(Topic))]
        public async Task<IHttpActionResult> PostTopic(Topic topic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Topics.Add(topic);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = topic.Id }, topic);
        }

        // DELETE: api/Topics/5
        [ResponseType(typeof(Topic))]
        public async Task<IHttpActionResult> DeleteTopic(int id)
        {
            Topic topic = await _db.Topics.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }

            _db.Topics.Remove(topic);
            await _db.SaveChangesAsync();

            return Ok(topic);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TopicExists(int id)
        {
            return _db.Topics.Count(e => e.Id == id) > 0;
        }
    }
}