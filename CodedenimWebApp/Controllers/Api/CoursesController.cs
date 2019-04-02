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
using System.Web.Http.Results;
using CodedenimWebApp.Constants;
using CodedenimWebApp.Controllers.Api.ApiViewModel;
using CodedenimWebApp.Models;
using CodeninModel;
using Microsoft.Ajax.Utilities;

namespace CodedenimWebApp.Controllers.Api
{
    //[Authorize]
    [System.Web.Http.RoutePrefix("api/Courses")]
    public class CoursesController : ApiController
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: api/Courses
        //Get all the Courses to the client
        [AllowAnonymous]
        [Route(nameof(GetCourses))]
        [HttpGet]
        public IHttpActionResult GetCourses()
        {
           // var courseIds = _db.AssignCourseCategories.Select(x => x.CourseId).ToList();
           
            var getCourses = _db.AssignCourseCategories.Include(x => x.CourseCategory)
                .Include(x => x.Courses.Modules).DistinctBy(s => s.CourseId)
                .Select(x => new CoursesVm
                {
                    CourseId = x.CourseId,
                    CourseCategoryId = x.CourseCategoryId,  
                    CourseCode = x.Courses.CourseCode,
                    CourseDescription = x.Courses.CourseDescription,
                    CategoryName = x.CourseCategory.CategoryName,
                    CourseName = x.Courses.CourseName,
                    ExpectedTime = x.Courses.ExpectedTime,
                    FileLocation =  x.Courses.FileLocation,
                    VideoLocation =  x.Courses.VideoLocation,
                    Modules = x.Courses.Modules.Select(c => new ModuleVm
                        {
                        ModuleDescription = c.ModuleDescription,
                        ModuleName = c.ModuleName,
                         ModuleId =c.ModuleId,
                         CourseId = c.CourseId,
                         ExpectedTime = c.ExpectedTime
                    })
                   
                }).ToList();
            
            if (getCourses != null)
            {
                return Ok(getCourses);
            }
            
            return BadRequest("No Courses to display");
        }


        


        // GET: api/Courses/5
        /// <summary>
        /// this method receives the Course id and 
        /// fetches all the Modules associated with that course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Course))]
        [Route("GetCourse/{id}")]
        public async Task<IHttpActionResult> GetCourse(int id)
        {
            // Course course = await _db.Courses.FindAsync(id);
            var modules = _db.Modules.Where(x => x.CourseId.Equals(id)).ToList();
            var course = await _db.Courses.Include(x => x.Modules).Where(x => x.CourseId.Equals(id))
                                        .Select(x => new CoursesVm
                                        {
                                            CourseId = x.CourseId,
                                            CourseCode = x.CourseCode,
                                            CourseDescription = x.CourseDescription,
                                            FileLocation =  x.FileLocation,
                                            CategoryName = x.CourseName,
                                            VideoLocation =  x.VideoLocation,
                                            Modules = x.Modules.Select(c => new ModuleVm
                                            {
                                                ModuleDescription = c.ModuleDescription,
                                                ModuleName = c.ModuleName,
                                                ModuleId = c.ModuleId,
                                                CourseId = c.CourseId,
                                                ExpectedTime = c.ExpectedTime
                                            })
                                        }).ToListAsync();

            //var course = await _db.Modules.Where(c => c.CourseId.Equals(id))
            //                                     .Select(x => new CoursesVm
            //                                     {
            //                                         CourseId = x.CourseId,
            //                                         CourseCode = x.Course.CourseCode,
            //                                         CategoryName = x.Course.CourseName,
            //                                          CourseDescription = x.Course.CourseDescription,
            //                                         ExpectedTime = x.Course.ExpectedTime,
            //                                         //x.Course.CourseCategory.CategoryName,
            //                                         //x.Course.CourseCategoryId,
            //                                         FileLocation = x.Course.FileLocation,



            //                                     }).ToListAsync();

            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        // PUT: api/Courses/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCourse(int id, Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != course.CourseId)
            {
                return BadRequest();
            }

            _db.Entry(course).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
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

        // POST: api/Courses
        [ResponseType(typeof(Course))]
        public async Task<IHttpActionResult> PostCourse(Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Courses.Add(course);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = course.CourseId }, course);
        }

        // DELETE: api/Courses/5
        [ResponseType(typeof(Course))]
        public async Task<IHttpActionResult> DeleteCourse(int id)
        {
            Course course = await _db.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _db.Courses.Remove(course);
            await _db.SaveChangesAsync();

            return Ok(course);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CourseExists(int id)
        {
            return _db.Courses.Count(e => e.CourseId == id) > 0;
        }
    }
}