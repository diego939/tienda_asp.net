using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
    /*
    create table categoria(
		id int primary key identity,
		descripcion varchar(250),
		activo bit default 1,
		fecha_registro datetime default getdate()
	)
	*/
{
    public class Categoria
    {
        public int id { get; set; }
        public string descripcion { get; set; }
		public bool activo { get; set; }
	}
}
