using System.Collections.Generic;

using Echo.Vectors;

namespace Echo.Objects
{
	public partial class Planet : CelestialBody
	{
		public Planet(SolarSystem orbiting) : base(orbiting)
		{
		}

		protected override string SystematicNamePrefix
		{
			get { return SolarSystem.Name; }
		}
	}
}