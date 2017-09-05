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
using CodeninModel;

namespace CodedenimWebApp.Controllers.Api
{
    public class TopicMaterialUploadsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/TopicMaterialUploads
        public IEnumerable<TopicMaterialUpload> GetTopicMaterialUploads()
        {
            return db.TopicMaterialUploads.ToList();
        }

        // GET: api/TopicMaterialUploads/5
        [ResponseType(typeof(TopicMaterialUpload))]
        public async Task<IHttpActionResult> GetTopicMaterialUpload(int id)
        {
            TopicMaterialUpload topicMaterialUpload = await db.TopicMaterialUploads.FindAsync(id);
            if (topicMaterialUpload == null)
            {
                return NotFound();
            }

            return Ok(topicMaterialUpload);
        }

        // PUT: api/TopicMaterialUploads/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTopicMaterialUpload(int id, TopicMaterialUpload topicMaterialUpload)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != topicMaterialUpload.TopicMaterialUploadId)
            {
                return BadRequest();
            }

            db.Entry(topicMaterialUpload).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TopicMaterialUploadExists(id))
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

        // POST: api/TopicMaterialUploads
        [ResponseType(typeof(TopicMaterialUpload))]
        public async Task<IHttpActionResult> PostTopicMaterialUpload(TopicMaterialUpload topicMaterialUpload)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TopicMaterialUploads.Add(topicMaterialUpload);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = topicMaterialUpload.TopicMaterialUploadId }, topicMaterialUpload);
        }

        // DELETE: api/TopicMaterialUploads/5
        [ResponseType(typeof(TopicMaterialUpload))]
        public async Task<IHttpActionResult> DeleteTopicMaterialUpload(int id)
        {
            TopicMaterialUpload topicMaterialUpload = await db.TopicMaterialUploads.FindAsync(id);
            if (topicMaterialUpload == null)
            {
                return NotFound();
            }

            db.TopicMaterialUploads.Remove(topicMaterialUpload);
            await db.SaveChangesAsync();

            return Ok(topicMaterialUpload);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TopicMaterialUploadExists(int id)
        {
            return db.TopicMaterialUploads.Count(e => e.TopicMaterialUploadId == id) > 0;
        }
    }
}