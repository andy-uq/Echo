using System.Collections.Generic;
using System.Linq;
using Echo.Celestial;
using Echo.Exceptions;

namespace Echo
{
	public class JumpCountTable
	{
		private readonly Dictionary<SolarSystem, JumpEntry> _jumpCountTable;

		public IEnumerable<JumpEntry> Table
		{
			get { return _jumpCountTable.Values; }
		}

		public JumpCountTable(IEnumerable<SolarSystem> solarSystems)
		{
			_jumpCountTable = solarSystems.ToDictionary(x => x, x => new JumpEntry(x));
			foreach (JumpEntry jumpEntry in _jumpCountTable.Values)
			{
				var connections = jumpEntry.GetConnectedSolarSystems();
				jumpEntry.DirectConnections.AddRange(connections.Select(x => _jumpCountTable[x]));
			}
		}

		public int GetJumpCount(SolarSystem from, SolarSystem to)
		{
			JumpEntry entry;
			if (_jumpCountTable.TryGetValue(from, out entry))
				return entry.GetJumpCount(to);

			throw new ItemNotFoundException("Solar System", from);
		}

		#region Nested type: JumpEntry

		public class JumpEntry
		{
			public JumpEntry(SolarSystem solarSystem)
			{
				SolarSystem = solarSystem;
				DirectConnections = new List<JumpEntry>();
				JumpCountCache = new Dictionary<SolarSystem, int>();
			}

			public SolarSystem SolarSystem { get; set; }
			private Dictionary<SolarSystem, int> JumpCountCache { get; set; }

			public List<JumpEntry> DirectConnections { get; set; }

			public int GetJumpCount(SolarSystem target)
			{
				int jumpCount;
				if (!JumpCountCache.TryGetValue(target, out jumpCount))
				{
					jumpCount = GetJumpCount(target, new Stack<long>());
					JumpCountCache.Add(target, jumpCount);
				}

				return jumpCount;
			}

			private int GetJumpCount(SolarSystem target, Stack<long> seen)
			{
				if (seen.Contains(SolarSystem.Id))
				{
					return int.MaxValue - 1;
				}

				if (SolarSystem.Id == target.Id)
					return 0;

				seen.Push(SolarSystem.Id);
				var leg = DirectConnections.Min(x => x.GetJumpCount(target, seen));
				return leg + 1;
			}

			public IEnumerable<SolarSystem> GetConnectedSolarSystems()
			{
				return SolarSystem.JumpGates.Where(j => j.ConnectsTo != null).Select(j => j.ConnectsTo.SolarSystem).Distinct();
			}
		}

		#endregion
	}
}