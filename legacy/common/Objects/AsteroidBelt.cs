using System;

using Echo.Events;
using Echo.Maths;
using Echo.Ships;
using Echo.Vectors;

namespace Echo.Objects
{
	public class AsteroidBelt : OrbitingObject
	{
		public uint AmountRemaining { get; private set; }
		public uint Richness { get; set; }

		public AsteroidBelt(SolarSystem solarSystem) : base(solarSystem)
		{
		}

		/// <summary>Mines this asteroid belt for ore</summary>
		/// <param name="ship">Ship to load ore onto</param>
		/// <returns>True if the ship has completed its mining operation (cargo hold is full)</returns>
		public bool Mine(Ship ship)
		{
			AssertShipInRange(ship, "mine this asteroid belt");
            
			var ore = new Ore();

			var oreQuantity = (uint) Math.Floor(ship.CargoHoldRemaining/ore.SizePerUnit);
            ore.Quantity = Min(Rand.Next(Richness / 2, Richness), oreQuantity, AmountRemaining);
			ore.Owner = ship.Owner;

			ship.AddCargo(ore);
			AmountRemaining -= ore.Quantity;

			Universe.EventPump.RaiseEvent(ship, EventType.ShipCargo, "Mined {0:n0} ore from {1}", ore.Quantity, Name);
			return (ship.CargoHoldRemaining < ore.SizePerUnit);
		}

		private static uint Min(params uint[] values)
		{
			uint min = uint.MaxValue;

			for (int i=0; i < values.Length; i++)
			{
				if ( values[i] < min )
					min = values[i];
			}

			return min;
		}

		public override void Tick(ulong tick)
		{
			base.Tick(tick);
			AmountRemaining = Richness * 100;
		}

		protected override string SystematicNamePrefix
		{
			get { return "AS"; }
		}
	}
}