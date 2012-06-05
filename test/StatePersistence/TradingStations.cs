using System;
using Echo.Builders;
using Echo.State;
using Echo.Structures;
using NUnit.Framework;

namespace Echo.Tests.StatePersistence
{
	[TestFixture]
	public class TradingStations : StateTest
	{
		private StructureState TradingStation
		{
			get { return Universe.TradingStation; }
		}

		[Test]
		public void Persist()
		{
			Database.UseOnceTo().Insert(TradingStation);
			DumpObjects("Structure");
		}

		[Test]
		public void Save()
		{
			var structure = Structure.Builder.For(TradingStation).Build(null, TradingStation).Resolve(null);
			var state = structure.Save();

			Assert.That(state.TradingStation, Is.Not.Null);

			var json = Database.Serializer.Serialize(state);
			Console.WriteLine(json);
		}

		[Test]
		public void Deserialise()
		{
			Database.UseOnceTo().Insert(TradingStation);
			var state = Database.UseOnceTo().GetById<StructureState>(TradingStation.Id);
			Assert.That(state, Is.Not.Null);
			Assert.That(state.TradingStation, Is.Not.Null);

			var structure = Structure.Builder.For(state).Build(null, state).Resolve(null);
			Assert.That(structure, Is.InstanceOf<TradingStation>());
		}
	}
}