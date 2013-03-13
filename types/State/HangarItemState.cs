using System.Collections.Generic;

namespace Echo.State
{
	public class HangarItemState
	{
		public ObjectReference Owner { get; set; }
		public IEnumerable<ItemState> Items { get; set; }
	}
}