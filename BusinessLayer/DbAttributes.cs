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
	public class DbPrimaryKey : Attribute 
	{
		private bool autoIncrement;
		public DbPrimaryKey(bool autoIncrement = true)
		{
			this.autoIncrement = autoIncrement;
		}
	}
	public class DbForeignKey : Attribute { }
	public class DbEntity : Attribute { }
	public class DbConstructor : Attribute { }
}
