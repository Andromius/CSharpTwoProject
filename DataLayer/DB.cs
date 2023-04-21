using BusinessLayer;
using System.Data.SQLite;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DataLayer
{
	public class DB
	{
		//HairdresserDB
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
			Assembly? businessLayer = Assembly.GetAssembly(typeof(DomainObject));
			StringBuilder sb = new StringBuilder();
			List<Type> types = GetDbEntities(businessLayer!);

			foreach (Type type in types)
			{
				Console.WriteLine(type.Name);
				sb.Append("CREATE TABLE IF NOT EXISTS ").Append(type.Name).AppendLine("(");
				foreach (PropertyInfo prop in type.GetProperties())
                {
					List<CustomAttributeData> attributes = prop.CustomAttributes.ToList();
					string name = string.Empty;
					if ( attributes.Exists(x => x.AttributeType == typeof(DbIgnore)) ) { continue; }
					else if ( attributes.Exists(x => x.AttributeType == typeof(DbPrimaryKey)) )
					{
						name = attributes.Find(x => x.AttributeType == typeof(DbAttr)).ConstructorArguments[0].Value.ToString();
						sb.Append(name).Append(' ').Append(GetDbTypeName(prop)).AppendLine(" PRIMARY KEY,");
					}
					else if ( attributes.Exists(x => x.AttributeType == typeof(DbForeignKey)) )
					{
						name = attributes.Find(x => x.AttributeType == typeof(DbAttr)).ConstructorArguments[0].Value.ToString();
						PropertyInfo fkProp = prop.PropertyType.GetRuntimeProperties().Where(x => x.GetCustomAttribute(typeof(DbPrimaryKey)) is not null).FirstOrDefault();
						sb.Append(name).Append("_id").Append(' ').Append(GetDbTypeName(fkProp)).Append(" REFERENCES ").Append(name).AppendLine(",");
					}
					else
					{
						name = attributes.Find(x => x.AttributeType == typeof(DbAttr)).ConstructorArguments[0].Value.ToString();
						sb.Append(name).Append(' ').Append(GetDbTypeName(prop)).AppendLine(",");
					}
				}
				sb.Replace("\r\n", "\n");
				sb.Remove(sb.Length - 2, 1);
				sb.AppendLine(");");
            }
			Console.WriteLine(sb.ToString());
			command.CommandText = sb.ToString();

			return command;
		}


		private static List<Type> GetDbEntities(Assembly assembly)
		{
			List<Type> types = assembly.GetTypes()
							.Where(t => t.GetCustomAttribute(typeof(DbEntity)) is not null
									&& !t.GetRuntimeProperties()
										.Where(p => p.GetCustomAttributes(typeof(DbForeignKey)).Any()).Any())
							.ToList();

			types.AddRange(assembly.GetTypes()
								.Where(t => t.GetCustomAttribute(typeof(DbEntity)) is not null
										&& t.GetRuntimeProperties()
											.Where(p => p.GetCustomAttributes(typeof(DbForeignKey)).Any()).Any()));
			return types;
		}

		private static string GetDbTypeName(PropertyInfo prop)
		{
			Type type = prop.PropertyType;
			if(type.IsGenericType)
				type = type.GetGenericArguments()[0];

			if (type == typeof(long)) { return "INTEGER"; }
			else if (type == typeof(int)) { return "INT"; }
			else if (type == typeof(DateTime)) { return "DATETIME"; }
			else if (type == typeof(string)) { return "VARCHAR(20)"; }
			return "TEXT";
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
