using System;
using System.Collections.Generic;
using System.Data;
using CapaEntidad;
using Microsoft.Data.SqlClient;

namespace CapaDatos
{
	public class CD_Cliente
	{
		public List<Cliente> Listar()
		{
			List<Cliente> lista = new List<Cliente>();

			try
			{
				using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadenaConexion()))
				{
					string query = "SELECT * FROM cliente";

					SqlCommand cmd = new SqlCommand(query, oconexion);
					cmd.CommandType = CommandType.Text;

					oconexion.Open();

					using (SqlDataReader dr = cmd.ExecuteReader())
					{
						while (dr.Read())
						{
							lista.Add(new Cliente()
							{
								id = Convert.ToInt32(dr["id"]),
								nombre = dr["nombre"].ToString(),
								apellidos = dr["apellidos"].ToString(),
								correo = dr["correo"].ToString(),
								clave = dr["clave"].ToString(),
								reestablecer = Convert.ToBoolean(dr["reestablecer"])
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error en CD_Cliente.Listar", ex);
			}

			return lista;
		}

		public int Registrar(Cliente obj, out string mensaje)
		{
			int idAutogenerado = 0;
			mensaje = string.Empty;

			try
			{
				using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadenaConexion()))
				{
					SqlCommand cmd = new SqlCommand("sp_RegistrarCliente", oconexion);
					cmd.CommandType = CommandType.StoredProcedure;

					cmd.Parameters.AddWithValue("@nombre", obj.nombre);
					cmd.Parameters.AddWithValue("@apellidos", obj.apellidos);
					cmd.Parameters.AddWithValue("@correo", obj.correo);
					cmd.Parameters.AddWithValue("@clave", obj.clave);

					cmd.Parameters.Add("@mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
					cmd.Parameters.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output;

					oconexion.Open();
					cmd.ExecuteNonQuery();

					idAutogenerado = Convert.ToInt32(cmd.Parameters["@resultado"].Value);
					mensaje = cmd.Parameters["@mensaje"].Value?.ToString();
				}
			}
			catch (Exception ex)
			{
				mensaje = ex.Message;
				idAutogenerado = 0;
			}

			return idAutogenerado;
		}

		public bool Editar(Cliente obj, out string mensaje)
		{
			bool resultado = false;
			mensaje = string.Empty;

			try
			{
				using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadenaConexion()))
				{
					SqlCommand cmd = new SqlCommand("sp_EditarCliente", oconexion);
					cmd.CommandType = CommandType.StoredProcedure;

					cmd.Parameters.AddWithValue("@id", obj.id);
					cmd.Parameters.AddWithValue("@nombre", obj.nombre);
					cmd.Parameters.AddWithValue("@apellidos", obj.apellidos);
					cmd.Parameters.AddWithValue("@correo", obj.correo);

					if (string.IsNullOrEmpty(obj.clave))
						cmd.Parameters.AddWithValue("@clave", DBNull.Value);
					else
						cmd.Parameters.AddWithValue("@clave", obj.clave);

					cmd.Parameters.Add("@mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
					cmd.Parameters.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output;

					oconexion.Open();
					cmd.ExecuteNonQuery();

					resultado = Convert.ToInt32(cmd.Parameters["@resultado"].Value) == 1;
					mensaje = cmd.Parameters["@mensaje"].Value?.ToString();
				}
			}
			catch (Exception ex)
			{
				mensaje = ex.Message;
				resultado = false;
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
					SqlCommand cmd = new SqlCommand(
						"DELETE FROM cliente WHERE id = @id",
						oconexion
					);

					cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
					cmd.CommandType = CommandType.Text;

					oconexion.Open();
					resultado = cmd.ExecuteNonQuery() > 0;

					if (!resultado)
						mensaje = "No se encontró el cliente para eliminar.";
				}
			}
			catch (Exception ex)
			{
				mensaje = ex.Message;
			}

			return resultado;
		}

		public bool CambiarClave(int id, string nuevaClave, out string mensaje)
		{
			bool resultado = false;
			mensaje = string.Empty;

			try
			{
				using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadenaConexion()))
				{
					SqlCommand cmd = new SqlCommand(
						"UPDATE cliente SET clave = @clave, reestablecer = 0 WHERE id = @id",
						oconexion
					);

					cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
					cmd.Parameters.Add("@clave", SqlDbType.VarChar, 250).Value = nuevaClave;

					oconexion.Open();
					resultado = cmd.ExecuteNonQuery() > 0;

					if (!resultado)
						mensaje = "No se pudo cambiar la clave.";
				}
			}
			catch (Exception ex)
			{
				mensaje = ex.Message;
			}

			return resultado;
		}

		public bool ReestablecerClave(int id, string clave, out string mensaje)
		{
			bool resultado = false;
			mensaje = string.Empty;

			try
			{
				using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadenaConexion()))
				{
					SqlCommand cmd = new SqlCommand(
						"update cliente set clave = @clave, reestablecer = 1 where id = @id",
						oconexion
					);

					cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
					cmd.Parameters.Add("@clave", SqlDbType.VarChar).Value = clave;
					cmd.CommandType = CommandType.Text;

					oconexion.Open();
					resultado = cmd.ExecuteNonQuery() > 0;

					if (!resultado)
					{
						mensaje = "No se pudo reestablecer la clave'Error en CD_Cliente.ReestablecerClave' ";
					}
				}
			}
			catch (Exception ex)
			{
				mensaje = ex.Message;
			}

			return resultado;
		}
	}
}
