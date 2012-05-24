using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Echo.Objects.Templates;

namespace Echo.Objects
{
	public partial class Planet
	{
		public class Template : LocationTemplate<Planet>
		{
			public Template(ObjectFactory universe, SolarSystem.Template solarSystemTemplate)
				: base(universe)
			{
				this.SolarSystem = solarSystemTemplate;
				Moons = new List<Moon.Template>();
			}

			public SolarSystem.Template SolarSystem { get; private set; }
			public double Size { get; set; }
			public List<Moon.Template> Moons { get; private set; }

			public override string Name
			{
				get
				{
					return base.Name ?? AutoName;
				}
			}

			private string AutoName
			{
				get
				{
					var pNum = RomanNumbers.ConvertToRomanNumerals(this.SolarSystem.Planets.IndexOf(this) + 1);
					return string.Format("{0} {1}", this.SolarSystem.Name, pNum);
				}
			}

			public double Extent
			{
				get
				{
					double extent = 0d;
					Moons.ForEach(m => extent = Math.Max(extent, m.Coordinates.Magnitude + m.Extent));

					return extent;
				}
			}

			public void Create(SolarSystem solarSystem)
			{
				var planet = new Planet(solarSystem) { Name = Name, Size = Size };
				Moons.ForEach(m => m.Create(planet));

				solarSystem.OrbitSun(planet, Coordinates);
			}

			protected override void ReadXml(XmlElement xTemplate)
			{
				base.ReadXml(xTemplate);
				Size = xTemplate.Double("size");

				Moons.Clear();
				xTemplate.SelectNodes("moons/moon").
					ForEach(LoadMoon);
			}

			private void LoadMoon(XmlElement xMoon)
			{
				var moon = new Moon.Template(Factory, this);
				moon.Load(xMoon);
				Moons.Add(moon);
			}

			protected override void WriteXml(XmlElement xTemplate)
			{
				base.WriteXml(xTemplate);
				xTemplate.Attribute("size", Size);

				XmlElement xMoons = xTemplate.Append("moons");
				Moons.ForEach(sc => sc.Save(xMoons));
			}

			protected override BaseTemplate<Planet> Clone(bool newID)
			{
				var clone = (Template)base.Clone(newID);
				clone.Moons = Moons.Clone();

				return clone;
			}
		}
	}
}