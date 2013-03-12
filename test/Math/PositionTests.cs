using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Echo.Celestial;
using Echo.Corporations;
using Echo.Ships;
using Echo.Tests.Mocks;
using Moq;
using NUnit.Framework;
using Echo;

namespace Echo.Tests.Math
{
	[TestFixture]
	public class PositionTests
	{
		class P : ILocation
		{
			public ObjectType ObjectType
			{
				get { throw new System.NotImplementedException(); }
			}

			public long Id
			{
				get { throw new System.NotImplementedException(); }
			}

			public string Name
			{
				get { throw new System.NotImplementedException(); }
			}

			public Position Position { get; set; }
		}
		
		private Mock<IIdResolver> _idResolver = new Mock<IIdResolver>(MockBehavior.Strict);

		[Test]
		public void UniversalCoordinates()
		{
			Vector v0 = new Vector(1, 0, 0);
			Vector v2 = new Vector(0,1,0);

			Position p = new Position(new P() { Position = new Position(null, v0)}, v2);
			
			Vector expected = new Vector(1, 1, 0);
			Assert.That(p.UniversalCoordinates, Is.EqualTo(expected));
		}

		[Test]
		public void GetSolarSystem()
		{
			var s = new MockUniverse();
			Assert.That(s.Universe.StarClusters, Is.Not.Empty);

			var builder = Universe.Builder.Build(s.Universe);
			builder.Add(Corporation.Builder.Build(s.MSCorp));

			var u = builder.Materialise();

			Assert.That(u.StarClusters, Is.Not.Empty);
			Assert.That(u.SolarSystems(), Is.Not.Empty);

			var sol = u.SolarSystems().Single(x => x.Id == s.SolarSystem.ObjectId);
			var earth = u.Planets().Single(p => p.Id == s.Earth.ObjectId);

			Assert.That(earth.Position.GetSolarSystem(), Is.EqualTo(sol));

			var ship = new Ship {Position = new Position(earth, new Vector(1, 1, 0))};
			Assert.That(ship.Position.GetSolarSystem(), Is.EqualTo(sol));
		}
	}
}