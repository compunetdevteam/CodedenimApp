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
    public class CourseCategoriesController : ApiController
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: api/CourseCategories
        public IQueryable GetCourseCategories()
        {
                return _db.CourseCategories.Select(x => new
                {
                    x.CategoryName,
                    x.CourseCategoryId,              
                    
                });
        }

        // GET: api/CourseCategories/5
        [ResponseType(typeof(CourseCategory))]
        public async Task<IHttpActionResult> GetCourseCategory(int id)
        {
            var courseCategory = await _db.Courses.Where(x => x.CourseCategoryId.Equals(id))
                                                                .Select(x => new
                                                                {
                                                                    x.CourseId,
                                                                    x.CourseCode,
                                                                    x.CourseDescription,
                                                                    x.FileLocation,
                                                                    x.CourseName,
                                                                    x.CourseCategoryId,
                                                                    x.ExpectedTime,
                                                                    x.CourseCategory.CategoryName

                                                                }).ToListAsync();

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