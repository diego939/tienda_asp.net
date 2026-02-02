using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
	/*
	create table provincia(
		id int primary key identity,
		descripcion varchar(250),
		id_pais int references pais(id)
	)
	*/
    public class Provincia
    {
        public int id { get; set; }
		public string descripcion { get; set; }

	}
}
