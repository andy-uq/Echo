using System.Collections.Generic;
using Echo.Celestial;

namespace Echo.Market
{
	public class MarketPlace : IObject
	{
		public MarketPlace()
		{
			Auctions = new List<Auction>();
		}

		public ObjectType ObjectType
		{
			get { return ObjectType.MarketPlace; }
		}

		public long Id { get; set; }
		public string Name { get; set; }

		public StarCluster StarCluster { get; set; }
		public List<Auction> Auctions { get; set; }

		public ulong AuctionLength { get; set; }

		public void Add(Auction auction)
		{
			auction.Expires = (long )AuctionLength;
			Auctions.Add(auction);
		}
	}
}