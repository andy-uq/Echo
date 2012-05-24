using System.Xml;

using Echo.Objects.Templates;

namespace Echo.Objects
{
	public partial class Moon
	{
		public class Template : LocationTemplate<Moon>
		{
			public Template(ObjectFactory universe, Planet.Template planetTemplate)
				: base(universe)
			{
				this.Planet = planetTemplate;
			}

			public double Size { get; set; }
			public Planet.Template Planet { get; private set; }

			public double Extent
			{
				get { return Size; }
			}

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
					var pNum = RomanNumbers.ConvertToRomanNumerals(this.Planet.Moons.IndexOf(this) + 1);
					return string.Format("{0} {1}", this.Planet.Name, pNum);
				}
			}

			public void Create(Planet planet)
			{
				var moon = new Moon(planet) { Name = Name };
				planet.AddSatellite(moon, Coordinates);
			}

			protected override void ReadXml(XmlElement xTemplate)
			{
				base.ReadXml(xTemplate);
				Size = xTemplate.Double("size");
			}

			protected override void WriteXml(XmlElement xTemplate)
			{
				base.WriteXml(xTemplate);
				xTemplate.Attribute("size", Size);
			}
		}
	}
}