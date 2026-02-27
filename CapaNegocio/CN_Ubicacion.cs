using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
	public class CN_Ubicacion
	{
		CD_Ubicacion obj = new CD_Ubicacion();

		public List<Pais> ListarPais()
		{
			return obj.ListarPais();
		}

		public List<Provincia> ListarProvincia(int idPais)
		{
			return obj.ListarProvincia(idPais);
		}

		public List<Departamento> ListarDepartamento(int idProvincia)
		{
			return obj.ListarDepartamento(idProvincia);
		}
	}
}
