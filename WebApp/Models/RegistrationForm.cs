using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
	public class RegistrationForm
	{
		public string Name { get; set; }
		public string Surname { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		[Display(Name = "Confirm password")]
		public string PasswordConfirm { get; set; }
	}
}
