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
using CodedenimWebApp.Services;
using CodedenimWebApp.ViewModels;
using CodeninModel;
using OfficeOpenXml;

namespace CodedenimWebApp.Controllers
{
    public class ModulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Modules
        public async Task<ActionResult> Index()
        {
            var modules = db.Modules.Include(m => m.Course);
            return View(await modules.ToListAsync());
        }

        public async Task<ActionResult> MyCourses(string id)
        {
            var tutorCourses = await db.TutorCourses.Where(x => x.TutorId.Equals(id)).ToListAsync();
            return View(tutorCourses);
        }
        // GET: Modules/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var moduleInfo = new CourseContentVm();
            Module module = await db.Modules.FindAsync(id);

 
            var course = await db.Courses.FindAsync(id);
            var courseId = await db.Modules.Where(x => x.ModuleId.Equals((int)id)).Select(x => x.CourseId).FirstOrDefaultAsync();
            var modeules = await db.Modules.Where(x => x.CourseId.Equals((int)id)).ToListAsync(); 
            var topics = await db.Topics.Where(x => x.ModuleId.Equals((int)id)).ToListAsync();
            var topicContents = new List<int>();
            foreach (var topic in topics)
            {
                var topicContent = db.TopicMaterialUploads.Where(x => x.TopicId.Equals(topic.TopicId))
                                                          .Select(x => x.TopicMaterialUploadId).ToList();
                topicContents.AddRange(topicContent);
            }
           
            moduleInfo.CoursesAD = course;
            moduleInfo.Modules = modeules;
            moduleInfo.Topics = topics;
            moduleInfo.ModulesAD = module;
            moduleInfo.TopicContentInts = topicContents;
            moduleInfo.CourseIdentifier = courseId;

            if (module == null)
            {
                return HttpNotFound();
            }
            return View(moduleInfo);
        }

        //GET: Modules/Create
        public ActionResult Create(int? id)
        {
            if (id != null)
            {
                ViewBag.CourseId = new SelectList(db.Courses.Where(x => x.CourseId.Equals(id.Value)).ToList(), "CourseId", "CourseName");
                // ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode");

            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName");
            //  ViewBag.CourseId = new SelectList(db.Courses.Where(x => x.CourseId.Equals(id.Value)).ToList(), "CourseId", "CourseCode");


            return View();
        }

        //POST: Modules/Create
        //To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ModuleId,CourseId,ModuleName,ModuleDescription,ExpectedTime")] Module module)
        {
            if (ModelState.IsValid)
            {
                db.Modules.Add(module);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(db.Modules, "CourseId", "CourseCode", module.CourseId);
            return View(module);
        }


        public PartialViewResult CreatePartial(int id)
        {
            ViewBag.CourseId = new SelectList(db.Courses.Where(x => x.CourseId.Equals((int)id)).ToList(), "CourseId", "CourseName");
            return PartialView();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreatePartial(Module module)
        {
            if (ModelState.IsValid)
            {
                db.Modules.Add(module);
                await db.SaveChangesAsync();
                return new JsonResult { Data = new { status = true, message = "Saved Succesfully" } };
                // RedirectToAction("Index","Courses");
            }

            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", module.CourseId);
            return PartialView(module);
        }
            
        public ActionResult Redirect()
        {
            return RedirectToAction("TutorDashBoard","Tutors");
        }
        // GET: Modules/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = await db.Modules.FindAsync(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", module.CourseId);
            return View(module);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ModuleId,CourseId,ModuleName,ModuleDescription,ExpectedTime")] Module module)
        {
            if (ModelState.IsValid)
            {
                db.Entry(module).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", module.CourseId);
            return View(module);
        }


        /// <summary>
        /// Partial Edit for Modules 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Modules/Edit/5
        public async Task<ActionResult> EditPartial(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var module = await db.Modules.FindAsync((int) id);
            if (module == null)
            {
                return HttpNotFound();
            }

           // ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", module.CourseId);
            ViewBag.CourseId = new SelectList(db.Courses.Where(x => x.CourseId.Equals(module.CourseId)).ToList(), "CourseId", "CourseName", module.CourseId);
            return PartialView(module);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPartial(Module module)
        {
            //ViewBag.CourseId = new SelectList(db.Modules.Where(x => x.ModuleId.Equals((int)id)).Select(x => x.CourseId).ToList(), "CourseId", "CourseName", module.CourseId);

            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", module.CourseId);
            if (ModelState.IsValid)
            {
                //module.ModuleId = (int)ViewBag.CourseId;
                db.Entry(module).State = EntityState.Modified;
                await db.SaveChangesAsync();
               return RedirectToAction("Details","Courses",new {id = module.CourseId});
            }
           
            return PartialView(module);
        }



        // GET: Modules/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = await db.Modules.FindAsync(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Module module = await db.Modules.FindAsync(id);
            db.Modules.Remove(module);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        // GET: Modules/Delete/5
        public async Task<ActionResult> DeletePartial(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = await  db.Modules.FindAsync(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return PartialView(module);
        }


        [HttpPost, ActionName("DeletePartial")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeletePartial(int id)
        {
            Module module = await db.Modules.FindAsync(id);
            var topics = db.Topics.Where(x => x.ModuleId.Equals(module.ModuleId)).ToList();
            foreach (var topic in topics)
            {
                var topicMaterials = db.TopicMaterialUploads.Where(x => x.TopicId.Equals(topic.TopicId)).ToList();
                db.TopicMaterialUploads.RemoveRange(topicMaterials);
            }
            db.Topics.RemoveRange(topics);
            db.Modules.Remove(module);
            await db.SaveChangesAsync();
            return RedirectToAction("Details","Courses", new{id = module.CourseId});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

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
                        int requiredField = 4;

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
                            var course = sheet.Cells[row, 1].Value.ToString().Trim();
                            var courseName = db.Courses.Where(x => x.CourseName.Equals(course)).Select(x => x.CourseId).FirstOrDefault();

                            int courseId = courseName;
                            string moduleName = sheet.Cells[row, 2].Value.ToString().ToUpper().Trim();
                            string moduleDescription = sheet.Cells[row, 3].Value.ToString().Trim().ToUpper();
                            var expectedTime = Int32.Parse(sheet.Cells[row, 4].Value.ToString().Trim());

                            //var subjectName = db.Subjects.Where(x => x.SubjectCode.Equals(subjectValue))
                            //    .Select(c => c.SubjectId).FirstOrDefault();

                          //  var category = db.Modules.Where(x => x.CourseId.Equals(courseName) && x.ModuleName.Equals(moduleName));
//var countFromDb = await category.CountAsync();
                            //if (countFromDb >= 1)
                            //{
                            //    return View("Error2");
                            //}


                            //foreach (var courseIdInExce in courseName)
                            //{
                               // var cs = courseIdInExce;

                                var myModule = new Module()
                                    {



                                        CourseId = courseId,


                                        ModuleName = moduleName,
                                        ModuleDescription = moduleDescription,
                                        ExpectedTime = expectedTime



                                    }

                                    ;
                                db.Modules.Add(myModule);
                            }
                            recordCount++;
                            //lastrecord = $"The last Updated record has the Student Id {studentId} and Subject Name is {subjectName}. Please Confirm!!!";
                        }
                    }
                }
                await db.SaveChangesAsync();
           // }
            return View("Index");
        }

    }
}
