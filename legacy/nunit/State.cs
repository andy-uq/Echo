using System.Xml;

using Echo.Entities;
using Echo.Objects;

using NUnit.Framework;

namespace Echo.Testing
{
	[TestFixture]
	public class StateTests
	{
		[Test]
		public void SavePlayerState()
		{
			var universe = new Universe();
            PlayerCorporation corporation = universe.RegisterPlayerCorporation("andy", "AAAA", "andy@ubiquity.co.nz");

			var state = universe.SaveState();

			var reloaded = new Universe();
			reloaded.LoadState(state);

			Assert.AreEqual(universe.Players.Count, reloaded.Players.Count);
			Assert.AreEqual(corporation.ID, reloaded.Players[0].ID);
		}

		[Test]
		public void SaveAgent()
		{
			const string stateXml = @"<universe objectID=""0"" nextObjectID=""2"" currentTick=""0"">
  <corporations>
    <corporation objectID=""1"">
		<agents />
	</corporation>
  </corporations>
</universe>";
            
			var xdoc = new XmlDocument();
			xdoc.LoadXml(stateXml);

			var universe = new Universe();
			universe.LoadState(xdoc.Select("universe"));

			Assert.AreEqual(1, universe.Corporations.Count);

			var corporation = universe.Corporations[0];
			var agent = corporation.Recruit();
			agent.Stats[AgentStatistic.Memory].SetValue(100);
            
			var state = universe.SaveState();

			var reloaded = new Universe();
			reloaded.LoadState(state);
			Assert.AreEqual(1, reloaded.Corporations.Count);
            
			var reloadedCorporation = reloaded.Corporations[0];            
			Assert.AreEqual(corporation.Employees.Count, reloadedCorporation.Employees.Count);

			var reloadedAgent = reloadedCorporation.Employees[0];
			Assert.AreEqual(agent.ObjectID, reloadedAgent.ObjectID);

			Assert.AreEqual(agent.Stats[AgentStatistic.Memory].Value, reloadedAgent.Stats[AgentStatistic.Memory].Value);
		}
	}
}