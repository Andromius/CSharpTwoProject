using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
	public class ExportJSONService<T> where T : DomainObject
	{
		private Stream _stream;
		public ExportJSONService(Stream stream)
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
				using(StreamWriter sw = new StreamWriter(_stream))
				{
					sw.Write(JsonSerializer.Serialize(elements, new JsonSerializerOptions()
					{
							WriteIndented = true,
							Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
					}));
					sw.Flush();
				}
			}
		}
	}
}
