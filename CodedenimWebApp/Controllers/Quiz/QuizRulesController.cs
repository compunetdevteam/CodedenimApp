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
using CodeninModel.CBTE;

namespace CodedenimWebApp.Controllers.Quiz
{
    public class QuizRulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: QuizRules
        public async Task<ActionResult> Index()
        {
            var quizRules = db.QuizRules.Include(q => q.Topic);
            return View(await quizRules.ToListAsync());
        }

        // GET: QuizRules/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuizRule quizRule = await db.QuizRules.FindAsync(id);
            if (quizRule == null)
            {
                return HttpNotFound();
            }
            return View(quizRule);
        }

        // GET: QuizRules/Create
        public ActionResult Create()
        {
            ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "TopicName");
            return View();
        }

        // POST: QuizRules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "QuizRuleId,TopicId,ScorePerQuestion,TotalQuestion,MaximumTime")] QuizRule quizRule)
        {
            if (ModelState.IsValid)
            {
                db.QuizRules.Add(quizRule);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "TopicName", quizRule.TopicId);
            return View(quizRule);
        }

        // GET: QuizRules/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuizRule quizRule = await db.QuizRules.FindAsync(id);
            if (quizRule == null)
            {
                return HttpNotFound();
            }
            ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "TopicName", quizRule.TopicId);
            return View(quizRule);
        }

        // POST: QuizRules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "QuizRuleId,TopicId,ScorePerQuestion,TotalQuestion,MaximumTime")] QuizRule quizRule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quizRule).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "TopicName", quizRule.TopicId);
            return View(quizRule);
        }

        // GET: QuizRules/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuizRule quizRule = await db.QuizRules.FindAsync(id);
            if (quizRule == null)
            {
                return HttpNotFound();
            }
            return View(quizRule);
        }

        // POST: QuizRules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            QuizRule quizRule = await db.QuizRules.FindAsync(id);
            db.QuizRules.Remove(quizRule);
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
