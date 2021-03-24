using System.IO;
using System.Web;
using AdyenApplePayTest.Models;
using Newtonsoft.Json;

namespace AdyenApplePayTest.Helpers
{
	public static class SettingsHelper
	{
		private const string FileName = "settings.json";

		public static Settings GetSettings()
		{
			var path = HttpContext.Current.Server.MapPath($"~/{FileName}");
			return JsonConvert.DeserializeObject<Settings>(File.ReadAllText(path));
		}
	}
}