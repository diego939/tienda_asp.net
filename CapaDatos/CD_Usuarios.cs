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
                    string query = "select id, nombres, apellidos, correo, activo from usuario";
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
					SqlCommand cmd = new SqlCommand("delete top (1) form usuario where id = @id", oconexion);
					cmd.Parameters.AddWithValue("@id", id);
					cmd.CommandType = CommandType.Text;
					oconexion.Open();
					resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
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
