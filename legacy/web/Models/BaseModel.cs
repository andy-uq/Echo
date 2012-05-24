namespace Echo.Web.Models
{
	public abstract class BaseModel
	{
		public string Title { get; set; }
		public string SubTitle { get; set; }
	}

	public class GenericModel : BaseModel
	{
	}
}