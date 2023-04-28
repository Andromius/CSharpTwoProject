using BusinessLayer.Services.FormValidationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Projekt.Validation
{
    public class ReservationFormValidation : IFormValidationService<ReservationForm>
    {
        public string Validate(ReservationForm form)
        {
            string ret = string.Empty;
            if (form.User == null)
            {
                ret += "Reservation must have a user assigned\n";
            }
            if (!form.Date.HasValue)
            {
                ret += "Reservation date must be filled in\n";
            }
            else if (form.Date.Value.Date <= DateTime.Today.Date)
            {
                ret += "Reservation date must be greater than today's date\n";
            }
            if (form.TimeSpan == null)
            {
                ret += "Reservation time must be filled in\n";
            }
            if (form.Service == null)
            {
                ret += "Service must be chosen\n";
            }
            return ret;
        }
    }
}
