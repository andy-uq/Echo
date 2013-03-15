using System.Collections.Generic;
using System.Linq;
using Echo.Celestial;
using Echo.Items;
using Echo.Ships;

namespace Echo.Tasks.Ships.Mining
{
	public class MiningTask : ShipTask<MineAsteroidParameters, MiningResult>
	{
		private readonly IItemFactory _itemFactory;
		private Ship Ship { get; set; }
		private AsteroidBelt AsteroidBelt { get; set; }

		public MiningTask(IItemFactory itemFactory)
		{
			_itemFactory = itemFactory;
		}

		protected override MiningResult SetParameters(MineAsteroidParameters parameters)
		{
			AsteroidBelt = parameters.AsteroidBelt;
			Ship = parameters.Ship;

			return Success();
		}
		
		public override ITaskResult Execute()
		{
			uint quantity = 0;

			IEnumerable<HardPoint> hardPoints =
				GetMiningHardPoints(Ship.HardPoints).Where(
					hp => hp.AimAt(AsteroidBelt));

			foreach (HardPoint hardPoint in hardPoints)
			{
				quantity += 1;
			}

			quantity = AsteroidBelt.Reduce(quantity);
			Item ore = _itemFactory.Build(AsteroidBelt.Ore, quantity);

			return Success(() => new MiningResult {Ore = ore});
		}

		private IEnumerable<HardPoint> GetMiningHardPoints(IEnumerable<HardPoint> hardPoints)
		{
			return hardPoints.Where(hp => hp.Weapon.WeaponInfo.IsMiningLaser());
		}
	}
}