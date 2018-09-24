using System.IO;
using System.Web;
using System.Web.Routing;

namespace Echo.Web.Controllers
{
	public class AssetHandler : IHttpHandler
	{
		public void ProcessRequest(HttpContext context)
		{
			var wrappedContext = new HttpContextWrapper(context);
			var routeData = RouteTable.Routes.GetRouteData(wrappedContext);

			var path = wrappedContext.Request.Path;
			if (routeData != null)
			{
				var assetName = Path.GetFileName(wrappedContext.Request.Path);

				var controller = routeData.Values["controller"];
				object area;
				if (routeData.Values.TryGetValue("area", out area))
				{
					path = string.Format("~/areas/{0}/views/{1}/{2}", area, controller, assetName);
				}
				else
				{
					path = string.Format("~/views/{0}/{1}", controller, assetName);
				}
			}

			var fileName = wrappedContext.Server.MapPath(path);
			if (File.Exists(fileName))
			{
				wrappedContext.Response.ContentType = "text/javascript";
				wrappedContext.Response.WriteFile(fileName);
				return;
			}

			wrappedContext.Response.StatusCode = 404;
		}

		public bool IsReusable { get { return true; } }
	}
}