using System;
using System.Collections.Generic;
using System.Linq;
using Echo.Corporations;
using Echo.Ships;
using Echo.Tests.Mocks;
using NUnit.Framework;
using Shouldly;

namespace Echo.Tests.Math
{
	[TestFixture]
	public class PositionTests
	{
		class P : ILocation
		{
			public ObjectType ObjectType => throw new NotImplementedException();

			public ulong Id => throw new NotImplementedException();

			public string Name => throw new NotImplementedException();

			public Position Position { get; set; }
		}
		
		[Test]
		public void UniversalCoordinates()
		{
			var v0 = new Vector(1, 0);
			var v1 = new Vector(0,1);

			var p = new Position(new P { Position = new Position(null, v0)}, v1);
			
			var expected = new Vector(1, 1);
			p.UniversalCoordinates.ShouldBe(expected);
		}

		[Test]
		public void GetSolarSystem()
		{
			var s = new MockUniverse();
			Assert.That(s.Universe.StarClusters, Is.Not.Empty);

			var builder = Universe.Builder.Build(s.Universe);
			builder.Add(Corporation.Builder.Build(s.MSCorp));

			var u = builder.Materialise();

			u.StarClusters.ShouldNotBeEmpty();
			u.SolarSystems().ShouldNotBeEmpty();

			var sol = u.SolarSystems().Single(x => x.Id == s.SolarSystem.ObjectId);
			var earth = u.Planets().Single(p => p.Id == s.Earth.ObjectId);

			earth.Position.GetSolarSystem().ShouldBe(sol);

			var ship = new Ship {Position = new Position(earth, new Vector(1, 1))};
			ship.Position.GetSolarSystem().ShouldBe(sol);
		}

		[Test]
		public void MovePositionForward()
		{
			var origin = new Vector(0, 0);
			var location = new P { Position = new Position(null, origin) };

			var p = new Position(location, origin);
			var result = p + new Vector(0, 1);

			result.ShouldBe(new Position(location, new Vector(0, 1)));
		}

		[Test]
		public void MovePositionBackward()
		{
			var origin = new Vector(0, 0);
			var location = new P { Position = new Position(null, origin) };

			var p = new Position(location, origin);
			var result = p - new Vector(0, 1);

			result.ShouldBe(new Position(location, new Vector(0, -1)));
		}

		[Test]
		public void PositionEquality()
		{
			var origin = new Vector(0, 0);
			var l1 = new P { Position = new Position(null, origin) };
			var p1 = new Position(l1, origin);
			
			var l2 = new P { Position = new Position(null, new Vector(1, 0)) };
			var p2 = new Position(l2, origin);

			(p1 == p2).ShouldBe(false);
			(p1 != p2).ShouldBe(true);

			((p2 + new Vector(-1, 0)) == p1).ShouldBe(true);

			Equals(p1, p2).ShouldBe(false);
		}

		[Test]
		public void AddToSet()
		{
			var origin = new Vector(0, 0);
			var l1 = new P { Position = new Position(null, origin) };

			var p = new Position(l1, origin);

			var set = new HashSet<Position>();
			set.Add(p).ShouldBe(true);
			set.Add(p).ShouldBe(false);
			
			var l2 = new P { Position = new Position(null, new Vector(1, 0)) };
			set.Add(new Position(l2, origin)).ShouldBe(true);
			set.Add(new Position(l2, new Vector(-1, 0))).ShouldBe(false);
		}
	}
}