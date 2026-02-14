using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using Microsoft.Data.SqlClient;

namespace CapaDatos
{
    public class CD_Usuarios
    {
        public List<Usuario> Listar()
        { 
            List<Usuario> lista = new List<Usuario>();

            try 
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadenaConexion()))
                {
                    string query = "select * from usuario";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
							lista.Add(new Usuario()
							{
								id = Convert.ToInt32(dr["id"]),
								nombres = dr["nombres"].ToString(),
								apellidos = dr["apellidos"].ToString(),
								correo = dr["correo"].ToString(),
								clave = dr["clave"].ToString(),
								restablecer = Convert.ToBoolean(dr["restablecer"]),
								activo = Convert.ToBoolean(dr["activo"])
							});
                        }
                    }
				}
			}
            catch (Exception ex)
			{
				throw new Exception("Error en CD_Usuarios.Listar", ex);
			}

			return lista;
		}

        public int Registrar(Usuario obj, out string mensaje)
        {
            int idAutogenerado = 0;
            mensaje = string.Empty;
            try
            {
				using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadenaConexion()))
                {
					SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", oconexion);
                    cmd.Parameters.AddWithValue("@nombres", obj.nombres);
					cmd.Parameters.AddWithValue("@apellidos", obj.apellidos);
					cmd.Parameters.AddWithValue("@correo", obj.correo);
					cmd.Parameters.AddWithValue("@clave", obj.clave);
					cmd.Parameters.AddWithValue("@activo", obj.activo);
					cmd.Parameters.Add("@resultado", SqlDbType.Int).Direction =  ParameterDirection.Output;
					cmd.Parameters.Add("@mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
					cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    idAutogenerado = Convert.ToInt32(cmd.Parameters["@resultado"].Value);
                    mensaje = cmd.Parameters["@mensaje"].Value.ToString();
				}

			}
            catch(Exception ex)
            {
                idAutogenerado = 0;
                mensaje = ex.Message;
            }
            return idAutogenerado;
        }

		public bool Editar(Usuario obj, out string mensaje)
		{
			bool resultado = false;
			mensaje = string.Empty;
			try
			{
				using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadenaConexion()))
				{
					SqlCommand cmd = new SqlCommand("sp_EditarUsuario", oconexion);
					cmd.Parameters.AddWithValue("@id", obj.id);
					cmd.Parameters.AddWithValue("@nombres", obj.nombres);
					cmd.Parameters.AddWithValue("@apellidos", obj.apellidos);
					cmd.Parameters.AddWithValue("@correo", obj.correo);
					cmd.Parameters.AddWithValue("@clave", obj.clave);
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
					SqlCommand cmd = new SqlCommand(
						"DELETE FROM usuario WHERE id = @id",
						oconexion
					);

					cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
					cmd.CommandType = CommandType.Text;

					oconexion.Open();
					resultado = cmd.ExecuteNonQuery() > 0;

					if (!resultado)
					{
						mensaje = "No se encontró el usuario para eliminar " + id;
					}
				}
			}
			catch (Exception ex)
			{
				mensaje = ex.Message;
			}

			return resultado;
		}

		public bool CambiarClave(int id,string nuevaClave, out string mensaje)
		{
			bool resultado = false;
			mensaje = string.Empty;

			try
			{
				using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadenaConexion()))
				{
					SqlCommand cmd = new SqlCommand(
						"update usuario set clave = @nuevaClave, restablecer = 0 where id = @id",
						oconexion
					);

					cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
					cmd.Parameters.Add("@nuevaClave", SqlDbType.VarChar).Value = nuevaClave;
					cmd.CommandType = CommandType.Text;

					oconexion.Open();
					resultado = cmd.ExecuteNonQuery() > 0;

					if (!resultado)
					{
						mensaje = "No se pudo reestablecer la clave'Error en CD_Usuarios.CambiarClave ";
					}
				}
			}
			catch (Exception ex)
			{
				mensaje = ex.Message;
			}

			return resultado;
		}

		public bool ReestablecerClave(int id,string clave ,out string mensaje)
		{
			bool resultado = false;
			mensaje = string.Empty;

			try
			{
				using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadenaConexion()))
				{
					SqlCommand cmd = new SqlCommand(
						"update usuario set clave = @clave, restablecer = 1 where id = @id",
						oconexion
					);

					cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
					cmd.Parameters.Add("@clave", SqlDbType.VarChar).Value = clave;
					cmd.CommandType = CommandType.Text;

					oconexion.Open();
					resultado = cmd.ExecuteNonQuery() > 0;

					if (!resultado)
					{
						mensaje = "No se pudo reestablecer la clave'Error en CD_Usuarios.ReestablecerClave' ";
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
