using System;
using System.Collections.Generic;
using System.Data;
using CapaEntidad;
using Microsoft.Data.SqlClient;

namespace CapaDatos
{
	public class CD_Marca
	{
		public List<Marca> Listar()
		{
			List<Marca> lista = new List<Marca>();

			try
			{
				using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadenaConexion()))
				{
					string query = "SELECT id, descripcion, activo FROM marca";
					SqlCommand cmd = new SqlCommand(query, oconexion);
					cmd.CommandType = CommandType.Text;
					oconexion.Open();

					using (SqlDataReader dr = cmd.ExecuteReader())
					{
						while (dr.Read())
						{
							lista.Add(new Marca()
							{
								id = Convert.ToInt32(dr["id"]),
								descripcion = dr["descripcion"].ToString(),
								activo = Convert.ToBoolean(dr["activo"])
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error en CD_Marca.Listar", ex);
			}

			return lista;
		}

		public int Registrar(Marca obj, out string mensaje)
		{
			int idAutogenerado = 0;
			mensaje = string.Empty;

			try
			{
				using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadenaConexion()))
				{
					SqlCommand cmd = new SqlCommand("sp_RegistrarMarca", oconexion);
					cmd.Parameters.AddWithValue("@descripcion", obj.descripcion);
					cmd.Parameters.AddWithValue("@activo", obj.activo);
					cmd.Parameters.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
					cmd.Parameters.Add("@mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
					cmd.CommandType = CommandType.StoredProcedure;

					oconexion.Open();
					cmd.ExecuteNonQuery();

					idAutogenerado = Convert.ToInt32(cmd.Parameters["@resultado"].Value);
					mensaje = cmd.Parameters["@mensaje"].Value.ToString();
				}
			}
			catch (Exception ex)
			{
				idAutogenerado = 0;
				mensaje = ex.Message;
			}

			return idAutogenerado;
		}

		public bool Editar(Marca obj, out string mensaje)
		{
			bool resultado = false;
			mensaje = string.Empty;

			try
			{
				using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadenaConexion()))
				{
					SqlCommand cmd = new SqlCommand("sp_EditarMarca", oconexion);
					cmd.Parameters.AddWithValue("@id", obj.id);
					cmd.Parameters.AddWithValue("@descripcion", obj.descripcion);
					cmd.Parameters.AddWithValue("@activo", obj.activo);
					cmd.Parameters.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
					cmd.Parameters.Add("@mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
					cmd.CommandType = CommandType.StoredProcedure;

					oconexion.Open();
					cmd.ExecuteNonQuery();

					resultado = Convert.ToBoolean(cmd.Parameters["@resultado"].Value);
					mensaje = cmd.Parameters["@mensaje"].Value.ToString();
				}
			}
			catch (Exception ex)
			{
				resultado = false;
				mensaje = ex.Message;
			}

			return resultado;
		}

		public bool Eliminar(int id, out string mensaje)
		{
			bool resultado = false;
			mensaje = string.Empty;

			try
			{
				using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadenaConexion()))
				{
					SqlCommand cmd = new SqlCommand("sp_EliminarMarca", oconexion);
					cmd.Parameters.AddWithValue("@id", id);
					cmd.Parameters.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
					cmd.Parameters.Add("@mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
					cmd.CommandType = CommandType.StoredProcedure;

					oconexion.Open();
					cmd.ExecuteNonQuery();

					resultado = Convert.ToBoolean(cmd.Parameters["@resultado"].Value);
					mensaje = cmd.Parameters["@mensaje"].Value.ToString();
				}
			}
			catch (Exception ex)
			{
				resultado = false;
				mensaje = ex.Message;
			}

			return resultado;
		}
	}
}

