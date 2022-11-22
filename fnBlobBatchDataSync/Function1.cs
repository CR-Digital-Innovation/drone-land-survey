using System;
using System.IO;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace fnBlobBatchDataSync
{
    public class Function1
    {
        private int? segmentSize;

        [FunctionName("Function1")]
        public void Run([BlobTrigger("vendorcontainersync1/{name}", Connection = "BlobConnString")] Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
            if (name.EndsWith(".eof"))
            {
                EmailTrigger("Upload Started");
                ListBlobsFlatListing();

            }
        }

        private async void ListBlobsFlatListing() // BlobContainerClient blobContainerClient,int? segmentSize
        {
            try
            {
                DAL dal = new();
                string? batchName = Guid.NewGuid().ToString("N");
                string dirName = DateTime.Now.ToString("yyyyMMdd") + "/" + batchName + "/";
                if (dal.CreateBatch(batchName, dirName))
                {
                    segmentSize = 10;
                    BlobContainerClient containerClient = new BlobContainerClient("", "vendorcontainersync1");

                    // Call the listing operation and return pages of the specified size.
                    var resultSegment = containerClient.GetBlobsAsync()
                        .AsPages(default, segmentSize);

                    // Enumerate the blobs returned for each page.
                    await foreach (Azure.Page<BlobItem> blobPage in resultSegment)
                    {
                        foreach (BlobItem blobItem in blobPage.Values)
                        {
                            Console.WriteLine("Found Blob name: {0}", blobItem.Name);
                            Console.WriteLine("Moving Blob name: {0}", blobItem.Name);
                            MoveFiles(blobItem.Name, dirName + blobItem.Name);
                            dal.CreateTransaction(batchName, blobItem.Name, dirName + blobItem.Name);
                        }

                        Console.WriteLine();
                    }
                    dal.UpdateBatch(batchName);
                    EmailTrigger("Upload Completed");
                }
            }
            catch (Exception)
            {

            }
            //catch (RequestFailedException e)
            //{
            //    Console.WriteLine(e.Message);
            //    Console.ReadLine();
            //    throw;
            //}
        }

        private static void MoveFiles(string srcBlobName, string destBlobName)
        {
            string accountName = "";
            string storageKey = "";
            string sourceContainerName = "";
            string destContainerName = "";

            var cred = new StorageCredentials(accountName, storageKey);
            var account = new CloudStorageAccount(cred, true);
            var client = account.CreateCloudBlobClient();

            var sourceContainer = client.GetContainerReference(sourceContainerName);
            var sourceBlob = sourceContainer.GetBlockBlobReference(srcBlobName);

            var destinationContainer = client.GetContainerReference(destContainerName);
            var destinationBlob = destinationContainer.GetBlockBlobReference(destBlobName);

            destinationBlob.StartCopy(sourceBlob);
            sourceBlob.Delete(Microsoft.Azure.Storage.Blob.DeleteSnapshotsOption.IncludeSnapshots);

        }


        private void EmailTrigger(string type)
        {
            EmailContent emailContent = new();
            string body = string.Empty;
            switch (type)
            {
                case "Upload Started":
                    body = @"<html>
                                <body>
                                      <p>Hi,</p>
                                      <p>Images are being transferred to a cloud container using Cloud Synchronization  service.</p>
                                      <p>This is an auto generated email. </p>
                                      <p>Regards,</p>
                                      <p style=""font-family: 'Trebuchet MS', 'Lucida Sans Unicode', 'Lucida Grande', 'Lucida Sans', Arial,sans-serif;font-weight: bold;""> AI/ML DevSecOps Team </p>
                                      <p><span>@</span><span style=""color:#ff6801;"">Cr</span><span style=""color:#013E7D;"">it</span><span style=""color:#ff6801;"">cal</span><span style=""color:#013E7D;"">River Inc 2022</span></p>
                               </body>
                            </html>";
                    break;
                case "Upload Completed":
                    body = @"<html>
                                <body>
                                      <p>Hi,</p>
                                      <p>Cloud Synchronization is completed. Initializing Orthomosaic image creation.</p>
                                      <p>This is an auto generated email. </p>
                                      <p>Regards,</p>
                                      <p style=""font-family: 'Trebuchet MS', 'Lucida Sans Unicode', 'Lucida Grande', 'Lucida Sans', Arial,sans-serif;font-weight: bold;""> AI/ML DevSecOps Team </p>
                                      <p><span>@</span><span style=""color:#ff6801;"">Cr</span><span style=""color:#013E7D;"">it</span><span style=""color:#ff6801;"">cal</span><span style=""color:#013E7D;"">River Inc 2022</span></p>
                               </body>
                            </html>";
                    break;
                default: break;
            }

            try
            {
                emailContent.ToAddress = "";
                emailContent.Subject = "Drone Survey Notification";
                emailContent.Body = body;

                EmailNotification.SendEmail(emailContent);
            }
            catch (Exception)
            {

            }
        }

        // Working 
        //public void Run([BlobTrigger("vendorcontainersync1/{name}", Connection = "BlobConnString")] Stream myBlob, string name, ILogger log)
        //{
        //    log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        //}

        // Default 
        //public void Run([BlobTrigger("samples-workitems/{name}", Connection = "appsetting.json")]Stream myBlob, string name, ILogger log)
        //{
        //    log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        //}
    }
}
