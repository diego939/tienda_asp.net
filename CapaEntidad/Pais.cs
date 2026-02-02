using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
	/*
	create table pais(
		id int primary key identity,
		descripcion varchar(250)
	)
	*/
	public class Pais
    {
		public int id { get; set; }
		public string descripcion { get; set; }
	}
}
