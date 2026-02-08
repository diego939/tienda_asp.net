using System;
using System.Collections.Generic;
using System.Data;
using CapaEntidad;
using Microsoft.Data.SqlClient;

namespace CapaDatos
{
	public class CD_Producto
	{
		public List<Producto> Listar()
		{
			List<Producto> lista = new List<Producto>();

			try
			{
				using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadenaConexion()))
				{
					string query = @"SELECT 
										p.id,
										p.nombre,
										p.descripcion,
										m.id[idMarca],
										m.descripcion[desMarca],
										c.id[idCategoria],
										c.descripcion[desCategoria],
										p.precio,
										p.stock,
										p.activo,
										p.ruta_imagen,
										p.nombre_imagen
										FROM producto p
										inner join marca m on m.id = p.id_marca
										inner join categoria c on c.id = p.id_categoria
										";

					SqlCommand cmd = new SqlCommand(query, oconexion);
					cmd.CommandType = CommandType.Text;

					oconexion.Open();

					using (SqlDataReader dr = cmd.ExecuteReader())
					{
						while (dr.Read())
						{
							lista.Add(new Producto()
							{
								id = Convert.ToInt32(dr["id"]),
								nombre = dr["nombre"].ToString(),
								descripcion = dr["descripcion"].ToString(),
								oMarca = new Marca()
								{
									id = Convert.ToInt32(dr["idMarca"]),
									descripcion = dr["desMarca"].ToString()
								},
								oCategoria = new Categoria()
								{
									id = Convert.ToInt32(dr["idCategoria"]),
									descripcion = dr["desCategoria"].ToString()
								},
								precio = Convert.ToDecimal(dr["precio"]),
								stock = Convert.ToInt32(dr["stock"]),
								activo = Convert.ToBoolean(dr["activo"]),
								ruta_imagen = dr["ruta_imagen"].ToString(),
								nombre_imagen = dr["nombre_imagen"].ToString()
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error en CD_Producto.Listar", ex);
			}

			return lista;
		}

		public int Registrar(Producto obj, out string mensaje)
		{
			int idAutogenerado = 0;
			mensaje = string.Empty;

			try
			{
				using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadenaConexion()))
				{
					SqlCommand cmd = new SqlCommand("sp_RegistrarProducto", oconexion);
					cmd.CommandType = CommandType.StoredProcedure;

					cmd.Parameters.AddWithValue("@nombre", obj.nombre);
					cmd.Parameters.AddWithValue("@descripcion", obj.descripcion);
					cmd.Parameters.AddWithValue("@id_marca", obj.oMarca.id);
					cmd.Parameters.AddWithValue("@id_categoria", obj.oCategoria.id);
					cmd.Parameters.AddWithValue("@precio", obj.precio);
					cmd.Parameters.AddWithValue("@stock", obj.stock);
					cmd.Parameters.AddWithValue("@activo", obj.activo);

					cmd.Parameters.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
					cmd.Parameters.Add("@mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

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

		public bool Editar(Producto obj, out string mensaje)
		{
			bool resultado = false;
			mensaje = string.Empty;

			try
			{
				using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadenaConexion()))
				{
					SqlCommand cmd = new SqlCommand("sp_EditarProducto", oconexion);
					cmd.CommandType = CommandType.StoredProcedure;

					cmd.Parameters.AddWithValue("@id", obj.id);
					cmd.Parameters.AddWithValue("@nombre", obj.nombre);
					cmd.Parameters.AddWithValue("@descripcion", obj.descripcion);
					cmd.Parameters.AddWithValue("@id_marca", obj.oMarca.id);
					cmd.Parameters.AddWithValue("@id_categoria", obj.oCategoria.id);
					cmd.Parameters.AddWithValue("@precio", obj.precio);
					cmd.Parameters.AddWithValue("@stock", obj.stock);
					cmd.Parameters.AddWithValue("@activo", obj.activo);

					cmd.Parameters.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
					cmd.Parameters.Add("@mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

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

		public bool GuardarImagen(Producto obj, out string mensaje)
		{
			bool resultado = false;
			mensaje = string.Empty;
			try
			{
				using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadenaConexion()))
				{
					string query = @"UPDATE producto SET ruta_imagen = @ruta_imagen, nombre_imagen = @nombre_imagen
									WHERE id = @id";
					SqlCommand cmd = new SqlCommand(query, oconexion);
					cmd.CommandType = CommandType.Text;
					cmd.Parameters.AddWithValue("@id", obj.id);
					cmd.Parameters.AddWithValue("@ruta_imagen", obj.ruta_imagen);
					cmd.Parameters.AddWithValue("@nombre_imagen", obj.nombre_imagen);
					oconexion.Open();
					if (cmd.ExecuteNonQuery() > 0)
					{
						resultado = true;
						mensaje = "Imagen guardada correctamente.";
					}
					else
					{
						resultado = false;
						mensaje = "No se pudo guardar la imagen.";

					}
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
					SqlCommand cmd = new SqlCommand("sp_EliminarProducto", oconexion);
					cmd.CommandType = CommandType.StoredProcedure;

					cmd.Parameters.AddWithValue("@id", id);
					cmd.Parameters.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
					cmd.Parameters.Add("@mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

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
