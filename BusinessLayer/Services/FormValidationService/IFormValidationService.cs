using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.FormValidationService
{
    public interface IFormValidationService<T> where T : class
	{
        public string Validate(T form);
    }
}
