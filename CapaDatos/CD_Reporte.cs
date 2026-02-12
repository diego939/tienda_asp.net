using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using Microsoft.Data.SqlClient;


namespace CapaDatos
{
	public class CD_Reporte
	{
		public DashBoard VerDashboard()
		{
			DashBoard objeto = new DashBoard();

			try
			{
				using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadenaConexion()))
				{
					SqlCommand cmd = new SqlCommand("sp_ReporteDashboard", oconexion);
					cmd.CommandType = CommandType.StoredProcedure;
					oconexion.Open();
					using (SqlDataReader dr = cmd.ExecuteReader())
					{
						while (dr.Read())
						{
							objeto = new DashBoard()
							{
								totalCliente = Convert.ToInt32(dr["totalCliente"]),
								totalVenta = Convert.ToInt32(dr["totalVenta"]),
								totalProducto = Convert.ToInt32(dr["totalProducto"])
							};
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error en CD_Reporte.VerDashboard", ex);
			}

			return objeto;
		}

		public List<Reporte> Ventas(string fechainicio, string fechafin, string idtransaccion)
		{
			List<Reporte> lista = new List<Reporte>();

			try
			{
				using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadenaConexion()))
				{
					SqlCommand cmd = new SqlCommand("sp_ReporteVentas", oconexion);
					cmd.Parameters.AddWithValue("@fechainicio", fechainicio);
					cmd.Parameters.AddWithValue("@fechafin", fechafin);
					cmd.Parameters.AddWithValue("@idtransaccion", idtransaccion);
					cmd.CommandType = CommandType.StoredProcedure;
					oconexion.Open();
					using (SqlDataReader dr = cmd.ExecuteReader())
					{
						while (dr.Read())
						{
							lista.Add(new Reporte()
							{
								fechaVenta = dr["fechaVenta"].ToString(),
								cliente = dr["cliente"].ToString(),
								producto = dr["producto"].ToString(),
								precio = Convert.ToDecimal(dr["precio"]),
								cantidad = Convert.ToInt32(dr["cantidad"]),
								total = Convert.ToDecimal(dr["total"]),
								transaccion = dr["transaccion"].ToString()
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error en CD_Reporte.Ventas", ex);
			}

			return lista;
		}
	}
}