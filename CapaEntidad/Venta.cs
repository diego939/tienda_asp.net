using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
	/*
	create table venta(
		id int primary key identity,
		id_cliente int references cliente(id),
		total_productos int,
		monto_total decimal(10,2),
		contacto varchar(250),
		id_distrito varchar(50),
		telefono varchar(50),
		direccion varchar(500),
		id_transaccion varchar(100),
		fecha_registro datetime default getdate()
	)
	*/
	public class Venta
    {
		public int id { get; set; }
		public int id_cliente { get; set; }
		public int total_productos { get; set; }
		public decimal monto_total { get; set; }
		public string contacto { get; set; }
		public string id_distrito { get; set; }
		public string telefono { get; set; }
		public string direccion { get; set; }
		public string id_transaccion { get; set; }
		public List<DetalleVenta> oDetalleVenta { get; set; }

	}
}
