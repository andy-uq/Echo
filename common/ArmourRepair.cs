using System.Linq;
using Echo.Ships;
using Echo.State;

namespace Echo
{
	public class ArmourRepair
	{
		public void Repair(Ship ship, ShieldInfo shieldInfo)
		{
			var statistic = ship.Statistics[shieldInfo.Statistic];

			var damage = statistic.Debuffs.OfType<Damage>().ToArray();
			var delta = shieldInfo.RepairPerTick;

			foreach (var d in damage)
			{
				if (d.Value < delta)
				{
					statistic.Remove(d);
					delta -= d.Value;
					continue;
				}

				d.Value -= delta;
				break;
			}

			statistic.Recalculate();
		}
	}
}