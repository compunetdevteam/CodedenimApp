using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using CodedenimWebApp.Models;
using CodeninModel;
using PagedList;

namespace CodedenimWebApp.Controllers.Api
{
    [System.Web.Http.RoutePrefix("api/Students")]
    public class StudentsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Students
        public IEnumerable<Student> GetStudents()
        {
            return db.Students.ToList();
        }



        // GET: api/Students/5
        [ResponseType(typeof(Student))]
        public async Task<IHttpActionResult> GetStudent(string id)
        {
           // Student student = await db.Students.FindAsync(id);
           var email = new Convert();
           
            var studentCourses = db.StudentAssignedCourses.Include(x => x.Courses).Where(x => x.Student.Email.Equals(id))
                                                                                 .Select(x =>new { x.Courses.CourseName,
                                                                                 x.Courses.CourseDescription, x.Courses.CourseCode,
                                                                                 x.Courses.ExpectedTime,
                                                                                 x.CourseId
                                                                                                
                                                                                 }).ToList();
            //var studentCourses = db.StudentAssignedCourses.Where(x => x.StudentId.Equals(student.StudentId))
            //                                              .Select(s => s.Courses);
            //if (student == null)
            //{
            //    return NotFound();
            //}

            return Ok(studentCourses);
        }

        //GET: api/Student
        //[ResponseType(typeof(Student))]
        [System.Web.Http.Route("TopicMaterial")]
        public IHttpActionResult GetTopicMaterial()
        {
            var location = db.TopicMaterialUploads.FirstOrDefault(x => x.TopicMaterialUploadId.Equals(2));
          //  var topicDetail = HttpContext.Current.Server.MapPath("~/MaterialUpload/Data Management_ What Is Master Data Management (Mdm).mp4");
            return Ok(location);
        }

        // PUT: api/Students/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStudent(string id, Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != student.StudentId)
            {
                return BadRequest();
            }

            db.Entry(student).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
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

        // POST: api/Students
        [ResponseType(typeof(Student))]
        public async Task<IHttpActionResult> PostStudent(Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Students.Add(student);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StudentExists(student.StudentId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = student.StudentId }, student);
        }

        // DELETE: api/Students/5
        [ResponseType(typeof(Student))]
        public async Task<IHttpActionResult> DeleteStudent(string id)
        {
            Student student = await db.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            db.Students.Remove(student);
            await db.SaveChangesAsync();

            return Ok(student);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StudentExists(string id)
        {
            return db.Students.Count(e => e.StudentId == id) > 0;
        }
    }
}