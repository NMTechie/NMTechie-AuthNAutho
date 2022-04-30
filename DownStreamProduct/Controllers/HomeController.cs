using DownStreamProduct.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace DownStreamProduct.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return Redirect("https://localhost:7147/Home/Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Callback()
        {
            //https://stackoverflow.com/questions/32974289/mvc-4-redirecttoaction-does-not-see-custom-header
            //https://social.msdn.microsoft.com/Forums/en-US/3c1f6144-606f-49c8-acfd-896d80ad8529/set-custom-request-header-and-retrieve-the-same-value-in-the-response?forum=aspstatemanagement
            //https://stackoverflow.com/questions/18026050/mvc-redirect-with-custom-headers
            return new OkObjectResult(new { IdToken = HttpContext.Request.QueryString.Value });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}