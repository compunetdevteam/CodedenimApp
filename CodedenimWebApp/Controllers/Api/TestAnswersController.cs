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
    [System.Web.Http.RoutePrefix("api/QuizAnswers")]
    public class TestAnswersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/TestAnswers
        public IQueryable<TestAnswer> GetTestAnswers()
        {
            return db.TestAnswers;
        }

        // GET: api/TestAnswers/5
        [ResponseType(typeof(TestAnswer))]
        public async Task<IHttpActionResult> GetTestAnswer(int id)
        {
            TestAnswer testAnswer = await db.TestAnswers.FindAsync(id);
            if (testAnswer == null)
            {
                return NotFound();
            }

            return Ok(testAnswer);
        }

        // PUT: api/TestAnswers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTestAnswer(int id, TestAnswer testAnswer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != testAnswer.TestAnswerId)
            {
                return BadRequest();
            }

            db.Entry(testAnswer).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestAnswerExists(id))
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

        // POST: api/TestAnswers
        [ResponseType(typeof(TestAnswer))]
        [Route("PostAnswer")]
        public async Task<IHttpActionResult> PostTestAnswer(TestAnswer testAnswer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TestAnswers.Add(testAnswer);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = testAnswer.TestAnswerId }, testAnswer);
        }

        // DELETE: api/TestAnswers/5
        [ResponseType(typeof(TestAnswer))]
        public async Task<IHttpActionResult> DeleteTestAnswer(int id)
        {
            TestAnswer testAnswer = await db.TestAnswers.FindAsync(id);
            if (testAnswer == null)
            {
                return NotFound();
            }

            db.TestAnswers.Remove(testAnswer);
            await db.SaveChangesAsync();

            return Ok(testAnswer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TestAnswerExists(int id)
        {
            return db.TestAnswers.Count(e => e.TestAnswerId == id) > 0;
        }
    }
}