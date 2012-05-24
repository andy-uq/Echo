using Echo.Bank;
using Echo.Entities;
using Echo.Objects;
using Echo.Ships;
using Echo.Vectors;

using NUnit.Framework;

namespace Echo.Testing
{
	[TestFixture]
	public class Trading
	{
		private class TestCase
		{
			private readonly IItem item;

			public TestCase()
			{			
				Universe = new Universe();

				Seller = new Corporation {Name = "Seller", Location = Universe };
				Buyer = new Corporation {Name = "Buyer", Location = Universe };

				var starCluster = new StarCluster();
				Universe.AddStarCluster(starCluster);
	
				var sol = new SolarSystem();
				starCluster.AddSolarSystem(sol);
		
				Refinery = new Refinery(sol, Seller);
				sol.OrbitSun(Refinery, 100d);

				var m1 = new Ship(Seller.Recruit()) {Name = "M1"};
				sol.EnterSystem(m1, Refinery.LocalCoordinates);
				Refinery.Dock(m1);

				var m2 = new Ship(Buyer.Recruit()) {Name = "M2"};
				sol.EnterSystem(m2, Refinery.LocalCoordinates);
				Refinery.Dock(m2);

				this.item = new Ore {Quantity = 1000, Location = Refinery, Owner = Seller};
			}

			public Refinery Refinery { get; private set; }
			public Corporation Seller { get; private set; }
			public Corporation Buyer { get; private set; }
			public Universe Universe { get; private set; }

			public Auction CreateAuction()
			{
				return Seller.CreateAuction(this.item, 10, 100);
			}
		}

		[Test]
		public void Buy()
		{
			var test = new TestCase();

			test.Buyer.Bank = 10000;

			Auction auction = test.CreateAuction();
			Assert.AreEqual(1000, auction.Quantity);

			uint auctionQuantity = auction.Quantity;
			IItem item;
			while (auctionQuantity > 100)
			{
				item = test.Buyer.Buy(auction, 100);
				auctionQuantity -= 100;

				Assert.AreEqual(auctionQuantity, auction.Quantity);
				Assert.AreEqual(100, item.Quantity);
				Assert.AreEqual(test.Buyer, item.Owner);
				Assert.AreEqual(2, test.Buyer.Assets.Count);
			}

			item = test.Buyer.Buy(auction, 100);
			Assert.AreEqual(test.Buyer, item.Owner);

			CollectionAssert.DoesNotContain(test.Seller.Assets, auction.Item);
			Assert.AreEqual(2, test.Buyer.Assets.Count);
			Assert.AreEqual(1, test.Seller.Assets.Count);
		}

		[Test]
		public void BuyPartial()
		{
			var test = new TestCase();

			test.Buyer.Bank = 10000;

			Auction auction = test.CreateAuction();
			Assert.AreEqual(1000, auction.Quantity);

			IItem item = test.Buyer.Buy(auction, 100);

			Assert.AreEqual(100, item.Quantity);
			Assert.AreEqual(test.Buyer, item.Owner);
			CollectionAssert.Contains(test.Buyer.Assets, item);
		}

		[Test]
		public void CreateAuction()
		{
			var test = new TestCase();
			Auction auction = test.CreateAuction();

			Assert.AreEqual(test.Seller, auction.Owner);
			CollectionAssert.DoesNotContain(test.Seller.Assets, auction.Item);
		}

		[Test]
		public void ExpireAuction()
		{
			var test = new TestCase();
			Auction auction = test.CreateAuction();

			Assert.AreEqual(test.Seller, auction.Owner);
			CollectionAssert.DoesNotContain(test.Seller.Assets, auction.Item);

			while (auction.Expires > 0)
				test.Universe.Tick();

			CollectionAssert.Contains(test.Seller.Assets, auction.Item);
		}
	}
}