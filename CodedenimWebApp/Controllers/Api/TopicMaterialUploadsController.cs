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
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: api/TopicMaterialUploads
        public IQueryable GetTopicMaterialUploads()
        {
            return _db.TopicMaterialUploads.Select(x => new
                                            {
                                                x.Description,
                                                x.FileLocation,
                                                x.Name,
                                                x.TopicMaterialUploadId,
                                               
                                                
                                            });
        }

        // GET: api/TopicMaterialUploads/5
        [ResponseType(typeof(TopicMaterialUpload))]
        public async Task<IHttpActionResult> GetTopicMaterialUpload(int id)
        {
          //  TopicMaterialUpload topicMaterialUpload = await _db.TopicMaterialUploads.FindAsync(id);
          var topicMaterialUpload = await _db.TopicMaterialUploads.Where(x => x.TopicId.Equals(id))
                                                                  .Select( x => new
                                                                    {
                                                                        x.TopicMaterialUploadId,
                                                                        x.TopicId,
                                                                        x.Description,
                                                                        x.FileLocation,
                                                                        FileType = x.FileType.ToString(),
                                                                        x.Course.StudentQuestions,
                                                                      
                                                                    }).ToListAsync();
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

            _db.Entry(topicMaterialUpload).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
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

            _db.TopicMaterialUploads.Add(topicMaterialUpload);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = topicMaterialUpload.TopicMaterialUploadId }, topicMaterialUpload);
        }

        // DELETE: api/TopicMaterialUploads/5
        [ResponseType(typeof(TopicMaterialUpload))]
        public async Task<IHttpActionResult> DeleteTopicMaterialUpload(int id)
        {
            TopicMaterialUpload topicMaterialUpload = await _db.TopicMaterialUploads.FindAsync(id);
            if (topicMaterialUpload == null)
            {
                return NotFound();
            }

            _db.TopicMaterialUploads.Remove(topicMaterialUpload);
            await _db.SaveChangesAsync();

            return Ok(topicMaterialUpload);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TopicMaterialUploadExists(int id)
        {
            return _db.TopicMaterialUploads.Count(e => e.TopicMaterialUploadId == id) > 0;
        }
    }
}