﻿using CodedenimWebApp.Models;
using CodeninModel.Forums;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CodedenimWebApp.Controllers
{
    public class ForumQuestionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ForumQuestions
        public async Task<ActionResult> Index()
        {

            var forumQuestions = db.ForumQuestions.Include(f => f.Forum).Include(f => f.ForumQuestionView).Include(f => f.Students).Include(x => x.ForumAnswers);
            return View(await forumQuestions.ToListAsync());
        }

        public ActionResult ForumQuestion(int? page)
        {
            int pageNo = (page ?? 1);
            int PerPageSize = 5;
            var forumQuestions = db.ForumQuestions.Include(f => f.Forum).Include(f => f.ForumQuestionView).Include(f => f.Students).Include(x => x.ForumAnswers);
            return View(forumQuestions.OrderBy(x => x.PostDate).ToPagedList(pageNo, PerPageSize));
        }


        public async Task<ActionResult> Asked()
        {
            var userId = User.Identity.GetUserId();

            ViewBag.MyQuestion = await db.ForumQuestions.Where(x => x.StudentId.Equals(userId)).Select(x => new { x.QuestionName, x.ForumQuestionId }).ToListAsync();
            return View();
        }

        // GET: ForumQuestions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForumQuestion forumQuestion = await db.ForumQuestions.FindAsync(id);
            if (forumQuestion == null)
            {
                return HttpNotFound();
            }
            return View(forumQuestion);
        }

        // GET: ForumQuestions/Create
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();

            var student = db.Students.Where(x => x.StudentId.Equals(userId)).ToList();

            //var enrolledCoursesId = db.CorperEnrolledCourses.Where(x => x.StudentId.Equals(userId))
            //                           .Select(x => x.CourseCategoryId).FirstOrDefault();
            //var courseId = db.AssignCourseCategories.Where(x => x.CourseCategoryId.Equals(enrolledCoursesId))
            //                        .Select(x => x.CourseId);


            ViewBag.CourseId = new SelectList(db.Fora, "CourseId", "ForumName");
            ViewBag.ForumQuestionId = new SelectList(db.ForumQuestionViews, "ForumQuestionId", "ForumQuestionId");
            ViewBag.StudentId = new SelectList(student, "StudentId", "FullName");
            return View();
        }

        // POST: ForumQuestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ForumQuestion forumQuestion)
        {
            if (ModelState.IsValid)
            {
                forumQuestion.StudentId = User.Identity.GetUserId();
                forumQuestion.PostDate = DateTime.Now;
                db.ForumQuestions.Add(forumQuestion);
                await db.SaveChangesAsync();
                return RedirectToAction("ForumQuestion");
            }

            ViewBag.CourseId = new SelectList(db.Fora, "CourseId", "ForumName", forumQuestion.CourseId);
            ViewBag.ForumQuestionId = new SelectList(db.ForumQuestionViews, "ForumQuestionId", "ForumQuestionId", forumQuestion.ForumQuestionId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FullName", forumQuestion.StudentId);
            return View(forumQuestion);
        }

        // GET: ForumQuestions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForumQuestion forumQuestion = await db.ForumQuestions.FindAsync(id);
            if (forumQuestion == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Fora, "CourseId", "ForumName", forumQuestion.CourseId);
            ViewBag.ForumQuestionId = new SelectList(db.ForumQuestionViews, "ForumQuestionId", "ForumQuestionId", forumQuestion.ForumQuestionId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "Institution", forumQuestion.StudentId);
            return View(forumQuestion);
        }

        // POST: ForumQuestions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ForumQuestionId,Title,QuestionName,CourseId,StudentId")] ForumQuestion forumQuestion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(forumQuestion).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Fora, "CourseId", "ForumName", forumQuestion.CourseId);
            ViewBag.ForumQuestionId = new SelectList(db.ForumQuestionViews, "ForumQuestionId", "ForumQuestionId", forumQuestion.ForumQuestionId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "Institution", forumQuestion.StudentId);
            return View(forumQuestion);
        }

        // GET: ForumQuestions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForumQuestion forumQuestion = await db.ForumQuestions.FindAsync(id);
            if (forumQuestion == null)
            {
                return HttpNotFound();
            }
            return View(forumQuestion);
        }

        // POST: ForumQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ForumQuestion forumQuestion = await db.ForumQuestions.FindAsync(id);
            db.ForumQuestions.Remove(forumQuestion);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public async Task<ActionResult> ForumThread(int? page)
        {
            int pageNo = (page ?? 1);
            int PerPageSize = 5;
            var forumQuestions = await db.ForumQuestions.Include(f => f.Forum).Include(f => f.ForumQuestionView).Include(f => f.Students).Include(x => x.ForumAnswers).ToListAsync();
            return View(forumQuestions.OrderBy(x => x.PostDate).ToPagedList(pageNo, PerPageSize));

        }
    }
}
