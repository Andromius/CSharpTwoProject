using BusinessLayer.Services.FormValidationService;
using System.Text.RegularExpressions;
using WebApp.Models;

namespace WebApp
{
	public class RegistrationValidationService : IFormValidationService<RegistrationForm>
	{
		public string Validate(RegistrationForm form)
		{
			Regex emailRegex = new Regex(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])");
			string ret = string.Empty;
			if (form.Name == null)
			{
				ret += "Name must be filled in\n";
			}
			else if (form.Surname == null)
			{
				ret += "Surname must be filled in\n";
			}
			if (form.Password == null)
			{
				ret += "Password must be filled in\n";
			}
			if (form.Password != form.PasswordConfirm)
			{
				ret += "Passwords must match\n";
			}
			if (form.Email == null)
			{
				ret += "Email must be filled in\n";
			}
			else if (!emailRegex.IsMatch(form.Email))
			{
				ret += "Incorrect email format\n";
			}
			return ret;
		}
	}
}
