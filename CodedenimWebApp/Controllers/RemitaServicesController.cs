using CodedenimWebApp.Models;
using CodedenimWebApp.Service;
using CodedenimWebApp.Services;
using CodedenimWebApp.ViewModels;
using CodeninModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftKampus.Controllers
{
   
    public class RemitaServicesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RemitaHash myHash = new RemitaHash();
        [AllowAnonymous]
        public ActionResult GetPaymentStatus(string message)
        {
            ViewBag.Message = message;
            return View();
        }


        public ActionResult ConfrimRrrPayment(RemitaResponse model)
        {
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetPaymentStatus(ConfirmRrr model)
        {
            if (!string.IsNullOrEmpty(model.rrr))
            {                
                return RedirectToAction("RetrySchoolFeePayment", "CourseCategories", new { rrr = model.rrr.Trim() });
                
            }
            ViewBag.Message = "RRR cannot be empty or the selected Category is not applicable yet ";
            return View();
        }


        // GET:RemitaServices     
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> PaymentNotification(List<RemitaNotificationVm> item)
        {
            if (ModelState.IsValid)
            {
                foreach (var model in item)
                {
                    if (!string.IsNullOrEmpty(model.orderRef) && !string.IsNullOrEmpty(model.rrr)
                        && !string.IsNullOrEmpty(model.serviceTypeId))
                    {
                        if (model.serviceTypeId.Equals(RemitaConfigParams.SERVICETYPEID))
                        {
                            await ProcessSchoolFee(model.rrr, model.orderRef);
                        }                       
                        else
                        {
                            return Content("Service Type is not registered on the Portal yet");
                        }

                    }
                }

            }
            return Content("Ok");
        }

        private async Task ProcessSchoolFee(string RRR, string orderID)
        {
            StudentPayment studentPayment;
            if (string.IsNullOrEmpty(orderID))
            {
                studentPayment = await db.StudentPayments.AsNoTracking()
                    .Where(x => x.ReferenceNo.Equals(RRR))
                    .FirstOrDefaultAsync();
            }
            else
            {
                studentPayment = await db.StudentPayments.AsNoTracking()
                    .Where(x => x.OrderId.Equals(orderID.Trim()))
                    .FirstOrDefaultAsync();
            }
            if (studentPayment != null)
            {
                if (studentPayment.IsPayed.Equals(true))
                {

                }
                else
                {
                    var log = await db.RemitaPaymentLogs.AsNoTracking()
                        .Where(x => x.OrderId.Equals(studentPayment.OrderId))
                        .FirstOrDefaultAsync();

                    var hashed = myHash.HashRemitedValidate(studentPayment.OrderId, RemitaConfigParams.APIKEY,
                        RemitaConfigParams.MERCHANTID);
                    string url = RemitaConfigParams.CHECKSTATUSURL + "/" + RemitaConfigParams.MERCHANTID + "/" +
                                 orderID + "/" + hashed + "/" + "orderstatus.reg";
                    string jsondata = new WebClient().DownloadString(url);
                    RemitaResponse result = JsonConvert.DeserializeObject<RemitaResponse>(jsondata);

                    if (result.Status.Equals("00") || result.Status.Equals("01"))
                    {
                        studentPayment.IsPayed = true;
                        studentPayment.PaymentStatus = result.Message;
                        studentPayment.ReferenceNo = result.Rrr;
                        db.Entry(studentPayment).State = EntityState.Modified;

                        log.Rrr = result.Rrr;
                        log.StatusCode = result.Status;
                        log.TransactionMessage = result.Message;
                        db.Entry(log).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        studentPayment.IsPayed = false;
                        studentPayment.PaymentStatus = result.Message;
                        studentPayment.ReferenceNo = result.Rrr;
                        db.Entry(studentPayment).State = EntityState.Modified;

                        log.Rrr = result.Rrr;
                        log.StatusCode = result.Status;
                        log.TransactionMessage = result.Message;
                        db.Entry(log).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                }


            }
        }
      

    }
}