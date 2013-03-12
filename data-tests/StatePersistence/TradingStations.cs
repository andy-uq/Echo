﻿using System;
using System.IO;
using Echo.Builders;
using Echo.Celestial;
using Echo.State;
using Echo.Structures;
using Echo.Tests;
using Echo.Tests.StatePersistence;
using NUnit.Framework;

namespace Echo.Data.Tests.StatePersistence
{
	[TestFixture]
	public class TradingStations : StateTest
	{
		public class WrappedObjectState
		{
			public string Id { get; set; }
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
			using ( var session = Database.OpenSession() )
			{
				session.Store(new WrappedObjectState(TradingStation));
				session.SaveChanges();
			}

			DumpObjects("WrappedObject");
		}

		[Test]
		public void Save()
		{
			var structure = Structure.Builder.For(TradingStation).Build(new Moon { Id = Universe.Moon.ObjectId }).Materialise();
			var state = structure.Save();

			Assert.That(state.TradingStation, Is.Not.Null);

			var writer = new StringWriter();
			var serialiser = Database.Conventions.CreateSerializer();

			serialiser.Serialize(writer, state);
			Console.WriteLine(writer.ToString());
		}

		[Test]
		public void Deserialise()
		{
			var wrapped = new WrappedObjectState(TradingStation);

			using ( var session = Database.OpenSession() )
			{
				session.Store(wrapped, string.Concat(wrapped.Value.GetType().Name, "/", wrapped.Value.ObjectId));
				session.SaveChanges();

				Assert.That(wrapped.Id, Is.Not.EqualTo(Guid.Empty));
			}

			using ( var session = Database.OpenSession() )
			{
				var tmp = session.Load<WrappedObjectState>(wrapped.Id);
				Assert.That(tmp, Is.Not.Null);

				var state = tmp.Value;
				Assert.That(state.TradingStation, Is.Not.Null);

				var structure = Structure.Builder.For(state).Build(null).Materialise();
				Assert.That(structure, Is.InstanceOf<TradingStation>());
			}
		}
	}
}