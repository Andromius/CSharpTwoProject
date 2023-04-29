using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BusinessLayer.Services
{
	public class ExportXMLService<T>
	{
		private Stream _stream;
		public ExportXMLService(Stream stream)
		{
			_stream = stream;
		}
		public void Export(List<T> elements)
		{
			if (elements.Count < 1)
			{
				_stream.Close();
				_stream.Dispose();
				return;
			}
			using (_stream)
			{
				DataContractSerializer xmlSerializer = new DataContractSerializer(typeof(List<T>));
				xmlSerializer.WriteObject(_stream, elements);
			}
		}
	}
}
