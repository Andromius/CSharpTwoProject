using BusinessLayer;
using System.Data.SQLite;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace DataLayer
{
	public class DB
	{
		//HairdresserDB
		private const string V =
			"CREATE TABLE Service " +
			"(" +
			"id INTEGER PRIMARY KEY AUTOINCREMENT," +
			"name VARCHAR(20)" +
			");" +
			"CREATE TABLE Reservation " +
			"(" +
			"id INTEGER PRIMARY KEY AUTOINCREMENT," +
			"name VARCHAR(20)," +
			"surname VARCHAR(20)," +
			"date_time_reservation DATETIME," +
			"service_id INTEGER," +
			"FOREIGN KEY (service_id) REFERENCES Service(id)" +
			");";
		private const string connString = "Data Source=database.db; Version = 3;";

		public static void CreateTables()
		{
			using (SQLiteConnection conn = new SQLiteConnection(connString))
			{
				conn.Open();
				using (SQLiteCommand cmd = CreateCreateCommand(conn))
				{
					cmd.ExecuteNonQuery();
				}
			}
		}

		public static SQLiteCommand CreateCreateCommand(SQLiteConnection conn)
		{
			SQLiteCommand command = new SQLiteCommand(conn);
			Assembly? bLayer = Assembly.GetAssembly(typeof(DomainObject));

			foreach (Type type in bLayer.GetTypes().Where(x => x.GetCustomAttribute(typeof(DbEntity)) is not null))
			{
				Console.WriteLine(type.FullName);
			}



			return command;
		}

		public static void DropTable(string table)
		{
			using (SQLiteConnection conn = new SQLiteConnection(connString))
			{
				conn.Open();
				using (SQLiteCommand cmd = new SQLiteCommand(conn))
				{
					cmd.CommandText = $"DROP TABLE {table}";
					cmd.ExecuteNonQuery();
				}
			}
		}
	}
}
