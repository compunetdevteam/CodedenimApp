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
using CodedenimWebApp.ViewModels;
using CodeninModel;

namespace CodedenimWebApp.Controllers.Api
{
    [System.Web.Http.RoutePrefix("api/CourseCategories")]
    public class CourseCategoriesController : ApiController
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        [HttpGet]
        [System.Web.Http.Route("CourseCategorys")]
        [ResponseType(typeof(CourseCategory))]
        // GET: api/CourseCategories
        //
        // these method will get all the courses based on the student type 
        // either Corper , undergraduate or regular student|
        // the query is performed based on the email parameter coming into the method
        public async Task<IHttpActionResult> CourseCategorys(string email)
        {
             var studentEmail = new ConvertEmail1();
            var studentId = studentEmail.ConvertEmailToId(email);
            var studentType =  _db.Students.AsNoTracking().Where(x => x.StudentId.Equals(studentId)).Select(x => x.AccountType).FirstOrDefault();
            var assignedCourses = await   _db.CourseCategories.AsNoTracking().Where(x => x.StudentType.Equals(studentType))
                                            .Select(x => new { x.CategoryName,
                                                x.Amount,
                                                x.CourseCategoryId,
                                                x.CategoryDescription,
                                                x.StudentType,
                                                x.ImageLocation})
                                            .ToListAsync();
            return Ok(assignedCourses);
            //return _db.CourseCategories.Select(x => new
            //{
            //    x.CategoryName,
            //    x.CourseCategoryId,             

            //});
        }

        //this method gets the courses based on the categoryId that comes in 
        // as a peremeter into the method
        // GET: api/CourseCategories/5
        [HttpGet]
        [ResponseType(typeof(CourseCategory))]
        public async Task<IHttpActionResult> GetCourseCategory(int id)
        {
            var courseCategory = await _db.AssignCourseCategories.Where(x => x.CourseCategoryId.Equals(id))
                                                                .Select(x => new
                                                                {
                                                                  x.CourseId,                                                                   
                                                                  x.Courses.CourseCode,
                                                                  x.Courses.CourseName,
                                                                  x.Courses.CourseDescription,
                                                                  x.Courses.ExpectedTime,
                                                                  x.Courses.Points,
                                                                  x.Courses.FileLocation,
                                                                    
                                                                //    //x.CourseCode,
                                                                //    //x.CourseDescription,
                                                                //    //x.FileLocation,
                                                                    //x.Courses.CourseName,
                                                                    //x.Courses.CourseDescription,
                                                                    //x.Courses.FileLocation,
                                                                //    x.CourseCategoryId,
                                                                ////  //  x.ExpectedTime,
                                                                /// 
                                                                //    x.CourseCategory.CategoryName,

                                                                //    x.CourseCategory.CategoryDescription,
                                                                //    x.CourseCategory.ImageLocation
                                                               
                                                                   // x.CourseCategory.

                                                                })
                                                                .ToListAsync();

            //var courseCategory = await db.CourseCategories.Where(x => x.CourseCategoryId.Equals(id))
            //                            .Select(x => new
            //                            {
            //                                x.Courses
            //                            }).ToListAsync();
            if (courseCategory == null)
            {
                return NotFound();
            }

            return Ok(courseCategory);
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
            //if (studentType == RoleName.Corper)
            //{
            //    var category = _db.CorperEnrolledCourses.Where(x => x.StudentId.Equals(studentId))
            //                         .Select(x => x.CorperEnrolledCoursesId).ToList();
            //    foreach (var categoryId in category)
            //    {
            //         myCourses.CorperCourses = await _db.CorperEnrolledCourses.Where(x => x.CourseCategoryId.Equals(categoryId)).ToListAsync();
            //    }
            //    //myCourses.CorperCourses = _db.CorperEnrolledCourses.Where(x => x.StudentId.Equals(studentId)).ToList();

            //}
            //else
            //{
                var category = _db.StudentPayments.Where(x => x.StudentId.Equals(studentId))
                                .Select(x => x.CourseCategoryId).ToList();

                foreach (var categoryId in category)
                {
                   var couseCategory = await _db.CourseCategories
                        .Where(x => x.CourseCategoryId.Equals(categoryId))
                        .FirstOrDefaultAsync();
                    var vm = new MyCourseCategoryVm
                    {
                        CourseCategoryId = couseCategory.CourseCategoryId,
                        CategoryName = couseCategory.CategoryName,
                        StudentType = couseCategory.StudentType,
                        ImageLocation = couseCategory.ImageLocation,
                        CategoryDescription = couseCategory.CategoryDescription,
                        Amount = couseCategory.Amount
                    };
                    myCourses.Add(vm);
                }
                //myCourses.StudentCourses = await _db.StudentPayments.Where(x => x.StudentId.Equals(studentId)).ToListAsync();
            //}
            return Ok(myCourses);
        }
        // PUT: api/CourseCategories/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCourseCategory(int id, CourseCategory courseCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

  

            if (id != courseCategory.CourseCategoryId)
            {
                return BadRequest();
            }

            _db.Entry(courseCategory).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseCategoryExists(id))
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

        // POST: api/CourseCategories
        [ResponseType(typeof(CourseCategory))]
        public async Task<IHttpActionResult> PostCourseCategory(CourseCategory courseCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.CourseCategories.Add(courseCategory);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = courseCategory.CourseCategoryId }, courseCategory);
        }

        // DELETE: api/CourseCategories/5
        [ResponseType(typeof(CourseCategory))]
        public async Task<IHttpActionResult> DeleteCourseCategory(int id)
        {
            CourseCategory courseCategory = await _db.CourseCategories.FindAsync(id);
            if (courseCategory == null)
            {
                return NotFound();
            }

            _db.CourseCategories.Remove(courseCategory);
            await _db.SaveChangesAsync();

            return Ok(courseCategory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CourseCategoryExists(int id)
        {
            return _db.CourseCategories.Count(e => e.CourseCategoryId == id) > 0;
        }
    }
}