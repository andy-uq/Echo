using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Echo.Events;
using Echo.Ships;
using Echo.Vectors;

namespace Echo.Objects
{
	public class JumpGate : OrbitingObject
	{
		public JumpGate(ILocation orbiting) : base(orbiting)
		{
		}

		public JumpGate ConnectsTo { get; set; }

		/// <summary>Jumps a ship away from this gate</summary>
		/// <param name="ship">Ship to jump</param>
		public void Jump(Ship ship)
		{
			if (ship == null)
				throw new ArgumentNullException("ship");

			if (ConnectsTo == null)
				throw new InvalidOperationException("This gate is an incomming gate only");

			ship.SolarSystem.LeaveSystem(ship);
			ConnectsTo.SolarSystem.EnterSystem(ship, ConnectsTo.LocalCoordinates);
		
			Universe.EventPump.RaiseEvent(ship, EventType.ShipJump, "Jumped from {0} to {1} in {2}", Name, ConnectsTo.Name, ConnectsTo.SolarSystem.Name);
		}

		protected override string SystematicNamePrefix
		{
			get { return "JG"; }
		}
	}
}
