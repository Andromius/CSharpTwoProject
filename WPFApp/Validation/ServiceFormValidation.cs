using BusinessLayer.Services.FormValidationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Projekt.Validation
{
	internal class ServiceFormValidation : IFormValidationService<ServiceForm>
	{
		public string Validate(ServiceForm form)
		{
			string ret = string.Empty;
			if(form.Service.Name == null)
			{
				ret += "Service name must be filled";
			}
			return ret;
		}
	}
}
