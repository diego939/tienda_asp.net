using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class Conexion
    {
		public static string cn = string.Empty;

		public static void SetCadenaConexion(string cadena)
		{
			cn = cadena;
		}

		public static string GetCadenaConexion()
		{
			return cn;
		}
	}
}
