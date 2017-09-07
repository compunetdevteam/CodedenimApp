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
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/CourseCategories
        public IEnumerable<CourseCategory> GetCourseCategories()
        {
            return db.CourseCategories.Include(c => c.Courses).ToList();
        }

        // GET: api/CourseCategories/5
        [ResponseType(typeof(CourseCategory))]
        public async Task<IHttpActionResult> GetCourseCategory(int id)
        {
            CourseCategory courseCategory = await db.CourseCategories.FindAsync(id);
            
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

            db.Entry(courseCategory).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
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

            db.CourseCategories.Add(courseCategory);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = courseCategory.CourseCategoryId }, courseCategory);
        }

        // DELETE: api/CourseCategories/5
        [ResponseType(typeof(CourseCategory))]
        public async Task<IHttpActionResult> DeleteCourseCategory(int id)
        {
            CourseCategory courseCategory = await db.CourseCategories.FindAsync(id);
            if (courseCategory == null)
            {
                return NotFound();
            }

            db.CourseCategories.Remove(courseCategory);
            await db.SaveChangesAsync();

            return Ok(courseCategory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CourseCategoryExists(int id)
        {
            return db.CourseCategories.Count(e => e.CourseCategoryId == id) > 0;
        }
    }
}