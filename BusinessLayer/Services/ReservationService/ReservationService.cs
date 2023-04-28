using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.ReservationService
{
	public class ReservationService : IReservationService
	{
		private IDataMappingService<Reservation> dataMappingService;
		public ReservationService(IDataMappingService<Reservation> dataMappingService)
		{
			this.dataMappingService = dataMappingService;
		}
		public async Task Reserve(User u, long serviceId, string date, string timeSpan)
		{
			Service s = new(serviceId);
			string[] times = timeSpan.Split('-');
			DateOnly dateOnly = DateOnly.Parse(date);
			times[0] = times[0].Trim();
			times[1] = times[1].Trim();
			DateTime dateStart = dateOnly.ToDateTime(TimeOnly.ParseExact(times[0], "HH:mm"));
			DateTime dateEnd = dateOnly.ToDateTime(TimeOnly.ParseExact(times[1], "HH:mm"));
			Reservation r = new Reservation(u, dateStart, dateEnd, s);

			await dataMappingService.Insert(r);
		}
	}
}
