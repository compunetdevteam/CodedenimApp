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
using CodeninModel.Quiz;
using OfficeOpenXml;

namespace CodedenimWebApp.Controllers
{
    public class TopicsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Topics
        public async Task<ActionResult> Index()
        {

            var topics = db.Topics.Include(t => t.Module);
            return View(await topics.ToListAsync());
        }

        public void TotalTopics()
        {
            ViewBag.TotalTopics = db.Topics.Count();
        }
        //public async Task<ActionResult> GetIndex()
        //{
        //    #region Server Side filtering
        //    //Get parameter for sorting from grid table
        //    // get Start (paging start index) and length (page size for paging)
        //    var draw = Request.Form.GetValues("draw").FirstOrDefault();
        //    var start = Request.Form.GetValues("start").FirstOrDefault();
        //    var length = Request.Form.GetValues("length").FirstOrDefault();
        //    //Get Sort columns values when we click on Header Name of column
        //    //getting column name
        //    var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
        //    //Soring direction(either desending or ascending)
        //    var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
        //    string search = Request.Form.GetValues("search[value]").FirstOrDefault();

        //    int pageSize = length != null ? Convert.ToInt32(length) : 0;
        //    int skip = start != null ? Convert.ToInt32(start) : 0;
        //    int totalRecords = 0;

        //    var v = db.Topics.AsNoTracking().Where(x => x.TopicId.ToString().Equals(search)).ToList();
        //    if (!string.IsNullOrEmpty(search))
        //    {
        //        //v = v.OrderBy(sortColumn + " " + sortColumnDir);
        //        v = db.Topics.AsNoTracking().Where(x => x.TopicId.ToString().Equals(search) || (x.ModuleId.ToString().Equals(search) || x.TopicName.Equals(search) || x.ExpectedTime.ToString().Equals(search)))
        //            .ToList();
        //    }
        //    totalRecords = v.Count();
        //    var data = v.Skip(skip).Take(pageSize).ToList();

        //    return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
        //    #endregion

        //    //return Json(new { data = await Db.Subjects.AsNoTracking().Select(s => new { s.SubjectId, s.SubjectCode, s.SubjectName }).ToListAsync() }, JsonRequestBehavior.AllowGet);
        //}

        //public async Task<PartialViewResult> Save(int id)
        //{
        //    var topics = await db.Topics.FindAsync(id);
        //    ViewBag.ModuleId = new SelectList(db.Modules, "ModuleId", "ModuleName");
        //    return PartialView(topics);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Save([Bind(Include = "TopicId,ModuleId,TopicName,ExpectedTime")] Topic topic)
        //{
        //    bool status = false;
        //    string message = string.Empty;

        //        if (ModelState.IsValid)
        //        {
        //            db.Topics.Add(topic);
        //            await db.SaveChangesAsync();
        //            return RedirectToAction("Index");
        //        }
        //        ViewBag.ModuleId = new SelectList(db.Modules, "ModuleId", "ModuleName", topic.ModuleId);    
        //        return new JsonResult { Data = new { status = status, message = message } };
            
        //}

        // GET: Topics/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var topics = new CourseContentVm();
            Topic topic = await db.Topics.FindAsync(id);
            
            var topicContent = await db.Topics.Where(x => x.TopicId.Equals((int)id)).ToListAsync();
            topics.Topics = topicContent;
            topics.TopicsAD = topic;
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topics);
        }

        // GET: Topics/Create
        public ActionResult Create()
        {
            ViewBag.ModuleId = new SelectList(db.Modules, "ModuleId", "ModuleName");
            return View();
        }

        // POST: Topics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TopicId,ModuleId,TopicName,ExpectedTime")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                db.Topics.Add(topic);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ModuleId = new SelectList(db.Modules, "ModuleId", "ModuleName", topic.ModuleId);
            return View(topic);
        }


        /// <summary>
        /// Create Partial for topic pop-up
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Topics/Edit/5
        public PartialViewResult CreatePartial(int id)
        {
            ViewBag.ModuleId = new SelectList(db.Modules.Where(x => x.ModuleId.Equals((int)id)).ToList(), "ModuleId", "ModuleName");
            return PartialView();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreatePartial(Topic topic)
        {
            if (ModelState.IsValid)
            {
                db.Topics.Add(topic);
                await db.SaveChangesAsync();
                return new JsonResult { Data = new { status = true, message = "Saved Succesfully" } };
                // RedirectToAction("Index","Courses");
            }

            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", topic.ModuleId);
            return PartialView(topic);
        }
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = await db.Topics.FindAsync(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            ViewBag.ModuleId = new SelectList(db.Modules, "ModuleId", "ModuleName", topic.ModuleId);
            return View(topic);
        }

        // POST: Topics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TopicId,ModuleId,TopicName,ExpectedTime")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(topic).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ModuleId = new SelectList(db.Modules, "ModuleId", "ModuleName", topic.ModuleId);
            return View(topic);
        }


        /// <summary>
        /// Partial for edit topic
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> EditPartial(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = await db.Topics.FindAsync(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            ViewBag.ModuleId = new SelectList(db.Modules.Where(x => x.ModuleId.Equals(topic.ModuleId)).ToList(), "ModuleId", "ModuleName", topic.ModuleId);

            //ViewBag.ModuleId = new SelectList(db.Modules, "ModuleId", "ModuleName", topic.ModuleId);
            return PartialView(topic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPartial(Topic topic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(topic).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details","Modules",new{id = topic.ModuleId});
            }
            ViewBag.ModuleId = new SelectList(db.Modules, "ModuleId", "ModuleName", topic.ModuleId);
            return PartialView(topic);
        }



        // GET: Topics/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = await db.Topics.FindAsync(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // POST: Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Topic topic = await db.Topics.FindAsync(id);
            db.Topics.Remove(topic);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        // GET: Topics/Delete/5
        public async Task<ActionResult> DeletePartial(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = await db.Topics.FindAsync(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return PartialView(topic);
        }

        // POST: Topics/Delete/5
        [HttpPost, ActionName("DeletePartial")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedPartial(int id)
        {
            Topic topic = await db.Topics.FindAsync(id);
            db.Topics.Remove(topic);
            await db.SaveChangesAsync();
            return RedirectToAction("Details","Modules",new{id = topic.ModuleId});
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
                            var moduleName = sheet.Cells[row, 1].Value.ToString().Trim();
                            var module = db.Topics.Where(x => x.Module.ModuleName.Equals(moduleName)).Select(x => x.ModuleId).FirstOrDefault();
                            int moduleId = module;
                            string topicName  = sheet.Cells[row, 2].Value.ToString().ToUpper().Trim();
                            int expectedTime = Int32.Parse(sheet.Cells[row, 3].Value.ToString().Trim().ToUpper());
                           
                            //var subjectName = db.Subjects.Where(x => x.SubjectCode.Equals(subjectValue))
                            //    .Select(c => c.SubjectId).FirstOrDefault();

                            var category = db.Topics.Where(x => x.ModuleId.Equals(moduleId) && x.TopicName.Equals(topicName));
                            var countFromDb = await category.CountAsync();
                            if (countFromDb >= 1)
                            {
                                return View("Error2");
                            }
                            var topic = new Topic()
                            {
                                TopicName = topicName,
                                ExpectedTime = expectedTime

                            };
                            db.Topics.Add(topic);

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
