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
				string clave = CN_Recursos.GenerarClave();
				string asunto = "Creación de cuenta";
				string mensajeCorreo = "<h3>Su cuenta ha sido creada exitosamente</h3><br></br><p>Su clave de acceso es: <strong>" + clave + "</strong></p>";
				bool respuesta = CN_Recursos.EnviarCorreo(obj.correo, asunto, mensajeCorreo);
				if (respuesta)
				{
					obj.clave = CN_Recursos.ConvertirSha256(clave);
					return objCD_Usuarios.Registrar(obj, out mensaje);
				}
				else
				{
					mensaje = "No se pudo enviar el correo";
					return 0;
				}
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

		public bool CambiarClave(int id, string nuevaClave, out string mensaje)
		{
			return objCD_Usuarios.CambiarClave(id, nuevaClave, out mensaje);
		}

		public bool ReestablecerClave(int id, string correo, out string mensaje)
		{
			mensaje = string.Empty;
			string nuevaClave = CN_Recursos.GenerarClave();
			bool resultado = objCD_Usuarios.ReestablecerClave(id, CN_Recursos.ConvertirSha256(nuevaClave), out mensaje);

			if (resultado)
			{
				string asunto = "Contraseña Reestablecida";
				string mensajeCorreo = "<h3>Su cuenta ha sido reestablecida exitosamente</h3><br></br><p>Su clave de acceso es: <strong>" + nuevaClave + "</strong></p>";
				bool respuesta = CN_Recursos.EnviarCorreo(correo, asunto, mensajeCorreo);
				if (respuesta)
				{
					mensaje = "Contraseña reestablecida y correo enviado";
					return true;
				}
				else
				{
					mensaje = "No se pudo enviar el correo";
					return false;
				}
			}
			else
			{
				mensaje = "No se pudo reestablecer la contraseña";
				return false;
			}

		}
	}
}
