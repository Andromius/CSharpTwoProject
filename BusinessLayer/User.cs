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
        public string Name { get; set; }
		[DbAttr("surname")]
		public string Surname { get; set; }
        public User(string name, string surname, long? id = null) : base(id)
        {
            Name = name;
            Surname = surname;
        }
    }
}
