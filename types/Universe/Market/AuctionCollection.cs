using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Echo.Items;

namespace Echo.Market
{
	public class AuctionCollection : IEnumerable<Auction>
	{
		private readonly List<Auction> _auctions;

		public AuctionCollection()
		{
			_auctions = new List<Auction>();
		}

		public AuctionCollection(IEnumerable<Auction> auctions)
		{
			_auctions = auctions.ToList();
		}


		public IEnumerator<Auction> GetEnumerator()
		{
			return _auctions.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(Auction auction)
		{
			if (auction.PricePerUnit <= 0)
				throw new ArgumentOutOfRangeException(nameof(auction), "Price per unit must be > 0");

			if (auction.Quantity <= 0)
				throw new ArgumentOutOfRangeException(nameof(auction), "Quantity must be > 0");

			_auctions.Add(auction);
		}

		public void AddRange(IEnumerable<Auction> auctions)
		{
			_auctions.AddRange(auctions);
		}

		public void Remove(Auction auction)
		{
			_auctions.Remove(auction);
		}
	}

	public class AuctionCollection<T> : IEnumerable<T>
		where T : Auction
	{
		private readonly IEnumerable<T> _auctions;

		public AuctionCollection(IEnumerable<Auction> auctions)
		{
			_auctions = auctions.OfType<T>();
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _auctions.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerable<T> For(Item item)
		{
			return
				from auction in _auctions
				where ObjectEqualityComparer.Equals(auction.Item, item)
				orderby auction.PricePerUnit
				select auction;
		}
	}
}