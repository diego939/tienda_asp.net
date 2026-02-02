using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Usuarios
    {
        private CD_Usuarios objCD_Usuarios = new CD_Usuarios();
        public List<Usuario> Listar()
        {
            return objCD_Usuarios.Listar();
		}

		public int Registrar(Usuario obj, out string mensaje)
        {
			mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.nombres))
            {
                mensaje = "El nombre del usuario es obligatorio";
				return 0;
			}
			if (string.IsNullOrEmpty(obj.apellidos))
			{
				mensaje = "El apellido del usuario es obligatorio";
				return 0;

			}
			if (string.IsNullOrEmpty(obj.correo))
			{
				mensaje = "El correo del usuario es obligatorio";
				return 0;
			}
			/*if (string.IsNullOrEmpty(obj.clave))
			{
				mensaje = "La clave del usuario es obligatoria";
				return 0;
			}*/

			if (string.IsNullOrEmpty(mensaje))
			{
				string clave = "test123";
				obj.clave = CN_Recursos.ConvertirSha256(clave);
				return objCD_Usuarios.Registrar(obj, out mensaje);
			}
			else
			{
				return 0;
			}

		}

		public bool Editar(Usuario obj, out string mensaje)
		{
			mensaje = string.Empty;

			if (string.IsNullOrEmpty(obj.nombres))
			{
				mensaje = "El nombre del usuario es obligatorio";
			}
			if (string.IsNullOrEmpty(obj.apellidos))
			{
				mensaje = "El apellido del usuario es obligatorio";
			}
			if (string.IsNullOrEmpty(obj.correo))
			{
				mensaje = "El correo del usuario es obligatorio";
			}
			/*if (string.IsNullOrEmpty(obj.clave))
			{
				mensaje = "La clave del usuario es obligatoria";
			}*/

			if (string.IsNullOrEmpty(mensaje))
			{
				return objCD_Usuarios.Editar(obj, out mensaje);
			}
			else
			{
				return false;
			}
		}

		public bool Eliminar(int id, out string mensaje)
		{
			return objCD_Usuarios.Eliminar(id, out mensaje);
		}
	}
}
