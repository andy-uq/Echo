

using System.Web;

namespace Echo.Web.Extensions
{
	public static class StringExtensions
	{
		public static IHtmlString ToJson(this object value)
		{
			var json = ServiceStack.Text.StringExtensions.ToJson(value);
			return new HtmlString(json);
		}
	}
}