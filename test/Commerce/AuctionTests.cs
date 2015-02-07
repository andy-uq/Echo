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

			_marketPlace.ObjectType.ShouldBe(ObjectType.MarketPlace);
		}

		[Test]
		public void GetMarketPlace()
		{
			var a = new Auction();
			a.MarketPlace.ShouldBe(null);

			a.Location = _tradingStation;
			a.MarketPlace.ShouldBe(_sc.MarketPlace);
		}

		[Test]
		public void OwnerIsSet()
		{
			var item = new Item(new ItemInfo(ItemCode.Veldnium)) {Owner = new Corporation(), Quantity = 10};
			var a = new Auction {Item = item};
			a.Owner.ShouldBe(item.Owner);
		}

		[Test]
		public void AuctionQuantity()
		{
			var item = new Item(new ItemInfo(ItemCode.Veldnium)) {Owner = new Corporation(), Quantity = 10};
			var a = new Auction {Item = item};
			a.Quantity.ShouldBe(item.Quantity);
		}

		[Test]
		public void ListAuctionWithNoMarketPlace()
		{
			var a = new Auction();
			Should.Throw<ArgumentNullException>(() => a.List(null));
		}

		[Test]
		public void ListAuction()
		{
			_sc.MarketPlace.AuctionLength = 10;

			var a = new Auction();
			a.List(_sc.MarketPlace);

			_sc.MarketPlace.Auctions.ShouldContain(a);
			a.Expires.ShouldBe(_sc.MarketPlace.AuctionLength);
		}
	}
}