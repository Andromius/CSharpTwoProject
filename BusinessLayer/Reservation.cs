using BusinessLayer.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
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
		public Reservation(long userId, DateTime reservationTime, long serviceId, long id) : base(id)
        {
			Reservee = new User(userId);
			ReservationTime = reservationTime;
			Service = new Service(serviceId);
        }
		public Reservation(User reservee, DateTime reservationTime, Service service)
		{
			Reservee = reservee;
			ReservationTime = reservationTime;
			Service = service;
		}
		public override string ToString()
		{
			return $"{Id} {Reservee.Name} {Reservee.Surname} {ReservationTime} {Service.Id}";
		}

		public async Task<User> GetReservee(IDataMappingService<User> dataMapper)
		{
			if (Reservee.IsPartial)
				Reservee = await dataMapper.SelectByID(Reservee.Id.Value);
			return Reservee;
		}
		public async Task<Service> GetService(IDataMappingService<Service> dataMapper)
		{
			if (Service.IsPartial)
				Service = await dataMapper.SelectByID(Service.Id.Value);
			return Service;
		}

	}
}
