using DroneSurvey.Models;
using Microsoft.AspNetCore.Mvc;

namespace DroneSurvey.Controllers
{
    public class ArcGISController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DisplayMyLocation()
        {
            return View();
        }

        public IActionResult AddressSearch()
        {
            return View();
        }

        public IActionResult CreatePolygon()
        {
            return View();
        }

        public IActionResult DisplayORIPolygon()
        {
            return View();
        }

        public IActionResult GeoServer()
        {
            return View();
        }

        public IActionResult ArcGISIFrame()
        {
            return View();
        }

        public IActionResult ArcGISFeatureLayer()
        {
            return View();
        }


    }
}
