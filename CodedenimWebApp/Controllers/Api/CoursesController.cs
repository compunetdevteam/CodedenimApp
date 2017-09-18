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
    public class CoursesController : ApiController
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: api/Courses
        //Get all the Courses to the client
        public IQueryable GetCourses()
        {
            return _db.Courses.Select(x => new
                                    {
                                        x.CourseId,
                                        x.CourseCode,
                                        x.CourseDescription,
                                        x.CourseCategory.CategoryName,
                                        x.CourseName,
                                        x.ExpectedTime,
                                        x.CourseCategoryId,
                                        x.FileLocation
                                       
                                       
                                    });
            //return db.Courses.Include(c => c.Modules).ToList();
        }

        // GET: api/Courses/5
        /// <summary>
        /// this method receives the Course id and 
        /// fetches all the Modules associated with that course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Course))]
        public async Task<IHttpActionResult> GetCourse(int id)
        {
            // Course course = await _db.Courses.FindAsync(id);
            //var course = await _db.Courses.Where(x => x.CourseCategoryId.Equals(id))
            //                            .Select(x => new
            //                            {
            //                                    x.CourseId,
            //                                    x.CourseCode,
            //                                    x.CourseDescription,
            //                                    x.FileLocation,
            //                                    x.CourseName,
            //                                    x.CourseCategoryId,

            //                            }).ToListAsync();

           var course = await  _db.Modules.Where(c => c.CourseId.Equals(id))
                                                .Select(x => new
                                                {
                                                    x.CourseId,
                                                    x.Course.CourseCode,
                                                    x.Course.CourseName,
                                                    x.Course.CourseDescription,
                                                    x.Course.ExpectedTime,
                                                    x.Course.CourseCategory.CategoryName,
                                                    x.Course.CourseCategoryId,
                                                    x.Course.FileLocation

                                                    //m.ModuleId,
                                                    //m.ModuleName,
                                                    //m.ModuleDescription,
                                                    //m.ExpectedTime,
                                                    //m.CourseId,
                                                    //m.Course.CourseName
                                                }).ToListAsync();
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