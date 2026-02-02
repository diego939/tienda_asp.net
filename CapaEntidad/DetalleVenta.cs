using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
	/*
    create table detalle_venta(
		id int primary key identity,
		id_venta int references venta(id),
		id_producto int references producto(id),
		cantidad int,
		total decimal(10,2)
	)
	*/
	public class DetalleVenta
    {
        public int id { get; set; }
		public int id_venta { get; set; }
		public Producto oProducto { get; set; }
		public int cantidad { get; set; }
		public decimal total { get; set; }
		public string id_transaccion { get; set; }
	}
}
