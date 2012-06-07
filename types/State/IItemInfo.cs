using Echo.Items;

namespace Echo.State
{
	public interface IItemInfo : IObject
	{
		ItemCode Code { get; }
	}
}