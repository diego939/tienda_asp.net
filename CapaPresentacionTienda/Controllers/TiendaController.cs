using CapaDatos;
using CapaEntidad;
using CapaNegocio;
using CapaNegocio.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CapaPresentacionTienda.Controllers
{
	[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
	public class TiendaController : Controller
	{
		private readonly PayPalService _payPalService;
		// GET: TiendaController

		public TiendaController(PayPalService payPalService)
		{
			_payPalService = payPalService;
		}

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult DetalleProducto(int id = 0)
		{
			Producto? oProducto = new CN_Producto().Listar()
									.FirstOrDefault(p => p.id == id);

			if (oProducto == null)
				return View(null);

			bool conversion;

			string rutaFisica = Path.Combine(
				Directory.GetCurrentDirectory(),
				"wwwroot",
				oProducto.ruta_imagen.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString())
			);

			Console.WriteLine("Ruta física detalle: " + rutaFisica);
			Console.WriteLine("Existe archivo detalle: " + System.IO.File.Exists(rutaFisica));

			oProducto.base64 = System.IO.File.Exists(rutaFisica)
				? CN_Recursos.ConvertirBase64(rutaFisica, out conversion)
				: "";

			oProducto.extension = Path.GetExtension(oProducto.nombre_imagen);

			return View(oProducto);
		}

		public ActionResult Carrito()
		{
			if (User.Identity == null || !User.Identity.IsAuthenticated)
				return RedirectToAction("Index", "Acceso");

			return View();
		}

		[HttpGet]
		public JsonResult ListaCategorias()
		{
			List<Categoria> listaCategoria = new List<Categoria>();

			listaCategoria = new CN_Categoria().Listar();

			return Json(new { data = listaCategoria });

		}

		[HttpPost]
		public JsonResult ListarMarcaPorCategoria(int idCategoria)
		{
			List<Marca> listaMarca = new List<Marca>();

			listaMarca = new CN_Marca().ListarMarcaPorCategoria(idCategoria);

			return Json(new { data = listaMarca });

		}

		[HttpPost]
		public JsonResult ListarProducto(int idCategoria, int idMarca)
		{
			List<Producto> listaProducto = new List<Producto>();
			bool conversion;

			CN_Producto cnProducto = new CN_Producto();

			listaProducto = cnProducto.Listar()
				.Select(producto =>
				{
					// 🔥 Construimos la ruta física real
					string rutaFisica = Path.Combine(
						Directory.GetCurrentDirectory(),
						"wwwroot",
						producto.ruta_imagen.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString())
					);

					Console.WriteLine("Ruta física: " + rutaFisica);
					Console.WriteLine("Existe archivo: " + System.IO.File.Exists(rutaFisica));

					return new Producto()
					{
						id = producto.id,
						nombre = producto.nombre,
						descripcion = producto.descripcion,
						oMarca = producto.oMarca,
						oCategoria = producto.oCategoria,
						precio = producto.precio,
						stock = producto.stock,
						ruta_imagen = producto.ruta_imagen,
						nombre_imagen = producto.nombre_imagen,
						base64 = CN_Recursos.ConvertirBase64(rutaFisica, out conversion),
						extension = Path.GetExtension(producto.nombre_imagen),
						activo = producto.activo
					};
				})
				.Where(producto =>
					(idCategoria == 0 || producto.oCategoria.id == idCategoria) &&
					(idMarca == 0 || producto.oMarca.id == idMarca) &&
					producto.stock > 0 &&
					producto.activo)
				.ToList();

			var listaFinal = listaProducto.Select(p => new
			{
				p.id,
				p.nombre,
				p.descripcion,
				Marca = p.oMarca.descripcion,
				Categoria = p.oCategoria.descripcion,
				p.precio,
				p.stock,
				p.activo,
				p.base64,
				p.extension
			}).ToList();

			return Json(new { data = listaFinal });
		}

		[HttpPost]
		public JsonResult AgregarCarrito(int idProducto, int cantidad)
		{
			string mensaje = string.Empty;
			bool respuesta = false;

			if (!User.Identity.IsAuthenticated)
				return Json(new { respuesta = false, mensaje = "Debe iniciar sesión." });

			int idCliente = int.Parse(User.FindFirst("id").Value);

			Carrito obj = new Carrito()
			{
				oCliente = new Cliente() { id = idCliente },
				oProducto = new Producto() { id = idProducto },
				cantidad = cantidad
			};

			respuesta = new CN_Carrito().Agregar(obj, out mensaje);

			int nuevaCantidad = new CN_Carrito().CantidadEnCarrito(idCliente);

			return Json(new
			{
				respuesta = respuesta,
				mensaje = mensaje,
				cantidad = nuevaCantidad
			});
		}

		[HttpGet]
		public JsonResult ListarCarrito()
		{
			if (!User.Identity.IsAuthenticated)
				return Json(new { data = new List<object>() });

			int idCliente = int.Parse(User.FindFirst("id").Value);

			var lista = new CN_Carrito().Listar(idCliente);

			var listaFinal = lista.Select(c => new
			{
				c.id, // id del carrito
				idProducto = c.oProducto.id, // 👈 AGREGAR ESTO
				imagen = c.oProducto.ruta_imagen,
				producto = c.oProducto.nombre,
				c.cantidad,
				precio = c.oProducto.precio,
				total = c.cantidad * c.oProducto.precio
			}).ToList();

			return Json(new { data = listaFinal });
		}

		[HttpPost]
		public JsonResult EliminarCarrito(int idCarrito)
		{
			string mensaje = string.Empty;
			bool respuesta = new CN_Carrito().Eliminar(idCarrito, out mensaje);

			int idCliente = int.Parse(User.FindFirst("id").Value);
			int nuevaCantidad = new CN_Carrito().CantidadEnCarrito(idCliente);

			return Json(new
			{
				respuesta = respuesta,
				mensaje = mensaje,
				cantidad = nuevaCantidad
			});
		}

		[HttpGet]
		public JsonResult ObtenerCantidadCarrito()
		{
			int cantidad = 0;

			if (!User.Identity.IsAuthenticated)
				return Json(new { cantidad = 0 });

			var claimId = User.FindFirst("id");

			if (claimId == null)
				return Json(new { cantidad = 0 });

			int idCliente = int.Parse(claimId.Value);

			cantidad = new CN_Carrito().CantidadEnCarrito(idCliente);

			return Json(new { cantidad = cantidad });
		}

		[HttpGet]
		public JsonResult ListarPais()
		{
			var lista = new CN_Ubicacion().ListarPais();
			return Json(new { data = lista });
		}

		[HttpPost]
		public JsonResult ListarProvincia(int idPais)
		{
			var lista = new CN_Ubicacion().ListarProvincia(idPais);
			return Json(new { data = lista });
		}

		[HttpPost]
		public JsonResult ListarDepartamento(int idProvincia)
		{
			var lista = new CN_Ubicacion().ListarDepartamento(idProvincia);
			return Json(new { data = lista });
		}

		[HttpPost]
		public async Task<IActionResult> CrearOrdenPaypal([FromBody] CrearOrdenRequest request)
		{
			int idCliente = int.Parse(User.FindFirst("id").Value);

			// Traer carrito completo
			var carrito = new CN_Carrito().Listar(idCliente);

			if (carrito == null || carrito.Count == 0)
			{
				return Json(new { status = false, mensaje = "El carrito está vacío" });
			}

			// Mapear items para PayPal para que arme el detalle de la orden (evitamos diferencias con el total)
			var items = carrito.Select(c => new PayPalItem
			{
				name = c.oProducto.nombre,
				unit_amount = new PayPalAmount
				{
					currency_code = "USD",
					value = c.oProducto.precio.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)
				},
				quantity = c.cantidad.ToString()
			}).Cast<object>().ToList();

			// Total calculado desde carrito (evitamos diferencias)
			var itemTotal = carrito.Sum(c => c.oProducto.precio * c.cantidad);

			// Llamada al servicio con items y total
			var ordenId = await _payPalService.CreateOrder(itemTotal, items);

			if (string.IsNullOrEmpty(ordenId))
			{
				return Json(new { status = false, mensaje = "PayPal no devolvió un ID de orden" });
			}

			return Json(new
			{
				status = true,
				id = ordenId
			});
		}

		[HttpPost]
		public async Task<JsonResult> CapturarPaypal([FromBody] CapturarOrdenRequest request)
		{
			var pago = await _payPalService.CaptureOrder(request.orderId);

			if (pago == null)
				return Json(new { status = false, mensaje = "Pago no aprobado" });

			int idCliente = int.Parse(User.FindFirst("id").Value);

			var carrito = new CN_Carrito().Listar(idCliente);

			Venta oVenta = new Venta()
			{
				oCliente = new Cliente() { id = idCliente },
				contacto = request.contacto,
				telefono = request.telefono,
				direccion = request.direccion,
				oDepartamento = new Departamento() { id = request.idDepartamento },

				id_transaccion = pago.CaptureId, // ID real de PayPal
				estado = pago.Status,            // Estado real de PayPal

				total_productos = carrito.Sum(x => x.cantidad),
				monto_total = carrito.Sum(x => x.cantidad * x.oProducto.precio),

				oDetalleVenta = carrito.Select(c => new DetalleVenta()
				{
					oProducto = new Producto() { id = c.oProducto.id },
					cantidad = c.cantidad,
					total = c.cantidad * c.oProducto.precio
				}).ToList()
			};

			string mensaje;
			int resultado = new CN_venta().Registrar(oVenta, out mensaje);

			if (resultado == 0)
				return Json(new { status = false, mensaje = mensaje });

			return Json(new
			{
				status = true,
				idTransaccion = pago.CaptureId
			});
		}

		public IActionResult MisCompras()
		{

			return View();
		}

		[HttpGet]
		public JsonResult ListarMisCompras()
		{
			int idCliente = int.Parse(User.FindFirst("id").Value);

			List<Venta> lista = new CN_venta().ListarPorCliente(idCliente);

			return Json(new { data = lista });
		}

		[HttpGet]
		public JsonResult ObtenerDetalleVenta(int idVenta)
		{
			List<DetalleVenta> detalle = new CN_venta().DetalleVenta(idVenta);

			return Json(new { data = detalle });
		}

	}
}
