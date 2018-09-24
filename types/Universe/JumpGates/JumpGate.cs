using System;
using Echo.Celestial;
using Echo.Ships;

namespace Echo.JumpGates
{
	public partial class JumpGate : OrbitingObject
	{
		public const double MinimumRangeToJump = 0.5;

		public override ObjectType ObjectType => ObjectType.JumpGate;

		public JumpGate ConnectsTo { get; set; }

		public SolarSystem SolarSystem => Position.GetSolarSystem();

		/// <summary>Jumps a ship away from this gate</summary>
		/// <param name="ship">Ship to jump</param>
		public void Jump(Ship ship)
		{
			if (ship == null) throw new ArgumentNullException(nameof(ship));

			if ( ConnectsTo == null )
				throw new InvalidOperationException("This gate is an incoming gate only");

			ship.SolarSystem.LeaveSystem(ship);
			ConnectsTo.SolarSystem.EnterSystem(ship, ConnectsTo.Position.LocalCoordinates);
		}

		public override string ToString()
		{
			return Name;
		}

		public bool OutOfRange(Ship ship)
		{
			return (ship.Position.UniversalCoordinates - Position.UniversalCoordinates).Magnitude > MinimumRangeToJump;
		}
	}
}