using System;
using Echo.Builders;
using Echo.State;
using Echo.Structures;
using NUnit.Framework;
using Echo;

namespace Echo.Tests.StatePersistence
{
	[TestFixture]
	public class Manufactories : StateTest
	{
		private StructureState Manufactory
		{
			get { return Universe.Manufactory; }
		}

		[Test]
		public void Persist()
		{
			Database.UseOnceTo().Insert(Manufactory);
			DumpObjects("Structure");
		}

		[Test]
		public void Save()
		{
			var structure = Structure.Builder.For(Manufactory).Build(null, Manufactory).Materialise();
			var state = structure.Save();

			Assert.That(state.Manufactory, Is.Not.Null);

			var json = Database.Serializer.Serialize(state);
			Console.WriteLine(json);
		}

		[Test]
		public void Deserialise()
		{
			Database.UseOnceTo().Insert(Manufactory);
			var state = Database.UseOnceTo().GetById<StructureState>(Manufactory.Id);
			Assert.That(state, Is.Not.Null);
			Assert.That(state.Manufactory, Is.Not.Null);
			Assert.That(state.TradingStation, Is.Null);

			var structure = Structure.Builder.For(Manufactory).Build(null, Manufactory).Materialise();
			Assert.That(structure, Is.InstanceOf<Manufactory>());
		}
	}
}