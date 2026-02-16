using CapaEntidad;
using CapaNegocio;
using Microsoft.AspNetCore.Mvc;
//AUTENTICACION METHODS -----------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
//AUTENTICACION METHODS -----------------------------------------------------------------------------------------

namespace CapaPresentacionAdmin.Controllers
{
	public class AccesoController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult CambiarClave()
		{
			return View();
		}
		public IActionResult ReestablecerClave()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Index(string correo, string clave)
		{
			Usuario? oUsuario = new Usuario();
			oUsuario = new CN_Usuarios().Listar()
			.Where(u => u.correo.Trim().ToLower() == correo.Trim().ToLower()
			&& u.clave == CN_Recursos.ConvertirSha256(clave))
			.FirstOrDefault();

			string hashIngresado = CN_Recursos.ConvertirSha256(clave);
			Console.WriteLine("HASH INGRESADO: " + hashIngresado);
			if (oUsuario == null)
			{
				ViewBag.ErrorMessage = "Email o contraseña incorrectos";
				return View();
			}
			else
			{
				if (oUsuario.restablecer == true)
				{
					TempData["id"] = oUsuario.id;
					TempData["clave"] = oUsuario.clave;
					TempData["correo"] = oUsuario.correo;
					return RedirectToAction("CambiarClave");
				}
				else
				{
					//AUTENTICACION METHODS -----------------------------------------------------------------------------------------
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, oUsuario.correo),
						new Claim("IdUsuario", oUsuario.id.ToString())
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

					return RedirectToAction("Index", "Home");
				}

			}
		}

		[HttpPost]
		public IActionResult CambiarClave(string id, string claveactual, string nuevaclave, string confirmarclave)
		{
			Usuario? oUsuario = new Usuario();
			oUsuario = new CN_Usuarios().Listar()
			.Where(u => u.id == int.Parse(id))
			.FirstOrDefault();

			if (oUsuario.clave != CN_Recursos.ConvertirSha256(claveactual))
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

			bool respuesta = new CN_Usuarios().CambiarClave(int.Parse(id), nuevaclave, out mensaje);

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
		public IActionResult ReestablecerClave( string correo)
		{
			Usuario? oUsuario = new Usuario();
			oUsuario = new CN_Usuarios().Listar()
			.Where(u => u.correo == correo)
			.FirstOrDefault();

			if (oUsuario == null)
			{
				ViewBag.ErrorMessage = "No se encontro un usuario con ese correo";
				return View();
			}

			string mensaje = string.Empty;

			bool respuesta = new CN_Usuarios().ReestablecerClave(oUsuario.id, correo, out mensaje);

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
