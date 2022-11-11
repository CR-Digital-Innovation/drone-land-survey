using DroneSurvey.Models;
using Microsoft.AspNetCore.Mvc;

namespace DroneSurvey.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }

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
            return PartialView("_ViewImageQC", vieImageModel);
        }
    }
}
