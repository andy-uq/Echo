﻿using System.Collections.Generic;
using System.Linq;
using Echo.Celestial;

namespace Echo.Market
{
	public partial class MarketPlace : IObject
	{
		private readonly AuctionCollection _auctions;
		private readonly List<Settlement> _settlements;

		public MarketPlace()
		{
			_auctions = new AuctionCollection();
			_settlements = new List<Settlement>();
		}

		public ObjectType ObjectType
		{
			get { return ObjectType.MarketPlace; }
		}

		public ulong Id { get; set; }
		public string Name { get; set; }

		public StarCluster StarCluster { get; set; }
		public AuctionCollection<BuyOrder> BuyOrders { get { return new AuctionCollection<BuyOrder>(_auctions); } }
		public AuctionCollection<SellOrder> SellOrders { get { return new AuctionCollection<SellOrder>(_auctions); } }
		public List<Settlement> Settlements { get { return _settlements; }}

		public long AuctionLength { get; set; }
		public long SettlementDelay { get; set; }

		public IEnumerable<Auction> Auctions
		{
			get { return _auctions; }
		}

		public void Add(Auction auction)
		{
			auction.Expires = AuctionLength;
			_auctions.Add(auction);
		}

		public void Remove(Auction auction)
		{
			_auctions.Remove(auction);
		}

		public Settlement Settle(Settlement settlement)
		{
			if (settlement.Success)
			{
				settlement.Timer = SettlementDelay;
				Settlements.Add(settlement);
			}

			return settlement;
		}
	}
}