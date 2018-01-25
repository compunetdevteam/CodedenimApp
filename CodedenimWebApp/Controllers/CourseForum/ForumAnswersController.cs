using CodedenimWebApp.Models;
using CodeninModel.Forums;
using Microsoft.AspNet.Identity;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CodedenimWebApp.Controllers.CourseForum
{
    public class ForumAnswersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ForumAnswers
        public async Task<ActionResult> Index(int? id)
        {
            var forumAnswers = await db.ForumAnswers.Include(f => f.ForumQuestions).ToListAsync();
            if (id != null)
            {
                forumAnswers = forumAnswers.Where(x => x.ForumQuestionId.Equals((int)id)).ToList();
                ViewBag.AnswerCount = forumAnswers.Count();
            }
           
            return View(forumAnswers);
        }

        // GET: ForumAnswers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForumAnswer forumAnswer = await db.ForumAnswers.FindAsync(id);
            if (forumAnswer == null)
            {
                return HttpNotFound();
            }   
            return View(forumAnswer);
        }

        // GET: ForumAnswers/Create
        public ActionResult Create(int id)
        {
           ViewBag.ForumQuestionId = new SelectList(db.ForumQuestions.Where(x => x.ForumQuestionId.Equals(id)), "ForumQuestionId", "Title");
           // ViewBag.QuestionId = db.ForumQuestions.Where(x => x.ForumQuestionId.Equals(id)).ToList();
            return View();
        }

        // POST: ForumAnswers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ForumAnswerId,Answer,ReplyDate,ForumQuestionId,UserId")] ForumAnswer forumAnswer)
        {
            if (ModelState.IsValid)
            {
                forumAnswer.UserId = User.Identity.GetUserId();
                forumAnswer.ReplyDate = DateTime.Now;
                db.ForumAnswers.Add(forumAnswer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

          //SS  ViewBag.ForumQuestionId = new SelectList(db.ForumQuestions, "ForumQuestionId", "Title", forumAnswer.ForumQuestionId);
            return View(forumAnswer);
        }

        // GET: ForumAnswers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForumAnswer forumAnswer = await db.ForumAnswers.FindAsync(id);
            if (forumAnswer == null)
            {
                return HttpNotFound();
            }
            ViewBag.ForumQuestionId = new SelectList(db.ForumQuestions, "ForumQuestionId", "Title", forumAnswer.ForumQuestionId);
            return View(forumAnswer);
        }

        // POST: ForumAnswers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ForumAnswerId,Answer,ReplyDate,ForumQuestionId,UserId")] ForumAnswer forumAnswer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(forumAnswer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ForumQuestionId = new SelectList(db.ForumQuestions, "ForumQuestionId", "Title", forumAnswer.ForumQuestionId);
            return View(forumAnswer);
        }

        // GET: ForumAnswers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForumAnswer forumAnswer = await db.ForumAnswers.FindAsync(id);
            if (forumAnswer == null)
            {
                return HttpNotFound();
            }
            return View(forumAnswer);
        }

        // POST: ForumAnswers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ForumAnswer forumAnswer = await db.ForumAnswers.FindAsync(id);
            db.ForumAnswers.Remove(forumAnswer);
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
