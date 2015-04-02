using System.Collections.Generic;
using System.Linq;
using Echo.Corporations;
using Echo.Items;

namespace Echo.Market
{
	public partial class Settlement
	{
		private readonly Dictionary<Corporation, long> _spendByOwner;
		private Item _item;

		public Settlement()
		{
			_spendByOwner = new Dictionary<Corporation, long>();
		}

		public Item Item { get { return _item; } }
		public bool Success { get { return _item != null; } }
		public long Sum { get { return _spendByOwner.Sum(x => x.Value); } }
		public long Timer { get; set; }

		public void Add(Auction auction, long settlementPrice, Item item)
		{
			long price;
			_spendByOwner.TryGetValue(auction.Owner, out price);
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
	}
}