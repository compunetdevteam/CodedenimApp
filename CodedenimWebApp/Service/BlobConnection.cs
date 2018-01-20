using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CodedenimWebApp.Controllers
{
    public partial class BlobConnection
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string accountName = "compunet";
            string accessKey = "Y97anRU5wIvoVHDRw8X5Tzm49u6Zx2/lsYP60JPKLNCk2gdrPF5wzyPlTkvllWSjPBRDrepRZzgtelEoB/jHtw==";

            try
            {
                StorageCredentials creden = new StorageCredentials(accountName, accessKey);
                CloudStorageAccount account = new CloudStorageAccount(creden, useHttps: true);
                CloudBlobClient client = account.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference("mysample");
                container.CreateIfNotExists();
                container.SetPermissions(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
                CloudBlockBlob blob = container.GetBlockBlobReference("04.XMLSchemas.pdf");
                using (Stream file = System.IO.File.OpenRead(@"C:\Users\DEV-PC\Documents\David\ASP"))
                {
                    blob.UploadFromStream(file);
                }
            }
            catch
            {

            }
        }
    }
}