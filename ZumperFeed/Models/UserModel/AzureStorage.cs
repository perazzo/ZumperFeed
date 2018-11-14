using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ZumperFeed.Models.UserModel
{
    public class AzureStorage
    {
        public CloudBlockBlob GetAzureBlob(string containername, string filename)
        {
            var creds = ConfigurationManager.AppSettings["irent_AzureStorageConnectionString"];
            var azureStorage = CloudStorageAccount.Parse(creds);
            var client = azureStorage.CreateCloudBlobClient();
            //create a blob container and make it publicly accessibile
            var baseContainer = client.GetContainerReference(containername);
            baseContainer.CreateIfNotExists();
            baseContainer.SetPermissions(new BlobContainerPermissions()
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });
            var blob = baseContainer.GetBlockBlobReference(filename);
            return blob;
        }
    }
}