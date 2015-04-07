using System;
using Echo.Agents;
using Echo.Builders;
using Echo.Celestial;
using Echo.Corporations;
using Echo.Items;
using Echo.Market;
using Echo.State;
using Echo.Structures;
using Echo.Tests;
using NUnit.Framework;

namespace Echo.Data.Tests.StatePersistence
{
	public class Markets : StateTest
	{
		class WrappedObjectState
		{
			public string Id { get; set; }
			public State.MarketPlaceState Value { get; set; }

			public WrappedObjectState(MarketPlaceState value)
			{
				Value = value;
			}
		}

		private MarketPlaceState Market
		{
			get { return Universe.StarCluster.MarketPlace; }
		}

		[Test]
		public void Persist()
		{
			using (var session = Database.OpenSession())
			{
				session.Store(new WrappedObjectState(Market));
				session.SaveChanges();
			}

			DumpObjects("WrappedObject");
		}

		[Test]
		public void Save()
		{
			var buyOrder = new BuyOrder { Id = Universe.BuyOrder.ObjectId };
			var sellOrder = new SellOrder { Id = Universe.SellOrder.ObjectId };
			var resolver = new IdResolutionContext(new IObject[] { buyOrder, sellOrder, new ItemInfo(ItemCode.Veldnium), new TradingStation { Id = Universe.TradingStation.ObjectId }, new Corporation { Id = Universe.MSCorp.ObjectId }, new Corporation { Id = Universe.AppleCorp.ObjectId } });

			var market = MarketPlace.Builder.Build(Market, new StarCluster()).Materialise(resolver);
			Assert.That(market, Is.InstanceOf<MarketPlace>());

			var state = market.Save();

			var json = Database.Conventions.CreateSerializer().Serialize(state);
			Console.WriteLine(json);
		}

		[Test]
		public void Deserialise()
		{
			var wrapped = new WrappedObjectState(Market);
			using (var session = Database.OpenSession())
			{
				session.Store(wrapped, string.Concat(wrapped.Value.GetType().Name, "/market"));
				session.SaveChanges();
			}

			using (var session = Database.OpenSession())
			{
				var buyOrder = new BuyOrder {Id = Universe.BuyOrder.ObjectId};
				var sellOrder = new SellOrder { Id = Universe.SellOrder.ObjectId };
				var resolver = new IdResolutionContext(new IObject[] { buyOrder, sellOrder, new ItemInfo(ItemCode.Veldnium), new TradingStation { Id = Universe.TradingStation.ObjectId }, new Corporation { Id = Universe.MSCorp.ObjectId }, new Corporation { Id = Universe.AppleCorp.ObjectId } });
				
				var state = session.Load<WrappedObjectState>(wrapped.Id).Value;
				Assert.That(state, Is.Not.Null);
				Assert.That(state.Settlements, Is.Not.Empty);
				
				var market = MarketPlace.Builder.Build(Market, null).Materialise(resolver);
				Assert.That(market, Is.InstanceOf<MarketPlace>());

				Assert.That(market.BuyOrders, Is.Not.Empty);
				Assert.That(market.SellOrders, Is.Not.Empty);
				Assert.That(market.Settlements, Is.Not.Empty);
			}
		}
	}
}