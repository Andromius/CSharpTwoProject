using BusinessLayer;
using BusinessLayer.Services;
using BusinessLayer.Services.AuthServices;
using BusinessLayer.Services.FormValidationService;
using BusinessLayer.Services.ReservationService;
using DataLayer;
using Microsoft.AspNet.Identity;
using WebApp.Models;

namespace WebApp
{
    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();
			builder.Services.AddSession();

			builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
			builder.Services.AddSingleton<IDataMappingService<User>, DataMapper<User>>();
			builder.Services.AddSingleton<IDataMappingService<Reservation>, DataMapper<Reservation>>();
			builder.Services.AddSingleton<IDataMappingService<Service>, DataMapper<Service>>();
			builder.Services.AddSingleton<IFormValidationService<ReservationForm>, ReservationValidationService>();
			builder.Services.AddSingleton<IFormValidationService<RegistrationForm>, RegistrationValidationService>();
			builder.Services.AddScoped<IReservationService, ReservationService>();
			builder.Services.AddScoped<IAccountService, AccountService>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseSession();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Login}/{action=Index}/{id?}");

			app.Run();
		}
	}
}