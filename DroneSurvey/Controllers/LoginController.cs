using DroneSurvey.Models;
using Microsoft.AspNetCore.Mvc;

namespace DroneSurvey.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Validate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Validate(Login login)
        {
            Validate validate = new();
            ExecutionResponse executionResponse=validate.ValidateUser(login);
            if (executionResponse != null)
            {
                if (executionResponse.ExecutionStatus == ExecutionStatus.Success)
                {
                    return RedirectToAction("Home", "Dashboard");
                }
                else
                {
                    login.ErrorMsg = "Invalid Credentials";
                    return View(login);
                }
            }
            else
            {
                return View();
            }
        }
    }
}
