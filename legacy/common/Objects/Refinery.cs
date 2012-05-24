using System;

using Echo.Entities;
using Echo.Events;
using Echo.Ships;
using Echo.Vectors;

namespace Echo.Objects
{
	public class Refinery : Structure
	{
		public Refinery(ILocation orbiting, Corporation owner) : base(orbiting, owner)
		{
			OreProcessedPerTick = 100;
			Efficiency = 0.1d;
		}

		public uint UnrefinedOre { get; private set; }
		public uint OreProcessedPerTick { get; set; }
		public double Efficiency { get; set; }
		public uint RefinedOre { get; private set; }

		protected override string SystematicNamePrefix
		{
			get { return "REF"; }
		}

		/// <summary>Load refined ore onto a ship</summary>
		/// <param name="ship"></param>
		/// <returns>True if the ship has picked up all the ore it can</returns>
		public bool LoadRefinedOre(Ship ship)
		{
			AssertShipInRange(ship, "load refined ore from this refinery");

			if (RefinedOre != 0)
			{
				var refinedOre = new RefinedOre();

				uint maxOreCanCarry = (refinedOre.SizePerUnit == 0d) ? RefinedOre : Math.Min((uint) Math.Floor(ship.CargoHoldRemaining/refinedOre.SizePerUnit), RefinedOre);

				RefinedOre -= maxOreCanCarry;

				refinedOre.Quantity = maxOreCanCarry;
				refinedOre.Owner = Owner;

				ship.AddCargo(refinedOre);

				Universe.EventPump.RaiseEvent(ship, EventType.ShipCargo, "Loaded {0:n0} refined ore from {1}", refinedOre.Quantity, Name);

				return (refinedOre.SizePerUnit == 0d) || (ship.CargoHoldRemaining < refinedOre.SizePerUnit);
			}

			return false;
		}

		/// <summary>Unload raw ore from a ship</summary>
		/// <param name="ship"></param>
		/// <returns>True if ore was unloaded</returns>
		public bool UnloadOre(Ship ship)
		{
			AssertShipInRange(ship, "unload raw ore to this refinery");

			IItem ore = ship.RemoveCargo(Ore.OreID);
			if (ore != null)
			{
				UnrefinedOre += ore.Quantity;
				Universe.EventPump.RaiseEvent(ship, EventType.ShipCargo, "Unloaded {0:n0} raw ore to {1}", ore.Quantity, Name);

				return true;
			}

			return false;
		}

		public override void Tick(ulong tick)
		{
			base.Tick(tick);

			uint rawOre = Math.Min(OreProcessedPerTick, UnrefinedOre);
			UnrefinedOre -= rawOre;

			var oreRefined = (uint) Math.Floor(rawOre*Efficiency);
			RefinedOre += oreRefined;

			if (rawOre > 0)
				Universe.EventPump.RaiseEvent(this, EventType.Production, "Refined {0:n0} ore into {1:n0} quadrium", rawOre, oreRefined);
		}

		public override StructureType StructureType
		{
			get { return StructureType.Refinery; }
		}
	}
}