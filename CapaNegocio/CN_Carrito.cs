using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;

namespace CapaNegocio
{
	public class CN_Carrito
	{
		private CD_Carrito objCapaDatos = new CD_Carrito();

		public bool Agregar(Carrito obj, out string mensaje)
		{
			mensaje = string.Empty;

			// 🚨 Solo bloqueamos el 0
			if (obj.cantidad == 0)
			{
				mensaje = "Cantidad inválida.";
				return false;
			}

			return objCapaDatos.Agregar(obj, out mensaje);
		}

		public List<Carrito> Listar(int idCliente)
		{
			return objCapaDatos.Listar(idCliente);
		}

		public bool Eliminar(int idCarrito, out string mensaje)
		{
			return objCapaDatos.Eliminar(idCarrito, out mensaje);
		}

		public bool ActualizarCantidad(int idCarrito, int nuevaCantidad, out string mensaje)
		{
			mensaje = string.Empty;

			if (nuevaCantidad <= 0)
			{
				mensaje = "La cantidad debe ser mayor a 0.";
				return false;
			}

			return objCapaDatos.ActualizarCantidad(idCarrito, nuevaCantidad, out mensaje);
		}

		public int CantidadEnCarrito(int idUsuario)
		{
			return new CD_Carrito().CantidadEnCarrito(idUsuario);
		}
	}
}
