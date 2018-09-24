using System.Collections.Generic;
using System.Linq;
using Echo.Celestial;
using Echo.Exceptions;

namespace Echo
{
	public class JumpCountTable
	{
		private readonly Dictionary<SolarSystem, JumpEntry> _jumpCountTable;

		public IEnumerable<JumpEntry> Table => _jumpCountTable.Values;

		public JumpCountTable(IEnumerable<SolarSystem> solarSystems)
		{
			_jumpCountTable = solarSystems.ToDictionary(x => x, x => new JumpEntry(x));
			foreach (var jumpEntry in _jumpCountTable.Values)
			{
				var connections = jumpEntry.GetConnectedSolarSystems();
				jumpEntry.DirectConnections.AddRange(connections.Select(x => _jumpCountTable[x]));
			}
		}

		public int GetJumpCount(SolarSystem from, SolarSystem to)
		{
			if (_jumpCountTable.TryGetValue(from, out var entry))
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
				if (!JumpCountCache.TryGetValue(target, out var jumpCount))
				{
					jumpCount = GetJumpCount(target, new Stack<ulong>());
					JumpCountCache.Add(target, jumpCount);
				}

				return jumpCount;
			}

			private int GetJumpCount(SolarSystem target, Stack<ulong> seen)
			{
				if (seen.Contains(SolarSystem.Id))
				{
					return int.MaxValue - 1;
				}

				if (SolarSystem.Id == target.Id)
					return 0;

				seen.Push(SolarSystem.Id);
				
				var options = DirectConnections.Select(x => new {x.SolarSystem.Name, JumpCount = x.GetJumpCount(target, seen)}).ToArray();
				var leg = options.Min(x => x.JumpCount);

				seen.Pop();
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