﻿using System;
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
    public class StudentAssignedCoursesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/StudentAssignedCourses
        public IEnumerable<StudentAssignedCourse> GetStudentAssignedCourses()
        {
            return db.StudentAssignedCourses.Include(t => t.Courses).ToList();
        }
        
        // GET: api/StudentAssignedCourses/5
        [ResponseType(typeof(StudentAssignedCourse))]
        public async Task<IHttpActionResult> GetStudentAssignedCourse(int id)
        {
            StudentAssignedCourse studentAssignedCourse = await db.StudentAssignedCourses.FindAsync(id);
            if (studentAssignedCourse == null)
            {
                return NotFound();
            }

            return Ok(studentAssignedCourse);
        }

        // PUT: api/StudentAssignedCourses/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStudentAssignedCourse(int id, StudentAssignedCourse studentAssignedCourse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != studentAssignedCourse.StudentAssignedCourseId)
            {
                return BadRequest();
            }

            db.Entry(studentAssignedCourse).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentAssignedCourseExists(id))
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

        // POST: api/StudentAssignedCourses
        [ResponseType(typeof(StudentAssignedCourse))]
        public async Task<IHttpActionResult> PostStudentAssignedCourse(StudentAssignedCourse studentAssignedCourse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
        

            db.StudentAssignedCourses.Add(studentAssignedCourse);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = studentAssignedCourse.StudentAssignedCourseId }, studentAssignedCourse);
        }

        // DELETE: api/StudentAssignedCourses/5
        [ResponseType(typeof(StudentAssignedCourse))]
        public async Task<IHttpActionResult> DeleteStudentAssignedCourse(int id)
        {
            StudentAssignedCourse studentAssignedCourse = await db.StudentAssignedCourses.FindAsync(id);
            if (studentAssignedCourse == null)
            {
                return NotFound();
            }

            db.StudentAssignedCourses.Remove(studentAssignedCourse);
            await db.SaveChangesAsync();

            return Ok(studentAssignedCourse);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StudentAssignedCourseExists(int id)
        {
            return db.StudentAssignedCourses.Count(e => e.StudentAssignedCourseId == id) > 0;
        }
    }
}