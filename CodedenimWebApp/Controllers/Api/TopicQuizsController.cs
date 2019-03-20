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
using CodeninModel.Quiz;

namespace CodedenimWebApp.Controllers.Api
{

    [System.Web.Http.RoutePrefix("api/Quiz")]
    public class TopicQuizsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/TopicQuizs  
        [Route("GetQuiz")]
        public  IEnumerable<QuizVm> GetTopicQuizs()
        {
            var topicQuiz = db.TopicQuizs.Select(x => new QuizVm
                                                        {
                                                               TopicQuizId = x.TopicQuizId,
                                                               ModuleId = x.ModuleId,
                                                               Question = x.Question,
                                                               Option1 = x.Option1,
                                                               Option2 = x.Option2,
                                                               Option3 = x.Option3,
                                                               Option4 = x.Option4,
                                                               Answer = x.Answer
                                                        }).ToList();
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