using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Categoria
    {
		private CD_Categoria objCD_Categorias = new CD_Categoria();
		public List<Categoria> Listar()
		{
			return objCD_Categorias.Listar();
		}

		public int Registrar(Categoria obj, out string mensaje)
		{
			mensaje = string.Empty;

			if (string.IsNullOrEmpty(obj.descripcion))
			{
				mensaje = "La descripción de la categoría es obligatoria";
				return 0;
			}

			if (string.IsNullOrEmpty(mensaje))
			{
				return objCD_Categorias.Registrar(obj, out mensaje);
			}
			else
			{
				return 0;
			}

		}

		public bool Editar(Categoria obj, out string mensaje)
		{
			mensaje = string.Empty;

			if (string.IsNullOrEmpty(obj.descripcion))
			{
				mensaje = "La descripción de la categoría es obligatoria";
				return false;
			}

			if (string.IsNullOrEmpty(mensaje))
			{
				return objCD_Categorias.Editar(obj, out mensaje);
			}
			else
			{
				return false;
			}
		}

		public bool Eliminar(int id, out string mensaje)
		{

			return objCD_Categorias.Eliminar(id, out mensaje);

		}
	}
}
