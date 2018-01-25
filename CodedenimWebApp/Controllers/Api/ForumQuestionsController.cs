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

namespace CodedenimWebApp.Controllers.Api.ForumApi
{
    [System.Web.Http.RoutePrefix("api/ForumQuestions")]
    public class ForumQuestionsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ForumQuestions
        public IQueryable GetForumQuestions()
        {
            var forumQuestion = db.ForumQuestions.AsNoTracking().Select(x => new
            {
                x.CourseId,
                x.ForumQuestionId,
                x.QuestionName,
                x.Title

            });
            return  forumQuestion;
        }


       // [System.Web.Http.Route("CheckIfStudentIsEnrolled")]
        // GET: api/ForumQuestions/5
        
        [ResponseType(typeof(ForumQuestion))]
        public async Task<IHttpActionResult> GetForumQuestion(int id)
        {

            var forumQuestion = await db.ForumQuestions
                                                   .AsNoTracking()
                                                  .Select(x => new {
                                                      x.CourseId,
                                                      x.ForumQuestionId,
                                                      x.PostDate,
                                                      x.QuestionName,
                                                     x.Students.FirstName,
                                                      x.Students.LastName,
                                                      x.Title,
                                                      x.ForumAnswers.Count
                                                  })
                                                  .ToListAsync();
            if (forumQuestion == null)
            {
                return NotFound();
            }

            return Ok(forumQuestion);
        }

        /// <summary>
        /// this method takes in a course id and 
        /// displays all the questions associated it
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [System.Web.Http.Route("AllForumQuestions")]
        public async Task<IHttpActionResult> AllForumQuestions(int id)
        {
           var forumQuestion = await db.ForumQuestions
                                       .Where(x => x.CourseId.Equals(id)).AsNoTracking()
                                                  .Select(x => new {
                                                      x.CourseId,
                                                      x.ForumQuestionId,
                                                      x.PostDate,
                                                      x.QuestionName,
                                                      x.Students.FirstName,
                                                      x.Students.LastName,
                                                      x.Title,
                                                      x.ForumAnswers.Count
                                                  })
                                                  .ToListAsync();
            if (forumQuestion == null)
            {
                return NotFound();
            }

            return Ok(forumQuestion);
        }

   

        // PUT: api/ForumQuestions/5
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
            forumQuestion.PostDate = DateTime.Now;
            db.Entry(forumQuestion).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
                return Ok("Updated Successfully");
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

        
        /// <summary>
        /// To post a forum quesition 
        /// a user needs to send and email address to the server 
        /// </summary>
        /// <param name="forumQuestion"></param>
        /// <returns></returns>
        // POST: api/ForumQuestions
        [ResponseType(typeof(ForumQuestion))]
        public async Task<IHttpActionResult> PostForumQuestion(string email,ForumQuestion forumQuestion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ConvertEmail convertEmail = new ConvertEmail();
             forumQuestion.StudentId  =  convertEmail.ConvertEmailToId(email);
            forumQuestion.PostDate = DateTime.Now;
            db.ForumQuestions.Add(forumQuestion);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = forumQuestion.ForumQuestionId }, forumQuestion);
        }

        // DELETE: api/ForumQuestions/5
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