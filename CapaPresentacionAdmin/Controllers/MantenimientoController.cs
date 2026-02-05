using CapaEntidad;
using CapaNegocio;
using Microsoft.AspNetCore.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class MantenimientoController : Controller
    {
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

	}
}
