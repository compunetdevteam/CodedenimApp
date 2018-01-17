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
using CodeninModel.Forums;

namespace CodedenimWebApp.Controllers.Api.ForumApi
{
    public class ForumAnswersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ForumAnswers
        public IQueryable<ForumAnswer> GetForumAnswers()
        {
            return db.ForumAnswers;
        }

        // GET: api/ForumAnswers/5
        [ResponseType(typeof(ForumAnswer))]
        public async Task<IHttpActionResult> GetForumAnswer(int id)
        {
            //ForumAnswer forumAnswer = await db.ForumAnswers.FindAsync(id);
            var forumAnswer = await db.ForumAnswers.Where(x => x.ForumQuestionId.Equals(id))
                                            .Select(x => new
                                            {
                                                x.ReplyDate,
                                                x.Id,
                                                x.ForumQuestions.QuestionName,
                                                x.Answer,
                                                x.UserId,
                                                x.VoteForumAnswers
                                            }).ToListAsync();
            if (forumAnswer == null)
            {
                return NotFound();
            }

            return Ok(forumAnswer);
        }

        // PUT: api/ForumAnswers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutForumAnswer(int id, ForumAnswer forumAnswer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != forumAnswer.Id)
            {
                return BadRequest();
            }

            db.Entry(forumAnswer).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ForumAnswerExists(id))
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

        // POST: api/ForumAnswers
        [ResponseType(typeof(ForumAnswer))]
        public async Task<IHttpActionResult> PostForumAnswer(ForumAnswer forumAnswer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ForumAnswers.Add(forumAnswer);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = forumAnswer.Id }, forumAnswer);
        }

        // DELETE: api/ForumAnswers/5
        [ResponseType(typeof(ForumAnswer))]
        public async Task<IHttpActionResult> DeleteForumAnswer(int id)
        {
            ForumAnswer forumAnswer = await db.ForumAnswers.FindAsync(id);
            if (forumAnswer == null)
            {
                return NotFound();
            }

            db.ForumAnswers.Remove(forumAnswer);
            await db.SaveChangesAsync();

            return Ok(forumAnswer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ForumAnswerExists(int id)
        {
            return db.ForumAnswers.Count(e => e.Id == id) > 0;
        }
    }
}