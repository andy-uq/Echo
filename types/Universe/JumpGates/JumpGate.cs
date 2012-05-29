using System;
using Echo.Celestial;
using Echo.Ships;
using EnsureThat;


namespace Echo.JumpGates
{
	public partial class JumpGate : OrbitingObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.JumpGate; }
		}

		public JumpGate ConnectsTo { get; set; }

		public Celestial.SolarSystem SolarSystem
		{
			get { return Position.GetSolarSystem(); }
		}

		/// <summary>Jumps a ship away from this gate</summary>
		/// <param name="ship">Ship to jump</param>
		public void Jump(Ship ship)
		{
			Ensure.That(() => ship).IsNotNull();

			if ( ConnectsTo == null )
				throw new InvalidOperationException("This gate is an incomming gate only");

			ship.SolarSystem.LeaveSystem(ship);
			ConnectsTo.SolarSystem.EnterSystem(ship, ConnectsTo.Position.LocalCoordinates);
		}
	}
}