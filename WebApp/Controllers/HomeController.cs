using BusinessLayer;
using BusinessLayer.Services;
using BusinessLayer.Services.FormValidationService;
using BusinessLayer.Services.ReservationService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using WebApp.Models;

namespace WebApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IReservationService _reservationService;
		private readonly IFormValidationService<ReservationForm> _formValidationService;
		private readonly IDataMappingService<Reservation> _dataMappingService;

        public HomeController(ILogger<HomeController> logger, IReservationService reservationService, IFormValidationService<ReservationForm> formValidationService, IDataMappingService<Reservation> dataMappingService)
        {
            _logger = logger;
            _reservationService = reservationService;
            _formValidationService = formValidationService;
            _dataMappingService = dataMappingService;
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
			List<Reservation> reservations = await _dataMappingService.SelectAll(new Dictionary<string, object>() { { "user_id", u!.Id!.Value } });
			ViewBag.List = reservations.OrderBy(x => x.ReservationStart).ToList();
			return View();
		}

		public async Task<IActionResult> Reserve() 
		{
			if (HttpContext.Session.GetString("user") is null)
			{
				return Unauthorized("You must be logged in to view this page!");
			}
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Reserve(ReservationForm form)
		{
			string validationMessage = _formValidationService.Validate(form);
            if (!string.IsNullOrEmpty(validationMessage))
			{
				ViewBag.ValidationMessage = validationMessage.Split('\n');
				return View();
			}

			await using (MemoryStream memoryStream = new MemoryStream(HttpContext.Session.Get("user")))
			{
				User u = await JsonSerializer.DeserializeAsync<BusinessLayer.User>(memoryStream);
				await _reservationService.Reserve(u, Convert.ToInt64(form.ServiceID), form.ReservationDate, form.ReservationTime);
			}

			return RedirectToAction("Index");
		}

        public async Task<IActionResult> Remove(long id)
        {
            await using (MemoryStream memoryStream = new MemoryStream(HttpContext.Session.Get("user")))
            {
                User u = await JsonSerializer.DeserializeAsync<BusinessLayer.User>(memoryStream);
                List<Reservation> reservations = await _dataMappingService.SelectAll(new Dictionary<string, object>() { { "user_id", u!.Id!.Value } });
				await _dataMappingService.Delete(reservations.Find(x => x.Id.Value == id));
            }
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}