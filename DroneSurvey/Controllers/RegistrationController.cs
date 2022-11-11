using DroneSurvey.Models;
using Microsoft.AspNetCore.Mvc;

namespace DroneSurvey.Controllers
{
    public class RegistrationController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(Register register)
        {
            return View();
        }
    }
}
