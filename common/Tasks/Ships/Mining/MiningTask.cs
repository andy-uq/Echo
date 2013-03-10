using System.Collections.Generic;
using System.Linq;
using Echo.Ships;
using Echo.Tasks.Ships;

namespace Echo.Tasks.Ships.Mining
{
	public class MiningTask : ShipTask<MineAsteroidParameters, MiningResult>
	{
		private readonly IItemFactory _itemFactory;

		public MiningTask(IItemFactory itemFactory)
		{
			_itemFactory = itemFactory;
		}

		public override MiningResult Execute(MineAsteroidParameters mineAsteroidParameters)
		{
			uint quantity = 0;

			var hardPoints = GetMiningHardPoints(mineAsteroidParameters.Ship.HardPoints).Where(hp => hp.AimAt(mineAsteroidParameters.AsteroidBelt));
			foreach (var hardPoint in hardPoints)
			{
				quantity += 1;
			}

			quantity = mineAsteroidParameters.AsteroidBelt.Reduce(quantity);
			var ore = _itemFactory.Build(mineAsteroidParameters.AsteroidBelt.Ore, quantity);

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
}