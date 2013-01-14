using System;
using Echo.Celestial;
using Echo.Corporations;
using Echo.Items;
using Echo.Market;
using Echo.Structures;
using NUnit.Framework;

namespace Echo.Tests.Commerce
{
	[TestFixture]
	public class AuctionTests
	{
		private StarCluster _sc;
		private SolarSystem _sol;
		private TradingStation _tradingStation;

		[SetUp]
		public void SetUp()
		{
			_sc = new StarCluster { MarketPlace = new MarketPlace() };
			_sol = new SolarSystem { Position = new Position(_sc, Vector.Zero) };
			_tradingStation = new TradingStation { Position = new Position(_sol, Vector.Zero) };
		}

		[Test]
		public void GetMarketPlace()
		{
			var a = new Auction();
			Assert.That(a.MarketPlace, Is.Null);

			a.Location = _tradingStation;
			Assert.That(a.MarketPlace, Is.EqualTo(_sc.MarketPlace));
		}

		[Test]
		public void OwnerIsSet()
		{
			var item = new Item() {Owner = new Corporation(), Quantity = 10 };
			var a = new Auction { Item = item };

			Assert.That(a.Owner, Is.EqualTo(item.Owner));
		}

		[Test]
		public void AuctionQuantity()
		{
			var item = new Item() { Quantity = 10 };
			var a = new Auction { Item = item };

			Assert.That(a.Quantity, Is.EqualTo(item.Quantity));
		}

		[Test, ExpectedException(typeof(ArgumentNullException), UserMessage = "Value cannot be null")]
		public void ListAuctionWithNoMarketPlace()
		{
			var a = new Auction();
			a.List(null);
		}

		[Test]
		public void ListAuction()
		{
			_sc.MarketPlace.AuctionLength = 10;

			var a = new Auction();
			a.List(_sc.MarketPlace);

			Assert.That(_sc.MarketPlace.Auctions, Contains.Item(a));
		}
	}
}