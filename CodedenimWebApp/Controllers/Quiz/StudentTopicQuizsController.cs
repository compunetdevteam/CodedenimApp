using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CodedenimWebApp.Models;
using CodeninModel.Quiz;

namespace CodedenimWebApp.Controllers.Quiz
{
    public class StudentTopicQuizsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: StudentTopicQuizs
        public async Task<ActionResult> Index()
        {
            var studentTopicQuizs = db.StudentTopicQuizs.Include(s => s.Student).Include(s => s.Topic);
            return View(await studentTopicQuizs.ToListAsync());
        }

        // GET: StudentTopicQuizs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentTopicQuiz studentTopicQuiz = await db.StudentTopicQuizs.FindAsync(id);
            if (studentTopicQuiz == null)
            {
                return HttpNotFound();
            }
            return View(studentTopicQuiz);
        }

        // GET: StudentTopicQuizs/Create
        public ActionResult Create()
        {
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FullName");
            ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "TopicName");
            return View();
        }

        // POST: StudentTopicQuizs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "StudentTopicQuizId,StudentQuestionId,StudentId,TopicId,Question,Option1,Option2,Option3,Option4,Check1,Check2,Check3,Check4,FilledAnswer,Answer,QuestionHint,QuestionNumber,IsCorrect,IsFillInTheGag,IsMultiChoiceAnswer,SelectedAnswer,TotalQuestion,ExamTime")] StudentTopicQuiz studentTopicQuiz)
        {
            if (ModelState.IsValid)
            {
                db.StudentTopicQuizs.Add(studentTopicQuiz);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "Institution", studentTopicQuiz.StudentId);
            ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "TopicName", studentTopicQuiz.TopicId);
            return View(studentTopicQuiz);
        }

        // GET: StudentTopicQuizs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentTopicQuiz studentTopicQuiz = await db.StudentTopicQuizs.FindAsync(id);
            if (studentTopicQuiz == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "Institution", studentTopicQuiz.StudentId);
            ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "TopicName", studentTopicQuiz.TopicId);
            return View(studentTopicQuiz);
        }

        // POST: StudentTopicQuizs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "StudentTopicQuizId,StudentQuestionId,StudentId,TopicId,Question,Option1,Option2,Option3,Option4,Check1,Check2,Check3,Check4,FilledAnswer,Answer,QuestionHint,QuestionNumber,IsCorrect,IsFillInTheGag,IsMultiChoiceAnswer,SelectedAnswer,TotalQuestion,ExamTime")] StudentTopicQuiz studentTopicQuiz)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentTopicQuiz).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "Institution", studentTopicQuiz.StudentId);
            ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "TopicName", studentTopicQuiz.TopicId);
            return View(studentTopicQuiz);
        }

        // GET: StudentTopicQuizs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentTopicQuiz studentTopicQuiz = await db.StudentTopicQuizs.FindAsync(id);
            if (studentTopicQuiz == null)
            {
                return HttpNotFound();
            }
            return View(studentTopicQuiz);
        }

        // POST: StudentTopicQuizs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            StudentTopicQuiz studentTopicQuiz = await db.StudentTopicQuizs.FindAsync(id);
            db.StudentTopicQuizs.Remove(studentTopicQuiz);
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
    }
}
