using Echo.Vectors;

namespace Echo.Objects.Templates
{
	public abstract class LocationTemplate<T> : BaseTemplate<T> where T : IObject
	{
		protected LocationTemplate(ObjectFactory universe) : base(universe)
		{
		}

		public Vector Coordinates { get; set; }

		protected override void ReadXml(System.Xml.XmlElement xTemplate)
		{
			base.ReadXml(xTemplate);

			var xCoordinates = xTemplate.Select("coordinates");
			double x = xCoordinates.Double("x");
			double y = xCoordinates.Double("y");

			Coordinates = new Vector(x, y, 0d);
		}

		protected override void WriteXml(System.Xml.XmlElement xTemplate)
		{
			base.WriteXml(xTemplate);
			xTemplate.Append("coordinates").Attribute("x", Coordinates.X).Attribute("y", Coordinates.Y);
		}
	}
}