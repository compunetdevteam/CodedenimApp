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
using CodedenimWebApp.Controllers.Api;
using CodedenimWebApp.Models;
using CodeninModel;

namespace CodedenimWebApp.Controllers
{
    public class AssignCourseCategories1Controller : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/AssignCourseCategories1
        public IEnumerable<AssignCourseCategory> GetAssignCourseCategories(string email)
        {
            var studentEmail = new ConvertEmail();
            var studentId = studentEmail.ConvertEmailToId(email);
            var studentType = db.Students.AsNoTracking().Where(x => x.Id.Equals(studentId)).Select(x => x.AccountType).FirstOrDefault();
            var assignedCourses = db.CourseCategories.AsNoTracking().Where(x => x.StudentType.Equals(studentType))
                .Select(x => new[] {x.CategoryName});
            return db.AssignCourseCategories;
        }

        // GET: api/AssignCourseCategories1/5
        [ResponseType(typeof(AssignCourseCategory))]
        public async Task<IHttpActionResult> GetAssignCourseCategory(int id)
        {
            AssignCourseCategory assignCourseCategory = await db.AssignCourseCategories.FindAsync(id);
            if (assignCourseCategory == null)
            {
                return NotFound();
            }

            return Ok(assignCourseCategory);
        }

        // PUT: api/AssignCourseCategories1/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAssignCourseCategory(int id, AssignCourseCategory assignCourseCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != assignCourseCategory.Id)
            {
                return BadRequest();
            }

            db.Entry(assignCourseCategory).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssignCourseCategoryExists(id))
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

        // POST: api/AssignCourseCategories1
        [ResponseType(typeof(AssignCourseCategory))]
        public async Task<IHttpActionResult> PostAssignCourseCategory(AssignCourseCategory assignCourseCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AssignCourseCategories.Add(assignCourseCategory);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = assignCourseCategory.Id }, assignCourseCategory);
        }

        // DELETE: api/AssignCourseCategories1/5
        [ResponseType(typeof(AssignCourseCategory))]
        public async Task<IHttpActionResult> DeleteAssignCourseCategory(int id)
        {
            AssignCourseCategory assignCourseCategory = await db.AssignCourseCategories.FindAsync(id);
            if (assignCourseCategory == null)
            {
                return NotFound();
            }

            db.AssignCourseCategories.Remove(assignCourseCategory);
            await db.SaveChangesAsync();

            return Ok(assignCourseCategory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AssignCourseCategoryExists(int id)
        {
            return db.AssignCourseCategories.Count(e => e.Id == id) > 0;
        }
    }
}