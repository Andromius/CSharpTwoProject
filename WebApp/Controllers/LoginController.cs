using BusinessLayer;
using BusinessLayer.Services.AuthServices;
using BusinessLayer.Services.FormValidationService;
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
		private IFormValidationService<RegistrationForm> _formValidationService;

		public LoginController(IAccountService accountService, IFormValidationService<RegistrationForm> formValidationService)
		{
			_accountService = accountService;
			_formValidationService = formValidationService;
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
					return RedirectToAction("Index", "Home");
				}
			}
			ViewBag.Message = "Invalid credentials";
			return View();
		}

		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegistrationForm form)
		{
			string validationMessage = _formValidationService.Validate(form);
			if (!string.IsNullOrEmpty(validationMessage))
			{
				ViewBag.ValidationMessage = validationMessage.Split('\n');
				return View();
			}

			if(!await _accountService.Register(form.Name, form.Surname, form.Email, form.Password))
			{
				validationMessage += "Could not register this user\n";
				ViewBag.ValidationMessage = validationMessage.Split('\n');
				return View();
			}

			return RedirectToAction("Index");
		}
	}
}
