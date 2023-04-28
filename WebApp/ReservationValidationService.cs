using BusinessLayer.Services.FormValidationService;
using WebApp.Models;

namespace WebApp
{
	public class ReservationValidationService : IFormValidationService<ReservationForm>
	{
		public string Validate(ReservationForm form)
		{
			string ret = string.Empty;
			if(form.ReservationDate == null)
			{
				ret += "Reservation date must be filled in\n";
			}
			else if (DateTime.Parse(form.ReservationDate) <= DateTime.Today)
			{
                ret += "Reservation date must be greater than today's date\n";
            }
			if(form.ReservationTime == null)
			{
                ret += "Reservation time must be filled in\n";
            }
			if(form.ServiceID == null)
			{
                ret += "Service must be chosen\n";
            }
			return ret;
		}
	}
}
