using System.Collections.Generic;
using System.Linq;
using Echo.Agents;
using Echo.Celestial;
using Echo.State;
using Echo.Tasks;

namespace Echo.Ships
{
	public partial class Ship : ILocation, IMoves
	{
		private readonly List<HardPoint> _hardPoints;

		public ObjectType ObjectType => ObjectType.Ship;

		public ulong Id { get; set; }
		public string Name { get; set; }
		public Position Position { get; set; }
		public Vector Heading { get; set; }
		public ShipStatistics Statistics { get; set; }
		public ShipInfo ShipInfo { get; set; }
		public HashSet<ITask> Tasks { get; }

		public IEnumerable<HardPoint> HardPoints => _hardPoints;

		public SolarSystem SolarSystem => Position.GetSolarSystem();

		public Agent Pilot { get; set; }

		public Ship()
		{
			_hardPoints = new List<HardPoint>();
			Tasks = new HashSet<ITask>();
		}

		/// <summary>
		/// Returns true if a hard point can aim at a particular location
		/// </summary>
		public bool CanTrack(ILocation target)
		{
			return _hardPoints.Any(x => x.CanTrack(target));
		}

		/// <summary>
		/// Returns true if a hard point can move in time to aim at a target
		/// </summary>
		public bool CanAimAt(ILocation target)
		{
			return _hardPoints.Any(x => x.InRange(target));
		}

		public void RegisterTask(ITask task)
		{
			Tasks.Add(task);
		}

		public void TaskComplete(ITask task)
		{
			Tasks.Remove(task);
		}
	}
}