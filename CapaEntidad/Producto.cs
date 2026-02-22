using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
	/*
	create table producto(
		id int primary key identity,
		nombre varchar(250),
		descripcion varchar(500),
		id_marca int references marca(id),
		id_categoria int references categoria(id),
		precio decimal(10,2) default 0,
		stock int default 0,
		ruta_imagen varchar(250),
		nombre_imagen varchar(250),
		activo bit default 1,
		fecha_registro datetime default getdate()
	)
	*/
    public class Producto
    {
		public int id { get; set; }
		public string nombre { get; set; }
		public string descripcion { get; set; }
		public Marca oMarca { get; set; }
		public Categoria oCategoria { get; set; }
		public decimal precio { get; set; }
		public string precioTexto { get; set; }
		public int stock { get; set; }
		public string ruta_imagen { get; set; }
		public string nombre_imagen { get; set; }
		public bool activo { get; set; }
		public string base64 { get; set; }

		public string extension { get; set; }
	}
}
