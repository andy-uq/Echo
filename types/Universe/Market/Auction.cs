using System;
using Echo.Agents;
using Echo.Corporations;
using Echo.Items;
using Echo.Ships;
using Echo.Structures;

namespace Echo.Market
{
	public class Auction : IObject
	{
		private Item _item;

		public ObjectType ObjectType
		{
			get { return ObjectType.Auction; }
		}

		public long Id { get; set; }
		public string Name { get; set; }
		public Corporation Owner { get; private set; }
		public Agent Trader { get; set; }
		public Structure Location { get; set; }

		public MarketPlace MarketPlace
		{
			get { return Location.Position.GetMarketPlace(); }
		}

		public Item Item
		{
			get { return _item; }
			set
			{
				_item = value;
				Owner = value.Owner;
			}
		}

		public int PricePerUnit { get; set; }
		public int BlockSize { get; set; }
		public long Expires { get; set; }
		public int Range { get; set; }

		public uint Quantity
		{
			get { return Item.Quantity; }
		}
	}
}