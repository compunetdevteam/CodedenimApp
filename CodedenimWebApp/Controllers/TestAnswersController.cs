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
using Microsoft.AspNet.Identity;
using CodedenimWebApp.ViewModels;
using CodedenimWebApp.Controllers.Api.ApiViewModel;
using CodedenimWebApp.Controllers.Api;

namespace CodedenimWebApp.Controllers
{
    public class TestAnswersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TestAnswers
        public async Task<ActionResult> Index()
        {
            var testAnswers = db.TestAnswers.Include(t => t.Student).Include(t => t.TestQuestion);
            return View(await testAnswers.ToListAsync());
        }

        // GET: TestAnswers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestAnswer testAnswer = await db.TestAnswers.FindAsync(id);
            if (testAnswer == null)
            {
                return HttpNotFound();
            }
            return View(testAnswer);
        }

        // GET: TestAnswers/Create
        public async Task<ActionResult> Create(int courseId)
        {
            var testVm = new TestVm();
            var questionInfo = db.TestQuestions
                                 .Include(x => x.Course)
                                 .Include(x => x.TestAnswer)
                                 .Where(x => x.CourseId.Equals(courseId))
                                 .Select(x => new
                                 
                                 {
                                     x.QuestionContent,
                                     x.QuestionInstruction,
                                     x.CourseId,
                                     x.Course.CourseName,
                                     x.TestQuestionId,
                                 })
                                 .ToList();

            //thinking of implementing of a way to show the answer on this same view thats
            //when the student has answered it 

            var questionAnswers = db.TestAnswers
                                    .Where(x => x.hasAnswered.Equals(true))
                                    .Select(x => new {
                                        x.hasAnswered,
                                        x.TestAnswerContent,
                                        x.TestQuestionId,
                                        x.StudentId
                                    }).ToList();


            //iterating over the result of answers
            foreach (var answers in questionAnswers)
            {
                var testAnswers = new TestAnswerVm()
                {
                    Answer = answers.TestAnswerContent,
                    hasAnswered = answers.hasAnswered,
                    QuestionId = answers.TestQuestionId,
                    StudentId = answers.StudentId

                };

                testVm.TestAnswers.Add(testAnswers);
            }


            foreach (var question in questionInfo)
            {
                var questions = new TestQuestionVm()
                {
                    Question = question.QuestionContent,
                   // Instruction = question.QuestionInstruction,
                    CourseId = question.CourseId,
                    //CourseName = question.CourseName,
                    QuestionId = question.TestQuestionId

                };

                


            testVm.CourseName = question.CourseName;
                testVm.Instruction = question.QuestionInstruction;
               testVm.TestQuestion.Add(questions);
            }
           
           // testVm.TestAnswers = await db.TestAnswers.ToListAsync();

            if (courseId != null)
            {
                ViewBag.StudentId = new SelectList(db.Students, "StudentId", "MatricNo");
                ViewBag.TestQuestionId = new SelectList(db.TestQuestions.Where(x => x.CourseId.Equals(courseId)).ToList(), "TestQuestionId", "QuestionContent");

            }

                       return View(testVm);
        }

        // POST: TestAnswers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TestAnswer testAnswer)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                testAnswer.DateSubmited = DateTime.Now;
                testAnswer.hasAnswered = true;
                testAnswer.StudentId = userId;

                db.TestAnswers.Add(testAnswer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "MatricNo", testAnswer.StudentId);
            ViewBag.TestQuestionId = new SelectList(db.TestQuestions, "TestQuestionId", "QuestionContent", testAnswer.TestQuestionId);
            return View(testAnswer);
        }

        // GET: TestAnswers/Create
        public async Task<ActionResult> CreateMobile(int courseId,string email)
        {
            var studentEmail = new ConvertEmail1();
            var studentId = studentEmail.ConvertEmailToId(email);
            var testVm = new TestVm();
            var questionInfo = db.TestQuestions
                                 .Include(x => x.Course)
                                 .Include(x => x.TestAnswer)
                                 .Where(x => x.CourseId.Equals(courseId))
                                 .Select(x => new

                                 {
                                     x.QuestionContent,
                                     x.QuestionInstruction,
                                     x.CourseId,
                                     x.Course.CourseName,
                                     x.TestQuestionId,
                                 })
                                 .ToList();

            //thinking of implementing of a way to show the answer on this same view thats
            //when the student has answered it 
            if(questionInfo != null || studentId != null)
            {
                var questionAnswers = db.TestAnswers
                                  .Where(x => x.hasAnswered.Equals(true))
                                  .Select(x => new {
                                      x.hasAnswered,
                                      x.TestAnswerContent,
                                      x.TestQuestionId,
                                      x.StudentId
                                  }).ToList();


                //iterating over the result of answers
                foreach (var answers in questionAnswers)
                {
                    var testAnswers = new TestAnswerVm()
                    {
                        Answer = answers.TestAnswerContent,
                        hasAnswered = answers.hasAnswered,
                        QuestionId = answers.TestQuestionId,
                        StudentId = answers.StudentId

                    };

                    testVm.TestAnswers.Add(testAnswers);
                }


                foreach (var question in questionInfo)
                {
                    var questions = new TestQuestionVm()
                    {
                        Question = question.QuestionContent,
                        // Instruction = question.QuestionInstruction,
                        CourseId = question.CourseId,
                        //CourseName = question.CourseName,
                        QuestionId = question.TestQuestionId

                    };




                    testVm.CourseName = question.CourseName;
                    testVm.Instruction = question.QuestionInstruction;
                    testVm.StudentId = studentId;
                    testVm.TestQuestion.Add(questions);
                }

                // testVm.TestAnswers = await db.TestAnswers.ToListAsync();

                if (courseId != null)
                {
                    ViewBag.StudentId = new SelectList(db.Students, "StudentId", "MatricNo");
                    ViewBag.TestQuestionId = new SelectList(db.TestQuestions.Where(x => x.CourseId.Equals(courseId)).ToList(), "TestQuestionId", "QuestionContent");

                }
            }
          

            return View(testVm);
        }

        // POST: TestAnswers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateMobile(QuizAnswerVm testanswer)
        {
            if (ModelState.IsValid)
            {
                var studentEmail = new ConvertEmail1();
                var studentId = studentEmail.ConvertEmailToId(testanswer.StudentEmail);
                TestAnswer testAnswer = new TestAnswer();
                testAnswer.DateSubmited = DateTime.Now;
                testAnswer.hasAnswered = true;
                testAnswer.StudentId = studentId;
                testAnswer.TestAnswerContent = testanswer.Answer;
                testAnswer.TestQuestionId = testanswer.QuestionId;

                db.TestAnswers.Add(testAnswer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "MatricNo", testanswer.StudentEmail);
            ViewBag.TestQuestionId = new SelectList(db.TestQuestions, "TestQuestionId", "QuestionContent", testanswer.QuestionId);
            return View(testanswer);
        }

        //partial method for submiting test response
        public JsonResult SubmitAnswer(string questionId,string questionAnswer)
        {
            var json = new JsonResult();
            var answers = new TestAnswer();
            var userId = User.Identity.GetUserId();
            var questionID = int.Parse(questionId);
            var duplicateAnswer = db.TestAnswers.AsNoTracking().Where(x => x.StudentId.Equals(userId) && x.TestQuestionId.Equals(questionID) && x.hasAnswered.Equals(true)).FirstOrDefault();
            if (questionId != null && questionAnswer != null)
            {
                if (duplicateAnswer == null)
                {
                    answers.StudentId = userId;
                    answers.TestQuestionId = int.Parse(questionId);
                    answers.TestAnswerContent = questionAnswer;
                    answers.DateSubmited = DateTime.Now;
                    answers.hasAnswered = true;
                    db.TestAnswers.Add(answers);
                    db.SaveChanges();

                    json =  new JsonResult()
                    {
                        // Probably include a more detailed error message.
                        Data = new { status = "success" }
                    };
                }
              

            }
            else
            {
                json = new JsonResult()
                {
                    // Probably include a more detailed error message.
                    Data = new { status = "fail" }
                };
            }
            return json;
           
        }

        //partial method for submiting test response
        public JsonResult SubmitAnswerMobile(string questionId, string questionAnswer, string studentId)
        {
            var json = new JsonResult();
            var answers = new TestAnswer();
            //var userId = User.Identity.GetUserId();
            var questionID = int.Parse(questionId);
            var duplicateAnswer = db.TestAnswers.AsNoTracking().Where(x => x.StudentId.Equals(studentId) && x.TestQuestionId.Equals(questionID) && x.hasAnswered.Equals(true)).FirstOrDefault();
            if (questionId != null && questionAnswer != null)
            {
                if (duplicateAnswer == null)
                {
                    answers.StudentId = studentId;
                    answers.TestQuestionId = int.Parse(questionId);
                    answers.TestAnswerContent = questionAnswer;
                    answers.DateSubmited = DateTime.Now;
                    answers.hasAnswered = true;
                    db.TestAnswers.Add(answers);
                    db.SaveChanges();

                    json = new JsonResult()
                    {
                        // Probably include a more detailed error message.
                        Data = new { status = "success" }
                    };
                }


            }
            else
            {
                json = new JsonResult()
                {
                    // Probably include a more detailed error message.
                    Data = new { status = "fail" }
                };
            }
            return json;

        }

        // GET: TestAnswers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestAnswer testAnswer = await db.TestAnswers.FindAsync(id);
            if (testAnswer == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "MatricNo", testAnswer.StudentId);
            ViewBag.TestQuestionId = new SelectList(db.TestQuestions, "TestQuestionId", "QuestionContent", testAnswer.TestQuestionId);
            return View(testAnswer);
        }

        // POST: TestAnswers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TestAnswerId,TestAnswerContent,hasAnswered,DateSubmited,TestQuestionId,StudentId")] TestAnswer testAnswer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(testAnswer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "MatricNo", testAnswer.StudentId);
            ViewBag.TestQuestionId = new SelectList(db.TestQuestions, "TestQuestionId", "QuestionContent", testAnswer.TestQuestionId);
            return View(testAnswer);
        }

        // GET: TestAnswers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestAnswer testAnswer = await db.TestAnswers.FindAsync(id);
            if (testAnswer == null)
            {
                return HttpNotFound();
            }
            return View(testAnswer);
        }

        // POST: TestAnswers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TestAnswer testAnswer = await db.TestAnswers.FindAsync(id);
            db.TestAnswers.Remove(testAnswer);
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
