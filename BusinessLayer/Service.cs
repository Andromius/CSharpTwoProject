using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace BusinessLayer
{
    [DbEntity]
    public class Service : DomainObject
	{
        [DbAttr("name")]
        public string? Name { get; set; }
        [DbIgnore]
        public bool IsPartial => Name == null;
        [DbConstructor]
        public Service(string name, long id) : base(id)
        {
            Id = id;
            Name = name;
        }
        public Service(long id) : base(id) { }
        public Service(string name)
        {
            Name = name;
        }
        public Service() : base() { }
		public override string ToString()
		{
            return $"{Name}";
		}
	}
}