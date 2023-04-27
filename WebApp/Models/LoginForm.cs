using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebApp.Models
{
	public class LoginForm
	{
		[Display(Name = "E-mail")]
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
	}
}
