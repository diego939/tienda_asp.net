using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using CapaDatos;
using CapaEntidad;

public class CD_Carrito
{
	public List<Carrito> Listar(int idCliente)
	{
		List<Carrito> lista = new List<Carrito>();

		try
		{
			using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadenaConexion()))
			{
				string query = @"
            select c.id,
                   p.id id_producto,
				   p.ruta_imagen,
                   p.nombre,
                   p.precio,
                   p.stock,
                   c.cantidad
            from carrito c
            inner join producto p on p.id = c.id_producto
            where c.id_cliente = @id_cliente";

				SqlCommand cmd = new SqlCommand(query, oconexion);
				cmd.Parameters.AddWithValue("@id_cliente", idCliente);

				cmd.CommandType = CommandType.Text;

				oconexion.Open();

				using (SqlDataReader dr = cmd.ExecuteReader())
				{
					while (dr.Read())
					{
						lista.Add(new Carrito()
						{
							id = Convert.ToInt32(dr["id"]),
							oProducto = new Producto()
							{
								id = Convert.ToInt32(dr["id_producto"]),
								nombre = dr["nombre"].ToString(),
								precio = Convert.ToDecimal(dr["precio"]),
								stock = Convert.ToInt32(dr["stock"]),
								ruta_imagen = dr["ruta_imagen"].ToString()
							},
							cantidad = Convert.ToInt32(dr["cantidad"])
						});
					}
				}
			}
		}
		catch
		{
			lista = new List<Carrito>();
		}

		return lista;
	}

	public bool Agregar(Carrito obj, out string mensaje)
	{
		bool respuesta = false;
		mensaje = string.Empty;

		try
		{
			using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadenaConexion()))
			{
				oconexion.Open();

				// 1️⃣ Obtener stock
				SqlCommand cmdStock = new SqlCommand(
					"select stock from producto where id = @id_producto",
					oconexion);

				cmdStock.Parameters.AddWithValue("@id_producto", obj.oProducto.id);

				int stockDisponible = Convert.ToInt32(cmdStock.ExecuteScalar());

				// 2️⃣ Verificar si ya existe
				SqlCommand cmdExiste = new SqlCommand(
					"select cantidad from carrito where id_cliente = @id_cliente and id_producto = @id_producto",
					oconexion);

				cmdExiste.Parameters.AddWithValue("@id_cliente", obj.oCliente.id);
				cmdExiste.Parameters.AddWithValue("@id_producto", obj.oProducto.id);

				object resultado = cmdExiste.ExecuteScalar();

				int cantidadActual = resultado != null ? Convert.ToInt32(resultado) : 0;
				int cantidadTotal = cantidadActual + obj.cantidad;

				// 🚨 NUEVA LÓGICA PARA EL BOTÓN -
				if (cantidadTotal <= 0)
				{
					SqlCommand cmdDelete = new SqlCommand(
						"delete from carrito where id_cliente = @id_cliente and id_producto = @id_producto",
						oconexion);

					cmdDelete.Parameters.AddWithValue("@id_cliente", obj.oCliente.id);
					cmdDelete.Parameters.AddWithValue("@id_producto", obj.oProducto.id);

					cmdDelete.ExecuteNonQuery();

					return true;
				}

				// 3️⃣ Validar stock
				if (stockDisponible < cantidadTotal)
				{
					mensaje = "Stock insuficiente.";
					return false;
				}

				if (resultado != null)
				{
					// 4️⃣ Update
					SqlCommand cmdUpdate = new SqlCommand(
						"update carrito set cantidad = @cantidad where id_cliente = @id_cliente and id_producto = @id_producto",
						oconexion);

					cmdUpdate.Parameters.AddWithValue("@cantidad", cantidadTotal);
					cmdUpdate.Parameters.AddWithValue("@id_cliente", obj.oCliente.id);
					cmdUpdate.Parameters.AddWithValue("@id_producto", obj.oProducto.id);

					cmdUpdate.ExecuteNonQuery();
				}
				else
				{
					// 5️⃣ Insert
					SqlCommand cmdInsert = new SqlCommand(
						"insert into carrito (id_cliente, id_producto, cantidad) values (@id_cliente, @id_producto, @cantidad)",
						oconexion);

					cmdInsert.Parameters.AddWithValue("@id_cliente", obj.oCliente.id);
					cmdInsert.Parameters.AddWithValue("@id_producto", obj.oProducto.id);
					cmdInsert.Parameters.AddWithValue("@cantidad", obj.cantidad);

					cmdInsert.ExecuteNonQuery();
				}

				respuesta = true;
			}
		}
		catch (Exception ex)
		{
			mensaje = ex.Message;
			respuesta = false;
		}

		return respuesta;
	}

	public bool ActualizarCantidad(int idCarrito, int nuevaCantidad, out string mensaje)
	{
		bool respuesta = false;
		mensaje = string.Empty;

		try
		{
			using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadenaConexion()))
			{
				oconexion.Open();

				// Obtener producto y stock
				SqlCommand cmdInfo = new SqlCommand(
					@"select p.stock
                  from carrito c
                  inner join producto p on p.id = c.id_producto
                  where c.id = @idCarrito",
					oconexion);

				cmdInfo.Parameters.AddWithValue("@idCarrito", idCarrito);

				int stockDisponible = Convert.ToInt32(cmdInfo.ExecuteScalar());

				if (stockDisponible < nuevaCantidad)
				{
					mensaje = "Stock insuficiente.";
					return false;
				}

				SqlCommand cmdUpdate = new SqlCommand(
					"update carrito set cantidad = @cantidad where id = @id",
					oconexion);

				cmdUpdate.Parameters.AddWithValue("@cantidad", nuevaCantidad);
				cmdUpdate.Parameters.AddWithValue("@id", idCarrito);

				cmdUpdate.ExecuteNonQuery();

				respuesta = true;
			}
		}
		catch (Exception ex)
		{
			mensaje = ex.Message;
			respuesta = false;
		}

		return respuesta;
	}

	public bool Eliminar(int idCarrito, out string mensaje)
	{
		bool respuesta = false;
		mensaje = string.Empty;

		try
		{
			using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadenaConexion()))
			{
				SqlCommand cmd = new SqlCommand(
					"delete from carrito where id = @id",
					oconexion);

				cmd.Parameters.AddWithValue("@id", idCarrito);

				cmd.CommandType = CommandType.Text;

				oconexion.Open();
				cmd.ExecuteNonQuery();

				respuesta = true;
			}
		}
		catch (Exception ex)
		{
			respuesta = false;
			mensaje = ex.Message;
		}

		return respuesta;
	}

	public int CantidadEnCarrito(int idUsuario)
	{
		int cantidad = 0;

		try
		{
			using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadenaConexion()))
			{
				string query = "SELECT SUM(cantidad) FROM carrito WHERE id_cliente = @idUsuario";

				SqlCommand cmd = new SqlCommand(query, oconexion);
				cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

				oconexion.Open();
				cantidad = Convert.ToInt32(cmd.ExecuteScalar());
			}
		}
		catch (Exception)
		{
			cantidad = 0;
		}

		return cantidad;
	}
}
