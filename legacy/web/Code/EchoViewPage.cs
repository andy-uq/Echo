using System.Web.UI;

using Echo.Web.Models;

namespace Echo.Web
{
	public class EchoViewPage : EchoViewPage<GenericModel>
	{
	}

	public class EchoViewPage<TModel> : System.Web.Mvc.ViewPage<TModel> where TModel : BaseModel
	{
		public EchoMasterPage<TModel> EchoMasterPage
		{
			get { return (EchoMasterPage<TModel>)Master; }
		}

		public string SubTitle { get; set; }
	}
}