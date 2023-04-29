using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace BusinessLayer
{
    [DbEntity]
	[DataContract]
	public class Service : DomainObject
	{
        [DbAttr("name")]
        [DataMember]
        public string? Name { get; set; }
        [DbIgnore]
        [JsonIgnore]
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