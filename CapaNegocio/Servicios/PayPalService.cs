using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
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

		public async Task<string> CreateOrder(decimal total)
		{
			var accessToken = await GetAccessToken();

			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue("Bearer", accessToken);

				var order = new
				{
					intent = "CAPTURE",
					purchase_units = new[]
					{
						new {
							amount = new {
								currency_code = "USD",
								value = total.ToString("F2",
									System.Globalization.CultureInfo.InvariantCulture)
							}
						}
					}
				};

				var content = new StringContent(
					Newtonsoft.Json.JsonConvert.SerializeObject(order),
					Encoding.UTF8,
					"application/json");

				var response = await client.PostAsync(
					$"{_baseUrl}/v2/checkout/orders",
					content);

				var json = await response.Content.ReadAsStringAsync();
				var data = JObject.Parse(json);

				return data["id"]?.ToString();
			}
		}

		public async Task<bool> CaptureOrder(string orderId)
		{
			var accessToken = await GetAccessToken();

			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue("Bearer", accessToken);

				var response = await client.PostAsync(
					$"{_baseUrl}/v2/checkout/orders/{orderId}/capture",
					null);

				var json = await response.Content.ReadAsStringAsync();
				var data = JObject.Parse(json);

				return data["status"]?.ToString() == "COMPLETED";
			}
		}
	}
}