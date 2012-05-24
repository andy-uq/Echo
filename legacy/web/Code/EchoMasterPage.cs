using System.Web.Mvc;

using Echo.Web.Models;

namespace Echo.Web
{
	public class EchoMasterPage : EchoMasterPage<GenericModel>
	{
	}

	public class EchoMasterPage<TModel> : ViewMasterPage<TModel> where TModel : BaseModel
	{
		public EchoViewPage<TModel> EchoPage
		{
			get { return (EchoViewPage<TModel>) Page; }
		}

		public string Title
		{
			get { return string.IsNullOrEmpty(EchoPage.Title) ? "Echo" : EchoPage.Title; } 
		}

		public string SubTitle
		{
			get { return EchoPage.SubTitle; }
		}
	}
}