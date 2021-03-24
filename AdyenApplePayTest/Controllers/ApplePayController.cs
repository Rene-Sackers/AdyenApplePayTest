
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AdyenApplePayTest.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AdyenApplePayTest.Controllers
{
	public class ApplePayController : Controller
	{
		private class AdyenApplePaySession
		{
			[JsonProperty("displayName")]
			public string DisplayName { get; set; }

			[JsonProperty("domainName")]
			public string DomainName { get; set; }

			[JsonProperty("merchantIdentifier")]
			public string MerchantIdentifier { get; set; }
		}

		private static async Task<string> CreateApplePaySession()
		{
			var settings = SettingsHelper.GetSettings();

			var client = new HttpClient();
			var model = new AdyenApplePaySession
			{
				DisplayName = settings.DisplayName,
				DomainName = settings.DomainName,
				MerchantIdentifier = settings.MerchantIdentifier
			};

			var requestContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
			var request = new HttpRequestMessage(HttpMethod.Post, "https://checkout-test.adyen.com/v64/applePay/sessions")
			{
				Content = requestContent,
				Headers =
				{
					{ "x-API-key", settings.AdyenApiKey }
				}
			};
			var response = await client.SendAsync(request);

			var jsonResponse = await response.Content.ReadAsStringAsync();
			var jObjectResponse = JObject.Parse(jsonResponse);
			var dataBlob = jObjectResponse["data"].Value<string>();

			return dataBlob;
		}

		[HttpGet]
		public async Task<ActionResult> ValidateMerchantAjax()
		{
			var sessionData = await CreateApplePaySession();

			return Content(sessionData);
		}
	}
}