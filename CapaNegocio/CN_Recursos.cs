using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace CapaNegocio
{
    public class CN_Recursos
    {
        public static string ConvertirSha256(string texto)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));
                foreach (byte b in result)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        public static string GenerarClave()
        {
            string clave = Guid.NewGuid().ToString("N").Substring(0, 6);
            return clave;
        }

        public static bool EnviarCorreo(string correo, string asunto, string mensaje)
        {
            bool resultado = false;
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(correo);
                mail.From = new MailAddress("diegodavidalmiron1990@gmail.com");
                mail.Subject = asunto;
                mail.Body = mensaje;
                mail.IsBodyHtml = true;

                var smtp = new SmtpClient()
                {
                    Credentials = new NetworkCredential("diegodavidalmiron1990@gmail.com", "hvacazvhenwiqtqd"),
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true
                };
                smtp.Send(mail);
                resultado = true;

            }
            catch (Exception)
            {
                resultado = false;
            }
            return resultado;
        }

		//USAR MAS ADELANTE PARA CONVERTIR LA IMAGEN A BASE64 PARA GUARDARLA EN LA BASE DE DATOS
		public static string ConvertirBase64(string texto, out bool conversion)
        {
            string resultadoBase64 = string.Empty;
            conversion = true;
            try
            {
                byte[] textoBytes = Encoding.UTF8.GetBytes(texto);
                resultadoBase64 = Convert.ToBase64String(textoBytes);

            }
            catch (Exception)
            {
                conversion = false;
            }
            return resultadoBase64;
        }
    }
}
