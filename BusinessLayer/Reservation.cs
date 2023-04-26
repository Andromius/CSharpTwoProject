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
		
		[DbAttr("reservation_start")]
		public DateTime ReservationStart { get; set; }
		[DbAttr("reservation_end")]
		public DateTime ReservationEnd { get; set; }

		[DbForeignKey]
		[DbAttr("service")]
		public Service Service { get; set; }

		[DbConstructor]
		public Reservation(long userId, DateTime reservationStart, DateTime reservationEnd, long serviceId, long id) : base(id)
        {
			Reservee = new User(userId);
			ReservationStart = reservationStart;
			ReservationEnd = reservationEnd;
			Service = new Service(serviceId);
        }
		public Reservation(User reservee, DateTime reservationStart, DateTime reservationEnd, Service service)
		{
			Reservee = reservee;
			ReservationStart = reservationStart;
			ReservationEnd = reservationEnd;
			Service = service;
		}
		public async Task<User> GetReservee(IDataMappingService<User> dataMapper)
		{
			if (Reservee.IsPartial)
				Reservee = await dataMapper.SelectWithCondition(new Dictionary<string, object>() { { "user_id", Reservee.Id.Value } });
			return Reservee;
		}
		public async Task<Service> GetService(IDataMappingService<Service> dataMapper)
		{
			if (Service.IsPartial)
				Service = await dataMapper.SelectWithCondition(new Dictionary<string, object>() { { "service_id", Service.Id.Value } });
			return Service;
		}

	}
}
