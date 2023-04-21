using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        [DbIgnore]
        public bool IsPartial => Name == null;
		[DbConstructor]
        public User(string name, string surname, long id) : base(id)
        {
            Name = name;
            Surname = surname;
        }
        public User(long id) : base(id) { }
        public User(string name, string surname) 
        {
            Name = name;
            Surname = surname;
        }
    }
}
