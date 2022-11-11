using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;

namespace DroneSurvey.Controllers
{
    public class FileUploadController : Controller
    {
        static string containerConnString = "DefaultEndpointsProtocol=https;AccountName=droneimagestorage;AccountKey=B2IITgBKK5vG1HsQBxSC5l77wHLK9AEQfjoljZ1ZnoRjIBZGv8WXa+kK05Jl5Tq1niJYgeD+totM+AStGhF8HA==;EndpointSuffix=core.windows.net";
        static string containerName = "droneimagecontainer";
        public IActionResult ImageUpload()
        {
            return View();
        }
            [HttpPost("FileUpload")]
        public async Task<IActionResult> ImageUpload(List<IFormFile> files)
        {
            string? batchName = Guid.NewGuid().ToString("N");
            BlobContainerClient containerClient = new BlobContainerClient(containerConnString, containerName);
            string dirName = DateTime.Now.ToString("yyyyMMdd") + "/" + batchName + "/";

            long size = files.Sum(f => f.Length);

            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    // full path to file in temp location
                    var filePath = Path.GetTempFileName(); //we are using Temp file name just for the example. Add your own file path.

                    var filePathOverCloude = dirName + formFile.FileName; //Path.GetFileName(filePath);

                    filePaths.Add(filePath);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                        stream.Position = 0;
                        containerClient.UploadBlob(filePathOverCloude, stream);
                    }
                }
            }
            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            return Ok(new { count = files.Count, size, filePaths });

            /*long size = files.Sum(f => f.Length);

            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    // full path to file in temp location
                    var filePath = Path.GetTempFileName(); //we are using Temp file name just for the example. Add your own file path.
                    filePaths.Add(filePath);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);

                    }
                }
            }
            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            return Ok(new { count = files.Count, size, filePaths }); */
        }
    }
}
