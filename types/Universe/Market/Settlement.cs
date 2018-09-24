using System.Collections.Generic;
using System.Linq;
using Echo.Corporations;
using Echo.Items;

namespace Echo.Market
{
	public partial class Settlement : IObject
	{
		private Dictionary<Corporation, long> _spendByOwner;
		private Item _item;

		public Settlement()
		{
			_spendByOwner = new Dictionary<Corporation, long>();
		}

		public Item Item => _item;
		public bool Success => _item != null;
		public long Sum => _spendByOwner.Sum(x => x.Value);
		public long Timer { get; set; }

		public void Add(Auction auction, long settlementPrice, Item item)
		{
			_spendByOwner.TryGetValue(auction.Owner, out var price);
			_spendByOwner[auction.Owner] = price + settlementPrice;

			if (_item == null)
			{
				_item = item;
			}
			else
			{
				_item.Quantity += item.Quantity;
			}
		}

		public ObjectType ObjectType => ObjectType.Settlement;
		public ulong Id { get; set; }
		public string Name { get; set; }
	}
}