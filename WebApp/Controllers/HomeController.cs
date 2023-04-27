using BusinessLayer;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using WebApp.Models;

namespace WebApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public async Task<IActionResult> Index()
		{
            if (HttpContext.Session.GetString("user") is null)
            {
                return Unauthorized("You must be logged in to view this page!");
            }

            User u;
            await using (MemoryStream memoryStream = new MemoryStream(HttpContext.Session.Get("user")))
            {
                u = await JsonSerializer.DeserializeAsync<BusinessLayer.User>(memoryStream);
            }
            ViewBag.List = await new DataLayer.DataMapper<Reservation>().SelectAll(new Dictionary<string, object>() { { "user_id", u!.Id!.Value} });
			return View();
		}

		public async Task<IActionResult> Reservation() 
		{
			if (HttpContext.Session.GetString("user") is null)
			{
				return Unauthorized("You must be logged in to view this page!");
			}

			User u;
			await using (MemoryStream memoryStream = new MemoryStream(HttpContext.Session.Get("user")))
			{
				u = await JsonSerializer.DeserializeAsync<BusinessLayer.User>(memoryStream);
			}
			ViewBag.User = u;
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}