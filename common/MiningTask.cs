using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Echo.Celestial;
using Echo.Items;
using Echo.Ships;
using Echo.State;

namespace Echo
{
	public class MiningTask : ShipTask<MiningResult>
	{
		public MiningResult Mine(Ship ship, AsteroidBelt asteroidBelt)
		{
			int quantity = 0;

			var hardPoints = GetMiningHardPoints(ship.HardPoints);
			foreach (var hardPoint in hardPoints)
			{
			}
			
			return Success();
		}

		private IEnumerable<HardPoint> GetMiningHardPoints(IEnumerable<HardPoint> hardPoints)
		{
			return Enumerable.Empty<HardPoint>();
		}
	}

	public class MiningResult : TaskResult, ITaskResult
	{
		string ITaskResult.ErrorCode
		{
			get { return ErrorCode.ToString(); }
		}

		object ITaskResult.ErrorParams
		{
			get { return Ship; }
		}

		public MiningTask.ErrorCode ErrorCode { get; set; }
		public Ship Ship { get; set; }

		public Item Ore { get; set; }
	}
}