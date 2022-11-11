using Azure.Storage.Blobs;
using DroneSurvey.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace DroneSurvey.Controllers
{
    public class ImageUploadController : Controller
    {
        private readonly string containerConnString = String.Empty;
        private readonly string containerName = String.Empty;
        public IConfiguration Configuration { get; set; }
        public ImageUploadController()
        {
            var builder = new ConfigurationBuilder()
           .AddJsonFile("appSettings.json");
            Configuration = builder.Build();
            containerConnString = Configuration["BlobStorageConnection:ContainerConnString"];
            containerName = Configuration["BlobStorageConnection:ContainerName"];
        }

        public IActionResult Upload()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            DAL dal = new();
            string? batchName = Guid.NewGuid().ToString("N");
            BlobContainerClient containerClient = new BlobContainerClient(containerConnString, containerName);
            string dirName = DateTime.Now.ToString("yyyyMMdd") + "/" + batchName + "/";
            if (dal.CreateBatch(batchName, dirName))
            {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        // full path to file in temp location
                        var filePath = Path.GetTempFileName();
                        var filePathOverCloude = dirName + file.FileName;
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                            stream.Position = 0;
                            containerClient.UploadBlob(filePathOverCloude, stream);
                            dal.CreateTransaction(batchName, file.FileName, filePathOverCloude);
                        }
                    }
                }
                dal.UpdateBatch(batchName);
            }
            return View();
        }

        /*[HttpPost]
        public IActionResult Upload(ImageUpload imageUpload)
        {
            DAL dal = new();
            folderPath = string.IsNullOrEmpty(imageUpload.DirPath) ? folderPath : imageUpload.DirPath;
            string? batchName = Guid.NewGuid().ToString("N");

            var files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);
            BlobContainerClient containerClient = new BlobContainerClient(containerConnString, containerName);
            string dirName = DateTime.Now.ToString("yyyyMMdd") + "/" + batchName + "/";
            if (dal.CreateBatch(batchName, dirName))
            {
                foreach (var file in files)
                {
                    var filePathOverCloude = dirName + Path.GetFileName(file);//file.Replace(folderPath, String.Empty);

                    using (MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(file)))
                    {
                        containerClient.UploadBlob(filePathOverCloude, stream);
                        dal.CreateTransaction(batchName, Path.GetFileName(file), filePathOverCloude);
                    }
                }
                dal.UpdateBatch(batchName);
            }
            return View();

        } */

        [HttpGet]
        public async Task<IActionResult> GetImageUploadHistory()
        {
            DAL dal = new();
            List<ImageUploadHistory>? imageUploadHistory = new();
            imageUploadHistory = dal.GetImageUploadHistory("");
            return Json(new { data = imageUploadHistory });
        }

        public PartialViewResult ViewImageModelPopup()
        {
            VieImageModel vieImageModel = new VieImageModel();
            return PartialView("_ViewImage", vieImageModel);
        }
    }
}
