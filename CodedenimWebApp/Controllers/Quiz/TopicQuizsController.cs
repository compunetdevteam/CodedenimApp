using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CodedenimWebApp.Controllers.Api.ForumApi;
using CodedenimWebApp.Models;
using CodedenimWebApp.Services;
using CodeninModel;
using CodeninModel.Quiz;
using OfficeOpenXml;

namespace CodedenimWebApp.Controllers.Quiz
{   
    public class TopicQuizsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TopicQuizs
        public async Task<ActionResult> Index() 
        {
            var topicQuizs = db.TopicQuizs;
            return View(await topicQuizs.ToListAsync());
        }

        // GET: TopicQuizs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TopicQuiz topicQuiz = await db.TopicQuizs.FindAsync(id);
            if (topicQuiz == null)
            {
                return HttpNotFound();
            }
            return View(topicQuiz);
        }

        // GET: TopicQuizs/Create
        public ActionResult Create()
        {
            ViewBag.ModuleId = new SelectList(db.Modules, "ModuleId", "ModuleName");
            var questionType = from QuestionType s in Enum.GetValues(typeof(QuestionType))
                select new {ID = s, Name = s.ToString()};
            ViewBag.QuestionType = new SelectList(questionType, "Name", "Name");

            return View();

        }

        // POST: TopicQuizs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TopicQuiz topicQuiz)
        {
            if (ModelState.IsValid)
            {
                db.TopicQuizs.Add(topicQuiz);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ModuleId = new SelectList(db.Modules, "ModuleId", "ModuleName", topicQuiz.ModuleId);
            return View(topicQuiz);
        }

        // GET: TopicQuizs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TopicQuiz topicQuiz = await db.TopicQuizs.FindAsync(id);
            if (topicQuiz == null)
            {
                return HttpNotFound();
            }
            ViewBag.TopicId = new SelectList(db.Modules, "ModuleId", "ModuleName", topicQuiz.ModuleId);
            return View(topicQuiz);
        }

        // POST: TopicQuizs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TopicQuizId,ModuleId,Question,Option1,Option2,Option3,Option4,Answer,QuestionHint,QuestionType,IsFillInTheGag,IsMultiChoiceAnswer,IsSingleChoiceAnswer")] TopicQuiz topicQuiz)
        {
            if (ModelState.IsValid)
            {
                db.Entry(topicQuiz).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.TopicId = new SelectList(db.Modules, "ModuleId", "ModuleName", topicQuiz.ModuleId);
            return View(topicQuiz);
        }

        // GET: TopicQuizs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TopicQuiz topicQuiz = await db.TopicQuizs.FindAsync(id);
            if (topicQuiz == null)
            {
                return HttpNotFound();
            }
            return View(topicQuiz);
        }

        // POST: TopicQuizs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TopicQuiz topicQuiz = await db.TopicQuizs.FindAsync(id);
            db.TopicQuizs.Remove(topicQuiz);
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


        //excel upload for the quiz

        [HttpPost]
        public async Task<ActionResult> ExcelUpload(HttpPostedFileBase excelfile)
        {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Please Select a excel file <br/>";
                return View();
            }
            HttpPostedFileBase file = Request.Files["excelfile"];
            if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
            {
                string lastrecord = "";
                int recordCount = 0;
                string message = "";
                string fileContentType = file.ContentType;
                byte[] fileBytes = new byte[file.ContentLength];
                var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                // Read data from excel file
                using (var package = new ExcelPackage(file.InputStream))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    foreach (var sheet in currentSheet)
                    {
                        ExcelValidation myExcel = new ExcelValidation();
                        //var workSheet = currentSheet.First();
                        var noOfCol = sheet.Dimension.End.Column;
                        var noOfRow = sheet.Dimension.End.Row;
                        int requiredField = 5;

                        string validCheck = myExcel.ValidateExcel(noOfRow, sheet, requiredField);
                        if (!validCheck.Equals("Success"))
                        {

                            string[] ssizes = validCheck.Split(' ');
                            string[] myArray = new string[2];
                            for (int i = 0; i < ssizes.Length; i++)
                            {
                                myArray[i] = ssizes[i];
                            }
                            string lineError =
                                $"Please Check sheet {sheet}, Line/Row number {myArray[0]}  and column {myArray[1]} is not rightly formatted, Please Check for anomalies ";
                            //ViewBag.LineError = lineError;
                            TempData["UserMessage"] = lineError;
                            TempData["Title"] = "Error.";
                            return View();
                        }

                        for (int row = 2; row <= noOfRow; row++)
                        {
                            var ModuleName = sheet.Cells[row, 1].Value.ToString().Trim();
                            var moduleId = db.Modules.Where(x => x.ModuleName.Equals(ModuleName)).Select(x => x.ModuleId)
                                .FirstOrDefault();
                       
                            string questionName = sheet.Cells[row, 2].Value.ToString().ToUpper().Trim();
                            string option1 = sheet.Cells[row, 3].Value.ToString().Trim().ToUpper();
                            string  option2 = sheet.Cells[row, 4].Value.ToString().ToUpper().Trim();
                            string option3 = sheet.Cells[row, 5].Value.ToString().Trim();
                            string option4 = sheet.Cells[row, 6].Value.ToString().Trim();
                            string answer = sheet.Cells[row, 7].Value.ToString().Trim();
                            string questionType = sheet.Cells[row, 8].Value.ToString().Trim();


                            //var subjectName = db.Subjects.Where(x => x.SubjectCode.Equals(subjectValue))
                            //    .Select(c => c.SubjectId).FirstOrDefault();

                            var category = db.TopicQuizs.Where(x => x.ModuleId.Equals(moduleId) && x.Question.Equals(questionName));
                            var countFromDb = await category.CountAsync();
                            if (countFromDb >= 1)
                            {
                                return View("Error2");
                            }
                            var quiz = new TopicQuiz()
                            {
                                ModuleId = moduleId,
                                Question = questionName,
                                Option1 = option1,
                                Option2 = option2,
                                Option3 = option3,
                                Option4 = option4,
                                Answer = answer,
                                QuestionType = questionType


                            };
                            db.TopicQuizs.Add(quiz);

                            recordCount++;
                            //lastrecord = $"The last Updated record has the Student Id {studentId} and Subject Name is {subjectName}. Please Confirm!!!";
                        }
                    }
                }
                await db.SaveChangesAsync();
            }
            return View("Index");
        }
    }
}
