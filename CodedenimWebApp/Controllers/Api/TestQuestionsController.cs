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
    public class TestQuestionsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/TestQuestions
        public IQueryable<TestQuestion> GetTestQuestions()
        {
            return db.TestQuestions;
        }

        /// <summary>
        /// this method will take a courseId and student email
        /// and then perform a query to get the questions
        /// </summary>
        /// <param name="id"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        // GET: api/TestQuestions/5
        [ResponseType(typeof(TestQuestion))]
        public async Task<IHttpActionResult> GetTestQuestion(int id, string email)
        {
            TestQuestion testQuestion = await db.TestQuestions.FindAsync(id); //finds the question id
            if (testQuestion == null)
            {
                return NotFound();
            }

            return Ok(testQuestion);
        }

        // PUT: api/TestQuestions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTestQuestion(int id, TestQuestion testQuestion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != testQuestion.TestQuestionId)
            {
                return BadRequest();
            }

            db.Entry(testQuestion).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestQuestionExists(id))
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

        // POST: api/TestQuestions
        [ResponseType(typeof(TestQuestion))]
        public async Task<IHttpActionResult> PostTestQuestion(TestQuestion testQuestion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TestQuestions.Add(testQuestion);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = testQuestion.TestQuestionId }, testQuestion);
        }

        // DELETE: api/TestQuestions/5
        [ResponseType(typeof(TestQuestion))]
        public async Task<IHttpActionResult> DeleteTestQuestion(int id)
        {
            TestQuestion testQuestion = await db.TestQuestions.FindAsync(id);
            if (testQuestion == null)
            {
                return NotFound();
            }

            db.TestQuestions.Remove(testQuestion);
            await db.SaveChangesAsync();

            return Ok(testQuestion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TestQuestionExists(int id)
        {
            return db.TestQuestions.Count(e => e.TestQuestionId == id) > 0;
        }
    }
}