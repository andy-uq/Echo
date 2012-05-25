using System.Collections.Generic;
using System.Linq;
using Echo.Ships;
using Echo.State;

namespace Echo
{
	namespace Builders
	{
		public static class BuilderExtensions
		{
			public static Weapon Build(this WeaponState state, ILocation location)
			{
				if ( state == null )
					return null;

				return new Weapon.Builder().Build(location, state);
			}	
			
			public static Ship Build(this ShipState state, ILocation location)
			{
				return new Ship.Builder().Build(location, state);
			}

			public static IEnumerable<HardPoint> Build(this IEnumerable<HardPointState> states, Ship ship)
			{
				if (states == null)
					return Enumerable.Empty<HardPoint>();

				var builder = new HardPoint.Builder();
				return states.Select(x => builder.Build(ship, x));
			}
		}
	}
}