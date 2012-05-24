namespace Echo.Web.Controls.Base
{
	public class EchoPage : System.Web.UI.Page
	{
		public EchoMasterPage EchoMasterPage
		{
			get { return (EchoMasterPage)Master; }
		}

		public string SubTitle { get; set; }

		protected override void OnLoad(System.EventArgs e)
		{
			base.OnLoad(e);
			if (IsPostBack == false)
			{
				DataBind();
			}
		}

		
	}
}