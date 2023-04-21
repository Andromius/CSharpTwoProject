using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
	[DbEntity]
	public class Reservation : DomainObject
	{
		[DbForeignKey]
		[DbAttr("user")]
		public User Reservee { get; set; }
		
		[DbAttr("date_time_reservation")]
		public DateTime ReservationTime { get; set; }

		[DbForeignKey]
		[DbAttr("service")]
		public Service Service { get; set; }

		[DbConstructor]
		public Reservation(long id, string name, string surname, DateTime reservationTime, long serviceId) : base(id)
        {
			Reservee = new User(name, surname);
			ReservationTime = reservationTime;
			Service = new Service(serviceId);
        }
		public Reservation(User reservee, DateTime reservationTime, Service service) : base()
		{
			Reservee = reservee;
			ReservationTime = reservationTime;
			Service = service;
		}
		public override string ToString()
		{
			return $"{Id} {Reservee.Name} {Reservee.Surname} {ReservationTime} {Service.Id}";
		}

	}
}
