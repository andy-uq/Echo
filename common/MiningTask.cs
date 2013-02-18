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
		private readonly IItemFactory _itemFactory;

		public MiningTask(IItemFactory itemFactory)
		{
			_itemFactory = itemFactory;
		}

		public MiningResult Mine(Ship ship, AsteroidBelt asteroidBelt)
		{
			uint quantity = 0;

			var hardPoints = GetMiningHardPoints(ship.HardPoints).Where(hp => hp.AimAt(asteroidBelt));
			foreach (var hardPoint in hardPoints)
			{
				quantity += 1;
			}

			quantity = asteroidBelt.Reduce(quantity);
			var ore = _itemFactory.Build(asteroidBelt.Ore, quantity);

			return Success(() => new MiningResult { Ore = ore });
		}

		private IEnumerable<HardPoint> GetMiningHardPoints(IEnumerable<HardPoint> hardPoints)
		{
			foreach (var hp in hardPoints)
			{
				if ( hp.Weapon.WeaponInfo.IsMiningLaser() )
					yield return hp;
			}
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