using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
	/*
    create table cliente(
		id int primary key identity,
		nombre varchar(250),
		apellidos varchar(250),
		correo varchar(250),
		clave varchar(250),
		reestablecer bit default 0,
		fecha_registro datetime default getdate()
	)*/
    public class Cliente
    {
        public int id { get; set; }
		public string nombre { get; set; }
		public string apellidos { get; set; }
		public string correo { get; set; }	
		public string clave { get; set; }
		public string confirmarClave { get; set; }
		public bool reestablecer { get; set; }

	}
}
