using System;

using Echo.Entities;
using Echo.Objects;

namespace Echo.Bank
{
	public class Auction : BaseObject
	{
		private readonly MarketPlace marketPlace;

		private IItem item;
		private Corporation owner;

		public Auction(MarketPlace marketPlace)
		{
			if (marketPlace == null)
				throw new ArgumentNullException("marketPlace");

			this.marketPlace = marketPlace;
			Expires = marketPlace.AuctionLength;
		}

		public IItem Item
		{
			get { return this.item; }
			set
			{
				this.item = value;
				this.owner = value.Owner;
			}
		}

		public uint PricePerUnit { get; set; }

		public uint BlockSize { get; set; }

		public ulong Expires { get; set; }

		public uint Quantity
		{
			get { return Item.Quantity; }
		}

		public Corporation Owner
		{
			get { return this.owner; }
		}

		public MarketPlace MarketPlace
		{
			get { return this.marketPlace; }
		}

		public override ObjectType ObjectType
		{
			get { return ObjectType.Auction; }
		}
	}
}