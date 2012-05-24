using System;

using Echo.Bank;

namespace Echo.Objects
{
	public partial class StarCluster : BaseLocation
	{
		private readonly ObjectCollection<SolarSystem> solarSystems;

		public ulong TemplateID { get; private set; }

		public StarCluster()
		{
			this.solarSystems = new ObjectCollection<SolarSystem>(this);
			MarketPlace = new MarketPlace(this);
		}

		public MarketPlace MarketPlace { get; private set; }

		public IReadOnlyList<SolarSystem> SolarSystems
		{
			get { return this.solarSystems; }
		}

		public void AddSolarSystem(SolarSystem solarSystem)
		{
			solarSystems.Add(solarSystem);
			Extent = Math.Max(Extent, solarSystem.LocalCoordinates.Magnitude + solarSystem.Extent);
		}

		protected override string SystematicNamePrefix
		{
			get { return "G"; }
		}

		public override ObjectType ObjectType
		{
			get { return ObjectType.StarCluster; }
		}

		public override void Tick(ulong tick)
		{
			MarketPlace.Tick(tick);
			solarSystems.ForEach(t => t.Tick(tick));
		}
	}
}