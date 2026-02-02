using System.Diagnostics;
using CapaPresentacionAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using CapaNegocio;
using CapaEntidad;

namespace CapaPresentacionAdmin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Usuarios()
        {
            return View();
        }
        [HttpGet]
        public JsonResult ListarUsuarios()
        {
            List<Usuario> lista = new List<Usuario>();
            try
            {
                CN_Usuarios objCN_Usuarios = new CN_Usuarios();
                lista = objCN_Usuarios.Listar();
            }
            catch (Exception ex)
            {
				ex.Message.ToString();
			}
            return Json(new { data = lista });
		}
        [HttpPost]
        public JsonResult GuardarUsuario([FromBody] Usuario objUsuario) { 
            object? resultado;
            string mensaje = string.Empty;
            try
            {
                CN_Usuarios objCN_Usuarios = new CN_Usuarios();
                if (objUsuario.id == 0)
                {
                    // Nuevo usuario
                    resultado = objCN_Usuarios.Registrar(objUsuario, out mensaje);
                }
                else
                {
                    // Editar usuario
                    resultado = objCN_Usuarios.Editar(objUsuario, out mensaje);
                }
            }
            catch (Exception ex)
            {
                resultado = null;
                mensaje = ex.Message;
			}
            return Json(new { resultado = resultado, mensaje = mensaje });
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
