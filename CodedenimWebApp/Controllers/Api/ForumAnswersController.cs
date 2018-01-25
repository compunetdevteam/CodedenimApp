using CodedenimWebApp.Models;
using CodeninModel.Forums;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace CodedenimWebApp.Controllers.Api
{
    [System.Web.Http.RoutePrefix("api/ForumAnswers")]
    public class ForumAnswersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ForumAnswers
        public IQueryable GetForumAnswers()
        {
            var forumAnswers = db.ForumAnswers
                                 .AsNoTracking()
                                 .Select(x => new
                                    {
                                     x.ForumQuestionId,
                                     x.ForumQuestions.QuestionName,
                                        x.ForumAnswerId,
                                        x.ReplyDate,
                                        x.Answer
                
                                    });
            return forumAnswers;
        }

        // GET: api/ForumAnswers/5
        [ResponseType(typeof(ForumAnswer))]
        public async Task<IHttpActionResult> GetForumAnswer(int id)
        {
            //ForumAnswer forumAnswer = await db.ForumAnswers.FindAsync(id);
            var forumAnswer = await db.ForumAnswers.Where(x => x.ForumQuestionId.Equals(id))
                                            .Select(x => new
                                            {
                                                
                                                x.ForumQuestionId,
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
        public async Task<IHttpActionResult> PostForumAnswer(string email,ForumAnswer forumAnswer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ConvertEmail convertEmail = new ConvertEmail();
            forumAnswer.UserId = convertEmail.ConvertEmailToId(email);
            forumAnswer.ReplyDate = DateTime.Now;
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