using System;
using System.Linq;
using Echo.Items;

namespace Echo.Market
{
	public class BuyOrder : Auction
	{
		public override Settlement List(MarketPlace marketPlace)
		{
			if (marketPlace == null) throw new ArgumentNullException(nameof(marketPlace));
			
			if (!marketPlace.StarCluster.Equals(Item.Location.GetStarCluster()))
				throw new InvalidOperationException($"Unable to list {Item} in target marketplace because they don't exist int the same Star Cluster.");

			var settlement = new Settlement();

			foreach (var sellOrder in marketPlace.SellOrders.For(Item).OrderBy(x => x.PricePerUnit))
			{
				if (OutOfRange(sellOrder) || sellOrder.OutOfRange(this))
					continue;

				if (sellOrder.PricePerUnit > PricePerUnit)
					continue;

				if (sellOrder.BlockSize > Quantity)
					continue;

				if (BuyFrom(sellOrder, settlement))
					return marketPlace.Settle(settlement);
			}

			marketPlace.Add(this);
			return marketPlace.Settle(settlement);
		}

		private bool BuyFrom(SellOrder sellOrder, Settlement settlement)
		{
			if (!GetBlockSize(sellOrder, out var commonBlockSize))
				return Quantity == 0;

			var targetQuantity = Math.Min(Quantity, sellOrder.Quantity);
			var numberOfBlocks = targetQuantity/commonBlockSize;
			var buyQuantity = numberOfBlocks*commonBlockSize;

			var item = new Item(sellOrder.Item, buyQuantity, Owner);
			settlement.Add(sellOrder, buyQuantity * sellOrder.PricePerUnit, item);

			sellOrder.Item.Quantity -= buyQuantity;
			if (sellOrder.Item.Quantity == 0)
			{
				sellOrder.Remove();
			}

			Item.Quantity -= buyQuantity;
			return Quantity == 0;
		}

		private bool GetBlockSize(Auction auction, out uint blockSize)
		{
			blockSize = BlockSize;
			while (blockSize < Quantity && blockSize < auction.Quantity)
			{
				if (blockSize%auction.BlockSize == 0)
				{
					return true;
				}

				blockSize += BlockSize;
			}

			return false;
		}
	}
}