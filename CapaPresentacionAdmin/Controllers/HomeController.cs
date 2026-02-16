using System.Diagnostics;
using CapaPresentacionAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using CapaNegocio;
using CapaEntidad;
using System.Data;
using ClosedXML.Excel;
//AUTENTICACION METHODS -----------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Authorization;
//AUTENTICACION METHODS -----------------------------------------------------------------------------------------

namespace CapaPresentacionAdmin.Controllers
{

	[Authorize]
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

		[HttpPost]
		public JsonResult EliminarUsuario(int id)
		{
			bool response = false;
			string mensaje = string.Empty;


            try
            {
				response = new CN_Usuarios().Eliminar(id, out mensaje);
			}
            catch(Exception ex)
            {
                response = false;
                mensaje = ex.Message;
            }
            return Json(new { resultado = response, mensaje = mensaje });

		}

        [HttpGet]
        public JsonResult VistaDashboard()
        {
            CN_Reporte objCN_Reporte = new CN_Reporte();
            DashBoard dashboard = objCN_Reporte.VerDashboard();
            return Json(new { data = dashboard });
		}

		[HttpGet]
		public JsonResult ListaReporteVentas(string fechainicio, string fechafin, string idtransaccion)
		{
			List<Reporte> listaReporte = new List<Reporte>();
            listaReporte = new CN_Reporte().Ventas(fechainicio, fechafin, idtransaccion);
            return Json(new { data = listaReporte });

		}

        [HttpPost]
        public FileResult ExportarReporteVentas(string fechainicio, string fechafin, string idtransaccion)
        {
            List<Reporte> listaReporte = new List<Reporte>();
            listaReporte = new CN_Reporte().Ventas(fechainicio, fechafin, idtransaccion);
            
            DataTable dt = new DataTable();

            dt.Locale = new System.Globalization.CultureInfo("es-AR");

            dt.Columns.Add("Fecha Venta", typeof(string));
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Producto", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("Transacción", typeof(string));
            dt.Columns.Add("Fecha Exportación", typeof(string));
            
            foreach(Reporte rep in listaReporte)
            {
                dt.Rows.Add(new object[] { rep.fechaVenta, rep.cliente, rep.producto, rep.precio, rep.cantidad, rep.total, rep.transaccion, DateTime.Now.ToString("dd/MM/yyyy") });
			}

            dt.TableName = "ReporteVentas";

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteVentas"+ DateTime.Now.ToString("dd/MM/yyyy") + ".xlsx");
                }
			}
		}   

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
