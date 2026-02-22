using System;
using System.Collections.Generic;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
	public class CN_Marca
	{
		private CD_Marca objCD_Marca = new CD_Marca();

		public List<Marca> Listar()
		{
			return objCD_Marca.Listar();
		}

		public int Registrar(Marca obj, out string mensaje)
		{
			mensaje = string.Empty;

			if (string.IsNullOrEmpty(obj.descripcion))
			{
				mensaje = "La descripción de la marca es obligatoria";
				return 0;
			}

			return objCD_Marca.Registrar(obj, out mensaje);
		}

		public bool Editar(Marca obj, out string mensaje)
		{
			mensaje = string.Empty;

			if (string.IsNullOrEmpty(obj.descripcion))
			{
				mensaje = "La descripción de la marca es obligatoria";
				return false;
			}

			return objCD_Marca.Editar(obj, out mensaje);
		}

		public bool Eliminar(int id, out string mensaje)
		{
			return objCD_Marca.Eliminar(id, out mensaje);
		}

		public List<Marca> ListarMarcaPorCategoria(int idCategoria)
		{
			return objCD_Marca.ListarMarcaPorCategoria(idCategoria);
		}
	}
}
