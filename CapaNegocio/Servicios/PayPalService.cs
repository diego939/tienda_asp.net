using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace CapaNegocio.Servicios
{
	public class PayPalService
	{
		private readonly string _clientId;
		private readonly string _secret;
		private readonly string _baseUrl;

		public PayPalService(IConfiguration configuration)
		{
			_clientId = configuration["PayPal:ClientId"];
			_secret = configuration["PayPal:Secret"];
			_baseUrl = configuration["PayPal:BaseUrl"];
		}

		private async Task<string> GetAccessToken()
		{
			using (var client = new HttpClient())
			{
				var byteArray = Encoding.ASCII.GetBytes($"{_clientId}:{_secret}");

				client.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue("Basic",
						Convert.ToBase64String(byteArray));

				var content = new StringContent(
					"grant_type=client_credentials",
					Encoding.UTF8,
					"application/x-www-form-urlencoded");

				var response = await client.PostAsync(
					$"{_baseUrl}/v1/oauth2/token",
					content);

				var json = await response.Content.ReadAsStringAsync();
				var data = JObject.Parse(json);

				return data["access_token"]?.ToString();
			}
		}

		public async Task<string> CreateOrder(decimal total, List<object> items)
		{
			var accessToken = await GetAccessToken();

			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue("Bearer", accessToken);

				// Creamos la orden con purchase_units y detalle de items
				var order = new
				{
					intent = "CAPTURE",
					purchase_units = new[]
					{
				new
				{
					amount = new
					{
						currency_code = "USD",
						value = total.ToString("F2", System.Globalization.CultureInfo.InvariantCulture),
						breakdown = new
						{
							item_total = new
							{
								currency_code = "USD",
								value = total.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)
							}
						}
					},
					items = items
				}
			}
				};

				var content = new StringContent(
					Newtonsoft.Json.JsonConvert.SerializeObject(order),
					Encoding.UTF8,
					"application/json");

				var response = await client.PostAsync($"{_baseUrl}/v2/checkout/orders", content);
				var json = await response.Content.ReadAsStringAsync();

				Console.WriteLine("PAYPAL RESPONSE:");
				Console.WriteLine(json);

				var data = JObject.Parse(json);

				if (!response.IsSuccessStatusCode)
				{
					throw new Exception("Error creando orden PayPal: " + json);
				}

				return data["id"]?.ToString();
			}
		}

		public async Task<PayPalCaptureResult> CaptureOrder(string orderId)
		{
			var accessToken = await GetAccessToken();

			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue("Bearer", accessToken);

				var content = new StringContent("{}", Encoding.UTF8, "application/json");

				var response = await client.PostAsync(
					$"{_baseUrl}/v2/checkout/orders/{orderId}/capture",
					content);

				var json = await response.Content.ReadAsStringAsync();

				if (!response.IsSuccessStatusCode)
					throw new Exception($"PayPal error: {response.StatusCode} - {json}");

				var data = JObject.Parse(json);

				var status = data["status"]?.ToString();

				if (status != "COMPLETED")
					return null;

				var captureId = data["purchase_units"][0]["payments"]["captures"][0]["id"].ToString();

				return new PayPalCaptureResult
				{
					CaptureId = captureId,
					Status = status
				};
			}
		}
	}
}