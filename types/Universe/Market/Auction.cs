using Echo.Agents;
using Echo.Corporations;
using Echo.Items;
using Echo.Structures;

namespace Echo.Market
{
	public abstract partial class Auction : IObject
	{
		private Item _item;
		private uint _blockSize;

		public ObjectType ObjectType => ObjectType.Auction;

		public ulong Id { get; set; }
		public string Name { get; set; }
		public Corporation Owner { get; private set; }
		public Agent Trader { get; set; }
		public Structure Location { get; set; }

		public MarketPlace MarketPlace => Location?.Position.GetMarketPlace();

		public Item Item
		{
			get => _item;
			set
			{
				_item = value;
				Owner = value.Owner;
			}
		}

		public int PricePerUnit { get; set; }

		public uint BlockSize
		{
			get => _blockSize < 1 ? 1 : _blockSize;
			set => _blockSize = value;
		}

		public long Expires { get; set; }
		public double Range { get; set; }

		public uint Quantity => Item.Quantity;

		public bool OutOfRange(Auction auction)
		{
			var vector = Location.Position - auction.Location.Position;
			return vector.Magnitude > Range;
		}

		public void Remove()
		{
			MarketPlace.Remove(this);
		}

		public abstract Settlement List(MarketPlace marketPlace);
	}
}