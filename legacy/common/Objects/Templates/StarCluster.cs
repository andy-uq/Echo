using System;
using System.Collections.Generic;
using System.Xml;

using Echo.Objects.Templates;

namespace Echo.Objects
{
	public partial class StarCluster
	{
		#region Nested type: Template

		public class Template : LocationTemplate<StarCluster>
		{
			public Template(ObjectFactory universe) : base(universe)
			{
				SolarSystems = new List<SolarSystem.Template>();
			}

			public override string Name
			{
				get
				{
					return base.Name ?? string.Format("C{0:d4}", Factory.StarClusters.IndexOf(this));
				}
				set
				{
					base.Name = value;
				}
			}

			public List<SolarSystem.Template> SolarSystems { get; private set; }

			public double Extent
			{
				get
				{
					double extent = 0d;
					SolarSystems.ForEach(s => extent = Math.Max(extent, s.Coordinates.Magnitude + s.Extent));

					return extent;
				}
			}

			public StarCluster Create()
			{
				var starCluster = new StarCluster {TemplateID = TemplateID, UniversalCoordinates = Coordinates, Name = Name};
				SolarSystems.ForEach(s => s.Create(starCluster));

				Universe.AddStarCluster(starCluster);

				return starCluster;
			}

			protected override void ReadXml(XmlElement xTemplate)
			{
				base.ReadXml(xTemplate);

				SolarSystems.Clear();
				xTemplate.SelectNodes("solarsystems/solarsystem").
					ForEach(LoadSolarSystem);
			}

			private void LoadSolarSystem(XmlElement xSolarSystem)
			{
				var solarsystem = new SolarSystem.Template(Factory, this);
				solarsystem.Load(xSolarSystem);
				SolarSystems.Add(solarsystem);
			}

			protected override void WriteXml(XmlElement xTemplate)
			{
				base.WriteXml(xTemplate);

				XmlElement xSolarSystems = xTemplate.Append("solarsystems");
				SolarSystems.ForEach(sc => sc.Save(xSolarSystems));
			}

			public Template Clone()
			{
				return (Template)Clone(false);
			}

			protected override BaseTemplate<StarCluster> Clone(bool newID)
			{
				var clone = (Template) base.Clone(newID);
				clone.SolarSystems = SolarSystems.Clone();

				return clone;
			}
		}

		#endregion
	}
}