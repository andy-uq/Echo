using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Echo.Items;
using EnsureThat;

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
			Ensure.That(auction.PricePerUnit, "pricePerUnit").IsGt(0);
			Ensure.That(auction.Quantity, "quantity").IsGt(0u);

			_auctions.Add(auction);
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
			return this._auctions.GetEnumerator();
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