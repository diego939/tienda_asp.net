using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
	public class CN_Cliente
	{
		private CD_Cliente objCD_Clientes = new CD_Cliente();

		public List<Cliente> Listar()
		{
			return objCD_Clientes.Listar();
		}

		public int Registrar(Cliente obj, out string mensaje)
		{
			mensaje = string.Empty;

			if (string.IsNullOrEmpty(obj.nombre))
			{
				mensaje = "El nombre del cliente es obligatorio";
				return 0;
			}

			if (string.IsNullOrEmpty(obj.apellidos))
			{
				mensaje = "El apellido del cliente es obligatorio";
				return 0;
			}

			if (string.IsNullOrEmpty(obj.correo))
			{
				mensaje = "El correo del cliente es obligatorio";
				return 0;
			}

			if (string.IsNullOrEmpty(mensaje))
			{
				return objCD_Clientes.Registrar(obj, out mensaje);
			}
			else
			{
				return 0;
			}
		}

		public bool Editar(Cliente obj, out string mensaje)
		{
			mensaje = string.Empty;

			if (string.IsNullOrEmpty(obj.nombre))
			{
				mensaje = "El nombre del cliente es obligatorio";
			}

			if (string.IsNullOrEmpty(obj.apellidos))
			{
				mensaje = "El apellido del cliente es obligatorio";
			}

			if (string.IsNullOrEmpty(obj.correo))
			{
				mensaje = "El correo del cliente es obligatorio";
			}

			if (string.IsNullOrEmpty(mensaje))
			{
				return objCD_Clientes.Editar(obj, out mensaje);
			}
			else
			{
				return false;
			}
		}

		public bool Eliminar(int id, out string mensaje)
		{
			return objCD_Clientes.Eliminar(id, out mensaje);
		}

		public bool CambiarClave(int id, string nuevaClave, out string mensaje)
		{
			string claveEncriptada = CN_Recursos.ConvertirSha256(nuevaClave);
			return objCD_Clientes.CambiarClave(id, claveEncriptada, out mensaje);
		}

		public bool ReestablecerClave(int id, string correo, out string mensaje)
		{
			mensaje = string.Empty;
			string nuevaClave = CN_Recursos.GenerarClave();

			bool resultado = objCD_Clientes.ReestablecerClave(
				id,
				CN_Recursos.ConvertirSha256(nuevaClave),
				out mensaje
			);

			if (resultado)
			{
				string asunto = "Contraseña Reestablecida";
				string mensajeCorreo = "<h3>Su cuenta ha sido reestablecida exitosamente</h3><br></br>" +
									   "<p>Su clave de acceso es: <strong>" + nuevaClave + "</strong></p>";

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
