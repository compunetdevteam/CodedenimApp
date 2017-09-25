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
            Topic topic = await db.Topics.FindAsync(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
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

        // GET: Topics/Edit/5
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
