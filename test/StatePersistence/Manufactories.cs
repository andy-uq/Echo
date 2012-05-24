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
		public void Deserialise()
		{
			Database.UseOnceTo().Insert(Manufactory);
			var state = Database.UseOnceTo().GetById<StructureState>(1L);
			Assert.That(state, Is.Not.Null);

			var structure = Structure.Builder.For(state).Build(new Universe(), state);
			Assert.That(structure, Is.InstanceOf<Manufactory>());
		}
	}
}