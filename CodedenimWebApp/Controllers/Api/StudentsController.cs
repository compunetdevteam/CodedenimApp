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
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: api/Students
        public IEnumerable<Student> GetStudents()
        {
            var studentid = RequestContext.Principal;
            return _db.Students.Where(x => x.StudentId.Equals(studentid));
            //  return db.Students.ToList();
        }



        // GET: api/Students/5
        [ResponseType(typeof(Student))]
        public async Task<IHttpActionResult> GetStudent(string id)
        {
           // Student student = await db.Students.FindAsync(id);
         //  var email = new ConvertEmail();
           
            //var studentCourses = _db.StudentAssignedCourses.Include(x => x.Courses).Where(x => x.Student.Email.Equals(id))
            //                                                                     .Select(x =>new {
            //                                                                        x.CourseId,
            //                                                                        x.Courses.CourseCode,
            //                                                                        x.Courses.CourseName,
            //                                                                         x.Courses.CourseDescription,
            //                                                                         x.Courses.ExpectedTime,
            //                                                                         x.Courses.CourseCategory.CategoryName,
            //                                                                         x.Courses.CourseCategoryId,
            //                                                                         x.Courses.FileLocation
                                                                                 
                                                                                                
            //                                                                     }).ToList();
            ////var studentCourses = db.StudentAssignedCourses.Where(x => x.StudentId.Equals(student.StudentId))
            ////                                              .Select(s => s.Courses);
            ////if (student == null)
            ////{
            ////    return NotFound();
            ////}

            return Ok(/*studentCourses*/);
        }

        /// <summary>
        /// method to check if a student exit
        /// then the android buttton should no show enrolled but Continue Course
        /// </summary>
        /// <param name="email"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(Student))]
        [System.Web.Http.Route("CheckIfStudentIsEnrolled")]
        public IHttpActionResult CheckIfStudentIsEnrolled(string email, int courseId)
        {
          //  bool Enrolled;
          // var converter = new ConvertEmail();
          //  var studentId = converter.ConvertEmailToId(email);
          ////  var isEnrolled = _db.StudentAssignedCourses.Any(x => x.StudentId.Equals(studentId) && x.CourseId.Equals(courseId));

            //if(!isEnrolled)
            //{
            //    Enrolled = true;
            //}
            //else
            //{
            //    Enrolled = false;
            //}
            return Ok();
        }




        //GET: api/Student
        //[ResponseType(typeof(Student))]
        [System.Web.Http.Route("TopicMaterial")]
        public IHttpActionResult GetTopicMaterial()
        {
            var location = _db.TopicMaterialUploads.FirstOrDefault(x => x.TopicMaterialUploadId.Equals(2));
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

            _db.Entry(student).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
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

            _db.Students.Add(student);

            try
            {
                await _db.SaveChangesAsync();
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
            Student student = await _db.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _db.Students.Remove(student);
            await _db.SaveChangesAsync();

            return Ok(student);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StudentExists(string id)
        {
            return _db.Students.Count(e => e.StudentId == id) > 0;
        }
    }
}