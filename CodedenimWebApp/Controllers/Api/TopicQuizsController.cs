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
    public class TopicQuizsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/TopicQuizs  
        public  IQueryable GetTopicQuizs()
        {
            var topicQuiz = db.TopicQuizs.Select(x => new
                                                        {
                                                               x.TopicQuizId,
                                                               x.ModuleId,
                                                                
                                                               x.Question,
                                                               x.Option1,
                                                               x.Option2,
                                                               x.Option3,
                                                               x.Option4
                                                        });
            return topicQuiz;
        }

        // GET: api/TopicQuizs/5
        [ResponseType(typeof(TopicQuiz))]
        public async Task<IHttpActionResult> GetTopicQuiz(int id)
        {
            TopicQuiz topicQuiz = await db.TopicQuizs.FindAsync(id);
            if (topicQuiz == null)
            {
                return NotFound();
            }

            return Ok(topicQuiz);
        }

        // PUT: api/TopicQuizs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTopicQuiz(int id, TopicQuiz topicQuiz)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != topicQuiz.TopicQuizId)
            {
                return BadRequest();
            }

            db.Entry(topicQuiz).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TopicQuizExists(id))
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

        // POST: api/TopicQuizs
        [ResponseType(typeof(TopicQuiz))]
        public async Task<IHttpActionResult> PostTopicQuiz(TopicQuiz topicQuiz)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TopicQuizs.Add(topicQuiz);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = topicQuiz.TopicQuizId }, topicQuiz);
        }

        // DELETE: api/TopicQuizs/5
        [ResponseType(typeof(TopicQuiz))]
        public async Task<IHttpActionResult> DeleteTopicQuiz(int id)
        {
            TopicQuiz topicQuiz = await db.TopicQuizs.FindAsync(id);
            if (topicQuiz == null)
            {
                return NotFound();
            }

            db.TopicQuizs.Remove(topicQuiz);
            await db.SaveChangesAsync();

            return Ok(topicQuiz);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TopicQuizExists(int id)
        {
            return db.TopicQuizs.Count(e => e.TopicQuizId == id) > 0;
        }
    }
}