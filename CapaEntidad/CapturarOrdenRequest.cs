using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
	public class CapturarOrdenRequest
	{
		public string contacto { get; set; }
		public string telefono { get; set; }
		public string direccion { get; set; }
		public int idDepartamento { get; set; }
		public string orderId { get; set; }

	}
}
