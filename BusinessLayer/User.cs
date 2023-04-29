using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BusinessLayer
{
    [DbEntity]
	[DataContract]
	public class User : DomainObject
	{
        [DbAttr("name")]
        [DataMember]
        public string? Name { get; set; }
		[DbAttr("surname")]
		[DataMember]
		public string? Surname { get; set; }
		[DbAttr("email")]
		[DataMember]
		public string? Email { get; set; }
		[DbAttr("pass")]
        [JsonIgnore]
		public string? Password { get; set; }
        [DbIgnore]
        [JsonIgnore]
		public bool IsPartial => Name == null;
		[DbConstructor]
        public User(string name, string surname, string email, string password, long id) : base(id)
        {
            Name = name;
            Surname = surname;
            Email = email;
            Password = password;
        }
        public User() : base() { }
        public User(long id) : base(id) { }
        public User(string name, string surname, string email, string password) 
        {
            Name = name;
            Surname = surname;
            Email = email;
            Password = new PasswordHasher().HashPassword(password);
        }
		public override string ToString()
		{
            return $"{Name} {Surname}";
		}
	}
}
