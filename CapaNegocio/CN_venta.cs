using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
	public class CN_venta
	{
		public int Registrar(Venta obj, out string mensaje)
		{
			return new CD_Venta().Registrar(obj, out mensaje);
		}

		public List<Venta> ListarPorCliente(int idCliente)
		{
			return new CD_Venta().ListarPorCliente(idCliente);
		}

		public List<DetalleVenta> DetalleVenta(int idVenta)
		{
			return new CD_Venta().DetalleVenta(idVenta);
		}
	}
}
