using System.Collections.Generic;

using Echo.Entities;
using Echo.Objects;
using Echo.Ships;

using NUnit.Framework;

namespace Echo.Testing
{
	[TestFixture]
	public class Manfacturing
	{
		[Test]
		public void BuildShip()
		{
			var universe = new Universe();
			var corp = new Corporation { Location = universe };
			var shipYard = new MockShipYard(universe, corp);

			var shipBlueprint = new ShipBlueprint {HardPoints = new[] {HardPoint.FactoryHardPoint(HardPointPosition.Front),}, BuildCost = 10, Speed = 20d, Location = shipYard, Owner = corp };
			shipBlueprint.Materials = new List<Material> {new Material(RefinedOre.RefinedOreID, "Refined Ore", 500)};
			shipBlueprint.Stats[ShipStatistic.HullIntegrity].SetValue(1000);
			shipBlueprint.Stats[ShipStatistic.Speed].SetValue(20);

			shipYard.Build(shipBlueprint);

			ulong tick = 0L;
			while ( shipYard.Ships.Count == 0 )
			{
				shipYard.Tick(tick);
				tick++;
			}
		}

		public class MockShipYard : ShipYard
		{
			public MockShipYard(ILocation orbiting, Corporation owner) : base(orbiting, owner)
			{
				var refinedOre = new RefinedOre();
				refinedOre.Quantity = 1000;
				refinedOre.Owner = owner;

				AddItem(refinedOre);
			}
		}
	}
}