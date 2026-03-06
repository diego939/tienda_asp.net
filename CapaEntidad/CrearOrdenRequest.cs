using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
	public class CrearOrdenRequest
	{
		public decimal Total { get; set; }
	}

	public class PayPalItem
	{
		public string name { get; set; }
		public PayPalAmount unit_amount { get; set; }
		public string quantity { get; set; }
	}

	public class PayPalAmount
	{
		public string currency_code { get; set; }
		public string value { get; set; }
	}
}
