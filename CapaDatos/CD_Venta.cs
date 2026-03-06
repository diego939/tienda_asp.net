using System;
using System.Collections.Generic;
using System.Data;
using CapaEntidad;
using Microsoft.Data.SqlClient;

namespace CapaDatos
{
	public class CD_Venta
	{
		public int Registrar(Venta obj, out string mensaje)
		{
			int idVentaGenerada = 0;
			mensaje = string.Empty;

			try
			{
				using (SqlConnection oconexion = new SqlConnection(Conexion.GetCadenaConexion()))
				{
					oconexion.Open();

					using (SqlTransaction tr = oconexion.BeginTransaction())
					{
						try
						{
							SqlCommand cmdVenta = new SqlCommand(@"
                                INSERT INTO venta
                                (id_cliente,total_productos,monto_total,contacto,telefono,direccion,id_departamento,id_transaccion, estado)
                                VALUES
                                (@id_cliente,@total_productos,@monto_total,@contacto,@telefono,@direccion,@id_departamento,@id_transaccion,@estado);
                                SELECT SCOPE_IDENTITY();", oconexion, tr);

							cmdVenta.CommandType = CommandType.Text;

							cmdVenta.Parameters.AddWithValue("@id_cliente", obj.oCliente.id);
							cmdVenta.Parameters.AddWithValue("@total_productos", obj.total_productos);
							cmdVenta.Parameters.AddWithValue("@monto_total", obj.monto_total);
							cmdVenta.Parameters.AddWithValue("@contacto", obj.contacto);
							cmdVenta.Parameters.AddWithValue("@telefono", obj.telefono);
							cmdVenta.Parameters.AddWithValue("@direccion", obj.direccion);
							cmdVenta.Parameters.AddWithValue("@id_departamento", obj.oDepartamento.id);
							cmdVenta.Parameters.AddWithValue("@id_transaccion", obj.id_transaccion);
							cmdVenta.Parameters.AddWithValue("@estado", obj.estado);

							idVentaGenerada = Convert.ToInt32(cmdVenta.ExecuteScalar());

							foreach (DetalleVenta dv in obj.oDetalleVenta)
							{
								SqlCommand cmdDetalle = new SqlCommand(@"
                                    INSERT INTO detalle_venta
                                    (id_venta,id_producto,cantidad,total)
                                    VALUES
                                    (@id_venta,@id_producto,@cantidad,@total)",
									oconexion, tr);

								cmdDetalle.CommandType = CommandType.Text;

								cmdDetalle.Parameters.AddWithValue("@id_venta", idVentaGenerada);
								cmdDetalle.Parameters.AddWithValue("@id_producto", dv.oProducto.id);
								cmdDetalle.Parameters.AddWithValue("@cantidad", dv.cantidad);
								cmdDetalle.Parameters.AddWithValue("@total", dv.total);

								cmdDetalle.ExecuteNonQuery();

								SqlCommand cmdStock = new SqlCommand(@"
                                    UPDATE producto 
                                    SET stock = stock - @cantidad 
                                    WHERE id = @id_producto",
									oconexion, tr);

								cmdStock.CommandType = CommandType.Text;

								cmdStock.Parameters.AddWithValue("@cantidad", dv.cantidad);
								cmdStock.Parameters.AddWithValue("@id_producto", dv.oProducto.id);

								cmdStock.ExecuteNonQuery();
							}

							SqlCommand cmdCarrito = new SqlCommand(
								"DELETE FROM carrito WHERE id_cliente = @id_cliente",
								oconexion, tr);

							cmdCarrito.CommandType = CommandType.Text;
							cmdCarrito.Parameters.AddWithValue("@id_cliente", obj.oCliente.id);
							cmdCarrito.ExecuteNonQuery();

							tr.Commit();
						}
						catch (Exception exTr)
						{
							tr.Rollback();
							idVentaGenerada = 0;
							mensaje = exTr.Message;
						}
					}
				}
			}
			catch (Exception ex)
			{
				idVentaGenerada = 0;
				mensaje = ex.Message;
			}

			return idVentaGenerada;
		}

		public List<Venta> ListarPorCliente(int idCliente)
		{
			List<Venta> lista = new List<Venta>();

			using (SqlConnection con = new SqlConnection(Conexion.cn))
			{
				string query = @"
        select id,id_transaccion,total_productos,
        monto_total,fecha_registro
        from venta
        where id_cliente = @idCliente";

				SqlCommand cmd = new SqlCommand(query, con);
				cmd.Parameters.AddWithValue("@idCliente", idCliente);

				con.Open();

				using (SqlDataReader dr = cmd.ExecuteReader())
				{
					while (dr.Read())
					{
						lista.Add(new Venta()
						{
							id = Convert.ToInt32(dr["id"]),
							id_transaccion = dr["id_transaccion"].ToString(),
							total_productos = Convert.ToInt32(dr["total_productos"]),
							monto_total = Convert.ToDecimal(dr["monto_total"]),
							fecha_registro = Convert.ToDateTime(dr["fecha_registro"])
						});
					}
				}
			}

			return lista;
		}

		public List<DetalleVenta> DetalleVenta(int idVenta)
		{
			List<DetalleVenta> lista = new List<DetalleVenta>();

			using (SqlConnection con = new SqlConnection(Conexion.cn))
			{
				string query = @"
        select p.nombre,dv.cantidad,dv.total
        from detalle_venta dv
        inner join producto p on p.id = dv.id_producto
        where dv.id_venta = @idVenta";

				SqlCommand cmd = new SqlCommand(query, con);
				cmd.Parameters.AddWithValue("@idVenta", idVenta);

				con.Open();

				using (SqlDataReader dr = cmd.ExecuteReader())
				{
					while (dr.Read())
					{
						lista.Add(new DetalleVenta()
						{
							oProducto = new Producto()
							{
								nombre = dr["nombre"].ToString()
							},
							cantidad = Convert.ToInt32(dr["cantidad"]),
							total = Convert.ToDecimal(dr["total"])
						});
					}
				}
			}

			return lista;
		}
	}
}