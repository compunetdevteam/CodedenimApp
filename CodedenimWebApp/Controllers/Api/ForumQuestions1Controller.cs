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

namespace CodedenimWebApp.Controllers.Api
{
    public class ForumQuestions1Controller : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ForumQuestions1
        public IQueryable GetForumQuestions()
        {
             var fora = db.Fora.Include(f => f.Course).Include(f => f.ForumView);
            return fora;
            //return db.ForumQuestions;
        }

        // GET: api/ForumQuestions1/5
        [ResponseType(typeof(ForumQuestion))]
        public async Task<IHttpActionResult> GetForumQuestion(int id)
        {
            ForumQuestion forumQuestion = await db.ForumQuestions.FindAsync(id);
            if (forumQuestion == null)
            {
                return NotFound();
            }

            return Ok(forumQuestion);
        }

        // PUT: api/ForumQuestions1/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutForumQuestion(int id, ForumQuestion forumQuestion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != forumQuestion.ForumQuestionId)
            {
                return BadRequest();
            }

            db.Entry(forumQuestion).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ForumQuestionExists(id))
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

        // POST: api/ForumQuestions1
        [ResponseType(typeof(ForumQuestion))]
        public async Task<IHttpActionResult> PostForumQuestion(ForumQuestion forumQuestion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ForumQuestions.Add(forumQuestion);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = forumQuestion.ForumQuestionId }, forumQuestion);
        }

        // DELETE: api/ForumQuestions1/5
        [ResponseType(typeof(ForumQuestion))]
        public async Task<IHttpActionResult> DeleteForumQuestion(int id)
        {
            ForumQuestion forumQuestion = await db.ForumQuestions.FindAsync(id);
            if (forumQuestion == null)
            {
                return NotFound();
            }

            db.ForumQuestions.Remove(forumQuestion);
            await db.SaveChangesAsync();

            return Ok(forumQuestion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ForumQuestionExists(int id)
        {
            return db.ForumQuestions.Count(e => e.ForumQuestionId == id) > 0;
        }
    }
}