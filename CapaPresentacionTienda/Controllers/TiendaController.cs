using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacionTienda.Controllers
{
	[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
	public class TiendaController : Controller
	{
		// GET: TiendaController
		public ActionResult Index()
		{
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

	}
}
