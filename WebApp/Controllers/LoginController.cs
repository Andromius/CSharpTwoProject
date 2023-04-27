using BusinessLayer.Services.AuthServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using WebApp.Models;

namespace WebApp.Controllers
{
	public class LoginController : Controller
	{
		private IAccountService _accountService;
		public LoginController(IAccountService accountService)
		{
			_accountService = accountService;
		}

		public IActionResult Index()
		{
			if(HttpContext.Session.Get("user") is not null)
			{
				HttpContext.Session.Remove("user");
			}
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Index(LoginForm form)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			BusinessLayer.User u = await _accountService.Login(form.Email, form.Password);
			if (u != null)
			{
				await using (MemoryStream stream = new MemoryStream())
				{
					await JsonSerializer.SerializeAsync(stream, u);
					HttpContext.Session.Set("user", stream.ToArray());
					return RedirectToAction("Reservation", "Home");
				}
			}
			ViewBag.Message = "Invalid credentials";
			return View();
		}
	}
}
