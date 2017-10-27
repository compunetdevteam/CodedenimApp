using System;
using System.Collections.Generic;
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
    public class StudentAssignedCoursesController : ApiController
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: api/StudentAssignedCourses
        public IEnumerable<StudentAssignedCourse> GetStudentAssignedCourses()
        {
            return _db.StudentAssignedCourses.ToList();
        }
        
        // GET: api/StudentAssignedCourses/5
        [ResponseType(typeof(StudentAssignedCourse))]
        public async Task<IHttpActionResult> GetStudentAssignedCourse(int id)
        {
            StudentAssignedCourse studentAssignedCourse = await _db.StudentAssignedCourses.FindAsync(id);
            if (studentAssignedCourse == null)
            {
                return NotFound();
            }

            return Ok(studentAssignedCourse);
        }

        // PUT: api/StudentAssignedCourses
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStudentAssignedCourse(StudentAssignedCourse studentAssignedCourse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (studentAssignedCourse  == null)
            {
                return BadRequest();
            }

            _db.Entry(studentAssignedCourse).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //if (!StudentAssignedCourseExists(studentAssignedCourse.CourseId))
                //{
                //    return NotFound();
                //}
                //else
                //{
                //    throw;
                //}
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/StudentAssignedCourses
        [ResponseType(typeof(StudentAssignedCourse))]
        public IHttpActionResult PostStudentAssignedCourse(StudentAssignedCourse studentAssignedCourse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var compare = new ConvertEmail();
            var studentId = compare.ConvertEmailToId(studentAssignedCourse.StudentId);
          //  var courseId = studentAssignedCourse.CourseId;


           // var isStudentCourseExist = _db.StudentAssignedCourses.Any(x => x.StudentId.Equals(studentId) &&
           //                                                            x.CourseId.Equals(courseId));


            //if (!isStudentCourseExist)
            //{
            //    studentAssignedCourse.StudentId =studentId;
            //    _db.StudentAssignedCourses.Add(studentAssignedCourse);
            //     _db.SaveChangesAsync();

            //    return CreatedAtRoute("DefaultApi", new {id = studentAssignedCourse.StudentAssignedCourseId},
            //        studentAssignedCourse);
            //}
            return Ok("Record Exist");
        }

       

        // DELETE: api/StudentAssignedCourses/5
        [ResponseType(typeof(StudentAssignedCourse))]
        public async Task<IHttpActionResult> DeleteStudentAssignedCourse(int id)
        {
            StudentAssignedCourse studentAssignedCourse = await _db.StudentAssignedCourses.FindAsync(id);
            if (studentAssignedCourse == null)
            {
                return NotFound();
            }

            _db.StudentAssignedCourses.Remove(studentAssignedCourse);
            await _db.SaveChangesAsync();

            return Ok(studentAssignedCourse);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StudentAssignedCourseExists(int id)
        {
            return _db.StudentAssignedCourses.Count(e => e.StudentAssignedCourseId == id) > 0;
        }
    }
}