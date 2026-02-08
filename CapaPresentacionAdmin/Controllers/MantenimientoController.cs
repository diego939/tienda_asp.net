using System.Globalization;
using CapaEntidad;
using CapaNegocio;
using Microsoft.AspNetCore.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class MantenimientoController : Controller
    {
		private readonly IWebHostEnvironment _env;

		public MantenimientoController(IWebHostEnvironment env)
		{
			_env = env;
		}
		public IActionResult Categorias()
        {
            return View();
        }
		public IActionResult Marcas()
		{
			return View();
		}
        public IActionResult Productos()
        {
            return View();
        }

		[HttpGet]
		public JsonResult ListarCategorias()
		{
			List<Categoria> lista = new List<Categoria>();
			try
			{
				CN_Categoria objCN_Categorias = new CN_Categoria();
				lista = objCN_Categorias.Listar();
			}
			catch (Exception ex)
			{
				ex.Message.ToString();
			}
			return Json(new { data = lista });
		}
		[HttpPost]
		public JsonResult GuardarCategoria([FromBody] Categoria objCategoria)
		{
			object? resultado;
			string mensaje = string.Empty;
			try
			{
				CN_Categoria objCN_Categorias = new CN_Categoria();
				if (objCategoria.id == 0)
				{
					// Nueva categoria
					resultado = objCN_Categorias.Registrar(objCategoria, out mensaje);
				}
				else
				{
					// Editar categoria
					resultado = objCN_Categorias.Editar(objCategoria, out mensaje);
				}
			}
			catch (Exception ex)
			{
				resultado = null;
				mensaje = ex.Message;
			}
			return Json(new { resultado = resultado, mensaje = mensaje });
		}

		[HttpPost]
		public JsonResult EliminarCategoria(int id)
		{
			bool response = false;
			string mensaje = string.Empty;


			try
			{
				response = new CN_Categoria().Eliminar(id, out mensaje);
			}
			catch (Exception ex)
			{
				response = false;
				mensaje = ex.Message;
			}
			return Json(new { resultado = response, mensaje = mensaje });

		}

		[HttpGet]
		public JsonResult ListarMarcas()
		{
			List<Marca> lista = new List<Marca>();

			try
			{
				CN_Marca objCN_Marca = new CN_Marca();
				lista = objCN_Marca.Listar();
			}
			catch (Exception ex)
			{
				ex.Message.ToString();
			}

			return Json(new { data = lista });
		}

		[HttpPost]
		public JsonResult GuardarMarca([FromBody] Marca objMarca)
		{
			object? resultado;
			string mensaje = string.Empty;

			try
			{
				CN_Marca objCN_Marca = new CN_Marca();

				if (objMarca.id == 0)
				{
					// Nueva marca
					resultado = objCN_Marca.Registrar(objMarca, out mensaje);
				}
				else
				{
					// Editar marca
					resultado = objCN_Marca.Editar(objMarca, out mensaje);
				}
			}
			catch (Exception ex)
			{
				resultado = null;
				mensaje = ex.Message;
			}

			return Json(new { resultado = resultado, mensaje = mensaje });
		}

		[HttpPost]
		public JsonResult EliminarMarca(int id)
		{
			bool response = false;
			string mensaje = string.Empty;

			try
			{
				response = new CN_Marca().Eliminar(id, out mensaje);
			}
			catch (Exception ex)
			{
				response = false;
				mensaje = ex.Message;
			}

			return Json(new { resultado = response, mensaje = mensaje });
		}

		[HttpGet]
		public JsonResult ListarProductos()
		{
			List<Producto> lista = new List<Producto>();

			try
			{
				CN_Producto objCN_Producto = new CN_Producto();
				lista = objCN_Producto.Listar();
			}
			catch (Exception ex)
			{
				ex.Message.ToString();
			}

			return Json(new { data = lista });
		}

		[HttpPost]
		public JsonResult GuardarProducto(string objeto, IFormFile archivoImagen)
		{
			bool operacionExitosa = true;
			string mensaje = string.Empty;

			try
			{
				// 1️ Deserializar producto
				Producto? objProducto = Newtonsoft.Json.JsonConvert
					.DeserializeObject<Producto>(objeto);

				// 2️ Validar y convertir precio
				if (!decimal.TryParse(
					objProducto.precioTexto,
					System.Globalization.NumberStyles.AllowDecimalPoint,
					new CultureInfo("es-AR"),
					out decimal precio))
				{
					return Json(new
					{
						resultado = false,
						mensaje = "Precio inválido. Use formato 1234,56"
					});
				}

				objProducto.precio = precio;

				CN_Producto objCN_Producto = new CN_Producto();

				// 3️ Registrar o editar producto
				if (objProducto.id == 0)
				{
					int idGenerado = objCN_Producto.Registrar(objProducto, out mensaje);

					if (idGenerado == 0)
					{
						return Json(new { resultado = false, mensaje });
					}

					objProducto.id = idGenerado;
				}
				else
				{
					operacionExitosa = objCN_Producto.Editar(objProducto, out mensaje);

					if (!operacionExitosa)
					{
						return Json(new { resultado = false, mensaje });
					}
				}

				// 4️ Guardar imagen si existe
				if (archivoImagen != null && archivoImagen.Length > 0)
				{
					string extension = Path.GetExtension(archivoImagen.FileName).ToLower();
					string[] extensionesPermitidas = { ".jpg", ".jpeg", ".png" };

					if (!extensionesPermitidas.Contains(extension))
					{
						return Json(new
						{
							resultado = false,
							mensaje = "Solo se permiten imágenes JPG o PNG"
						});
					}

					string nombreImagen = $"producto_{objProducto.id}{extension}";

					string carpetaImagenes = Path.Combine(
						_env.WebRootPath,
						"imagenes",
						"productos"
					);

					if (!Directory.Exists(carpetaImagenes))
						Directory.CreateDirectory(carpetaImagenes);

					string rutaFisica = Path.Combine(carpetaImagenes, nombreImagen);

					using (FileStream stream = new FileStream(rutaFisica, FileMode.Create))
					{
						archivoImagen.CopyTo(stream);
					}

					string rutaWeb = $"/imagenes/productos/{nombreImagen}";

					bool imagenGuardada = objCN_Producto.GuardarImagen(new Producto
					{
						id = objProducto.id,
						ruta_imagen = rutaWeb,
						nombre_imagen = nombreImagen
					}, out mensaje);

					if (!imagenGuardada)
					{
						return Json(new
						{
							resultado = false,
							mensaje = "Producto guardado, pero falló la imagen: " + mensaje
						});
					}

					objProducto.ruta_imagen = rutaWeb;
					objProducto.nombre_imagen = nombreImagen;

					objCN_Producto.Editar(objProducto, out mensaje);
				}

				return Json(new
				{
					resultado = true,
					mensaje = "Producto guardado correctamente"
				});
			}
			catch (Exception ex)
			{
				return Json(new
				{
					resultado = false,
					mensaje = "Error inesperado: " + ex.Message
				});
			}
		}


		[HttpPost]
		public JsonResult EliminarProducto(int id)
		{
			bool response = false;
			string mensaje = string.Empty;

			try
			{
				response = new CN_Producto().Eliminar(id, out mensaje);
			}
			catch (Exception ex)
			{
				response = false;
				mensaje = ex.Message;
			}

			return Json(new { resultado = response, mensaje = mensaje });
		}

		[HttpPost]
		public JsonResult GuardarImagenProducto(int id, string ruta_imagen, string nombre_imagen)
		{
			bool response = false;
			string mensaje = string.Empty;

			try
			{
				Producto obj = new Producto()
				{
					id = id,
					ruta_imagen = ruta_imagen,
					nombre_imagen = nombre_imagen
				};

				response = new CN_Producto().GuardarImagen(obj, out mensaje);
			}
			catch (Exception ex)
			{
				response = false;
				mensaje = ex.Message;
			}

			return Json(new { resultado = response, mensaje = mensaje });
		}

		//USAR MAS ADELANTE PARA CONVERTIR LA IMAGEN A BASE64 PARA GUARDARLA EN LA BASE DE DATOS
		[HttpPost]
		public JsonResult ConvertirImagenBase64(int id)
		{
			bool conversion = false;

			Producto? oProducto = new CN_Producto()
				.Listar()
				.FirstOrDefault(p => p.id == id);

			if (oProducto == null)
			{
				return Json(new
				{
					conversion = false,
					textoBase64 = "",
					extension = ""
				});
			}

			string textoBase64 = CN_Recursos.ConvertirBase64(
				Path.Combine(oProducto.ruta_imagen, oProducto.nombre_imagen),
				out conversion
			);

			return Json(new
			{
				conversion = conversion,
				textoBase64 = textoBase64,
				extension = oProducto.nombre_imagen
			});
		}

	}
}
