using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CodedenimWebApp.Models;
using CodeninModel;

namespace CodedenimWebApp.Controllers
{
	public class TopicMaterialUploadsController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: TopicMaterialUploads
		public async Task<ActionResult> Index()
		{
			var topicMaterialUploads = db.TopicMaterialUploads.Include(t => t.Course);
			return View(await topicMaterialUploads.ToListAsync());
		}


		[HttpPost]
		public PartialViewResult Upload(HttpPostedFileBase file)
		{
			// Verify that the user selected a file
			if (file != null && file.ContentLength > 0)
			{
				// extract only the fielname
				var fileName = Path.GetFileName(file.FileName);
				// store the file inside ~/App_Data/uploads folder
				var path = Path.Combine(Server.MapPath("~/uploads"), fileName);
				

					 file.SaveAs(path);
			}
			// redirect back to the index action to show the form once again
			return PartialView("Create");
		}



		// GET: TopicMaterialUploads/Details/5
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			TopicMaterialUpload topicMaterialUpload = await db.TopicMaterialUploads.FindAsync(id);
			if (topicMaterialUpload == null)
			{
				return HttpNotFound();
			}
			return View(topicMaterialUpload);
		}

		// GET: TopicMaterialUploads/Create
		public ActionResult Create()
		{
			ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "TopicName");
			return View();
		}

		// POST: TopicMaterialUploads/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = "TopicMaterialUploadId,TopicId,Tutor,FileType,Name,Description,FileLocation")] TopicMaterialUpload topicMaterialUpload)
		{
			if (ModelState.IsValid)
			{
				db.TopicMaterialUploads.Add(topicMaterialUpload);
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}

			ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "TopicName", topicMaterialUpload.TopicId);
			return View(topicMaterialUpload);
		}

		// GET: TopicMaterialUploads/Edit/5
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			TopicMaterialUpload topicMaterialUpload = await db.TopicMaterialUploads.FindAsync(id);
			if (topicMaterialUpload == null)
			{
				return HttpNotFound();
			}
			ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "TopicName", topicMaterialUpload.TopicId);
			return View(topicMaterialUpload);
		}

		// POST: TopicMaterialUploads/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "TopicMaterialUploadId,TopicId,Tutor,FileType,Name,Description,FileLocation")] TopicMaterialUpload topicMaterialUpload)
		{
			if (ModelState.IsValid)
			{
				db.Entry(topicMaterialUpload).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "TopicName", topicMaterialUpload.TopicId);
			return View(topicMaterialUpload);
		}

		// GET: TopicMaterialUploads/Delete/5
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			TopicMaterialUpload topicMaterialUpload = await db.TopicMaterialUploads.FindAsync(id);
			if (topicMaterialUpload == null)
			{
				return HttpNotFound();
			}
			return View(topicMaterialUpload);
		}

		// POST: TopicMaterialUploads/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			TopicMaterialUpload topicMaterialUpload = await db.TopicMaterialUploads.FindAsync(id);
			db.TopicMaterialUploads.Remove(topicMaterialUpload);
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
