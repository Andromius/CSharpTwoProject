using BusinessLayer;
using BusinessLayer.Services;
using BusinessLayer.Services.AuthServices;
using DataLayer;
using Microsoft.AspNet.Identity;

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