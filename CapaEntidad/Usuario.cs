using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
	/*
    create table usuario(
		id int primary key identity,
		nombres varchar(100),
		apellidos varchar(100),
		correo varchar(100),
		clave varchar(250),
		restablecer bit default 1,
		activo bit default 1,
		fecha_registro datetime default getdate()
	)
	*/
    public class Usuario
    {
		public int id { get; set; }
		public string nombres { get; set; }
		public string apellidos { get; set; }
		public string correo { get; set; }
		public string clave { get; set; }
		public bool restablecer { get; set; }
		public bool activo { get; set; }
	}
}
