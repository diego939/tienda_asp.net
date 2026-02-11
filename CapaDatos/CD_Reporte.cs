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
	}
}