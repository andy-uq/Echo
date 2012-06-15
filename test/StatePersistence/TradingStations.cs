using System;
using Echo.Builders;
using Echo.Celestial;
using Echo.State;
using Echo.Structures;
using NUnit.Framework;

namespace Echo.Tests.StatePersistence
{
	[TestFixture]
	public class TradingStations : StateTest
	{
		public class WrappedObjectState
		{
			public Guid Id { get; set; }
			public StructureState Value { get; set; }

			public WrappedObjectState(StructureState value)
			{
				Value = value;
			}
		}	
	
		private StructureState TradingStation
		{
			get { return Universe.TradingStation; }
		}

		[Test]
		public void Persist()
		{
			Database.UseOnceTo().Insert(new WrappedObjectState(TradingStation));
			DumpObjects("WrappedObject");
		}

		[Test]
		public void Save()
		{
			var structure = Structure.Builder.For(TradingStation).Build(new Moon { Id = Universe.Moon.ObjectId }, TradingStation).Materialise();
			var state = structure.Save();

			Assert.That(state.TradingStation, Is.Not.Null);

			var json = Database.Serializer.Serialize(state);
			Console.WriteLine(json);
		}

		[Test]
		public void Deserialise()
		{
			var wrapper = new WrappedObjectState(TradingStation);

			Database.UseOnceTo().Insert(wrapper);
			var state = Database.UseOnceTo().GetById<WrappedObjectState>(wrapper.Id).Value;
			Assert.That(state, Is.Not.Null);
			Assert.That(state.TradingStation, Is.Not.Null);

			var structure = Structure.Builder.For(state).Build(null, state).Materialise();
			Assert.That(structure, Is.InstanceOf<TradingStation>());
		}
	}
}