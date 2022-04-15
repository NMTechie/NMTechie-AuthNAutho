using DCT.EIMS.UnifiedLogin.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DCT.EIMS.UnifiedLogin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string idToken=string.Empty;
            string accessToken= string.Empty;
            string refreshToken= string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                idToken = HttpContext.GetTokenAsync("id_token").Result;
                accessToken =  HttpContext.GetTokenAsync("access_token").Result;                
                refreshToken = HttpContext.GetTokenAsync("refresh_token").Result;
            }
            return new OkObjectResult(new { IdToken = idToken, AccessToken = accessToken, RefreshToken = refreshToken });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}