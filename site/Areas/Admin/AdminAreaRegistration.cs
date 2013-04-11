using System.Web.Mvc;

namespace Echo.Web.Areas.Admin
{
	public class AdminAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "Admin";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.Routes.IgnoreRoute("{area}/{controller}/{name}.js", new {  });
			context.Routes.IgnoreRoute("{area}/{controller}/{action}/{name}.js", new {  });

			context.MapRoute(
				"Admin_default",
				"admin/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
