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
using CodeninModel;
using OfficeOpenXml;

namespace CodedenimWebApp.Controllers
{
    public class CourseCategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CourseCategories
        public async Task<ActionResult> Index()
        {
            return View(await db.CourseCategories.ToListAsync());
        }

        // GET: CourseCategories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseCategory courseCategory = await db.CourseCategories.FindAsync(id);
            if (courseCategory == null)
            {
                return HttpNotFound();
            }
            return View(courseCategory);
        }


        // GET: CourseCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CourseCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CourseCategoryId,CategoryName")] CourseCategory courseCategory)
        {
            if (ModelState.IsValid)
            {
                db.CourseCategories.Add(courseCategory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(courseCategory);
        }

        // GET: CourseCategories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseCategory courseCategory = await db.CourseCategories.FindAsync(id);
            if (courseCategory == null)
            {
                return HttpNotFound();
            }
            return View(courseCategory);
        }

        // POST: CourseCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CourseCategoryId,CategoryName")] CourseCategory courseCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(courseCategory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(courseCategory);
        }

        // GET: CourseCategories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseCategory courseCategory = await db.CourseCategories.FindAsync(id);
            if (courseCategory == null)
            {
                return HttpNotFound();
            }
            return View(courseCategory);
        }

        // POST: CourseCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CourseCategory courseCategory = await db.CourseCategories.FindAsync(id);
            db.CourseCategories.Remove(courseCategory);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        public ActionResult UpLoadCategory()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> UpLoadCategory(HttpPostedFileBase excelfile)
        {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Please Select a excel file <br/>";
                return View("UpLoadCategory");
            }
            else
            {
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
                        ExcelValidation myExcel = new ExcelValidation();
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        int requiredField = 1;

                        string validCheck = myExcel.ValidateExcel(noOfRow, workSheet, requiredField);
                        if (!validCheck.Equals("Success"))
                        {
                            //string row = "";
                            //string column = "";
                            string[] ssizes = validCheck.Split(' ');
                            string[] myArray = new string[2];
                            for (int i = 0; i < ssizes.Length; i++)
                            {
                                myArray[i] = ssizes[i];
                                // myArray[i] = ssizes[];
                            }
                            string lineError = $"Line/Row number {myArray[0]}  and column {myArray[1]} is not rightly formatted, Please Check for anomalies ";
                            //ViewBag.LineError = lineError;
                            TempData["UserMessage"] = lineError;
                            TempData["Title"] = "Error.";
                            return View();
                        }

                        for (int row = 2; row <= noOfRow; row++)
                        {
                            string CategoryName = workSheet.Cells[row, 1].Value.ToString().Trim();
                     
               
                            try
                            {
                                var category = new CourseCategory()
                                {
                                    CategoryName = CategoryName,
                                   
                            
                                };
                                db.CourseCategories.Add(category);


                                recordCount++;
                                //    lastrecord =
                                //        $"The last Updated record has the Last Name {lastName} and First Name {firstName} with Phone Number {phoneNumber}";
                                //
                            }
                            catch (Exception e)
                            {
                                message = $"You have successfully Uploaded {recordCount} records...  and {lastrecord}";
                                TempData["UserMessage"] = message;
                                TempData["Title"] = "Success.";
                                return View("Error3");
                            }


                        }
                        await db.SaveChangesAsync();
                        message = $"You have successfully Uploaded {recordCount} records...  and {lastrecord}";
                        TempData["UserMessage"] = message;
                        TempData["Title"] = "Success.";
                        return RedirectToAction("Index", "CourseCategories");
                    }
                }
                else
                {
                    ViewBag.Error = "File type is Incorrect <br/>";
                    return View("UploadStudent");
                }
            }
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
