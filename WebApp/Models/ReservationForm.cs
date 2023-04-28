using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebApp.Models
{
    public class ReservationForm
    {
        [Display(Name = "Reservation date")]
        public string ReservationDate { get; set; }
        [Display(Name = "Reservation time")]
        public string ReservationTime { get; set; }
        [Display(Name = "Service")]
        public string ServiceID { get; set; }
    }
}
