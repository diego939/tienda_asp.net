using System;
using System.Collections.Generic;
using CapaEntidad;

public class Venta
{
	public int id { get; set; }
	public Cliente oCliente { get; set; }
	public int total_productos { get; set; }
	public decimal monto_total { get; set; }
	public string contacto { get; set; }
	public string telefono { get; set; }
	public string direccion { get; set; }

	public Departamento oDepartamento { get; set; }

	public string id_transaccion { get; set; }
	public DateTime fecha_registro { get; set; }

	public List<DetalleVenta> oDetalleVenta { get; set; }
}