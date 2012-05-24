using System.Xml;

using Echo.Objects;
using Echo.Objects.Templates;

namespace Echo.Entities
{
	public partial class Corporation
	{
		#region Nested type: CorporationTemplate

		public class Template : CorporationTemplate<Corporation>
		{
			public Template(ObjectFactory universe) : base(universe)
			{
			}
		}

		public class CorporationTemplate<T> : BaseTemplate<T> where T : Corporation, new()
		{
			public CorporationTemplate(ObjectFactory universe) : base(universe)
			{
				StartupEmployeeCount = 5;
				StartupCapital = (ulong) 10E3;
			}

			public uint StartupEmployeeCount { get; set; }
			public ulong StartupCapital { get; set; }

			protected virtual T Create()
			{
				var corporation = new T {Location = Universe, Bank = StartupCapital};

				for (int i = 0; i < StartupEmployeeCount; i++)
					corporation.Recruit();

				return corporation;
			}

			public virtual T Create(string name)
			{
				T corporation = Create();
				corporation.Name = name;

				return corporation;
			}

			protected override void ReadXml(XmlElement xCorporation)
			{
				base.ReadXml(xCorporation);

				StartupCapital = xCorporation.UInt64("startupCapital");
				StartupEmployeeCount = xCorporation.UInt32("startupEmployeeCount");
			}

			protected override void WriteXml(XmlElement xAgent)
			{
				base.WriteXml(xAgent);

				xAgent.Attribute("startupEmployeeCount", StartupEmployeeCount);
				xAgent.Attribute("startupCapital", StartupCapital);
			}
		}

		#endregion
	}
}