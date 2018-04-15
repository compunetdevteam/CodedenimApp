using CodedenimWebApp.Models;
using CodeninModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace CodedenimWebApp.Service
{
    public class GetDataPayPal
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        public string GetPayPalResponse(string tx)
        {

            try
            {
                string authToken = WebConfigurationManager.AppSettings["AccessToken"];
                string txToken = tx;
                string query = string.Format("cmd=_notify-synch&tx={0}&at={1}", txToken, authToken);
                string url = WebConfigurationManager.AppSettings["PaypalSubmitURL"];
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = query.Length;
                StreamWriter outStringWriter = new StreamWriter(req.GetRequestStream(), Encoding.ASCII);
                outStringWriter.Write(query);
                outStringWriter.Close();
                StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream());
                string strResponse = reader.ReadToEnd();
                reader.Close();
                if (strResponse.StartsWith("SUCCESS"))
                    return strResponse;
                else
                    return String.Empty;



            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
        public StudentPaypalPayment InformationOrder(string data)
        {
            string key, value;
            var order = new StudentPaypalPayment();
            try
            {
                string[] strArray = data.Split('\n');
                for (int i = 1; i < strArray.Length - 1; i++)
                {
                    string[] strArrayTemp = strArray[i].Split('=');
                    key = strArrayTemp[0];
                    value = HttpUtility.UrlDecode(strArrayTemp[1]);
                    switch (key)
                    {
                        case "payment_status":
                            order.PaymentStatus = value;
                            break;
                        case "first_name":
                            order.PayerFirstName = value;
                            break;
                        case "last_name":
                            order.PayerLastName = value;
                            break;
                        case "mc_gross":
                            order.Amount = value;
                            break;
                        case "payer_email":
                            order.PayerEmail = value;
                            break;
                        case "tx_token":
                            order.TxToken = value;
                            break;
                        case "receiver_email":
                            order.ReceiverEmail = value;
                            break;
                        case "mc_currency":
                            order.Currency = value;
                            break;
                        case "item_name":
                            order.ItemName = value;
                            break;
                        case "payment_date":
                            order.PaymentDate = value;
                            break;
                        case "item_number":
                            order.CourseCategoryId = 1;
                            break;
                        case "payment_id":
                            order.PayerId = value;
                            break;
                        case "custom":
                            order.StudentId = value;
                            break;
                    }
                }
                db.StudentPaypalPayments.Add(order);
                db.SaveChanges();
            }
            catch (Exception)
            {

                throw;

            }
            return order;
        }
    }
}