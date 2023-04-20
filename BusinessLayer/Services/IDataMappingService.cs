using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
	public interface IDataMappingService<T>
	{
		public Task<T?> SelectByID(long id);
		public Task<bool> Insert(T obj);
		public Task<int> Delete(T obj);
		public Task<int> Update(T obj);
	}
}
