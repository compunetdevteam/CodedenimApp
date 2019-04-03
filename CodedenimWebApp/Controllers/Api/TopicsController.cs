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
using CodedenimWebApp.Constants;
using CodedenimWebApp.Controllers.Api.ApiViewModel;
using CodedenimWebApp.Models;
using CodeninModel.Quiz;

namespace CodedenimWebApp.Controllers.Api
{
    [RoutePrefix("api/Topics")]
    public class TopicsController : ApiController
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: api/Topics
        [Route("GetTopics")]
        public IQueryable GetTopics()
        {
            //return db.Topics.Include(t => t.MaterialUploads).ToList();
            return _db.Topics.Include(x => x.MaterialUploads).Select(x => new TopicVm
            {
                ExpectedTime = x.ExpectedTime,
                ModuleId = x.ModuleId,
                TopicId = x.TopicId,
                TopicName = x.TopicName,
                TopicMaterialVm = x.MaterialUploads.Select(t => new TopicMaterialVm {
                    TopicId = t.TopicId,
                    TopicMaterialId = t.TopicMaterialUploadId,
                    Name  = t.Name,
                    Description =  t.Description,
                    FileLocation = t.FileLocation,
                    FileType = t.FileType.ToString(),
                    TextContent = t.TextContent,
                })
               
             
            });
        }

        // GET: api/Topics/5
        [ResponseType(typeof(Topic))]

        public async Task<IHttpActionResult> GetTopic(int id)
        {
            //Topic topic = await db.Topics.FindAsync(id);
            var topic = await _db.Topics.Where(t => t.ModuleId.Equals(id))
                                               .Select(x => new TopicVm
                                               {
                                                   ExpectedTime = x.ExpectedTime,
                                                   ModuleId = x.ModuleId,
                                                   TopicId = x.TopicId,
                                                   TopicName = x.TopicName,
                                                   TopicMaterialVm = x.MaterialUploads.Select(t => new TopicMaterialVm
                                                   {
                                                       TopicId = t.TopicId,
                                                       TopicMaterialId = t.TopicMaterialUploadId,
                                                       Name = t.Name,
                                                       Description = t.Description,
                                                       FileLocation =  t.FileLocation,
                                                       FileType = t.FileType.ToString(),
                                                       TextContent = t.TextContent,
                                                   })
                                               }).FirstOrDefaultAsync();
            if (topic == null)
            {
                return NotFound();
            }

            return Ok(topic);
        }

        //// PUT: api/Topics/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutTopic(int id, Topic topic)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != topic.TopicId)
        //    {
        //        return BadRequest();
        //    }

        //    _db.Entry(topic).State = EntityState.Modified;

        //    try
        //    {
        //        await _db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!TopicExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/Topics
        //[ResponseType(typeof(Topic))]
        //public async Task<IHttpActionResult> PostTopic(Topic topic)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    _db.Topics.Add(topic);
        //    await _db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = topic.TopicId }, topic);
        //}

        //// DELETE: api/Topics/5
        //[ResponseType(typeof(Topic))]
        //public async Task<IHttpActionResult> DeleteTopic(int id)
        //{
        //    Topic topic = await _db.Topics.FindAsync(id);
        //    if (topic == null)
        //    {
        //        return NotFound();
        //    }

        //    _db.Topics.Remove(topic);
        //    await _db.SaveChangesAsync();

        //    return Ok(topic);
        //}

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
            return _db.Topics.Count(e => e.TopicId == id) > 0;
        }
    }
}