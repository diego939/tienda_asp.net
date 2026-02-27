using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using CapaEntidad;

namespace CapaDatos
{
	public class CD_Ubicacion
	{
		public List<Pais> ListarPais()
		{
			List<Pais> lista = new List<Pais>();

			try
			{
				using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadenaConexion()))
				{
					string query = "SELECT id, descripcion FROM pais";

					SqlCommand cmd = new SqlCommand(query, oconexion);
					cmd.CommandType = CommandType.Text;

					oconexion.Open();

					using (SqlDataReader dr = cmd.ExecuteReader())
					{
						while (dr.Read())
						{
							lista.Add(new Pais()
							{
								id = Convert.ToInt32(dr["id"]),
								descripcion = dr["descripcion"].ToString()
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error en CD_Ubicacion.ListarPais", ex);
			}

			return lista;
		}

		public List<Provincia> ListarProvincia(int idPais)
		{
			List<Provincia> lista = new List<Provincia>();

			try
			{
				using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadenaConexion()))
				{
					string query = "SELECT id, descripcion FROM provincia WHERE id_pais = @idPais";

					SqlCommand cmd = new SqlCommand(query, oconexion);
					cmd.CommandType = CommandType.Text;
					cmd.Parameters.AddWithValue("@idPais", idPais);

					oconexion.Open();

					using (SqlDataReader dr = cmd.ExecuteReader())
					{
						while (dr.Read())
						{
							lista.Add(new Provincia()
							{
								id = Convert.ToInt32(dr["id"]),
								descripcion = dr["descripcion"].ToString()
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error en CD_Ubicacion.ListarProvincia", ex);
			}

			return lista;
		}

		public List<Departamento> ListarDepartamento(int idProvincia)
		{
			List<Departamento> lista = new List<Departamento>();

			try
			{
				using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadenaConexion()))
				{
					string query = "SELECT id, descripcion FROM departamento WHERE id_provincia = @idProvincia";

					SqlCommand cmd = new SqlCommand(query, oconexion);
					cmd.CommandType = CommandType.Text;
					cmd.Parameters.AddWithValue("@idProvincia", idProvincia);

					oconexion.Open();

					using (SqlDataReader dr = cmd.ExecuteReader())
					{
						while (dr.Read())
						{
							lista.Add(new Departamento()
							{
								id = Convert.ToInt32(dr["id"]),
								descripcion = dr["descripcion"].ToString()
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error en CD_Ubicacion.ListarDepartamento", ex);
			}

			return lista;
		}
	}
}