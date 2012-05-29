using System;
using Echo.Corporations;

namespace Echo.Market
{
	public class Auction : IObject
	{
		private IItem _item;

		public Auction(MarketPlace marketPlace)
		{
			if ( marketPlace == null )
				throw new ArgumentNullException("marketPlace");

			this.MarketPlace = marketPlace;
			Expires = marketPlace.AuctionLength;
		}

		public ObjectType ObjectType
		{
			get { return ObjectType.Auction; }
		}

		public long Id { get; set; }
		public string Name { get; set; }
		public Corporation Owner { get; private set; }
		public MarketPlace MarketPlace { get; private set; }

		public IItem Item
		{
			get { return this._item; }
			set
			{
				this._item = value;
				this.Owner = value.Owner;
			}
		}

		public uint PricePerUnit { get; set; }
		public uint BlockSize { get; set; }
		public ulong Expires { get; set; }
		public uint Range { get; set; }

		public uint Quantity
		{
			get { return Item.Quantity; }
		}

		public void Tick(ulong tick)
		{
			throw new NotImplementedException();
		}
	}
}