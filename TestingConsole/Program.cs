using BusinessLayer;
using DataLayer;
using System.Reflection;

namespace TestingConsole
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			if (!File.Exists("database.db"))
			{
				DB.CreateTables();
			}
			DB.CreateCreateCommand(new System.Data.SQLite.SQLiteConnection());
			Service service = new Service(2, "Strih");
			Reservation reservation = await new DataMapper<Reservation>().SelectByID(5);
			//reservation.ReservationTime = DateTime.Now;
			Console.WriteLine(reservation);
			//DataMapper<Reservation> dm = new DataMapper<Reservation>();
			//DataMapper<Service> dms = new DataMapper<Service>();
			//Console.WriteLine(await dms.Insert(service));
			//Console.WriteLine(await dm.Insert(reservation));
			//Console.WriteLine(await dm.Update(reservation));
		}
	}
}