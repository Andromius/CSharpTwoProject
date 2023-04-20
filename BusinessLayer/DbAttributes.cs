namespace BusinessLayer
{
	public class DbAttr : Attribute
	{
		private string dbname;
		public DbAttr(string dbname)
		{
			this.dbname = dbname;
		}
	}

	public class DbIgnore : Attribute { }
	public class DbInner : Attribute { }
	public class DbForeignKey : Attribute { }
	public class DbEntity : Attribute { }
	public class DbConstructor : Attribute { }
}
