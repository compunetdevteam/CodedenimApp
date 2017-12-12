using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CodedenimWebApp.Service
{
    public class BlobService
    {
        public CloudBlobContainer GetCloudBlobContainer()
        {
            string connString = "DefaultEndpointsProtocol=https;AccountName=compunet;AccountKey=Y97anRU5wIvoVHDRw8X5Tzm49u6Zx2/lsYP60JPKLNCk2gdrPF5wzyPlTkvllWSjPBRDrepRZzgtelEoB/jHtw==;EndpointSuffix=core.windows.net";
            string destContainer = "codedenim";

            // Get a reference to the storage account  
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(destContainer);
            if (blobContainer.CreateIfNotExists())
            {
                blobContainer.SetPermissions(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });

            }
            return blobContainer;
        }

        public static string  BlobUri = "https://compunet.blob.core.windows.net/codedenim";
    }
}