using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
	public interface IDataMappingService<T>
	{
		public Task<T?> SelectWithCondition(Dictionary<string, object> conditionParameters);
		public Task<List<T>> SelectAll(Dictionary<string, object>? conditionParameters = null);
		public Task<bool> Insert(T obj);
		public Task<int> Delete(T obj);
		public Task<int> Update(T obj);
	}
}
