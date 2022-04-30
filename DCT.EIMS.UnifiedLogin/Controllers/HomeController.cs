using DCT.EIMS.UnifiedLogin.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DCT.EIMS.UnifiedLogin.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Start()
        {
            return new OkObjectResult(new { Message = "The SuiteLogin Strated" });
        } 

        [Authorize]
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
            /*
             * https://stackoverflow.com/questions/32974289/mvc-4-redirecttoaction-does-not-see-custom-header
            https://social.msdn.microsoft.com/Forums/en-US/3c1f6144-606f-49c8-acfd-896d80ad8529/set-custom-request-header-and-retrieve-the-same-value-in-the-response?forum=aspstatemanagement
            https://stackoverflow.com/questions/18026050/mvc-redirect-with-custom-headers
            Response.Headers.Add("id_token",idToken);
            Response.Headers.Add("access_token", accessToken);
            Response.Headers.Add("refresh_token", refreshToken);
            Response.Redirect($"https://localhost:7013/Home/Callback?idtoken={idToken}&refresh_token={refreshToken}");
            */
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