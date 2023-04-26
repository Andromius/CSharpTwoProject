using BusinessLayer;
using BusinessLayer.Services;
using System.Data.SQLite;
using System.Reflection;
using System.Text;

namespace DataLayer
{
	public class DataMapper<T> : IDataMappingService<T> where T : DomainObject
	{
		private readonly string connString = $"Data Source={System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\HairdresserDB.db; Version = 3;";
		private readonly string SQL_DELETE = $"DELETE FROM {typeof(T).Name} WHERE id = @id;";	
		public async Task<T?> SelectWithCondition(Dictionary<string, object> conditionParameters)
		{
			T? obj = null;
			ConstructorInfo? constructor = typeof(T).GetConstructors()
								.Where(x => x.GetCustomAttribute(typeof(DbConstructor)) is not null)
								.FirstOrDefault();
			
			if (constructor == null)
			{
				return obj;
			}

			object[] parameters = new object[constructor.GetParameters().Length];

			await using (SQLiteConnection conn = new SQLiteConnection(connString))
			{
				await conn.OpenAsync();
				await using (SQLiteCommand cmd = CreateSelectCommand(conn, conditionParameters))
				{
					await using (SQLiteDataReader reader = cmd.ExecuteReader())
					{
						while (await reader.ReadAsync())
						{
							reader.GetValues(parameters);
							obj = (T)constructor.Invoke(parameters);
						}
					}
				}
			}
			return obj;
		}

		public async Task<List<T>> SelectAll()
		{
			List<T> objs = new();
			ConstructorInfo? constructor = typeof(T).GetConstructors()
								.Where(x => x.GetCustomAttribute(typeof(DbConstructor)) is not null)
								.FirstOrDefault();

			if (constructor == null)
			{
				return objs;
			}

			object[] parameters = new object[constructor.GetParameters().Length];

			await using (SQLiteConnection conn = new SQLiteConnection(connString))
			{
				await conn.OpenAsync();
				await using (SQLiteCommand cmd = CreateSelectCommand(conn))
				{
					await using (SQLiteDataReader reader = cmd.ExecuteReader())
					{
						while (await reader.ReadAsync())
						{
							reader.GetValues(parameters);
							objs.Add((T)constructor.Invoke(parameters));
						}
					}
				}
			}
			return objs;
		}

		public async Task<bool> Insert(T obj)
		{
			int rowsAffected = 0;
			await using (SQLiteConnection conn = new SQLiteConnection(connString))
			{
				await conn.OpenAsync();
				await using (SQLiteCommand cmd = CreateInsertCommand(obj, conn))
				{
					rowsAffected = await cmd.ExecuteNonQueryAsync();
				}
			}
			return rowsAffected > 0;
		}

		public async Task<int> Delete(T obj)
		{
			int rowsAffected = 0;
			await using (SQLiteConnection conn = new SQLiteConnection(connString))
			{
				await conn.OpenAsync();
				await using (SQLiteCommand cmd = CreateDeleteCommand(obj.Id!.Value, conn))
				{
					rowsAffected = await cmd.ExecuteNonQueryAsync();
				}
			}
			return rowsAffected;
		}

		public async Task<int> Update(T obj)
		{
			int rowsAffected = 0;
			await using (SQLiteConnection conn = new SQLiteConnection(connString))
			{
				await conn.OpenAsync();
				await using (SQLiteCommand cmd = CreateUpdateCommand(obj, conn))
				{
					rowsAffected = await cmd.ExecuteNonQueryAsync();
				}
			}
			return rowsAffected;
		}
		private SQLiteCommand CreateSelectCommand(SQLiteConnection conn, Dictionary<string, object>? conditionParameters = null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append($"SELECT * FROM {typeof(T).Name}");
			SQLiteCommand cmd = new SQLiteCommand(conn);
			if (conditionParameters is not null)
			{
				stringBuilder.Append(" WHERE ");
				foreach (KeyValuePair<string, object> item in conditionParameters)
				{
					stringBuilder.Append(item.Key).Append(" = @").Append(item.Key).Append(" AND ");
					cmd.Parameters.AddWithValue($"@{item.Key}", item.Value);
				}
				stringBuilder.Remove(stringBuilder.Length - 5, 5);
			}
			stringBuilder.Append(';');
			cmd.CommandText = stringBuilder.ToString();
			Console.WriteLine(cmd.CommandText);
			return cmd;
		}
		private SQLiteCommand CreateDeleteCommand(long id, SQLiteConnection conn)
		{
			SQLiteCommand cmd = new SQLiteCommand(conn);
			cmd.CommandText = SQL_DELETE;
			cmd.Parameters.AddWithValue("@id", id);
			return cmd;
		}
		private SQLiteCommand CreateInsertCommand(T obj, SQLiteConnection conn)
		{
			SQLiteCommand cmd = new SQLiteCommand(conn);
			StringBuilder metaSb = new StringBuilder();
			StringBuilder sb = new StringBuilder();
			PropertyInfo[] entityProps = obj.GetType().GetRuntimeProperties().ToArray();
			metaSb.Append("PRAGMA foreign_keys = YES; INSERT INTO ").Append(typeof(T).Name).Append(" (");
			sb.Append('(');
			foreach (PropertyInfo entityProp in entityProps)
			{
				var dbAttribute = entityProp.CustomAttributes.First();
				string dbAttrName;
				if (dbAttribute.AttributeType == typeof(DbIgnore) || dbAttribute.AttributeType == typeof(DbPrimaryKey)) continue;
				else if (dbAttribute.AttributeType == typeof(DbForeignKey))
				{
					var innerObj = entityProp.GetValue(obj);
					PropertyInfo property = innerObj!.GetType()
														.GetRuntimeProperties()
														.Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(DbPrimaryKey)))
														.FirstOrDefault()!;

					dbAttrName = property.CustomAttributes.Last().ConstructorArguments[0].Value!.ToString()!;
					metaSb.Append(entityProp.CustomAttributes.Last().ConstructorArguments[0].Value)
						.Append('_')
						.Append(dbAttrName)
						.Append(',');
					sb.Append('@')
						.Append(entityProp.CustomAttributes.Last().ConstructorArguments[0].Value)
						.Append('_')
						.Append(dbAttrName)
						.Append(',');
					cmd.Parameters.AddWithValue($"@{entityProp.CustomAttributes.Last().ConstructorArguments[0].Value}_{dbAttrName}", property.GetValue(innerObj));
					continue;
				}
				dbAttrName = dbAttribute.ConstructorArguments[0].Value!.ToString()!;
				metaSb.Append(dbAttrName)
					.Append(',');
				sb.Append('@')
					.Append(dbAttrName)
					.Append(',');
				cmd.Parameters.AddWithValue($"@{dbAttrName}", entityProp.GetValue(obj));
			}
			metaSb.Remove(metaSb.Length - 1, 1)
				.Append(") VALUES ");
			sb.Remove(sb.Length - 1, 1)
				.Append(')')
				.Append(';');
			metaSb.Append(sb);
			cmd.CommandText = metaSb.ToString();
			return cmd;
		}
		private SQLiteCommand CreateUpdateCommand(T obj, SQLiteConnection conn)
		{
			SQLiteCommand cmd = new SQLiteCommand(conn);
			StringBuilder metaSb = new StringBuilder();
			PropertyInfo[] entityProps = obj.GetType().GetRuntimeProperties().ToArray();
			metaSb.Append("PRAGMA foreign_keys = YES;UPDATE ").AppendLine(typeof(T).Name).AppendLine("SET");
			foreach (PropertyInfo entityProp in entityProps)
			{
				var dbAttribute = entityProp.CustomAttributes.First();
				string dbAttrName;
				if (dbAttribute.AttributeType == typeof(DbIgnore) || dbAttribute.AttributeType == typeof(DbPrimaryKey)) continue;
				else if (dbAttribute.AttributeType == typeof(DbForeignKey))
				{
					var innerObj = entityProp.GetValue(obj);
					PropertyInfo property = innerObj!.GetType()
														.GetRuntimeProperties()
														.Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(DbPrimaryKey)))
														.FirstOrDefault()!;
					Console.WriteLine(property.Name);
					dbAttrName = property.CustomAttributes.Last().ConstructorArguments[0].Value!.ToString()!;
					metaSb.Append(entityProp.CustomAttributes.Last().ConstructorArguments[0].Value)
						.Append('_')
						.Append(dbAttrName)
						.Append(" = @")
						.Append(entityProp.CustomAttributes.Last().ConstructorArguments[0].Value)
						.Append('_')
						.Append(dbAttrName)
						.AppendLine(",");
					cmd.Parameters.AddWithValue($"@{entityProp.CustomAttributes.Last().ConstructorArguments[0].Value}_{dbAttrName}", property.GetValue(innerObj));
					continue;
				}
				dbAttrName = dbAttribute.ConstructorArguments[0].Value!.ToString()!;
				metaSb.Append(dbAttrName)
					.Append(" = @")
					.Append(dbAttrName)
					.AppendLine(",");
				cmd.Parameters.AddWithValue($"@{dbAttrName}", entityProp.GetValue(obj));
			}
			metaSb.Remove(metaSb.Length - 3, 1);
			metaSb.Append("WHERE id = @id;");
			cmd.Parameters.AddWithValue("@id", obj.Id!.Value);
			cmd.CommandText = metaSb.ToString();
			return cmd;
		}
	}
}
