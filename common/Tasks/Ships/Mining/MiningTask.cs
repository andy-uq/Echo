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
		private uint TimeRemaining { get; set; }

		public MiningTask(IItemFactory itemFactory)
		{
			_itemFactory = itemFactory;
		}

		protected override MiningResult SetParameters(MineAsteroidParameters parameters)
		{
			AsteroidBelt = parameters.AsteroidBelt;
			Ship = parameters.Ship;

			if ( AsteroidBelt != null )
			{
				TimeRemaining = AsteroidBelt.Difficulty;
			}

			return Success();
		}
		
		public override ITaskResult Execute()
		{
			return Mine();
		}

		private IEnumerable<HardPoint> GetMiningHardPoints(IEnumerable<HardPoint> hardPoints)
		{
			return hardPoints.Where(hp => hp.Weapon.WeaponInfo.IsMiningLaser());
		}

		public MiningResult Mine()
		{
			TimeRemaining--;

			if (TimeRemaining > 0)
			{
				Ship.RegisterTask(this);
				return new MiningResult {Success = true, StatusCode = StatusCode.Pending};
			}

			Ship.TaskComplete(this);

			double min = 0, max = 0;

			var hardPoints = GetMiningHardPoints(Ship.HardPoints)
				.Where(hp => hp.AimAt(AsteroidBelt))
				.Select(hp => hp.Weapon.WeaponInfo);

			foreach (var miningLaser in hardPoints)
			{
				min += miningLaser.MinimumDamage*miningLaser.Speed;
				max += miningLaser.MaximumDamage*miningLaser.Speed;
			}

			var quantity = (uint) System.Math.Floor((min + max)/2.0);
				
			quantity = AsteroidBelt.Reduce(quantity);
			Item ore = _itemFactory.Build(AsteroidBelt.Ore, quantity);

			return Success(() => new MiningResult {Ore = ore});
		}
	}
}