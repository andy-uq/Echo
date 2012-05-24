using Echo.Vectors;

namespace Echo.Objects
{
	public partial class Moon : CelestialBody
	{
		public Moon(CelestialBody orbiting) : base(orbiting)
		{
		}

		protected override string SystematicNamePrefix
		{
			get { return Orbiting.Name; }
		}
	}
}