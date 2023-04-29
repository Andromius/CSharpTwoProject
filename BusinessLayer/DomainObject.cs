using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    [DataContract]
    public abstract class DomainObject
	{
        [DbPrimaryKey]
        [DataMember]
        [DbAttr("id")]
        public long? Id { get; set; }
        public DomainObject(long? id = null)
        {
            Id = id;
        }
    }
}
