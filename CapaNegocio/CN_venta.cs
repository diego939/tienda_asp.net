using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;

namespace CapaNegocio
{
	public class CN_venta
	{
		public int Registrar(Venta obj, out string mensaje)
		{
			return new CD_Venta().Registrar(obj, out mensaje);
		}
	}
}
