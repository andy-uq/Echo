﻿namespace Echo.State
{
	public class ItemState : IObjectState
	{
		public long Id { get; set; }
		public string Name { get; set; }

		public uint Quantity { get; set; }
	}
}