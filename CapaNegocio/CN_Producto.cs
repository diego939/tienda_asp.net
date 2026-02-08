using System;
using System.Collections.Generic;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
	public class CN_Producto
	{
		private CD_Producto objCD_Producto = new CD_Producto();

		public List<Producto> Listar()
		{
			return objCD_Producto.Listar();
		}

		public int Registrar(Producto obj, out string mensaje)
		{
			mensaje = string.Empty;

			if (string.IsNullOrWhiteSpace(obj.nombre))
			{
				mensaje = "El nombre del producto es obligatorio";
				return 0;
			}

			if (string.IsNullOrWhiteSpace(obj.descripcion))
			{
				mensaje = "La descripcion del producto es obligatorio";
				return 0;
			}

			if (obj.oMarca == null || obj.oMarca.id == 0)
			{
				mensaje = "Debe seleccionar una marca válida";
				return 0;
			}

			if (obj.oCategoria == null || obj.oCategoria.id == 0)
			{
				mensaje = "Debe seleccionar una categoría válida";
				return 0;
			}

			if (obj.precio <= 0)
			{
				mensaje = "El precio debe ser mayor a cero";
				return 0;
			}

			if (obj.stock < 0)
			{
				mensaje = "El stock no puede ser negativo";
				return 0;
			}

			return objCD_Producto.Registrar(obj, out mensaje);
		}

		public bool Editar(Producto obj, out string mensaje)
		{
			mensaje = string.Empty;

			if (obj.id == 0)
			{
				mensaje = "ID de producto inválido";
				return false;
			}

			if (string.IsNullOrWhiteSpace(obj.nombre))
			{
				mensaje = "El nombre del producto es obligatorio";
				return false;
			}

			if (string.IsNullOrWhiteSpace(obj.descripcion))
			{
				mensaje = "La descripcion del producto es obligatorio";
				return false;
			}

			if (obj.oMarca == null || obj.oMarca.id == 0)
			{
				mensaje = "Debe seleccionar una marca válida";
				return false;
			}

			if (obj.oCategoria == null || obj.oCategoria.id == 0)
			{
				mensaje = "Debe seleccionar una categoría válida";
				return false;
			}

			if (obj.precio <= 0)
			{
				mensaje = "El precio debe ser mayor a cero";
				return false;
			}

			if (obj.stock < 0)
			{
				mensaje = "El stock no puede ser negativo";
				return false;
			}

			return objCD_Producto.Editar(obj, out mensaje);
		}

		public bool GuardarImagen(Producto obj, out string mensaje)
		{
			mensaje = string.Empty;

			if (obj.id == 0)
			{
				mensaje = "ID de producto inválido";
				return false;
			}

			if (string.IsNullOrEmpty(obj.ruta_imagen) || string.IsNullOrEmpty(obj.nombre_imagen))
			{
				mensaje = "Los datos de la imagen son obligatorios";
				return false;
			}

			return objCD_Producto.GuardarImagen(obj, out mensaje);
		}

		public bool Eliminar(int id, out string mensaje)
		{
			if (id == 0)
			{
				mensaje = "ID de producto inválido";
				return false;
			}

			return objCD_Producto.Eliminar(id, out mensaje);
		}
	}
}

