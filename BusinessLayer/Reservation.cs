using BusinessLayer.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BusinessLayer
{
	[DbEntity]
	[DataContract]
	public class Reservation : DomainObject
	{
		[DbForeignKey]
		[JsonInclude]
		[DataMember]
		[DbAttr("user")]
		public User Reservee { private get; set; }
		
		[DbAttr("reservation_start")]
		[DataMember]
		public DateTime ReservationStart { get; set; }
		[DbAttr("reservation_end")]
		[DataMember]
		public DateTime ReservationEnd { get; set; }

		[DbForeignKey]
		[JsonInclude]
		[DataMember]
		[DbAttr("service")]
		public Service Service { private get; set; }

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
				Reservee = await dataMapper.SelectWithCondition(new Dictionary<string, object>() { { "id", Reservee.Id.Value } });
			return Reservee;
		}
		public async Task<Service> GetService(IDataMappingService<Service> dataMapper)
		{
			if (Service.IsPartial)
				Service = await dataMapper.SelectWithCondition(new Dictionary<string, object>() { { "id", Service.Id.Value } });
			return Service;
		}

	}
}
