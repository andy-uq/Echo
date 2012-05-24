using System;
using System.Collections.Generic;

using Echo.Entities;
using Echo.Events;
using Echo.Objects;

using Ubiquity.u2ool.Collections;
using Ubiquity.u2ool.Exceptions;

namespace Echo.Bank
{
	public class MarketPlace : BaseObject
	{
		private readonly List<Auction> auctions;
		private readonly StarCluster starCluster;

		public ReadOnlyList<Auction> Auctions
		{
			get { return this.auctions; }
		}

		public MarketPlace(StarCluster starCluster)
		{
			this.auctions = new List<Auction>();
			this.starCluster = starCluster;

			Location = starCluster;
			AuctionLength = 200;
		}

		public Auction CreateAuction(IItem item, uint pricePerUnit, uint blockSize)
		{
			if (item == null)
				throw new ArgumentNullException("item");

			if (item.Quantity % blockSize != 0)
				throw new InvalidOperationException("Block size needs to be a factor of the amount being sold.");

			var auction = new Auction(this);
			auction.BlockSize = blockSize;
			auction.Item = item;
			auction.PricePerUnit = pricePerUnit;

			this.auctions.Add(auction);
			return auction;
		}

		public uint AuctionLength
		{
			get; set;
		}

		public StarCluster StarCluster
		{
			get { return this.starCluster; }
		}

		public IItem Buy(Auction auction, uint quantity, Agent agent)
		{
			if (agent.Location.StarCluster != this.starCluster)
				throw new InvalidOperationException("The agent needs to be within the same star cluster as this market place");

			if (this.auctions.Contains(auction) == false)
				throw new ItemNotFoundException("Auction", auction);

			if (quantity % auction.BlockSize != 0)
				throw new InvalidOperationException("Need to purchase this item in {0:n0} increments.".Expand(auction.BlockSize));
			
			if (quantity > auction.Quantity)
				quantity = auction.Quantity;

			ulong totalPrice = quantity*auction.PricePerUnit;
			if (totalPrice > agent.Employer.Bank)
			{
				throw new ArgumentException("This corporation cannot afford to make this purchase");
			}
            
			IItem auctionItem = auction.Item;

			agent.Employer.Bank -= totalPrice;
			auctionItem.Owner.Bank += totalPrice;
            
			if (auction.Quantity == quantity)
			{
				this.auctions.Remove(auction);
			}
			else
			{
				auctionItem = auctionItem.Unstack(quantity);
			}

			auction.Owner.AuctionSold(auction);
			auctionItem.Owner = agent.Employer;

			Universe.EventPump.RaiseEvent(agent.Employer, EventType.AuctionBuy, "{0} bought {1} from {2}", agent.Name, auctionItem, auction.Owner);
			Universe.EventPump.RaiseEvent(auction.Owner, EventType.AuctionSold, "{0} bought {1}", agent.Name, auctionItem);

			return auctionItem;
		}

		public override ObjectType ObjectType
		{
			get { return ObjectType.MarketPlace; }
		}

		public override void Tick(ulong tick)
		{
			auctions.ForEach(a => a.Expires--);
			for ( int index = auctions.Count - 1; index >= 0; index-- )
			{
				var auction = this.auctions[index];
				if (auction.Expires > 0)
					continue;

				auction.Owner.AuctionExpired(auction);
				this.auctions.RemoveAt(index);
			}
		}
	}
}