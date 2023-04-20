using System.Reflection.Metadata.Ecma335;

namespace BusinessLayer
{
    [DbEntity]
    public class Service : DomainObject
	{
		[DbAttr("name")]
		public string? Name { get; set; }
        [DbIgnore]
        public bool IsPartial => Name == null;
        public Service(long id, string? name = null) : base(id)
        {
            Id = id;
            Name = name;
        }
    }
}