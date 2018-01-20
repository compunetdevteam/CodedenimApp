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
using CodedenimWebApp.Controllers.Api;
using CodedenimWebApp.Models;
using CodeninModel;

namespace CodedenimWebApp.Controllers
{
    [System.Web.Http.RoutePrefix("api/AccountApi")]
    public class CourseRatingsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/CourseRatings
        [HttpGet]
        [System.Web.Http.Route("CourseRatings")]
        public Rating GetCourseRatings()
        {
            var rating = new Rating();
            rating.Likes = db.CourseRatings.Count(x => x.Rating == 1);
            rating.Dislikes = db.CourseRatings.Count(x => x.Dislike == 1);
       //     var rating = new Rating{ Likes =  like, Dislikes = dislikes};

            return rating ;
        }

        [HttpPost]
        [System.Web.Http.Route("Rating")]
        public async Task Rating(int like, int dislike, int courseId)
        {

            var courseRating = new CourseRating();
            var isCourseIdExist = db.CourseRatings.Any(x => x.CourseId.Equals(courseId));
            if (!isCourseIdExist)
            {
                var newCourse = db.Courses.Where(x => x.CourseId.Equals(courseId)).Select(x => x.CourseId)
                    .FirstOrDefault();
                courseRating.CourseId = newCourse;
                if (like == 1)
                {

                    courseRating.Rating = 1;
                    courseRating.Dislike = 0;
                }
                else
                {
                    courseRating.Rating = 0;
                    courseRating.Dislike = 1;
                }

               


            }
            else
            {
                courseRating = db.CourseRatings.FirstOrDefault(x => x.CourseId.Equals(courseId));
                if (like == 1)
                {

                    courseRating.Rating = 1;
                    courseRating.Dislike = 0;
                }
                else
                {
                    courseRating.Rating = 0;
                    courseRating.Dislike = 1;
                }
            }

            db.CourseRatings.Add(courseRating);
            db.SaveChanges();
            
        }

        // GET: api/CourseRatings/5
            [ResponseType(typeof(CourseRating))]
        public async Task<IHttpActionResult> GetCourseRating(int id)
        {
            CourseRating courseRating = await db.CourseRatings.FindAsync(id);
            if (courseRating == null)
            {
                return NotFound();
            }

            return Ok(courseRating);
        }

        // PUT: api/CourseRatings/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCourseRating(int id, CourseRating courseRating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != courseRating.CourseRatingId)
            {
                return BadRequest();
            }

            db.Entry(courseRating).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseRatingExists(id))
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

        // POST: api/CourseRatings
        [ResponseType(typeof(CourseRating))]
        public async Task<IHttpActionResult> PostCourseRating(CourseRating courseRating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CourseRatings.Add(courseRating);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = courseRating.CourseRatingId }, courseRating);
        }

        // DELETE: api/CourseRatings/5
        [ResponseType(typeof(CourseRating))]
        public async Task<IHttpActionResult> DeleteCourseRating(int id)
        {
            CourseRating courseRating = await db.CourseRatings.FindAsync(id);
            if (courseRating == null)
            {
                return NotFound();
            }

            db.CourseRatings.Remove(courseRating);
            await db.SaveChangesAsync();

            return Ok(courseRating);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CourseRatingExists(int id)
        {
            return db.CourseRatings.Count(e => e.CourseRatingId == id) > 0;
        }
    }

 
}