using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessLayer
{
    [DbEntity]
    public class User : DomainObject
	{
        [DbAttr("name")]
        public string? Name { get; set; }
		[DbAttr("surname")]
		public string? Surname { get; set; }
		[DbAttr("email")]
		public string? Email { get; set; }
		[DbAttr("pass")]
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
