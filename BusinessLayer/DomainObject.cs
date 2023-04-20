using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
	public abstract class DomainObject
	{
        [DbIgnore]
        [DbAttr("id")]
        public long? Id { get; set; }
        public DomainObject(long? id = null)
        {
            Id = id;
        }
    }
}
