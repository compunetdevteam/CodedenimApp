﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CodedenimWebApp.Controllers.Api.ApiViewModel;
using CodedenimWebApp.Models;
using CodeninModel;

namespace CodedenimWebApp.Controllers.Api
{
    [System.Web.Http.RoutePrefix("api/Modules")]
    public class ModulesController : ApiController
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private readonly ConvertEmail _convert = new ConvertEmail();

        // GET: api/Modules
        [Route("GetModules")]
        public IQueryable GetModules()
        {
            // return db.Modules.Include(m => m.Topics).ToList();
            var topicVm = new List<TopicVm>();

            return _db.Modules.Include(x => x.Topics).Select(x => new ModuleVm
            {
                CourseId = x.CourseId,
                ExpectedTime = x.ExpectedTime,
                ModuleDescription = x.ModuleDescription,
                ModuleId = x.ModuleId,
                ModuleName = x.ModuleName,
                Topics = x.Topics.Select(t => new TopicVm
                {
                    TopicId = t.TopicId,
                    ModuleId = t.ModuleId,
                    TopicName = t.TopicName,
                    ExpectedTime = t.ExpectedTime
                })
            });
        }


    
        // GET: api/Modules/5
        [Route("GetModule")]
        public async Task<IHttpActionResult> GetModule(int id, string email)
        {
            var access = await CourseAvailability(id, email);
            var module = new ModuleTrackVm();
            if (access.Item1)
            {
                module.ModuleVms = await _db.Modules.Where(c => c.CourseId.Equals(id))
                                       .Select(m => new ModuleVm
                                       {
                                           ModuleId = m.ModuleId,
                                           ModuleName = m.ModuleName,
                                           ModuleDescription = m.ModuleDescription,
                                           ExpectedTime = m.ExpectedTime,
                                           CourseId = m.CourseId,
                                           Topics = m.Topics.Select(t => new TopicVm
                                           {
                                               TopicId = t.TopicId,
                                               ModuleId = t.ModuleId,
                                               TopicName = t.TopicName,
                                               ExpectedTime = t.ExpectedTime
                                           })
                                       }).ToListAsync();
            }
            else
            {
                module.ModuleVms = null;
                module.Message = access.Item2;
            }          

            return Ok(module);
        }

        async Task<Tuple<bool, string>> CourseAvailability(int courseId, string email)
        {
            var course = await _db.Courses.FindAsync(courseId);
            var studentId = _convert.ConvertEmailToId(email);
            if (!string.IsNullOrEmpty(studentId) && course != null)
            {
                if (course.CourseNumber == 1)
                {
                    var newTrack = new StudentCourseTrack
                    {
                        CourseId = courseId,
                        StudentId = studentId,
                        StartDate = DateTime.Now.Date
                    };
                    _db.StudentCourseTracks.Add(newTrack);
                    await _db.SaveChangesAsync();
                    return new Tuple<bool, string>(true, "");
                }

                var track = _db.StudentCourseTracks.Include(i => i.Course).AsNoTracking().FirstOrDefault(x => x.StudentId.Equals(studentId));
                if(track != null)
                {
                    if (course.CourseNumber < track.Course.CourseNumber)
                    {
                        return new Tuple<bool, string>(true, "");
                    }                    

                    int nextCourse = track.Course.CourseNumber + 1;
                    var endDate = Convert.ToDateTime(track.EndDate);
                    int dateCompare1 = DateTime.Compare(DateTime.Now.Date, endDate);

                    if (course.CourseNumber.Equals(nextCourse) && dateCompare1 > 0)
                    {
                        track.CourseId = course.CourseId;
                        track.StartDate = DateTime.Now.Date;
                        _db.Entry(track).State = EntityState.Modified;
                        await _db.SaveChangesAsync();
                        return new Tuple<bool, string>(true, "");
                    }
                    else if(course.CourseNumber > nextCourse)
                    {
                        var studentQuiz = _db.TestAnswers.AsNoTracking().FirstOrDefault(x => x.StudentId.Equals(studentId));
                        if(studentQuiz == null)
                        {
                            return new Tuple<bool, string>(false, $"You have to finish the prerequisite course ({track.Course.CourseName}) before you can take this course");
                        }
                        else
                        {
                            var remainingDays = endDate.Subtract(DateTime.Now.Date);
                            return new Tuple<bool, string>(false, $"You have {remainingDays} day(s) to start the selected course ({course.CourseName})");
                        }
                      
                    }
                  
                }
                var firstCourse = _db.Courses.AsNoTracking().FirstOrDefault(x => x.CourseNumber.Equals(1));
                return new Tuple<bool, string>(false, $"You have to start the first course ({firstCourse?.CourseName}) before you can take this course");
            }
            return new Tuple<bool, string>(false, "Course selected doesn't exist yet for this category of student");
        }



        //// PUT: api/Modules/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutModule(int id, Module module)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != module.ModuleId)
        //    {
        //        return BadRequest();
        //    }

        //    _db.Entry(module).State = EntityState.Modified;

        //    try
        //    {
        //        await _db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ModuleExists(id))
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

        //// POST: api/Modules
        //[ResponseType(typeof(Module))]
        //public async Task<IHttpActionResult> PostModule(Module module)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    _db.Modules.Add(module);
        //    await _db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = module.ModuleId }, module);
        //}

        //// DELETE: api/Modules/5
        //[ResponseType(typeof(Module))]
        //public async Task<IHttpActionResult> DeleteModule(int id)
        //{
        //    Module module = await _db.Modules.FindAsync(id);
        //    if (module == null)
        //    {
        //        return NotFound();
        //    }

        //    _db.Modules.Remove(module);
        //    await _db.SaveChangesAsync();

        //    return Ok(module);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ModuleExists(int id)
        {
            return _db.Modules.Count(e => e.ModuleId == id) > 0;
        }
    }
}