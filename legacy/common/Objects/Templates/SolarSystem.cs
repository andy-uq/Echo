using System;
using System.Collections.Generic;
using System.Xml;

using Echo.Objects.Templates;

namespace Echo.Objects
{
	public partial class SolarSystem
	{
		public class Template : LocationTemplate<SolarSystem>
		{
			public Template(ObjectFactory universe, StarCluster.Template starClusterTemplate) : base(universe)
			{
				StarCluster = starClusterTemplate;
				Planets = new List<Planet.Template>();
			}

            public List<Planet.Template> Planets { get; private set; }
			public StarCluster.Template StarCluster { get; private set; }

			public override string Name
			{
				get
				{
					return base.Name ?? string.Format("S{0:d4}-{1:d4}", Factory.StarClusters.IndexOf(this.StarCluster), this.StarCluster.SolarSystems.IndexOf(this));
				}
			}

			public double Extent
			{
				get
				{
					double extent = 0d;
					Planets.ForEach(p => extent = Math.Max(extent, p.Coordinates.Magnitude + p.Extent));

					return extent;
				}
			}

			public void Create(StarCluster starCluster)
			{
				var solarSystem = new SolarSystem { Name = Name, Location = starCluster, LocalCoordinates = Coordinates };
				Planets.ForEach(p => p.Create(solarSystem));
				
				starCluster.AddSolarSystem(solarSystem);
			}

			protected override void ReadXml(XmlElement xTemplate)
			{
				base.ReadXml(xTemplate);

				Planets.Clear();
				xTemplate.SelectNodes("planets/planet").
					ForEach(LoadPlanet);
			}

			private void LoadPlanet(XmlElement xPlanet)
			{
				var planet = new Planet.Template(Factory, this);
				planet.Load(xPlanet);
				Planets.Add(planet);
			}

			protected override void WriteXml(XmlElement xTemplate)
			{
				base.WriteXml(xTemplate);

				XmlElement xSolarSystems = xTemplate.Append("planets");
				Planets.ForEach(sc => sc.Save(xSolarSystems));
			}

			protected override BaseTemplate<SolarSystem> Clone(bool newID)
			{
				var clone = (Template)base.Clone(newID);
				clone.Planets = Planets.Clone();

				return clone;
			}
		}
	}
}