using System.Security.Claims;
using CapaEntidad;
using CapaNegocio;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace CapaPresentacionTienda.Controllers
{
	public class AccesoController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Registrar()
		{
			return View();
		}

		public IActionResult CambiarClave()
		{
			return View();
		}

		public IActionResult Reestablecer()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Index(string correo, string clave)
		{
			Cliente? oCliente = new Cliente();
			oCliente = new CN_Cliente().Listar()
			.Where(c => c.correo.Trim().ToLower() == correo.Trim().ToLower()
			&& u.clave == CN_Recursos.ConvertirSha256(clave))
			.FirstOrDefault();

			string hashIngresado = CN_Recursos.ConvertirSha256(clave);
			Console.WriteLine("HASH INGRESADO: " + hashIngresado);
			if (oCliente == null)
			{
				ViewBag.ErrorMessage = "Email o contraseña incorrectos";
				return View();
			}
			else
			{
				if (oCliente.reestablecer == true)
				{
					TempData["id"] = oCliente.id;
					return RedirectToAction("CambiarClave");
				}
				else
				{
					//AUTENTICACION METHODS -----------------------------------------------------------------------------------------
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, oCliente.correo),
						new Claim("id", oCliente.id.ToString())
					};

					var identity = new ClaimsIdentity(
						claims,
						CookieAuthenticationDefaults.AuthenticationScheme);

					var principal = new ClaimsPrincipal(identity);

					HttpContext.SignInAsync(
						CookieAuthenticationDefaults.AuthenticationScheme,
						principal
					).Wait();
					//AUTENTICACION METHODS -----------------------------------------------------------------------------------------
					return RedirectToAction("Index", "Tienda");
				}

			}
		}

		[HttpPost]
		public IActionResult Registrar(Cliente objetoCliente)
		{
			int resultado;
			string mensaje = string.Empty;

			ViewData["nombre"] = string.IsNullOrEmpty(objetoCliente.nombre) ? "" : objetoCliente.nombre;
			ViewData["apellidos"] = string.IsNullOrEmpty(objetoCliente.apellidos) ? "" : objetoCliente.apellidos;
			ViewData["correo"] = string.IsNullOrEmpty(objetoCliente.correo) ? "" : objetoCliente.correo;

			if (objetoCliente.clave != objetoCliente.confirmarClave)
			{
				ViewBag.ErrorMensaje = "Las contraseñas no coinciden";
				return View();
			}
			else
			{
				resultado = new CN_Cliente().Registrar(objetoCliente, out mensaje);
				if (resultado > 0)
				{
					ViewBag.ErrorMensaje = null;
					return RedirectToAction("Index", "Acceso");
				}
				else
				{
					ViewBag.ErrorMensaje = mensaje;
					return View();
				}
			}
		}

		[HttpPost]
		public IActionResult CambiarClave(string id, string claveactual, string nuevaclave, string confirmarclave)
		{
			Cliente? oCliente = new Cliente();
			oCliente = new CN_Cliente().Listar()
			.Where(u => u.id == int.Parse(id))
			.FirstOrDefault();

			if (oCliente.clave != CN_Recursos.ConvertirSha256(claveactual))
			{
				TempData["id"] = id;
				ViewData["clave"] = "";
				ViewBag.ErrorMessage = "Contraseña actual es incorrecta";
				return View();
			}
			else if (nuevaclave != confirmarclave)
			{
				TempData["id"] = id;
				ViewData["clave"] = claveactual;
				ViewBag.ErrorMessage = "Las contraseñas nuevas no coinciden";
				return View();
			}
			nuevaclave = CN_Recursos.ConvertirSha256(nuevaclave);
			string mensaje = string.Empty;

			bool respuesta = new CN_Cliente().CambiarClave(int.Parse(id), nuevaclave, out mensaje);

			if (respuesta)
			{
				return RedirectToAction("Index");
			}
			else
			{
				TempData["id"] = id;
				ViewBag.ErrorMessage = mensaje;
				return View();
			}
		}

		[HttpPost]
		public IActionResult Reestablecer(string correo)
		{
			Cliente? oCliente = new Cliente();
			oCliente = new CN_Cliente().Listar()
			.Where(u => u.id == int.Parse(id))
			.FirstOrDefault();

			if (oCliente == null)
			{
				ViewBag.ErrorMessage = "No se encontro un usuario con ese correo";
				return View();
			}

			string mensaje = string.Empty;

			bool respuesta = new CN_Cliente().ReestablecerClave(oCliente.id, correo, out mensaje);

			if (respuesta)
			{
				ViewBag.Error = null;
				return RedirectToAction("Index", "Acceso");
			}
			else
			{
				ViewBag.ErrorMessage = mensaje;
				return View();
			}
		}

		public ActionResult CerrarSesion()
		{
			//AUTENTICACION METHODS -----------------------------------------------------------------------------------------
			HttpContext.SignOutAsync(
				CookieAuthenticationDefaults.AuthenticationScheme
			).Wait();
			//AUTENTICACION METHODS -----------------------------------------------------------------------------------------
			return RedirectToAction("Index", "Acceso");
		}
	}
}
