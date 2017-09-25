using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CodedenimWebApp.Models;
using CodeninModel;
using Microsoft.AspNet.Identity;
using PayStack.Net;

namespace CodedenimWebApp.Controllers
{
    public class PaymentTypesController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: PaymentTypes
        public async Task<ActionResult> Index()
        {
            return View(await _db.PaymentTypes.ToListAsync());
        }


        public ActionResult Pay()
        {
            var testOrLiveSecret = ConfigurationManager.AppSettings["PayStackSecret"];
            var api = new PayStackApi(testOrLiveSecret);


            var userId = User.Identity.GetUserId();

            if (userId != null)
            {


                var person = _db.Users.AsNoTracking().SingleOrDefault(x => x.Id.Equals(userId));
                //var email = _db.Students.AsNoTracking().Where(x => x.StudentId.Equals(userId)).Select(x => x.Email).ToString();
                //var amount = _db.PaymentTypes
                //    .Where(x => x.PaymentTypeId.Equals(person.PaymentTypeId)).Select(x => x.Amount).FirstOrDefault();

                // var convert = new KoboToNaira();
               // var convertedamount = KoboToNaira.ConvertKoboToNaira(amount); 
                var transactionInitializaRequest = new TransactionInitializeRequest
                {
                    //Reference = "SwifKampus",
                    AmountInKobo = 1500000,
                    CallbackUrl = "http://localhost:64301/home/Dashboard",
                    Email = person.Email,
                    Bearer = "Application fee",

                    //CustomFields = new List<CustomField>
                    //{
                    //    new CustomField("professionalworkerid","professionalworkerid", worker.ProfessionalWorkerId),
                    //    new CustomField("amount", "amount", model.Amount.ToString(CultureInfo.InvariantCulture))
                    //    //new CustomField("sessionid", "sessionid", schoolFeePayment.SessionId.ToString()),
                    //    //new CustomField("feepaymentid","feepaymentid", schoolFeePayment.FeeCategoryId.ToString())
                    //    //new CustomField("paymentmode","paymentmode", schoolFeePayment.p)
                    //}

                };
                var response = api.Transactions.Initialize(transactionInitializaRequest);

                if (response.Status)
                {
                    //redirect to authorization url
                    return RedirectPermanent(response.Data.AuthorizationUrl);
                    // return Content("Successful");
                }
                return Content("An error occurred");
            }
            return RedirectToAction("Login", "Account");
            // Initializing a transaction
            //var response = api.Transactions.Initialize("davidzagi93@gmail.com", 5000000);
            //if (response.Status)
            //{

            //}
            //// use response.Data
            //else
            //{

            //}
            //// show response.Message

            //// Verifying a transaction
            //var verifyResponse = api.Transactions.Verify("transaction-reference"); // auto or supplied when initializing;
            //if (verifyResponse.Status)
            //{

            //}
            ///* 
            //    You can save the details from the json object returned above so that the authorization code 
            //    can be used for charging subsequent transactions

            //    // var authCode = verifyResponse.Data.Authorization.AuthorizationCode
            //    // Save 'authCode' for future charges!
            //*/
            //return View();
        }

        public async Task<ActionResult> ConfrimPayment(string reference)
        {
            var testOrLiveSecret = ConfigurationManager.AppSettings["PayStackSecret"];
            var api = new PayStackApi(testOrLiveSecret);
            //Verifying a transaction
            //var verifyResponse = api.Transactions.Verify(reference); // auto or supplied when initializing;
            //if (verifyResponse.Status)
            //{
            //    /* 
            //       You can save the details from the json object returned above so that the authorization code 
            //       can be used for charging subsequent transactions

            //       // var authCode = verifyResponse.Data.Authorization.AuthorizationCode
            //       // Save 'authCode' for future charges!

            //   */
            //    //var customfieldArray = verifyResponse.Data.Metadata.CustomFields.A

            //    var convertedValues = new List<SelectableEnumItem>();
            //    var valuepair = verifyResponse.Data.Metadata.Where(x => x.Key.Contains("custom")).Select(s => s.Value);

            //    foreach (var item in valuepair)
            //    {
            //        convertedValues = ((JArray)item).Select(x => new SelectableEnumItem
            //        {
            //            key = (string)x["display_name"],
            //            value = (string)x["value"]
            //        }).ToList();
            //    }
            //    //var studentid = _db.Users.Find(id);
            //    var professionalPayment = new ProfessionalPayment()
            //    {
            //        //FeeCategoryId = Convert.ToInt32(verifyResponse.Data.Metadata.CustomFields[3].Value),
            //        ProfessionalWorkerId = convertedValues.Where(x => x.key.Equals("professionalworkerid")).Select(s => s.value).FirstOrDefault(),
            //        PaymentDateTime = DateTime.Now,
            //        Amount = Convert.ToDecimal(convertedValues.Where(x => x.key.Equals("amount")).Select(s => s.value).FirstOrDefault()),
            //        IsPayed = true,
            //        //StudentId = "HAS-201",
            //        AmountPaid = _query.ConvertToNaira(verifyResponse.Data.Amount),

            //    };
            //    _db.ProfessionalPayments.Add(professionalPayment);
            //    await _db.SaveChangesAsync();
            //}

            return RedirectToAction("ConfrimPayment");
        }



        public static class KoboToNaira
        {
            public static int ConvertKoboToNaira(int naira)
            {

                var kobo = naira * 100;
                return kobo;
            }
        }

        public interface ITransactionsApi
        {
            TransactionInitializeResponse Initialize(string email, int amount);
            TransactionInitializeResponse Initialize(TransactionInitializeRequest request);
            TransactionVerifyResponse Verify(string reference);
            TransactionListResponse List(TransactionListRequest request = null);
            TransactionFetchResponse Fetch(string transactionId);
            TransactionTimelineResponse Timeline(string transactionIdOrReference);
            TransactionTotalsResponse Totals(DateTime? from = null, DateTime? to = null);

            TransactionExportResponse Export(DateTime? from = null, DateTime? to = null,
                bool settled = false, string paymentPage = null);
        }

        // GET: PaymentTypes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentType paymentType = await _db.PaymentTypes.FindAsync(id);
            if (paymentType == null)
            {
                return HttpNotFound();
            }
            return View(paymentType);
        }

        // GET: PaymentTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PaymentTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PaymentTypeId,PaymentName,Amount")] PaymentType paymentType)
        {
            if (ModelState.IsValid)
            {
                _db.PaymentTypes.Add(paymentType);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(paymentType);
        }

        // GET: PaymentTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentType paymentType = await _db.PaymentTypes.FindAsync(id);
            if (paymentType == null)
            {
                return HttpNotFound();
            }
            return View(paymentType);
        }

        // POST: PaymentTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PaymentTypeId,PaymentName,Amount")] PaymentType paymentType)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(paymentType).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(paymentType);
        }

        // GET: PaymentTypes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentType paymentType = await _db.PaymentTypes.FindAsync(id);
            if (paymentType == null)
            {
                return HttpNotFound();
            }
            return View(paymentType);
        }

        // POST: PaymentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PaymentType paymentType = await _db.PaymentTypes.FindAsync(id);
            _db.PaymentTypes.Remove(paymentType);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
