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
using CodeninModel;

namespace CodedenimWebApp.Controllers.Api
{
    [System.Web.Http.RoutePrefix("api/EnrollForCourses")]
    public class EnrollForCoursesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/EnrollForCourses
        public IQueryable<EnrollForCourse> GetEnrollForCourses()
        {
            return db.EnrollForCourses;
        }


        // GET: api/EnrollForCourses/5
        //this method gets the list of courses that the student has enrolled into 
        [HttpGet]
        [System.Web.Http.Route("EnrolledForCourses")]
        [ResponseType(typeof(EnrollForCourse))]
        public async Task<IHttpActionResult> GetEnrolledForCourses(string email)
        {

            var studentEmail = new ConvertEmail1();
            var studentId = studentEmail.ConvertEmailToId(email);
            var enrollCategoryId = db.EnrollForCourses.AsNoTracking().Where(x => x.StudentId.Equals(studentId))
                                     .Select(x => x.CourseCategoryId).FirstOrDefault();

            var PaidCourses = db.EnrollForCourses.AsNoTracking().Where(x => x.StudentId.Equals(studentId))
                .Select(x => x.CourseCategoryId).FirstOrDefault();

            var courses = await db.AssignCourseCategories.AsNoTracking()
                .Where(x => x.CourseCategoryId.Equals(enrollCategoryId)).ToListAsync();
            //EnrollForCourse enrollForCourse = await db.EnrollForCourses.FindAsync(email);
            //if (enrollForCourse == null)
            //{
            //    return NotFound();
            //}

            //return Ok(enrollForCourse);
            return Ok(courses);
        }
        // GET: api/EnrollForCourses/5
        [ResponseType(typeof(EnrollForCourse))]
        public async Task<IHttpActionResult> GetEnrollForCourse(int id)
        {
            EnrollForCourse enrollForCourse = await db.EnrollForCourses.FindAsync(id);
            if (enrollForCourse == null)
            {
                return NotFound();
            }

            return Ok(enrollForCourse);
        }

        // PUT: api/EnrollForCourses/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEnrollForCourse(int id, EnrollForCourse enrollForCourse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != enrollForCourse.Id)
            {
                return BadRequest();
            }

            db.Entry(enrollForCourse).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnrollForCourseExists(id))
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

        [HttpPost]
        // POST: api/EnrollForCourses
        [ResponseType(typeof(EnrollForCourse))]
        public async Task<IHttpActionResult> PostEnrollForCourse(EnrollForCourse enrollForCourse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var studentEmail = new ConvertEmail();
            var studentId = studentEmail.ConvertEmailToId(enrollForCourse.StudentId);
            enrollForCourse.StudentId = studentId;
            db.EnrollForCourses.Add(enrollForCourse);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = enrollForCourse.Id }, enrollForCourse);
        }

        // DELETE: api/EnrollForCourses/5
        [ResponseType(typeof(EnrollForCourse))]
        public async Task<IHttpActionResult> DeleteEnrollForCourse(int id)
        {
            EnrollForCourse enrollForCourse = await db.EnrollForCourses.FindAsync(id);
            if (enrollForCourse == null)
            {
                return NotFound();
            }

            db.EnrollForCourses.Remove(enrollForCourse);
            await db.SaveChangesAsync();

            return Ok(enrollForCourse);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EnrollForCourseExists(int id)
        {
            return db.EnrollForCourses.Count(e => e.Id == id) > 0;
        }
    }
}