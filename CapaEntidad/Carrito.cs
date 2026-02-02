using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
	/*
	create table carrito(
		id int primary key identity,
		id_cliente int references cliente(id),
		id_producto int references producto(id),
		cantidad int
	)
	*/
    public class Carrito
    {
		public int id { get; set; }
		public Cliente oCliente { get; set; }
		public Producto oProducto { get; set; }
		public int cantidad { get; set; }
	}
}
