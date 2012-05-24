using System.Xml;

using Echo.Objects;

namespace Echo.Entities
{
	public partial class Corporation
	{
		#region Nested type: CorporationState

		internal class CorporationState : CorporationState<Corporation>
		{
			public CorporationState(Universe.UniverseState universeState) : base(universeState)
			{
			}
		}

		internal class CorporationState<T> : BaseObjectState<T> where T : Corporation, new()
		{
			public CorporationState(Universe.UniverseState universeState) : base(universeState)
			{
			}

			protected override T ReadXml(XmlElement xObject)
			{
				T corporation = base.ReadXml(xObject);

				var agentState = new Agent.AgentState<T>(Universe);
				agentState.LoadAll(corporation.employees, xObject.Select("agents"));

				return corporation;
			}

			protected override void WriteXml(T obj, XmlElement xObject)
			{
				base.WriteXml(obj, xObject);

				var agentState = new Agent.AgentState<T>(Universe);
				agentState.Save(obj.employees, xObject.Append("agents"));
			}
		}

		#endregion
	}
}