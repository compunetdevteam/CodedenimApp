namespace CodedenimWebApp.Service
{
    public static class RemitaConfigParams
    {
        //public const string MERCHANTID = "2547916";
        //public const string SERVICETYPEID = "4430731";



        //public const string APIKEY = "1946";


        public const string MERCHANTID = "2587711795";
        public const string SERVICETYPEID = "2587615591";



        public const string APIKEY = "245183";
        //public const string GATEWAYURL = "http://www.remitademo.net/remita/ecomm/init.reg";
        //public const string CHECKSTATUSURL = "http://www.remitademo.net/remita/ecomm";
        public const string GATEWAYURL = "https://login.remita.net/remita/ecomm/init.reg";
        public const string CHECKSTATUSURL = "https://login.remita.net/remita/ecomm";
    }

    public class RemitaRePostVm
    {
        public string merchantId { get; set; }
        public string hash { get; set; }
        public string rrr { get; set; }
        public string responseurl { get; set; }
    }
}