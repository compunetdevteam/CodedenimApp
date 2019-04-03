using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Description;
using CodedenimWebApp.Controllers.Api.ApiViewModel;
using CodedenimWebApp.Models;
using CodeninModel;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;

namespace CodedenimWebApp.Controllers.Api
{
    [System.Web.Http.RoutePrefix("api/EnrollForCourses")]
    public class EnrollForCoursesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ResponseMessage response = new ResponseMessage();

        // GET: api/EnrollForCourses
        //public IQueryable<EnrollForCourse> GetEnrollForCourses()
        //{
        //    return db.EnrollForCourses;
        //}
        [HttpGet]
        [Route("CheckPayment")]
        public async Task<IHttpActionResult> CheckPayment(string email)
        {
            var studentEmail = new ConvertEmail1();
            var studentId = studentEmail.ConvertEmailToId(email);
            var enrolledCategory = false;
            var courses = new Course();
            if (studentId != null)
            {
                //get the list of all enrolled categories
                enrolledCategory = db.StudentPayments.AsNoTracking().Where(x => x.StudentId.Equals(studentId) && x.IsPayed.Equals(true)).Any();
                if (enrolledCategory)
                {
                    response.Message = "User has paid for category";
                    response.Status = true;
                }
                else
                {
                    response.Message = "User has not paid";
                    response.Status = false;
                }
                return Ok(response);
            }

            return Ok(response);
        }



        // GET: api/EnrollForCourses
        //public IQueryable<EnrollForCourse> GetEnrollForCourses()
        //{
        //    return db.EnrollForCourses;
        //}
        [HttpGet]
        [Route("Checker")]
        public async Task<IHttpActionResult> Checker(string email, int categoryId)
        {
            var studentEmail = new ConvertEmail1();
            var studentId = studentEmail.ConvertEmailToId(email);
            var enrolledCategory = false;
            var courses = new Course();
            if(studentId != null)
            {
                //get the list of all enrolled categories
                enrolledCategory = db.StudentPayments.AsNoTracking().Where(x => x.StudentId.Equals(studentId) && x.IsPayed.Equals(true) && x.CourseCategoryId.Equals(categoryId)).Any();
                if (enrolledCategory)
                {
                    response.Message = "User has paid for category";
                    response.Status = true;
                }
                else
                {
                    response.Message = "User has not paid";
                    response.Status = false;
                }
                return Ok(response);
            }
           
            return Ok(response);
        }

        // GET: api/EnrollForCourses/5
        //this method gets the list of courses that the student has enrolled into(payed for)
        [HttpGet]
        [System.Web.Http.Route("GetEnrolledCategories")]
        [ResponseType(typeof(EnrollForCourse))]
        public async Task<IHttpActionResult> GetEnrolledCategories(string email)
        {
            var studentEmail = new ConvertEmail1();
            var studentId = studentEmail.ConvertEmailToId(email);
            var enrolledCategory = new List<StudentPayment>();
            var courses = new Course();

            var category = new List<MyCoursesVm>();
            if (studentId != null)
            {
                //get the list of all enrolled categories
                 enrolledCategory = db.StudentPayments.AsNoTracking().Where(x => x.StudentId.Equals(studentId) && x.IsPayed.Equals(true))
                                    .ToList();
               
                foreach (var item in enrolledCategory.DistinctBy(x => x.CourseCategoryId))
                {
                  
                    //getting the different courses that was paid for based on the category
                    //TODO: fix the issue : same course return for all different courses
                    var paidCourses = db.AssignCourseCategories.Include(x => x.Courses).Where(x => x.CourseCategoryId.Equals(item.CourseCategoryId))
                                                                        .Select(x => new MyCoursesVm
                                                                        {
                                                                            StudentId = studentId,
                                                                            CourseCategoryId = x.CourseCategoryId,
                                                                            CategoryName = x.CourseCategory.CategoryName,
                                                                        // Courses = x.Courses.Modules.Select()
                                                                    }).FirstOrDefault();

                    category.Add(paidCourses);
                }

            }





            //var PaidCourses = db.EnrollForCourses.AsNoTracking().Where(x => x.StudentId.Equals(studentId))
            //    .Select(x => x.CourseCategoryId).FirstOrDefault();

            //var courses = await db.AssignCourseCategories.AsNoTracking()
            //    .Where(x => x.CourseCategoryId.Equals(enrollCategoryId)).ToListAsync();
            //EnrollForCourse enrollForCourse = await db.EnrollForCourses.FindAsync(email);
            //if (enrollForCourse == null)
            //{
            //    return NotFound();
            //}
            response.Message = "Student has not paid";
            response.Status = false;
            //return Ok(enrollForCourse);
            return Ok(response);
        }

        // GET: api/EnrollForCourses/5
        /// <summary>
        /// return the courses that the use has started
        /// </summary>
        /// <param name="id", name="email">CourseId and Email</param>
        /// <returns>List<Coureses></returns>
        [ResponseType(typeof(EnrollForCourse))]
        public async Task<IHttpActionResult> GetEnrolledCourse(int id, string email)
        {
            var studentEmail = new ConvertEmail1();
            var studentId = studentEmail.ConvertEmailToId(email);
            EnrollForCourse enrollForCourse = await db.EnrollForCourses
                                                      .Where(x => x.StudentId.Equals(studentId))
                                                      .FirstOrDefaultAsync();
            if (enrollForCourse == null)
            {
                var enroll = new EnrollForCourse();
                enroll.StudentId = studentId;
                enroll.DateStarted = DateTime.Now;
                enroll.CourseId = id;
                db.EnrollForCourses.Add(enroll);
                db.SaveChanges();

                response.Message = $"you just enrolled for a course";
                response.Status = false;
                return Ok(response);
            }


            return Ok(enrollForCourse);
        }

        //// PUT: api/EnrollForCourses/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutEnrollForCourse(int id, EnrollForCourse enrollForCourse)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != enrollForCourse.EnrollForCourseId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(enrollForCourse).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!EnrollForCourseExists(id))
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

        //[HttpPost]
        //// POST: api/EnrollForCourses
        //[ResponseType(typeof(EnrollForCourse))]
        //public async Task<IHttpActionResult> PostEnrollForCourse(EnrollForCourse enrollForCourse)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var studentEmail = new ConvertEmail();
        //    var studentId = studentEmail.ConvertEmailToId(enrollForCourse.StudentId);
        //    enrollForCourse.StudentId = studentId;
        //    db.EnrollForCourses.Add(enrollForCourse);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = enrollForCourse.EnrollForCourseId }, enrollForCourse);
        //}

        //// DELETE: api/EnrollForCourses/5
        //[ResponseType(typeof(EnrollForCourse))]
        //public async Task<IHttpActionResult> DeleteEnrollForCourse(int id)
        //{
        //    EnrollForCourse enrollForCourse = await db.EnrollForCourses.FindAsync(id);
        //    if (enrollForCourse == null)
        //    {
        //        return NotFound();
        //    }

        //    db.EnrollForCourses.Remove(enrollForCourse);
        //    await db.SaveChangesAsync();

        //    return Ok(enrollForCourse);
        //}

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
            return db.EnrollForCourses.Count(e => e.EnrollForCourseId == id) > 0;
        }
    }
}