using System;
using Echo.Celestial;
using Echo.Corporations;
using Echo.Items;
using Echo.Market;
using Echo.State;
using Echo.Structures;
using NUnit.Framework;
using Shouldly;

namespace Echo.Tests.Commerce
{
	[TestFixture]
	public class AuctionTests
	{
		private StarCluster _sc;
		private SolarSystem _sol;
		private TradingStation _tradingStation;
		private MarketPlace _marketPlace;

		[SetUp]
		public void SetUp()
		{
			_marketPlace = new MarketPlace();
			_sc = new StarCluster { MarketPlace = _marketPlace };
			_sol = new SolarSystem { Position = new Position(_sc, Vector.Zero) };
			_tradingStation = new TradingStation { Position = new Position(_sol, Vector.Zero) };

			_marketPlace.StarCluster = _sc;
			_marketPlace.ObjectType.ShouldBe(ObjectType.MarketPlace);
		}

		[Test]
		public void GetMarketPlace()
		{
			var a = new BuyOrder();
			a.MarketPlace.ShouldBe(null);

			a.Location = _tradingStation;
			a.MarketPlace.ShouldBe(_sc.MarketPlace);
		}

		[Test]
		public void OwnerIsSet()
		{
			var item = new Item(new ItemInfo(ItemCode.Veldnium)) {Owner = new Corporation(), Quantity = 10};
			var a = new BuyOrder {Item = item};
			a.Owner.ShouldBe(item.Owner);
		}

		[Test]
		public void AuctionQuantity()
		{
			var item = new Item(new ItemInfo(ItemCode.Veldnium)) {Owner = new Corporation(), Quantity = 10};
			var a = new BuyOrder { Item = item };
			a.Quantity.ShouldBe(item.Quantity);
		}

		[Test]
		public void ListAuctionWithNoMarketPlace()
		{
			var a = new BuyOrder();
			Should.Throw<ArgumentNullException>(() => a.List(null));
		}

		[Test]
		public void ListBuyOrder()
		{
			_sc.MarketPlace.AuctionLength = 10;

			var item = new Item(new ItemInfo(ItemCode.Veldnium)) { Owner = new Corporation(), Quantity = 10, Location = _sc };
			var a = new BuyOrder { Item = item, PricePerUnit = 120 };

			a.List(_sc.MarketPlace);

			_sc.MarketPlace.Auctions.ShouldContain(a);
			a.Expires.ShouldBe(_sc.MarketPlace.AuctionLength);
		}

		[Test]
		public void ListBuyOrderWithOpposingSellOrder()
		{
			var selling = new Item(new ItemInfo(ItemCode.Veldnium)) { Owner = new Corporation(), Quantity = 10, Location = _sc };
			var sellOrder = new SellOrder { Item = selling, Location = _tradingStation, PricePerUnit = 100 };

			_sc.MarketPlace.Add(sellOrder);
			_sc.MarketPlace.AuctionLength = 10;

			var item = new Item(new ItemInfo(ItemCode.Veldnium)) { Owner = new Corporation(), Quantity = 10, Location = _sc };
			var a = new BuyOrder { Item = item, Location = _tradingStation, PricePerUnit = 120 };

			var settlement = a.List(_sc.MarketPlace);

			settlement.Success.ShouldBe(true);
			settlement.Sum.ShouldBe(1000);
			settlement.Item.Quantity.ShouldBe(10u);

			_sc.MarketPlace.Auctions.ShouldNotContain(a);
			_sc.MarketPlace.Auctions.ShouldNotContain(sellOrder);
		}

		[Test]
		public void ChooseCheapestSellOrder()
		{
			var expensiveItem = new Item(new ItemInfo(ItemCode.Veldnium)) { Owner = new Corporation(), Quantity = 10, Location = _sc };
			var expensive = new SellOrder { Item = expensiveItem, Location = _tradingStation, PricePerUnit = 100 };

			var cheapItem = new Item(new ItemInfo(ItemCode.Veldnium)) { Owner = new Corporation(), Quantity = 10, Location = _sc };
			var cheapest = new SellOrder { Item = cheapItem, Location = _tradingStation, PricePerUnit = 80 };

			_sc.MarketPlace.Add(expensive);
			_sc.MarketPlace.Add(cheapest);
			_sc.MarketPlace.AuctionLength = 10;

			var item = new Item(new ItemInfo(ItemCode.Veldnium)) { Owner = new Corporation(), Quantity = 10, Location = _sc };
			var a = new BuyOrder { Item = item, Location = _tradingStation, PricePerUnit = 120 };

			var settlement = a.List(_sc.MarketPlace);

			settlement.Success.ShouldBe(true);
			settlement.Item.Quantity.ShouldBe(10u);
			settlement.Sum.ShouldBe(800);

			_sc.MarketPlace.Auctions.ShouldNotContain(a);
			_sc.MarketPlace.Auctions.ShouldNotContain(cheapest);
		}

		[Test]
		public void ListSellOrderWithCompatibleBlockSizes()
		{
			var selling = new Item(new ItemInfo(ItemCode.Veldnium)) { Owner = new Corporation(), Quantity = 150, Location = _sc };
			var sellOrder = new SellOrder { Item = selling, Location = _tradingStation, PricePerUnit = 100, BlockSize = 5 };

			_sc.MarketPlace.Add(sellOrder);
			_sc.MarketPlace.AuctionLength = 10;

			var item = new Item(new ItemInfo(ItemCode.Veldnium)) { Owner = new Corporation(), Quantity = 180, Location = _sc };
			var a = new BuyOrder { Item = item, Location = _tradingStation, PricePerUnit = 120, BlockSize = 3 };

			var settlement = a.List(_sc.MarketPlace);

			settlement.Success.ShouldBe(true);
			settlement.Sum.ShouldBe(150 * 100);
			settlement.Item.Quantity.ShouldBe(150u);

			_sc.MarketPlace.Auctions.ShouldContain(a);
			a.Item.Quantity.ShouldBe(30u);
		}

		[Test]
		public void ListBuyOrderWithPartialBuy()
		{
			var selling = new Item(new ItemInfo(ItemCode.Veldnium)) { Owner = new Corporation(), Quantity = 150, Location = _sc };
			var sellOrder = new SellOrder { Item = selling, Location = _tradingStation, PricePerUnit = 100, BlockSize = 10 };

			_sc.MarketPlace.Add(sellOrder);
			_sc.MarketPlace.AuctionLength = 10;

			var item = new Item(new ItemInfo(ItemCode.Veldnium)) { Owner = new Corporation(), Quantity = 200, Location = _sc };
			var a = new BuyOrder { Item = item, Location = _tradingStation, PricePerUnit = 120, BlockSize = 100 };

			var settlement = a.List(_sc.MarketPlace);

			settlement.Success.ShouldBe(true);
			settlement.Item.Quantity.ShouldBe(100u);
			settlement.Sum.ShouldBe(100 * 100);
			settlement.Timer = _sc.MarketPlace.SettlementDelay;

			_sc.MarketPlace.Auctions.ShouldContain(sellOrder);
			_sc.MarketPlace.Auctions.ShouldContain(a);
			_sc.MarketPlace.Settlements.ShouldContain(settlement);
		}

		[Test]
		public void ListSellOrderWithIncompatibleBlockSizes()
		{
			var selling = new Item(new ItemInfo(ItemCode.Veldnium)) { Owner = new Corporation(), Quantity = 150, Location = _sc };
			var sellOrder = new SellOrder { Item = selling, Location = _tradingStation, PricePerUnit = 100, BlockSize = 150 };

			_sc.MarketPlace.Add(sellOrder);
			_sc.MarketPlace.AuctionLength = 10;

			var item = new Item(new ItemInfo(ItemCode.Veldnium)) { Owner = new Corporation(), Quantity = 180, Location = _sc };
			var a = new BuyOrder { Item = item, Location = _tradingStation, PricePerUnit = 120, BlockSize = 100 };

			var settlement = a.List(_sc.MarketPlace);
			
			settlement.Success.ShouldBe(false);
			settlement.Sum.ShouldBe(0);
		}
	}
}