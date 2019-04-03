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
using CodedenimWebApp.Constants;
using CodedenimWebApp.Controllers.Api.ApiViewModel;
using CodedenimWebApp.Models;
using CodedenimWebApp.ViewModels;
using CodeninModel;
using Microsoft.Ajax.Utilities;
using PagedList;

namespace CodedenimWebApp.Controllers.Api
{
    [System.Web.Http.RoutePrefix("api/Students")]
    public class StudentsController : ApiController
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private readonly ResponseMessage _response = new ResponseMessage();

        // GET: api/Students
        public IEnumerable<Student> GetStudents()
        {
            var studentid = RequestContext.Principal;
            return _db.Students.Where(x => x.StudentId.Equals(studentid));
            //  return db.Students.ToList();
        }



        // GET: api/Students/5
        //[ResponseType(typeof(Student))]
        [HttpGet]
        [Route("GetStudent")]
        public async Task<IHttpActionResult> GetStudent(string email)
        {
            //TODO
            Student student =  _db.Students.FirstOrDefault(x => x.Email.ToUpper().Equals(email.ToUpper()));
            var studentDetaills = new ApiViewModel.StudentVm();
            if (student != null)
            {
                var studentEmail = new ConvertEmail1();
                var studentId = studentEmail.ConvertEmailToId(email);
                var model = await _db.Students.Where(x => x.StudentId.Equals(studentId)).ToListAsync();
                studentDetaills = model.Select(x => new ApiViewModel.StudentVm
                {
                    AccountType = x.AccountType,
                    Title = x.Title,
                    Name = x.FullName,
                    Discpline = x.Discpline,
                    Institution = x.Institution,
                    MatricNo = x.MatricNo,
                    Email = x.Email,
                    Gender = x.Gender,
                    PhoneNumber = x.PhoneNumber,
                    Picture = Constant.FilePath + x.FileLocation
                }).FirstOrDefault();



              return  Ok(studentDetaills);
            }
          
            //var studentCourses = db.StudentAssignedCourses.Where(x => x.StudentId.Equals(student.StudentId))
            //                                              .Select(s => s.Courses);
            //if (student == null)
            //{
            //    return NotFound();
            //}

            return Ok(studentDetaills);
        }


        /// <summary>
        /// method to check the student account type and display mycourse
        /// </summary>
        /// <param name="email"></param>
        /// <param name="courseCategory"></param>
        /// <returns>enrolled courses</returns>
        [HttpGet]
        [System.Web.Http.Route("MyCourses")]
        [ResponseType(typeof(CourseCategory))]
        public async Task<IHttpActionResult> MyCourses(string email)
        {
            var student = new ConvertEmail();
            var studentEmail = student.ConvertEmailToId(email);
            var studentId = studentEmail;
            var studentType = _db.Students.Where(x => x.Email.Equals(email)).Select(x => x.AccountType).FirstOrDefault();


            var myCourses = new List<MyCourseCategoryVm>();
            var category = _db.StudentPayments.DistinctBy(x => x.CourseCategoryId).Where(x => x.StudentId.Equals(studentId) && x.IsPayed.Equals(true))
                            .Select(x => x.CourseCategoryId).ToList();

            if (category != null)
            {
                foreach (var categoryId in category)
                {
                    var couseCategory = await _db.AssignCourseCategories
                                        .Where(x => x.CourseCategoryId.Equals(categoryId))
                         .FirstOrDefaultAsync();
                    var vm = new MyCourseCategoryVm
                    {
                        CourseCategoryId = couseCategory.CourseCategoryId,
                        CategoryName = couseCategory.CourseCategory.CategoryName,
                        StudentType = couseCategory.CourseCategory.StudentType,
                        ImageLocation = couseCategory.CourseCategory.ImageLocation,
                        CategoryDescription = couseCategory.CourseCategory.CategoryDescription,
                        Amount = couseCategory.CourseCategory.Amount,
                        Courses = _db.AssignCourseCategories.Include(x => x.Courses)
                                        .Where(x => x.CourseCategoryId.Equals(categoryId))
                                    .Select(x => new CoursesVm
                                    {
                                        CourseId = x.CourseId,
                                        CourseName = x.Courses.CourseName,
                                        CourseDescription = x.Courses.CourseDescription,
                                        FileLocation = x.Courses.FileLocation,

                                    }).ToList()

                    };
                    myCourses.Add(vm);
                }
                return Ok(myCourses);
            }

            //myCourses.StudentCourses = await _db.StudentPayments.Where(x => x.StudentId.Equals(studentId)).ToListAsync();
            //}
            else
            {
                _response.Message = "No courses to display";
                _response.Status = false;
                return Ok(_response);
            }
        }

        /// <summary>
        /// method to check if a student exit
        /// then the android buttton should no show enrolled but Continue Course
        /// </summary>
        /// <param name="email"></param>
        /// <param name="courseId"></param>
        [HttpGet]
        [ResponseType(typeof(Student))]
        [System.Web.Http.Route("CheckIfStudentIsEnrolled")]
        public async Task<IHttpActionResult> CheckIfStudentIsEnrolled(string email, int courseId)
        {
            //var student = new ConvertEmail();
            //var studentId = student.ConvertEmailToId(email);
            //var type = new Converter();
            var student = new ConvertEmail();
            var studentEmail = student.ConvertEmailToId(email);
            var studentId = studentEmail;
            var studentType = _db.Students.Where(x => x.Email.Equals(email)).Select(x => x.AccountType).FirstOrDefault();
           // var studentType = type.UserType(email);

            //execute this block of code if the user is a corper
            if (studentType == RoleName.Corper)
            {


                var corperEnrollment = _db.CorperEnrolledCourses.Any(x => x.StudentId.Equals(studentId) &&
                                                       x.Category.CourseCategoryId.Equals(courseId));
                var enrollStudent = new CorperEnrolledCourses();
                //if corper has not enrolled the enroll the corper using
                //the if statement block
                if (corperEnrollment != true)
                {
                    enrollStudent.StudentId = studentId;
                    enrollStudent.CourseCategoryId = courseId;
                    _db.CorperEnrolledCourses.Add(enrollStudent);
                    _db.SaveChanges();
                    return Ok("Enrollment was successful");
                }
                return Ok("No need to Enroll because you have enrolled");
            }
  
                var hasStudentPayed = _db.StudentPayments.Any(x => x.StudentId.Equals(studentId) && x.IsPayed.Equals(true) && x.CourseCategoryId.Equals(courseId));
                if (hasStudentPayed != true)
                {

                    return Ok("Redirect User to Pay on the Web");
                }
                return Ok("student Has Enrolled for this course");
        

     
        }
        /// <summary>
        /// method that checks if the student has paid
        /// this method takes in the student id and the categoryId and check for the payment 
        /// on the student payment table and check if the status is true
        /// </summary>
        /// <param name="email"></param>
        /// <param name="learningPathId"></param>
        /// <returns></returns>
        //GET: api/Student
        //[ResponseType(typeof(Student))]
        [HttpGet]
        [ResponseType(typeof(Student))]
        [System.Web.Http.Route("PaymentStatus")]
        public async Task<IHttpActionResult> GetPaymentStatus(string email, int courseCategoryId)
        {
            var studentEmail = new ConvertEmail1();
            var studentId = studentEmail.ConvertEmailToId(email);
            var payment = _db.StudentPayments.Where(x => x.StudentId.Equals(studentId) && x.CourseCategoryId.Equals(courseCategoryId) && x.IsPayed.Equals(true)).FirstOrDefault();
            if (payment != null)
            {
                return Ok("Has Paid");
            }
            return Unauthorized();
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

        //// PUT: api/Students/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutStudent(string id, Student student)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != student.StudentId)
        //    {
        //        return BadRequest();
        //    }

        //    _db.Entry(student).State = EntityState.Modified;

        //    try
        //    {
        //        await _db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!StudentExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/Students
        //[ResponseType(typeof(Student))]
        //public async Task<IHttpActionResult> PostStudent(Student student)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    _db.Students.Add(student);

        //    try
        //    {
        //        await _db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (StudentExists(student.StudentId))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtRoute("DefaultApi", new { id = student.StudentId }, student);
        //}

        //// DELETE: api/Students/5
        //[ResponseType(typeof(Student))]
        //public async Task<IHttpActionResult> DeleteStudent(string id)
        //{
        //    Student student = await _db.Students.FindAsync(id);
        //    if (student == null)
        //    {
        //        return NotFound();
        //    }

        //    _db.Students.Remove(student);
        //    await _db.SaveChangesAsync();

        //    return Ok(student);
        //}

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