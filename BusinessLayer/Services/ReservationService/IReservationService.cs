using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.ReservationService
{
	public interface IReservationService
	{
		public Task Reserve(User u, long serviceId, string date, string timeSpan);
	}
}
