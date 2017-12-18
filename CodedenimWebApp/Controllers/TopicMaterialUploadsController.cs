	using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
	using System.Web.Hosting;
	using System.Web.Mvc;
using CodedenimWebApp.Models;
	using CodedenimWebApp.Service;
	using CodedenimWebApp.ViewModels;
	using CodeninModel;
	using Microsoft.AspNet.Identity;
	using Microsoft.WindowsAzure.Storage.Blob;

namespace CodedenimWebApp.Controllers
{
	public class TopicMaterialUploadsController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();
	    private int moduleId = 0;
        //BlobService _blobService = new BlobService();

		// GET: TopicMaterialUploads
		public async Task<ActionResult> Index()
		{
			
			var topicMaterialUploads = db.TopicMaterialUploads.Include(t => t.Course);
			return View(await topicMaterialUploads.ToListAsync());
		}
        /// <summary>
        ///topic id is what comes into these method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
	    public PartialViewResult _StudentContent(int? id)
	    {
	        var viewModel = new CourseContentVm
	        {
	            Modules = db.Modules.Where(x => x.ModuleId.Equals(id)).ToList(),
	            Topics = db.Topics.Include(x => x.MaterialUploads).ToList(),

	        };
            return PartialView(viewModel);
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


        /// <summary>
        /// this method takes in a topic id and  a courseId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		// GET: TopicMaterialUploads/Details/5
		public async Task<ActionResult> Details(int id,int? courseId)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TopicMaterialUpload topicMaterialUpload = db.TopicMaterialUploads.Find(id);
            if (topicMaterialUpload == null)
            {
                return View("NoContent");
            }
            var topicContent = db.TopicMaterialUploads.Where(x => x.TopicId.Equals(id)).ToList();
		    var ModuleName = db.Topics.Where(x => x.TopicId.Equals(id)).Select(x => x.Module.ModuleName).FirstOrDefault();
		   RedirectToAction("SideBarContentForMaterial",new{topidId = id});
            List<Module> moduleDetail = null;
            if (courseId != null)
		    {
		         moduleDetail = db.Modules.Include(x => x.Topics).Where(x => x.CourseId.Equals((int)courseId)).ToList();
            }
		   
		    if (!topicContent.Any())
		    {
		        return View("NoContent");

		    }
		    var contents = new CourseContentVm();
		    contents.ModulesName = ModuleName;
		    contents.Materials = topicContent;
		    contents.Modules = moduleDetail;
		    // PartialView(contents);
            return View(contents);


        }


        // GET: TopicMaterialUploads/Details/5
        /// <summary>
        /// this will display the content of the course for a student
        /// </summary>
        /// <param name="id"></param>
        /// <returns>topic material</returns>
  
        public PartialViewResult DetailsContent(int id)
		{
		
		  //  var topic = db.Topics.Find(id);
		    var topicContent = db.TopicMaterialUploads.FirstOrDefault(x => x.TopicMaterialUploadId.Equals(id));
		    var contents = new CourseContentVm();
            //var fileName = db.TopicMaterialUploads.Where(x => x.TopicMaterialUploadId.Equals(id)).Select(x => x.FileLocation).FirstOrDefault();
            //CloudBlobContainer blobContainer = _blobService.GetCloudBlobContainer();
            //CloudBlockBlob blob = blobContainer.GetBlockBlobReference(fileName);

            //      contents.Blob = blob.OpenRead();
            TopicMaterialUpload topicMaterialUpload = db.TopicMaterialUploads.Find(id);

            if (topicMaterialUpload == null)
            {
                return PartialView("NoContent");
            }

            contents.Material = topicContent;
            return PartialView(contents);
            //this partial view will be uncommented when blob storage is activated
           // return PartialView(topicMaterialUpload);
		}

	    public PartialViewResult SideBarContentForMaterial(int id)
	    {
	        var contents = new CourseContentVm();
            var topicContent = db.TopicMaterialUploads.Where(x => x.TopicId.Equals(id)).ToList();
	        contents.Materials = topicContent;
            return PartialView(contents);
	    }


	
		public ActionResult Create(int id)
		{
		   // var user = User.Identity.GetUserName();
		    var topicMaterial = db.Topics.Find(id);


			ViewBag.TopicId = new SelectList(db.Topics.Where(x => x.TopicId.Equals(id)), "TopicId", "TopicName");
			ViewBag.UserName =  User.Identity.GetUserName();

			return View();
		}

		// POST: TopicMaterialUploads/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(TopicMaterialUpload topicMaterialUpload, HttpPostedFileBase File)
		{
			if (ModelState.IsValid)
			{
			   // var tutorId = User.Identity.GetUserId();
			    var topicId = topicMaterialUpload.TopicId;
			    var moduleId = db.Topics.Where(x => x.TopicId.Equals(topicId)).Select(x => x.ModuleId).FirstOrDefault();
				string _FileName = String.Empty;
                if (File.ContentLength > 0)
                {
                    _FileName = Path.GetFileName(File.FileName);
                    string path = HostingEnvironment.MapPath("~/MaterialUpload/") + _FileName;
                    topicMaterialUpload.FileLocation = path;
                    var directory = new DirectoryInfo(HostingEnvironment.MapPath("~/MaterialUpload/"));
                    if (directory.Exists == false)
                    {
                        directory.Create();
                    }
                    File.SaveAs(path);
                    //CloudBlobContainer blobContainer = _blobService.GetCloudBlobContainer();
                    //List<string> blobs = new List<string>();
                    //foreach (var blobItem in blobContainer.ListBlobs())
                    //{
                    //    blobs.Add(blobItem.Uri.ToString());

                    //}
                    //foreach (string item in Request.Files)
                    //{

                   // if (File.ContentLength == 0)
                   // {
                   //     ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "TopicName", topicMaterialUpload.TopicId);
                   //     return View(topicMaterialUpload);
                   // }
                       

                   //     if (File.ContentLength > 0)
                   //     {

                   //         CloudBlobContainer blobContainer = _blobService.GetCloudBlobContainer();
                   //         CloudBlockBlob blob = blobContainer.GetBlockBlobReference(File.FileName);
                   //         blob.UploadFromStream(File.InputStream);
                   //     }
                   //// }
                    //return RedirectToAction("Upload");
                }
               
                topicMaterialUpload.FileLocation = _FileName;


				db.TopicMaterialUploads.Add(topicMaterialUpload);
				await db.SaveChangesAsync();
				return RedirectToAction("Details","Modules",new{id= moduleId});
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
        /// <summary>
        /// this the a method that deletes from the azure blob container
        /// it takes an id of TopicMaterialUpload
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			TopicMaterialUpload topicMaterialUpload = await db.TopicMaterialUploads.FindAsync(id);
			db.TopicMaterialUploads.Remove(topicMaterialUpload);
            //var Name = db.TopicMaterialUploads.Where(x => x.TopicMaterialUploadId.Equals(id)).Select(x => x.FileLocation).FirstOrDefault();
            //Uri uri = new Uri(BlobService.BlobUri +"/" + Name);
            //string filename = Path.GetFileName(uri.LocalPath);
            //CloudBlobContainer blobContainer = _blobService.GetCloudBlobContainer();
            //CloudBlockBlob blob = blobContainer.GetBlockBlobReference(filename);
            //blob.Delete();

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
