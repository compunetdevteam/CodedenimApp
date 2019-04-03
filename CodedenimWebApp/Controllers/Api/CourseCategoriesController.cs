using CodedenimWebApp.Constants;
using CodedenimWebApp.Controllers.Api.ApiViewModel;
using CodedenimWebApp.Models;
using CodedenimWebApp.ViewModels;
using CodeninModel;
using Microsoft.Ajax.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace CodedenimWebApp.Controllers.Api
{
    [System.Web.Http.RoutePrefix("api/CourseCategories")]
    public class CourseCategoriesController : ApiController
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private ConvertEmail1 email_Id = new ConvertEmail1();
        private ResponseMessage _response = new ResponseMessage();
        [HttpGet]
        [System.Web.Http.Route("CourseCategorys")]
        [ResponseType(typeof(CourseCategory))]
        // GET: api/CourseCategories
        //
        // these method will get all the courses based on the student type x`
        // either Corper , undergraduate or regular student|
        // the query is performed based on the email parameter coming into the method
        public async Task<IHttpActionResult> CourseCategorys(string email)
        {
            // var studentEmail = new ConvertEmail();
            var studentId = _db.Users.AsNoTracking().Where(x => x.Email.Equals(email))
                                            .Select(x => x.Id).FirstOrDefault();
            //var studentId = studentEmail.ConvertEmailToId(email);
            var studentType =  _db.Students.AsNoTracking().Where(x => x.StudentId.Equals(studentId)).Select(x => x.AccountType).FirstOrDefault();
            var assignedCourses = await  _db.CourseCategories.AsNoTracking().Where(x => x.StudentType.Equals(studentType))
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

        [HttpGet]
        [Route("GetACategories")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetCategories()
        {
            var categories =await  _db.CourseCategories.AsNoTracking()
                                                 .Select(x =>
                                                            new CourseCategoryVm {
                                                                Amount = x.Amount,
                                                                CategoryDescription = x.CategoryDescription,
                                                                CategoryName = x.CategoryName,
                                                                CourseCategoryId = x.CourseCategoryId,
                                                                ImageLocation  = x.ImageLocation,
                                                                StudentType = x.StudentType
                                                            })
                                                 .ToListAsync();
            if(categories != null)
            {
                return Ok(categories);
            }
            return BadRequest();
        }

        //this method gets the courses based on the categoryId that comes in 
        // as a peremeter into the method
        // GET: api/CourseCategories/5
        [HttpGet]
        [ResponseType(typeof(CourseCategory))]
        public async Task<IHttpActionResult> GetCourseCategory(int id)
        {
            var courseCategory = await _db.AssignCourseCategories.Where(x => x.CourseCategoryId.Equals(id))
                                                                .Select(x => new CourseCategoryVm
                                                                {
                                                                   CourseId = x.CourseId,                                                                   
                                                                  CourseCode = x.Courses.CourseCode,
                                                                  CategoryName = x.Courses.CourseName,
                                                                  CategoryDescription = x.Courses.CourseDescription,
                                                                  ExpectedTime = x.Courses.ExpectedTime,
                                                                  Point = x.Courses.Points,
                                                                  ImageLocation =  x.Courses.FileLocation,
                                                                    
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



        ///this api method checks the payment status of a user
        ///strings in the method should not be changed because it will break the mobile end point
        ///and make it not to work Pls NOTE !!!!
        [HttpPost]
        [System.Web.Http.Route("CheckPayment")]
        [ResponseType(typeof(CourseCategory))]
        public async Task<IHttpActionResult> CheckPayment(CheckPaymentVm payment)
        {
            if (ModelState.IsValid)
            {
                var responseStatus = new CheckPaymentStatus();
                var studentId = email_Id.ConvertEmailToId(payment.Email);
                var status = await _db.StudentPayments.Where(x => x.StudentId.Equals(studentId) 
                                                        && x.CourseCategoryId.Equals(payment.CategoryId) 
                                                        && x.IsPayed.Equals(true)).AnyAsync();
                if(status == true)
                {
                    responseStatus.status = "Paid"; 
                    //if any string in this method changes the mobile endpoint will break and not work
                    return Ok(responseStatus);
                }
                else
                {
                    //code to check if the transaction of a student is pending
                    var status2 = await _db.StudentPayments.Where(x => x.StudentId.Equals(studentId)
                                                           && x.CourseCategoryId.Equals(payment.CategoryId)
                                                           && x.IsPayed.Equals(false) 
                                                           && x.PaymentStatus.Equals("Transaction Pending"))
                                                           .AnyAsync();
                    if (status2 == true)
                    {
                        responseStatus.status = "Pending";
                        return Ok(responseStatus);
                    }
                    responseStatus.status = "Not Paid";
                    return Ok(responseStatus);
                }
            }
        
            return Json("SomeThing went wrong with the request");
        }


        //// PUT: api/CourseCategories/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutCourseCategory(int id, CourseCategory courseCategory)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

  

        //    if (id != courseCategory.CourseCategoryId)
        //    {
        //        return BadRequest();
        //    }

        //    _db.Entry(courseCategory).State = EntityState.Modified;

        //    try
        //    {
        //        await _db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CourseCategoryExists(id))
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

        //// POST: api/CourseCategories
        //[ResponseType(typeof(CourseCategory))]
        //public async Task<IHttpActionResult> PostCourseCategory(CourseCategory courseCategory)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    _db.CourseCategories.Add(courseCategory);
        //    await _db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = courseCategory.CourseCategoryId }, courseCategory);
        //}

        //// DELETE: api/CourseCategories/5
        //[ResponseType(typeof(CourseCategory))]
        //public async Task<IHttpActionResult> DeleteCourseCategory(int id)
        //{
        //    CourseCategory courseCategory = await _db.CourseCategories.FindAsync(id);
        //    if (courseCategory == null)
        //    {
        //        return NotFound();
        //    }

        //    _db.CourseCategories.Remove(courseCategory);
        //    await _db.SaveChangesAsync();

        //    return Ok(courseCategory);
        //}

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