using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
	/*
	create table departamento(
		id int primary key identity,
		descripcion varchar(250),
		id_pais int references pais(id),
		id_provincia int references provincia(id)
	)
	*/
    public class Departamento
    {
		public string id { get; set; }
		public string descripcion { get; set; }
	}
}
